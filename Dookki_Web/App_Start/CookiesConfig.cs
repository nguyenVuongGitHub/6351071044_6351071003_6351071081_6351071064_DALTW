using Dookki_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Mvc;

namespace Dookki_Web.App_Start
{
    public class CookiesConfig
    {
        public const int DefaultExpirationDays = 7;
        private static readonly List<string> CookieLogs = new List<string>();
        private static readonly JavaScriptSerializer JsonSerializer = new JavaScriptSerializer();

        /// <summary>
        /// Sets a cookie for an object by serializing it to JSON.
        /// </summary>
        public static void SetCookie(HttpResponseBase response, string name, object value, int? daysToExpire = null)
        {
            string serializedValue = JsonSerializer.Serialize(value);
            HttpCookie cookie = new HttpCookie(name, serializedValue)
            {
                Expires = DateTime.Now.AddDays(daysToExpire ?? DefaultExpirationDays),
                HttpOnly = true,  // Prevent JavaScript access
                Secure = true,    // Use HTTPS only
                SameSite = SameSiteMode.Strict // Restrict cross-site usage
            };

            response.Cookies.Add(cookie);
            LogCookieActivity($"Set Cookie: Name={name}, Value={serializedValue}");
        }

        /// <summary>
        /// Sets a cookie for an object by serializing it to JSON.
        /// </summary>
        public static void SetACCOUNTCookie(HttpResponseBase response, string name, ACCOUNT value, int? daysToExpire = null)
        {
            var tmp = new JSONConvertAccount
            {
                ID = value.ID,
                UserName = value.UserName,
                Password = value.Password,
                Role = value.Role
            };
            string serializedValue = JsonSerializer.Serialize(tmp);
            HttpCookie cookie = new HttpCookie(name, serializedValue)
            {
                Expires = DateTime.Now.AddDays(daysToExpire ?? DefaultExpirationDays),
                HttpOnly = true,  // Prevent JavaScript access
                Secure = true,    // Use HTTPS only
                SameSite = SameSiteMode.Strict // Restrict cross-site usage
            };

            response.Cookies.Add(cookie);
            LogCookieActivity($"Set Cookie: Name={name}, Value={serializedValue}");
        }

        /// <summary>
        /// Retrieves a cookie and deserializes it into an object.
        /// </summary>
        public static T GetCookie<T>(HttpRequestBase request, string name) where T : class
        {
            HttpCookie cookie = request.Cookies[name];
            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                try
                {
                    T value = JsonSerializer.Deserialize<T>(cookie.Value);
                    LogCookieActivity($"Get Cookie: Name={name}, Value={cookie.Value}");
                    return value;
                }
                catch (Exception ex)
                {
                    LogCookieActivity($"Error deserializing cookie: Name={name}, Error={ex.Message}");
                }
            }
            else
            {
                LogCookieActivity($"Get Cookie: Name={name}, Not Found or Empty");
            }
            return null;
        }

        

        /// <summary>
        /// Deletes a cookie.
        /// </summary>
        public static void DeleteCookie(HttpResponseBase response, string name)
        {
            HttpCookie cookie = new HttpCookie(name)
            {
                Expires = DateTime.Now.AddDays(-1) // Expire the cookie immediately
            };
            response.Cookies.Add(cookie);

            LogCookieActivity($"Delete Cookie: Name={name}");
        }

        /// <summary>
        /// Logs cookie activities.
        /// </summary>
        private static void LogCookieActivity(string message)
        {
            CookieLogs.Add($"{DateTime.Now}: {message}");
        }

        /// <summary>
        /// Retrieves all logged cookie activities.
        /// </summary>
        public static IEnumerable<string> GetUserCookieLogs()
        {
            return CookieLogs;
        }

        ///// OTHER FUNCTION ////

        /// <summary>
        /// Take account admin
        /// </summary>
        /// <returns>ACCOUNT: Object</returns>
        public static ACCOUNT GetAdmin(HttpRequestBase req)
        {
            return GetCookie<ACCOUNT>(req, "admin");
        }

        /// <summary>
        /// Use LINQ to take Admin's name
        /// </summary>
        /// <returns>Admin's name: string</returns>
        public static string GetAdminNameByAccountID(int ID)
        {
            using (var context = new DOOKKIEntities())
            {
                // Sử dụng LINQ để truy vấn
                var name = context.Admins
                    .Where(admin => admin.IDAccount == ID) // Điều kiện WHERE
                    .Select(admin => admin.Name) // Chọn cột Name
                    .FirstOrDefault(); // Lấy giá trị đầu tiên (hoặc null nếu không có)
                return name;
            }
        }

        /// <summary>
        /// Take account admin
        /// </summary>
        /// <returns>Admin's name: string</returns>
        public static string GetAdminName(HttpRequestBase req)
        {
            var admin = GetAdmin(req);
            if (admin != null)
            {
                // Gọi phương thức LINQ từ UserRepository
                return GetAdminNameByAccountID(admin.ID);
            }
            return null;
        }

        /// <summary>
        /// Take account user
        /// </summary>
        /// <returns>ACCOUNT: Object</returns>
        public static ACCOUNT GetUser(HttpRequestBase req)
        {
            return GetCookie<ACCOUNT>(req, "user");
        }

        /// <summary>
        /// Take account user
        /// </summary>
        /// <returns>User's name: string</returns>
        public static string GetUserNameByAccountID(int ID)
        {
            using (var context = new DOOKKIEntities())
            {
                // Sử dụng LINQ để truy vấn
                var name = context.Customers
                    .Where(user => user.IDAccount == ID) // Điều kiện WHERE
                    .Select(user => user.Name) // Chọn cột Name
                    .FirstOrDefault(); // Lấy giá trị đầu tiên (hoặc null nếu không có)
                return name;
            }
        }

        /// <summary>
        /// Take account user
        /// </summary>
        /// <returns>User's name: string</returns>
        public static string GetUserName(HttpRequestBase req)
        {
            var user = GetUser(req);
            if (user != null)
            {
                // Gọi phương thức LINQ từ UserRepository
                return GetUserNameByAccountID(user.ID);
            }
            return null;
        }
    }
}