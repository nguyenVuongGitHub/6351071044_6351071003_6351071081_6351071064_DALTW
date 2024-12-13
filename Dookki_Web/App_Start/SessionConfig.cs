using Dookki_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dookki_Web.App_Start
{
    public static class SessionConfig
    {
        //1. Luu session cho user
        public static void  SetAdmin(ACCOUNT admin)
        {
            HttpContext.Current.Session["admin"] = admin;
        }
        //2. Lay session
        public static ACCOUNT GetAdmin()
        {
            return (ACCOUNT)HttpContext.Current.Session["admin"];
        }
        public static void SetUser(ACCOUNT user)
        {
            HttpContext.Current.Session["user"] = user;
        }
        public static ACCOUNT GetUser()
        {
            return (ACCOUNT)HttpContext.Current.Session["user"];
        }
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
        public static string GetAdminName()
        {
            var admin = GetAdmin();
            if (admin != null)
            {
                // Gọi phương thức LINQ từ UserRepository
                return GetAdminNameByAccountID(admin.ID);
            }
            return null;
        }
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
        public static string GetUserName()
        {
            var user = GetUser();
            if (user != null)
            {
                // Gọi phương thức LINQ từ UserRepository
                return GetUserNameByAccountID(user.ID);
            }
            return null;
        }
    }
}