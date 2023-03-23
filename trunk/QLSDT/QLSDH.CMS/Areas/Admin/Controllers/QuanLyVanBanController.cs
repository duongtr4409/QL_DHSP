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
    public class QuanLyVanBanController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        public string urlFile = "Upload\\VanBan\\";
        
        public async Task<ActionResult> Index()
        {
            List<ChuyenMucVanBan> list = _unitOfWork.GetRepositoryInstance<ChuyenMucVanBan>().GetAllRecords().OrderBy(o => o.TenChuyenMuc).ToList();
            ViewBag.ListChuyenMucVanBan = list;

            return View();
        }


        public async Task<ActionResult> LoadData(int idchuyemuc = 0)
        {
            try
            {
                List<VanBan> list_data = new List<VanBan>();

                if (idchuyemuc > 0)
                {
                    list_data = _unitOfWork.GetRepositoryInstance<VanBan>().GetListByParameter(o => o.ChuyenMuc == idchuyemuc).OrderByDescending(x => x.TieuDe).ToList();
                }
                else
                {
                    list_data = _unitOfWork.GetRepositoryInstance<VanBan>().GetAllRecords().OrderByDescending(x => x.TieuDe).ToList();
                }

                List<VanBanViewModel> listVanBanModel = new List<Models.VanBanViewModel>();
                VanBanViewModel vb = new VanBanViewModel();
                foreach (var item in list_data)
                {
                    vb = new VanBanViewModel();
                    vb.Id = item.Id;
                    vb.TieuDe = item.TieuDe;
                    vb.URL = item.URL;
                    vb.ChuyenMuc = item.ChuyenMuc;
                    vb.TenChuyenMuc = _unitOfWork.GetRepositoryInstance<ChuyenMucVanBan>().GetFirstOrDefaultByParameter(o => o.Id == item.ChuyenMuc).TenChuyenMuc;
                    vb.DauMuc = item.DauMuc;
                    vb.Anh = item.Anh;
                    vb.HinhThuc = item.HinhThuc;
                    vb.TrangThai = item.TrangThai;
                    vb.CreatedAt = item.CreatedAt;
                    vb.UpdatedAt = item.UpdatedAt;
                    vb.CreatedBy = item.CreatedBy;
                    vb.UpdatedBy = item.UpdatedBy;
                    listVanBanModel.Add(vb);
                }
                if (list_data.Count == 0)
                {
                    TempData["message"] = "Không tìm thấy kết quả nào";
                }
                return PartialView("_PartialData", listVanBanModel);

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
                string tieude = Request.Form["tieude"] != null ? Request.Form["tieude"].ToString() : "";
                string chuyemuc = Request.Form["chuyemuc"] != null ? Request.Form["chuyemuc"].ToString() : "";
                string daumuc = Request.Form["daumuc"] != null ? Request.Form["daumuc"].ToString() : "";

                var vbocheck = _unitOfWork.GetRepositoryInstance<VanBan>().GetFirstOrDefaultByParameter(x => x.TieuDe == tieude.Trim());
                if (vbocheck != null)
                {
                    return Json("Văn bản đã tồn tại", JsonRequestBehavior.AllowGet);

                }
                else
                {
                    VanBan vb = new VanBan();
                    HttpPostedFileBase file = Request.Files[0];
                    if (file != null)
                    {
                        string namefile = string.Empty;
                        if (file != null && !string.IsNullOrEmpty(file.FileName))
                        {
                            // upload file mới
                            //namefile = Server.MapPath("~/" + urlFile + file.FileName);
                            namefile = System.Web.Hosting.HostingEnvironment.MapPath("~/" + urlFile + file.FileName);
                            file.SaveAs(namefile);

                            vb.URL = file.FileName;
                        }
                    }

                    vb.TieuDe = tieude;
                    vb.ChuyenMuc = int.Parse(chuyemuc);
                    vb.DauMuc = daumuc;
                    var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                    vb.UpdatedBy = user.Username;
                    vb.UpdatedAt = DateTime.Now;

                    _unitOfWork.GetRepositoryInstance<VanBan>().Add(vb);
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
                string tieude = Request.Form["tieude"] != null ? Request.Form["tieude"].ToString() : "";
                string chuyemuc = Request.Form["chuyemuc"] != null ? Request.Form["chuyemuc"].ToString() : "";
                string daumuc = Request.Form["daumuc"] != null ? Request.Form["daumuc"].ToString() : "";
                string url = Request.Form["url"] != null ? Request.Form["url"].ToString() : "";


                var vb = _unitOfWork.GetRepositoryInstance<VanBan>().GetFirstOrDefaultByParameter(x => x.Id == id);
                if (vb != null)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    if (file != null)
                    {
                        string namefile = string.Empty;
                        if (file != null && !string.IsNullOrEmpty(file.FileName))
                        {
                            // delete file 
                            if (System.IO.File.Exists("~/" + urlFile + vb.URL))
                            {
                                System.IO.File.Delete("~/" + urlFile + vb.URL);
                            }
                            // upload file mới
                            namefile = Server.MapPath("~/" + urlFile + file.FileName);
                            file.SaveAs(namefile);

                            vb.URL = file.FileName;
                        }
                        else
                        {
                            vb.URL = url;
                        }
                    }
                    
                    vb.TieuDe = tieude;
                   
                    vb.ChuyenMuc = int.Parse(chuyemuc);
                    vb.DauMuc = daumuc;
                    var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                    vb.UpdatedBy = user.Username;
                    vb.UpdatedAt = DateTime.Now;

                    _unitOfWork.GetRepositoryInstance<VanBan>().Update(vb);
                    _unitOfWork.SaveChanges();
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Ngành không tồn tại", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json("cập nhật ngành lỗi", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Xoa(int id)
        {
            string mesage = "";
            try
            {
                VanBan vb = _unitOfWork.GetRepositoryInstance<VanBan>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (vb != null)
                {
                    _unitOfWork.GetRepositoryInstance<VanBan>().Remove(vb);
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