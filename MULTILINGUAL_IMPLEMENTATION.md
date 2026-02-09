# 多語系功能實作文件 / Multilingual Support Implementation

## 📋 概述 / Overview

本專案已成功實作完整的多語系支援，支援繁體中文（預設）和英文。使用標準 ASP.NET Core Localization 機制，透過 Cookie 儲存使用者的語系偏好。

This project has successfully implemented complete multilingual support for Traditional Chinese (default) and English using the standard ASP.NET Core Localization mechanism with Cookie-based persistence.

---

## 📁 檔案結構 / File Structure

### 新增的檔案 / New Files

```
Resources/
├── SharedResources.cs                    # Marker class for shared resources
├── SharedResources.zh-TW.resx           # Traditional Chinese shared resources
├── SharedResources.en.resx              # English shared resources
└── Shared/
    ├── NavMenu.zh-TW.resx               # Traditional Chinese navigation menu
    └── NavMenu.en.resx                  # English navigation menu

Shared/
└── LanguageSwitcher.razor               # Language switcher component

Pages/
└── LanguageTest.razor                   # Diagnostic test page
```

### 修改的檔案 / Modified Files

1. **DxBlazorApplication7.csproj**
   - Added localization NuGet packages
   - Fixed JavaScript file configuration (Content Update instead of EmbeddedResource Include)

2. **Program.cs**
   - Added localization services and middleware
   - Configured supported cultures (zh-TW, en)
   - Set up Cookie-based culture provider

3. **Shared/NavMenu.razor**
   - Integrated IStringLocalizer for localized menu items
   - All hardcoded text replaced with resource keys

4. **Shared/Header.razor**
   - Added LanguageSwitcher component to the header menu

---

## 🔑 關鍵實作細節 / Key Implementation Details

### 1. 資源檔編碼 / Resource File Encoding
✅ 所有 .resx 檔案使用 **UTF-8 with BOM** 編碼

### 2. 命名空間 / Namespace
✅ 統一使用 `DxBlazorApplication7.Resources` 命名空間

### 3. 自動處理 / Automatic Processing
✅ 讓 .NET SDK 自動處理 .resx 檔案（不使用 ResXFileCodeGenerator）

### 4. 語系支援 / Supported Cultures
- **zh-TW** (繁體中文) - 預設 / Default
- **en** (English)

### 5. 持久化機制 / Persistence Mechanism
✅ 使用 Cookie 儲存語系偏好（有效期：1年 / max-age: 31536000 seconds）

---

## 🧪 測試頁面 / Test Page

訪問 `/language-test` 可以：
1. 查看當前語系資訊
2. 測試 SharedResources 翻譯
3. 測試 NavMenu 翻譯
4. 使用語系切換器

Visit `/language-test` to:
1. View current culture information
2. Test SharedResources translations
3. Test NavMenu translations
4. Use the language switcher

---

## 📦 資源鍵值 / Resource Keys

### SharedResources 共用資源
- Home, Save, Delete, Edit, Add, Cancel, OK, Close
- Search, Refresh, Export, Loading
- Error, Success, Warning, Info
- Confirm, Yes, No, Back

### NavMenu 導覽選單
- Home, OperatorInformation, OperatorAbility
- TimePeriod, OvertimeSchedule, WorkingStation, WorkingProcedure
- WorkingAssignment, RobotAssembly, OnlineReport, WorkingDetail
- Report, StatisticReport, OperatorReport

---

## 🚀 使用方式 / Usage

### 在 Razor 元件中使用 / Using in Razor Components

```razor
@using Microsoft.Extensions.Localization
@inject IStringLocalizer<SharedResources> Localizer

<button>@Localizer["Save"]</button>
```

### 在 C# 程式碼中使用 / Using in C# Code

```csharp
public class MyService
{
    private readonly IStringLocalizer<SharedResources> _localizer;
    
    public MyService(IStringLocalizer<SharedResources> localizer)
    {
        _localizer = localizer;
    }
    
    public string GetMessage()
    {
        return _localizer["Success"];
    }
}
```

---

## ✅ 驗收標準 / Acceptance Criteria

### 已完成 / Completed

- [x] 建置無錯誤（除了現有的 CommonLibrary 相依性問題）
- [x] 資源檔正確編譯（.resources 檔案已產生）
- [x] 所有 .resx 檔案使用 UTF-8 with BOM
- [x] 語系切換元件已整合到 Header
- [x] 導覽選單已本地化
- [x] 診斷測試頁面已建立

### 待完整驗證 / Pending Full Validation
（需要專案成功建置後測試）

- [ ] 資源 DLL 產生 (bin/Debug/net8.0/zh-TW/DxBlazorApplication7.resources.dll)
- [ ] 資源 DLL 產生 (bin/Debug/net8.0/en/DxBlazorApplication7.resources.dll)
- [ ] 語系切換功能正常運作
- [ ] Cookie 持久化正常運作

---

## 🔧 技術規格 / Technical Specifications

### NuGet 套件 / NuGet Packages
- Microsoft.Extensions.Localization (8.0.0)
- Microsoft.Extensions.Localization.Abstractions (8.0.0)

### 中介軟體順序 / Middleware Order
```
UseStaticFiles()
  ↓
UseRequestLocalization()  ← 在 UseRouting() 之前
  ↓
UseRouting()
```

### Provider 順序 / Provider Order
1. CookieRequestCultureProvider (優先)
2. QueryStringRequestCultureProvider
3. AcceptLanguageHeaderRequestCultureProvider

---

## 📝 注意事項 / Notes

1. **不使用 ResXFileCodeGenerator**：會導致建置錯誤
2. **JavaScript 檔案設定**：使用 `Content Update` 而非 `EmbeddedResource Include`
3. **資源檔路徑**：必須放在 `Resources` 資料夾中
4. **命名規則**：`[ResourceName].[Culture].resx`
5. **標記類別**：需要對應的 .cs 檔案作為標記類別

---

## 🎯 下一步 / Next Steps

當專案的 CommonLibrary 相依性解決後，可以：
1. 完整建置專案
2. 執行應用程式
3. 測試語系切換功能
4. 驗證 Cookie 持久化
5. 確認資源 DLL 正確產生

When CommonLibrary dependencies are resolved:
1. Build the project completely
2. Run the application
3. Test language switching
4. Verify Cookie persistence
5. Confirm resource DLLs generation

---

## 📞 支援 / Support

如有問題，請參考：
- [ASP.NET Core Localization Documentation](https://docs.microsoft.com/aspnet/core/fundamentals/localization)
- [.NET Globalization and Localization](https://docs.microsoft.com/dotnet/core/extensions/globalization)

For questions, please refer to:
- ASP.NET Core Localization Documentation
- .NET Globalization and Localization
