# 多語系功能測試指南
# Multi-Language Feature Testing Guide

## 🎯 快速測試步驟 / Quick Test Steps

### 前置需求 / Prerequisites

確保 CommonLibrary 和 RazorCommonLibrary 專案可用後，執行以下步驟：

Once CommonLibrary and RazorCommonLibrary projects are available, follow these steps:

### 1. 建置專案 / Build Project

```bash
cd /path/to/ProductionManagementSystem
dotnet clean
dotnet restore
dotnet build
```

**預期結果 / Expected Result**: 
- ✅ 0 錯誤 (0 Errors)
- ✅ 0 警告或僅有 DevExpress 授權警告 (0 Warnings or only DevExpress license warnings)

### 2. 驗證資源 DLL / Verify Resource DLLs

```bash
ls -la bin/Debug/net8.0/zh-TW/
ls -la bin/Debug/net8.0/en/
```

**預期結果 / Expected Result**: 
```
bin/Debug/net8.0/zh-TW/DxBlazorApplication7.resources.dll  ✅
bin/Debug/net8.0/en/DxBlazorApplication7.resources.dll     ✅
```

### 3. 執行應用程式 / Run Application

```bash
dotnet run
```

或使用 Visual Studio / Rider 的 Debug 模式

### 4. 測試診斷頁面 / Test Diagnostic Page

在瀏覽器中訪問 / Open in browser:

```
http://localhost:5000/language-test
或
https://localhost:5001/language-test
```

**檢查項目 / Checklist**:
- [ ] 頁面正常載入
- [ ] 當前語系顯示為 zh-TW
- [ ] SharedResources 表格中所有項目顯示 ✅ 成功
- [ ] NavMenu 表格中所有項目顯示 ✅ 成功
- [ ] 成功率顯示 100%

### 5. 測試語系切換 / Test Language Switching

#### 5.1 切換至英文 / Switch to English

1. 在頁面右上角找到語系切換器
2. 點擊下拉選單
3. 選擇 "English 🇺🇸"
4. 頁面應該自動重新載入

**檢查項目 / Checklist**:
- [ ] 頁面自動重新載入
- [ ] 當前語系變更為 en
- [ ] 所有翻譯變更為英文
- [ ] 導覽選單顯示英文
- [ ] SharedResources 表格中的翻譯為英文
- [ ] NavMenu 表格中的翻譯為英文

#### 5.2 切換回繁體中文 / Switch back to Traditional Chinese

1. 點擊語系切換器
2. 選擇 "繁體中文 🇹🇼"
3. 頁面應該自動重新載入

**檢查項目 / Checklist**:
- [ ] 頁面自動重新載入
- [ ] 當前語系變更為 zh-TW
- [ ] 所有翻譯變更為繁體中文
- [ ] 導覽選單顯示繁體中文

### 6. 測試 Cookie 持久化 / Test Cookie Persistence

#### 6.1 設定語系 / Set Language

1. 使用語系切換器選擇英文
2. 等待頁面重新載入

#### 6.2 重新整理頁面 / Refresh Page

1. 按 F5 或點擊瀏覽器的重新整理按鈕
2. 或關閉瀏覽器後重新開啟

**檢查項目 / Checklist**:
- [ ] 重新整理後語系仍為英文
- [ ] Cookie 仍然存在（使用開發者工具檢查）
- [ ] Cookie 名稱為 `.AspNetCore.Culture`
- [ ] Cookie 值格式為 `c=en|uic=en`

#### 6.3 檢查 Cookie 詳細資訊 / Check Cookie Details

使用瀏覽器開發者工具（F12）：

1. Application / Storage → Cookies
2. 找到 `.AspNetCore.Culture` cookie

**預期值 / Expected Values**:
- Name: `.AspNetCore.Culture`
- Value: `c=en|uic=en` 或 `c=zh-TW|uic=zh-TW`
- Path: `/`
- Max-Age: 31536000 (1 年 / 1 year)

### 7. 測試導覽選單 / Test Navigation Menu

#### 7.1 繁體中文模式 / Traditional Chinese Mode

選擇繁體中文，檢查導覽選單項目：

