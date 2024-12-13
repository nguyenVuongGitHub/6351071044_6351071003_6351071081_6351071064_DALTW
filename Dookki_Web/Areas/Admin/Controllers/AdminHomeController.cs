using Dookki_Web.App_Start;
using Dookki_Web.Models;
using Dookki_Web.Models.Map;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dookki_Web.Areas.Admin.Controllers
{
    public class AdminHomeController : Controller
    {
        DOOKKIEntities db = new DOOKKIEntities();
        // GET: Admin/HomeAdmin
        [HttpPost]
        public ActionResult UpdateStatus(bool IsFull)
        {
            // Store the checkbox state in the database or session
            Session["TableStatus"] = IsFull;  // Example with session
                                              // Redirect back to the same view to avoid resubmission on refresh
            return RedirectToAction("Index");
        }
        [RoleAdmin]
        [HttpGet]
        public ActionResult Index(string searchTerm, int? year)
        {

            // Retrieve the checkbox state from the session
            bool isFull = (Session["TableStatus"] != null) ? (bool)Session["TableStatus"] : false;

            // Pass the value to the view using ViewBag
            ViewBag.IsFull = isFull;

            // caculate year
            // Generate a list of years (e.g., from 2000 to the current year)
            int currentYear = DateTime.Now.Year;
            List<int> years = new List<int>();
            for (int i = currentYear; i >= 2018; i--) // Adjust the range as needed
            {
                years.Add(i);
            }

            // Pass the list to the view
            ViewBag.Years = new SelectList(years);

            // Default selected year
            ViewBag.Year = year ?? currentYear;

            // pie chart
            double[] chartPieData = GetChartPieData(year ?? currentYear);
            string[] chartPieLable = { "Khách không có tài khoản", "Khách có tài khoản" };


            // line chart
            double[] chartLineData = ProfitByYear(year ?? currentYear);
            string[] chartLineLable = { "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4",
                "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8",
                "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12", };

            var today = DateTime.Now.Date;  // Only day/month/year
            var ordersPending = db.Orders
                .Where(o => o.Status == "Pending")
                .ToList();
            ViewBag.Today = today.ToString("dd/MM/yyyy"); 
            ViewBag.OrdersPending = ordersPending;


            // Truyền dữ liệu qua ViewBag hoặc Model
            ViewBag.ChartPieData = chartPieData;
            ViewBag.ChartPieLable = chartPieLable;

            ViewBag.ChartLineData = chartLineData;
            ViewBag.ChartLineLable = chartLineLable;

            var requests = db.BookingRequests.AsQueryable();
            // Nếu có từ khóa tìm kiếm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                requests = requests.Where(t => t.Name.ToLower().Contains(searchTerm.ToLower())
                || t.Phone.Contains(searchTerm));
            }
            return View(requests.ToList());
        }

        [RoleAdmin]
        public ActionResult Statistical(int? year)
        {
            // caculate year
            // Generate a list of years (e.g., from 2000 to the current year)
            int currentYear = DateTime.Now.Year;
            List<int> years = new List<int>();
            for (int i = currentYear; i >= 2018; i--) // Adjust the range as needed
            {
                years.Add(i);
            }

            // Pass the list to the view
            ViewBag.Years = new SelectList(years);

            // Default selected year
            ViewBag.Year = year ?? currentYear;

            // pie chart
            double[] chartPieData = GetChartPieData(year ?? currentYear);
            string[] chartPieLable = { "Khách không có tài khoản", "Khách có tài khoản" };


            // line chart
            double[] chartLineData = ProfitByYear(year ?? currentYear);
            string[] chartLineLable = { "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4",
                "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8",
                "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12", };




            // Truyền dữ liệu qua ViewBag hoặc Model
            ViewBag.ChartPieData = chartPieData;
            ViewBag.ChartPieLable = chartPieLable;

            ViewBag.ChartLineData = chartLineData;
            ViewBag.ChartLineLable = chartLineLable;
            return View();
        }



        private double[] GetChartPieData(int? year)
        {
            int totalBill = db.Orders.Count();
            int customerHasAccount = 0;


            foreach (var order in db.Orders.Include(o=>o.Customer))
            {
                if (order.OrderDetails.Any() && order.OrderDetails.ElementAt(0).Payment.day.Year == year && order.Status == "Finish")
                {
                    if (order.Customer.IDAccount != 0)
                    {
                        customerHasAccount++;
                    }
                }

            }

            int customerHasNotAccount = totalBill - customerHasAccount;


            double percentHasNotAccount = Math.Round((double)customerHasNotAccount / totalBill * 100, 2);
            double percentHasAccount = Math.Round((double)customerHasAccount / totalBill * 100, 2);


            double[] chartData = new double[] { percentHasNotAccount, percentHasAccount };

            return chartData;
        }
        private double[] ProfitByYear(int? year)
        {

            // Sample data — replace with real data from the database
            double[] monthlyRevenues = new double[12];
            foreach (var order in db.Orders)
            {

                if (order.OrderDetails.Any() && order.OrderDetails.ElementAt(0).Payment.day.Year == year && order.Status == "Finish")
                {
                    decimal totalBill = 0;
                    foreach (var orderDetail in order.OrderDetails)
                    {
                        totalBill += orderDetail.quantily * orderDetail.Ticket.Price;
                    }
                    // Determine the month (0-based index)
                    int month = order.OrderDetails.ElementAt(0).Payment.day.Month - 1;

                    // Accumulate revenue for the corresponding month
                    monthlyRevenues[month] += (double)totalBill;
                }
            }


            double totalRevenue = monthlyRevenues.Sum();


            // Format the total revenue in VND (Vietnamese Dong) format
            //string formattedRevenue = totalRevenue.ToString("N0", new System.Globalization.CultureInfo("vi-VN"));
            ViewBag.TotalRevenue = totalRevenue.ToString("N0", new System.Globalization.CultureInfo("vi-VN")); ;

            return monthlyRevenues;
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            mapAccount map = new mapAccount();
            var admin = map.find(username, password,"admin");
            
            //1. Co: sang trang dashboard admin
            if (admin != null)
            {
                //SessionConfig.SetAdmin(admin);
                CookiesConfig.SetACCOUNTCookie(Response, "admin", admin);
                return RedirectToAction("Index");
            }

            //2. ko co: Quay lai trang login, bao loi
            ViewBag.error = "Tên đăng nhập hoặc mật khẩu không đúng";
            return View();
        }
        public ActionResult Logout()
        {
            //SessionConfig.SetAdmin(null);
            CookiesConfig.DeleteCookie(Response, "admin");
            return RedirectToAction("Login");
        }
        [RoleAdmin]
        public ActionResult RequestDetailOrder(int id)
        {
            var order = db.Orders.FirstOrDefault(o => o.ID == id);
            ViewBag.Today = DateTime.Now.Date.ToString("dd/MM/yyyy");

            return View(order);
        }
        [RoleAdmin]
        public ActionResult RequestDetail(int id)
        {
            var booking = db.BookingRequests.FirstOrDefault(bk => bk.ID == id);
            return View(booking);
        }
        [RoleAdmin]
        public ActionResult AcceptRequest(int id)
        {
            var booking = db.BookingRequests.FirstOrDefault(bk => bk.ID == id);
            booking.Status = "Accepted";
            db.BookingRequests.AddOrUpdate(booking);
            db.SaveChanges();
            return View(booking);
        }
        [RoleAdmin]
        public ActionResult AcceptRequestOrder(int id)
        {
            var order = db.Orders.FirstOrDefault(bk => bk.ID == id);
            order.Status = "Accepted";
            db.Orders.AddOrUpdate(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [RoleAdmin]
        public ActionResult DeleteRequest(int id)
        {
            var booking = db.BookingRequests.FirstOrDefault(bk => bk.ID == id);
            booking.Status = "Deleted";
            db.BookingRequests.AddOrUpdate(booking);
            db.SaveChanges();

            return RedirectToAction("ListAcceptedRequest", "AdminHome");
        }
        [RoleAdmin]
        public ActionResult DeleteRequestOrder(int id)
        {
            var order = db.Orders.FirstOrDefault(bk => bk.ID == id);
            order.Status = "Deleted";
            db.Orders.AddOrUpdate(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [RoleAdmin]
        [HttpGet]
        public ActionResult ListAcceptedRequest(string searchTerm)
        {
            IQueryable<BookingRequest> bookingsQuery = db.BookingRequests.Where(bk => bk.Status == "Accepted");


            if (!string.IsNullOrEmpty(searchTerm))
            {
                bookingsQuery = bookingsQuery.Where(bk => bk.Name.ToLower().Contains(searchTerm.ToLower())
                                                        || bk.Phone.Contains(searchTerm));
            }


            var bookings = bookingsQuery.ToList();

            return View(bookings);

        }

        [HttpPost]
        public ActionResult ExportChartLineData(int? year)
        {
            // Đăng ký giấy phép dùng free, ko có dòng này thì code hiểu là mk dùng của thương mại (phải trả phí)
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Lấy dữ liệu từ hàm ProfitByYear
            double[] chartLineData = ProfitByYear(year ?? DateTime.Now.Year);
            string[] chartLineLable = {
                        "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4",
                        "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8",
                        "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"
                    };

            using(ExcelPackage package = new ExcelPackage())
            {
                // Tạo một worksheet
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Báo cáo doanh thu");

                // Thêm title
                string title = $"Doanh thu năm {year ?? DateTime.Now.Year}";
                worksheet.Cells[1, 1, 2, 3].Merge = true;
                worksheet.Cells[1, 1].Value = title;
                worksheet.Cells[1, 1].Style.Font.Size = 15;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Thêm tiêu đề 
                for(int i = 3; i < 16; i++)
                {
                    int j = 2;
                    worksheet.Cells[i, j, i, j + 1].Merge = true;
                }
                worksheet.Cells[3, 1].Value = "Tháng";
                worksheet.Cells[3, 2].Value = "Doanh thu (VND)";
                worksheet.Row(3).Style.Font.Bold = true;

                // Thêm dữ liệu
                int startRow = 4;
                for (int i = 0; i < chartLineData.Length; i++)
                {
                    worksheet.Cells[i + 4, 1].Value = chartLineLable[i]; // Tên tháng
                    worksheet.Cells[i + 4, 2].Value = chartLineData[i];  // Doanh thu
                }

                // Định dạng cột "Doanh thu"
                worksheet.Column(2).Style.Numberformat.Format = "#,##0"; // Định dạng số có dấu phẩy

                // TÍnh tổng doanh thu
                double totalRevenue = chartLineData.Sum();
                int totalRow = startRow + chartLineData.Length; // DÒng típ theo
                worksheet.Cells[totalRow, 2, totalRow, 3].Merge = true;

                worksheet.Cells[totalRow, 1].Value = "Tổng doanh thu:";
                worksheet.Cells[totalRow, 1].Style.Font.Bold = true;

                worksheet.Cells[totalRow, 2].Value = totalRevenue;
                worksheet.Cells[totalRow, 2].Style.Font.Bold = true;
                worksheet.Cells[totalRow, 2].Style.Numberformat.Format = "#,##0"; // Định dạng số có dấu phẩy

                // Định dạng tổng cộng (màu nền nhẹ)
                worksheet.Cells[totalRow, 1, totalRow, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[totalRow, 1, totalRow, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                // Auto-fit cột
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Trả file Excel về dưới dạng stream
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string fileName = $"BaoCaoDoanhThu_{year}.xlsx";
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                return File(stream, contentType, fileName);
            }
        }

        public ActionResult Delivery()
        {
            DateTime today = DateTime.Now.Date;

            // Stae = accepted
            var acceptedOrders = db.Orders
                .Where(o => (o.Status == "Accepted" || o.Status == "Delivering") && o.Time != null)
                .ToList();

            ViewBag.Today = today.ToString("dd/MM/yyyy");
            return View(acceptedOrders);
        }

        public ActionResult SetDelivering(int id)
        {
            var order = db.Orders.FirstOrDefault(o => o.ID == id);
            if(order != null)
            {
                order.Status = "Delivering";
                db.Orders.AddOrUpdate(order);
                db.SaveChanges();
            }
            return RedirectToAction("Delivery");
        }
        public ActionResult SetFinished(int id)
        {
            var order = db.Orders.FirstOrDefault(o => o.ID == id);
            if (order != null)
            {
                order.Status = "Finish";
                db.Orders.AddOrUpdate(order);
                db.SaveChanges();
            }
            return RedirectToAction("Delivery");
        }

    }
}