# i18n (Internationalization) System Documentation

## Overview

This Production Management System now includes comprehensive multilingual support for Traditional Chinese (zh-TW) and English (en). The i18n system provides automatic language detection, persistent language preferences, and seamless translation across the entire application.

## Architecture

### Directory Structure

```
wwwroot/js/i18n/
├── config.js               # Core i18n configuration and initialization
├── languageSwitcher.js     # Language switcher UI component
├── translationUpdater.js   # DOM translation update handler
└── locales/
    ├── zh-TW.json         # Traditional Chinese translations
    └── en.json            # English translations
```

## Core Features

### 1. Automatic Language Detection
- Detects browser language on first visit
- Falls back to Traditional Chinese (zh-TW) as default
- Supports both `zh` and `en` language codes

### 2. Persistent Language Preference
- Stores selected language in `localStorage` with key `preferredLanguage`
- Automatically loads saved preference on subsequent visits
- Persists across browser sessions

### 3. Translation Function
- `window.i18n.t(key)` - Main translation function
- Supports nested keys using dot notation (e.g., `t('nav.dashboard')`)
- Returns original key if translation is missing (graceful fallback)

### 4. Language Switcher UI
- Visual dropdown component with language options
- Shows current language with indicator
- Provides instant language switching
- Automatically reloads page to apply translations

### 5. DOM Translation Updates
- Automatic updates for elements with `data-i18n` attribute
- Preserves HTML formatting (spacing, line breaks)
- Supports attribute translation with `data-i18n-attr`
- MutationObserver for dynamic content updates

## Usage

### Basic Translation in Razor Components

Add the `data-i18n` attribute to any HTML element:

```html
<h1 data-i18n="app.title">產線 ( 報工 - ESOP ) 系統</h1>
<button data-i18n="button.submit">提交</button>
```

### Translation with HTML Formatting

Preserve spacing and line breaks:

```html
<h5 class="pb-2" data-i18n="home.title">&emsp;&emsp;&emsp;線上報工<br></h5>
```

The translation updater automatically preserves `&emsp;`, `&nbsp;`, and `<br>` tags.

### Using Translation Function in JavaScript

```javascript
// Wait for i18n to be ready
window.addEventListener('i18nReady', () => {
    // Get translation
    const title = window.i18n.t('app.title');
    
    // Get current language
    const lang = window.i18n.getCurrentLanguage(); // 'zh-TW' or 'en'
    
    // Change language
    window.i18n.setLanguage('en'); // Reloads page
});
```

### Adding the Language Switcher

The language switcher is automatically rendered in any element with id `language-switcher-container`:

```html
<div id="language-switcher-container"></div>
```

## Translation File Structure

Both `zh-TW.json` and `en.json` follow the same structure:

```json
{
  "app": {
    "title": "Application Title",
    "subtitle": "Subtitle"
  },
  "nav": {
    "home": "Home",
    "dashboard": "Dashboard"
  },
  "button": {
    "submit": "Submit",
    "cancel": "Cancel"
  },
  "message": {
    "success": "Success",
    "error": "Error"
  }
}
```

### Translation Categories

1. **app** - Application-wide text (title, subtitle)
2. **nav** - Navigation menu items
3. **auth** - Authentication (login, logout, username, password)
4. **home** - Homepage content
5. **button** - Common button labels
6. **form** - Form labels and validation messages
7. **message** - System messages (success, error, confirmations)
8. **table** - Table headers and pagination
9. **date** - Date-related labels
10. **status** - Status indicators
11. **language** - Language-related text

## Adding New Translations

### Step 1: Add to Translation Files

Add the same key to both `zh-TW.json` and `en.json`:

**zh-TW.json:**
```json
{
  "mySection": {
    "myKey": "我的翻譯"
  }
}
```

**en.json:**
```json
{
  "mySection": {
    "myKey": "My Translation"
  }
}
```

### Step 2: Use in HTML

```html
<div data-i18n="mySection.myKey">我的翻譯</div>
```

### Step 3: Or Use in JavaScript

