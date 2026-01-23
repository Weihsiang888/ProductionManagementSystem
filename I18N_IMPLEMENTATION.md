# i18n Implementation for ProductionManagementSystem

## Overview
This branch implements comprehensive internationalization (i18n) support for the Production Management System, allowing users to switch between Traditional Chinese (zh-TW), Simplified Chinese (zh-CN), and English (en-US).

## Changes Made

### 1. Infrastructure Setup
- **Created Resources folder** with marker class `SharedResource.cs`
- **Created .resx files** for three languages:
  - `ProductionManagementSystem.SharedResource.resx` (zh-TW - default)
  - `ProductionManagementSystem.SharedResource.en-US.resx`
  - `ProductionManagementSystem.SharedResource.zh-CN.resx`

### 2. Program.cs Configuration
- Added localization services: `builder.Services.AddLocalization(options => options.ResourcesPath = "Resources")`
- Configured `RequestLocalizationOptions` with supported cultures
- Set zh-TW as default culture
- Prioritized `CookieRequestCultureProvider` for culture selection
- Added `app.UseRequestLocalization(requestLocalizationOptions)` middleware

### 3. Culture Controller
- Created `Controllers/CultureController.cs`
- Implements GET endpoint `/Culture/Set?culture={culture}&redirectUri={uri}`
- Sets culture cookie with 1-year expiration
- Includes security validation for redirect URIs (only allows local URLs)

### 4. Global Localization Support
- Updated `_Imports.razor` to include:
  - `@using Microsoft.Extensions.Localization`
  - `@using ProductionManagementSystem`
  - `@inject IStringLocalizer<SharedResource> L`
- All components now have access to localizer via `L` variable

### 5. Header Component
- Added language selector dropdown menu
- Implemented `ChangeLanguage()` method
- Replaced hardcoded strings with localized versions:
  - System title
  - Home menu item
  - ESOP menu item
  - Login/Logout labels

### 6. Page Updates
Updated pages to use localization:
- `ESOP_Folder_Page.razor` - Grid captions and buttons
- `ESOPStatusPage.razor` - Headers and column captions
- More pages follow this pattern (see resource keys)

### 7. Resource Keys
Initial resource keys include:
- Navigation: Home, ESOP, SelectLanguage
- Actions: OpenESOP, OpenSOP, Close, Cancel, Save, Print, Login, Logout
- Labels: Directory, Version, FileName, StationName, ProcedureName, ModelName
- System: SystemTitle, LoginGuest, PleaseSelect, Error_NoESOPDocument
- Languages: TraditionalChinese, SimplifiedChinese, English

## Testing Instructions

### How to Test Language Switching

1. **Start the application:**
   ```bash
   dotnet run
   ```

2. **Access the header menu:**
   - Look for the "選擇語言" (Select Language) menu item
   - Click to open dropdown

3. **Select a language:**
   - Choose from: 繁體中文 (zh-TW), 簡體中文 (zh-CN), or English (en-US)
   - The page will reload with the new language

4. **Verify cookie persistence:**
   - Check browser cookies for `.AspNetCore.Culture`
   - Format: `c=zh-TW|uic=zh-TW` (or zh-CN, en-US)
   - Cookie expires in 1 year

5. **Verify UI changes:**
   - Check header title changes language
   - Check menu items change language
   - Check grid captions and buttons change language

### Cookie Verification
In browser DevTools:
1. Open Application/Storage → Cookies
2. Look for `.AspNetCore.Culture` cookie
3. Value should be: `c={culture}|uic={culture}`

## Known Limitations

### 1. External Dependencies
This project references external libraries (`CommonLibrary`, `RazorCommonLibrary`) that are not available in the sandbox environment. The implementation is complete but cannot be built/tested without these dependencies.

### 2. Remaining Pages
Due to the scope of changes, not all 47 pages have been updated yet. The pattern has been established in:
- Header.razor
- ESOP_Folder_Page.razor
- ESOPStatusPage.razor

Additional pages can follow the same pattern:
- Replace hardcoded strings with `@L["ResourceKey"]`
- Add new resource keys as needed
- Maintain all existing logic

### 3. PDF.js Localization
The PDF.js viewer locale switching has not been implemented in this PR. This would require:
- Frontend JavaScript to detect current culture
- Dynamic loading of PDF.js locale files
- Integration with `/pdfjs/web/locale/{lang}/viewer.ftl`

**Recommendation:** Handle PDF.js localization in a separate PR to minimize risk.

### 4. Dynamic Strings
Some strings may contain:
- Variable interpolation
- Formatted strings
- Complex expressions

These should be reviewed manually and handled on a case-by-case basis.

## File Structure

```
ProductionManagementSystem/
├── Controllers/
│   └── CultureController.cs (NEW)
├── Resources/ (NEW)
│   ├── SharedResource.cs
│   ├── ProductionManagementSystem.SharedResource.resx
│   ├── ProductionManagementSystem.SharedResource.en-US.resx
│   └── ProductionManagementSystem.SharedResource.zh-CN.resx
├── Program.cs (MODIFIED)
├── _Imports.razor (MODIFIED)
├── Shared/
│   └── Header.razor (MODIFIED)
└── Pages/
    ├── ESOP_Folder_Page.razor (MODIFIED)
    └── ESOPStatusPage.razor (MODIFIED)
```

## Rollback Instructions

If you need to rollback this feature:

### Option 1: Close/Delete Branch
```bash
git checkout main
git branch -D feature/i18n
git push origin --delete feature/i18n
```

### Option 2: Revert Commits
```bash
git revert <commit-hash>
git push origin copilot/featurei18n-support
```

### Option 3: Feature Flag (Future)
Consider implementing a feature flag to disable i18n without code changes.

## Security Considerations

### CultureController
- Only accepts local redirects (via `Url.IsLocalUrl()`)
- Validates referer header host matches request host
- Falls back to root path if validation fails
- Cookie is marked as essential and scoped to root path

### Cookie Settings
- Path: `/` (site-wide)
- Expires: 1 year
- IsEssential: true
- Secure: true (HTTPS only)
- HttpOnly: true (not accessible via JavaScript)
- SameSite: Lax (CSRF protection)

## Future Improvements

1. **Complete Page Coverage**
   - Update remaining 44+ pages
   - Add more resource keys as needed
   - Handle dynamic strings appropriately

2. **PDF.js Integration**
   - Implement client-side locale switching
   - Load appropriate viewer.ftl files

3. **Resource Management**
   - Consider using ResX Manager for easier editing
   - Add validation for missing translations
   - Implement fallback mechanisms

4. **Testing**
   - Add unit tests for CultureController
   - Add integration tests for language switching
   - Test cookie persistence across sessions

5. **Performance**
   - Consider caching localized strings
   - Monitor impact on page load times

## Contributing

When adding new UI strings:
1. Add key to all three .resx files
2. Use descriptive key names (PascalCase)
3. Ensure translations are accurate
4. Test all three languages

## Support

For questions or issues:
- Review this documentation
- Check resource files for available keys
- Follow established patterns in modified pages
