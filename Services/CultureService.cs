using System.Globalization;

namespace DxBlazorApplication7.Services
{
    public class CultureService
    {
        public event EventHandler? CultureChanged;

        public CultureInfo CurrentCulture { get; private set; } = new CultureInfo("zh-TW");

        public void SetCulture(string culture)
        {
            if (CurrentCulture.Name != culture)
            {
                CurrentCulture = new CultureInfo(culture);
                CultureInfo.CurrentCulture = CurrentCulture;
                CultureInfo.CurrentUICulture = CurrentCulture;
                CultureChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public List<CultureInfo> SupportedCultures { get; } = new()
        {
            new CultureInfo("zh-TW"),
            new CultureInfo("en")
        };
    }
}
