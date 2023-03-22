using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEMIS.Model;
using TEMIS.CMS.Common;
using TEMIS.CMS.Repository;
using System.Threading.Tasks;
using CoreAPI.Entity;
using TEMIS.CMS.Areas.Admin.Models;
namespace TEMIS.CMS.Areas.Admin.Controllers
{
    [AuditAction]
    public class QuanLyLichBaoVeController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        public string urlFile = "Upload\\VanBan\\";
        // GET: Admin/QuanLyLichBaoVe

        public async Task<ActionResult> Index()
        {
            List<ChuyenNganhDaoTao> listChuyenNganhDaoTao = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetAllRecords().OrderBy(o => o.MaChuyenNganh).ToList();
            ViewBag.ListChuyenNganhDaoTao = listChuyenNganhDaoTao;

            List<KhoaHoc> listKhoaHoc = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetAllRecords().OrderBy(o => o.MaKhoa).ToList();
            ViewBag.ListKhoaHoc = listKhoaHoc;

            return View();
        }


        public async Task<ActionResult> LoadData(string makhoahoc = "", int nganhid = 0, string mancs = "")
        {
            try
            {
                List<LichBaoVe> list_data = new List<LichBaoVe>();

                if (makhoahoc != "" && nganhid > 0 && mancs != "")
                {
                    list_data = _unitOfWork.GetRepositoryInstance<LichBaoVe>().GetListByParameter(o => o.KhoaHoc == makhoahoc && o.ChuyenNganhId == nganhid && o.MaNCS == mancs).OrderByDescending(x => x.Id).ToList();
                }
                else if (makhoahoc != "" && nganhid > 0 && mancs == "")
                {
                    list_data = _unitOfWork.GetRepositoryInstance<LichBaoVe>().GetListByParameter(o => o.KhoaHoc == makhoahoc && o.ChuyenNganhId == nganhid).OrderByDescending(x => x.Id).ToList();
                }
                else if (makhoahoc != "" && nganhid == 0 && mancs != "")
                {
                    list_data = _unitOfWork.GetRepositoryInstance<LichBaoVe>().GetListByParameter(o => o.KhoaHoc == makhoahoc && o.MaNCS == mancs).OrderByDescending(x => x.Id).ToList();
                }
                else if (makhoahoc == "" && nganhid > 0 && mancs != "")
                {
                    list_data = _unitOfWork.GetRepositoryInstance<LichBaoVe>().GetListByParameter(o => o.ChuyenNganhId == nganhid && o.MaNCS == mancs).OrderByDescending(x => x.Id).ToList();
                }
                else if (makhoahoc == "" && nganhid > 0 && mancs == "")
                {
                    list_data = _unitOfWork.GetRepositoryInstance<LichBaoVe>().GetListByParameter(o => o.ChuyenNganhId == nganhid).OrderByDescending(x => x.Id).ToList();
                }
                else
                {
                    list_data = _unitOfWork.GetRepositoryInstance<LichBaoVe>().GetAllRecords().OrderByDescending(x => x.Id).Take(300).ToList();
                }

                return PartialView("_PartialData", list_data);
            }
            catch (Exception ex)
            {
                string mss = ex.Message;
                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> ThemMoi()
        {
            try
            {
                string tendetai = Request.Form["tendetai"] != null ? Request.Form["tendetai"].ToString() : "";
                string mancs = Request.Form["mancs"] != null ? Request.Form["mancs"].ToString() : "";
                string tenncs = Request.Form["tenncs"] != null ? Request.Form["tenncs"].ToString() : "";

                string khoahoc = Request.Form["khoahoc"] != null ? Request.Form["khoahoc"].ToString() : "";
                int chuyennganhdaotaoId = Request.Form["chuyennganhdaotao"] != null ? int.Parse(Request.Form["chuyennganhdaotao"].ToString()) : 0;
                string capbaove = Request.Form["capbaove"] != null ? Request.Form["capbaove"].ToString() : "";

                DateTime ngaybaove = Request.Form["ngaybaove"] != null ? DateTime.Parse(Request.Form["ngaybaove"].ToString()) : DateTime.Now;
                string giobaove = Request.Form["giobaove"] != null ? Request.Form["giobaove"].ToString() : "";
                string diadiem = Request.Form["diadiem"] != null ? Request.Form["diadiem"].ToString() : "";

                var lichbvcheck = _unitOfWork.GetRepositoryInstance<LichBaoVe>().GetFirstOrDefaultByParameter(x => x.TenDeTai == tendetai.Trim() && x.MaNCS == mancs);
                if (lichbvcheck != null)
                {
                    return Json("Bản ghi đã tồn tại", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ChuyenNganhDaoTao chuyennganhdaotao = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetFirstOrDefaultByParameter(o => o.Id == chuyennganhdaotaoId);
                    LichBaoVe lbv = new LichBaoVe();
                    lbv.TenDeTai = tendetai;
                    lbv.MaNCS = mancs;
                    lbv.TenNCS = tenncs;
                    lbv.KhoaHoc = khoahoc;
                    lbv.ChuyenNganhId = chuyennganhdaotaoId;
                    lbv.TenChuyenNganh = chuyennganhdaotao.TenChuyenNganh;
                    lbv.CapBaoVe = capbaove;
                    lbv.NgayBaoVe = ngaybaove;
                    lbv.GioBaoVe = giobaove;
                    lbv.DiaDiem = diadiem;

                    var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                    lbv.CreatedBy = user.Username;
                    lbv.CreatedAt = DateTime.Now;
                    lbv.UpdatedBy = user.Username;
                    lbv.UpdatedAt = DateTime.Now;

                    _unitOfWork.GetRepositoryInstance<LichBaoVe>().Add(lbv);
                    _unitOfWork.SaveChanges();
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json("Thêm mới lỗi", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<JsonResult> Update()
        {
            try
            {
                int id = Request.Form["id"] != null ? int.Parse(Request.Form["id"].ToString()) : 0;
                string tendetai = Request.Form["tendetai"] != null ? Request.Form["tendetai"].ToString() : "";
                string mancs = Request.Form["mancs"] != null ? Request.Form["mancs"].ToString() : "";
                string tenncs = Request.Form["tenncs"] != null ? Request.Form["tenncs"].ToString() : "";

                string khoahoc = Request.Form["khoahoc"] != null ? Request.Form["khoahoc"].ToString() : "";
                int chuyennganhdaotaoId = Request.Form["chuyennganhdaotao"] != null ? int.Parse(Request.Form["chuyennganhdaotao"].ToString()) : 0;
                string capbaove = Request.Form["capbaove"] != null ? Request.Form["capbaove"].ToString() : "";

                DateTime ngaybaove = Request.Form["ngaybaove"] != null ? DateTime.Parse(Request.Form["ngaybaove"].ToString()) : DateTime.Now;
                string giobaove = Request.Form["giobaove"] != null ? Request.Form["giobaove"].ToString() : "";
                string diadiem = Request.Form["diadiem"] != null ? Request.Form["diadiem"].ToString() : "";

                LichBaoVe lbv = _unitOfWork.GetRepositoryInstance<LichBaoVe>().GetFirstOrDefaultByParameter(x => x.Id == id);
                if (lbv != null)
                {
                    ChuyenNganhDaoTao chuyennganhdaotao = _unitOfWork.GetRepositoryInstance<ChuyenNganhDaoTao>().GetFirstOrDefaultByParameter(o => o.Id == chuyennganhdaotaoId);
                    lbv.TenDeTai = tendetai;
                    lbv.MaNCS = mancs;
                    lbv.TenNCS = tenncs;
                    lbv.KhoaHoc = khoahoc;
                    lbv.ChuyenNganhId = chuyennganhdaotaoId;
                    lbv.TenChuyenNganh = chuyennganhdaotao.TenChuyenNganh;
                    lbv.CapBaoVe = capbaove;
                    lbv.NgayBaoVe = ngaybaove;
                    lbv.GioBaoVe = giobaove;
                    lbv.DiaDiem = diadiem;

                    var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                    lbv.UpdatedBy = user.Username;
                    lbv.UpdatedAt = DateTime.Now;

                    _unitOfWork.GetRepositoryInstance<LichBaoVe>().Update(lbv);
                    _unitOfWork.SaveChanges();
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Bản ghi không tồn tại", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json("cập nhật bản ghi lỗi", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Xoa(int id)
        {
            string mesage = "";
            try
            {
                LichBaoVe lbv = _unitOfWork.GetRepositoryInstance<LichBaoVe>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (lbv != null)
                {
                    _unitOfWork.GetRepositoryInstance<LichBaoVe>().Remove(lbv);
                    _unitOfWork.SaveChanges();
                    mesage = "Xóa thành công";
                }
                else
                {
                    mesage = "Không tìm thấy bản ghi cần xóa";
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