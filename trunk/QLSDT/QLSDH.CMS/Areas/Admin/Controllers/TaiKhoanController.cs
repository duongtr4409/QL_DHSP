using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TEMIS.CMS.Repository;
using TEMIS.CMS.Common;
using TEMIS.CMS.Areas.Admin.Models;
using TEMIS.Model;
using System.Net.Http;
using Newtonsoft.Json;
using TEMIS.CMS.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Net;
using CoreAPI.Entity;
using TEMIS.CMS.Common;
using System.Net.Mail;

namespace TEMIS.CMS.Areas.Admin.Controllers
{
    // [Roles(PublicConstant.ROLE_ADMIN, PublicConstant.ROLE_SUPPERADMIN)]
    [AuditAction]
    public class TaiKhoanController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public async Task<ActionResult> Index()
        {

            List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = listKhoa;

            List<OrganizationInfo> listDivisions = await CoreAPI.CoreAPI.GetListDivisions();
            ViewBag.ListDivisions = listDivisions;

            List<OrganizationInfo> listInstitues = await CoreAPI.CoreAPI.GetListInstitues();
            ViewBag.ListInstitues = listInstitues;

            var lstUser = await CoreAPI.CoreAPI.GetListTaiKhoan(0);
            return View(lstUser);
        }
        /// <summary>
        /// DS tai khoan giang vien ngoai truong
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Teachers()
        {

            List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = listKhoa;

            List<OrganizationInfo> listDivisions = await CoreAPI.CoreAPI.GetListDivisions();
            ViewBag.ListDivisions = listDivisions;

            List<OrganizationInfo> listInstitues = await CoreAPI.CoreAPI.GetListInstitues();
            ViewBag.ListInstitues = listInstitues;

            var lstUser = await CoreAPI.CoreAPI.GetListTaiKhoan(0);
            return View(lstUser);
        }
        /// <summary>
        /// DS tai khoan NCS
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> NCS()
        {
            List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = listKhoa;

            List<KhoaHoc> listKhoaHoc = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetAllRecords().ToList();
            ViewBag.ListKhoaHoc = listKhoaHoc;

            var lstUser = _unitOfWork.GetRepositoryInstance<NCS>().GetAllRecords();
            return View(lstUser);
        }


