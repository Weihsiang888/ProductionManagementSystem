/**
 * Language Switcher Component Helper
 * Provides UI functionality for language switching
 */

/**
 * Render language switcher dropdown
 * @param {string} containerId - ID of container element to render into
 */
function renderLanguageSwitcher(containerId) {
    const container = document.getElementById(containerId);
    if (!container) {
        console.error('Language switcher container not found:', containerId);
        return;
    }
    
    const currentLang = window.i18n.getCurrentLanguage();
    const languages = window.i18n.languages;
    const currentLangObj = languages.find(lang => lang.code === currentLang);
    
    const html = `
        <div class="language-switcher">
            <button id="lang-switcher-btn" class="lang-switcher-btn">
                <span>🌐</span>
                <span>${currentLangObj ? currentLangObj.nativeName : currentLang}</span>
                <span class="lang-switcher-arrow">▼</span>
            </button>
            <div id="lang-dropdown" class="lang-dropdown">
                ${languages.map(lang => `
                    <div class="lang-option ${currentLang === lang.code ? 'active' : ''}" data-lang="${lang.code}">
                        ${lang.nativeName} ${currentLang === lang.code ? '✓' : ''}
                    </div>
                `).join('')}
            </div>
        </div>
    `;
    
    container.innerHTML = html;
    
    // Add event listeners
    const btn = document.getElementById('lang-switcher-btn');
    const dropdown = document.getElementById('lang-dropdown');
    
    btn.addEventListener('click', (e) => {
        e.stopPropagation();
        dropdown.classList.toggle('visible');
    });
    
    // Close dropdown when clicking outside
    document.addEventListener('click', () => {
        dropdown.classList.remove('visible');
    });
    
    // Language option click handlers
    const options = container.querySelectorAll('.lang-option');
    options.forEach(option => {
        option.addEventListener('click', () => {
            const lang = option.getAttribute('data-lang');
            if (lang && lang !== currentLang) {
                window.i18n.setLanguage(lang);
            }
        });
    });
}

// Auto-render when i18n is ready
window.addEventListener('i18nReady', () => {
    // Try to render language switcher if container exists
    const container = document.getElementById('language-switcher-container');
    if (container) {
        renderLanguageSwitcher('language-switcher-container');
    }
});

// Export to window
window.languageSwitcher = {
    render: renderLanguageSwitcher
};
