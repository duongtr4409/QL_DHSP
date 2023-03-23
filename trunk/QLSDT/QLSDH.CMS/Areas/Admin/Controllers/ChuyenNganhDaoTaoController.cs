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
using CoreAPI;
using TEMIS.CMS.Common;
namespace TEMIS.CMS.Areas.Admin.Controllers
{
    [AuditAction]
    public class ChuyenNganhDaoTaoController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();

        // GET: Admin/ChuyenNganhDaoTao
        public async Task<ActionResult> Index()
        {
            List<OrganizationInfo> list = await CoreAPI.CoreAPI.GetListKhoa();
            List<ChuyenNganhDaoTao> list_data = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetAllRecords().ToList();
            ViewBag.ListKhoa = list;
            ViewBag.ListChuyenNganhDaoTao = list_data;
            return View();
        }

        public async Task<ActionResult> LoadData(int nganhid = 0)
        {
            try
            {
                List<ChuyenNganhDaoTao> list_data = new List<ChuyenNganhDaoTao>();
                List<NganhDaoTao> listNganhDaoTao = _unitOfWork.GetRepositoryInstance<NganhDaoTao>().GetAllRecords().ToList();


                if (nganhid > 0)
                {
                    list_data = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetListByParameter(o => o.NganhId == nganhid).OrderByDescending(x => x.TenChuyenNganh).ToList();
                }
                else
                {
                    list_data = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetAllRecords().OrderByDescending(x => x.TenChuyenNganh).ToList();
                }
                for(var i = 0; i < list_data.Count; i++)
                {
                    for (var j = 0; j < listNganhDaoTao.Count; j++)
                    {
                        if (list_data[i].TenNganh == listNganhDaoTao[j].TenNganh)
                        {
                            list_data[i].MaNganh = listNganhDaoTao[j].MaNganh;
                        }
                    }
                }
                if (list_data.Count == 0)
                {
                    TempData["message"] = "Không tìm thấy kết quả nào";
                }
                
                return PartialView("_PartialChuyenNghanh", list_data);

            }
            catch (Exception ex)
            {
                string mss = ex.Message;
                throw;
            }
        }

        public JsonResult LoadNganhByKhoa(int khoaid, int? nganhid = 0)
        {
            string str = "";
            try
            {
                List<NganhDaoTao> listData = _unitOfWork.GetRepositoryInstance<NganhDaoTao>().GetListByParameter(x => x.KhoaId == khoaid).ToList();
                if (listData.Count > 0)
                {
                    str += "<option value=\"0\">--------- Chọn Ngành --------</option>";
                    foreach (var item in listData)
                    {
                        if (nganhid == item.Id)
                        {
                            str += "<option selected value=\"" + item.Id + "\">" + item.TenNganh + "</option>";

                        }
                        else
                        {
                            str += "<option value=\"" + item.Id + "\">" + item.TenNganh + "</option>";
                        }

                    }
                }
            }
            catch (Exception)
            {
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ThemMoi(string machuyennganh, string tenchuyennganh, int Idnganh, string tennganh, int khoaid, string tenkhoa, string manganhthacsi)
        {
            try
            {
                var nganh = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetFirstOrDefaultByParameter(x => x.TenChuyenNganh == tennganh.Trim() && x.MaChuyenNganh == machuyennganh);
                if (nganh != null)
                {
                    return Json("Chuyên ngành đã tồn tại", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    nganh = new ChuyenNganhDaoTao();
                    nganh.MaChuyenNganh = machuyennganh;
                    nganh.TenChuyenNganh = tenchuyennganh;
                    nganh.NganhId = Idnganh;
                    nganh.TenNganh = tennganh;
                    nganh.MaSoCN_ThS = manganhthacsi;
                    nganh.KhoaId = khoaid;
                    nganh.TenKhoa = tenkhoa;
                    nganh.CreatedAt = DateTime.Now;
                    nganh.UpdatedAt = DateTime.Now;
                    var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                    nganh.CreatedBy = user.Username;
                    nganh.UpdatedBy = user.Username;
                    _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().Add(nganh);
                    _unitOfWork.SaveChanges();
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json("Thêm mới ngành lỗi", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Update(int id, string machuyennganh, string tenchuyennganh, int Idnganh, string tennganh, int khoaid, string tenkhoa, string manganhthacsi)
        {
            try
            {
                var nganh = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetFirstOrDefaultByParameter(x => x.Id == id);
                if (nganh != null)
                {
                    nganh.MaChuyenNganh = machuyennganh;
                    nganh.TenChuyenNganh = tenchuyennganh;
                    nganh.NganhId = Idnganh;
                    nganh.TenNganh = tennganh;
                    nganh.MaSoCN_ThS = manganhthacsi;
                    nganh.KhoaId = khoaid;
                    nganh.TenKhoa = tenkhoa;
                    _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().Update(nganh);
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

        public JsonResult XoaChuyenNganh(int id)
        {
            string mesage = "";
            try
            {
                List<HocPhan> lst = _unitOfWork.GetRepositoryInstance<HocPhan>().GetListByParameter(x => x.ChuyenNganhId == id).ToList();
                if(lst.Count == 0)
                {
                    ChuyenNganhDaoTao checkNganh = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetFirstOrDefaultByParameter(o => o.Id == id);
                    if (checkNganh != null)
                    {
                        _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().Remove(checkNganh);
                        _unitOfWork.SaveChanges();
                    }
                }
                else
                {
                    mesage = "Không thể xóa do vẫn còn học phần trong chuyên ngành này";
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