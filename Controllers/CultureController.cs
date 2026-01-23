using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace DxBlazorApplication7.Controllers
{
    [Route("[controller]/[action]")]
    public class CultureController : Controller
    {
        /// <summary>
        /// Sets the culture via cookie and redirects to the specified URI
        /// </summary>
        /// <param name="culture">Culture code (e.g., zh-TW, zh-CN, en-US)</param>
        /// <param name="redirectUri">Relative URI to redirect to after setting culture</param>
        [HttpGet]
        public IActionResult Set(string culture, string? redirectUri)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions 
                    { 
                        Expires = DateTimeOffset.UtcNow.AddYears(1),
                        IsEssential = true,
                        Path = "/",
                        Secure = true,
                        HttpOnly = true,
                        SameSite = SameSiteMode.Lax
                    }
                );
            }

            // Security check: only allow local redirects
            if (!string.IsNullOrEmpty(redirectUri) && Url.IsLocalUrl(redirectUri))
            {
                return LocalRedirect(redirectUri);
            }

            // Fallback to Referer or root
            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer) && Uri.TryCreate(referer, UriKind.Absolute, out var refererUri))
            {
                // Only redirect to same host
                if (refererUri.Host == Request.Host.Host)
                {
                    return Redirect(referer);
                }
            }

            return LocalRedirect("/");
        }
    }
}
