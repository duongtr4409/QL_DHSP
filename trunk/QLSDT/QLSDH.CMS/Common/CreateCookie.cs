using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEMIS.CMS.Common
{
    public class CreateCookie
    {
        /// <summary>
        /// Stores a value in a user Cookie, creating it if it doesn't exists yet.
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        /// <param name="cookieDomain">Cookie domain (or NULL to use default domain value)</param>
        /// <param name="keyName">Cookie key name (if the cookie is a keyvalue pair): if NULL or EMPTY, the cookie will be treated as a single variable.</param>
        /// <param name="value">Value to store into the cookie</param>
        /// <param name="expirationDate">Expiration Date (set it to NULL to leave default expiration date)</param>
        /// <param name="httpOnly">set it to TRUE to enable HttpOnly, FALSE otherwise (default: false)</param>
        /// <param name="sameSite">set it to 'None', 'Lax', 'Strict' or '(-1)' to not add it (default: '(-1)').</param>
        /// <param name="secure">set it to TRUE to enable Secure (HTTPS only), FALSE otherwise</param>
        public static void StoreInCookie(
            string cookieName,
            string cookieDomain,
            string keyName,
            string value,
            DateTime? expirationDate,
            bool httpOnly = false,
            //SameSiteMode sameSite = (SameSiteMode)(-1),
            bool secure = false)
        {
            // NOTE: we have to look first in the response, and then in the request.
            // This is required when we update multiple keys inside the cookie.
            HttpCookie cookie = HttpContext.Current.Response.Cookies.AllKeys.Contains(cookieName)
                ? HttpContext.Current.Response.Cookies[cookieName]
                : HttpContext.Current.Request.Cookies[cookieName];

            if (cookie == null) cookie = new HttpCookie(cookieName);
            if (!String.IsNullOrEmpty(keyName)) cookie.Values.Set(keyName, value);
            else cookie.Value = value;
            if (expirationDate.HasValue) cookie.Expires = expirationDate.Value;
            if (!String.IsNullOrEmpty(cookieDomain)) cookie.Domain = cookieDomain;
            if (httpOnly) cookie.HttpOnly = true;
            cookie.Secure = secure;
            //cookie.SameSite = sameSite;
            HttpContext.Current.Response.Cookies.Set(cookie);
        }
    }
}