**預期翻譯 / Expected Translations**:
- [ ] 首頁 (Home)
- [ ] 產線人員資本資料 (Operator Information)
- [ ] 作業人員技能配置 (Operator Ability)
- [ ] 產線工作時段 (Time Period)
- [ ] 產線加班時段 (Overtime Schedule)
- [ ] 產線工站 (Working Station)
- [ ] 產線工站作業程序 (Working Procedure)
- [ ] 產線人員派工作業 / RobotAssembly (Working Assignment / Robot Assembly)
- [ ] 線上報工作業 (Online Report)
- [ ] 產線人員派工細節 / Report (Working Detail / Report)
- [ ] 工單統計(工時)報表 (Statistic Report)
- [ ] 人員統計(工時)報表 (Operator Report)

#### 7.2 英文模式 / English Mode

選擇英文，檢查導覽選單項目：

**預期翻譯 / Expected Translations**:
- [ ] Home
- [ ] Operator Information
- [ ] Operator Ability Configuration
- [ ] Production Line Work Period
- [ ] Production Line Overtime Period
- [ ] Production Line Station
- [ ] Production Line Station Procedure
- [ ] Production Line Staff Assignment / Robot Assembly
- [ ] Online Reporting Operation
- [ ] Production Line Staff Assignment Detail / Report
- [ ] Work Order Statistics (Work Hours) Report
- [ ] Operator Statistics (Work Hours) Report

### 8. 測試各頁面翻譯 / Test Page Translations

訪問不同頁面，確認翻譯正確：

#### 首頁 / Home Page
```
http://localhost:5000/
```
- [ ] 標題和內容根據語系顯示

#### 其他頁面 / Other Pages
- [ ] OperatorInformation
- [ ] OperatorAbilityPage
- [ ] TimePeriod
- [ ] OvertimeSchedule
- [ ] WorkingTypeGroup
- [ ] WorkingType
- [ ] WorkingListRobotAssembly
- [ ] WorkingResponsePage/Detail
- [ ] WorkingReportPage
- [ ] StatisticReport
- [ ] OperatorReport

---

## 🔍 進階測試 / Advanced Testing

### 測試資源載入 / Test Resource Loading

#### 使用反射檢查資源 / Check Resources with Reflection

在任何 Razor 頁面或組件中添加：

```csharp
@inject IStringLocalizer<DxBlazorApplication7.Resources.SharedResources> SharedLocalizer

@code {
    protected override void OnInitialized()
    {
        // 測試翻譯
        var home = SharedLocalizer["Home"];
        var save = SharedLocalizer["Save"];
        Console.WriteLine($"Home: {home}");
        Console.WriteLine($"Save: {save}");
    }
}
```

**預期輸出 / Expected Output**:
- zh-TW: `Home: 首頁`, `Save: 儲存`
- en: `Home: Home`, `Save: Save`

### 測試 IStringLocalizer 注入 / Test IStringLocalizer Injection

創建測試組件：

```razor
@page "/localizer-test"
@inject IStringLocalizer<DxBlazorApplication7.Resources.SharedResources> Localizer
@inject IStringLocalizer<NavMenu> NavLocalizer

<h3>Localizer Test</h3>

<p>SharedResources.Home: @Localizer["Home"]</p>
<p>SharedResources.Save: @Localizer["Save"]</p>
<p>NavMenu.Home: @NavLocalizer["Home"]</p>
<p>NavMenu.OperatorInformation: @NavLocalizer["OperatorInformation"]</p>

<p>Current Culture: @System.Globalization.CultureInfo.CurrentCulture.Name</p>
<p>Current UI Culture: @System.Globalization.CultureInfo.CurrentUICulture.Name</p>
```

### 測試資源不存在的情況 / Test Missing Resources

```csharp
var nonExistent = SharedLocalizer["NonExistentKey"];
// 應該返回 "NonExistentKey" (鍵值本身)
```

---

## 📊 驗收標準 / Acceptance Criteria

### ✅ 全部通過才算完成 / All Must Pass

1. [ ] `dotnet build` 成功，0 錯誤
2. [ ] 資源 DLL 存在於 `bin/Debug/net8.0/zh-TW/` 和 `en/`
3. [ ] `/language-test` 頁面所有翻譯顯示 ✅
4. [ ] 語系切換正常運作（立即生效）
5. [ ] Cookie 持久化正常（重新整理後保持語系）
6. [ ] 導覽選單翻譯正確
7. [ ] 所有頁面翻譯正確
8. [ ] 預設語系為繁體中文（zh-TW）

---

## 🐛 常見問題排除 / Troubleshooting

### 問題 1: 翻譯不顯示，顯示資源鍵 / Translations not showing, displaying resource keys

**可能原因 / Possible Causes**:
1. 資源 DLL 未生成
2. 資源檔案路徑不正確
3. IStringLocalizer 注入不正確

