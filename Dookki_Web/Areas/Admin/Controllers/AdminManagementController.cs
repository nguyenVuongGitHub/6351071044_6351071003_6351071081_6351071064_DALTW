using Dookki_Web.App_Start;
using Dookki_Web.Models;
using Dookki_Web.Models.Map;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Dookki_Web.Areas.Admin.Controllers
{
    [RoleAdmin]
    public class AdminManagementController : Controller
    {
        private readonly DOOKKIEntities _context = new DOOKKIEntities();
        // GET: Admin/AdminManagement
        public ActionResult Index()
        {
            var accounts = new mapAccount().ListAccount()
                                           .OrderBy(account => account.Role) // Sorts by Role in ascending order
                                           .ToList(); // Converts to a list (if not already a list)
            return View(accounts);
        }


        [HttpGet]
        public ActionResult AddAdmin()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddAdmin(ACCOUNT account, string phone, string name)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // 1. Đặt Role mặc định là "admin"
                    account.Role = "admin";

                    // 2. Thêm ACCOUNT vào database
                    _context.ACCOUNTs.Add(account);
                    _context.SaveChanges(); // Lưu để lấy IDAccount cho bảng Admin

                    // 3. Tạo bản ghi cho bảng Admin
                    Dookki_Web.Models.Admin admin = new Dookki_Web.Models.Admin
                     {
                        IDAccount = account.ID, // ID vừa được tạo trong bảng ACCOUNT
                        Phone = phone,
                        Name = name
                    };
                    _context.Admins.Add(admin);

                    // 4. Lưu cả hai bảng
                    _context.SaveChanges();

                    // 5. Chuyển hướng sau khi thêm thành công
                    return RedirectToAction("Index"); // Redirect đến danh sách admin
                }
                catch (Exception ex)
                {
                    // Log lỗi nếu cần thiết
                    ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                }
            }

            return View(account);
        }

        [HttpGet]
        public ActionResult EditAdmin(int id)
        {
            // Tìm admin dựa vào IDAccount
            var admin = (from a in _context.Admins
                         join ac in _context.ACCOUNTs on a.IDAccount equals ac.ID
                         where a.IDAccount == id
                         select new AdminViewModel
                         {
                             IDAccount = ac.ID,
                             Username = ac.UserName,
                             Password = ac.Password,
                             Role = ac.Role,
                             Name = a.Name,
                             Phone = a.Phone
                         }).FirstOrDefault();

            if (admin == null)
            {
                Response.StatusCode = 404;
                return HttpNotFound("Không tìm thấy admin.");
            }

            return View(admin);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditAdmin(AdminViewModel model)
        {
            // Kiểm tra dữ liệu hợp lệ
            if (!ModelState.IsValid)
            {
                ViewBag.Thongbao = "Dữ liệu không hợp lệ.";
                return View(model);
            }

            // Tìm tài khoản và admin liên quan
            var account = _context.ACCOUNTs.SingleOrDefault(ac => ac.ID == model.IDAccount);
            var admin = _context.Admins.SingleOrDefault(ad => ad.IDAccount == model.IDAccount);

            if (account == null || admin == null)
            {
                Response.StatusCode = 404;
                return HttpNotFound("Không tìm thấy admin để chỉnh sửa.");
            }

            // Cập nhật thông tin tài khoản
            account.UserName = model.Username;
            account.Password = model.Password;

            // Vai trò luôn là "admin"
            account.Role = "admin";

            // Cập nhật thông tin admin
            admin.Name = model.Name;
            admin.Phone = model.Phone;

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

       [HttpGet]
        public ActionResult RemoveAdmin(int id)
        {
            // Lấy thông tin Admin cần xóa dựa trên IDAccount
            var admin = _context.Admins.FirstOrDefault(a => a.IDAccount == id);
            if (admin == null)
            {
                return HttpNotFound("Không tìm thấy admin cần xóa.");
            }

            return View(admin);
        }
        [HttpPost, ActionName("RemoveAdmin")]
        public ActionResult RemoveAdminConfirm(int id)
        {
            try
            {
                // Tìm Admin theo IDAccount
                var admin = _context.Admins.FirstOrDefault(a => a.IDAccount == id);
                if (admin != null)
                {
                    // Xóa Admin
                    _context.Admins.Remove(admin);
                }

                // Xóa tài khoản trong bảng ACCOUNT
                var account = _context.ACCOUNTs.FirstOrDefault(a => a.ID == id);
                if (account != null)
                {
                    _context.ACCOUNTs.Remove(account);
                }

                // Lưu thay đổi
                _context.SaveChanges();

                // Chuyển hướng về danh sách admin
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi xóa admin: " + ex.Message);
                return View();
            }
        }
    }
}