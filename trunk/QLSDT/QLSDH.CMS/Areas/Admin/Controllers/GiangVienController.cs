using CoreAPI.Entity;
using Excel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TEMIS.CMS.Areas.Admin.Models;
using TEMIS.CMS.Common;
using TEMIS.CMS.Models;
using TEMIS.CMS.Repository;
using TEMIS.Model;
using TEMIS.CMS.Common;
using TEMIS.Model;

namespace TEMIS.CMS.Areas.Admin.Controllers
{
    [AuditAction]
    public class GiangVienController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        private Model.TEMIS_systemEntities db = new Model.TEMIS_systemEntities();

        public GiangVienController()
        {
        }
        // GET: Admin/GiangVien
        public async Task<ActionResult> Index()
        {
            List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = listKhoa;

            return View();
        }

        public async Task<ActionResult> GetListGiangVien(int khoaid)
        {
            List<GiangVienAPI> list = await CoreAPI.CoreAPI.GetListGiangVien(khoaid);
            List<ChucDanhAPI> listChucDanh = await CoreAPI.CoreAPI.GetListChucDanh();
            List<HocHamHocViAPI> listHocHamHocViAPI = await CoreAPI.CoreAPI.GetListHocHamHocVi();
            List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
            List<GiangVienViewModel> listGiangVien = new List<GiangVienViewModel>();
            GiangVienViewModel giangvien;
            int i = 1;
            foreach (GiangVienAPI item in list)
            {
                GiangVienDetailAPI gv = await CoreAPI.CoreAPI.GetThongTinGiangVien(item.Id);
                giangvien = new GiangVienViewModel();
                giangvien.STT = i;
                giangvien.HoTen = item.Name;
                giangvien.NgaySinh = gv.Birthday;
                giangvien.GioiTinh = gv.Gender;
                giangvien.NoiSinh = "";
                giangvien.HoKhau = "";
                giangvien.ThongTinLienLac = "";
                if(listChucDanh.Where(o => o.Id == item.TitleId).SingleOrDefault() != null)
                {
                    giangvien.ChucDanh = listChucDanh.Where(o => o.Id == item.TitleId).SingleOrDefault().Name;
                }
                else
                {
                    giangvien.ChucDanh = "Khác";
                }
                if(listHocHamHocViAPI.Where(o => o.Id == item.DegreeId).SingleOrDefault() != null)
                {
                    giangvien.HocHamHocVi = listHocHamHocViAPI.Where(o => o.Id == item.DegreeId).SingleOrDefault().Name;
                }
                else
                {
                    giangvien.HocHamHocVi = "Khác";
                }
                if (listKhoa.Where(o => o.Id == item.DepartmentId).SingleOrDefault()!= null)
                {
                    giangvien.KHoa = listKhoa.Where(o => o.Id == item.DepartmentId).SingleOrDefault().Name;
                }
                else
                {
                    giangvien.KHoa = "Khác";
                }
                listGiangVien.Add(giangvien);

                i = i + 1;
            }

            return PartialView("patialListGiangVien", listGiangVien);
        }

