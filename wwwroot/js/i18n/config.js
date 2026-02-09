/**
 * i18n Configuration and Translation System
 * Provides comprehensive internationalization support for the Production Management System
 */

// Available languages configuration
const languages = [
    { code: 'zh-TW', name: '繁體中文', nativeName: '繁體中文' },
    { code: 'en', name: 'English', nativeName: 'English' }
];

// Translation cache
let translations = {};
let currentLanguage = null;

/**
 * Detect browser language preference
 * @returns {string} Language code (zh-TW or en)
 */
function detectBrowserLanguage() {
    const browserLang = navigator.language || navigator.userLanguage;
    
    // Check if browser language matches any of our supported languages
    if (browserLang.startsWith('zh')) {
        return 'zh-TW';
    } else if (browserLang.startsWith('en')) {
        return 'en';
    }
    
    // Default to Traditional Chinese
    return 'zh-TW';
}

/**
 * Get the current language from localStorage or detect from browser
 * @returns {string} Current language code
 */
function getCurrentLanguage() {
    if (currentLanguage) {
        return currentLanguage;
    }
    
    // Try to get from localStorage
    const storedLang = localStorage.getItem('preferredLanguage');
    if (storedLang && languages.some(lang => lang.code === storedLang)) {
        currentLanguage = storedLang;
        return currentLanguage;
    }
    
    // Detect from browser
    currentLanguage = detectBrowserLanguage();
    localStorage.setItem('preferredLanguage', currentLanguage);
    return currentLanguage;
}

/**
 * Set the current language and update translations dynamically
 * @param {string} lang - Language code to set
 */
function setLanguage(lang) {
    if (!languages.some(l => l.code === lang)) {
        console.error(`Unsupported language: ${lang}`);
        return;
    }
    
    localStorage.setItem('preferredLanguage', lang);
    const oldLanguage = currentLanguage;
    currentLanguage = lang;
    
    // Reload translations and update DOM without page reload
    loadTranslations().then(() => {
        // Trigger translation updates
        window.dispatchEvent(new CustomEvent('languageChanged', { 
            detail: { oldLanguage, newLanguage: lang } 
        }));
        
        // Update all elements with translations
        if (window.translationUpdater) {
            window.translationUpdater.updateAll();
            window.translationUpdater.updateMenu();
        }
        
        // Update language switcher if present
        if (window.languageSwitcher) {
            const container = document.getElementById('language-switcher-container');
            if (container) {
                window.languageSwitcher.render('language-switcher-container');
            }
        }
    });
}

/**
 * Load translation file for the current language
 * @returns {Promise<Object>} Translation object
 */
async function loadTranslations() {
    const lang = getCurrentLanguage();
    
    try {
        const response = await fetch(`/js/i18n/locales/${lang}.json`);
        if (!response.ok) {
            throw new Error(`Failed to load translations for ${lang}`);
        }
        translations = await response.json();
        return translations;
    } catch (error) {
        console.error('Error loading translations:', error);
        // Return empty object as fallback
        return {};
    }
}

/**
 * Get nested property from object using dot notation
 * @param {Object} obj - Object to search
 * @param {string} path - Dot-separated path (e.g., 'nav.dashboard')
 * @returns {*} Value at path or undefined
 */
function getNestedProperty(obj, path) {
    // Validate input
    if (!path || typeof path !== 'string') {
        return undefined;
    }
    
    return path.split('.').reduce((current, key) => current?.[key], obj);
}

/**
 * Translate a key to the current language
 * @param {string} key - Translation key (supports dot notation)
 * @returns {string} Translated text or key if translation not found
 */
function t(key) {
    if (!key) {
        return '';
    }
    
    const value = getNestedProperty(translations, key);
    
    // Return translated value if found, otherwise return the key itself as fallback
    return value !== undefined ? value : key;
}

/**
 * Initialize i18n system
 * Load translations and set up the language
 */
async function initializeI18n() {
    await loadTranslations();
    
    // Dispatch event to notify that i18n is ready
    window.dispatchEvent(new CustomEvent('i18nReady'));
}

// Auto-initialize when script loads
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initializeI18n);
} else {
    initializeI18n();
}

// Export functions to window object for global access
window.i18n = {
    getCurrentLanguage,
    setLanguage,
    t,
    languages,
    loadTranslations
};
