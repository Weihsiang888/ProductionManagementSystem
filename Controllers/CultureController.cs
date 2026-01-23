using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace DxBlazorApplication7.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CultureController : ControllerBase
    {
        // Supported cultures for validation
        private static readonly HashSet<string> SupportedCultures = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "zh-TW",
            "en-US",
            "zh-CN"
        };

        /// <summary>
        /// Sets the culture cookie and redirects to the specified URI
        /// </summary>
        /// <param name="culture">Culture code (e.g., zh-TW, en-US, zh-CN)</param>
        /// <param name="redirectUri">Relative path to redirect to after setting culture</param>
        /// <returns>Redirect result</returns>
        [HttpGet("Set")]
        public IActionResult SetCulture(string culture, string? redirectUri)
        {
            // Validate culture against supported list
            if (!string.IsNullOrEmpty(culture) && SupportedCultures.Contains(culture))
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

            // Determine redirect URL with additional validation
            string redirectUrl = "/";

            if (!string.IsNullOrEmpty(redirectUri))
            {
                // Validate that URL is local and doesn't contain suspicious patterns
                if (Url.IsLocalUrl(redirectUri) && 
                    !redirectUri.Contains("//", StringComparison.OrdinalIgnoreCase) &&
                    redirectUri.StartsWith("/", StringComparison.OrdinalIgnoreCase))
                {
                    redirectUrl = redirectUri;
                }
            }
            else if (Request.Headers.ContainsKey("Referer"))
            {
                var referer = Request.Headers["Referer"].ToString();
                if (!string.IsNullOrEmpty(referer) && Uri.TryCreate(referer, UriKind.Absolute, out var refererUri))
                {
                    // Only use referer if it's from the same host
                    if (string.Equals(refererUri.Host, Request.Host.Host, StringComparison.OrdinalIgnoreCase))
                    {
                        redirectUrl = refererUri.PathAndQuery;
                    }
                }
            }

            return LocalRedirect(redirectUrl);
        }
    }
}
