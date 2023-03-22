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
using TEMIS.CMS.Models;
using CoreAPI.Entity;

namespace TEMIS.CMS.Areas.Admin.Controllers
{
    public class HocTapController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        private Model.TEMIS_systemEntities db = new Model.TEMIS_systemEntities();

        public HocTapController()
        {
        }
        public HocTapController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
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

        [AuditAction]
        public async Task<ActionResult> Index()
        {
            return View();
        }

        public ActionResult GetListHocTap()
        {
            List<ViewModelHocPhan> lstHocPhan = new List<ViewModelHocPhan>();
            try
            {
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                NCS ncs = _unitOfWork.GetRepositoryInstance<NCS>().GetFirstOrDefaultByParameter(o => o.Ma == user.Username);
                if(ncs != null)
                {
                    List<HocPhan> listData = _unitOfWork.GetRepositoryInstance<HocPhan>().GetListByParameter(o => o.KhoaId == ncs.KhoaId).ToList(); // lấy danh sách hp theo Khoa đăng ký dự tuyển
                    foreach (var item in listData)
                    {
                        ViewModelHocPhan hp = new ViewModelHocPhan();
                        hp.Id = item.Id;
                        hp.TenHocPhan = item.TenHocPhan;
                        hp.SoDVHT = item.SoDVHT;
                        hp.DieuKien = item.DieuKien;
                        hp.TuChon = item.TuChon;
                        hp.SoTietLyThuyet = item.SoTietLyThuyet;
                        hp.SoTietThucHanh = item.SoTietThucHanh;
                        hp.MaHocPhan = item.MaHocPhan;
                        hp.CreatedAt = item.CreatedAt;
                        hp.UpdatedAt = item.UpdatedAt;
                        hp.CreatedBy = item.CreatedBy;
                        hp.UpdatedBy = item.UpdatedBy;
                        hp.KhoaHocId = item.KhoaHocId;
                        hp.KhoaId = item.KhoaId;
                        hp.NganhId = item.NganhId;
                        hp.ChuyenNganhId = item.ChuyenNganhId;
                        hp.TinChi = item.SoTinChi;
                        Diem diem = _unitOfWork.GetRepositoryInstance<Diem>().GetFirstOrDefaultByParameter(o => o.MaHocVien == ncs.Ma);
                        if (diem != null)
                        {
                            if (item.LoaiHP == 1)
                            {
                                hp.DiemHocPhan = diem.DiemHP1.ToString();
                            }
                            else if (item.LoaiHP == 2)
                            {
                                hp.DiemHocPhan = diem.DiemHP2.ToString();
                            }
                            else if (item.LoaiHP == 3)
                            {
                                hp.DiemHocPhan = diem.DiemHP3.ToString();
                            }
                            else if (item.LoaiHP == 4)
                            {
                                hp.DiemHocPhan = diem.DiemHP4.ToString();
                            }
                        }
                        else
                        {
                            hp.DiemHocPhan = "-";
                        }
                        HocPhan_NCS hocPhan_NCS = _unitOfWork.GetRepositoryInstance<HocPhan_NCS>().GetFirstOrDefaultByParameter(o => o.MaHocPhan == item.MaHocPhan && o.MaNCS == user.Username);
                        if (hocPhan_NCS != null)
                        {
                            hp.TrangThaiDangKy = true;
                        }
                        else
                        {
                            hp.TrangThaiDangKy = false;
                        }

                        lstHocPhan.Add(hp);
                    }
                }
                else
                {
                    return PartialView("_PartialData", null);
                }
            }
            catch (Exception)
            {
                return View();
            }

            return PartialView("_PartialData", lstHocPhan);
        }

        [AuditAction]
        public async Task<JsonResult> DangKyHocPhan(int id)
        {
            try
            {
                HocPhan hocphan = new HocPhan();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                hocphan = _unitOfWork.GetRepositoryInstance<HocPhan>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (hocphan != null)
                {
                    HocPhan_NCS hocphan_NCS = new HocPhan_NCS();
                    hocphan_NCS.MaNCS = user.Username;
                    hocphan_NCS.MaHocPhan = hocphan.MaHocPhan;
                    hocphan_NCS.TenHocPhan = hocphan.TenHocPhan;
                    hocphan_NCS.TinChi = hocphan.SoTinChi;
                    hocphan_NCS.Status = false;
                    hocphan_NCS.CreatedAt = DateTime.Now;
                    hocphan_NCS.UpdatedAt = DateTime.Now;
                    hocphan_NCS.CreatedBy = user.Username;
                    hocphan_NCS.UpdatedBy = user.Username;
                    hocphan_NCS.TuChon = true;
                    _unitOfWork.GetRepositoryInstance<HocPhan_NCS>().Add(hocphan_NCS);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    TempData["error"] = "Bản ghi không tồn tại";
                    return Json("Bản ghi không tồn tại", JsonRequestBehavior.AllowGet);
                }

                TempData["message"] = "Đăng ký thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi đăng ký: " + ex.Message;
                return Json("Lỗi đăng ký", JsonRequestBehavior.AllowGet);
            }
        }
    }
}