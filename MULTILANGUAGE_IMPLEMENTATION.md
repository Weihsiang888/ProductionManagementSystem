# Multi-Language Implementation Guide

This document describes the multi-language (i18n/L10n) architecture implemented in **ProductionManagementSystem**, supporting Traditional Chinese (`zh-TW`, default) and English (`en`).

---

## Architecture Overview

The localization system uses **ASP.NET Core Localization** with the following components:

| Component | Role |
|-----------|------|
| `Program.cs` | Registers localization services and middleware |
| `Resources/SharedResources.resx` | Default (zh-TW) fallback resource file |
| `Resources/SharedResources.zh-TW.resx` | Traditional Chinese resource strings |
| `Resources/SharedResources.en.resx` | English resource strings |
| `Resources/SharedResources.cs` | Marker class for `IStringLocalizer<SharedResources>` |
| `Shared/LanguageSwitcher.razor` | UI component for switching languages |

### Culture Switching via Query String

The active culture is determined by the `culture` and `ui-culture` query string parameters:

```
https://yoursite.com/                          → zh-TW (default)
https://yoursite.com/?culture=en&ui-culture=en → English
https://yoursite.com/?culture=zh-TW&ui-culture=zh-TW → Traditional Chinese (explicit)
```

This is handled by `QueryStringRequestCultureProvider` configured in `Program.cs`.

---

## How It Works

### 1. Service Registration (`Program.cs`)

```csharp
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var supportedCultures = new[] { "zh-TW", "en" };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("zh-TW");
    options.SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
    options.SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new QueryStringRequestCultureProvider { QueryStringKey = "culture", UIQueryStringKey = "ui-culture" },
    };
});
```

### 2. Middleware (`Program.cs`)

```csharp
app.UseRequestLocalization();  // Must be before UseRouting()
app.UseRouting();
```

### 3. Injecting the Localizer (Razor Components)

In any Razor component or page, inject and use `IStringLocalizer<SharedResources>`:

```razor
@inject IStringLocalizer<SharedResources> Localizer

<h1>@Localizer["SystemTitle"]</h1>
<button>@Localizer["Save"]</button>
```

The global `_Imports.razor` already includes:
```razor
@using Microsoft.Extensions.Localization
@using DxBlazorApplication7.Resources
```
So you do **not** need to add these `@using` directives in individual components.

---

## Resource File Structure

```
Resources/
├── SharedResources.cs           ← Marker class (required)
├── SharedResources.resx         ← Default fallback (zh-TW content)
├── SharedResources.zh-TW.resx   ← Traditional Chinese
└── SharedResources.en.resx      ← English
```

### Available Resource Keys

#### Common
| Key | zh-TW | en |
|-----|-------|----|
| `Home` | 首頁 | Home |
| `Save` | 儲存 | Save |
| `Delete` | 刪除 | Delete |
| `Search` | 搜尋 | Search |
| `Export` | 匯出 | Export |
| `Refresh` | 重新整理 | Refresh |
| `Cancel` | 取消 | Cancel |
| `Confirm` | 確認 | Confirm |
| `Edit` | 編輯 | Edit |
| `Add` | 新增 | Add |
| `Close` | 關閉 | Close |
| `Login` | 登入 | Login |
| `Logout` | 登出 | Logout |
| `Language` | 語言 | Language |

#### Header / System
| Key | zh-TW | en |
|-----|-------|----|
| `SystemTitle` | 產線 ( 報工 - ESOP ) 系統 | Production Line ( Work Report - ESOP ) System |
| `LoginGuest` | Login : Guest | Login : Guest |
| `LogoutUser` | 登出 | Logout |

#### App (Authorization/NotFound)
| Key | zh-TW | en |
|-----|-------|----|
| `NotAuthorized` | 你沒有授權可以存取該服務 | You are not authorized to access this service |
| `PageNotFound` | 找不到此頁面 | Page Not Found |
| `PageNotFoundDesc1` | 對不起，未能找到您要的網頁。 | Sorry, the page you are looking for does not exist. |
| `PageNotFoundDesc2` | 可能該網頁已被移除... | The page may have been removed... |

