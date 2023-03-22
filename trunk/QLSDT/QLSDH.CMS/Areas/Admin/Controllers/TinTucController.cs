using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TEMIS.Model;
using TEMIS.CMS.Common;
using TEMIS.CMS.Repository;
using System.Threading.Tasks;
using TEMIS.CMS.Areas.Admin.Models;

namespace TEMIS.CMS.Areas.Admin.Controllers
{
    [AuditAction]
    public class TinTucController : Controller
    {
        private TEMIS_systemEntities db = new TEMIS_systemEntities();
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();

        // GET: Admin/TinTuc
        public ActionResult Index()
        {
            List<DanhMucTinTuc> listDanhMuc = _unitOfWork.GetRepositoryInstance<DanhMucTinTuc>().GetAllRecords().ToList();
            ViewBag.listDanhMuc = listDanhMuc;
            return View();
        }

        public async Task<ActionResult> LoadData(int danhmucid = 0)
        {
            try
            {
                List<TinTuc> list_data = new List<TinTuc>();

                if (danhmucid > 0)
                {
                    list_data = _unitOfWork.GetRepositoryInstance<TinTuc>().GetListByParameter(o => o.DanhMuc == danhmucid).OrderByDescending(x => x.Id).ToList();
                }
                else
                {
                    list_data = _unitOfWork.GetRepositoryInstance<TinTuc>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
                }

                if (list_data.Count == 0)
                {
                    TempData["message"] = "Không tìm thấy kết quả nào";
                }
                List<Models.TinTucViewModel> listTin = new List<Models.TinTucViewModel>();
                foreach (var item in list_data)
                {
                    TinTucViewModel tin = new TinTucViewModel();
                    tin.Id = int.Parse(item.Id.ToString());
                    tin.TieuDe = item.TieuDe;
                    tin.MoTaNgan = item.MoTaNgan;
                    tin.AnhDaiDien = item.AnhDaiDien;
                    tin.NoiDung = item.NoiDung;
                    tin.CreateDate = item.CreatedAt != null ? DateTime.Parse(item.CreatedAt.ToString()) : DateTime.Now;
                    tin.DanhMucId = item.DanhMuc != null ? int.Parse(item.DanhMuc.ToString()) : 0;
                    tin.TenDanhMuc = tin.DanhMucId > 0 ? _unitOfWork.GetRepositoryInstance<DanhMucTinTuc>().GetFirstOrDefaultByParameter(x => x.Id == item.DanhMuc).TenDanhMuc : "";
                    tin.Status = bool.Parse(item.Status.ToString());
                    listTin.Add(tin);
                }
                return PartialView("_PartialListData", listTin);

            }
            catch (Exception ex)
            {
                string mss = ex.Message;
                throw;
            }

        }

        // GET: Admin/TinTuc/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TinTuc tinTuc = db.TinTuc.Find(id);
            if (tinTuc == null)
            {
                return HttpNotFound();
            }
            TinTucViewModel tin = new TinTucViewModel();
            tin.Id = int.Parse(tinTuc.Id.ToString());
            tin.TieuDe = tinTuc.TieuDe;
            tin.MoTaNgan = tinTuc.MoTaNgan;
            tin.AnhDaiDien = tinTuc.AnhDaiDien;
            tin.NoiDung = tinTuc.NoiDung;
            tin.CreateDate = tinTuc.CreatedAt != null ? DateTime.Parse(tinTuc.CreatedAt.ToString()) : DateTime.Now;
            tin.DanhMucId = tinTuc.DanhMuc != null ? int.Parse(tinTuc.DanhMuc.ToString()) : 0;
            tin.TenDanhMuc = tin.DanhMucId > 0 ? _unitOfWork.GetRepositoryInstance<DanhMucTinTuc>().GetFirstOrDefaultByParameter(x => x.Id == tinTuc.DanhMuc).TenDanhMuc : "";
            tin.Status = bool.Parse(tinTuc.Status.ToString());
            return View(tin);
        }

