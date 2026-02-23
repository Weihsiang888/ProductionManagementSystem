# Multi-Language Implementation Guide

This application supports Traditional Chinese (zh-TW) and English (en) using ASP.NET Core localization.

## How It Works

- Resource files in `Resources/` folder contain key-value pairs for each language
- `SharedResources.resx` - Default (Traditional Chinese)
- `SharedResources.zh-TW.resx` - Traditional Chinese
- `SharedResources.en.resx` - English
- Culture is selected via query string parameters: `?culture=en&ui-culture=en`
- The `LanguageSwitcher` component in the header allows switching languages

## How to Switch Language

Append `?culture=en&ui-culture=en` or `?culture=zh-TW&ui-culture=zh-TW` to any URL.

Example: `https://yourapp/?culture=en&ui-culture=en`

## How to Add a New Language

1. Create `Resources/SharedResources.{culture-code}.resx` (e.g., `SharedResources.ja.resx` for Japanese)
2. Add all keys from `SharedResources.resx` with translated values
3. Add the culture code to the `supportedCultures` array in `Program.cs`
4. Add a button in `Shared/LanguageSwitcher.razor`

## How to Add New Resource Keys

1. Add the key/value to `Resources/SharedResources.resx` (zh-TW default)
2. Add the key/value to `Resources/SharedResources.zh-TW.resx`
3. Add the key/value to `Resources/SharedResources.en.resx`
4. Use `@L["KeyName"]` in your Razor components (requires `@inject IStringLocalizer<SharedResources> L`)

## Testing

Test with English: Add `?culture=en&ui-culture=en` to the URL
Test with Traditional Chinese: Add `?culture=zh-TW&ui-culture=zh-TW` to the URL
