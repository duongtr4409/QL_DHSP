using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEMIS.CMS.Common;
using TEMIS.CMS.Repository;
using TEMIS.Model;
using System.Web;
using System.Threading.Tasks;
using CoreAPI.Entity;

namespace TEMIS.CMS.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        public JsonResult KeepSessionAlive()
        {
            return new JsonResult { Data = "Success" };
        }

        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        public async Task<ActionResult> Index()
        {
            string url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
            Uri myUri = new Uri(url);
            string param_accessToken = HttpUtility.ParseQueryString(myUri.Query).Get("accessToken");
            if (!string.IsNullOrEmpty(param_accessToken))
            {
                var loginInfo = await CoreAPI.CoreAPI.ValidateLogin(param_accessToken);
                if (loginInfo != null)
                {
                    var role = _unitOfWork.GetRepositoryInstance<UserRoles>().GetFirstOrDefaultByParameter(x => x.UserName.Equals(loginInfo.Username));
                    CoreAPI.CoreAPI.AuthToken = "vYlFowfUqJ2jc2BVCas0QixbBa1uK4ehekwn7cpdAsmiYPVcFOXKzBuMlJEO5uKvoVF1FXYjuP0i6ODvEZp5hUsczjCyXbiPh04CKdWr0xakLLPWFTJfFPOUmmCgVjMS637409529212396028";
                    Session[PublicConstant.LOGIN_INFO] = loginInfo;
                    if (role != null)
                    {
                        Session[PublicConstant.ROLE_INFO] = role;
                        if (role.Role.Equals(PublicConstant.ROLE_NCS))
                        {
                            return Redirect("~/HoSo/Index");
                        }
                        else if (role.Role.Equals(PublicConstant.ROLE_ADMINSTRATOR) || role.Role.Equals(PublicConstant.ROLE_CB_PHONG_SDH))
                        {
                            return Redirect("~/Admin/HomeAdmin");
                        }
                        else if (role.Role.Equals(PublicConstant.ROLE_CB_PHONG_TAI_CHINH))
                        {
                            return Redirect("~/Admin/QuanLyHocPhi/ApproveList");
                        }
                        else if (role.Role.Equals(PublicConstant.ROLE_GIANG_VIEN_HD) || role.Role.Equals(PublicConstant.ROLE_TRUONG_KHOA_DT))
                        {
                            return Redirect("~/Admin/ThiSinhTuyenSinh/Index");
                        }
                        else if (role.Role.Equals(PublicConstant.ROLE_CB_THU_VIEN))
                        {
                            return Redirect("~/Admin/QuanLyThuVien");
                        }
                    }
                    else
                    {
                        role = new UserRoles();
                        role.Email = loginInfo.Email;
                        role.UserName = loginInfo.Username;
                        role.Role = PublicConstant.ROLE_GIANG_VIEN_HD;
                        role.CreatedAt = DateTime.Now;
                        role.UpdatedAt = DateTime.Now;
                        role.CreatedBy = loginInfo.Username;
                        role.UpdatedBy = loginInfo.Username;
                        _unitOfWork.GetRepositoryInstance<UserRoles>().Add(role);
                        _unitOfWork.SaveChanges();
                        Session[PublicConstant.ROLE_INFO] = role;

                        return Redirect("~/Admin/ThiSinhTuyenSinh/Index");
                    }
                }
            }
            if (User.IsInRole(PublicConstant.ROLE_ADMINSTRATOR) ||
                        User.IsInRole(PublicConstant.ROLE_CB_PHONG_SDH))
            {
                return Redirect("~/Admin/HomeAdmin");
            }
            else if (User.IsInRole(PublicConstant.ROLE_CB_PHONG_TAI_CHINH))
            {
                return Redirect("~/Admin/QuanLyHocPhi/ApproveList");
            }
            else if (User.IsInRole(PublicConstant.ROLE_GIANG_VIEN_HD) || User.IsInRole(PublicConstant.ROLE_TRUONG_KHOA_DT))
            {
                return Redirect("~/Admin/QuanLyDiem/TraCuuDiem");

            }
            else if (User.IsInRole(PublicConstant.ROLE_CB_THU_VIEN))
            {
                return Redirect("~/Admin/QuanLyThuVien");
            }
            else
            {
                List<TinTuc> listTin = _unitOfWork.GetRepositoryInstance<TinTuc>().GetListByParameter(o => o.DanhMuc == 1).OrderByDescending(o => o.CreatedAt).Take(6).ToList();
                List<TinTuc> listThongBao = _unitOfWork.GetRepositoryInstance<TinTuc>().GetListByParameter(o => o.DanhMuc == 2).OrderByDescending(o => o.CreatedAt).Take(6).ToList();
                ViewBag.listTin = listTin;
                ViewBag.listThongBao = listThongBao;
                return View();
            }
        }
        public ActionResult TinTuc(int id)
        {
            TinTuc tin = _unitOfWork.GetRepositoryInstance<TinTuc>().GetFirstOrDefaultByParameter(o => o.Id == id);
            tin.CountView = tin.CountView == null ? 1 : tin.CountView + 1;
            _unitOfWork.GetRepositoryInstance<TinTuc>().Update(tin);
            _unitOfWork.SaveChanges();
            List<TinTuc> listTin = _unitOfWork.GetRepositoryInstance<TinTuc>().GetListByParameter(o => o.DanhMuc == 1 && o.Id != tin.Id).OrderByDescending(o => o.CreatedAt).Take(6).ToList();
            ViewBag.listTin = listTin;
            return View(tin);
        }
        public ActionResult ThongBao(int id)
        {
            TinTuc thongbao = _unitOfWork.GetRepositoryInstance<TinTuc>().GetFirstOrDefaultByParameter(o => o.Id == id);
            List<TinTuc> listThongBao = _unitOfWork.GetRepositoryInstance<TinTuc>().GetListByParameter(o => o.DanhMuc == 2).OrderByDescending(o => o.CreatedAt).Take(6).ToList();
            ViewBag.listThongBao = listThongBao;
            return View(thongbao);
        }

        public ActionResult GetListThongBaoMenu()
        {
            List<ThongBao> listThongBao = new List<ThongBao>();
            var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
            if (user != null)
            {
                listThongBao = _unitOfWork.GetRepositoryInstance<Model.ThongBao>().GetListByParameter(o => o.MaNCS == user.Username || o.Email == user.Username).OrderByDescending(o => o.Id).ToList();
            }

            return PartialView("partialThongBao", listThongBao);
        }

        public JsonResult UpdateStatus(int? id = 0)
        {
            string str = "";
            var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
            try
            {
                if (id == 0)
                {

                    List<ThongBao> lst = _unitOfWork.GetRepositoryInstance<ThongBao>().GetListByParameter(o => (o.MaNCS == user.Username || o.Email == user.Username) && o.TrangThai != 1).ToList();
                    if (lst.Count() > 0)
                    {
                        foreach (ThongBao item in lst)
                        {
                            item.TrangThai = 1;
                            _unitOfWork.GetRepositoryInstance<ThongBao>().Update(item);
                        }
                    }
                }
                else
                {
                    ThongBao noti = _unitOfWork.GetRepositoryInstance<ThongBao>().GetFirstOrDefaultByParameter(x => x.Id == id);
                    if (noti != null)
                    {
                        noti.TrangThai = 1;
                        _unitOfWork.GetRepositoryInstance<ThongBao>().Update(noti);
                    }
                }
                _unitOfWork.SaveChanges();
            }
            catch (Exception)
            {
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public async Task<ActionResult> ThongBaoTrungTuyen()
        {
            string url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
            Uri myUri = new Uri(url);
            string param_accessToken = HttpUtility.ParseQueryString(myUri.Query).Get("accessToken");
            if (!string.IsNullOrEmpty(param_accessToken))
            {
                var loginInfo = await CoreAPI.CoreAPI.ValidateLogin(param_accessToken);
                if (loginInfo != null)
                {
                    var role = _unitOfWork.GetRepositoryInstance<UserRoles>().GetFirstOrDefaultByParameter(x => x.UserName.Equals(loginInfo.Username));
                    CoreAPI.CoreAPI.AuthToken = "vYlFowfUqJ2jc2BVCas0QixbBa1uK4ehekwn7cpdAsmiYPVcFOXKzBuMlJEO5uKvoVF1FXYjuP0i6ODvEZp5hUsczjCyXbiPh04CKdWr0xakLLPWFTJfFPOUmmCgVjMS637409529212396028";
                    Session[PublicConstant.LOGIN_INFO] = loginInfo;
                    if (role != null)
                    {
                        Session[PublicConstant.ROLE_INFO] = role;
                        if (role.Role.Equals(PublicConstant.ROLE_NCS))
                        {
                            return Redirect("~/HoSo/Index");
                        }
                        else if (role.Role.Equals(PublicConstant.ROLE_ADMINSTRATOR) || role.Role.Equals(PublicConstant.ROLE_CB_PHONG_SDH))
                        {
                            return Redirect("~/Admin/HomeAdmin");
                        }
                        else if (role.Role.Equals(PublicConstant.ROLE_CB_PHONG_TAI_CHINH))
                        {
                            return Redirect("~/Admin/QuanLyHocPhi/ApproveList");
                        }
                        else if (role.Role.Equals(PublicConstant.ROLE_GIANG_VIEN_HD) || role.Role.Equals(PublicConstant.ROLE_TRUONG_KHOA_DT))
                        {
                            return Redirect("~/Admin/ThiSinhTuyenSinh/Index");
                        }
                        else if (role.Role.Equals(PublicConstant.ROLE_CB_THU_VIEN))
                        {
                            return Redirect("~/Admin/QuanLyThuVien");
                        }
                    }
                    else
                    {
                        role = new UserRoles();
                        role.Email = loginInfo.Email;
                        role.UserName = loginInfo.Username;
                        role.Role = PublicConstant.ROLE_GIANG_VIEN_HD;
                        role.CreatedAt = DateTime.Now;
                        role.UpdatedAt = DateTime.Now;
                        role.CreatedBy = loginInfo.Username;
                        role.UpdatedBy = loginInfo.Username;
                        _unitOfWork.GetRepositoryInstance<UserRoles>().Add(role);
                        _unitOfWork.SaveChanges();
                        Session[PublicConstant.ROLE_INFO] = role;

                        return Redirect("~/Admin/ThiSinhTuyenSinh/Index");
                    }
                }
            }
            if (User.IsInRole(PublicConstant.ROLE_ADMINSTRATOR) ||
                        User.IsInRole(PublicConstant.ROLE_CB_PHONG_SDH))
            {
                return Redirect("~/Admin/HomeAdmin");
            }
            else if (User.IsInRole(PublicConstant.ROLE_CB_PHONG_TAI_CHINH))
            {
                return Redirect("~/Admin/QuanLyHocPhi/ApproveList");
            }
            else if (User.IsInRole(PublicConstant.ROLE_GIANG_VIEN_HD) || User.IsInRole(PublicConstant.ROLE_TRUONG_KHOA_DT))
            {
                return Redirect("~/Admin/QuanLyDiem/TraCuuDiem");

            }
            else if (User.IsInRole(PublicConstant.ROLE_CB_THU_VIEN))
            {
                return Redirect("~/Admin/QuanLyThuVien");
            }
            else
            {
                List<TinTuc> listTin = _unitOfWork.GetRepositoryInstance<TinTuc>().GetListByParameter(o => o.DanhMuc == 1).OrderByDescending(o => o.CreatedAt).Take(6).ToList();
                List<TinTuc> listThongBao = _unitOfWork.GetRepositoryInstance<TinTuc>().GetListByParameter(o => o.DanhMuc == 2).OrderByDescending(o => o.CreatedAt).Take(6).ToList();
                ViewBag.listTin = listTin;
                ViewBag.listThongBao = listThongBao;
                return View();
            }
        }
    }
}