using System.Globalization;

namespace DxBlazorApplication7.Configuration
{
    /// <summary>
    /// Configuration for supported cultures in the application
    /// </summary>
    public static class CultureConfiguration
    {
        /// <summary>
        /// List of supported culture names
        /// </summary>
        public static readonly string[] SupportedCultureNames = new[]
        {
            "zh-TW",  // Traditional Chinese
            "en-US",  // English (US)
            "zh-CN"   // Simplified Chinese
        };

        /// <summary>
        /// Default culture for the application
        /// </summary>
        public const string DefaultCultureName = "zh-TW";

        /// <summary>
        /// Gets the array of supported CultureInfo objects
        /// </summary>
        public static CultureInfo[] GetSupportedCultures()
        {
            return SupportedCultureNames
                .Select(name => new CultureInfo(name))
                .ToArray();
        }

        /// <summary>
        /// Validates if a culture is supported
        /// </summary>
        /// <param name="cultureName">Culture name to validate</param>
        /// <returns>True if culture is supported, false otherwise</returns>
        public static bool IsCultureSupported(string cultureName)
        {
            return !string.IsNullOrEmpty(cultureName) && 
                   SupportedCultureNames.Contains(cultureName, StringComparer.OrdinalIgnoreCase);
        }
    }
}
