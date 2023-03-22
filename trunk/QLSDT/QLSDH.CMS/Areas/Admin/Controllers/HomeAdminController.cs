using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TEMIS.CMS.Repository;
using TEMIS.Model;
using TEMIS.CMS.Common;
using CoreAPI.Entity;

namespace TEMIS.CMS.Areas.Admin.Controllers
{
    [AuditAction]
    //[AuthorizeRoles(PublicConstant.ROLE_ADMINSTRATOR, PublicConstant.ROLE_CB_PHONG_SDH)]
    public class HomeAdminController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        [HttpPost]
        public JsonResult KeepSessionAlive()
        {
            return new JsonResult { Data = "Success" };
        }
        // GET: Admin/HomeAdmin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetListThongBaoMenu()
        {
            List<SysNotification> listThongBao = new List<SysNotification>();
            var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
            int idpartment = -1;
            //if (user!=null)
            //{
            //    idpartment =user.DepartmentId;
            //}
            if (user != null)
            {
                listThongBao = _unitOfWork.GetRepositoryInstance<Model.SysNotification>().GetAllRecords().OrderByDescending(o => o.Id).ToList();
            }

            return PartialView("partialThongBao", listThongBao);
        }

        public JsonResult UpdateStatus(int? id = 0)
        {
            string str = "";
            try
            {
                if(id == 0)
                {
                    List<SysNotification> lst = _unitOfWork.GetRepositoryInstance<SysNotification>().GetListByParameter(x => x.Status != 1).ToList();
                    if (lst.Count() > 0)
                    {
                        foreach(SysNotification item in lst)
                        {
                            item.Status = 1;
                            _unitOfWork.GetRepositoryInstance<SysNotification>().Update(item);
                        }
                    }
                }
                else
                {
                    SysNotification noti = _unitOfWork.GetRepositoryInstance<SysNotification>().GetFirstOrDefaultByParameter(x => x.Id == id);
                    if (noti != null)
                    {
                        noti.Status = 1;
                        _unitOfWork.GetRepositoryInstance<SysNotification>().Update(noti);
                    }
                }             
                _unitOfWork.SaveChanges();
            }
            catch (Exception)
            {
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        //public async Task<JsonResult> LoadCity()
        //{
        //    try
        //    {
        //        var listObjCity = _unitOfWork.GetRepositoryInstance<SoGDDT_Tinh>().GetAllRecords();
        //        var listStrCity = "";
        //        foreach (SoGDDT_Tinh item in listObjCity)
        //        {
        //            listStrCity += "<option value = \"" + item.MaTinh + "\">" + item.DonVi + "</option>";
        //        }
        //        return Json(listStrCity, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json("cityfail", JsonRequestBehavior.AllowGet);
        //    }
        //}
        //public async Task<JsonResult> LoadDistrict(string id)
        //{
        //    try
        //    {
        //        var listObjDistr = _unitOfWork.GetRepositoryInstance<PhongGDDT_Huyen>().GetListByParameter(x => x.MaTinh.Equals(id.Trim()) == true);
        //        var listStrDistr = "<option value = \"" + -1 + "\">Chọn huyện</option>"; ;
        //        foreach (PhongGDDT_Huyen item in listObjDistr)
        //        {
        //            listStrDistr += "<option value = \"" + item.MaHuyen + "\">" + item.TenHuyen + "</option>";
        //        }
        //        return Json(listStrDistr, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json("cityfail", JsonRequestBehavior.AllowGet);
        //    }

        //}

        //public async Task<JsonResult> LoadMaChuongTrinh()
        //{
        //    try
        //    {
        //        var lst = _unitOfWork.GetRepositoryInstance<ChuongTrinhBoiDuong>().GetAllRecords();
        //        var listStr = "";
        //        foreach (ChuongTrinhBoiDuong item in lst)
        //        {
        //            listStr += "<option value = \"" + item.MaChuongTrinh + "\" class = \"pull-right col-md-12 col-xs-12\" style = \"line-height: 1.42857143;    height: 34px; background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc \">" + item.TenChuongTrinh + "</option>";
        //        }
        //        return Json(listStr, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json("Fail", JsonRequestBehavior.AllowGet);
        //    }

        //}

        //public async Task<JsonResult> LoadMaMonHoc()
        //{
        //    try
        //    {
        //        var lst = _unitOfWork.GetRepositoryInstance<MonHoc>().GetAllRecords();
        //        var listStr = "";
        //        foreach (MonHoc item in lst)
        //        {
        //            listStr += "<option value = \"" + item.MaMon + "\" class = \"pull-right col-md-12 col-xs-12\" style = \"line-height: 1.42857143;    height: 34px; background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc \">" + item.TenMon + "</option>";
        //        }
        //        return Json(listStr, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json("Fail", JsonRequestBehavior.AllowGet);
        //    }

        //}

    }
}