        // GET: Admin/TinTuc/Create
        public ActionResult Create()
        {
            List<DanhMucTinTuc> listDanhMuc = _unitOfWork.GetRepositoryInstance<DanhMucTinTuc>().GetAllRecords().ToList();
            ViewBag.listDanhMuc = listDanhMuc;
            return View();
        }


        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.TinTucModel viewModel)
        {
            TinTuc tintuc = new TinTuc();
            HttpPostedFileBase file = Request.Files["files"];
            if (file.FileName != "" && file.FileName != null)
            {
                string filename = DateTime.Now.ToString("hh_ss_dd_MM_yyy_") + file.FileName;
                string path = Server.MapPath("~/Images/TinTuc/" + filename);
                file.SaveAs(path);
                tintuc.AnhDaiDien = filename;
            }

            if (ModelState.IsValid)
            {
                tintuc.TieuDe = viewModel.TieuDe;
                tintuc.MoTaNgan = viewModel.MoTaNgan;
                tintuc.NoiDung = viewModel.NoiDung;
                tintuc.CreatedAt = DateTime.Now;
                tintuc.Status = false;
                tintuc.DanhMuc = viewModel.DanhMuc;
                _unitOfWork.GetRepositoryInstance<TinTuc>().Add(tintuc);
                _unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        // GET: Admin/TinTuc/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TinTuc tinTuc = db.TinTuc.Find(id);
            if (tinTuc == null)
            {
                return HttpNotFound();
            }

            List<DanhMucTinTuc> listDanhMuc = _unitOfWork.GetRepositoryInstance<DanhMucTinTuc>().GetAllRecords().ToList();
            ViewBag.listDanhMuc = listDanhMuc;

            return View(tinTuc);
        }


        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Models.TinTucModel viewModel)
        {
            var tintuc = db.TinTuc.Find(viewModel.Id);
            HttpPostedFileBase file = Request.Files["files"];
            if (file.FileName != null && file.FileName != "")
            {
                string filename = DateTime.Now.ToString("hh_ss_dd_MM_yyy_") + file.FileName;
                string path = Server.MapPath("~/Images/TinTuc/" + filename);

                if (tintuc.AnhDaiDien != null)
                {
                    string removeImage = tintuc.AnhDaiDien;
                    System.IO.File.Delete(Server.MapPath("~/Images/TinTuc/" + removeImage));
                }

                file.SaveAs(path);
                tintuc.AnhDaiDien = filename;
            }
            tintuc.TieuDe = viewModel.TieuDe;
            tintuc.MoTaNgan = viewModel.MoTaNgan;
            tintuc.NoiDung = viewModel.NoiDung;
            tintuc.UpdatedAt = DateTime.Now;
            tintuc.DanhMuc = viewModel.DanhMuc;
            _unitOfWork.GetRepositoryInstance<TinTuc>().Update(tintuc);
            _unitOfWork.SaveChanges();

            return RedirectToAction("Index");
        }
        
        public async Task<JsonResult> Xoa(long id)
        {
            try
            {
                TinTuc tinTuc = new TinTuc();
                tinTuc = _unitOfWork.GetRepositoryInstance<TinTuc>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (tinTuc != null)
                {
                    if (tinTuc.AnhDaiDien != null)
                    {
                        string removeImage = tinTuc.AnhDaiDien;
                        System.IO.File.Delete(Server.MapPath("~/Images/TinTuc/" + removeImage));
                    }

                    _unitOfWork.GetRepositoryInstance<TinTuc>().Remove(tinTuc);
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

        public async Task<JsonResult> Duyet(long id)
        {
            try
            {
                TinTuc tinTuc = new TinTuc();
                tinTuc = _unitOfWork.GetRepositoryInstance<TinTuc>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (tinTuc != null)
                {
                    tinTuc.Status = true;
                    _unitOfWork.GetRepositoryInstance<TinTuc>().Update(tinTuc);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    TempData["error"] = "bản ghi không tồn tại";
                    return Json("bản ghi không tồn tại", JsonRequestBehavior.AllowGet);
                }

                TempData["message"] = "Duyệt thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi duyệt tin: " + ex.Message;
                return Json("Lỗi duyệt tin", JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> HuyDuyet(long id)
        {
            try
            {
                TinTuc tinTuc = new TinTuc();
                tinTuc = _unitOfWork.GetRepositoryInstance<TinTuc>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (tinTuc != null)
                {
                    tinTuc.Status = false;
                    _unitOfWork.GetRepositoryInstance<TinTuc>().Update(tinTuc);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    TempData["error"] = "bản ghi không tồn tại";
                    return Json("bản ghi không tồn tại", JsonRequestBehavior.AllowGet);
                }

                TempData["message"] = "Hủy duyệt thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi duyệt tin: " + ex.Message;
                return Json("Lỗi duyệt tin", JsonRequestBehavior.AllowGet);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
