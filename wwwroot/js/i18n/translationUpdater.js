/**
 * DOM Translation Updater
 * Updates text content in the DOM with translations
 */

/**
 * Update text content of an element using translation key
 * @param {string} elementId - ID of element to update
 * @param {string} translationKey - Translation key to use
 */
function updateElementText(elementId, translationKey) {
    const element = document.getElementById(elementId);
    if (element && window.i18n) {
        element.textContent = window.i18n.t(translationKey);
    }
}

/**
 * Update all elements with data-i18n attribute
 */
function updateAllTranslations() {
    if (!window.i18n) {
        console.warn('i18n not initialized yet');
        return;
    }
    
    // Find all elements with data-i18n attribute
    const elements = document.querySelectorAll('[data-i18n]');
    elements.forEach(element => {
        const key = element.getAttribute('data-i18n');
        if (key) {
            const translatedText = window.i18n.t(key);
            
            // Update text content or specific attribute
            const attr = element.getAttribute('data-i18n-attr');
            if (attr) {
                element.setAttribute(attr, translatedText);
            } else {
                // Preserve leading spacing characters (emsp, nbsp, etc)
                const currentHTML = element.innerHTML;
                const leadingSpaces = currentHTML.match(/^(&emsp;|&nbsp;|\s)*/);
                const trailingTags = currentHTML.match(/<br\s*\/?>$/i);
                
                let newHTML = translatedText;
                if (leadingSpaces) {
                    newHTML = leadingSpaces[0] + newHTML;
                }
                if (trailingTags) {
                    newHTML = newHTML + trailingTags[0];
                }
                
                element.innerHTML = newHTML;
            }
        }
    });
}

/**
 * Update Blazor menu items with translations
 * This is a helper function specific to the DevExpress Blazor menu
 * Uses Promise-based approach to wait for menu to be rendered
 */
function updateBlazorMenuTranslations() {
    if (!window.i18n) return;
    
    // Function to attempt menu translation
    const attemptMenuTranslation = () => {
        // Update title
        const titleElement = document.querySelector('.icon-logo.font-style');
        if (titleElement) {
            const titleText = window.i18n.t('app.title');
            // Use CSS class instead of multiple &emsp; for spacing
            titleElement.textContent = titleText;
            titleElement.style.paddingLeft = '12em'; // Equivalent to 12 &emsp;
        }
        
        // Update menu items by their text content
        const menuItems = document.querySelectorAll('.dxbl-menu-item .font-style, .dxbl-menu-item-content .font-style');
        if (menuItems.length > 0) {
            menuItems.forEach(item => {
                const currentText = item.textContent.trim();
                
                // Map Chinese text to translation keys
                const textKeyMap = {
                    '首頁': 'nav.home',
                    'ESOP': 'nav.esop'
                };
                
                if (textKeyMap[currentText]) {
                    item.textContent = window.i18n.t(textKeyMap[currentText]);
                }
            });
            
            // Update login/logout text
            updateLoginLogoutText();
            return true;
        }
        return false;
    };
    
    // Try immediately
    if (attemptMenuTranslation()) {
        return;
    }
    
    // If not available, use MutationObserver to wait for menu
    const observer = new MutationObserver((mutations, obs) => {
        if (attemptMenuTranslation()) {
            obs.disconnect();
        }
    });
    
    observer.observe(document.body, {
        childList: true,
        subtree: true
    });
    
    // Timeout to stop observing after 5 seconds
    setTimeout(() => observer.disconnect(), 5000);
}

/**
 * Update login/logout button text
 */
function updateLoginLogoutText() {
    const loginButtons = document.querySelectorAll('.tab-title-bold');
    loginButtons.forEach(button => {
        const text = button.textContent.trim();
        if (text.includes('Login : Guest') || text === 'Login : Guest') {
            button.textContent = window.i18n.t('auth.loginAs');
        }
        // Keep logout with user name as-is since it's dynamic
    });
}

/**
 * Initialize translation updates
 */
function initTranslationUpdates() {
    if (!window.i18n) {
        console.warn('i18n not ready for translation updates');
        return;
    }
    
    // Initial update
    updateAllTranslations();
    updateBlazorMenuTranslations();
    
    // Set up MutationObserver to handle dynamic content
    const observer = new MutationObserver((mutations) => {
        let shouldUpdate = false;
        mutations.forEach(mutation => {
            if (mutation.addedNodes.length > 0) {
                shouldUpdate = true;
            }
        });
        
        if (shouldUpdate) {
            updateAllTranslations();
        }
    });
    
    // Observe the body for changes
    observer.observe(document.body, {
        childList: true,
        subtree: true
    });
}

// Initialize when i18n is ready
window.addEventListener('i18nReady', () => {
    initTranslationUpdates();
});

// Handle language changes
window.addEventListener('languageChanged', () => {
    initTranslationUpdates();
});

// Initialize when Blazor is ready (if available)
if (window.Blazor) {
    // Blazor provides a promise when it starts
    window.Blazor.start().then(() => {
        // Use MutationObserver to wait for initial render
        const observer = new MutationObserver(() => {
            if (document.querySelector('.content')) {
                initTranslationUpdates();
                observer.disconnect();
            }
        });
        
        observer.observe(document.body, {
            childList: true,
            subtree: true
        });
        
        // Timeout to stop observing after 5 seconds
        setTimeout(() => observer.disconnect(), 5000);
    }).catch(() => {
        // Fallback if Blazor is not available
        initTranslationUpdates();
    });
} else {
    // Fallback for non-Blazor pages
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initTranslationUpdates);
    } else {
        initTranslationUpdates();
    }
}

// Export functions
window.translationUpdater = {
    updateElement: updateElementText,
    updateAll: updateAllTranslations,
    updateMenu: updateBlazorMenuTranslations,
    init: initTranslationUpdates
};
