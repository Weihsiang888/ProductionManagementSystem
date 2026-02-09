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
        <div class="language-switcher" style="position: relative; display: inline-block;">
            <button id="lang-switcher-btn" class="lang-switcher-btn" style="
                background: transparent;
                border: 2px solid dodgerblue;
                color: dodgerblue;
                padding: 8px 16px;
                border-radius: 4px;
                cursor: pointer;
                font-weight: bold;
                font-size: 0.9em;
                display: flex;
                align-items: center;
                gap: 8px;
            ">
                <span>🌐</span>
                <span>${currentLangObj ? currentLangObj.nativeName : currentLang}</span>
                <span style="font-size: 0.7em;">▼</span>
            </button>
            <div id="lang-dropdown" class="lang-dropdown" style="
                display: none;
                position: absolute;
                top: 100%;
                right: 0;
                background: white;
                border: 1px solid #ccc;
                border-radius: 4px;
                box-shadow: 0 2px 8px rgba(0,0,0,0.15);
                margin-top: 4px;
                min-width: 150px;
                z-index: 1000;
            ">
                ${languages.map(lang => `
                    <div class="lang-option" data-lang="${lang.code}" style="
                        padding: 10px 16px;
                        cursor: pointer;
                        border-bottom: 1px solid #f0f0f0;
                        ${currentLang === lang.code ? 'background: #f0f8ff; font-weight: bold;' : ''}
                    ">
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
        dropdown.style.display = dropdown.style.display === 'none' ? 'block' : 'none';
    });
    
    // Close dropdown when clicking outside
    document.addEventListener('click', () => {
        dropdown.style.display = 'none';
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
        
        // Hover effect
        option.addEventListener('mouseenter', (e) => {
            if (e.target.getAttribute('data-lang') !== currentLang) {
                e.target.style.background = '#f5f5f5';
            }
        });
        
        option.addEventListener('mouseleave', (e) => {
            if (e.target.getAttribute('data-lang') !== currentLang) {
                e.target.style.background = 'white';
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
