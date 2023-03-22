using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TEMIS.CMS.Repository;
using TEMIS.Model;
using System.IO;
using TEMIS.CMS.Common;
using CoreAPI.Entity;

namespace TEMIS.CMS.Areas.Admin.Controllers
{
    [AuditAction]
    public class QuanLyBieuMauController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        //public string urlFile = "E:\\LamViec\\Project_HNEU\\Soure Code\\QLSDT\\QLSDH.CMS\\theme_admin\\FileBieuMau\\";
        public string urlFile = "theme_admin\\FileBieuMau\\";

        // GET: Admin/QuanLyBieuMau
        public ActionResult Index()
        {
            var lstBieumau = _unitOfWork.GetRepositoryInstance<BieuMau>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
            return View(lstBieumau);
        }

        // GET: Admin/QuanLyBieuMau/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BieuMau bieuMau = _unitOfWork.GetRepositoryInstance<BieuMau>().GetFirstOrDefaultByParameter(x => x.Id == id);
            if (bieuMau == null)
            {
                return HttpNotFound();
            }
            return View(bieuMau);
        }

        // GET: Admin/QuanLyBieuMau/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/QuanLyBieuMau/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Template,FileUrl")] BieuMau bieuMau)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["files"];
                string namefile = string.Empty;
                if (file != null && !string.IsNullOrEmpty(file.FileName))
                {
                    namefile = Server.MapPath("~/" + urlFile + file.FileName);

                    //namefile = Path.Combine(Server.MapPath(urlFile), file.FileName);
                    namefile = Server.MapPath("~/" + urlFile + file.FileName);

                    file.SaveAs(namefile);

                    bieuMau.FileUrl = file.FileName;
                    bieuMau.UpdatedAt = DateTime.Now;
                    bieuMau.CreatedAt = DateTime.Now;
                    var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                    bieuMau.UpdatedBy = user.Username;
                    bieuMau.CreatedBy = user.Username;
                    _unitOfWork.GetRepositoryInstance<BieuMau>().Add(bieuMau);
                    _unitOfWork.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(bieuMau);
        }

        // GET: Admin/QuanLyBieuMau/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BieuMau bieuMau = _unitOfWork.GetRepositoryInstance<BieuMau>().GetFirstOrDefaultByParameter(x => x.Id == id);
            if (bieuMau == null)
            {
                return HttpNotFound();
            }
            return View(bieuMau);
        }

        // POST: Admin/QuanLyBieuMau/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Template,FileUrl")] BieuMau bieuMau, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["fupload"];
                string namefile = string.Empty;
                if (file != null && !string.IsNullOrEmpty(file.FileName))
                {
                    // delete file 
                    if (System.IO.File.Exists("~/" + urlFile + bieuMau.FileUrl))
                    {
                        System.IO.File.Delete("~/" + urlFile + bieuMau.FileUrl);
                    }
                    // upload file mới
                    namefile = Server.MapPath("~/" + urlFile + file.FileName);
                    //namefile = Path.Combine(Server.MapPath(urlFile), file.FileName);

                    file.SaveAs(namefile);

                    bieuMau.FileUrl = file.FileName;

                    var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                    bieuMau.UpdatedBy = user.Username;
                    bieuMau.UpdatedAt = DateTime.Now;
                }

                _unitOfWork.GetRepositoryInstance<BieuMau>().Update(bieuMau);
                _unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bieuMau);
        }

        public JsonResult XoaBieuMau(int id)
        {
            string mesage = "";
            try
            {
                BieuMau bieumau = _unitOfWork.GetRepositoryInstance<BieuMau>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (bieumau != null)
                {
                    // xóa file
                    if (System.IO.File.Exists("~/" + urlFile + bieumau.FileUrl))
                    {
                        System.IO.File.Delete("~/" + urlFile + bieumau.FileUrl);
                    }
                    _unitOfWork.GetRepositoryInstance<BieuMau>().Remove(bieumau);
                    _unitOfWork.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                mesage = "Lỗi xóa bản ghi: " + ex.Message;
            }

            return Json(mesage, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
