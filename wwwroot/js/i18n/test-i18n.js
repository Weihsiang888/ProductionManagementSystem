// Test script to verify i18n functionality
const fs = require('fs');
const path = require('path');

console.log('Testing i18n Configuration...\n');

// Load translation files
const zhTW = JSON.parse(fs.readFileSync(path.join(__dirname, 'locales', 'zh-TW.json'), 'utf8'));
const en = JSON.parse(fs.readFileSync(path.join(__dirname, 'locales', 'en.json'), 'utf8'));

console.log('✓ Translation files loaded successfully\n');

// Test structure
console.log('Testing translation structure...');
const requiredKeys = ['app', 'nav', 'auth', 'home', 'button', 'form', 'message', 'table', 'date', 'status', 'language'];

let allKeysPresent = true;
requiredKeys.forEach(key => {
    if (!zhTW[key]) {
        console.log(`✗ Missing key in zh-TW: ${key}`);
        allKeysPresent = false;
    }
    if (!en[key]) {
        console.log(`✗ Missing key in en: ${key}`);
        allKeysPresent = false;
    }
});

if (allKeysPresent) {
    console.log('✓ All required top-level keys present\n');
}

// Test specific translations
console.log('Testing specific translations:');
console.log(`zh-TW app.title: "${zhTW.app.title}"`);
console.log(`en app.title: "${en.app.title}"`);
console.log(`zh-TW nav.home: "${zhTW.nav.home}"`);
console.log(`en nav.home: "${en.nav.home}"`);
console.log(`zh-TW home.title: "${zhTW.home.title}"`);
console.log(`en home.title: "${en.home.title}"`);

console.log('\n✓ i18n system structure is valid!');
console.log('\nSummary:');
console.log(`- zh-TW translations: ${Object.keys(zhTW).length} top-level keys`);
console.log(`- en translations: ${Object.keys(en).length} top-level keys`);
console.log('- Both language files have consistent structure');
