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

namespace TEMIS.CMS.Areas.Admin.Controllers
{
    public class NghienCuuController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        private Model.TEMIS_systemEntities db = new Model.TEMIS_systemEntities();

        public NghienCuuController()
        {
        }
        // GET: Admin/GiangVien
        public async Task<ActionResult> Index()
        {
            try
            {
                var user = (CoreAPI.Entity.TaiKhoan)Session[PublicConstant.LOGIN_INFO];
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
                    TruongThongTin_NCS tttUpload = _unitOfWork.GetRepositoryInstance<Model.TruongThongTin_NCS>().GetFirstOrDefaultByParameter(o => o.TruongThongTinId == item.Id && o.MaNCS == user.Username);
                    if (tttUpload != null)
                    {
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
                    ttt_NCS.MaNCS = user.Username;

                    listHuongDan.Add(ttt_NCS);

                    i = i + 1;
                }
                List<VanBan> listVanBan = _unitOfWork.GetRepositoryInstance<Model.VanBan>().GetListByParameter(o => o.ChuyenMuc == 4).ToList();
                ViewBag.listVanBan = listVanBan;

                DangKyTuyenSinh dkts = _unitOfWork.GetRepositoryInstance<DangKyTuyenSinh>().GetFirstOrDefaultByParameter(o => o.MaNCS == user.Username);
                ViewBag.DangKyTuyenSinh = dkts;

                //tab 1
                BaoVeTongQuan bvtq = _unitOfWork.GetRepositoryInstance<BaoVeTongQuan>().GetFirstOrDefaultByParameter(o => o.MaNCS == user.Username);
                ViewBag.BaoVeTongQuan = bvtq;
                ThongTinDeTai thongtindetai = _unitOfWork.GetRepositoryInstance<ThongTinDeTai>().GetFirstOrDefaultByParameter(o => o.MaNCS == user.Username);
                ViewBag.ThongTinDeTai = thongtindetai;
                List<NguoiHuongDan> ListNguoiHuongDan = _unitOfWork.GetRepositoryInstance<NguoiHuongDan>().GetListByParameter(o => o.MaNCS == user.Username).ToList();
                ViewBag.NguoiHuongDan = ListNguoiHuongDan;

                //tab 2
                List<DanhSachChuyenDe> ChuyenDe = _unitOfWork.GetRepositoryInstance<DanhSachChuyenDe>().GetListByParameter(o => o.MaNCS == user.Username).ToList();
                ViewBag.DanhSachChuyenDe = ChuyenDe;

                List<TieuBanChamChuyenDe> TieuBanChamChuyenDe = _unitOfWork.GetRepositoryInstance<TieuBanChamChuyenDe>().GetListByParameter(o => o.MaNCS == user.Username).ToList();
                ViewBag.TieuBanChamChuyenDe = TieuBanChamChuyenDe;

                // tab 3
                List<CongTrinhKhoaHoc> congTrinhKhoaHoc = _unitOfWork.GetRepositoryInstance<CongTrinhKhoaHoc>().GetListByParameter(o => o.MaNCS == user.Username).ToList();
                ViewBag.CongTrinhKhoaHoc = congTrinhKhoaHoc;

                // tab 4
                List<BaoVeCapBoMon> congBaoVeCapBoMon = _unitOfWork.GetRepositoryInstance<BaoVeCapBoMon>().GetListByParameter(o => o.MaNCS == user.Username).ToList();
                ViewBag.BaoVeCapBoMon = congBaoVeCapBoMon;

                List<QuyetDinh> ListQuyetDinh = _unitOfWork.GetRepositoryInstance<QuyetDinh>().GetListByParameter(o => o.MaNCS == user.Username).ToList();
                ViewBag.ListQuyetDinh = ListQuyetDinh;

                List<DanhSachHoiDong> ListDanhSachHoiDong = _unitOfWork.GetRepositoryInstance<DanhSachHoiDong>().GetListByParameter(o => o.MaNCS == user.Username).ToList();
                ViewBag.DanhSachHoiDong = ListDanhSachHoiDong;

                List<KetQuaBaoVe> ListKetQuaBaoVe = _unitOfWork.GetRepositoryInstance<KetQuaBaoVe>().GetListByParameter(o => o.MaNCS == user.Username).ToList();
                ViewBag.ListKetQuaBaoVe = ListKetQuaBaoVe;

                // tab 5
                NCS ncs = _unitOfWork.GetRepositoryInstance<NCS>().GetFirstOrDefaultByParameter(o => o.Ma == user.Username);
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
                    var khoa = listKhoa.Where(o => o.Id == item.DepartmentId).SingleOrDefault();
                    if (khoa != null)
                        giangvien.KHoa = khoa.Name;
                    else giangvien.KHoa = "";
                    listGiangVien.Add(giangvien);
                }
                ViewBag.GiangVien = listGiangVien;
                List<PhanBienDocLap> ListPhanBienDocLap = _unitOfWork.GetRepositoryInstance<PhanBienDocLap>().GetListByParameter(o => o.MaNCS == user.Username).ToList();
                ViewBag.PhanBienDocLap = ListPhanBienDocLap;

                // tab 6
                List<BaoVeCapTruong> ListBaoVeCapTruong = _unitOfWork.GetRepositoryInstance<BaoVeCapTruong>().GetListByParameter(o => o.MaNCS == user.Username).ToList();
                ViewBag.ListBaoVeCapTruong = ListBaoVeCapTruong;

                List<GiayToBaoVeLuanAn> ListGiayToBaoVeLuanAn = _unitOfWork.GetRepositoryInstance<GiayToBaoVeLuanAn>().GetListByParameter(o => o.MaNCS == user.Username).ToList();
                ViewBag.ListGiayToBaoVeLuanAn = ListGiayToBaoVeLuanAn;

                HoSoThamDinh hosothamdinh = _unitOfWork.GetRepositoryInstance<HoSoThamDinh>().GetFirstOrDefaultByParameter(o => o.MaNCS == user.Username);
                ViewBag.HoSoThamDinh = hosothamdinh;

                // tab 7

                BaoVe_NCS baove_NCS = _unitOfWork.GetRepositoryInstance<BaoVe_NCS>().GetFirstOrDefaultByParameter(o => o.MaNCS == user.Username);
                ViewBag.BaoVeNCS = baove_NCS;

                return View(listHuongDan);
            }
            catch (Exception ex)
            {

                return View();
            }

        }

        [HttpPost]
        public async Task<JsonResult> UploadHuongDan()
        {
            try
            {
                string fileUrl = "";
                var user = (CoreAPI.Entity.TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                int id = Request.Form["hdfId"] != null ? int.Parse(Request.Form["hdfId"].ToString()) : 0;
                string mancs = Request.Form["hdfMaNCS"] != null ? Request.Form["hdfMaNCS"].ToString() : "";
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
                        fname = Path.Combine(Server.MapPath("~/Upload/HoSoBaoVe/"), fname);
                        file.SaveAs(fname);
                    }
                }
                else
                {
                    TempData["error"] = "Bạn chưa chọn file upload";
                    return Json("Bạn chưa chọn file upload", JsonRequestBehavior.AllowGet);
                }

                TruongThongTin_NCS ttt_ncs = _unitOfWork.GetRepositoryInstance<TruongThongTin_NCS>().GetFirstOrDefaultByParameter(o => o.TruongThongTinId == id && o.MaNCS == mancs);
                if (ttt_ncs != null)
                {
                    ttt_ncs.Status = PublicConstant.STT_NCS_CHODUYET;
                    ttt_ncs.Url = fileUrl;
                    ttt_ncs.UpdatedAt = DateTime.Now;
                    ttt_ncs.UpdatedBy = user.Username;
                    _unitOfWork.GetRepositoryInstance<TruongThongTin_NCS>().Update(ttt_ncs);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    ttt_ncs = new TruongThongTin_NCS();
                    ttt_ncs.MaNCS = mancs.Trim();
                    ttt_ncs.TruongThongTinId = id;
                    ttt_ncs.Status = PublicConstant.STT_NCS_CHODUYET;
                    ttt_ncs.Url = fileUrl;
                    ttt_ncs.CreatedAt = DateTime.Now;
                    ttt_ncs.UpdatedAt = DateTime.Now;
                    ttt_ncs.CreatedBy = user.Username;
                    ttt_ncs.UpdatedBy = user.Username;
                    _unitOfWork.GetRepositoryInstance<TruongThongTin_NCS>().Add(ttt_ncs);
                    _unitOfWork.SaveChanges();
                }

                TempData["message"] = "Upload hồ sơ thành công!";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Lỗi upload hồ sơ: " + ex.Message;
                return Json("Lỗi upload", JsonRequestBehavior.AllowGet);
            }
        }

        // tab 1


        // tab 2
        public JsonResult ThemMoiChuyenDe(string tenchuyende, int sotinchi)
        {
            try
            {
                var user = (CoreAPI.Entity.TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                var chuyende = _unitOfWork.GetRepositoryInstance<DanhSachChuyenDe>().GetFirstOrDefaultByParameter(x => x.TenChuyenDe == tenchuyende && x.MaNCS == user.Username);
                if (chuyende != null)
                {
                    return Json("Chuyên đề đã tồn tại", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    chuyende = new DanhSachChuyenDe();
                    chuyende.TenChuyenDe = tenchuyende;
                    chuyende.SoTinChi = sotinchi;
                    chuyende.CreatedAt = DateTime.Now;
                    chuyende.UpdatedAt = DateTime.Now;
                    chuyende.CreatedBy = user.Username;
                    chuyende.UpdatedBy = user.Username;
                    chuyende.MaNCS = user.Username;
                    _unitOfWork.GetRepositoryInstance<DanhSachChuyenDe>().Add(chuyende);
                    _unitOfWork.SaveChanges();
                    return Json("OK", JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception)
            {
                return Json("Thêm mới lỗi", JsonRequestBehavior.AllowGet);
            }
        }


        //tab 3
        public JsonResult ThemMoiCTKH(string namcongbo, string tencongtrinhkhoahoc, string tentapchi, string noixuatban, string vaitro, int type)
        {
            try
            {
                var user = (CoreAPI.Entity.TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                var ctkh = _unitOfWork.GetRepositoryInstance<CongTrinhKhoaHoc>().GetFirstOrDefaultByParameter(x => x.TenCTKH == tencongtrinhkhoahoc && x.MaNCS == user.Username && x.Type == type);
                if (ctkh != null)
                {
                    return Json("Công trình khoa học đã tồn tại", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ctkh = new CongTrinhKhoaHoc();
                    ctkh.NamCB = namcongbo;
                    ctkh.TenCTKH = tencongtrinhkhoahoc;
                    ctkh.TenTapChi = tentapchi;
                    ctkh.NoiXB = noixuatban;
                    ctkh.VaiTro = vaitro;
                    ctkh.Type = type;
                    ctkh.Status = 0;
                    ctkh.CreatedAt = DateTime.Now;
                    ctkh.UpdatedAt = DateTime.Now;
                    ctkh.CreatedBy = user.Username;
                    ctkh.UpdatedBy = user.Username;
                    ctkh.MaNCS = user.Username;
                    _unitOfWork.GetRepositoryInstance<CongTrinhKhoaHoc>().Add(ctkh);
                    _unitOfWork.SaveChanges();
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json("Thêm mới lỗi", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult EditCTKH(long id, string namcongbo, string tencongtrinhkhoahoc, string tentapchi, string noixuatban, string vaitro, int type)
        {
            try
            {
                var user = (CoreAPI.Entity.TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                var ctkh = _unitOfWork.GetRepositoryInstance<CongTrinhKhoaHoc>().GetFirstOrDefaultByParameter(x => x.Id == id);
                if (ctkh != null)
                {
                    ctkh.NamCB = namcongbo;
                    ctkh.TenCTKH = tencongtrinhkhoahoc;
                    ctkh.TenTapChi = tentapchi;
                    ctkh.NoiXB = noixuatban;
                    ctkh.VaiTro = vaitro;
                    ctkh.CreatedAt = DateTime.Now;
                    ctkh.UpdatedAt = DateTime.Now;
                    ctkh.CreatedBy = user.Username;
                    ctkh.UpdatedBy = user.Username;
                    ctkh.MaNCS = user.Username;
                    ctkh.Type = type;
                    _unitOfWork.GetRepositoryInstance<CongTrinhKhoaHoc>().Update(ctkh);
                    _unitOfWork.SaveChanges();
                }

                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Thêm mới lỗi", JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> XoaCTKH(long id)
        {
            try
            {
                CongTrinhKhoaHoc ctkhoahoc = new CongTrinhKhoaHoc();
                ctkhoahoc = _unitOfWork.GetRepositoryInstance<CongTrinhKhoaHoc>().GetFirstOrDefaultByParameter(o => o.Id == id);
                if (ctkhoahoc != null)
                {
                    _unitOfWork.GetRepositoryInstance<CongTrinhKhoaHoc>().Remove(ctkhoahoc);
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

        // tab 4


        // tab 5
        public JsonResult ThemMoiHoiDongBaoVe(string giaovien, string phone, string trachnhiem, int thutu, string mancs)
        {
            try
            {
                var user = (CoreAPI.Entity.TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                var hoidongphanbiendoclap = _unitOfWork.GetRepositoryInstance<HoiDongPhanBienDocLap>().GetFirstOrDefaultByParameter(x => x.ThuTuHienThi == thutu && x.MaNCS == mancs);
                if (hoidongphanbiendoclap != null)
                {
                    hoidongphanbiendoclap.HoTen = giaovien;
                    hoidongphanbiendoclap.DienThoai = phone;
                    hoidongphanbiendoclap.TrachNhiem = trachnhiem;
                    hoidongphanbiendoclap.ThuTuHienThi = thutu;
                    hoidongphanbiendoclap.UpdatedAt = DateTime.Now;
                    hoidongphanbiendoclap.UpdatedBy = user.Username;
                    hoidongphanbiendoclap.MaNCS = user.Username;
                    _unitOfWork.GetRepositoryInstance<HoiDongPhanBienDocLap>().Update(hoidongphanbiendoclap);
                }
                else
                {
                    hoidongphanbiendoclap = new HoiDongPhanBienDocLap();
                    hoidongphanbiendoclap.HoTen = giaovien;
                    hoidongphanbiendoclap.DienThoai = phone;
                    hoidongphanbiendoclap.TrachNhiem = trachnhiem;
                    hoidongphanbiendoclap.ThuTuHienThi = thutu;
                    hoidongphanbiendoclap.CreatedAt = DateTime.Now;
                    hoidongphanbiendoclap.UpdatedAt = DateTime.Now;
                    hoidongphanbiendoclap.CreatedBy = user.Username;
                    hoidongphanbiendoclap.UpdatedBy = user.Username;
                    hoidongphanbiendoclap.MaNCS = user.Username;
                    _unitOfWork.GetRepositoryInstance<HoiDongPhanBienDocLap>().Add(hoidongphanbiendoclap);
                }

                _unitOfWork.SaveChanges();
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Thêm mới lỗi", JsonRequestBehavior.AllowGet);
            }
        }


        // tab 6



        //tab 7


    }
}