        public async Task<ActionResult> ThemMoiUser()
        {
            List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = listKhoa;

            List<HocHamHocViAPI> listHocHamHocVi = await CoreAPI.CoreAPI.GetListHocHamHocVi();
            ViewBag.ListHocHamHocVi = listHocHamHocVi;

            List<ChucDanhAPI> listChucDanh = await CoreAPI.CoreAPI.GetListChucDanh();
            ViewBag.ListChucDanh = listChucDanh;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ThemMoiUser(FormCollection form)
        {
            try
            {
                Model.GiangVien gv = new Model.GiangVien();
                gv.Code = form["Code"];
                gv.HoTen = form["HoTen"];
                gv.NgaySinh = DateTime.Parse(form["NgaySinh"].ToString());
                gv.GioiTinh = form["GioiTinh"];
                gv.NoiSinh = form["NoiSinh"];
                gv.HoKhau = form["HoKhau"];
                gv.DiaChi = form["DiaChi"];
                gv.SoDienThoai = form["SoDienThoai"];
                gv.Email = form["Email"];
                gv.ChucDanhId = form["ChucDanh"] != null ? int.Parse(form["ChucDanh"]) : 0;
                gv.KhoaId = form["Khoa"] != null ? int.Parse(form["Khoa"]) : 0;
                gv.HocHamHocViId = form["HocHamHocVi"] != null ? int.Parse(form["HocHamHocVi"]) : 0;
                db.GiangVien.Add(gv);
                db.SaveChanges();
                TempData["message"] = "Thêm mới thành công";
                return RedirectToAction("GiangVienNgoaiTruong");

                #region save
                //var user = new ApplicationUser
                //{
                //    UserName = form["Code"],
                //    TwoFactorEnabled = true,
                //    Email = form["Email"],
                //    PrivateKey = TimeSensitivePassCode.GeneratePresharedKey()
                //};


                //var result = await UserManager.CreateAsync(user, "123456");
                //if (result.Succeeded)
                //{
                //    UserManager.AddToRole(user.Id, PublicConstant.ROLE_GIANG_VIEN_HD.ToString());
                //    TempData["message"] = "Thêm mới thành công";
                //    return RedirectToAction("Index", "GiangVien");
                //}
                //else
                //{
                //    TempData["error"] = "Lỗi tạo tài khoản";
                //}
                #endregion
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi mhêm mới";
                string mss = ex.Message;
            }
            return View();
        }

        public ActionResult UpLoadExcel()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> UploadExcelFile(HttpPostedFileBase upload)
        {
            string error = "";
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    try
                    {
                        Stream stream = upload.InputStream;
                        IExcelDataReader reader = null;

                        if (upload.FileName.EndsWith(".xls"))
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else if (upload.FileName.EndsWith(".xlsx"))
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        else
                        {
                            ModelState.AddModelError("File", "File không đúng địng dạng");
                            TempData["error"] = "File không đúng địng dạng";
                            return View();
                        }

                        reader.IsFirstRowAsColumnNames = true;

                        DataSet ds = reader.AsDataSet();
                        DataTable dt = ds.Tables[0];
                        string fileExelName = upload.FileName;
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows.Count < 2)
                            {
                                TempData["error"] = "File excel phải có ít nhất từ 2 bản ghi trở lên";
                                return RedirectToAction("GiangVienNgoaiTruong");
                            }

                            if (dt.Rows.Count > 1000)
                            {
                                TempData["error"] = "Số lượng tối đa 1000 bản ghi trong mỗi lần up";
                                return RedirectToAction("GiangVienNgoaiTruong");
                            }

                            string userName = User.Identity.GetUserName();

                            TEMIS.Model.GiangVien gv;
                            List<ImportExelFail> listError = new List<ImportExelFail>();
                            List<ImportExelFail> listSuccess = new List<ImportExelFail>();
                            ImportExelFail importFail = new ImportExelFail();
                            ImportExelFail importSuccess = new ImportExelFail();
                            foreach (DataRow row in dt.Rows)
                            {
                                try
                                {
                                        gv = new Model.GiangVien();
                                        gv.HoTen = row.ItemArray[1].ToString();
                                        gv.NgaySinh = DateTime.ParseExact(row.ItemArray[2].ToString(), "dd/MM/yyyy", CultureInfo.CurrentCulture);
                                        gv.GioiTinh = row.ItemArray[3].ToString();
                                        gv.NoiSinh = row.ItemArray[4].ToString();
                                        gv.HoKhau = row.ItemArray[5].ToString();
                                        gv.DiaChi = row.ItemArray[6].ToString();
                                        gv.SoDienThoai = row.ItemArray[7].ToString();
                                        gv.Email = row.ItemArray[8].ToString();
                                        string tenkhoa = row.ItemArray[9].ToString().ToLower();
                                        List<OrganizationInfo> list = await CoreAPI.CoreAPI.GetListKhoa();
                                        foreach (OrganizationInfo item in list)
                                        {
                                            if (item.Name.ToLower().Equals(tenkhoa))
                                            {
                                                gv.KhoaId = item.Id;
                                                break;
                                            }
                                        }

                                        string chucdanh = row.ItemArray[10].ToString().ToLower();
                                        List<ChucDanhAPI> listChucDanh = await CoreAPI.CoreAPI.GetListChucDanh();
                                        foreach (ChucDanhAPI item in listChucDanh)
                                        {
                                            if (item.Name.ToLower().Equals(chucdanh))
                                            {
                                                gv.ChucDanhId = item.Id;
                                                break;
                                            }
                                        }

                                        string hochamhocvi = row.ItemArray[11].ToString().ToLower();
                                        List<HocHamHocViAPI> listHocHamHocVi = await CoreAPI.CoreAPI.GetListHocHamHocVi();
                                        foreach (HocHamHocViAPI item in listHocHamHocVi)
                                        {
                                            if (item.Name.ToLower().Equals(hochamhocvi))
                                            {
                                                gv.HocHamHocViId = item.Id;
                                                break;
                                            }
                                        }

                                        if (gv.KhoaId != null && gv.ChucDanhId != null && gv.HocHamHocViId != null)
                                        {
                                            _unitOfWork.GetRepositoryInstance<TEMIS.Model.GiangVien>().Add(gv);
                                            _unitOfWork.SaveChanges();
                                        }
                                        else
                                        {
                                            importFail = new ImportExelFail();
                                            importFail.HoTen = gv.HoTen;
                                            importFail.STT = row.ItemArray[0].ToString();
                                            if (gv.KhoaId == null)
                                            {
                                                importFail.Message = "Khoa không đúng!";
                                            }
                                            else if (gv.ChucDanhId == null)
                                            {
                                                importFail.Message = "Chức danh không đúng!";
                                            }
                                            else
                                            {
                                                importFail.Message = "Học hàm học vị không đúng!";
                                            }
                                            listError.Add(importFail);
                                        }
                                       
                                }
                                catch (Exception ex)
                                {
                                    //TempData["error"] = "File dữ liệu không phù hợp với bản mẫu!";
                                    importFail = new ImportExelFail();
                                    importFail.HoTen = row.ItemArray[1].ToString();
                                    importFail.STT = row.ItemArray[0].ToString();
                                    importFail.Message = "Lỗi dữ liệu không phù hợp!";
                                    listError.Add(importFail);
                                    continue;
                                }
                            }
                            TempData["listError"] = null;
                            TempData["listError"] = listError;

                            TempData["message"] = "Import danh sách đơn hàng thành công!";
                            reader.Close();
                            return RedirectToAction("UpLoadExcel");
                        }
                        else
                        {
                            TempData["error"] = "File không có dữ liệu!";
                        }
                        reader.Close();
                        return RedirectToAction("UpLoadExcel");
                    }
                    catch (Exception ex)
                    {
                        TempData["error"] = "Lỗi xảy ra trong quá trình import file excel:" + ex.Message;
                        return RedirectToAction("GiangVienNgoaiTruong");
                    }
                }
                else
                {
                    TempData["error"] = "File không có dữ liệu !";
                }
            }
            return RedirectToAction("GiangVienNgoaiTruong");
        }


        public ActionResult DongBoGiangVien()
        {
            return View();
        }

        #region Đồng bộ save
        //[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async System.Threading.Tasks.Task<ActionResult> DongBoGiangVien(HttpPostedFileBase upload)
        //{
        //    List<Admin.Models.GiangVienAPI> listData = await GetListGiangVien();
        //    if (listData.Count > 0)
        //    {
        //        List<OrganizationInfo> listKhoa = await GetListKhoa();
        //        List<ChucDanhAPI> listChucDanh = await GetListChucDanh();
        //        Model.GiangVien gv = new Model.GiangVien();
        //        ThongTinCanBoAPI thongtin = new ThongTinCanBoAPI();

        //        foreach (var item in listData)
        //        {
        //            gv = db.GiangViens.Where(o => o.Code == item.Code).SingleOrDefault();
        //            if (gv == null) // giảng viên chưa có trong db -> đồng bộ về
        //            {
        //                thongtin = new ThongTinCanBoAPI();
        //                thongtin = await GetThongTinCanBo(item.Id);
        //                if (thongtin != null)
        //                {
        //                    gv = new Model.GiangVien();
        //                    gv.Code = item.Code;
        //                    gv.HoTen = item.Name;
        //                    gv.NgaySinh = DateTime.Parse(thongtin.Birthday.ToString());
        //                    gv.GioiTinh = item.Gender;
        //                    gv.ChucDanhId = listChucDanh.Where(o => o.Id == item.TitleId).SingleOrDefault().Id;
        //                    gv.KhoaId = listKhoa.Where(o => o.Id == item.DepartmentId).SingleOrDefault().Id;
        //                    db.GiangViens.Add(gv);
        //                    db.SaveChanges();
        //                }
        //            }
        //        }
        //    }

        //    TempData["message"] = "Đồng bộ dữ liệu hoàn tất";
        //    return RedirectToAction("Index", "GiangVien");
        //}

        //public async Task<List<Admin.Models.GiangVienAPI>> GetListGiangVien()
        //{
        //    List<Admin.Models.GiangVienAPI> listData = new List<Admin.Models.GiangVienAPI>();
        //    using (var client = new HttpClient())
        //    {
        //        try
        //        {
        //            string url = string.Empty;
        //            url = PublicConstant.BASE_URL_API + "/api_v2/staff/getstave?departmentid=0";
        //            HttpResponseMessage request = await client.GetAsync(url);
        //            if (request.IsSuccessStatusCode)
        //            {
        //                var json = await request.Content.ReadAsStringAsync();
        //                clsRespon response = JsonConvert.DeserializeObject<clsRespon>(json);
        //                if (response.success == "true")
        //                {
        //                    listData = JsonConvert.DeserializeObject<List<Admin.Models.GiangVienAPI>>(response.data.ToString());
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //    }

        //    return listData;
        //}
        //public async Task<ThongTinCanBoAPI> GetThongTinCanBo(int Id)
        //{
        //    ThongTinCanBoAPI thongtincanbo = new ThongTinCanBoAPI();
        //    using (var client = new HttpClient())
        //    {
        //        try
        //        {
        //            string url = string.Empty;
        //            url = PublicConstant.BASE_URL_API + "/api_v2/staff/getstaff?id=" + Id;
        //            HttpResponseMessage request = await client.GetAsync(url);
        //            if (request.IsSuccessStatusCode)
        //            {
        //                var json = await request.Content.ReadAsStringAsync();
        //                clsRespon response = JsonConvert.DeserializeObject<clsRespon>(json);
        //                if (response.success == "true")
        //                {
        //                    thongtincanbo = JsonConvert.DeserializeObject<ThongTinCanBoAPI>(response.data.ToString());
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //    }

        //    return thongtincanbo;
        //}
        //public async Task<List<Admin.Models.ChucDanhAPI>> GetListChucDanh()
        //{
        //    List<ChucDanhAPI> listChucDanh = new List<ChucDanhAPI>();
        //    using (var client = new HttpClient())
        //    {
        //        try
        //        {
        //            string url = string.Empty;
        //            url = PublicConstant.BASE_URL_API + "/api_v2/staff/getTitles";
        //            HttpResponseMessage request = await client.GetAsync(url);
        //            if (request.IsSuccessStatusCode)
        //            {
        //                var json = await request.Content.ReadAsStringAsync();
        //                clsRespon response = JsonConvert.DeserializeObject<clsRespon>(json);
        //                if (response.success == "true")
        //                {
        //                    listChucDanh = JsonConvert.DeserializeObject<List<ChucDanhAPI>>(response.data.ToString());
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //    }

        //    return listChucDanh;
        //}
        //public async Task<List<Admin.Models.OrganizationInfo>> GetListKhoa()
        //{
        //    List<Admin.Models.OrganizationInfo> listKhoa = new List<Admin.Models.OrganizationInfo>();
        //    using (var client = new HttpClient())
        //    {
        //        try
        //        {
        //            string url = string.Empty;
        //            url = PublicConstant.BASE_URL_API + "/api_v2/organize/getfaculties";
        //            HttpResponseMessage request = await client.GetAsync(url);
        //            if (request.IsSuccessStatusCode)
        //            {
        //                var json = await request.Content.ReadAsStringAsync();
        //                clsRespon response = JsonConvert.DeserializeObject<clsRespon>(json);
        //                if (response.success == "true")
        //                {
        //                    listKhoa = JsonConvert.DeserializeObject<List<Admin.Models.OrganizationInfo>>(response.data.ToString());
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //    }

        //    return listKhoa;
        //}

        #endregion

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                Model.GiangVien giangvien = _unitOfWork.GetRepositoryInstance<Model.GiangVien>().GetFirstOrDefaultByParameter(x => x.Id == id);
                if (giangvien != null)
                {

                    List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
                    ViewBag.ListKhoa = listKhoa;

                    List<HocHamHocViAPI> listHocHamHocVi = await CoreAPI.CoreAPI.GetListHocHamHocVi();
                    ViewBag.ListHocHamHocVi = listHocHamHocVi;

                    List<ChucDanhAPI> listChucDanh = await CoreAPI.CoreAPI.GetListChucDanh();
                    ViewBag.ListChucDanh = listChucDanh;
                    return View(giangvien);
                }
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(FormCollection form)
        {
            Model.GiangVien gv = new Model.GiangVien();
            try
            {
                int id = int.Parse(form["hdfId"]);
                gv = _unitOfWork.GetRepositoryInstance<Model.GiangVien>().GetFirstOrDefaultByParameter(x => x.Id == id);
                gv.Code = form["Code"];
                gv.HoTen = form["HoTen"];
                gv.NgaySinh = DateTime.Parse(form["NgaySinh"].ToString());
                gv.GioiTinh = form["GioiTinh"];
                gv.NoiSinh = form["NoiSinh"];
                gv.HoKhau = form["HoKhau"];
                gv.DiaChi = form["DiaChi"];
                gv.SoDienThoai = form["SoDienThoai"];
                gv.Email = form["Email"];
                gv.ChucDanhId = form["ChucDanh"] != null ? int.Parse(form["ChucDanh"]) : 0;
                gv.KhoaId = form["Khoa"] != null ? int.Parse(form["Khoa"]) : 0;
                gv.HocHamHocViId = form["HocHamHocVi"] != null ? int.Parse(form["HocHamHocVi"]) : 0;
                _unitOfWork.GetRepositoryInstance<Model.GiangVien>().Update(gv);
                _unitOfWork.SaveChanges();
                TempData["message"] = "Cập nhật thông tin thành công";
                return RedirectToAction("GiangVienNgoaiTruong");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi cập nhật thông tin: " + ex.Message;
            }
            return View(gv);
        }

        public ActionResult ThongTinKhoaHoc(string magiaovien)
        {
            if (magiaovien == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var magiaovienAgument = new SqlParameter("magiangvien", System.Data.SqlDbType.NVarChar) { Value = magiaovien };
            //SP_ThongTinKHoaHocGiaVien_Result info = _unitOfWork.GetRepositoryInstance<SP_ThongTinKHoaHocGiaVien_Result>().GetResultBySqlProcedure("SP_ThongTinKHoaHocGiaVien @magiangvien", magiaovienAgument).SingleOrDefault();

            //return View(info);
            return View();
        }
        public async Task<JsonResult> Xoa(long id)
        {
            try
            {
                Model.GiangVien gv = new Model.GiangVien();
                gv = _unitOfWork.GetRepositoryInstance<Model.GiangVien>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (gv != null)
                {
                    _unitOfWork.GetRepositoryInstance<Model.GiangVien>().Remove(gv);
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

        public async Task<ActionResult> GiangVienNgoaiTruong()
        {
            List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = listKhoa;

            return View();
        }

        public async Task<ActionResult> GetListGiangVienNgoaiTruong(int khoaid)
        {
            List<Model.GiangVien> listgv = khoaid > 0 ? _unitOfWork.GetRepositoryInstance<Model.GiangVien>().GetListByParameter(o => o.KhoaId == khoaid).ToList() : _unitOfWork.GetRepositoryInstance<Model.GiangVien>().GetAllRecords().ToList();
            List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
            List<ChucDanhAPI> listChucDanh = await CoreAPI.CoreAPI.GetListChucDanh();
            List<HocHamHocViAPI> listHocHamHocVi = await CoreAPI.CoreAPI.GetListHocHamHocVi();
            ViewBag.ListGV = listgv;
            List<GiangVienViewModel> listGiangVien = new List<GiangVienViewModel>();
            GiangVienViewModel gv = new GiangVienViewModel();
            int i = 1;
            foreach (var item in listgv)
            {
                gv = new GiangVienViewModel();
                gv.Id = int.Parse(item.Id.ToString());
                gv.STT = i;
                gv.HoTen = item.HoTen;
                gv.NgaySinh = DateTime.Parse(item.NgaySinh.ToString());
                gv.GioiTinh = item.GioiTinh;
                gv.NoiSinh = item.NoiSinh;
                gv.HoKhau = item.HoKhau;
                gv.ThongTinLienLac = item.SoDienThoai + "," + item.Email;
                if(item.ChucDanhId != null)
                {
                    gv.ChucDanh = listChucDanh.Count > 0 ? listChucDanh.Where(o => o.Id == item.ChucDanhId).SingleOrDefault().Name : "";
                }
                if (item.KhoaId != null)
                {
                    gv.KHoa = listKhoa.Count > 0 ? listKhoa.Where(o => o.Id == item.KhoaId).SingleOrDefault().Name : "";
                }
                if (item.HocHamHocViId != null)
                {
                    gv.HocHamHocVi = listHocHamHocVi.Count > 0 ? listHocHamHocVi.Where(o => o.Id == item.HocHamHocViId).SingleOrDefault().Name : "";
                }
                listGiangVien.Add(gv);
                i = i + 1;
            }

            return PartialView("patialListGiangVienNgoaiTruong", listGiangVien);
        }
        public async Task<ActionResult> HocPhanGiangVien()
        {
            List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = listKhoa;

            return View();
        }
        public async Task<ActionResult> GetHocPhan_GV(int khoaid)
        {
            List<Model.GiangVien> listgv = _unitOfWork.GetRepositoryInstance<Model.GiangVien>().GetListByParameter(o => o.KhoaId == khoaid).ToList();
            List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
            List<ChucDanhAPI> listChucDanh = await CoreAPI.CoreAPI.GetListChucDanh();
            List<HocHamHocViAPI> listHocHamHocVi = await CoreAPI.CoreAPI.GetListHocHamHocVi();
            ViewBag.ListGV = listgv;
            List<GiangVienViewModel> listGiangVien = new List<GiangVienViewModel>();
            GiangVienViewModel gv = new GiangVienViewModel();
            int i = 1;
            foreach (var item in listgv)
            {
                gv = new GiangVienViewModel();
                gv.Id = int.Parse(item.Id.ToString());
                gv.STT = i;
                gv.HoTen = item.HoTen;
                gv.NgaySinh = DateTime.Parse(item.NgaySinh.ToString());
                gv.GioiTinh = item.GioiTinh;
                gv.NoiSinh = item.NoiSinh;
                gv.HoKhau = item.HoKhau;
                gv.ThongTinLienLac = item.SoDienThoai + "," + item.Email;
                gv.ChucDanh = listChucDanh.Count > 0 ? listChucDanh.Where(o => o.Id == item.ChucDanhId).SingleOrDefault().Name : "";
                gv.KHoa = listKhoa.Count > 0 ? listKhoa.Where(o => o.Id == item.KhoaId).SingleOrDefault().Name : "";
                gv.HocHamHocVi = listHocHamHocVi.Count > 0 ? listHocHamHocVi.Where(o => o.Id == item.HocHamHocViId).SingleOrDefault().Name : "";
                listGiangVien.Add(gv);
                i = i + 1;
            }

            List<HocPhanGiangVienViewModel> result = new List<HocPhanGiangVienViewModel>();

            var listHocPhan = _unitOfWork.GetRepositoryInstance<HocPhan>().GetListByParameter(o => (khoaid == 0 || o.KhoaId == khoaid)).OrderByDescending(o => o.Id).ToList();
            foreach (var item in listHocPhan)
            {
                HocPhanGiangVienViewModel viewModel = new HocPhanGiangVienViewModel();
                var hocphangv = _unitOfWork.GetRepositoryInstance<HocPhan_GiangVien>().GetFirstOrDefaultByParameter(x => x.IdHocPhan == item.Id);
                if (hocphangv != null)
                {
                    int hochamhocvi = 0;
                    if (hocphangv.StaffId == 0)
                    {
                        var giangvienngoaitruong = _unitOfWork.GetRepositoryInstance<TEMIS.Model.GiangVien>().GetFirstOrDefaultByParameter(x => x.Id == hocphangv.IdGiangVien);
                        if (giangvienngoaitruong != null)
                        {
                            hochamhocvi = giangvienngoaitruong.HocHamHocViId.Value;
                            viewModel.TenGiangVien = giangvienngoaitruong.HoTen;
                            viewModel.HocHamHocVi = listHocHamHocVi.Count > 0 ? listHocHamHocVi.Where(o => o.Id == hochamhocvi).SingleOrDefault().Name : "";
                            viewModel.IdGiangVien = (int)giangvienngoaitruong.Id;
                            viewModel.StaffId = 0;
                        }
                    }
                    else
                    {
                        //var gvtrongtruong = listGiangVien.Where(o => o.Id == hocphangv.StaffId).FirstOrDefault();
                        int staffid = hocphangv.StaffId.Value;
                        var gvtrongtruong = await CoreAPI.CoreAPI.GetThongTinGiangVien(staffid);
                        if (gvtrongtruong != null)
                        {
                            hochamhocvi = gvtrongtruong.DegreeId;
                            viewModel.TenGiangVien = gvtrongtruong.Name;
                            viewModel.HocHamHocVi = listHocHamHocVi.Count > 0 ? listHocHamHocVi.Where(o => o.Id == hochamhocvi).SingleOrDefault().Name : "";
                            viewModel.IdGiangVien = 0;
                            viewModel.StaffId = gvtrongtruong.Id;
                        }
                    }

                }
                else
                {
                    viewModel.TenGiangVien = string.Empty;
                    viewModel.HocHamHocVi = string.Empty;
                    viewModel.IdGiangVien = 0;
                    viewModel.StaffId = 0;
                }
                viewModel.IdHocPhan = item.Id;
                viewModel.TenHocPhan = item.TenHocPhan;
                viewModel.MaHocPhan = item.MaHocPhan;
                result.Add(viewModel);
            }
            return PartialView("_PartialViewHocPhanGV", result);
        }
        public async Task<ActionResult> LoadGVByKhoa(int loaigv, int khoaid)
        {
            string str = "";
            try
            {
                List<GiangVienView> lstGV = new List<GiangVienView>();
                if (loaigv == 0 || loaigv == -1)
                {
                    //Giảng viên trong trường
                    List<GiangVienAPI> listGVtrong = await CoreAPI.CoreAPI.GetListGiangVien(khoaid);
                    List<HocHamHocViAPI> listHocHamHocViAPI = await CoreAPI.CoreAPI.GetListHocHamHocVi();

                    if (listGVtrong.Count > 0)
                    {
                        foreach (var item in listGVtrong)
                        {
                            GiangVienView gv = new GiangVienView();
                            gv.name = item.Name;
                            gv.hocham = listHocHamHocViAPI.Where(o => o.Id == item.DegreeId).SingleOrDefault().Name;
                            gv.staffid = item.Id;
                            gv.idgv = 0;
                            lstGV.Add(gv);
                        }
                    }
                }
                else if (loaigv == 1)
                {
                    //Giảng viên ngoài trường
                    var lstGVngoai = _unitOfWork.GetRepositoryInstance<Model.GiangVien>().GetListByParameter(x => x.KhoaId == khoaid).ToList();
                    if (lstGVngoai.Count > 0)
                    {
                        foreach (var item in lstGVngoai)
                        {
                            GiangVienView gv = new GiangVienView();
                            gv.name = item.HoTen;
                            gv.hocham = item.HocHamHocViId.ToString();
                            gv.staffid = 0;
                            gv.idgv = (int)item.Id;
                            lstGV.Add(gv);
                        }
                    }
                }

                return PartialView("_PartialViewGiangVien", lstGV);
            }
            catch (Exception ex)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(controllerName, actionName, user == null ? string.Empty : user.Username, ex);
            }
            return PartialView("_PartialViewGiangVien", null);
        }
        public async Task<JsonResult> AddGiangVienToHP(int idhp, int khoaid, int staffid, int idgv)
        {
            try
            {
                var hocphan = _unitOfWork.GetRepositoryInstance<HocPhan>().GetFirstOrDefaultByParameter(x => x.Id == idhp);
                if (hocphan != null)
                {
                    string maHP = hocphan.MaHocPhan;
                    var lstncs_hp = _unitOfWork.GetRepositoryInstance<HocPhan_NCS>().GetListByParameter(o => o.MaHocPhan == maHP).ToList();
                    var khoahoc = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetAllRecords().FirstOrDefault();
                    var checkhp_gv = _unitOfWork.GetRepositoryInstance<HocPhan_GiangVien>().GetFirstOrDefaultByParameter(o => o.IdHocPhan == idhp);
                    if (checkhp_gv != null)
                    {
                        if (checkhp_gv.StaffId > 0 || checkhp_gv.IdGiangVien > 0)
                        {
                            TempData["error"] = "Học phần đã được gán cho giảng viên";
                            return Json("error", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        checkhp_gv = new HocPhan_GiangVien();
                        var lstYear = await CoreAPI.CoreAPI.GetYears();
                        var lstHedaotao = await CoreAPI.CoreAPI.GetGrades();
                        var lstDMQuyDoi = await CoreAPI.CoreAPI.GetCategories();
                        var lstQuyDoi = await CoreAPI.CoreAPI.GetConvertions();
                        int yearid = lstYear.LastOrDefault() != null ? lstYear.LastOrDefault().Id : 0;
                        checkhp_gv.YearId = yearid;
                        var lstHocky = await CoreAPI.CoreAPI.GetSemester(yearid);
                        checkhp_gv.GradeId = lstHedaotao.LastOrDefault() != null ? lstHedaotao.LastOrDefault().Id : 0;
                        checkhp_gv.ConversionId = lstQuyDoi.LastOrDefault() != null ? lstQuyDoi.LastOrDefault().Id : 0;
                        checkhp_gv.Departmentid = khoaid;
                        checkhp_gv.ForDepartmentId = khoaid;
                        checkhp_gv.Name = string.Empty;
                        checkhp_gv.LessionTime = 0;
                        checkhp_gv.TeachingTime = string.Empty;
                        checkhp_gv.Class = hocphan.TenHocPhan;
                        checkhp_gv.Size = lstncs_hp.Count;
                        checkhp_gv.Paid = false;
                        checkhp_gv.Course = khoahoc != null ? khoahoc.MaKhoa : "";
                        checkhp_gv.Desc = string.Empty;
                        checkhp_gv.Specializing = string.Empty;
                        checkhp_gv.InvitedPartner = string.Empty;
                        checkhp_gv.LinkedPartner = string.Empty;
                        checkhp_gv.SubjectName = hocphan.TenHocPhan;
                        checkhp_gv.SemesterId = lstHocky.LastOrDefault() != null ? lstHocky.LastOrDefault().Id : 0;
                        checkhp_gv.Status = true;
                        checkhp_gv.IdHocPhan = hocphan.Id;
                        checkhp_gv.KhoaId = khoaid;
                        if (staffid > 0)
                        {
                            checkhp_gv.Invited = false;
                            checkhp_gv.StaffId = staffid;
                            checkhp_gv.IdGiangVien = 0;
                            checkhp_gv.LoaiGiangVien = 0; // giảng viên trong trường

                            var gvTrongTruong = await CoreAPI.CoreAPI.GetThongTinGiangVien(staffid);
                            checkhp_gv.InvitedDegreeId = gvTrongTruong != null ? gvTrongTruong.DegreeId : 0;
                        }
                        else if (idgv > 0)
                        {
                            checkhp_gv.Invited = true;
                            checkhp_gv.StaffId = 0;
                            checkhp_gv.IdGiangVien = idgv;
                            checkhp_gv.LoaiGiangVien = 1; // giảng viên ngoài trường
                            var gvNgoaiTruong = _unitOfWork.GetRepositoryInstance<Model.GiangVien>().GetFirstOrDefaultByParameter(x => x.Id == idgv);
                            checkhp_gv.InvitedDegreeId = gvNgoaiTruong != null ? gvNgoaiTruong.HocHamHocViId : 0;
                        }
                        _unitOfWork.GetRepositoryInstance<HocPhan_GiangVien>().Add(checkhp_gv);
                        _unitOfWork.SaveChanges();
                    }
                }
                else
                {
                    TempData["error"] = "Học phần không tồn tại";
                    return Json("Học phần không tồn tại", JsonRequestBehavior.AllowGet);
                }

                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi gán giảng viên cho học phần: " + ex.Message;
                return Json("Lỗi gán giảng viên cho học phần", JsonRequestBehavior.AllowGet);
            }
        }
    }
}