```javascript
const text = window.i18n.t('mySection.myKey');
```

## API Reference

### window.i18n

Main i18n namespace providing all translation functionality.

#### Methods

**`getCurrentLanguage(): string`**
- Returns: Current language code ('zh-TW' or 'en')
- Example: `const lang = window.i18n.getCurrentLanguage();`

**`setLanguage(lang: string): void`**
- Parameters: Language code to set
- Effect: Saves to localStorage and reloads page
- Example: `window.i18n.setLanguage('en');`

**`t(key: string): string`**
- Parameters: Translation key (supports dot notation)
- Returns: Translated text or original key if not found
- Example: `const text = window.i18n.t('nav.home');`

**`loadTranslations(): Promise<Object>`**
- Returns: Promise resolving to translation object
- Used internally, rarely needed externally

#### Properties

**`languages: Array<Object>`**
- Array of supported language objects
- Structure: `[{ code: 'zh-TW', name: '繁體中文', nativeName: '繁體中文' }, ...]`

### window.languageSwitcher

Language switcher UI component.

#### Methods

**`render(containerId: string): void`**
- Parameters: ID of container element
- Effect: Renders language switcher dropdown
- Example: `window.languageSwitcher.render('my-container');`

### window.translationUpdater

DOM translation update handler.

#### Methods

**`updateElement(elementId: string, translationKey: string): void`**
- Updates specific element with translation
- Example: `window.translationUpdater.updateElement('myDiv', 'nav.home');`

**`updateAll(): void`**
- Updates all elements with data-i18n attributes
- Called automatically on page load

**`updateMenu(): void`**
- Updates Blazor menu items (DevExpress specific)
- Called automatically on page load

**`init(): void`**
- Initializes translation updates and MutationObserver
- Called automatically when i18n is ready

## Events

### i18nReady

Fired when i18n system is initialized and translations are loaded.

```javascript
window.addEventListener('i18nReady', () => {
    console.log('i18n is ready!');
    // Safe to use window.i18n.t() here
});
```

## Best Practices

1. **Always provide fallback text**: Include default text in HTML elements even when using data-i18n
2. **Use consistent naming**: Follow the established key structure (category.item)
3. **Group related translations**: Keep translations organized by feature/page
4. **Test both languages**: Verify translations in both zh-TW and en
5. **Handle missing translations**: The system gracefully returns the key if translation is missing

## Integration with Blazor

The i18n system is integrated into the Blazor application through:

1. **_Layout.cshtml** - Includes all i18n scripts
2. **Header.razor** - Contains language switcher container
3. **Index.razor** - Uses data-i18n attributes for content
4. **Other Razor pages** - Can use data-i18n attributes as needed

## Testing

A test script is provided to verify translations:

```bash
cd wwwroot/js/i18n
node test-i18n.js
```

This validates:
- Both translation files load correctly
- All required keys are present
- Structure is consistent between languages

## Browser Support

The i18n system uses modern JavaScript features:
- localStorage API
- Fetch API
- Arrow functions
- Template literals
- Async/await

Supported browsers:
- Chrome/Edge 60+
- Firefox 55+
- Safari 11+
- Opera 47+

## Troubleshooting

### Translations not appearing

1. Check browser console for errors
2. Verify translation key exists in both JSON files
3. Ensure scripts are loaded in correct order in _Layout.cshtml
4. Wait for 'i18nReady' event before using translations

### Language not persisting

1. Check if localStorage is enabled in browser
2. Verify no browser extensions blocking localStorage
3. Check browser privacy settings

### Language switcher not showing

1. Ensure container element has correct id: `language-switcher-container`
2. Check if scripts are loaded properly
3. Verify no JavaScript errors in console

## Future Enhancements

Potential improvements for the i18n system:

1. Additional language support (Simplified Chinese, Japanese, etc.)
2. Date/time localization formatting
3. Number formatting based on locale
4. Currency formatting
5. Pluralization support
6. Translation memory/caching optimization
7. Admin interface for managing translations
8. A/B testing for translations

## License

This i18n implementation is part of the Production Management System and follows the same license terms.
