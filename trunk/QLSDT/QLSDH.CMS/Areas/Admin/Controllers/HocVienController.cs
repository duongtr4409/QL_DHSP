using CoreAPI.Entity;
using Excel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TEMIS.CMS.Areas.Admin.Models;
using TEMIS.CMS.Common;
using TEMIS.CMS.Models;
using TEMIS.CMS.Repository;
using TEMIS.Model;
using TEMIS.CMS.Common;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace TEMIS.CMS.Areas.Admin.Controllers
{
    [AuditAction]
    public class HocVienController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        private Model.TEMIS_systemEntities db = new Model.TEMIS_systemEntities();
        public string parthdowload = "Upload\\FileBMDowload\\";
        //public string url_download = "http://qlncs.hnue.edu.vn/upload/FileBMDowload/";
        public string url_download = "http://192.168.100.23:2806/";
        //link local
        public string url_domain = "http://localhost:50498/";
        // link public
        //public string url_domain = "https://qlsdh.2bsystem.com.vn/";


        public HocVienController()
        {
        }
        public HocVienController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationSignInManager _signInManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        // GET: Admin/HocVien
        public async Task<ActionResult> Index()
        {
            List<OrganizationInfo> list = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = list;
            return View();
        }

        public async Task<ActionResult> ThongKeTuyenSinhNCS()
        {
            List<Model.DanhMucThongTin> lstDanhMucThongTin = _unitOfWork.GetRepositoryInstance<Model.DanhMucThongTin>().GetAllRecords().ToList();
            return View(lstDanhMucThongTin);
        }

        public async Task<ActionResult> GetListHocVien(int khoaid = 0, string mancs = "", string tenncs = "")
        {
            List<Model.NCS> listNCSDb = new List<NCS>();
            if (khoaid > 0)
            {
                listNCSDb = _unitOfWork.GetRepositoryInstance<Model.NCS>().GetListByParameter(o => o.KhoaId == khoaid).OrderBy(o => o.HoTen).ToList();

            }
            else
            {
                listNCSDb = _unitOfWork.GetRepositoryInstance<Model.NCS>().GetAllRecords().OrderBy(o => o.HoTen).ToList();

            }
            if (!string.IsNullOrEmpty(mancs))
            {
                listNCSDb = listNCSDb.Where(x => x.Ma.Contains(mancs)).ToList();
            }
            if (!string.IsNullOrEmpty(tenncs))
            {
                listNCSDb = listNCSDb.Where(x => x.HoTen.Contains(tenncs)).ToList();
            }




            //if (khoaid > 0 && mancs != "" && tenncs != "")
            //{
            //}
            //else if (khoaid > 0 && mancs != "" && tenncs == "")
            //{
            //    listNCSDb = _unitOfWork.GetRepositoryInstance<Model.NCS>().GetListByParameter(o => o.KhoaId == khoaid || o.Ma == mancs).OrderBy(o => o.HoTen).ToList();
            //}
            //else if (khoaid == 0 && mancs != "" && tenncs != "")
            //{
            //    listNCSDb = _unitOfWork.GetRepositoryInstance<Model.NCS>().GetListByParameter(o => o.Ma == mancs || o.HoTen.Contains(tenncs)).OrderBy(o => o.HoTen).ToList();
            //}
            //else if (khoaid > 0 && mancs == "" && tenncs != "")
            //{
            //    listNCSDb = _unitOfWork.GetRepositoryInstance<Model.NCS>().GetListByParameter(o => o.KhoaId == khoaid || o.HoTen.Contains(tenncs)).OrderBy(o => o.HoTen).ToList();
            //}
            //else if (khoaid == 0 && mancs == "" && tenncs == "")
            //{
            //    listNCSDb = _unitOfWork.GetRepositoryInstance<Model.NCS>().GetAllRecords().Take(200).OrderBy(o => o.HoTen).ToList();
            //}

            List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
            List<ChucDanhAPI> listChucDanh = await CoreAPI.CoreAPI.GetListChucDanh();
            ViewBag.ListGV = listNCSDb;
            List<NCSViewModel> listNCS = new List<NCSViewModel>();
            NCSViewModel ncs = new NCSViewModel();
            int i = 1;
            foreach (var item in listNCSDb)
            {
                ncs = new NCSViewModel();
                ncs.Id = int.Parse(item.Id.ToString());
                ncs.STT = i;
                ncs.Ma = item.Ma;
                ncs.HoTen = item.HoTen;
                ncs.NgaySinh = DateTime.Parse(item.NgaySinh.ToString());
                ncs.GioiTinh = item.GioiTinh;
                ncs.NoiSinh = item.NoiSinh;
                ncs.HoKhau = item.HoTen;
                ncs.Email = item.Email;
                ncs.DienThoai = item.DienThoai;
                ncs.ChucDanh = item.ChucDanhId > 0 && listChucDanh.Count > 0 ? listChucDanh.Where(o => o.Id == item.ChucDanhId).SingleOrDefault().Name : "";
                ncs.Khoa = listKhoa.Count > 0 ? listKhoa.Where(o => o.Id == item.KhoaId).SingleOrDefault().Name : "";
                var baove_ncs = _unitOfWork.GetRepositoryInstance<BaoVe_NCS>().GetFirstOrDefaultByParameter(x => x.MaNCS == item.Ma);
                if (baove_ncs == null)
                {
                    ncs.progress = 0;
                }
                else
                {
                    if (baove_ncs.Buoc1 == 1)
                    {
                        //ncs.progress = (float)(100 * 1 / 7);
                        ncs.progress = 1;
                    }
                    if (baove_ncs.Buoc2 == 1)
                    {
                        ncs.progress = (float)(100 * 2 / 7);
                        ncs.progress = 2;
                    }
                    if (baove_ncs.Buoc3 == 1)
                    {
                        ncs.progress = (float)(100 * 3 / 7);
                        ncs.progress = 3;
                    }
                    if (baove_ncs.Buoc4 == 1)
                    {
                        ncs.progress = (float)(100 * 4 / 7);
                        ncs.progress = 4;
                    }
                    if (baove_ncs.Buoc5 == 1)
                    {
                        ncs.progress = (float)(100 * 5 / 7);
                        ncs.progress = 5;
                    }
                    if (baove_ncs.Buoc6 == 1)
                    {
                        ncs.progress = (float)(100 * 6 / 7);
                        ncs.progress = 6;
                    }
                    if (baove_ncs.Buoc7 == 1)
                    {
                        ncs.progress = (float)(100 * 7 / 7);
                        ncs.progress = 7;
                    }
                }
                listNCS.Add(ncs);
                i = i + 1;
            }

            return PartialView("patialHocVien", listNCS);
        }

        public ActionResult ThemMoiUser()
        {
            return View();
        }
        //[HttpPost]
        //public async Task<ActionResult> ThemMoiUser(FormCollection form)
        //{
        //    try
        //    {
        //        var role = form["role"].ToString();
        //        var user = new ApplicationUser
        //        {
        //            UserName = form["tentruycap"],
        //            TwoFactorEnabled = true,
        //            Email = form["email"],
        //            PrivateKey = TimeSensitivePassCode.GeneratePresharedKey()
        //        };
        //        Models.HocVien hv = new Models.HocVien();
        //        hv.ApplicationUserId = user.Id;
        //        hv.MaHocVien = "";
        //        hv.Ho = form["Ho"];
        //        hv.Ten = form["Ten"];
        //        hv.NgaySinh = DateTime.Parse(form["NgaySinh"].ToString());
        //        hv.DiaChi = form["DiaChi"];
        //        hv.GioiTinh = form["GioiTinh"];
        //        hv.NoiSinh = form["NoiSinh"];
        //        hv.HoKhau = form["HoKhau"];
        //        hv.DiaChi = form["DiaChi"];
        //        hv.DienThoai = form["DienThoai"];
        //        hv.Email = form["email"];
        //        hv.UserName = form["tentruycap"];
        //        hv.Password = form["matkhau"];

        //        hv.DanToc = form["DanToc"];
        //        hv.QuocTich = form["QuocTich"];

        //        hv.RoleId = 1;
        //        user.HocVien = hv;

        //        var result = await UserManager.CreateAsync(user, hv.Password);
        //        if (result.Succeeded)
        //        {
        //            UserManager.AddToRole(user.Id, PublicConstant.ROLE_GIAOVIEN.ToString());
        //            TempData["message"] = "Thêm mới thành công";
        //            return RedirectToAction("Index", "HocVien");
        //        }
        //        else
        //        {
        //            TempData["error"] = "Lỗi tạo tài khoản";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["error"] = "Lỗi tạo tài khoản";
        //        string mss = ex.Message;
        //    }
        //    return View();
        //}

        public ActionResult UpLoadExcel()
        {
            return View();
        }

        public async Task<JsonResult> ExportExcel(string id)
        {
            try
            {
                int buoc = int.Parse(id);
                List<NCS> lstNCS = new List<NCS>();
                List<BaoVe_NCS> lstBaoVe_NCS = new List<BaoVe_NCS>();
                if (buoc == -1)
                {
                    lstBaoVe_NCS = _unitOfWork.GetRepositoryInstance<BaoVe_NCS>().GetListByParameter(x => x.Status == 0).ToList();
                }
                else if (buoc == 1)
                {
                    lstBaoVe_NCS = _unitOfWork.GetRepositoryInstance<BaoVe_NCS>().GetListByParameter(x => x.Buoc1 == 1).ToList();
                }
                else if (buoc == 2)
                {
                    lstBaoVe_NCS = _unitOfWork.GetRepositoryInstance<BaoVe_NCS>().GetListByParameter(x => x.Buoc2 == 1).ToList();
                }
                else if (buoc == 3)
                {
                    lstBaoVe_NCS = _unitOfWork.GetRepositoryInstance<BaoVe_NCS>().GetListByParameter(x => x.Buoc3 == 1).ToList();
                }
                else if (buoc == 4)
                {
                    lstBaoVe_NCS = _unitOfWork.GetRepositoryInstance<BaoVe_NCS>().GetListByParameter(x => x.Buoc4 == 1).ToList();
                }
                else if (buoc == 5)
                {
                    lstBaoVe_NCS = _unitOfWork.GetRepositoryInstance<BaoVe_NCS>().GetListByParameter(x => x.Buoc5 == 1).ToList();
                }
                else if (buoc == 6)
                {
                    lstBaoVe_NCS = _unitOfWork.GetRepositoryInstance<BaoVe_NCS>().GetListByParameter(x => x.Buoc6 == 1).ToList();
                }
                else if (buoc == 7)
                {
                    lstBaoVe_NCS = _unitOfWork.GetRepositoryInstance<BaoVe_NCS>().GetListByParameter(x => x.Buoc7 == 1).ToList();
                }
                else if (buoc == 8)
                {
                    lstBaoVe_NCS = _unitOfWork.GetRepositoryInstance<BaoVe_NCS>().GetListByParameter(x => x.Status == 1).ToList();
                }
                foreach (BaoVe_NCS item in lstBaoVe_NCS)
                {
                    var ncs = _unitOfWork.GetRepositoryInstance<NCS>().GetFirstOrDefaultByParameter(x => x.Ma == item.MaNCS);
                    if (ncs != null)
                    {
                        lstNCS.Add(ncs);
                    }
                }



                //tạo file excel

                using (ExcelPackage p = new ExcelPackage())
                {
                    List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
                    p.Workbook.Properties.Author = "Admin";
                    p.Workbook.Properties.Title = "Báo cáo thống kê";
                    //Tạo sheet
                    p.Workbook.Worksheets.Add("Sheet1");
                    ExcelWorksheet ws = p.Workbook.Worksheets[1];
                    // fontsize mặc định cho cả sheet
                    ws.Cells.Style.Font.Size = 13;
                    // font family mặc định cho cả sheet
                    ws.Cells.Style.Font.Name = "Times New Roman";

                    string[] arrColumnHeader = {"STT","Mã HV","Họ và", "tên","Giới tính",
                    "Ngày sinh","Số điện thoại","Email","Khoa",
                    "Tình trạng"};
                    var countColHeader = arrColumnHeader.Count();

                    ws.Cells[1, 1].Value = "BỘ GIÁO DỤC VÀ ĐÀO TẠO";
                    ws.Cells[1, 1, 1, 4].Merge = true;
                    ws.Cells[2, 1].Value = "Công ty Cổ phần Hệ thống 2B";
                    ws.Cells[2, 1, 2, 6].Merge = true;
                    ws.Cells[3, 1].Value = "HỘI ĐỒNG TUYỂN SINH NCS NĂM 2020";
                    ws.Cells[3, 1, 3, 4].Merge = true;
                    ws.Cells[3, 1, 3, 4].Style.Font.Bold = true;

                    ws.Cells[1, 7].Value = "CỘNG HOÀ XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                    ws.Cells[1, 7, 1, 11].Merge = true;
                    ws.Cells[1, 7, 1, 11].Style.Font.Bold = true;
                    ws.Cells[1, 7, 1, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[2, 7].Value = "ĐỘC LẬP - TỰ DO - HẠNH PHÚC";
                    ws.Cells[2, 7, 2, 11].Merge = true;
                    ws.Cells[2, 7, 2, 11].Style.Font.Bold = true;
                    ws.Cells[2, 7, 2, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    if (buoc == -1)
                    {
                        ws.Cells[4, 1].Value = "DANH SÁCH NGHIÊN CỨU SINH CHƯA HOÀN THÀNH HỒ SƠ";
                    }
                    else if (buoc == 8)
                    {
                        ws.Cells[4, 1].Value = "DANH SÁCH NGHIÊN CỨU SINH HOÀN THÀNH HỒ SƠ";
                    }
                    else
                    {
                        var lst = _unitOfWork.GetRepositoryInstance<DanhMucThongTin>().GetAllRecords().ToList();
                        ws.Cells[4, 1].Value = "DANH SÁCH NGHIÊN CỨU SINH HOÀN THÀNH BƯỚC" + _unitOfWork.GetRepositoryInstance<DanhMucThongTin>().GetFirstOrDefaultByParameter(x => x.Id == buoc).TenDanhMuc;
                    }

                    ws.Cells[4, 1, 4, countColHeader].Merge = true;
                    ws.Cells[4, 1, 4, countColHeader].Style.Font.Bold = true;
                    ws.Cells[4, 1, 4, countColHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    int colIndex = 1;
                    int rowIndex = 5;
                    foreach (var item in arrColumnHeader)
                    {
                        var cell = ws.Cells[rowIndex, colIndex];

                        //set màu thành gray
                        var fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(System.Drawing.Color.DarkGray);

                        //căn chỉnh các border
                        var border = cell.Style.Border;
                        border.Bottom.Style =
                            border.Top.Style =
                            border.Left.Style =
                            border.Right.Style = ExcelBorderStyle.Medium;

                        //gán giá trị
                        cell.Value = item;

                        colIndex++;
                    }
                    int sothutu = 0;
                    foreach (NCS dkts in lstNCS)
                    {
                        colIndex = 1;
                        rowIndex++;
                        sothutu++;
                        ws.Cells[rowIndex, colIndex++].Value = sothutu;
                        ws.Cells[rowIndex, colIndex++].Value = dkts.Ma;
                        string[] hovaten = dkts.HoTen.Split(' ');
                        string hoten = "";
                        for (int k = 0; k < hovaten.Count() - 1; k++)
                        {
                            hoten += hovaten[k];
                        }
                        ws.Cells[rowIndex, colIndex++].Value = hoten;
                        ws.Cells[rowIndex, colIndex++].Value = hovaten[hovaten.Count() - 1];
                        ws.Cells[rowIndex, colIndex++].Value = DateTime.Parse(dkts.NgaySinh.ToString()).ToString("dd/MM/yyyy");
                        ws.Cells[rowIndex, colIndex++].Value = dkts.GioiTinh;
                        ws.Cells[rowIndex, colIndex++].Value = dkts.DienThoai;
                        ws.Cells[rowIndex, colIndex++].Value = dkts.Email;
                        ws.Cells[rowIndex, colIndex++].Value = listKhoa.Count > 0 ? listKhoa.Where(o => o.Id == dkts.KhoaId).SingleOrDefault().Name : ""; ;
                        if (buoc == -1)
                        {
                            ws.Cells[rowIndex, colIndex++].Value = "CHƯA HOÀN THÀNH HỒ SƠ";
                        }
                        else if (buoc == 8)
                        {
                            ws.Cells[rowIndex, colIndex++].Value = "HOÀN THÀNH HỒ SƠ";
                        }
                        else
                        {
                            ws.Cells[rowIndex, colIndex++].Value = "HOÀN THÀNH BƯỚC" + buoc;
                        }
                    }
                    rowIndex++;
                    ws.Cells[rowIndex, 1].Value = "(Danh sách gồm có " + lstNCS.Count() + " NCS)";
                    ws.Cells[rowIndex, 1, rowIndex, countColHeader].Merge = true;
                    ws.Cells[rowIndex, 1, rowIndex, countColHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;




                    byte[] bin = p.GetAsByteArray();
                    string name = "Danhsachtuyensinh";
                    string filename_new = $"{name}_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.xls";
                    var filePath = Server.MapPath("~/" + parthdowload + filename_new);
                    //File(byteArray, "", $"{filePath}");
                    System.IO.File.WriteAllBytes(filePath, bin);

                    var systemPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    var filePathdowload = System.IO.Path.Combine(systemPath, filename_new);
                    // Create a new WebClient instance.
                    //WebClient myWebClient = new WebClient();

                    // Download the Web resource and save it into the current filesystem folder.
                    //myWebClient.DownloadFile(filePath, filePathdowload);

                    //return Json(url_download + filename_new, JsonRequestBehavior.AllowGet);
                    return Json(url_domain + parthdowload + filename_new, JsonRequestBehavior.AllowGet);
                    

                }
            }
            catch (Exception ex)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        //[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async System.Threading.Tasks.Task<ActionResult> UploadExcelFile(HttpPostedFileBase upload)
        //{
        //    string error = "";
        //    if (ModelState.IsValid)
        //    {
        //        if (upload != null && upload.ContentLength > 0)
        //        {
        //            try
        //            {
        //                Stream stream = upload.InputStream;
        //                IExcelDataReader reader = null;

        //                if (upload.FileName.EndsWith(".xls"))
        //                {
        //                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
        //                }
        //                else if (upload.FileName.EndsWith(".xlsx"))
        //                {
        //                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        //                }
        //                else
        //                {
        //                    ModelState.AddModelError("File", "File không đúng địng dạng");
        //                    TempData["error"] = "File không đúng địng dạng";
        //                    return View();
        //                }

        //                reader.IsFirstRowAsColumnNames = true;

        //                DataSet ds = reader.AsDataSet();
        //                DataTable dt = ds.Tables[0];
        //                string fileExelName = upload.FileName;
        //                if (dt.Rows.Count > 0)
        //                {
        //                    if (dt.Rows.Count < 2)
        //                    {
        //                        TempData["error"] = "File excel phải có ít nhất từ 2 bản ghi trở lên";
        //                        return RedirectToAction("Index", "Home");
        //                    }

        //                    if (dt.Rows.Count > 1000)
        //                    {
        //                        TempData["error"] = "Số lượng tối đa 1000 bản ghi trong mỗi lần up";
        //                        return RedirectToAction("Index", "Home");
        //                    }

        //                    string userName = User.Identity.GetUserName();

        //                    Models.HocVien hv;

        //                    foreach (DataRow row in dt.Rows)
        //                    {
        //                        try
        //                        {
        //                            if (row["Họ Và Tên"].ToString() != "")
        //                            {
        //                                //var role = row["Nhóm Quyền"].ToString();
        //                                var user = new ApplicationUser
        //                                {
        //                                    UserName = row["Tên Truy Cập"].ToString(),
        //                                    TwoFactorEnabled = true,
        //                                    Email = row["Email"].ToString(),
        //                                    PrivateKey = TimeSensitivePassCode.GeneratePresharedKey()
        //                                };
        //                                hv = new Models.HocVien();
        //                                hv.ApplicationUserId = user.Id;
        //                                hv.Ho = row["Họ"].ToString();
        //                                hv.Ten = row["Tên"].ToString();
        //                                hv.NgaySinh = DateTime.Parse(row["Ngày Sinh"].ToString());
        //                                hv.DiaChi = row["Địa Chỉ"].ToString();
        //                                hv.GioiTinh = row["Giới Tính"].ToString();
        //                                hv.NoiSinh = row["Nơi Sinh"].ToString();
        //                                hv.HoKhau = row["Hộ Khẩu"].ToString();
        //                                hv.DiaChi = row["Địa Chỉ"].ToString();
        //                                hv.DienThoai = row["Số Điện Thoại"].ToString();
        //                                hv.Email = row["Email"].ToString();
        //                                hv.DanToc = row["Dân Tộc"].ToString();
        //                                hv.QuocTich = row["Quốc Tịch"].ToString();

        //                                hv.RoleId = 1;
        //                                user.HocVien = hv;


        //                                var message = await UserManager.CreateAsync(user, hv.Password);
        //                                if (message.Succeeded)
        //                                {
        //                                    UserManager.AddToRole(user.Id, PublicConstant.ROLE_GIAOVIEN.ToString());
        //                                }
        //                                else
        //                                {
        //                                    error = "Lỗi tạo tài khoản";
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            error = "Lỗi tạo tài khoản : " + ex.Message;
        //                            break;
        //                        }
        //                    }

        //                    if (error == "")
        //                    {
        //                        _unitOfWork.SaveChanges();
        //                        TempData["message"] = "Import danh sách thành công!";
        //                    }
        //                    else
        //                    {
        //                        error = "Lỗi upload : " + error;
        //                    }
        //                }
        //                else
        //                {
        //                    TempData["error"] = "File không có dữ liệu!";
        //                }

        //                reader.Close();
        //            }
        //            catch (Exception ex)
        //            {
        //                TempData["error"] = "Lỗi xảy ra trong quá trình import file excel:" + ex.Message;
        //            }
        //        }
        //        else
        //        {
        //            TempData["error"] = "File không có dữ liệu !";
        //        }
        //    }



        //    return RedirectToAction("Index", "HocVien");
        //}

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                Model.NCS hv = _unitOfWork.GetRepositoryInstance<Model.NCS>().GetFirstOrDefaultByParameter(x => x.Id == id);
                if (hv != null)
                {
                    return View(hv);
                }
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ViewModelHoSoGV hoSo, FormCollection form)
        {
            try
            {
                //Model.HocVien hv = _unitOfWork.GetRepositoryInstance<Model.HocVien>().GetFirstOrDefaultByParameter(x => x.Id == hoSo.HoSogvInfo.Id);
                //TempData["info"] = "Cập nhật thông tin account thành công ";
                //hv.Ho = form["Ho"];
                //hv.Ten = form["Ten"];
                //hv.NgaySinh = DateTime.Parse(form["NgaySinh"].ToString());
                //hv.DiaChi = form["DiaChi"];
                //hv.GioiTinh = form["GioiTinh"];
                //hv.NoiSinh = form["NoiSinh"];
                //hv.HoKhau = form["HoKhau"];
                //hv.DiaChi = form["DiaChi"];
                //hv.DienThoai = form["SoDienThoai"];
                //hv.Email = form["Email"];
                //hv.DanToc = form["DanToc"];
                //hv.QuocTich = form["QuocTich"];

                TempData["error"] = "Cập nhật thông tin thành công";
                //_unitOfWork.GetRepositoryInstance<Model.HocVien>().Update(hv);
                //_unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi cập nhật thông tin user: " + ex.Message;
            }
            return View(hoSo);
        }


        //public async Task<ActionResult> PhanQuyen(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    //var user = await UserManager.FindByIdAsync(id);
        //    Model.HocVien UserAdmin = db.HocViens.Find(id);
        //    var user = await UserManager.FindByIdAsync(UserAdmin.ApplicationUserId);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    var userRoles = await UserManager.GetRolesAsync(user.Id);

        //    return View(new Models.EditUserViewModel()
        //    {
        //        Id = user.Id,
        //        //Email = user.Email,
        //        RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
        //        {
        //            Selected = userRoles.Contains(x.Name),
        //            Text = x.Name,
        //            Value = x.Name
        //        })
        //    });
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PhanQuyen(FormCollection form, params string[] selectedRole)
        {
            int id = form.Get("Id") != null ? int.Parse(form.Get("Id")) : 0;
            Model.NCS userInfo = _unitOfWork.GetRepositoryInstance<Model.NCS>().GetFirstOrDefaultByParameter(o => o.Id == id);
            if (userInfo != null)
            {
                //var user = await UserManager.FindByIdAsync(userInfo.ApplicationUserId);

                //if (ModelState.IsValid)
                //{
                //    if (user == null)
                //    {
                //        return HttpNotFound();
                //    }

                //    var userRoles = await UserManager.GetRolesAsync(user.Id);

                //    selectedRole = selectedRole ?? new string[] { };

                //    var result = await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());

                //    if (!result.Succeeded)
                //    {
                //        ModelState.AddModelError("", result.Errors.First());
                //        return View();
                //    }
                //    result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());

                //    if (!result.Succeeded)
                //    {
                //        ModelState.AddModelError("", result.Errors.First());
                //        return View();
                //    }
                //    return RedirectToAction("Index");
                //}
                ModelState.AddModelError("", "Something failed.");
            }
            return View();
        }

        public async Task<ActionResult> Detail(long? id)
        {
            Model.NCS NCSDb = new NCS();
            NCSDb = _unitOfWork.GetRepositoryInstance<Model.NCS>().GetFirstOrDefaultByParameter(o => o.Id == id);

            List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
            List<ChucDanhAPI> listChucDanh = await CoreAPI.CoreAPI.GetListChucDanh();

            NCSViewModel ncs = new NCSViewModel();
            ncs = new NCSViewModel();
            ncs.Id = int.Parse(NCSDb.Id.ToString());
            ncs.Ma = NCSDb.Ma;
            ncs.HoTen = NCSDb.HoTen;
            ncs.NgaySinh = DateTime.Parse(NCSDb.NgaySinh.ToString());
            ncs.GioiTinh = NCSDb.GioiTinh;
            ncs.NoiSinh = NCSDb.NoiSinh;
            ncs.HoKhau = NCSDb.HoTen;
            ncs.Email = NCSDb.Email;
            ncs.DienThoai = NCSDb.DienThoai;
            ncs.ChucDanh = NCSDb.ChucDanhId > 0 && listChucDanh.Count > 0 ? listChucDanh.Where(o => o.Id == NCSDb.ChucDanhId).SingleOrDefault().Name : "";
            ncs.Khoa = listKhoa.Count > 0 ? listKhoa.Where(o => o.Id == NCSDb.KhoaId).SingleOrDefault().Name : "";
            ncs.DiaChi = NCSDb.DiaChi;
            ncs.DanToc = NCSDb.DanToc;
            ncs.QuocTich = NCSDb.QuocTich;

            return View(ncs);
        }

        public ActionResult ThongTinKhoaHoc(string magiaovien)
        {
            if (magiaovien == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var magiaovienAgument = new SqlParameter("mahocvien", System.Data.SqlDbType.NVarChar) { Value = magiaovien };
            //SP_ThongTinKHoaHocGiaVien_Result info = _unitOfWork.GetRepositoryInstance<SP_ThongTinKHoaHocGiaVien_Result>().GetResultBySqlProcedure("SP_ThongTinKHoaHocGiaVien @magiangvien", magiaovienAgument).SingleOrDefault();

            //return View(info);
            return View();
        }
        public async Task<JsonResult> Xoa(long id)
        {
            try
            {
                //TEMIS.Model.HocVien hsgv = new TEMIS.Model.HocVien();
                //hsgv = _unitOfWork.GetRepositoryInstance<TEMIS.Model.HocVien>().GetFirstOrDefaultByParameter(o => o.Id == id);
                //if (hsgv != null)
                //{
                //    AspNetUser user = _unitOfWork.GetRepositoryInstance<AspNetUser>().GetFirstOrDefaultByParameter(o => o.Id == hsgv.ApplicationUserId);
                //    if (user != null)
                //    {
                //        _unitOfWork.GetRepositoryInstance<AspNetUser>().Remove(user);
                //        _unitOfWork.GetRepositoryInstance<TEMIS.Model.HocVien>().Remove(hsgv);
                //        _unitOfWork.SaveChanges();
                //    }
                //}
                //else
                //{
                //    TempData["error"] = "bản ghi không tồn tại";
                //    return Json("bản ghi không tồn tại", JsonRequestBehavior.AllowGet);
                //}

                TempData["message"] = "Xóa thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi xóa: " + ex.Message;
                return Json("Lỗi xóa", JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> QuanLyHV(string maHV)
        {
            try
            {
                List<TruongThongTinViewModel> listHuongDan = new List<TruongThongTinViewModel>();
                List<Model.TruongThongTin> lstTruongThongTin = _unitOfWork.GetRepositoryInstance<Model.TruongThongTin>().GetAllRecords().ToList();
                int i = 1;
                foreach (var item in lstTruongThongTin)
                {
                    TruongThongTinViewModel ttt_NCS = new TruongThongTinViewModel();
                    ttt_NCS.STT = i;
                    ttt_NCS.Id = item.Id;
                    ttt_NCS.IdDanhMuc = item.IdDanhMuc;
                    ttt_NCS.TenTruongThongTin = item.TenTruongThongTin;
                    ttt_NCS.LoaiTruongThongTin = item.LoaiTruongThongTin;
                    ttt_NCS.Status = item.Status;
                    ttt_NCS.CreatedAt = item.CreatedAt;
                    ttt_NCS.CreatedBy = item.CreatedBy;
                    ttt_NCS.UpdatedAt = item.UpdatedAt;
                    ttt_NCS.UpdatedBy = item.UpdatedBy;
                    TruongThongTin_NCS tttUpload = _unitOfWork.GetRepositoryInstance<Model.TruongThongTin_NCS>().GetFirstOrDefaultByParameter(o => o.TruongThongTinId == item.Id && o.MaNCS == maHV);
                    if (tttUpload != null)
                    {
                        ttt_NCS.Url = tttUpload.Url;
                        if (tttUpload.Status != null)
                        {
                            ttt_NCS.StatusUpload = (int)tttUpload.Status;
                        }
                        else
                        {
                            ttt_NCS.StatusUpload = -2;
                        }
                    }
                    else
                    {
                        ttt_NCS.StatusUpload = -2;
                    }
                    ttt_NCS.MaNCS = maHV;

                    listHuongDan.Add(ttt_NCS);

                    i = i + 1;
                }

                ViewBag.MaNCS = maHV;

                List<VanBan> listVanBan = _unitOfWork.GetRepositoryInstance<Model.VanBan>().GetListByParameter(o => o.ChuyenMuc == 4).ToList();
                ViewBag.listVanBan = listVanBan;

                DangKyTuyenSinh dkts = _unitOfWork.GetRepositoryInstance<DangKyTuyenSinh>().GetFirstOrDefaultByParameter(o => o.MaNCS == maHV);
                ViewBag.DangKyTuyenSinh = dkts;

                //tab 1
                BaoVeTongQuan bvtq = _unitOfWork.GetRepositoryInstance<BaoVeTongQuan>().GetFirstOrDefaultByParameter(o => o.MaNCS == maHV);
                ViewBag.BaoVeTongQuan = bvtq;
                ThongTinDeTai thongtindetai = _unitOfWork.GetRepositoryInstance<ThongTinDeTai>().GetFirstOrDefaultByParameter(o => o.MaNCS == maHV);
                ViewBag.ThongTinDeTai = thongtindetai;
                List<NguoiHuongDan> ListNguoiHuongDan = _unitOfWork.GetRepositoryInstance<NguoiHuongDan>().GetListByParameter(o => o.MaNCS == maHV).ToList();
                ViewBag.NguoiHuongDan = ListNguoiHuongDan;

                //tab 2
                List<DanhSachChuyenDe> ChuyenDe = _unitOfWork.GetRepositoryInstance<DanhSachChuyenDe>().GetListByParameter(o => o.MaNCS == maHV).ToList();
                ViewBag.DanhSachChuyenDe = ChuyenDe;

                List<TieuBanChamChuyenDe> TieuBanChamChuyenDe = _unitOfWork.GetRepositoryInstance<TieuBanChamChuyenDe>().GetListByParameter(o => o.MaNCS == maHV).ToList();
                ViewBag.TieuBanChamChuyenDe = TieuBanChamChuyenDe;

                // tab 3
                List<CongTrinhKhoaHoc> congTrinhKhoaHoc = _unitOfWork.GetRepositoryInstance<CongTrinhKhoaHoc>().GetListByParameter(o => o.MaNCS == maHV).ToList();
                ViewBag.CongTrinhKhoaHoc = congTrinhKhoaHoc;

                // tab 4
                List<BaoVeCapBoMon> congBaoVeCapBoMon = _unitOfWork.GetRepositoryInstance<BaoVeCapBoMon>().GetListByParameter(o => o.MaNCS == maHV).ToList();
                ViewBag.BaoVeCapBoMon = congBaoVeCapBoMon;

                List<QuyetDinh> ListQuyetDinh = _unitOfWork.GetRepositoryInstance<QuyetDinh>().GetListByParameter(o => o.MaNCS == maHV).ToList();
                ViewBag.ListQuyetDinh = ListQuyetDinh;

                List<DanhSachHoiDong> ListDanhSachHoiDong = _unitOfWork.GetRepositoryInstance<DanhSachHoiDong>().GetListByParameter(o => o.MaNCS == maHV).ToList();
                ViewBag.DanhSachHoiDong = ListDanhSachHoiDong;

                List<KetQuaBaoVe> ListKetQuaBaoVe = _unitOfWork.GetRepositoryInstance<KetQuaBaoVe>().GetListByParameter(o => o.MaNCS == maHV).ToList();
                ViewBag.ListKetQuaBaoVe = ListKetQuaBaoVe;

                // tab 5
                NCS ncs = _unitOfWork.GetRepositoryInstance<NCS>().GetFirstOrDefaultByParameter(o => o.Ma == maHV);
                List<GiangVienAPI> list = await CoreAPI.CoreAPI.GetListGiangVien(int.Parse(ncs.KhoaId.ToString()));
                List<ChucDanhAPI> listChucDanh = await CoreAPI.CoreAPI.GetListChucDanh();
                List<HocHamHocViAPI> listHocHamHocViAPI = await CoreAPI.CoreAPI.GetListHocHamHocVi();
                List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
                List<GiangVienViewModel> listGiangVien = new List<GiangVienViewModel>();
                GiangVienViewModel giangvien;
                foreach (GiangVienAPI item in list)
                {
                    GiangVienDetailAPI gv = await CoreAPI.CoreAPI.GetThongTinGiangVien(item.Id);
                    giangvien = new GiangVienViewModel();
                    giangvien.HoTen = item.Name;
                    giangvien.NgaySinh = gv.Birthday;
                    giangvien.GioiTinh = gv.Gender;
                    giangvien.NoiSinh = "";
                    giangvien.HoKhau = "";
                    giangvien.ThongTinLienLac = "";
                    var chucdanh = listChucDanh.Where(o => o.Id == item.TitleId).SingleOrDefault();
                    if (chucdanh != null)
                        giangvien.ChucDanh = chucdanh.Name;
                    else giangvien.ChucDanh = "";
                    var hochamhocvi = listHocHamHocViAPI.Where(o => o.Id == item.DegreeId).SingleOrDefault();
                    if (hochamhocvi != null)
                        giangvien.HocHamHocVi = hochamhocvi.Name;
                    else giangvien.HocHamHocVi = "";
                    var department = item.DepartmentId;
                    var getKhoa = listKhoa.Where(o => o.Id == department).SingleOrDefault();
                    if (getKhoa != null)
                        giangvien.KHoa = getKhoa.Name;
                    else giangvien.KHoa = "";
                    listGiangVien.Add(giangvien);
                }
                ViewBag.GiangVien = listGiangVien;
                List<PhanBienDocLap> ListPhanBienDocLap = _unitOfWork.GetRepositoryInstance<PhanBienDocLap>().GetListByParameter(o => o.MaNCS == maHV).ToList();
                ViewBag.PhanBienDocLap = ListPhanBienDocLap;

                // tab 6
                List<BaoVeCapTruong> ListBaoVeCapTruong = _unitOfWork.GetRepositoryInstance<BaoVeCapTruong>().GetListByParameter(o => o.MaNCS == maHV).ToList();
                ViewBag.ListBaoVeCapTruong = ListBaoVeCapTruong;

                GiayToBaoVeLuanAn ListGiayToBaoVeLuanAn = _unitOfWork.GetRepositoryInstance<GiayToBaoVeLuanAn>().GetFirstOrDefaultByParameter(o => o.MaNCS == maHV);
                ViewBag.ListGiayToBaoVeLuanAn = ListGiayToBaoVeLuanAn;

                HoSoThamDinh hosothamdinh = _unitOfWork.GetRepositoryInstance<HoSoThamDinh>().GetFirstOrDefaultByParameter(o => o.MaNCS == maHV);
                ViewBag.HoSoThamDinh = hosothamdinh;

                //tab 7


                BaoVe_NCS baove_NCS = _unitOfWork.GetRepositoryInstance<BaoVe_NCS>().GetFirstOrDefaultByParameter(o => o.MaNCS == maHV);
                ViewBag.BaoVe_NCS = baove_NCS;

                return View(listHuongDan);
            }
            catch (Exception ex)
            {
                return View();
            }

        }

        // tab 1
        [HttpPost]
        public async Task<JsonResult> UpdateThongTinDeTai()
        {
            try
            {
                string fileUrl = "";
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                string mancs = Request.Form["MaNCS"] != null ? Request.Form["MaNCS"].ToString() : "";
                string SoQDThongTinDeTai = Request.Form["soquyetdinh"] != null ? Request.Form["soquyetdinh"].ToString() : "";
                string NgayKyThongTinDeTai = Request.Form["ngayky"] != null ? Request.Form["ngayky"].ToString() : "";
                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;

                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] ffiles = file.FileName.Split(new char[] { '\\' });
                            fname = user.Username + "_" + ffiles[ffiles.Length - 1];
                        }
                        else
                        {
                            fname = user.Username + "_" + file.FileName;
                        }
                        fileUrl = fname;
                        fname = Path.Combine(Server.MapPath("~/Upload/QuyetDinh/"), fname);
                        file.SaveAs(fname);
                    }
                }
                // insert bảng thông báo
                ThongBao thongbao = new ThongBao();
                thongbao.MaNCS = mancs;
                thongbao.CreatedAt = DateTime.Now;
                thongbao.UpdatedAt = DateTime.Now;
                thongbao.CreatedBy = user.Username;
                thongbao.UpdatedBy = user.Username;

                ThongTinDeTai thongtindetai = new ThongTinDeTai();
                thongtindetai = _unitOfWork.GetRepositoryInstance<ThongTinDeTai>().GetFirstOrDefaultByParameter(o => o.MaNCS == mancs);
                if (thongtindetai != null)
                {
                    thongbao.Title = "Cập nhật thông tin đề tài";
                    thongtindetai.SoQuyetDinh = SoQDThongTinDeTai;
                    thongtindetai.NgayKy = DateTime.Parse(NgayKyThongTinDeTai);
                    thongtindetai.MaNCS = mancs;
                    thongtindetai.FileKiemChung = fileUrl;
                    thongtindetai.UpdatedAt = DateTime.Now;
                    thongtindetai.UpdatedBy = user.Username;
                    _unitOfWork.GetRepositoryInstance<ThongTinDeTai>().Update(thongtindetai);
                }
                else
                {
                    DangKyTuyenSinh dkts = new DangKyTuyenSinh();
                    dkts = _unitOfWork.GetRepositoryInstance<Model.DangKyTuyenSinh>().GetFirstOrDefaultByParameter(o => o.MaNCS == mancs);
                    thongtindetai = new ThongTinDeTai();
                    thongtindetai.SoQuyetDinh = SoQDThongTinDeTai;
                    thongtindetai.NgayKy = DateTime.Parse(NgayKyThongTinDeTai);
                    thongtindetai.MaNCS = mancs;
                    thongtindetai.FileKiemChung = fileUrl;
                    thongtindetai.TenDeTai = dkts.TenDeTai;
                    thongtindetai.NHD1 = dkts.NHD2;
                    thongtindetai.NHD2 = dkts.NHD1;
                    thongtindetai.CreatedAt = DateTime.Now;
                    thongtindetai.UpdatedAt = DateTime.Now;
                    thongtindetai.CreatedBy = user.Username;
                    thongtindetai.UpdatedBy = user.Username;
                    _unitOfWork.GetRepositoryInstance<ThongTinDeTai>().Add(thongtindetai);

                    thongbao.Title = "Thêm mới đề tài";
                }

                _unitOfWork.GetRepositoryInstance<ThongBao>().Add(thongbao);

                _unitOfWork.SaveChanges();

                TempData["message"] = "Upload thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi upload: " + ex.Message;
                return Json("Lỗi upload", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ThemMoiNguoiHuongDan(string hoten, string coquancongtac, string vaitrothamgia, string loaiquyetdinh, string mancs)
        {
            try
            {
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                NguoiHuongDan nhd = new NguoiHuongDan();
                nhd.HoTen = hoten;
                nhd.CoQuanCongtac = coquancongtac;
                nhd.VaiTroThamGia = vaitrothamgia;
                nhd.LoaiQuyetDinh = loaiquyetdinh;
                nhd.CreatedAt = DateTime.Now;
                nhd.UpdatedAt = DateTime.Now;
                nhd.CreatedBy = user.Username;
                nhd.UpdatedBy = user.Username;
                nhd.MaNCS = mancs;
                _unitOfWork.GetRepositoryInstance<NguoiHuongDan>().Add(nhd);

                // insert bảng thông báo
                ThongBao thongbao = new ThongBao();
                thongbao.MaNCS = mancs;
                thongbao.CreatedAt = DateTime.Now;
                thongbao.UpdatedAt = DateTime.Now;
                thongbao.CreatedBy = user.Username;
                thongbao.UpdatedBy = user.Username;
                thongbao.Title = "Thêm mới người hướng dẫn";
                _unitOfWork.GetRepositoryInstance<ThongBao>().Add(thongbao);

                _unitOfWork.SaveChanges();
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Thêm mới lỗi", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateNguoiHuongDan(long id, string soquyetdinh, string ngayky = "")
        {
            try
            {
                var user = (CoreAPI.Entity.TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                var nguoihuongdan = _unitOfWork.GetRepositoryInstance<NguoiHuongDan>().GetFirstOrDefaultByParameter(x => x.Id == id);
                if (nguoihuongdan == null)
                {
                    return Json("Người hướng dẫn không tồn tại", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    nguoihuongdan.SoQDDieuChinhNHD = soquyetdinh;
                    nguoihuongdan.NgayKy = ngayky != "" ? DateTime.Parse(ngayky) : DateTime.Now;
                    nguoihuongdan.UpdatedAt = DateTime.Now;
                    nguoihuongdan.UpdatedBy = user.Username;
                    nguoihuongdan.MaNCS = user.Username;
                    _unitOfWork.GetRepositoryInstance<NguoiHuongDan>().Update(nguoihuongdan);

                    // insert bảng thông báo
                    ThongBao thongbao = new ThongBao();
                    thongbao.MaNCS = nguoihuongdan.MaNCS;
                    thongbao.CreatedAt = DateTime.Now;
                    thongbao.UpdatedAt = DateTime.Now;
                    thongbao.CreatedBy = user.Username;
                    thongbao.UpdatedBy = user.Username;
                    thongbao.Title = "Cập nhật người hướng dẫn";
                    _unitOfWork.GetRepositoryInstance<ThongBao>().Add(thongbao);

                    _unitOfWork.SaveChanges();
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json("Lỗi cập nhật", JsonRequestBehavior.AllowGet);
            }
        }

        // tab 2
        public JsonResult UpdateChuyenDe(long id, string ngaybaove = "", string diemso = "")
        {
            try
            {
                var user = (CoreAPI.Entity.TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                var chuyende = _unitOfWork.GetRepositoryInstance<DanhSachChuyenDe>().GetFirstOrDefaultByParameter(x => x.Id == id);
                if (chuyende == null)
                {
                    return Json("Chuyên đề không tồn tại", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    chuyende.DiemSo = diemso != "" ? double.Parse(diemso) : 0;
                    chuyende.NgayBaoVe = ngaybaove != "" ? DateTime.Parse(ngaybaove) : DateTime.Now;
                    chuyende.UpdatedAt = DateTime.Now;
                    chuyende.UpdatedBy = user.Username;
                    chuyende.MaNCS = user.Username;
                    _unitOfWork.GetRepositoryInstance<DanhSachChuyenDe>().Update(chuyende);


                    // insert bảng thông báo
                    ThongBao thongbao = new ThongBao();
                    thongbao.MaNCS = chuyende.MaNCS;
                    thongbao.CreatedAt = DateTime.Now;
                    thongbao.UpdatedAt = DateTime.Now;
                    thongbao.CreatedBy = user.Username;
                    thongbao.UpdatedBy = user.Username;
                    thongbao.Title = "Cập nhật chuyên đề";
                    _unitOfWork.GetRepositoryInstance<ThongBao>().Add(thongbao);

                    _unitOfWork.SaveChanges();
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json("Cập nhật lỗi", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateQuyetDinh()
        {
            try
            {
                string fileUrl = "";
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                string mancs = Request.Form["MaNCS"] != null ? Request.Form["MaNCS"].ToString() : "";
                string SoQDThongTinDeTai = Request.Form["soquyetdinh"] != null ? Request.Form["soquyetdinh"].ToString() : "";
                string NgayKyThongTinDeTai = Request.Form["ngayky"] != null ? Request.Form["ngayky"].ToString() : "";
                int type = Request.Form["type"] != null ? int.Parse(Request.Form["type"].ToString()) : 0;
                int tab = Request.Form["tab"] != null ? int.Parse(Request.Form["tab"].ToString()) : 0;
                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;

                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] ffiles = file.FileName.Split(new char[] { '\\' });
                            fname = user.Username + "_" + ffiles[ffiles.Length - 1];
                        }
                        else
                        {
                            fname = user.Username + "_" + file.FileName;
                        }
                        fileUrl = fname;
                        fname = Path.Combine(Server.MapPath("~/Upload/QuyetDinh/"), fname);
                        file.SaveAs(fname);
                    }
                }

                // insert bảng thông báo
                ThongBao thongbao = new ThongBao();
                thongbao.MaNCS = mancs;
                thongbao.CreatedAt = DateTime.Now;
                thongbao.UpdatedAt = DateTime.Now;
                thongbao.CreatedBy = user.Username;
                thongbao.UpdatedBy = user.Username;

                QuyetDinh quyetdinh = new QuyetDinh();
                quyetdinh = _unitOfWork.GetRepositoryInstance<QuyetDinh>().GetFirstOrDefaultByParameter(o => o.MaNCS == mancs && o.Type == type && o.Tab == tab);
                if (quyetdinh != null)
                {
                    quyetdinh.SoQuyetDinh = SoQDThongTinDeTai;
                    quyetdinh.NgayKy = DateTime.Parse(NgayKyThongTinDeTai);
                    quyetdinh.Tab = tab;
                    quyetdinh.Type = type;
                    quyetdinh.UpdatedAt = DateTime.Now;
                    quyetdinh.UpdatedBy = user.Username;
                    _unitOfWork.GetRepositoryInstance<QuyetDinh>().Update(quyetdinh);

                    thongbao.Title = "Cập nhật quyết định thành lập tiểu ban chấm chuyên đề tiến sĩ";
                }
                else
                {
                    quyetdinh = new QuyetDinh();
                    quyetdinh.SoQuyetDinh = SoQDThongTinDeTai;
                    quyetdinh.NgayKy = DateTime.Parse(NgayKyThongTinDeTai);
                    quyetdinh.MaNCS = mancs;
                    quyetdinh.Tab = tab;
                    quyetdinh.Type = type;
                    quyetdinh.CreatedAt = DateTime.Now;
                    quyetdinh.UpdatedAt = DateTime.Now;
                    quyetdinh.CreatedBy = user.Username;
                    quyetdinh.UpdatedBy = user.Username;
                    _unitOfWork.GetRepositoryInstance<QuyetDinh>().Add(quyetdinh);

                    thongbao.Title = "Thêm mới quyết định thành lập tiểu ban chấm chuyên đề tiến sĩ";
                }
                _unitOfWork.GetRepositoryInstance<ThongBao>().Add(thongbao);
                _unitOfWork.SaveChanges();

                TempData["message"] = "Upload thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi upload: " + ex.Message;
                return Json("Lỗi upload", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateTieuBanChamChuyenNganh(long id, string hoten, string coquancongtac, string vaitrothamgia)
        {
            try
            {
                var user = (CoreAPI.Entity.TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                var tieuban = _unitOfWork.GetRepositoryInstance<TieuBanChamChuyenDe>().GetFirstOrDefaultByParameter(x => x.Id == id);
                if (tieuban == null)
                {
                    return Json("Tiểu ban không tồn tại", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    tieuban.HoTen = hoten;
                    tieuban.CoQuanCongTac = coquancongtac;
                    tieuban.VaiTroThamGia = vaitrothamgia;
                    tieuban.UpdatedAt = DateTime.Now;
                    tieuban.UpdatedBy = user.Username;
                    tieuban.MaNCS = user.Username;
                    _unitOfWork.GetRepositoryInstance<TieuBanChamChuyenDe>().Update(tieuban);

                    // insert bảng thông báo
                    ThongBao thongbao = new ThongBao();
                    thongbao.MaNCS = user.Username;
                    thongbao.CreatedAt = DateTime.Now;
                    thongbao.UpdatedAt = DateTime.Now;
                    thongbao.CreatedBy = user.Username;
                    thongbao.UpdatedBy = user.Username;
                    thongbao.Title = "Cập nhật tiểu ban chấm chuyên ngành";
                    _unitOfWork.GetRepositoryInstance<ThongBao>().Add(thongbao);

                    _unitOfWork.SaveChanges();
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json("Lỗi cập nhật", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ThemMoiTieuBan(string hoten, string coquancongtac, string vaitrothamgia, int type, string mancs)
        {
            try
            {
                TieuBanChamChuyenDe tieuban = new TieuBanChamChuyenDe();
                tieuban = _unitOfWork.GetRepositoryInstance<TieuBanChamChuyenDe>().GetFirstOrDefaultByParameter(o => o.MaNCS == mancs && o.HoTen == hoten && o.Type == type);
                if (tieuban != null)
                {
                    return Json("Tiểu ban đã tồn tại trong tiểu ban", JsonRequestBehavior.AllowGet);
                }
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                tieuban = new TieuBanChamChuyenDe();
                tieuban.HoTen = hoten;
                tieuban.CoQuanCongTac = coquancongtac;
                tieuban.VaiTroThamGia = vaitrothamgia;
                tieuban.Type = type;
                tieuban.CreatedAt = DateTime.Now;
                tieuban.UpdatedAt = DateTime.Now;
                tieuban.CreatedBy = user.Username;
                tieuban.UpdatedBy = user.Username;
                tieuban.MaNCS = mancs;
                _unitOfWork.GetRepositoryInstance<TieuBanChamChuyenDe>().Add(tieuban);

                // insert bảng thông báo
                ThongBao thongbao = new ThongBao();
                thongbao.MaNCS = user.Username;
                thongbao.CreatedAt = DateTime.Now;
                thongbao.UpdatedAt = DateTime.Now;
                thongbao.CreatedBy = user.Username;
                thongbao.UpdatedBy = user.Username;
                thongbao.Title = "Thêm mới tiểu ban chấm chuyên ngành";
                _unitOfWork.GetRepositoryInstance<ThongBao>().Add(thongbao);

                _unitOfWork.SaveChanges();
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Thêm mới lỗi", JsonRequestBehavior.AllowGet);
            }
        }

        //tab 3
        public JsonResult UpdateTrangThaiCTKH(string langui, string ngaygui, string ngaynhanketqua, string ketquaphanbien, string mancs)
        {
            try
            {
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                PhanBienDocLap pbdl = new PhanBienDocLap();
                pbdl.LanGui = langui;
                pbdl.NgayGui = DateTime.Parse(ngaygui);
                pbdl.NgaNhan = DateTime.Parse(ngaynhanketqua);
                pbdl.KetQua = ketquaphanbien;
                pbdl.CreatedAt = DateTime.Now;
                pbdl.UpdatedAt = DateTime.Now;
                pbdl.CreatedBy = user.Username;
                pbdl.UpdatedBy = user.Username;
                pbdl.MaNCS = mancs;
                _unitOfWork.GetRepositoryInstance<PhanBienDocLap>().Add(pbdl);

                // insert bảng thông báo
                ThongBao thongbao = new ThongBao();
                thongbao.MaNCS = user.Username;
                thongbao.CreatedAt = DateTime.Now;
                thongbao.UpdatedAt = DateTime.Now;
                thongbao.CreatedBy = user.Username;
                thongbao.UpdatedBy = user.Username;
                thongbao.Title = "Cập nhật trạng thái CTKH";
                _unitOfWork.GetRepositoryInstance<ThongBao>().Add(thongbao);

                _unitOfWork.SaveChanges();
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Thêm mới lỗi", JsonRequestBehavior.AllowGet);
            }
        }

        // tab 4
        public JsonResult ThemMoiHoiDong(string hoten, string coquancongtac, string vaitrothamgia, int type, int tab, string mancs)
        {
            try
            {
                DanhSachHoiDong tieuban = new DanhSachHoiDong();
                tieuban = _unitOfWork.GetRepositoryInstance<DanhSachHoiDong>().GetFirstOrDefaultByParameter(o => o.MaNCS == mancs && o.HoTen == hoten && o.Type == type && o.Tab == tab);
                if (tieuban != null)
                {
                    return Json("Hội đồng đã tồn tại", JsonRequestBehavior.AllowGet);
                }
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                tieuban = new DanhSachHoiDong();
                tieuban.HoTen = hoten;
                tieuban.CoQuanCongTac = coquancongtac;
                tieuban.VaiTroThamGia = vaitrothamgia;
                tieuban.Type = type;
                tieuban.Tab = tab;
                tieuban.MaNCS = mancs;
                tieuban.CreatedAt = DateTime.Now;
                tieuban.UpdatedAt = DateTime.Now;
                tieuban.CreatedBy = user.Username;
                tieuban.UpdatedBy = user.Username;
                _unitOfWork.GetRepositoryInstance<DanhSachHoiDong>().Add(tieuban);

                // insert bảng thông báo
                ThongBao thongbao = new ThongBao();
                thongbao.MaNCS = user.Username;
                thongbao.CreatedAt = DateTime.Now;
                thongbao.UpdatedAt = DateTime.Now;
                thongbao.CreatedBy = user.Username;
                thongbao.UpdatedBy = user.Username;
                thongbao.Title = "Thêm mới hội đồng bảo vệ cấp bộ môn";
                _unitOfWork.GetRepositoryInstance<ThongBao>().Add(thongbao);

                _unitOfWork.SaveChanges();
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Thêm mới lỗi", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ThemMoiThoiGianKetQuaBaoVe(string tenlanbaove, string giobaove, string ngaybaove = "", string diadiembaove = "", string ketquabaove = "", int type = 0, int tab = 0, string mancs = "")
        {
            try
            {
                KetQuaBaoVe ketqua = new KetQuaBaoVe();
                ketqua = _unitOfWork.GetRepositoryInstance<KetQuaBaoVe>().GetFirstOrDefaultByParameter(o => o.MaNCS == mancs && o.TenLanBaoVe == tenlanbaove && o.Type == type && o.Tab == tab);
                if (ketqua != null)
                {
                    return Json("Tên lần bảo vệ đã tồn tại", JsonRequestBehavior.AllowGet);
                }
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ketqua = new KetQuaBaoVe();
                ketqua.TenLanBaoVe = tenlanbaove;
                ketqua.GioBaoVe = giobaove;
                ketqua.NgayBaoVe = ngaybaove != "" ? DateTime.Parse(ngaybaove) : DateTime.Now;
                ketqua.DiaDiem = diadiembaove;
                ketqua.KetQuaBaoVe1 = ketquabaove;
                ketqua.Type = type;
                ketqua.Tab = tab;
                ketqua.MaNCS = mancs;
                ketqua.CreatedAt = DateTime.Now;
                ketqua.UpdatedAt = DateTime.Now;
                ketqua.CreatedBy = user.Username;
                ketqua.UpdatedBy = user.Username;
                _unitOfWork.GetRepositoryInstance<KetQuaBaoVe>().Add(ketqua);

                // insert bảng thông báo
                ThongBao thongbao = new ThongBao();
                thongbao.MaNCS = user.Username;
                thongbao.CreatedAt = DateTime.Now;
                thongbao.UpdatedAt = DateTime.Now;
                thongbao.CreatedBy = user.Username;
                thongbao.UpdatedBy = user.Username;
                thongbao.Title = "Thêm mới thời gian kết quả bảo vệ cấp bộ môn";
                _unitOfWork.GetRepositoryInstance<ThongBao>().Add(thongbao);

                _unitOfWork.SaveChanges();
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Thêm mới lỗi", JsonRequestBehavior.AllowGet);
            }
        }

        // tab 5
        public JsonResult ThemMoiPhanBienDocLap(string langui, string ngaygui, string ngaynhanketqua, string ketquaphanbien, string mancs)
        {
            try
            {
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                PhanBienDocLap pbdl = new PhanBienDocLap();
                pbdl.LanGui = langui;
                pbdl.NgayGui = DateTime.Parse(ngaygui);
                pbdl.NgaNhan = DateTime.Parse(ngaynhanketqua);
                pbdl.KetQua = ketquaphanbien;
                pbdl.CreatedAt = DateTime.Now;
                pbdl.UpdatedAt = DateTime.Now;
                pbdl.CreatedBy = user.Username;
                pbdl.UpdatedBy = user.Username;
                pbdl.MaNCS = mancs;
                _unitOfWork.GetRepositoryInstance<PhanBienDocLap>().Add(pbdl);

                // insert bảng thông báo
                ThongBao thongbao = new ThongBao();
                thongbao.MaNCS = user.Username;
                thongbao.CreatedAt = DateTime.Now;
                thongbao.UpdatedAt = DateTime.Now;
                thongbao.CreatedBy = user.Username;
                thongbao.UpdatedBy = user.Username;
                thongbao.Title = "Thêm mới kết quả phản biện độc lập";
                _unitOfWork.GetRepositoryInstance<ThongBao>().Add(thongbao);

                _unitOfWork.SaveChanges();
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Thêm mới lỗi", JsonRequestBehavior.AllowGet);
            }
        }

        // tab 6
        public JsonResult ThemMoiGiayTo(string tengiayto, string mancs)
        {
            try
            {
                GiayToBaoVeLuanAn giayto = new GiayToBaoVeLuanAn();
                giayto = _unitOfWork.GetRepositoryInstance<GiayToBaoVeLuanAn>().GetFirstOrDefaultByParameter(o => o.MaNCS == mancs && o.TenGiayTo == tengiayto);
                if (giayto != null)
                {
                    return Json("Giấy tờ này đã tồn tại", JsonRequestBehavior.AllowGet);
                }
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                giayto = new GiayToBaoVeLuanAn();
                giayto.TenGiayTo = tengiayto;
                giayto.MaNCS = mancs;
                giayto.CreatedAt = DateTime.Now;
                giayto.UpdatedAt = DateTime.Now;
                giayto.CreatedBy = user.Username;
                giayto.UpdatedBy = user.Username;
                _unitOfWork.GetRepositoryInstance<GiayToBaoVeLuanAn>().Add(giayto);

                // insert bảng thông báo
                ThongBao thongbao = new ThongBao();
                thongbao.MaNCS = user.Username;
                thongbao.CreatedAt = DateTime.Now;
                thongbao.UpdatedAt = DateTime.Now;
                thongbao.CreatedBy = user.Username;
                thongbao.UpdatedBy = user.Username;
                thongbao.Title = "Thêm mới giấy tờ bảo vệ cấp trường";
                _unitOfWork.GetRepositoryInstance<ThongBao>().Add(thongbao);

                _unitOfWork.SaveChanges();
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Thêm mới lỗi", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ThemMoiThamDinh()
        {
            try
            {
                var user = (CoreAPI.Entity.TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                string mancs = Request.Form["hdfMaNCS"] != null ? Request.Form["hdfMaNCS"].ToString() : "";
                string txtDotThamDinh = Request.Form["txtDotThamDinh"] != null ? Request.Form["txtDotThamDinh"].ToString() : "";
                string txtLoaiThamDinh = Request.Form["txtLoaiThamDinh"] != null ? Request.Form["txtLoaiThamDinh"].ToString() : "";
                string txtCongVanSo = Request.Form["txtCongVanSo"] != null ? Request.Form["txtCongVanSo"].ToString() : "";
                DateTime txtCongVanNgayKy = Request.Form["txtCongVanNgayKy"] != null ? DateTime.Parse(Request.Form["txtCongVanNgayKy"].ToString()) : DateTime.Now;
                string txtKetQuaThamDinh = Request.Form["txtKetQuaThamDinh"] != null ? Request.Form["txtKetQuaThamDinh"].ToString() : "";
                HoSoThamDinh hosothamdinh = _unitOfWork.GetRepositoryInstance<HoSoThamDinh>().GetFirstOrDefaultByParameter(o => o.MaNCS == mancs);
                if (hosothamdinh == null)
                {
                    hosothamdinh = new HoSoThamDinh();
                    hosothamdinh.MaNCS = mancs;
                    hosothamdinh.DotTĐ = txtDotThamDinh;
                    hosothamdinh.LoaiThamDinh = txtLoaiThamDinh;
                    hosothamdinh.SoCV = int.Parse(txtCongVanSo);
                    hosothamdinh.NgayKy = txtCongVanNgayKy;
                    hosothamdinh.KetQua = txtKetQuaThamDinh;

                    hosothamdinh.CreatedAt = DateTime.Now;
                    hosothamdinh.CreatedBy = user.Username;
                    hosothamdinh.UpdatedAt = DateTime.Now;
                    hosothamdinh.UpdatedBy = user.Username;
                    _unitOfWork.GetRepositoryInstance<HoSoThamDinh>().Update(hosothamdinh);

                    // insert bảng thông báo
                    ThongBao thongbao = new ThongBao();
                    thongbao.MaNCS = user.Username;
                    thongbao.CreatedAt = DateTime.Now;
                    thongbao.UpdatedAt = DateTime.Now;
                    thongbao.CreatedBy = user.Username;
                    thongbao.UpdatedBy = user.Username;
                    thongbao.Title = "Thêm mới hồ sơ thẩm định";
                    _unitOfWork.GetRepositoryInstance<ThongBao>().Add(thongbao);

                    _unitOfWork.SaveChanges();
                }

                TempData["message"] = "Thêm mới thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi thêm mới: " + ex.Message;
                return Json("Lỗi thêm mới", JsonRequestBehavior.AllowGet);
            }
        }

        //tab 7

        public async Task<JsonResult> UpdateStatus(int id, string maHV, int status)
        {
            try
            {
                var ttt_NCS = _unitOfWork.GetRepositoryInstance<TruongThongTin_NCS>().GetFirstOrDefaultByParameter(x => x.TruongThongTinId == id && x.MaNCS == maHV);
                var ttt = _unitOfWork.GetRepositoryInstance<TruongThongTin>().GetFirstOrDefaultByParameter(x => x.Id == id);
                if (ttt_NCS != null)
                {
                    ttt_NCS.Status = status;
                    _unitOfWork.GetRepositoryInstance<TruongThongTin_NCS>().Update(ttt_NCS);

                    // insert bảng thông báo
                    ThongBao thongbao = new ThongBao();
                    thongbao.MaNCS = maHV;
                    thongbao.CreatedAt = DateTime.Now;
                    thongbao.UpdatedAt = DateTime.Now;
                    var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                    thongbao.CreatedBy = user.Username;
                    thongbao.UpdatedBy = user.Username;
                    if (ttt != null)
                    {
                        if (status == 1)
                        {
                            thongbao.Title = "Hoàn thành " + ttt.TenTruongThongTin;
                        }
                        else
                        {
                            thongbao.Title = "Thực hiện lại" + ttt.TenTruongThongTin;
                        }
                    }

                    _unitOfWork.GetRepositoryInstance<ThongBao>().Add(thongbao);

                    _unitOfWork.SaveChanges();
                    TempData["message"] = "Cập nhật thành công!";
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["error"] = "Không tìm thấy trường thông tin NCS";
                    return Json("Lỗi thêm mới", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi cập nhật: " + ex.Message;
                return Json("Lỗi cập nhật", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mancs"></param>
        /// <param name="status">0: chưa hoàn thành, thực hiện lại , 1: Xác nhận hoàn thành hồ sơ</param>
        /// <param name="buoc">bước thực hiện</param>
        /// <returns></returns>
        public async Task<JsonResult> UpdateProcess(string mancs, int status, int buoc, string type)
        {
            try
            {
                // insert bảng thông báo
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ThongBao thongbao = new ThongBao();
                thongbao.MaNCS = mancs;
                thongbao.CreatedAt = DateTime.Now;
                thongbao.UpdatedAt = DateTime.Now;
                thongbao.CreatedBy = user.Username;
                thongbao.UpdatedBy = user.Username;

                BaoVe_NCS baove_ncs = _unitOfWork.GetRepositoryInstance<BaoVe_NCS>().GetFirstOrDefaultByParameter(x => x.MaNCS == mancs);
                if (baove_ncs != null)
                {
                    if (type == "tab")
                    {
                        if (buoc == 1)
                        {
                            baove_ncs.Buoc1 = status;
                            thongbao.Title = "Hoàn thành bước bảo vệ tổng quan";
                        }
                        else if (buoc == 2)
                        {
                            baove_ncs.Buoc2 = status;
                            thongbao.Title = "Hoàn thành bước bảo vệ 3 chuyên đề NCS";
                        }
                        else if (buoc == 3)
                        {
                            baove_ncs.Buoc3 = status;
                            thongbao.Title = "Hoàn thành bước công trình KH";
                        }
                        else if (buoc == 4)
                        {
                            baove_ncs.Buoc4 = status;
                            thongbao.Title = "Hoàn thành bước bảo vệ cấp bội môn";
                        }
                        else if (buoc == 5)
                        {
                            baove_ncs.Buoc5 = status;
                            thongbao.Title = "Hoàn thành bước phản  biện độc lập";
                        }
                        else if (buoc == 6)
                        {
                            baove_ncs.Buoc6 = status;
                            thongbao.Title = "Hoàn thành bước bảo vệ cấp trường";
                        }
                        else if (buoc == 7)
                        {
                            baove_ncs.Buoc7 = status;
                            thongbao.Title = "Hoàn thành bước sau bảo vệ";
                        }

                        if (baove_ncs.Buoc1 == 1 && baove_ncs.Buoc2 == 1 && baove_ncs.Buoc3 == 1 && baove_ncs.Buoc4 == 1 && baove_ncs.Buoc5 == 1 && baove_ncs.Buoc6 == 1 && baove_ncs.Buoc7 == 1)
                        {
                            baove_ncs.Status = 1;
                            thongbao.Title = "Hoàn thành hồ sơ online";
                        }
                        else
                        {
                            baove_ncs.Status = 0;
                        }
                    }
                    else if (type == "all")
                    {
                        baove_ncs.Status = 1;
                        baove_ncs.Buoc1 = 1;
                        baove_ncs.Buoc2 = 1;
                        baove_ncs.Buoc3 = 1;
                        baove_ncs.Buoc4 = 1;
                        baove_ncs.Buoc5 = 1;
                        baove_ncs.Buoc6 = 1;
                        baove_ncs.Buoc7 = 1;

                        thongbao.Title = "Hoàn thành hồ sơ online";
                    }

                    baove_ncs.CreatedAt = DateTime.Now;
                    baove_ncs.UpdatedAt = DateTime.Now;
                    baove_ncs.CreatedBy = user.Username;
                    baove_ncs.UpdatedBy = user.Username;
                    _unitOfWork.GetRepositoryInstance<BaoVe_NCS>().Update(baove_ncs);
                    _unitOfWork.GetRepositoryInstance<ThongBao>().Add(thongbao);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    TempData["error"] = "Không tìm thấy bản ghi";
                    return Json("Không tìm thấy bản ghi", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi cập nhật: " + ex.Message;
                return Json("Lỗi cập nhật", JsonRequestBehavior.AllowGet);
            }

            TempData["message"] = "Cập nhật thành công!";
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

    }
}