**解決方法 / Solutions**:
```bash
# 1. 清理並重新建置
dotnet clean
dotnet build

# 2. 檢查資源 DLL
ls -la bin/Debug/net8.0/zh-TW/
ls -la bin/Debug/net8.0/en/

# 3. 檢查 Program.cs 配置
# 確認: builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
```

### 問題 2: 語系切換後沒有效果 / Language switching has no effect

**可能原因 / Possible Causes**:
1. Cookie 沒有正確設定
2. UseRequestLocalization 中間件未配置
3. JavaScript 執行失敗

**解決方法 / Solutions**:
```csharp
// 1. 檢查 Program.cs
app.UseRequestLocalization(); // 必須在 UseRouting 之前

// 2. 檢查瀏覽器開發者工具 Console
// 看是否有 JavaScript 錯誤

// 3. 手動檢查 Cookie
// Application → Cookies → .AspNetCore.Culture
```

### 問題 3: Cookie 沒有持久化 / Cookie not persisting

**可能原因 / Possible Causes**:
1. Cookie 設定錯誤
2. 瀏覽器隱私設定阻擋 Cookie

**解決方法 / Solutions**:
```javascript
// 檢查 LanguageSwitcher.razor 中的 Cookie 設定
document.cookie = '.AspNetCore.Culture=c={culture}|uic={culture}; path=/; max-age=31536000';

// 確認瀏覽器允許 Cookie
// 設定 → 隱私和安全性 → Cookie 和網站資料
```

### 問題 4: 建置錯誤 ResXFileCodeGenerator / Build error ResXFileCodeGenerator

**解決方法 / Solution**:
```xml
<!-- 在 .csproj 中，確保沒有以下設定： -->
<!-- ❌ 錯誤做法 -->
<EmbeddedResource Update="Resources\SharedResources.zh-TW.resx">
  <Generator>ResXFileCodeGenerator</Generator>
</EmbeddedResource>

<!-- ✅ 正確做法：完全不設定 Generator，讓 .NET SDK 自動處理 -->
```

---

## 📝 測試報告範本 / Test Report Template

```markdown
# 多語系功能測試報告
# Multi-Language Feature Test Report

**測試日期 / Test Date**: YYYY-MM-DD
**測試人員 / Tester**: [Your Name]
**環境 / Environment**: Development / Staging / Production

## 測試結果 / Test Results

### 1. 建置測試 / Build Test
- [ ] ✅ 通過 / Pass
- [ ] ❌ 失敗 / Fail
- **備註 / Notes**: 

### 2. 資源 DLL 驗證 / Resource DLL Verification
- [ ] ✅ zh-TW DLL 存在
- [ ] ✅ en DLL 存在
- **備註 / Notes**: 

### 3. 診斷頁面測試 / Diagnostic Page Test
- [ ] ✅ 頁面載入成功
- [ ] ✅ SharedResources 100% 成功
- [ ] ✅ NavMenu 100% 成功
- **備註 / Notes**: 

### 4. 語系切換測試 / Language Switching Test
- [ ] ✅ 切換至英文成功
- [ ] ✅ 切換至繁體中文成功
- [ ] ✅ 翻譯正確顯示
- **備註 / Notes**: 

### 5. Cookie 持久化測試 / Cookie Persistence Test
- [ ] ✅ 重新整理後語系保持
- [ ] ✅ Cookie 正確設定
- **備註 / Notes**: 

### 6. 導覽選單測試 / Navigation Menu Test
- [ ] ✅ 繁體中文翻譯正確
- [ ] ✅ 英文翻譯正確
- **備註 / Notes**: 

## 總結 / Summary

**整體狀態 / Overall Status**: ✅ 通過 / ❌ 失敗
**發現問題 / Issues Found**: 
**建議 / Recommendations**: 
```

---

## 🎉 完成檢查清單 / Completion Checklist

全部打勾才算完成 / All must be checked to complete:

- [ ] 建置成功（0 錯誤）
- [ ] 資源 DLL 已生成
- [ ] 診斷頁面測試通過
- [ ] 語系切換正常運作
- [ ] Cookie 持久化正常
- [ ] 繁體中文翻譯正確
- [ ] 英文翻譯正確
- [ ] 導覽選單翻譯正確
- [ ] 所有頁面翻譯正確
- [ ] 已填寫測試報告

---

**Happy Testing! 測試愉快！** 🎊
