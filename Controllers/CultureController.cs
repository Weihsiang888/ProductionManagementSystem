using DxBlazorApplication7.Configuration;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace DxBlazorApplication7.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CultureController : ControllerBase
    {
        /// <summary>
        /// Sets the culture cookie and redirects to the specified URI
        /// </summary>
        /// <param name="culture">Culture code (e.g., zh-TW, en-US, zh-CN)</param>
        /// <param name="redirectUri">Relative path to redirect to after setting culture</param>
        /// <returns>Redirect result</returns>
        [HttpGet("Set")]
        public IActionResult SetCulture(string culture, string? redirectUri)
        {
            // Validate culture against supported list using shared configuration
            if (CultureConfiguration.IsCultureSupported(culture))
            {
                // Set the culture cookie with security attributes
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddYears(1),
                        IsEssential = true,
                        Path = "/",
                        SameSite = SameSiteMode.Lax,
                        Secure = Request.IsHttps, // Only set Secure flag on HTTPS
                        HttpOnly = true // Prevent JavaScript access to protect against XSS
                    }
                );
            }

            // Determine safe redirect URL
            string redirectUrl = GetSafeRedirectUrl(redirectUri);
            return LocalRedirect(redirectUrl);
        }

        /// <summary>
        /// Validates and returns a safe redirect URL
        /// </summary>
        /// <param name="redirectUri">The requested redirect URI</param>
        /// <returns>A validated safe redirect URL</returns>
        private string GetSafeRedirectUrl(string? redirectUri)
        {
            // Try to use the provided redirect URI if valid
            if (!string.IsNullOrEmpty(redirectUri) && IsValidLocalUrl(redirectUri))
            {
                return redirectUri;
            }

            // Try to use referer if valid
            if (Request.Headers.ContainsKey("Referer"))
            {
                var referer = Request.Headers["Referer"].ToString();
                if (!string.IsNullOrEmpty(referer) && 
                    Uri.TryCreate(referer, UriKind.Absolute, out var refererUri) &&
                    string.Equals(refererUri.Host, Request.Host.Host, StringComparison.OrdinalIgnoreCase))
                {
                    return refererUri.PathAndQuery;
                }
            }

            // Default to root
            return "/";
        }

        /// <summary>
        /// Validates that a URL is safe for local redirect
        /// </summary>
        /// <param name="url">URL to validate</param>
        /// <returns>True if URL is safe for redirect, false otherwise</returns>
        private bool IsValidLocalUrl(string url)
        {
            // Parse and validate the redirect URI structure
            if (!Uri.TryCreate(url, UriKind.Relative, out _))
            {
                return false;
            }

            // Check basic requirements
            if (!Url.IsLocalUrl(url) || !url.StartsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // Ensure no protocol-relative URLs or suspicious patterns
            if (url.StartsWith("//", StringComparison.OrdinalIgnoreCase) ||
                url.Contains("://", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
    }
}
