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
            if (!string.IsNullOrEmpty(culture))
            {
                // Set the culture cookie
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddYears(1),
                        IsEssential = true,
                        Path = "/"
                    }
                );
            }

            // Determine redirect URL
            string redirectUrl = "/";

            if (!string.IsNullOrEmpty(redirectUri) && Url.IsLocalUrl(redirectUri))
            {
                redirectUrl = redirectUri;
            }
            else if (Request.Headers.ContainsKey("Referer"))
            {
                var referer = Request.Headers["Referer"].ToString();
                if (!string.IsNullOrEmpty(referer) && Uri.TryCreate(referer, UriKind.Absolute, out var refererUri))
                {
                    redirectUrl = refererUri.PathAndQuery;
                }
            }

            return LocalRedirect(redirectUrl);
        }
    }
}
