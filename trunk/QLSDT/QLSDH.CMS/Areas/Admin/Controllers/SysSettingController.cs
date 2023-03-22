using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TEMIS.CMS.Repository;
using TEMIS.Model;
using TEMIS.CMS.Common;
namespace TEMIS.CMS.Areas.Admin.Controllers
{
    // [Roles(PublicConstant.ROLE_ADMIN, PublicConstant.ROLE_SUPPERADMIN)]
    //[Authorize]
    [AuditAction]
    public class SysSettingController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();

        // GET: Admin/ChucDanh
        public ActionResult Index()
        {
            var lstData = _unitOfWork.GetRepositoryInstance<SysSetting>().GetAllRecords().OrderBy(o => o.SKey).ToList();
            return View(lstData);
        }

        public async Task<JsonResult> ThemMoi(string key, string name, string value)
        {
            try
            {
                SysSetting sysSetting = _unitOfWork.GetRepositoryInstance<SysSetting>().GetFirstOrDefaultByParameter(o => o.SKey == key);
                if (sysSetting != null)
                {
                    TempData["error"] = "Key đã  tồn tại trên hệ thống";
                    return Json("Key đã  tồn tại trên hệ thống", JsonRequestBehavior.AllowGet);
                }
                sysSetting = new SysSetting();
                sysSetting.SKey = key;
                sysSetting.Name = name;
                sysSetting.Value = value;
                _unitOfWork.GetRepositoryInstance<SysSetting>().Add(sysSetting);
                _unitOfWork.SaveChanges();

                TempData["message"] = "Thêm  mới thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi thêm mới: " + ex.Message;
                return Json("Lỗi thêm mới", JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> Sua(int id, string key, string name, string value)
        {
            try
            {
                SysSetting sysSetting = _unitOfWork.GetRepositoryInstance<SysSetting>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (sysSetting != null)
                {
                    sysSetting.SKey = key;
                    sysSetting.Name = name;
                    sysSetting.Value = value;
                    _unitOfWork.GetRepositoryInstance<SysSetting>().Update(sysSetting);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    TempData["error"] = "bản ghi không tồn tại";
                    return Json("bản ghi không tồn tại", JsonRequestBehavior.AllowGet);
                }

                TempData["message"] = "Cập nhật thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi Cập nhật: " + ex.Message;
                return Json("Lỗi Cập nhật", JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> Xoa(long id)
        {
            try
            {
                SysSetting setting = _unitOfWork.GetRepositoryInstance<SysSetting>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (setting != null)
                {
                    _unitOfWork.GetRepositoryInstance<SysSetting>().Remove(setting);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    TempData["error"] = "bản ghi không tồn tại";
                    return Json("bản ghi không tồn tại", JsonRequestBehavior.AllowGet);
                }

                TempData["message"] = "Xóa thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi xóa: " + ex.Message;
                return Json("Lỗi xóa", JsonRequestBehavior.AllowGet);
            }
        }
    }
}