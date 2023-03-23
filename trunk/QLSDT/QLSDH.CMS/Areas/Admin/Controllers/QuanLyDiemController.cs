using CoreAPI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TEMIS.CMS.Areas.Admin.Models;
using TEMIS.CMS.Common;
using TEMIS.CMS.Repository;
using TEMIS.Model;
using TEMIS.CMS.Areas.Admin.Models;
using System.IO;
using Excel;
using System.Data;
using System.Net;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace TEMIS.CMS.Areas.Admin.Controllers
{
    [AuditAction]
    public class QuanLyDiemController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        private TEMIS_systemEntities db = new TEMIS_systemEntities();
        public string urlFile = "FileMauExcel\\";
        public string parthdowload = "Upload\\FileBMDowload\\";
        //public string url_download = "http://14.225.5.64:8765/upload/FileBMDowload/";
        public string url_download = "http://qlncs.hnue.edu.vn/upload/FileBMDowload/";

        // GET: Admin/QuanLyDiem
        public async Task<ActionResult> Test()
        {            
            return View();
        }
       
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            List<OrganizationInfo> list = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = list;

            List<KhoaHoc> listKH = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
            ViewBag.ListKhoaHoc = listKH;

            var loginInfo = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
            int staffid = int.Parse(loginInfo.StaffId.ToString());
            var user = await CoreAPI.CoreAPI.GetThongTinGiangVien(staffid);
            int department = user.DepartmentId;
            var idkhoa = list.Where(x => x.Id == department).FirstOrDefault();
            if (idkhoa != null)
            {
                ViewBag.IDKHOA = idkhoa;
            }
            else
            {
                ViewBag.IDKHOA = -1;
            }
            ViewBag.IDKHOA = 78;
            return View();
        }

        public async Task<ActionResult> TraCuuDiem()
        {
            List<OrganizationInfo> list = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = list;

            List<KhoaHoc> listKH = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
            ViewBag.ListKhoaHoc = listKH;

            var loginInfo = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
            int staffid = int.Parse(loginInfo.StaffId.ToString());
            var user = await CoreAPI.CoreAPI.GetThongTinGiangVien(staffid);
            int department = user.DepartmentId;
            var idkhoa = list.Where(x => x.Id == department).FirstOrDefault();
            if (idkhoa != null)
            {
                ViewBag.IDKHOA = idkhoa;
            }
            else
            {
                ViewBag.IDKHOA = -1;
            }
            ViewBag.IDKHOA = 78;
            return View();
        }

        public async Task<ActionResult> LoaDataTraCuuDiem(int khoaid = 0, int nganhid = 0, int khoahocid = 0, string hoten = "")
        {
            try
            {
                List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
                List<DiemThiViewModel> listDiemView = (from diem in db.Diem
                                                       join ncs in db.NCS on diem.MaHocVien equals ncs.Ma
                                                       where (nganhid == 0 || ncs.NganhId == nganhid) && (khoaid == 0 || ncs.KhoaId == khoaid) && (khoahocid == 0 || ncs.KHoaHocId == khoahocid) && (hoten == "" || ncs.HoTen.Contains(hoten))
                                                       select new DiemThiViewModel
                                                       {
                                                           Id = diem.Id,
                                                           MaHocVien = diem.MaHocVien,
                                                           TenHocVien = diem.TenHocVien,
                                                           HocPhanId = diem.HocPhanId.ToString(),
                                                           TenHocPhan = "",
                                                           SoDiem = diem != null && diem.SoDiem != null ? diem.SoDiem.Value : 0,
                                                           DiemHP1 = diem != null && diem.DiemHP1 != null ? diem.DiemHP1.Value : 0,
                                                           DiemHP2 = diem != null && diem.DiemHP2 != null ? diem.DiemHP2.Value : 0,
                                                           DiemHP3 = diem != null && diem.DiemHP3 != null ? diem.DiemHP3.Value : 0,
                                                           DiemHP4 = diem != null && diem.DiemHP4 != null ? diem.DiemHP4.Value : 0,
                                                           KhoaId = ncs.KhoaId.ToString(),
                                                           TenKhoa = "",
                                                           KhoaHocId = ncs.KHoaHocId.ToString(),
                                                           TenKhoaHoc = "",
                                                           CreatedAt = diem.CreatedAt,
                                                           UpdatedAt = diem.UpdatedAt,
                                                           CreatedBy = diem.CreatedBy,
                                                           UpdatedBy = diem.UpdatedBy
                                                       }).ToList();

                foreach (var item in listDiemView)
                {
                    int hpId = int.Parse(item.HocPhanId);
                    int khoaId = int.Parse(item.KhoaId);
                    int khoahocId = int.Parse(item.KhoaHocId);
                    item.TenHocPhan = item.HocPhanId != "" ? _unitOfWork.GetRepositoryInstance<HocPhan>().GetFirstOrDefaultByParameter(o => o.Id == hpId).TenHocPhan : "";
                    item.TenKhoa = item.KhoaId != "" ? listKhoa.Where(o => o.Id == khoaId).SingleOrDefault().Name : "";
                    item.TenKhoaHoc = item.KhoaHocId != "" ? _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetFirstOrDefaultByParameter(o => o.Id == khoahocId).MaKhoa : "";
                }

                return PartialView("_PartialTraCuuDiemThi", listDiemView);
            }
            catch (Exception ex)
            {
                return PartialView("_PartialTraCuuDiemThi", null);
            }
        }


        public async Task<JsonResult> DownLoadExcel(int khoaid = 0, int nganhid = 0, int khoahocid = 0, string hoten = "")
        {
            try
            {
                List<OrganizationInfo> listKhoa = await CoreAPI.CoreAPI.GetListKhoa();
                List<DiemThiViewModel> listDiemView = (from diem in db.Diem
                                                       join ncs in db.NCS on diem.MaHocVien equals ncs.Ma
                                                       where (nganhid == 0 || ncs.NganhId == nganhid) && (khoaid == 0 || ncs.KhoaId == khoaid) && (khoahocid == 0 || ncs.KHoaHocId == khoahocid) && (hoten == "" || ncs.HoTen.Contains(hoten))
                                                       select new DiemThiViewModel
                                                       {
                                                           Id = diem.Id,
                                                           MaHocVien = diem.MaHocVien,
                                                           TenHocVien = diem.TenHocVien,
                                                           HocPhanId = diem.HocPhanId.ToString(),
                                                           TenHocPhan = "",
                                                           SoDiem = diem != null && diem.SoDiem != null ? diem.SoDiem.Value : 0,
                                                           DiemHP1 = diem != null && diem.DiemHP1 != null ? diem.DiemHP1.Value : 0,
                                                           DiemHP2 = diem != null && diem.DiemHP2 != null ? diem.DiemHP2.Value : 0,
                                                           DiemHP3 = diem != null && diem.DiemHP3 != null ? diem.DiemHP3.Value : 0,
                                                           DiemHP4 = diem != null && diem.DiemHP4 != null ? diem.DiemHP4.Value : 0,
                                                           KhoaId = ncs.KhoaId.ToString(),
                                                           TenKhoa = "",
                                                           KhoaHocId = ncs.KHoaHocId.ToString(),
                                                           TenKhoaHoc = "",
                                                           CreatedAt = diem.CreatedAt,
                                                           UpdatedAt = diem.UpdatedAt,
                                                           CreatedBy = diem.CreatedBy,
                                                           UpdatedBy = diem.UpdatedBy
                                                       }).ToList();

                foreach (var item in listDiemView)
                {
                    int hpId = int.Parse(item.HocPhanId);
                    int khoaId = int.Parse(item.KhoaId);
                    int khoahocId = int.Parse(item.KhoaHocId);
                    item.TenHocPhan = item.HocPhanId != "" ? _unitOfWork.GetRepositoryInstance<HocPhan>().GetFirstOrDefaultByParameter(o => o.Id == hpId).TenHocPhan : "";
                    item.TenKhoa = item.KhoaId != "" ? listKhoa.Where(o => o.Id == khoaId).SingleOrDefault().Name : "";
                    item.TenKhoaHoc = item.KhoaHocId != "" ? _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetFirstOrDefaultByParameter(o => o.Id == khoahocId).MaKhoa : "";
                }

                #region
                using (ExcelPackage p = new ExcelPackage())
                {

                    p.Workbook.Properties.Author = "Admin";
                    p.Workbook.Properties.Title = "Báo cáo thống kê";
                    int j = 1;

                    #region Tạo sheet 1 toàn trường (theo khoá)
                        p.Workbook.Worksheets.Add($"DS Diem");
                        ExcelWorksheet ws = p.Workbook.Worksheets[j];
                        ws.Cells.Style.WrapText = true;
                        string[] arrColumnHeader = {"STT","Mã HV","Họ và tên",
                    "Khoa","Khóa","HP1","HP2","HP3","HP4"};

                        var countColHeader = arrColumnHeader.Count();

                        //ws.Cells[1, 1].Value = "BỘ GIÁO DỤC VÀ ĐÀO TẠO";
                        //ws.Cells[1, 1, 1, 3].Merge = true;
                        //ws.Cells[2, 1].Value = "Công ty Cổ phần Hệ thống 2B";
                        //ws.Cells[2, 1, 2, 5].Merge = true;

                        //ws.Cells[1, 6].Value = "CỘNG HOÀ XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                        //ws.Cells[1, 6, 1, 9].Merge = true;
                        //ws.Cells[1, 6, 1, 9].Style.Font.Bold = true;
                        //ws.Cells[1, 6, 1, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //ws.Cells[2, 6].Value = "ĐỘC LẬP - TỰ DO - HẠNH PHÚC";
                        //ws.Cells[2, 6, 2, 9].Merge = true;
                        //ws.Cells[2, 6, 2, 9].Style.Font.Bold = true;
                        //ws.Cells[2, 6, 2, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //ws.Cells[3, 1].Value = $"DANH SÁCH NGHIÊN CỨU SINH KHOÁ {dot.MaKhoaHoc} - NĂM {dot.NgayBatDau.Value.Year} ({tendot})";
                        //ws.Cells[3, 1, 3, countColHeader].Merge = true;
                        //ws.Cells[3, 1, 3, countColHeader].Style.Font.Bold = true;
                        //ws.Cells[3, 1, 3, countColHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //ws.Cells[4, 1].Value = $"(Kèm theo QĐ số    / QĐ-ĐHSP HN, ngày  tháng   năm    của Hiệu trưởng trường ĐHSP Hà Nội";
                        //ws.Cells[4, 1, 4, countColHeader].Merge = true;
                        //ws.Cells[4, 1, 4, countColHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //ws.Cells[4, 1, 4, countColHeader].Style.Font.Italic = true;

                        int colIndex = 1;
                        int rowIndex = 1;
                        foreach (var item in arrColumnHeader)
                        {
                            var cell = ws.Cells[rowIndex, colIndex];
                            var fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            var colFromHex = System.Drawing.ColorTranslator.FromHtml("#78746b");
                            fill.BackgroundColor.SetColor(colFromHex);
                            cell.Value = item;

                            colIndex++;
                        }
                        int sothutu = 0;
                        foreach (var item in listDiemView)
                        {
                            colIndex = 1;
                            rowIndex++;
                            sothutu++;
                            ws.Cells[rowIndex, colIndex++].Value = sothutu;
                            ws.Cells[rowIndex, colIndex++].Value = item.MaHocVien;
                            ws.Cells[rowIndex, colIndex++].Value = item.TenHocVien;
                            ws.Cells[rowIndex, colIndex++].Value = item.TenKhoa;
                            ws.Cells[rowIndex, colIndex++].Value = item.TenKhoaHoc;
                            ws.Cells[rowIndex, colIndex++].Value = item.DiemHP1;
                            ws.Cells[rowIndex, colIndex++].Value = item.DiemHP2;
                            ws.Cells[rowIndex, colIndex++].Value = item.DiemHP3;
                            ws.Cells[rowIndex, colIndex++].Value = item.DiemHP4;
                    }
                        //rowIndex++;
                        //ws.Cells[rowIndex, 2].Value = "(Danh sách gồm có";
                        //ws.Cells[rowIndex, 2, rowIndex, 3].Merge = true;
                        //ws.Cells[rowIndex, 4].Value = lst.Count().ToString();
                        //ws.Cells[rowIndex, 5].Value = "NCS)";
                        //ws.Row(rowIndex).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        ws.Cells[1, 1, rowIndex - 1, countColHeader].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws.Cells[1, 1, rowIndex - 1, countColHeader].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells[1, 1, rowIndex - 1, countColHeader].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells[1, 1, rowIndex - 1, countColHeader].Style.Border.Left.Style = ExcelBorderStyle.Thin;

                        ws.Cells[1, 1, 1, countColHeader].Style.Border.Top.Style = ExcelBorderStyle.Thick;
                        ws.Cells[1, countColHeader, rowIndex, countColHeader].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                        ws.Cells[rowIndex, 1, rowIndex, countColHeader].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                        ws.Cells[1, 1, rowIndex, 1].Style.Border.Left.Style = ExcelBorderStyle.Thick;

                        ws.Cells[1, 1, 1, countColHeader].Style.Border.Top.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));
                        ws.Cells[1, countColHeader, rowIndex, countColHeader].Style.Border.Right.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));
                        ws.Cells[rowIndex, 1, rowIndex, countColHeader].Style.Border.Bottom.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));
                        ws.Cells[1, 1, rowIndex, 1].Style.Border.Left.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#090858"));



                        #region set style
                        ws.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //ws.Cells[5, 2, rowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.None;
                    //ws.Cells[5, 3, rowIndex, 3].Style.Border.Left.Style = ExcelBorderStyle.None;
                    //ws.Cells.Style.Font.Name = "Times New Roman";
                    //ws.Cells[1, 1, 2, countColHeader].Style.Font.Size = 12;
                    //ws.Cells[3, 1, 3, countColHeader].Style.Font.Size = 14;
                    //ws.Cells[4, 1, 4, countColHeader].Style.Font.Size = 11;
                    //ws.Cells[4, 1, 4, countColHeader].Style.Font.Italic = true;
                    //ws.Cells[5, 1, 5, countColHeader].Style.Font.Bold = true;
                    //ws.Cells[5, 1, rowIndex, countColHeader].Style.Font.Size = 12;
                        ws.Cells.Style.Font.Size = 12;
                        ws.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //ws.Column(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        //ws.Column(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        //ws.Column(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Column(1).Width = 8;
                        ws.Column(2).Width = 16;
                        ws.Column(3).Width = 18;
                        ws.Column(4).Width = 30;
                        ws.Column(5).Width = 10;
                        ws.Column(6).Width = 12;
                        ws.Column(7).Width = 12;
                        ws.Column(8).Width = 12;
                        ws.Column(9).Width = 12;
                        #endregion

                        //rowIndex++;
                        //ws.Cells[rowIndex, 7].Value = "HIỆU TRƯỞNG";
                        //ws.Cells[rowIndex, 7, rowIndex, 8].Merge = true;
                        //ws.Cells[rowIndex, 7, rowIndex, 8].Style.Font.Bold = true;
                        //ws.Cells[rowIndex, 7, rowIndex, 8].Style.Font.Size = 12;

                        //ws.Cells[rowIndex + 5, 7].Value = "GS.TS Nguyễn Văn Minh";
                        //ws.Cells[rowIndex + 5, 7, rowIndex + 5, 8].Merge = true;
                        //ws.Cells[rowIndex + 5, 7, rowIndex + 5, 8].Style.Font.Bold = true;
                        //ws.Cells[rowIndex + 5, 7, rowIndex + 5, 8].Style.Font.Size = 12;
                        //j++;
                    #endregion



                    byte[] bin = p.GetAsByteArray();
                    string name = "Danhsachdiem";
                    string filename_new = $"{name}_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.xls";
                    var filePath = Server.MapPath("~/" + parthdowload + filename_new);
                    //File(byteArray, "", $"{filePath}");
                    System.IO.File.WriteAllBytes(filePath, bin);

                    var systemPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    var filePathdowload = System.IO.Path.Combine(systemPath, filename_new);
                    // Create a new WebClient instance.
                    WebClient myWebClient = new WebClient();

                    // Download the Web resource and save it into the current filesystem folder.
                    myWebClient.DownloadFile(filePath, filePathdowload);

                    return Json(url_download + filename_new, JsonRequestBehavior.AllowGet);


                }
                #endregion
            }
            catch (Exception ex)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DiemHP(long id)
        {
            string ma_ncs = "";
            List<HocPhan_NCS> Result = new List<HocPhan_NCS>();
            var ncs = _unitOfWork.GetRepositoryInstance<NCS>().GetFirstOrDefaultByParameter(x => x.Id == id);
            if (ncs != null)
            {
                ma_ncs = ncs.Ma;
                Result = _unitOfWork.GetRepositoryInstance<HocPhan_NCS>().GetListByParameter(x => x.MaNCS.Equals(ma_ncs) && x.Status == true).ToList();
            }
            return View(Result);
        }
        public ActionResult DiemHV(long id, string khoa, string khoahoc)
        {
            string ma_hp = "";
            List<DiemThiNCSTheoHocPhanViewModel> Result_ = new List<DiemThiNCSTheoHocPhanViewModel>();
            List<HocPhan_NCS> Result = new List<HocPhan_NCS>();
            var hocphan = _unitOfWork.GetRepositoryInstance<HocPhan>().GetFirstOrDefaultByParameter(x => x.Id == id);
            if (hocphan != null)
            {
                ma_hp = hocphan.MaHocPhan;
                Result = _unitOfWork.GetRepositoryInstance<HocPhan_NCS>().GetListByParameter(x => x.MaHocPhan.Equals(ma_hp) && x.Status == true).ToList();
            }
            if (Result.Count>0)
            {
                foreach (var item in Result)
                {
                    DiemThiNCSTheoHocPhanViewModel model = new DiemThiNCSTheoHocPhanViewModel();
                    var ncs = _unitOfWork.GetRepositoryInstance<NCS>().GetFirstOrDefaultByParameter(x => x.Ma.Equals(item.MaNCS) == true);
                    model.TenNCS = ncs.HoTen;
                    model.Id = item.Id;
                    model.MaHocPhan = item.MaHocPhan;
                    model.MaNCS = item.MaNCS;
                    model.Status = item.Status;
                    model.TenHocPhan = item.TenHocPhan;
                    model.TinChi = item.TinChi;
                    model.TuChon = item.TuChon;
                    model.UpdatedAt = item.UpdatedAt;
                    model.UpdatedBy = item.UpdatedBy;
                    model.CreatedAt = item.CreatedAt;
                    model.CreatedBy = item.CreatedBy;
                    model.Diem = item.Diem;
                    model.DiemDieuKien = item.DiemDieuKien;
                    model.DiemThi = item.DiemThi;
                    Result_.Add(model);
                }
            }
            return View(Result_);
        }
        public JsonResult SaveDiemNCSTheoMon(int id, float diemdk, float diemthi)
        {
            try
            {
                var diem_ncs = _unitOfWork.GetRepositoryInstance<HocPhan_NCS>().GetFirstOrDefaultByParameter(x => x.Id == id);
                if (diem_ncs == null)
                {
                    return Json("Không tìm thấy thông tin môn học của NCS", JsonRequestBehavior.AllowGet);
                }
                if (diemdk >= 0)
                    diem_ncs.DiemDieuKien = Math.Round(diemdk, 2);
                if (diemthi >= 0)
                    diem_ncs.DiemThi = Math.Round(diemthi, 2);

                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                diem_ncs.UpdatedBy = user.Username;
                diem_ncs.UpdatedAt = DateTime.Now;
                _unitOfWork.GetRepositoryInstance<HocPhan_NCS>().Update(diem_ncs);
                _unitOfWork.SaveChanges();

                if (diem_ncs.DiemDieuKien >= 0 && diem_ncs.DiemThi >= 0)
                {
                    var checkhp = _unitOfWork.GetRepositoryInstance<HocPhan>().GetFirstOrDefaultByParameter(x => x.MaHocPhan == diem_ncs.MaHocPhan);
                    if (checkhp != null)
                    {
                        double? diemtongkethp = (diem_ncs.DiemDieuKien * 3 + diem_ncs.DiemThi * 7) / 10;
                        Diem diemhp = new Diem();
                        diemhp = _unitOfWork.GetRepositoryInstance<Diem>().GetFirstOrDefaultByParameter(x => x.MaHocVien == diem_ncs.MaNCS);
                        int thuochp = checkhp.LoaiHP.Value;
                        if (diemhp != null)
                        {

                            if (thuochp == 1)
                            {
                                diemhp.DiemHP1 = diemtongkethp;
                            }

                            else if (thuochp == 2)
                            {
                                diemhp.DiemHP2 = diemtongkethp;
                            }
                            else if (thuochp == 3)
                            {
                                diemhp.DiemHP3 = diemtongkethp;
                            }
                            else if (thuochp == 4)
                            {
                                diemhp.DiemHP4 = diemtongkethp;
                            }
                            diemhp.UpdatedAt = DateTime.Now;
                            diemhp.UpdatedBy = user.Username;
                            _unitOfWork.GetRepositoryInstance<Diem>().Update(diemhp);
                            _unitOfWork.SaveChanges();
                        }
                        else
                        {
                            NCS ncs = _unitOfWork.GetRepositoryInstance<NCS>().GetFirstOrDefaultByParameter(x => x.Ma == diem_ncs.MaNCS);
                            if (ncs != null)
                            {
                                diemhp = new Diem();
                                diemhp.MaHocVien = diem_ncs.MaNCS;
                                diemhp.HocPhanId = checkhp.Id;
                                diemhp.CreatedAt = DateTime.Now;
                                diemhp.UpdatedAt = DateTime.Now;
                                diemhp.CreatedBy = user.Username;
                                diemhp.UpdatedBy = user.Username;
                                diemhp.TenHocVien = ncs.HoTen;
                                diemhp.HocVienId = ncs.Id;
                                if (thuochp == 1)
                                {
                                    diemhp.DiemHP1 = diemtongkethp;
                                }
                                else if (thuochp == 2)
                                {
                                    diemhp.DiemHP2 = diemtongkethp;
                                }
                                else if (thuochp == 3)
                                {
                                    diemhp.DiemHP3 = diemtongkethp;
                                }
                                else if (thuochp == 4)
                                {
                                    diemhp.DiemHP4 = diemtongkethp;
                                }
                                _unitOfWork.GetRepositoryInstance<Diem>().Add(diemhp);
                                _unitOfWork.SaveChanges();
                            }

                        }
                    }

                }
                return Json("OK", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(controllerName, actionName, user == null ? string.Empty : user.Username, ex);
                return Json("cập nhật giá tri lỗi", JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> LoadDataHP_mon(int idKhoa, int idNganh, int idKhoaHoc)
        {
            try
            {

                List<DiemNCSTheoHocPhanViewModel> str_Result = new List<DiemNCSTheoHocPhanViewModel>();
                List<HocPhan> list_hp = new List<HocPhan>();
                if (idNganh > 0)
                {
                    NganhDaoTao nganh = _unitOfWork.GetRepositoryInstance<NganhDaoTao>().GetFirstOrDefaultByParameter(o => o.MaNganh.Equals(idNganh.ToString()) == true);
                    list_hp = _unitOfWork.GetRepositoryInstance<HocPhan>().GetListByParameter(x => x.NganhId == nganh.Id/* && x.KhoaHocId == idKhoaHoc*/).ToList();
                }
                else if (idKhoa > 0)
                {
                    list_hp = _unitOfWork.GetRepositoryInstance<HocPhan>().GetListByParameter(x => x.KhoaId == idKhoa/* && x.KhoaHocId == idKhoaHoc*/).ToList();
                }
                else
                {
                    //list_hp = _unitOfWork.GetRepositoryInstance<HocPhan>().GetListByParameter(x => x.KhoaHocId == idKhoaHoc).ToList();
                }

                if (list_hp.Count == 0)
                {
                    //TempData["message"] = "Không tìm thấy kết quả nào";
                }
                else
                {
                    foreach (var item in list_hp)
                    {
                        DiemNCSTheoHocPhanViewModel lstMon = new DiemNCSTheoHocPhanViewModel();
                        lstMon.Id = item.Id;
                        var lstkhoa = await CoreAPI.CoreAPI.GetListKhoa();
                        int khoa_id = item.KhoaId.Value;
                        int nganh_id = item.NganhId.Value;
                        int khoahoc_id = item.KhoaHocId.Value;
                        lstMon.Khoa = lstkhoa.Where(x => x.Id == khoa_id).FirstOrDefault().Name.ToString();
                        lstMon.Nganh = _unitOfWork.GetRepositoryInstance<NganhDaoTao>().GetFirstOrDefaultByParameter(x => x.Id == nganh_id).TenNganh;
                        lstMon.KhoaHoc = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetFirstOrDefaultByParameter(x => x.Id == khoahoc_id).MaKhoa;
                        lstMon.MaHP = item.MaHocPhan;
                        lstMon.TenHP = item.TenHocPhan;
                        str_Result.Add(lstMon);
                    }
                }

                return PartialView("_PartialMonHoc", str_Result);

            }
            catch (Exception ex)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(controllerName, actionName, user == null ? string.Empty : user.Username, ex);
                return PartialView("_PartialMonHoc", null);
            }
        }


        public ActionResult QuanLyDT(long id)
        {
            string ma_ncs = "";
            List<HocPhan_NCS> Result = new List<HocPhan_NCS>();
            var ncs = _unitOfWork.GetRepositoryInstance<NCS>().GetFirstOrDefaultByParameter(x => x.Id == id);
            if (ncs != null)
            {
                ma_ncs = ncs.Ma;
                Result = _unitOfWork.GetRepositoryInstance<HocPhan_NCS>().GetListByParameter(x => x.MaNCS.Equals(ma_ncs)).ToList();
            }
            List<HocPhan> listHocphan = _unitOfWork.GetRepositoryInstance<HocPhan>().GetListByParameter(o => o.KhoaId == ncs.KhoaId).ToList(); // lấy danh sách hp theo Khoa đăng ký dự tuyển
            ViewBag.HOCPHAN = listHocphan;
            return View(Result);
        }

        public async System.Threading.Tasks.Task<ActionResult> QuanLyLop()
        {
            List<OrganizationInfo> list = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = list;
            List<KhoaHoc> listKH = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
            ViewBag.ListKhoaHoc = listKH;
            return View();
        }
        public async System.Threading.Tasks.Task<ActionResult> Themmoilop()
        {
            List<OrganizationInfo> list = await CoreAPI.CoreAPI.GetListKhoa();
            ViewBag.ListKhoa = list;
            List<KhoaHoc> listKH = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetAllRecords().OrderByDescending(x => x.Id).ToList();
            ViewBag.ListKhoaHoc = listKH;
            return View();
        }
        public async Task<ActionResult> LoadDataNCS(int idKhoa, string maNganh, int idKhoaHoc, string strtenNCS)
        {
            try
            {
                List<DiemNCSViewModel> str_Result = new List<DiemNCSViewModel>();
                List<NCS> list_ncs = new List<NCS>();
                var check_nganh = _unitOfWork.GetRepositoryInstance<NganhDaoTao>().GetFirstOrDefaultByParameter(x => x.MaNganh == maNganh);
                int idNganh = 0;
                if (check_nganh != null)
                {
                    idNganh = check_nganh.Id;
                }
                if (idNganh > 0)
                {
                    list_ncs = _unitOfWork.GetRepositoryInstance<NCS>().GetListByParameter(x => x.NganhId == idNganh /*&& x.KHoaHocId == idKhoaHoc*/).ToList();
                }
                else if (idKhoa > 0)
                {
                    list_ncs = _unitOfWork.GetRepositoryInstance<NCS>().GetListByParameter(x => x.KhoaId == idKhoa /*&& x.KHoaHocId == idKhoaHoc*/).ToList();
                }
                else
                {
                    list_ncs = _unitOfWork.GetRepositoryInstance<NCS>().GetListByParameter(x => x.KHoaHocId == idKhoaHoc).ToList();
                }
                if (!string.IsNullOrEmpty(strtenNCS))
                {
                    list_ncs = list_ncs.Where(x => x.HoTen.Contains(strtenNCS)).OrderByDescending(x => x.Id).ToList();
                }
                if (list_ncs.Count == 0)
                {
                    TempData["message"] = "Không tìm thấy kết quả nào";
                }
                else
                {
                    foreach (var item in list_ncs)
                    {
                        DiemNCSViewModel diemNCS = new DiemNCSViewModel();
                        diemNCS.Id = item.Id;
                        var lstkhoa = await CoreAPI.CoreAPI.GetListKhoa();
                        int khoa_id = item.KhoaId.Value;
                        int nganh_id = item.NganhId.Value;
                        int khoahoc_id = item.KHoaHocId.Value;
                        diemNCS.Khoa = lstkhoa.Where(x => x.Id == khoa_id).FirstOrDefault().Name.ToString();
                        diemNCS.Nganh = _unitOfWork.GetRepositoryInstance<NganhDaoTao>().GetFirstOrDefaultByParameter(x => x.Id == nganh_id).TenNganh;
                        diemNCS.KhoaHoc = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetFirstOrDefaultByParameter(x => x.Id == khoahoc_id).MaKhoa;
                        diemNCS.MaHV = item.Ma;
                        diemNCS.TenNCS = item.HoTen;
                        str_Result.Add(diemNCS);
                    }
                }

                return PartialView("_PartialNCS", str_Result);

            }
            catch (Exception ex)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(controllerName, actionName, user == null ? string.Empty : user.Username, ex);
                return PartialView("_PartialNCS", null);
            }
        }
        public JsonResult SaveDiem(int id, float diemdk, float diemthi)
        {
            try
            {
                var diem_ncs = _unitOfWork.GetRepositoryInstance<HocPhan_NCS>().GetFirstOrDefaultByParameter(x => x.Id == id);
                if (diem_ncs == null)
                {
                    return Json("Không tìm thấy thông tin môn học của NCS", JsonRequestBehavior.AllowGet);
                }
                if (diemdk >= 0)
                    diem_ncs.DiemDieuKien = Math.Round(diemdk, 1);
                if (diemthi >= 0)
                    diem_ncs.DiemThi = Math.Round(diemthi, 1);

                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                diem_ncs.UpdatedBy = user.Username;
                diem_ncs.UpdatedAt = DateTime.Now;
                _unitOfWork.GetRepositoryInstance<HocPhan_NCS>().Update(diem_ncs);
                _unitOfWork.SaveChanges();

                if (diem_ncs.DiemDieuKien >= 0 && diem_ncs.DiemThi >= 0)
                {
                    var checkhp = _unitOfWork.GetRepositoryInstance<HocPhan>().GetFirstOrDefaultByParameter(x => x.MaHocPhan == diem_ncs.MaHocPhan);
                    if (checkhp != null)
                    {
                        double? diemtongkethp = (diem_ncs.DiemDieuKien * 3 + diem_ncs.DiemThi * 7) / 10;
                        diemtongkethp = Math.Round(diemtongkethp.Value, 1);
                        Diem diemhp = new Diem();
                        diemhp = _unitOfWork.GetRepositoryInstance<Diem>().GetFirstOrDefaultByParameter(x => x.MaHocVien == diem_ncs.MaNCS);
                        int thuochp = checkhp.LoaiHP.Value;
                        if (diemhp != null)
                        {

                            if (thuochp == 1)
                            {
                                diemhp.DiemHP1 = diemtongkethp;
                            }

                            else if (thuochp == 2)
                            {
                                diemhp.DiemHP2 = diemtongkethp;
                            }
                            else if (thuochp == 3)
                            {
                                diemhp.DiemHP3 = diemtongkethp;
                            }
                            else if (thuochp == 4)
                            {
                                diemhp.DiemHP4 = diemtongkethp;
                            }
                            diemhp.UpdatedAt = DateTime.Now;
                            diemhp.UpdatedBy = user.Username;
                            _unitOfWork.GetRepositoryInstance<Diem>().Update(diemhp);
                            _unitOfWork.SaveChanges();
                        }
                        else
                        {
                            NCS ncs = _unitOfWork.GetRepositoryInstance<NCS>().GetFirstOrDefaultByParameter(x => x.Ma == diem_ncs.MaNCS);
                            if (ncs != null)
                            {
                                diemhp = new Diem();
                                diemhp.MaHocVien = diem_ncs.MaNCS;
                                diemhp.HocPhanId = checkhp.Id;
                                diemhp.CreatedAt = DateTime.Now;
                                diemhp.UpdatedAt = DateTime.Now;
                                diemhp.CreatedBy = user.Username;
                                diemhp.UpdatedBy = user.Username;
                                diemhp.TenHocVien = ncs.HoTen;
                                diemhp.HocVienId = ncs.Id;
                                if (thuochp == 1)
                                {
                                    diemhp.DiemHP1 = diemtongkethp;
                                }
                                else if (thuochp == 2)
                                {
                                    diemhp.DiemHP2 = diemtongkethp;
                                }
                                else if (thuochp == 3)
                                {
                                    diemhp.DiemHP3 = diemtongkethp;
                                }
                                else if (thuochp == 4)
                                {
                                    diemhp.DiemHP4 = diemtongkethp;
                                }
                                _unitOfWork.GetRepositoryInstance<Diem>().Add(diemhp);
                                _unitOfWork.SaveChanges();
                            }

                        }
                    }

                }
                return Json("OK", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                ExceptionLogging.SendErrorToText(controllerName, actionName, user == null ? string.Empty : user.Username, ex);
                return Json("cập nhật giá tri lỗi", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult UpdateHocPhan(int idncs, string mahocphan)
        {
            try
            {
                HocPhan_NCS hocPhanncs = _unitOfWork.GetRepositoryInstance<HocPhan_NCS>().GetFirstOrDefaultByParameter(x => x.Id == idncs);
                if (hocPhanncs == null)
                {
                    TempData["message"] = "Học phần NCS không tồn tại!";
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    HocPhan hocPhan = _unitOfWork.GetRepositoryInstance<HocPhan>().GetFirstOrDefaultByParameter(x => x.MaHocPhan.Equals(mahocphan));
                    if (hocPhanncs == null)
                    {
                        TempData["message"] = "Học phần không tồn tại!";
                        return Json("error", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        hocPhanncs.TenHocPhan = hocPhan.TenHocPhan;
                        hocPhanncs.MaHocPhan = hocPhan.MaHocPhan;
                        hocPhanncs.TinChi = hocPhan.SoTinChi;
                        hocPhanncs.TuChon = hocPhan.TuChon;

                        var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                        hocPhanncs.UpdatedBy = user.Username;
                        hocPhanncs.UpdatedAt = DateTime.Now;
                        _unitOfWork.GetRepositoryInstance<HocPhan_NCS>().Update(hocPhanncs);
                        _unitOfWork.SaveChanges();
                    }
                }
                TempData["message"] = "Cập nhật thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi cập nhật học phần NCS: " + ex.Message;
                return Json("Lỗi thêm mới", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult DuyetHocPhan(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    string[] listId = id.TrimEnd(',').Split(',').ToArray();

                    var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                    foreach (var item in listId)
                    {
                        int id_hpncs = int.Parse(item);
                        HocPhan_NCS hocPhan_NCS = _unitOfWork.GetRepositoryInstance<HocPhan_NCS>().GetFirstOrDefaultByParameter(x => x.Id == id_hpncs);
                        if (hocPhan_NCS != null)
                        {
                            hocPhan_NCS.Status = true;
                            hocPhan_NCS.UpdatedBy = user.Username;
                            hocPhan_NCS.UpdatedAt = DateTime.Now;

                            _unitOfWork.GetRepositoryInstance<HocPhan_NCS>().Update(hocPhan_NCS);
                            _unitOfWork.SaveChanges();
                        }
                    }
                    TempData["message"] = "Cập nhật thành công!";
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["message"] = "Lỗi chọn học phần duyệt!";
                    return Json("ERROR", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi duyệt học phần: " + ex.Message;
                return Json("Lỗi thêm mới", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadExcelFile(HttpPostedFileBase upload)
        {
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
                        var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                        reader.IsFirstRowAsColumnNames = true;
                        DataSet result = reader.AsDataSet();
                        DataTable dt = result.Tables[0];
                        string fileExelName = upload.FileName;
                        if (dt.Rows.Count > 0)
                        {
                            try
                            {
                                HocPhan_NCS hocphanncs = new HocPhan_NCS();
                                HocPhan hp = new HocPhan();
                                foreach (DataRow row in dt.Rows)
                                {
                                    string mahp = row["MAHP"].ToString();
                                    string mancs = row["MANCS"].ToString();
                                    float diemĐieuKien = row["DIEMDK"] != null ? float.Parse(row["DIEMDK"].ToString()) : 0;
                                    float diemThi = row["DIEMTHI"] != null ? float.Parse(row["DIEMTHI"].ToString()) : 0;
                                    hp = _unitOfWork.GetRepositoryInstance<HocPhan>().GetFirstOrDefaultByParameter(o => o.MaHocPhan == mahp);
                                    //MAHP, TENHP, MANCS, TENNCS, DIEMDK, DIEMTHI
                                    hocphanncs = _unitOfWork.GetRepositoryInstance<HocPhan_NCS>().GetFirstOrDefaultByParameter(o => o.MaHocPhan == mahp && o.MaNCS == mancs);
                                    if (hocphanncs != null) // update
                                    {
                                        hocphanncs.MaHocPhan = mahp;
                                        hocphanncs.TinChi = hp.SoTinChi;
                                        hocphanncs.TuChon = hp.TuChon;
                                        hocphanncs.TenHocPhan = row["TENHP"].ToString();
                                        hocphanncs.MaNCS = mancs;
                                        hocphanncs.DiemDieuKien = diemĐieuKien;
                                        hocphanncs.DiemThi = diemThi;
                                        hocphanncs.UpdatedBy = user.Username;
                                        hocphanncs.UpdatedAt = DateTime.Now;
                                        _unitOfWork.GetRepositoryInstance<HocPhan_NCS>().Update(hocphanncs);
                                    }
                                    else
                                    {
                                        hocphanncs = new HocPhan_NCS();
                                        hocphanncs.MaHocPhan = mahp;
                                        hocphanncs.TinChi = hp.SoTinChi;
                                        hocphanncs.TuChon = hp.TuChon;
                                        hocphanncs.TenHocPhan = row["TENHP"].ToString();
                                        hocphanncs.MaNCS = mancs;
                                        hocphanncs.DiemDieuKien = diemĐieuKien;
                                        hocphanncs.DiemThi = diemThi;
                                        hocphanncs.CreatedBy = user.Username;
                                        hocphanncs.CreatedAt = DateTime.Now;
                                        hocphanncs.UpdatedBy = user.Username;
                                        hocphanncs.UpdatedAt = DateTime.Now;
                                        _unitOfWork.GetRepositoryInstance<HocPhan_NCS>().Add(hocphanncs);
                                    }

                                    _unitOfWork.SaveChanges();

                                    if (diemĐieuKien >= 0 && diemThi >= 0)
                                    {
                                        if (hp != null)
                                        {
                                            double? diemtongkethp = (diemĐieuKien * 3 + diemThi * 7) / 10;
                                            Diem diemhp = new Diem();
                                            diemhp = _unitOfWork.GetRepositoryInstance<Diem>().GetFirstOrDefaultByParameter(x => x.MaHocVien == mancs);
                                            int thuochp = hp.LoaiHP.Value;
                                            if (diemhp != null)
                                            {

                                                if (thuochp == 1)
                                                {
                                                    diemhp.DiemHP1 = diemtongkethp;
                                                }

                                                else if (thuochp == 2)
                                                {
                                                    diemhp.DiemHP2 = diemtongkethp;
                                                }
                                                else if (thuochp == 3)
                                                {
                                                    diemhp.DiemHP3 = diemtongkethp;
                                                }
                                                else if (thuochp == 4)
                                                {
                                                    diemhp.DiemHP4 = diemtongkethp;
                                                }
                                                diemhp.UpdatedAt = DateTime.Now;
                                                diemhp.UpdatedBy = user.Username;
                                                _unitOfWork.GetRepositoryInstance<Diem>().Update(diemhp);
                                                _unitOfWork.SaveChanges();
                                            }
                                            else
                                            {
                                                NCS ncs = _unitOfWork.GetRepositoryInstance<NCS>().GetFirstOrDefaultByParameter(x => x.Ma == mancs);
                                                if (ncs != null)
                                                {
                                                    diemhp = new Diem();
                                                    diemhp.MaHocVien = mancs;
                                                    diemhp.HocPhanId = hp.Id;
                                                    diemhp.CreatedAt = DateTime.Now;
                                                    diemhp.UpdatedAt = DateTime.Now;
                                                    diemhp.CreatedBy = user.Username;
                                                    diemhp.UpdatedBy = user.Username;
                                                    diemhp.TenHocVien = ncs.HoTen;
                                                    diemhp.HocVienId = ncs.Id;
                                                    if (thuochp == 1)
                                                    {
                                                        diemhp.DiemHP1 = diemtongkethp;
                                                    }
                                                    else if (thuochp == 2)
                                                    {
                                                        diemhp.DiemHP2 = diemtongkethp;
                                                    }
                                                    else if (thuochp == 3)
                                                    {
                                                        diemhp.DiemHP3 = diemtongkethp;
                                                    }
                                                    else if (thuochp == 4)
                                                    {
                                                        diemhp.DiemHP4 = diemtongkethp;
                                                    }
                                                    _unitOfWork.GetRepositoryInstance<Diem>().Add(diemhp);
                                                    _unitOfWork.SaveChanges();
                                                }
                                            }
                                        }
                                    }
                                }
                                TempData["message"] = "Import danh sách thành công!";
                                return RedirectToAction("Index");
                            }
                            catch (Exception ex)
                            {
                                TempData["error"] = "File dữ liệu không phù hợp với bản mẫu!";
                            }
                        }
                        else
                        {
                            TempData["error"] = "File không có dữ liệu!";
                        }
                        reader.Close();
                        return RedirectToAction("Index");

                    }
                    catch (Exception ex)
                    {
                        TempData["error"] = "Lỗi xảy ra trong quá trình import file excel:" + ex.Message;
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["error"] = "File không có dữ liệu !";
                }
            }
            return RedirectToAction("Index");
        }
    }
}