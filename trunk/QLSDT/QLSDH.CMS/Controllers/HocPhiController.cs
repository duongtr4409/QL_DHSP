using CoreAPI.Entity;
using Excel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TEMIS.CMS.Common;
using TEMIS.CMS.Models;
using TEMIS.CMS.Repository;
using TEMIS.Model;

namespace TEMIS.CMS.Areas.Admin.Controllers
{
    public class HocPhiController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        private Model.TEMIS_systemEntities db = new Model.TEMIS_systemEntities();

        public HocPhiController()
        {
        }
        public HocPhiController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationSignInManager _signInManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

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

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        // GET: Admin/GiangVien
        public async Task<ActionResult> Index()
        {
            var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
            var lstHocPhi = _unitOfWork.GetRepositoryInstance<HocPhi>().GetListByParameter(x => x.UserName == user.Email || x.MaNCS == user.Username).ToList();
            return View(lstHocPhi);
        }

        [HttpPost]
        public async Task<JsonResult> UploadHocPhi()
        {
            try
            {
                string fileUrl = "";
                var user = (CoreAPI.Entity.TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                int id = Request.Form["hdfId"] != null ? int.Parse(Request.Form["hdfId"].ToString()) : 0;
                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;

                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] ffiles = file.FileName.Split(new char[] { '\\' });
                            fname = user.Username + "_" + ffiles[ffiles.Length - 1];
                        }
                        else
                        {
                            fname = user.Username + "_" + file.FileName;
                        }
                        fileUrl = fname;
                        fname = Path.Combine(Server.MapPath("~/Images/BienLai/"), fname);
                        file.SaveAs(fname);
                    }
                    var item = _unitOfWork.GetRepositoryInstance<HocPhi>().GetFirstOrDefaultByParameter(x => x.Id == id);
                    item.File = fileUrl;
                    item.TrangThai = PublicConstant.CHO_DUYET;
                    _unitOfWork.GetRepositoryInstance<HocPhi>().Update(item);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    TempData["error"] = "Bạn chưa file upload";
                    return Json("Bạn chưa chọn ảnh upload", JsonRequestBehavior.AllowGet);
                }



                TempData["message"] = "Upload file thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi upload file: " + ex.Message;
                return Json("Lỗi upload", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            try
            {
                if (file != null)
                {
                    string pic = System.IO.Path.GetFileName(file.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/Images/BienLai"), pic);
                    file.SaveAs(path);
                    int id = int.Parse(Request.Form["id_HocPhi"]);
                    var item = _unitOfWork.GetRepositoryInstance<HocPhi>().GetFirstOrDefaultByParameter(x => x.Id == id);
                    item.File = pic;
                    _unitOfWork.GetRepositoryInstance<HocPhi>().Update(item);
                    _unitOfWork.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }

        }
    }
}