#### Navigation (Nav_*)
| Key | zh-TW | en |
|-----|-------|----|
| `Nav_Home` | 首頁 | Home |
| `Nav_OperatorInformation` | 產線人員資本資料 | Operator Information |
| `Nav_OperatorAbility` | 作業人員技能配置 | Operator Ability |
| `Nav_TimePeriod` | 產線工作時段 | Time Period |
| `Nav_OvertimeSchedule` | 產線加班時段 | Overtime Schedule |
| `Nav_WorkingTypeGroup` | 產線工站 | Working Type Group |
| `Nav_WorkingType` | 產線工站作業程序 | Working Type |
| `Nav_WorkingListRobotAssembly` | 產線人員派工作業 | Working List |
| `Nav_WorkingResponse` | 線上報工作業 | Working Response |
| `Nav_WorkingReport` | 產線人員派工細節 | Working Report |
| `Nav_StatisticReport` | 工單統計(工時)報表 | Statistic Report |
| `Nav_OperatorReport` | 人員統計(工時)報表 | Operator Report |

#### Index Page (Index_*)
Keys: `Index_Title1`, `Index_Desc1`, `Index_Desc2`, `Index_Feature1`–`Index_Feature5`, `Index_Summary`, `Index_Title2`, `Index_SOP1`–`Index_SOP9`

---

## How to Add a New Language

1. **Create a new resource file** in `Resources/`:
   ```
   Resources/SharedResources.{culture-code}.resx
   ```
   Example for Japanese: `Resources/SharedResources.ja.resx`

2. **Add all keys** from `SharedResources.en.resx` with translated values.

3. **Register the culture** in `Program.cs`:
   ```csharp
   var supportedCultures = new[] { "zh-TW", "en", "ja" };
   ```

4. **Add a button** in `Shared/LanguageSwitcher.razor`:
   ```razor
   <button class="btn btn-sm ..." @onclick='() => SwitchLanguage("ja")'>日本語</button>
   ```

---

## How to Add/Maintain Resource Keys

1. Open the appropriate `.resx` file (e.g., `SharedResources.zh-TW.resx`).
2. Add a new `<data>` element:
   ```xml
   <data name="MyNewKey" xml:space="preserve"><value>我的新文字</value></data>
   ```
3. Add the corresponding key to **all** language files (`SharedResources.en.resx`, `SharedResources.resx`, etc.).
4. Use the key in your component:
   ```razor
   @Localizer["MyNewKey"]
   ```

> **Tip:** Key names use PascalCase. Group related keys with a prefix (e.g., `Nav_`, `Index_`).

---

## LanguageSwitcher Component

The `Shared/LanguageSwitcher.razor` component renders language toggle buttons in the Header (top-right area). It:

- Reads the current culture from the URL's `culture` query parameter.
- On button click, updates `culture` and `ui-culture` in the query string and reloads the page (`forceLoad: true`) so the server-side culture provider can take effect.
- Does **not** use cookies; culture state is entirely in the URL query string.

### Example Usage

The component is embedded in `Header.razor` as a `DxMenuItem`:
```razor
<DxMenuItem CssClass="notoggle" Position="ItemPosition.End">
    <Template>
        <LanguageSwitcher />
    </Template>
</DxMenuItem>
```

---

## Query String Rules and Example URLs

| Action | URL Example |
|--------|-------------|
| Default (zh-TW) | `https://yoursite.com/` |
| Switch to English | `https://yoursite.com/?culture=en&ui-culture=en` |
| Switch to zh-TW (explicit) | `https://yoursite.com/?culture=zh-TW&ui-culture=zh-TW` |
| Navigate to a page in English | `https://yoursite.com/ESOPPage?culture=en&ui-culture=en` |

> **Note:** The culture query parameters are preserved when the `LanguageSwitcher` switches languages on any page.
