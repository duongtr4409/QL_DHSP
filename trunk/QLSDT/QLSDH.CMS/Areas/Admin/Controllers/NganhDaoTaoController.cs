using CoreAPI.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TEMIS.CMS.Areas.Admin.Models;
using TEMIS.CMS.Common;
using TEMIS.CMS.Repository;
using TEMIS.Model;
using TEMIS.CMS.Common;
namespace TEMIS.CMS.Areas.Admin.Controllers
{
    [AuditAction]
    public class NganhDaoTaoController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();

        // GET: Admin/NganhDaoTao
        public async Task<ActionResult> Index()
        {
            List<OrganizationInfo> list = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = list;

            List<NganhDaoTao> listNganhDaoTao = _unitOfWork.GetRepositoryInstance<NganhDaoTao>().GetAllRecords().ToList();
            ViewBag.listNganhDaoTao = listNganhDaoTao;
            return View();
        }


        public async Task<ActionResult> LoadData(int idKhoa = 0)
        {
            try
            {
                List<NganhDaoTao> list_data = new List<NganhDaoTao>();
                
                if (idKhoa > 0)
                {
                    list_data = _unitOfWork.GetRepositoryInstance<NganhDaoTao>().GetListByParameter(o => o.KhoaId == idKhoa).OrderByDescending(x => x.TenNganh).ToList();
                }
                else
                {
                    list_data = _unitOfWork.GetRepositoryInstance<NganhDaoTao>().GetAllRecords().OrderByDescending(x => x.TenNganh).ToList();
                }

                if (list_data.Count == 0)
                {
                    TempData["message"] = "Không tìm thấy kết quả nào";
                }
                return PartialView("_PartialListData", list_data);

            }
            catch (Exception ex)
            {
                string mss = ex.Message;
                throw;
            }

        }

        public JsonResult ThemMoi(string manganh, string tennganh, int khoaid, string tenkhoa)
        {
            try
            {
                var nganh = _unitOfWork.GetRepositoryInstance<NganhDaoTao>().GetFirstOrDefaultByParameter(x => x.TenNganh == tennganh.Trim());
                if (nganh != null)
                {
                    return Json("Ngành đã tồn tại", JsonRequestBehavior.AllowGet);

                }
                else
                {
                    nganh = new NganhDaoTao();
                    nganh.MaNganh = manganh;
                    nganh.TenNganh = tennganh;
                    nganh.KhoaId = khoaid;
                    nganh.TenKhoa = tenkhoa;
                    _unitOfWork.GetRepositoryInstance<NganhDaoTao>().Add(nganh);
                    _unitOfWork.SaveChanges();
                    return Json("OK", JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception)
            {
                return Json("Thêm mới ngành lỗi", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Update(int id, string manganh, string tennganh, int khoaid, string tenkhoa)
        {
            try
            {
                var nganh = _unitOfWork.GetRepositoryInstance<NganhDaoTao>().GetFirstOrDefaultByParameter(x => x.Id == id);
                if (nganh != null)
                {
                    nganh.MaNganh = manganh;
                    nganh.TenNganh = tennganh;
                    nganh.KhoaId = khoaid;
                    nganh.TenKhoa = tenkhoa;
                    _unitOfWork.GetRepositoryInstance<NganhDaoTao>().Update(nganh);
                    _unitOfWork.SaveChanges();
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Ngành không tồn tại", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json("cập nhật ngành lỗi", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult XoaNganh(int id)
        {
            string mesage = "";
            try
            {
                List<ChuyenNganhDaoTao> listChuyenNghanhDaoTao = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetListByParameter(o => o.NganhId == id).ToList();
                if (listChuyenNghanhDaoTao.Count == 0)
                {
                    NganhDaoTao checkNganh = _unitOfWork.GetRepositoryInstance<NganhDaoTao>().GetFirstOrDefaultByParameter(o => o.Id == id);
                    if (checkNganh != null)
                    {
                        _unitOfWork.GetRepositoryInstance<NganhDaoTao>().Remove(checkNganh);
                        _unitOfWork.SaveChanges();
                    }
                }
                else
                {
                    mesage = "Không thể xóa do vẫn còn chuyên nghành trong nghành này";
                }
            }
            catch (Exception ex)
            {
                mesage = "Lỗi xóa bản ghi: " + ex.Message;
            }

            return Json(mesage, JsonRequestBehavior.AllowGet);
        }
    }
}