        /// <summary>
        /// Khoa tai khoan UserRoles
        /// </summary>
        /// <returns></returns>
        public JsonResult LockUserRoles(string username)
        {
            string mesage = "";
            try
            {
                UserRoles userrole = _unitOfWork.GetRepositoryInstance<UserRoles>().GetFirstOrDefaultByParameter(o => o.UserName == username);
                if (userrole != null)
                {
                    if (userrole.IsLock == true)
                    {
                        userrole.IsLock = false;
                    }
                    else
                    {
                        userrole.IsLock = true;
                    }
                    userrole.UpdatedAt = DateTime.Now;
                    var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                    userrole.UpdatedBy = user.Username;
                    _unitOfWork.GetRepositoryInstance<UserRoles>().Update(userrole);
                    _unitOfWork.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                mesage = "Lỗi thay đổi trạng thái: " + ex.Message;
            }

            return Json(mesage, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Khoa tai khoan User
        /// </summary>
        /// <returns></returns>
        public JsonResult LockUser(string username)
        {
            string mesage = "";
            try
            {
                User user = _unitOfWork.GetRepositoryInstance<User>().GetFirstOrDefaultByParameter(o => o.UserName == username);
                if (user != null)
                {
                    if (user.IsLock == true)
                    {
                        user.IsLock = false;
                    }
                    else
                    {
                        user.IsLock = true;
                    }
                    _unitOfWork.GetRepositoryInstance<User>().Update(user);
                    _unitOfWork.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                mesage = "Lỗi thay đổi trạng thái: " + ex.Message;
            }

            return Json(mesage, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> PhanQuyen(string userName)
        {
            if (userName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userRoles = _unitOfWork.GetRepositoryInstance<UserRoles>().GetFirstOrDefaultByParameter(x => x.UserName.Equals(userName));
            var editUser = new EditUserViewModel();
            editUser.userName = userName;
            editUser.RolesList = _unitOfWork.GetRepositoryInstance<Roles>().GetAllRecords().ToList().Select(x => new SelectListItem()
            {
                Selected = userRoles == null ? false : userRoles.Role.Contains(x.Name),
                Text = x.Name,
                Value = x.Name
            }); ;
            return View(editUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PhanQuyen(FormCollection form, params string[] selectedRole)
        {
            try
            {
                string userName = form.Get("userName") != null ? form.Get("userName") : string.Empty;
                if (!string.IsNullOrEmpty(userName))
                {
                    selectedRole = selectedRole ?? new string[] { };
                    var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                    var userRole = _unitOfWork.GetRepositoryInstance<UserRoles>().GetFirstOrDefaultByParameter(x => x.UserName.Equals(userName));
                    if (userRole != null)
                    {
                        userRole.Role = selectedRole[0];
                        userRole.UpdatedAt = DateTime.Now;
                        userRole.UpdatedBy = user.Username;
                        _unitOfWork.GetRepositoryInstance<UserRoles>().Update(userRole);
                        TempData["message"] = "Phân quyền thành công";
                    }
                    else
                    {
                        foreach (string r in selectedRole)
                        {
                            _unitOfWork.GetRepositoryInstance<UserRoles>().Add(new UserRoles() { Role = r, UserName = userName, CreatedAt = DateTime.Now, CreatedBy = user.Username, IsLock = false, UpdatedAt = DateTime.Now, UpdatedBy = user.Username });
                            TempData["message"] = "Phân quyền thành công";
                        }
                    }
                    _unitOfWork.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["error"] = "Phân quyền không thành công" + ex.Message;
                return View();
            }

        }

        public async Task<ActionResult> Roles()
        {
            return View();
        }

        public async Task<JsonResult> ChangePasss(long id, string password)
        {
            string mesage = "";
            try
            {
                User user = _unitOfWork.GetRepositoryInstance<User>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (user != null)
                {
                    TaiKhoan info = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                    user.PassWord = Utility.Encrypt(password, true);
                    user.UpdatedAt = DateTime.Now;
                    user.UpdatedBy = info.Username;
                    _unitOfWork.GetRepositoryInstance<User>().Update(user);
                    _unitOfWork.SaveChanges();
                    string title = "Đặt lại mật khẩu";
                    string content = "Mật khẩu mới của bạn là : " + password;
                    if (Utility.SendMail(user.Email, title, content).Equals(""))
                    {
                        TempData["message"] = "Gửi thành công";
                        return Json("OK", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        TempData["error"] = "Lỗi gửi mail!";
                        return Json(mesage, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    TempData["error"] = "Lỗi tài khoản";
                    return Json("Lỗi tài khoản", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                mesage = "Lỗi đặt lại mật khẩu: " + ex.Message;
                return Json(mesage, JsonRequestBehavior.AllowGet);
            }   
        }

        public async Task<JsonResult> ResetPassword(string username, string email)
        {
            string mesage = "";
            try
            {
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var stringChars = new char[6];
                var random = new Random();
                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }
                var password = new string(stringChars);

                string title = "Đặt lại mật khẩu";
                string content = "Mật khẩu mới của bạn là : " + password;
                if (Utility.SendMail(email, title, content).Equals(""))
                {
                    // lưu db
                    User user = _unitOfWork.GetRepositoryInstance<User>().GetFirstOrDefaultByParameter(o => o.UserName == username || o.Email == email);
                    if (user != null)
                    {
                        TaiKhoan info = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                        user.PassWord = Utility.Encrypt(password, true);
                        user.UpdatedAt = DateTime.Now;
                        user.UpdatedBy = info.Username;
                        _unitOfWork.GetRepositoryInstance<User>().Update(user);
                        _unitOfWork.SaveChanges();
                    }
                    TempData["message"] = "Gửi thành công";
                }
                else
                {
                    TempData["error"] = "Lỗi gửi mail!";
                }
            }
            catch (Exception ex)
            {
                mesage = "Lỗi đặt lại mật khẩu: " + ex.Message;
            }

            return Json(mesage, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ListUser(int departmentId)
        {
            try
            {
                List<TaiKhoan> listTaiKhoan = new List<TaiKhoan>();

                listTaiKhoan = await CoreAPI.CoreAPI.GetListTaiKhoan(departmentId);
                if (listTaiKhoan.Count == 0)
                {
                    TempData["message"] = "Không tìm thấy kết quả nào";
                    return PartialView("_PartialListUser", null);
                }
                foreach (TaiKhoan item in listTaiKhoan)
                {
                    var userRole = _unitOfWork.GetRepositoryInstance<UserRoles>().GetFirstOrDefaultByParameter(x => x.UserName.Equals(item.Username));
                    if (userRole != null)
                    {
                        item.RoleName = userRole.Role == null ? "" : userRole.Role;
                        item.isLock = userRole.IsLock == null ? false : (bool)(userRole.IsLock);
                    }
                    else
                    {
                        item.isLock = false;
                    }
                }

                return PartialView("_PartialListUser", listTaiKhoan);

            }
            catch (Exception ex)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(controllerName, actionName, user == null ? string.Empty : user.Username, ex);
                return PartialView("_PartialListUser", null);
            }

        }
        public async Task<ActionResult> ListTeacher(int departmentId)
        {
            try
            {
                List<TaiKhoan> listTaiKhoan = new List<TaiKhoan>();
                var lstGiangVien = departmentId != 0 ? _unitOfWork.GetRepositoryInstance<Model.GiangVien>().GetListByParameter(x => x.KhoaId == departmentId)
                    : _unitOfWork.GetRepositoryInstance<Model.GiangVien>().GetAllRecords();
                if (lstGiangVien.Count() == 0)
                {
                    TempData["message"] = "Không tìm thấy kết quả nào";
                    return PartialView("_PartialListUser2", null);
                }
                foreach (var item in lstGiangVien)
                {
                    var user = _unitOfWork.GetRepositoryInstance<User>().GetFirstOrDefaultByParameter(x => x.UserName == item.UserName);
                    if (user != null)
                    {
                        listTaiKhoan.Add(new TaiKhoan() { Username = item.UserName, Email = item.Email, Name = item.HoTen, isLock = user.IsLock.Value, Id = item.Id });
                    }
                }
                return PartialView("_PartialListUser2", listTaiKhoan);
            }
            catch (Exception ex)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(controllerName, actionName, user == null ? string.Empty : user.Username, ex);
                return PartialView("_PartialListUser2", null);
            }
        }
        public async Task<ActionResult> ListNCS(int departmentId)
        {
            try
            {
                List<TaiKhoan> listTaiKhoan = new List<TaiKhoan>();

                var lstNCS = departmentId != 0 ? _unitOfWork.GetRepositoryInstance<NCS>().GetListByParameter(x => x.KhoaId == departmentId).OrderBy(x=>x.Ma)
                    : _unitOfWork.GetRepositoryInstance<NCS>().GetAllRecords().OrderBy(x=>x.Ma);
                if (lstNCS.Count() != 0)
                {
                    foreach (var item in lstNCS)
                    {
                        var user = _unitOfWork.GetRepositoryInstance<User>().GetFirstOrDefaultByParameter(x => x.UserName == item.Ma);
                        if (user != null)
                        {
                            listTaiKhoan.Add(new TaiKhoan() { Username = item.Ma, Email = item.Email, Name = item.HoTen, isLock = user.IsLock.Value,Id = user.Id });
                        }
                    }
                }
               
                int chuyennganhid = 0;
                if (departmentId != 0)
                {
                    var chuyennganh = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetFirstOrDefaultByParameter(x => x.KhoaId == departmentId);
                    if (chuyennganh != null) chuyennganhid = chuyennganh.Id;
                }

                var lstNCS_chuaduyet = chuyennganhid != 0 ? _unitOfWork.GetRepositoryInstance<DangKyTuyenSinh>().GetListByParameter(x => x.ChuyenNghanhDuTuyenId == chuyennganhid).OrderBy(x=>x.Email)
                    : _unitOfWork.GetRepositoryInstance<DangKyTuyenSinh>().GetListByParameter(x=>x.MaNCS ==null).OrderBy(x => x.Email);
                foreach (DangKyTuyenSinh item in lstNCS_chuaduyet)
                {
                    string email = item.Email;
                    string hoten = item.HoTen;
                    var user = _unitOfWork.GetRepositoryInstance<User>().GetFirstOrDefaultByParameter(x => x.Email == email);
                    if (user.IsLock == null) user.IsLock = false;
                    if (user != null)
                    {
                        listTaiKhoan.Add(new TaiKhoan() { Username = user.Email, Email = user.Email, Name = hoten, isLock = user.IsLock.Value, Id = user.Id });
                    }
                }
                return PartialView("_PartialListUser2", listTaiKhoan);
            }
            catch (Exception ex)
            {
                string mss = ex.Message;
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(controllerName, actionName, user == null ? string.Empty : user.Username, ex);
                return PartialView("_PartialListUser2", null);
            }
        }
    }
}