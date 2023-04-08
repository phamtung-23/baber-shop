using BaberShop1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BaberShop1.Areas.Admin.Controllers
{
    
    public class HomeController : Controller
    {
        BARBERSHOPEntities _db = new BARBERSHOPEntities();
        // GET: Admin/Home
        public ActionResult User()
        {
            var v = from t in _db.ACCOUNT_USER
                    select t;
            return PartialView(v.ToList());
        }

        public ActionResult Index()
        {
            var NumberUser = from t in _db.ACCOUNT_USER
                    select t;
            ViewBag.NumberUser = NumberUser.Count();

            var NumberService = from t in _db.SERVICE_SHOP
                             select t;
            ViewBag.NumberService = NumberService.Count();

            var NumberBooking = from t in _db.BOOKINGs
                                select t;
            ViewBag.NumberBooking = NumberBooking.Count();

            var NumberNhanVien = from t in _db.NHANVIENs
                                select t;
            ViewBag.NumberNV = NumberNhanVien.Count();

            var NumberMenu = from t in _db.MENUs
                                 select t;
            ViewBag.NumberMenu = NumberMenu.Count();
            return PartialView();
        }
        public ActionResult Service()
        {
            var v = from t in _db.SERVICE_SHOP
                    select t;
            return PartialView(v.ToList());
        }
        public ActionResult Booking()
        {
            var v = from t in _db.BOOKINGs
                    select t;
            return PartialView(v.ToList());
        }
        public ActionResult Menu()
        {
            var v = from t in _db.MENUs
                    select t;
            return PartialView(v.ToList());
        }
        public ActionResult Employee()
        {
            var v = from t in _db.NHANVIENs
                    select t;
            return PartialView(v.ToList());
        }

        public ActionResult AddUser()
        {

            return PartialView();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(ACCOUNT_USER accountUser)
        {

            if (ModelState.IsValid)
            {
                var check = _db.ACCOUNT_USER.FirstOrDefault(s => s.USERNAME == accountUser.USERNAME);
                if (accountUser.USERNAME == null || accountUser.PASSWORD_USER == null)
                {
                    ViewBag.error = "Vui lòng điền đầy đủ thông tin!!";
                    return View();
                }
                if (check == null)
                {
                    accountUser.PASSWORD_USER = GetMD5(accountUser.PASSWORD_USER);
                    _db.Configuration.ValidateOnSaveEnabled = false;
                    _db.ACCOUNT_USER.Add(accountUser);
                    _db.SaveChanges();
                    return RedirectToAction("/User");
                }
                else
                {

                    ViewBag.error = "Username đã tồn tại!!!";

                    return View();
                }


            }
            return View();
        }

        public ActionResult EditUser(int id)
        {
            var CurrentUser = _db.ACCOUNT_USER.Where(x => x.ID_USER == id).FirstOrDefault();
            return PartialView(CurrentUser);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(ACCOUNT_USER userEdit, int id)
        {
            
            var updateUser = _db.ACCOUNT_USER.Where(x => x.ID_USER == id).FirstOrDefault();
            if (updateUser != null)
            {
               
                updateUser.USERNAME = userEdit.USERNAME;
                updateUser.PASSWORD_USER = GetMD5(userEdit.PASSWORD_USER);
                updateUser.STATUS_ACCOUNT = userEdit.STATUS_ACCOUNT;
                updateUser.CHECK_EMPLOYEE = userEdit.CHECK_EMPLOYEE;
                _db.SaveChanges();
                return RedirectToAction("/User");
            }

            return PartialView();

        }

        public JsonResult DeleteUser(int id)
        {
            Boolean result = false;
            var obj = _db.ACCOUNT_USER.Find(id);
            if (obj != null)
            {
                _db.ACCOUNT_USER.Remove(obj);
                _db.SaveChanges();
                result = true;
            }
            return Json(result);
        }
        public ActionResult AddService()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AddService(SERVICE_SHOP service_shop, HttpPostedFileBase img)
        {
            var fileName = "";
            var path = "";

            if (ModelState.IsValid)
            {
                if(img != null)
                {
                    fileName = img.FileName;
                    path = Path.Combine(Server.MapPath("~/Content/Upload/img/Service"), fileName);
                    img.SaveAs(path);
                    service_shop.IMG = fileName;
                }
                else
                {
                    service_shop.IMG = "Logo.png";
                }
                var check = _db.SERVICE_SHOP.FirstOrDefault(s => s.ID_SERVICE == service_shop.ID_SERVICE);
                if (service_shop.NAME_SERVICE == null || service_shop.PRICE == null || service_shop.IMG == null || service_shop.DESCRIPSTION == null)
                {
                    ViewBag.error = "Vui lòng điền đầy đủ thông tin!!";
                    return View();
                }
                if (check == null)
                {

                    _db.Configuration.ValidateOnSaveEnabled = false;
                    _db.SERVICE_SHOP.Add(service_shop);
                    _db.SaveChanges();
                    return RedirectToAction("/Service");
                }
                else
                {

                    ViewBag.error = "Dịch vụ đã tồn tại!!!";
                    return View();
                }


            }
            return View();
        }



        public ActionResult EditService(int id)
        {
            var CurrentService = _db.SERVICE_SHOP.Where(x => x.ID_SERVICE == id).FirstOrDefault();
            return PartialView(CurrentService);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditService(SERVICE_SHOP serviceEdit, int id, HttpPostedFileBase img)
        {
            var fileName = "";
            var path = "";
            var updateService = _db.SERVICE_SHOP.Where(x => x.ID_SERVICE == id).FirstOrDefault();

            if (updateService != null)
            {
                if (img != null)
                {
                    fileName = img.FileName;
                    path = Path.Combine(Server.MapPath("~/Content/Upload/img/Service"), fileName);
                    img.SaveAs(path);

                    updateService.NAME_SERVICE = serviceEdit.NAME_SERVICE;
                    updateService.PRICE = (serviceEdit.PRICE);
                    updateService.IMG = fileName;
                    updateService.DESCRIPSTION = serviceEdit.DESCRIPSTION;
                    updateService.STATUS_SERVICE = serviceEdit.STATUS_SERVICE;
                    _db.SaveChanges();
                    return RedirectToAction("/Service");
                }
                else
                {
                    updateService.NAME_SERVICE = serviceEdit.NAME_SERVICE;
                    updateService.PRICE = (serviceEdit.PRICE);
                    updateService.DESCRIPSTION = serviceEdit.DESCRIPSTION;
                    updateService.STATUS_SERVICE = serviceEdit.STATUS_SERVICE;
                    _db.SaveChanges();
                    return RedirectToAction("/Service");
                }
                
            }

            return PartialView();

        }

        public JsonResult DeleteService(int id)
        {
            Boolean result = false;
            var obj = _db.SERVICE_SHOP.Find(id);
            if (obj != null)
            {
                _db.SERVICE_SHOP.Remove(obj);
                _db.SaveChanges();
                result = true;
            }
            return Json(result);
        }


        public ActionResult AddBooking()
        {
            var v = from t in _db.ACCOUNT_USER
                    select t;
            ViewBag.listIDUser = new SelectList(v,"ID_USER","USERNAME");

           

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AddBooking(BOOKING book)
        {
            if (ModelState.IsValid)
            {
                
                    if (book.ID_USER == null || book.NAME_BOOK == null || book.PHONE_BOOK == null || book.TIME_BOOKING == null || book.DATE_BOOKING == null)
                    {
                        Response.Write("<script>alert('Data inserted successfully')</script>");
                        return View();
                    }
                    else
                    {
                        _db.Configuration.ValidateOnSaveEnabled = false;
                        book.TRANGTHAI = 0;
                        _db.BOOKINGs.Add(book);
                        _db.SaveChanges();
                        return RedirectToAction("/Booking");
                    }
                
            }
            return View();
        }

        public ActionResult EditBooking(int id)
        {

            var CurrentBook = _db.BOOKINGs.Where(x => x.ID_BOOKING == id).FirstOrDefault();
            var v = from t in _db.ACCOUNT_USER
                    select t;
            DateTimeFormatInfo myDateTimeFormat = new DateTimeFormatInfo();
            myDateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            ViewBag.date = CurrentBook.DATE_BOOKING.Value.GetDateTimeFormats('u')[0].Substring(0, 10);





            List<SelectListItem> status = new List<SelectListItem>();
            status.Add(new SelectListItem { Text = "success", Value = "0", Selected = true });
            status.Add(new SelectListItem { Text = "waiting", Value = "1" });
            status.Add(new SelectListItem { Text = "cancel", Value = "2" });
            ViewBag.Status = status;

            return PartialView(CurrentBook);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditBooking(BOOKING booking, int id)
        {
            var dateUpdate = Request.Form["DateBooking"];
            var updateBooking = _db.BOOKINGs.Where(x => x.ID_BOOKING == id).FirstOrDefault();
            DateTime myDate = Convert.ToDateTime(dateUpdate);
            if (updateBooking != null)
            {
                updateBooking.NAME_BOOK = booking.NAME_BOOK;
                updateBooking.PHONE_BOOK = (booking.PHONE_BOOK);
                updateBooking.DATE_BOOKING = myDate;
                updateBooking.TIME_BOOKING = booking.TIME_BOOKING;
                updateBooking.COMMENT = booking.COMMENT;
                updateBooking.TRANGTHAI = booking.TRANGTHAI;
                _db.SaveChanges();
                return RedirectToAction("/Booking");
    
                
            }
            return View();
        }
        public ActionResult AddEmployee()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEmployee(NHANVIEN nhanvien)
        {

            if (ModelState.IsValid)
            {

                if (nhanvien.NAME_NHANVIEN == null || nhanvien.SEX == null || nhanvien.SKILL == null || nhanvien.AVT == null)
                {
                    ViewBag.error = "Vui lòng điền đầy đủ thông tin!!";
                    return View();
                }
                else
                {

                    _db.NHANVIENs.Add(nhanvien);
                    _db.SaveChanges();
                    return RedirectToAction("/Employee");
                }



            }
            return View();
        }



        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
    }
}