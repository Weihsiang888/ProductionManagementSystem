# 多語系功能實作說明 (Multilingual Implementation Guide)

## 已完成項目 (Completed Items)

### 1. 核心架構 (Core Architecture)
- ✅ Program.cs 配置完成
- ✅ 支援語言：繁體中文 (zh-TW) 和英文 (en)
- ✅ Cookie 持久化設定

### 2. 已本地化的元件 (Localized Components)
- ✅ Shared/Header.razor - 頁首元件
- ✅ Shared/NavMenu.razor - 導覽選單
- ✅ Shared/LanguageSwitcher.razor - 語言切換器
- ✅ Pages/Index.razor - 首頁
- ✅ App.razor - 錯誤訊息

### 3. 資源檔結構 (Resource File Structure)
```
Resources/
├── SharedResources.cs (空類別)
├── SharedResources.zh-TW.resx (共用資源 - 繁中)
├── SharedResources.en.resx (共用資源 - 英文)
├── Shared/
│   ├── Header.zh-TW.resx
│   ├── Header.en.resx
│   ├── NavMenu.zh-TW.resx
│   └── NavMenu.en.resx
└── Pages/
    ├── Index.zh-TW.resx
    └── Index.en.resx
```

## 如何為新頁面添加多語系支援 (How to Add Multilingual Support to New Pages)

### 步驟 1: 建立資源檔
在 `Resources/Pages/` 目錄下建立對應的 .resx 檔案：
- `PageName.zh-TW.resx` (繁體中文)
- `PageName.en.resx` (英文)

### 步驟 2: 更新 Razor 頁面
在頁面開頭加入：
```razor
@using Microsoft.Extensions.Localization
@inject IStringLocalizer<PageName> Localizer
```

### 步驟 3: 替換硬編碼文字
將原本的硬編碼中文文字：
```razor
<h1>產線人員資料</h1>
```

改為使用 Localizer：
```razor
<h1>@Localizer["OperatorInformation"]</h1>
```

## 語言切換功能 (Language Switching)

語言切換器已整合在 Header 中，使用者可以：
1. 點擊右上角的語言下拉選單
2. 選擇「繁體中文 🇹🇼」或「English 🇺🇸」
3. 頁面自動重新載入並顯示所選語言
4. 語言選擇會保存在 Cookie 中

## 待完成頁面 (Pages to be Localized)

以下頁面尚未本地化，可依照上述步驟進行：

### 管理頁面 (Management Pages)
- OperatorInformationPage.razor
- OperatorAbilityPage.razor
- TimePeriodPage.razor
- OvertimeSchedulePage.razor
- WorkingTypeGroupPage.razor
- WorkingTypePage.razor
- ComparisonPage.razor
- ReasonTypePage.razor
- StationStatusPage.razor
- ProcedureStatusPage.razor
- ModelStatusPage.razor

### 工作派工與報工 (Work Assignment and Reporting)
- WorkingListPage.razor
- WorkingListRobotAssemblyPage.razor
- WorkingListSubAssemblyIOModulePage.razor
- WorkingReportPage.razor
- WorkingResponsePage.razor
- WorkingOrderResponsePage.razor
- WorkingDataPage.razor
- ModifyWorkOrderPage.razor
- AdditionalTimePage.razor

### ESOP 相關頁面 (ESOP Pages)
- ESOPStatusPage.razor
- ESOPSelectionPage.razor
- ESOPCreatePage.razor
- ESOPMainMapPage.razor
- ESOPProcessMapPage.razor
- ESOP_1st_Page.razor
- ESOP_2nd_Page.razor
- ESOP_3rd_Page.razor
- ESOP_Folder_Page.razor

### 報表頁面 (Report Pages)
- Report/StatisticReportPage.razor
- Report/OperatorReportPage.razor
- Report/OperatorDayReportPage.razor
- Report/ReportPage.razor
- Report/ReportPage1.razor
- Report/ReportPage2.razor
- Report/ReportPage3.razor
- Report/ReportPage4.razor
- Report/ReportPage5.razor
- Report/ReportPage6.razor
- Report/ReportPage7.razor
- Report/ReportPage71.razor

### 登入/登出頁面 (Login/Logout Pages)
- LoginPage.razor
- LogoutPage.razor

## 技術細節 (Technical Details)

### Program.cs 配置
```csharp
// 設定支援的文化
var supportedCultures = new[]
{
    new CultureInfo("zh-TW"),
    new CultureInfo("en")
};

// 配置請求本地化選項
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("zh-TW");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
});
```

### Cookie 格式
語言選擇保存在 Cookie 中：
```
.AspNetCore.Culture=c=zh-TW|uic=zh-TW
```

## 常用翻譯參考 (Common Translations)

| 繁體中文 | English |
|---------|---------|
| 首頁 | Home |
| 儲存 | Save |
| 刪除 | Delete |
| 編輯 | Edit |
| 新增 | Add |
| 取消 | Cancel |
| 確定 | OK |
| 重新整理 | Refresh |
| 產線人員資本資料 | Operator Information |
| 作業人員技能配置 | Operator Skills Configuration |
| 產線工作時段 | Working Time Period |
| 產線加班時段 | Overtime Schedule |
| 產線工站 | Working Station |
| 產線工站作業程序 | Working Procedure |
| 線上報工作業 | Online Work Report |
| 工單統計(工時)報表 | Work Order Statistics (Hours) |
| 人員統計(工時)報表 | Operator Statistics (Hours) |

## 測試建議 (Testing Recommendations)

1. 切換語言後檢查所有文字是否正確顯示
2. 驗證 Cookie 是否正確保存語言選擇
3. 重新載入頁面後語言選擇應保持
4. 檢查 DevExpress 元件的文字是否本地化
5. 測試在不同瀏覽器中的相容性

## 注意事項 (Notes)

1. 不要修改 `wwwroot/pdfjs/web/locale/` 目錄下的檔案（這些是 PDF.js 的檔案）
2. 資源檔必須使用正確的 XML 格式
3. 命名規則：繁體中文使用 `.zh-TW.resx`，英文使用 `.en.resx`
4. 專業術語應保持一致性
5. 共用的翻譯可以放在 `SharedResources` 中
