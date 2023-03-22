using Com.Gosol.QLKH.DAL.QuanTriHeThong;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.DanhMuc;
using Com.Gosol.QLKH.Models.QLKH;
using Com.Gosol.QLKH.Ultilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.Gosol.QLKH.DAL.QLKH
{
    public interface IDeTaiDAL
    {
        public List<DeTaiModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow, int LinhVucNghienCuu, int LinhVucKinhTeXaHoi, int CapQuanLy, int TrangThai, int CanBoID, List<int> listCanBoID);
        public DeTaiModel GetByID(int DeTaiID, string serverPath);
        public BaseResultModel Insert(DeTaiModel DeTaiModel);
        public BaseResultModel Update(DeTaiModel DeTaiModel);
        public BaseResultModel Update_TrangThaiDeTai(DeTaiModel DeTaiModel);
        public BaseResultModel Edit_ThongTinChiTiet(ThongTinChiTietDeTaiModel ThongTinChiTiet);
        public BaseResultModel Delete_ThongTinChiTiet(ThongTinChiTietDeTaiModel ThongTinChiTiet);
        public BaseResultModel Edit_KetQuaNghienCuu(KetQuaNghienCuuModel KetQuaNghienCuu);
        public BaseResultModel Delete_KetQuaNghienCuu(KetQuaNghienCuuModel KetQuaNghienCuu);
        public BaseResultModel Delete(DeTaiModel DeTaiModel);
        public List<DuLieuNghienCuuKhoaHocModel> DuLieuNghienCuuKhoaHoc(List<int> StaffId, int? Year);
        public List<DeTaiModel> GetDeTaiByCanBoID(int CanBoID);
        public List<KetQuaNghienCuuModel> GetKetQuaNghienCuuByDeTaiID(int DeTaiID);
        public List<DanhMucCoQuanDonViModel> SoLuongDeTaiTheoCoQuan();
        public List<ThongTinCTNhaKhoaHocModel> DuLieuHoatDongKhoHocKhac(List<int> StaffId, int? Year);
    }
    public class DeTaiDAL : IDeTaiDAL
    {
        /// <summary>
        /// Lấy danh sách đề tài
        /// </summary>
        /// <param name="p"></param>
        /// <param name="TotalRow"></param>
        /// <param name="LinhVucNghienCuu"></param>
        /// <param name="LinhVucKinhTeXaHoi"></param>
        /// <param name="CapQuanLy"></param>
        /// <param name="TrangThai"></param>
        /// <param name="CanBoID"></param>
        /// <returns></returns>
        public List<DeTaiModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow, int LinhVucNghienCuu, int LinhVucKinhTeXaHoi, int CapQuanLy, int TrangThai, int CanBoID, List<int> listCanBoID)
        {
            List<ThongTinChiTietDeTaiModel> Result = new List<ThongTinChiTietDeTaiModel>();
            var pList = new SqlParameter("@ListCanBoID", SqlDbType.Structured);
            pList.TypeName = "dbo.list_ID";
            var tbCanBoID = new DataTable();
            tbCanBoID.Columns.Add("CanBoID", typeof(string));
            listCanBoID.ForEach(x => tbCanBoID.Rows.Add(x));

            SqlParameter[] parameters = new SqlParameter[]
                      {
                        new SqlParameter("@Keyword",SqlDbType.NVarChar),
                        new SqlParameter("@OrderByName",SqlDbType.NVarChar),
                        new SqlParameter("@OrderByOption",SqlDbType.NVarChar),
                        new SqlParameter("@pLimit",SqlDbType.Int),
                        new SqlParameter("@pOffset",SqlDbType.Int),
                        new SqlParameter("@TotalRow",SqlDbType.Int),
                        new SqlParameter("@LinhVucNghienCuu",SqlDbType.Int),
                        new SqlParameter("@CapQuanLy",SqlDbType.Int),
                        new SqlParameter("@CanBoID",SqlDbType.Int),
                        pList,
                      };
            parameters[0].Value = p.Keyword == null ? "" : p.Keyword.Trim();
            parameters[1].Value = p.OrderByName;
            parameters[2].Value = p.OrderByOption;
            parameters[3].Value = p.Limit;
            parameters[4].Value = p.Offset;
            parameters[5].Direction = ParameterDirection.Output;
            parameters[5].Size = 8;
            parameters[6].Value = LinhVucNghienCuu;
            parameters[7].Value = CapQuanLy;
            parameters[8].Value = CanBoID;
            parameters[9].Value = tbCanBoID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_DeTai_GetPagingBySearch", parameters))
                {
                    while (dr.Read())
                    {
                        ThongTinChiTietDeTaiModel deTai = new ThongTinChiTietDeTaiModel();
                        deTai.DeTaiID = Utils.ConvertToInt32(dr["DeTaiID"], 0);
                        deTai.TenDeTai = Utils.ConvertToString(dr["TenDeTai"], string.Empty);
                        deTai.MaDeTai = Utils.ConvertToString(dr["MaDeTai"], string.Empty);
                        deTai.LoaiHinhNghienCuu = Utils.ConvertToInt32(dr["LoaiHinhNghienCuu"], 0);
                        deTai.TenLoaiHinhNghienCuu = Utils.ConvertToString(dr["TenLoaiHinhNghienCuu"], string.Empty);
                        deTai.LinhVucNghienCuu = Utils.ConvertToInt32(dr["LinhVucNghienCuu"], 0);
                        deTai.TenLinhVucNghienCuu = Utils.ConvertToString(dr["TenLinhVucNghienCuu"], string.Empty);
                        deTai.LinhVucKinhTeXaHoi = Utils.ConvertToInt32(dr["LinhVucKinhTeXaHoi"], 0);
                        deTai.TenLinhVucKinhTeXaHoi = Utils.ConvertToString(dr["TenLinhVucKinhTeXaHoi"], string.Empty);
                        deTai.CapQuanLy = Utils.ConvertToInt32(dr["NhiemVu"], 0);
                        deTai.TenCapQuanLy = Utils.ConvertToString(dr["TenCapQuanLy"], string.Empty);
                        deTai.NhiemVu = Utils.ConvertToInt32(dr["NhiemVu"], 0);
                        deTai.CoQuanChuQuan = Utils.ConvertToString(dr["CoQuanChuQuan"], string.Empty);
                        deTai.DonViQuanLyKhoaHoc = Utils.ConvertToString(dr["DonViQuanLyKhoaHoc"], string.Empty);
                        deTai.ChuNhiemDeTaiID = Utils.ConvertToInt32(dr["ChuNhiemDeTaiID"], 0);
                        deTai.TenChuNhiemDeTai = Utils.ConvertToString(dr["TenChuNhiemDeTai"], string.Empty);
                        deTai.VaiTroChuNhiemID = Utils.ConvertToInt32(dr["VaiTroChuNhiemID"], 0);
                        deTai.NamBatDau = Utils.ConvertToString(dr["NamBatDau"], string.Empty);
                        deTai.NamKetThuc = Utils.ConvertToString(dr["NamKetThuc"], string.Empty);
                        deTai.KinhPhiDHSP = Utils.ConvertToDecimal(dr["KinhPhiDHSP"], 0);
                        deTai.NguonKhac = Utils.ConvertToDecimal(dr["NguonKhac"], 0);
                        deTai.MucTieu = Utils.ConvertToString(dr["MucTieu"], string.Empty);
                        deTai.ThanhVienNghienCuuStr = Utils.ConvertToString(dr["ThanhVienNghienCuuStr"], string.Empty);
                        deTai.CacNoiDungChinhStr = Utils.ConvertToString(dr["CacNoiDungChinhStr"], string.Empty);
                        //deTai.CacNoiDungChinh = Utils.ConvertToString(dr["CacNoiDungChinh"], string.Empty);
                        deTai.SanPhamDangKy = Utils.ConvertToString(dr["SanPhamDangKy"], string.Empty);
                        deTai.KhaNangUngDung = Utils.ConvertToString(dr["KhaNangUngDung"], string.Empty);
                        deTai.TrangThai = Utils.ConvertToInt32(dr["TrangThai"], 0);
                        //file
                        deTai.NghiepVuID = Utils.ConvertToInt32(dr["NghiepVuID"], 0);
                        deTai.FileDinhKemID = Utils.ConvertToInt32(dr["FileDinhKemID"], 0);
                        deTai.TenFileGoc = Utils.ConvertToString(dr["TenFileGoc"], string.Empty);
                        deTai.TenFileHeThong = Utils.ConvertToString(dr["TenFileHeThong"], string.Empty);
                        deTai.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        deTai.LoaiFile = Utils.ConvertToInt32(dr["LoaiFile"], 0);
                        deTai.NguoiTaoFileID = Utils.ConvertToInt32(dr["NguoiTaoID"], 0);
                        //deTai.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        deTai.NgayTao = Utils.ConvertToDateTime(dr["NgayTao"], DateTime.Now);
                        deTai.NoiDungFile = Utils.ConvertToString(dr["NoiDungFile"], string.Empty);
                        deTai.TenNguoiTao = Utils.ConvertToString(dr["TenNguoiTao"], string.Empty);

                        Result.Add(deTai);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parameters[5].Value, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            List<DeTaiModel> Data = new List<DeTaiModel>();
            if (Result.Count > 0)
            {
                Data = Result.GroupBy(p => p.DeTaiID)
                    .Select(g => new DeTaiModel
                    {
                        DeTaiID = g.Key,
                        TenDeTai = g.FirstOrDefault().TenDeTai,
                        MaDeTai = g.FirstOrDefault().MaDeTai,
                        LoaiHinhNghienCuu = g.FirstOrDefault().LoaiHinhNghienCuu,
                        TenLoaiHinhNghienCuu = g.FirstOrDefault().TenLoaiHinhNghienCuu,
                        LinhVucNghienCuu = g.FirstOrDefault().LinhVucNghienCuu,
                        TenLinhVucNghienCuu = g.FirstOrDefault().TenLinhVucNghienCuu,
                        LinhVucKinhTeXaHoi = g.FirstOrDefault().LinhVucKinhTeXaHoi,
                        TenLinhVucKinhTeXaHoi = g.FirstOrDefault().TenLinhVucKinhTeXaHoi,
                        CapQuanLy = g.FirstOrDefault().CapQuanLy,
                        TenCapQuanLy = g.FirstOrDefault().TenCapQuanLy,
                        NhiemVu = g.FirstOrDefault().NhiemVu,
                        CoQuanChuQuan = g.FirstOrDefault().CoQuanChuQuan,
                        DonViQuanLyKhoaHoc = g.FirstOrDefault().DonViQuanLyKhoaHoc,
                        ChuNhiemDeTaiID = g.FirstOrDefault().ChuNhiemDeTaiID,
                        TenChuNhiemDeTai = g.FirstOrDefault().TenChuNhiemDeTai,
                        VaiTroChuNhiemID = g.FirstOrDefault().VaiTroChuNhiemID,
                        NamBatDau = g.FirstOrDefault().NamBatDau,
                        NamKetThuc = g.FirstOrDefault().NamKetThuc,
                        KinhPhiDHSP = g.FirstOrDefault().KinhPhiDHSP,
                        NguonKhac = g.FirstOrDefault().NguonKhac,
                        MucTieu = g.FirstOrDefault().MucTieu,
                        ThanhVienNghienCuuStr = g.FirstOrDefault().ThanhVienNghienCuuStr,
                        CacNoiDungChinhStr = g.FirstOrDefault().CacNoiDungChinhStr,
                        SanPhamDangKy = g.FirstOrDefault().SanPhamDangKy,
                        KhaNangUngDung = g.FirstOrDefault().KhaNangUngDung,
                        TrangThai = g.FirstOrDefault().TrangThai,
                        FileDinhKem = Result.Where(x => x.DeTaiID == g.Key && x.FileDinhKemID > 0)
                                        .Select(y => new FileDinhKemModel
                                        {
                                            NghiepVuID = g.Key,
                                            FileDinhKemID = y.FileDinhKemID,
                                            TenFileHeThong = y.TenFileHeThong,
                                            TenFileGoc = y.TenFileGoc,
                                            LoaiFile = y.LoaiFile,
                                            FileUrl = y.FileUrl,
                                            NoiDung = y.NoiDungFile,
                                            NgayTao = y.NgayTao,
                                            NguoiTaoID = y.NguoiTaoID,
                                            TenNguoiTao = y.TenNguoiTao,
                                        }
                                        ).ToList()
                    }
                    ).OrderByDescending(x => x.DeTaiID).ToList();
            }

            return Data;
        }

        /// <summary>
        /// Lấy chi tiết đề tài theo DeTaiID
        /// </summary>
        /// <param name="DeTaiID"></param>
        /// <returns></returns>
        public DeTaiModel GetByID(int DeTaiID, string serverPath)
        {
            List<DeTaiModel> Result = new List<DeTaiModel>();
            List<ThongTinChiTietDeTaiModel> deTais = new List<ThongTinChiTietDeTaiModel>();

            SqlParameter[] parameters = new SqlParameter[]
            {
                  new SqlParameter("@DeTaiID",SqlDbType.Int)
            };
            parameters[0].Value = DeTaiID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_DeTai_GetByID", parameters))
                {
                    while (dr.Read())
                    {
                        ThongTinChiTietDeTaiModel deTai = new ThongTinChiTietDeTaiModel();
                        deTai.DeTaiID = Utils.ConvertToInt32(dr["DeTaiID"], 0);
                        deTai.TenDeTai = Utils.ConvertToString(dr["TenDeTai"], string.Empty);
                        deTai.MaDeTai = Utils.ConvertToString(dr["MaDeTai"], string.Empty);
                        deTai.LoaiHinhNghienCuu = Utils.ConvertToInt32(dr["LoaiHinhNghienCuu"], 0);
                        deTai.LinhVucNghienCuu = Utils.ConvertToInt32(dr["LinhVucNghienCuu"], 0);
                        deTai.LinhVucKinhTeXaHoi = Utils.ConvertToInt32(dr["LinhVucKinhTeXaHoi"], 0);
                        deTai.CapQuanLy = Utils.ConvertToInt32(dr["NhiemVuDeTai"], 0);
                        deTai.NhiemVuDeTai = Utils.ConvertToInt32(dr["NhiemVuDeTai"], 0);
                        deTai.CoQuanChuQuan = Utils.ConvertToString(dr["CoQuanChuQuan"], string.Empty);
                        deTai.DonViQuanLyKhoaHoc = Utils.ConvertToString(dr["DonViQuanLyKhoaHoc"], string.Empty);
                        deTai.TenChuNhiemDeTai = Utils.ConvertToString(dr["TenChuNhiemDeTai"], string.Empty);
                        deTai.ChuNhiemDeTaiID = Utils.ConvertToInt32(dr["ChuNhiemDeTaiID"], 0);
                        deTai.CoQuanChuNhiemID = Utils.ConvertToInt32(dr["CoQuanChuNhiemID"], 0);
                        deTai.VaiTroChuNhiemID = Utils.ConvertToInt32(dr["VaiTroChuNhiemID"], 0);
                        deTai.NamBatDau = Utils.ConvertToString(dr["NamBatDau"], string.Empty);
                        deTai.NamKetThuc = Utils.ConvertToString(dr["NamKetThuc"], string.Empty);
                        deTai.SoLanGiaHanThucHien = Utils.ConvertToInt32(dr["SoLanGiaHanThucHien"], 0);
                        deTai.KinhPhiDHSP = Utils.ConvertToDecimal(dr["KinhPhiDHSP"], 0);
                        deTai.NguonKhac = Utils.ConvertToDecimal(dr["NguonKhac"], 0);
                        deTai.MucTieu = Utils.ConvertToString(dr["MucTieu"], string.Empty);
                        //deTai.CacNoiDungChinh = Utils.ConvertToString(dr["CacNoiDungChinh"], string.Empty);
                        deTai.TenNoiDung = Utils.ConvertToString(dr["TenNoiDung"], string.Empty);
                        deTai.MoTa = Utils.ConvertToString(dr["MoTa"], string.Empty);
                        deTai.TrangThaiTienDo = Utils.ConvertToInt32(dr["TrangThaiTienDo"], 0);
                        deTai.GhiChu = Utils.ConvertToString(dr["GhiChu"], string.Empty);
                        deTai.SanPhamDangKy = Utils.ConvertToString(dr["SanPhamDangKy"], string.Empty);
                        deTai.KhaNangUngDung = Utils.ConvertToString(dr["KhaNangUngDung"], string.Empty);
                        deTai.DonViPhoiHop = Utils.ConvertToString(dr["DonViPhoiHop"], string.Empty);
                        deTai.TrangThai = Utils.ConvertToInt32(dr["TrangThai"], 0);
                        deTai.NguoiTaoID = Utils.ConvertToInt32(dr["NguoiTaoID"], 0);
                        //thành viên nghiên cúu
                        deTai.ThanhVienNghienCuuID = Utils.ConvertToInt32(dr["ThanhVienNghienCuuID"], 0);
                        deTai.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        deTai.VaiTro = Utils.ConvertToInt32(dr["VaiTro"], 0);
                        deTai.LaCanBoTrongTruong = Utils.ConvertToInt32(dr["LaCanBoTrongTruong"], 0);
                        //thông tin chi tiết 
                        deTai.NguoiTaoChiTietID = Utils.ConvertToInt32(dr["NguoiTaoChiTietID"], 0);
                        deTai.ChiTietDeTaiID = Utils.ConvertToInt32(dr["ChiTietDeTaiID"], 0);
                        deTai.LoaiThongTin = Utils.ConvertToInt32(dr["LoaiThongTin"], 0);
                        deTai.NgayThucHien = Utils.ConvertToNullableDateTime(dr["NgayThucHien"], null);
                        deTai.NoiDungThucHien = Utils.ConvertToString(dr["NoiDungThucHien"], string.Empty);
                        deTai.KetQuaDatDuoc = Utils.ConvertToString(dr["KetQuaDatDuoc"], string.Empty);
                        deTai.TongKinhPhiDuocDuyet = Utils.ConvertToDecimal(dr["TongKinhPhiDuocDuyet"], 0);
                        deTai.TienDoCap = Utils.ConvertToDecimal(dr["TienDoCap"], 0);
                        deTai.TienDoQuyetToan = Utils.ConvertToDecimal(dr["TienDoQuyetToan"], 0);
                        deTai.NoiDungDaLam = Utils.ConvertToString(dr["NoiDungDaLam"], string.Empty);
                        deTai.SanPham = Utils.ConvertToString(dr["SanPham"], string.Empty);
                        deTai.NoiDungChuyenGiao = Utils.ConvertToString(dr["NoiDungChuyenGiao"], string.Empty);
                        deTai.ThoiGianThucHien = Utils.ConvertToString(dr["ThoiGianThucHien"], string.Empty);
                        deTai.NoiDungDanhGia = Utils.ConvertToString(dr["NoiDungDanhGia"], string.Empty);
                        deTai.KetQuaThucHien = Utils.ConvertToString(dr["KetQuaThucHien"], string.Empty);
                        deTai.XepLoai = Utils.ConvertToInt32(dr["XepLoai"], 0);
                        deTai.XepLoaiKhac = Utils.ConvertToString(dr["XepLoaiKhac"], string.Empty);
                        deTai.LoaiNghiemThu = Utils.ConvertToInt32(dr["LoaiNghiemThu"], 0);
                        deTai.NgayNghiemThu = Utils.ConvertToNullableDateTime(dr["NgayNghiemThu"], null);
                        deTai.QuyetDinh = Utils.ConvertToString(dr["QuyetDinh"], string.Empty);
                        //kết quả nghiên cứu
                        deTai.NguoiTaoKQID = Utils.ConvertToInt32(dr["NguoiTaoKQID"], 0);
                        deTai.KetQuaNghienCuuID = Utils.ConvertToInt32(dr["KetQuaNghienCuuID"], 0);
                        deTai.LoaiKetQua = Utils.ConvertToInt32(dr["LoaiKetQua"], 0);
                        deTai.NhiemVu = Utils.ConvertToInt32(dr["NhiemVu"], 0);
                        deTai.ThoiGian = Utils.ConvertToString(dr["ThoiGian"], string.Empty);
                        deTai.TacGia = Utils.ConvertToString(dr["TacGia"], string.Empty);
                        deTai.TieuDe = Utils.ConvertToString(dr["TieuDe"], string.Empty);
                        deTai.TenTapChiSachHoiThao = Utils.ConvertToString(dr["TenTapChiSachHoiThao"], string.Empty);
                        deTai.So = Utils.ConvertToString(dr["So"], string.Empty);
                        deTai.Trang = Utils.ConvertToString(dr["Trang"], string.Empty);
                        deTai.NhaXuatBan = Utils.ConvertToString(dr["NhaXuatBan"], string.Empty);
                        //
                        deTai.TieuDe = Utils.ConvertToString(dr["TieuDe"], string.Empty);
                        deTai.TacGia = Utils.ConvertToString(dr["TacGia"], string.Empty);
                        deTai.TenTapChiSachHoiThao = Utils.ConvertToString(dr["TenTapChiSachHoiThao"], string.Empty);
                        deTai.So = Utils.ConvertToString(dr["So"], string.Empty);
                        deTai.Trang = Utils.ConvertToString(dr["Trang"], string.Empty);
                        deTai.NhaXuatBan = Utils.ConvertToString(dr["NhaXuatBan"], string.Empty);
                        deTai.KhoangThoiGian = Utils.ConvertToString(dr["KhoangThoiGian"], string.Empty);
                        deTai.LoaiBaiBao = Utils.ConvertToInt32(dr["LoaiBaiBao"], 0);
                        deTai.ISSN = Utils.ConvertToString(dr["ISSN"], string.Empty);
                        deTai.NhiemVuBaiBao = Utils.ConvertToInt32(dr["NhiemVuBaiBao"], 0);
                        deTai.LoaiNhiemVu = Utils.ConvertToInt32(dr["LoaiNhiemVu"], 0);
                        deTai.TenHoiThao = Utils.ConvertToString(dr["TenHoiThao"], string.Empty);

                        deTai.Tap = Utils.ConvertToString(dr["Tap"], string.Empty);
                        deTai.NamDangTai = Utils.ConvertToInt32(dr["NamDangTai"], 0);
                        deTai.LinkBaiBao = Utils.ConvertToString(dr["LinkBaiBao"], string.Empty);
                        deTai.LinhVucNganhKhoaHoc = Utils.ConvertToInt32(dr["LinhVucNganhKhoaHoc"], 0);
                        deTai.HeSoAnhHuong = Utils.ConvertToString(dr["HeSoAnhHuong"], string.Empty);
                        deTai.ChiSo = Utils.ConvertToInt32(dr["ChiSo"], 0);
                        deTai.RankSCIMAG = Utils.ConvertToInt32(dr["RankSCIMAG"], 0);
                        deTai.DiemTapChi = Utils.ConvertToDecimal(dr["DiemTapChi"], 0);
                        deTai.CapHoiThao = Utils.ConvertToInt32(dr["CapHoiThao"], 0);
                        deTai.NgayHoiThao = Utils.ConvertToNullableDateTime(dr["NgayHoiThao"], null);
                        deTai.DiaDiemToChuc = Utils.ConvertToString(dr["DiaDiemToChuc"], string.Empty);
                        deTai.LoaiDaoTao = Utils.ConvertToInt32(dr["LoaiDaoTao"], 0);
                        deTai.TenHocVien = Utils.ConvertToString(dr["TenHocVien"], string.Empty);
                        deTai.TenLuanVan = Utils.ConvertToString(dr["TenLuanVan"], string.Empty);
                        deTai.NguoiHuongDan = Utils.ConvertToString(dr["NguoiHuongDan"], string.Empty);
                        deTai.NamBaoVe = Utils.ConvertToInt32(dr["NamBaoVe"], 0);
                        deTai.ChuBienID = Utils.ConvertToInt32(dr["ChuBienID"], 0);
                        deTai.CoQuanChuBienID = Utils.ConvertToInt32(dr["CoQuanChuBienID"], 0);
                        deTai.NamXuatBan = Utils.ConvertToInt32NullAble(dr["NamXuatBan"], null);

                        deTai.CoSoDaoTao = Utils.ConvertToString(dr["CoSoDaoTao"], string.Empty);
                        //file
                        deTai.NghiepVuID = Utils.ConvertToInt32(dr["NghiepVuID"], 0);
                        deTai.FileDinhKemID = Utils.ConvertToInt32(dr["FileDinhKemID"], 0);
                        deTai.TenFileGoc = Utils.ConvertToString(dr["TenFileGoc"], string.Empty);
                        deTai.TenFileHeThong = Utils.ConvertToString(dr["TenFileHeThong"], string.Empty);
                        deTai.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        deTai.LoaiFile = Utils.ConvertToInt32(dr["LoaiFile"], 0);
                        deTai.NguoiTaoFileID = Utils.ConvertToInt32(dr["NguoiTaoFileID"], 0);
                        deTai.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        deTai.NgayTao = Utils.ConvertToDateTime(dr["NgayTao"], DateTime.Now);
                        deTai.NoiDungFile = Utils.ConvertToString(dr["NoiDungFile"], string.Empty);
                        deTai.TenNguoiTao = Utils.ConvertToString(dr["TenNguoiTaoFile"], string.Empty);
                        //tac gia
                        deTai.TacGiaID = Utils.ConvertToInt32(dr["TacGiaID"], 0);
                        deTai.CanBoTGID = Utils.ConvertToInt32(dr["CanBoTGID"], 0);
                        deTai.CoQuanTGID = Utils.ConvertToInt32(dr["CoQuanTGID"], 0);
                        deTai.TenTacGia = Utils.ConvertToString(dr["TenTacGia"], string.Empty);

                        deTais.Add(deTai);
                    }
                    dr.Close();
                }
                if (deTais.Count > 0)
                {
                    Result = (from m in deTais
                              group m by m.DeTaiID into ctt
                              from item in ctt
                              select new DeTaiModel()
                              {
                                  DeTaiID = item.DeTaiID,
                                  TenDeTai = item.TenDeTai,
                                  MaDeTai = item.MaDeTai,
                                  LoaiHinhNghienCuu = item.LoaiHinhNghienCuu,
                                  LinhVucNghienCuu = item.LinhVucNghienCuu,
                                  LinhVucKinhTeXaHoi = item.LinhVucKinhTeXaHoi,
                                  CapQuanLy = item.CapQuanLy,
                                  NhiemVu = item.NhiemVuDeTai,
                                  CoQuanChuQuan = item.CoQuanChuQuan,
                                  DonViQuanLyKhoaHoc = item.DonViQuanLyKhoaHoc,
                                  ChuNhiemDeTaiID = item.ChuNhiemDeTaiID,
                                  CoQuanChuNhiemID = item.CoQuanChuNhiemID,
                                  VaiTroChuNhiemID = item.VaiTroChuNhiemID,
                                  NamBatDau = item.NamBatDau,
                                  NamKetThuc = item.NamKetThuc,
                                  SoLanGiaHanThucHien = item.SoLanGiaHanThucHien,
                                  KinhPhiDHSP = item.KinhPhiDHSP,
                                  NguonKhac = item.NguonKhac,
                                  MucTieu = item.MucTieu,
                                  //CacNoiDungChinh = item.CacNoiDungChinh,
                                  SanPhamDangKy = item.SanPhamDangKy,
                                  KhaNangUngDung = item.KhaNangUngDung,
                                  DonViPhoiHop = item.DonViPhoiHop,
                                  TrangThai = item.TrangThai,
                                  NguoiTaoID = item.NguoiTaoID,
                                  FileDinhKem = (from i in deTais.Where(x => x.FileDinhKemID > 0).ToList().GroupBy(x => x.FileDinhKemID)
                                                 select new FileDinhKemModel()
                                                 {
                                                     NghiepVuID = i.FirstOrDefault().DeTaiID,
                                                     FileDinhKemID = i.FirstOrDefault().FileDinhKemID,
                                                     TenFileHeThong = i.FirstOrDefault().TenFileHeThong,
                                                     TenFileGoc = i.FirstOrDefault().TenFileGoc,
                                                     LoaiFile = i.FirstOrDefault().LoaiFile,
                                                     FileUrl = i.FirstOrDefault().FileUrl,
                                                     NoiDung = i.FirstOrDefault().NoiDungFile,
                                                     NgayTao = i.FirstOrDefault().NgayTao,
                                                     NguoiTaoID = i.FirstOrDefault().NguoiTaoFileID,
                                                     CoQuanID = i.FirstOrDefault().CoQuanID,
                                                     TenNguoiTao = i.FirstOrDefault().TenNguoiTao,
                                                 }
                                                         ).ToList(),
                                  ThanhVienNghienCuu = (from i in deTais.Where(x => x.ThanhVienNghienCuuID > 0).ToList().GroupBy(x => x.ThanhVienNghienCuuID)
                                                        select new ThanhVienNghienCuuModel()
                                                        {
                                                            ThanhVienNghienCuuID = i.FirstOrDefault().ThanhVienNghienCuuID,
                                                            CanBoID = i.FirstOrDefault().CanBoID,
                                                            VaiTro = i.FirstOrDefault().VaiTro,
                                                            LaCanBoTrongTruong = i.FirstOrDefault().LaCanBoTrongTruong,
                                                        }
                                                         ).ToList(),
                                  CacNoiDungChinh = (from i in deTais.Where(x => x.ChiTietDeTaiID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTin.TienDoThucHien).ToList().GroupBy(x => x.ChiTietDeTaiID)
                                                     select new NoiDungChinhModel()
                                                     {
                                                         ChiTietDeTaiID = i.FirstOrDefault().ChiTietDeTaiID ?? 0,
                                                         DeTaiID = i.FirstOrDefault().DeTaiID,
                                                         TenNoiDung = i.FirstOrDefault().TenNoiDung,
                                                         MoTa = i.FirstOrDefault().MoTa,
                                                         NguoiTaoID = i.FirstOrDefault().NguoiTaoChiTietID,
                                                     }
                                                         ).OrderBy(x => x.ChiTietDeTaiID).ToList(),
                                  TienDoThucHien = (from i in deTais.Where(x => x.ChiTietDeTaiID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTin.TienDoThucHien).ToList().GroupBy(x => x.ChiTietDeTaiID)
                                                    select new TienDoThucHienModel()
                                                    {
                                                        ChiTietDeTaiID = i.FirstOrDefault().ChiTietDeTaiID ?? 0,
                                                        DeTaiID = i.FirstOrDefault().DeTaiID,
                                                        NgayThucHien = i.FirstOrDefault().NgayThucHien,
                                                        NoiDungThucHien = i.FirstOrDefault().NoiDungThucHien,
                                                        KetQuaDatDuoc = i.FirstOrDefault().KetQuaDatDuoc,
                                                        TenNoiDung = i.FirstOrDefault().TenNoiDung,
                                                        TrangThaiTienDo = i.FirstOrDefault().TrangThaiTienDo,
                                                        GhiChu = i.FirstOrDefault().GhiChu,
                                                        NguoiTaoID = i.FirstOrDefault().NguoiTaoChiTietID,
                                                    }
                                                         ).OrderBy(x => x.NgayThucHien).ToList(),
                                  KinhPhi = (from i in deTais.Where(x => x.ChiTietDeTaiID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTin.KinhPhi).ToList().GroupBy(x => x.ChiTietDeTaiID)
                                             select new KinhPhiModel()
                                             {
                                                 ChiTietDeTaiID = i.FirstOrDefault().ChiTietDeTaiID ?? 0,
                                                 DeTaiID = i.FirstOrDefault().DeTaiID,
                                                 TongKinhPhiDuocDuyet = i.FirstOrDefault().TongKinhPhiDuocDuyet,
                                                 TienDoCap = i.FirstOrDefault().TienDoCap,
                                                 TienDoQuyetToan = i.FirstOrDefault().TienDoQuyetToan,
                                                 NguoiTaoID = i.FirstOrDefault().NguoiTaoChiTietID,
                                             }
                                                         ).OrderBy(x => x.ChiTietDeTaiID).ToList(),
                                  SanPhamDeTai = (from i in deTais.Where(x => x.ChiTietDeTaiID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTin.SanPhamDeTai).ToList().GroupBy(x => x.ChiTietDeTaiID)
                                                  select new SanPhamDeTaiModel()
                                                  {
                                                      ChiTietDeTaiID = i.FirstOrDefault().ChiTietDeTaiID ?? 0,
                                                      DeTaiID = i.FirstOrDefault().DeTaiID,
                                                      NoiDungDaLam = i.FirstOrDefault().NoiDungDaLam,
                                                      SanPham = i.FirstOrDefault().SanPham,
                                                      NguoiTaoID = i.FirstOrDefault().NguoiTaoChiTietID,
                                                      FileDinhKem = new List<FileDinhKemModel>(),
                                                  }
                                                         ).OrderBy(x => x.ChiTietDeTaiID).ToList(),
                                  KetQuaChuyenGiao = (from i in deTais.Where(x => x.ChiTietDeTaiID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTin.KetQuaChuyenGiao).ToList().GroupBy(x => x.ChiTietDeTaiID)
                                                      select new KetQuaChuyenGiaoModel()
                                                      {
                                                          ChiTietDeTaiID = i.FirstOrDefault().ChiTietDeTaiID ?? 0,
                                                          DeTaiID = i.FirstOrDefault().DeTaiID,
                                                          NoiDungChuyenGiao = i.FirstOrDefault().NoiDungChuyenGiao,
                                                          NguoiTaoID = i.FirstOrDefault().NguoiTaoChiTietID,
                                                          FileDinhKem = new List<FileDinhKemModel>(),
                                                      }
                                                         ).OrderBy(x => x.ChiTietDeTaiID).ToList(),
                                  DanhGiaGiaiDoan = (from i in deTais.Where(x => x.ChiTietDeTaiID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTin.DanhGiaGiaiDoan).ToList().GroupBy(x => x.ChiTietDeTaiID)
                                                     select new DanhGiaGiaiDoanModel()
                                                     {
                                                         ChiTietDeTaiID = i.FirstOrDefault().ChiTietDeTaiID ?? 0,
                                                         DeTaiID = i.FirstOrDefault().DeTaiID,
                                                         ThoiGianThucHien = i.FirstOrDefault().ThoiGianThucHien,
                                                         NoiDungDanhGia = i.FirstOrDefault().NoiDungDanhGia,
                                                         KetQuaThucHien = i.FirstOrDefault().KetQuaThucHien,
                                                         NguoiTaoID = i.FirstOrDefault().NguoiTaoChiTietID,
                                                         FileDinhKem = new List<FileDinhKemModel>(),
                                                     }
                                                         ).OrderBy(x => x.ThoiGianThucHien).ToList(),
                                  KetQuaDanhGia = (from i in deTais.Where(x => x.ChiTietDeTaiID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTin.KetQuaDanhGia).ToList().GroupBy(x => x.ChiTietDeTaiID)
                                                   select new KetQuaDanhGiaModel()
                                                   {
                                                       ChiTietDeTaiID = i.FirstOrDefault().ChiTietDeTaiID ?? 0,
                                                       DeTaiID = i.FirstOrDefault().DeTaiID,
                                                       ThoiGianThucHien = i.FirstOrDefault().ThoiGianThucHien,
                                                       NoiDungDanhGia = i.FirstOrDefault().NoiDungDanhGia,
                                                       LoaiKetQua = i.FirstOrDefault().LoaiKetQua,
                                                       XepLoai = i.FirstOrDefault().XepLoai,
                                                       XepLoaiKhac = i.FirstOrDefault().XepLoaiKhac,
                                                       LoaiNghiemThu = i.FirstOrDefault().LoaiNghiemThu,
                                                       NgayNghiemThu = i.FirstOrDefault().NgayNghiemThu,
                                                       QuyetDinh = i.FirstOrDefault().QuyetDinh,
                                                       NguoiTaoID = i.FirstOrDefault().NguoiTaoChiTietID,
                                                       FileDinhKem = new List<FileDinhKemModel>(),
                                                   }
                                                         ).OrderBy(x => x.ThoiGianThucHien).ToList(),
                                  //KetQuaNghienCuu = (from i in deTais.Where(x => x.KetQuaNghienCuuID > 0).ToList().GroupBy(x => x.KetQuaNghienCuuID)
                                  //                   select new KetQuaNghienCuuModel()
                                  //                   {
                                  //                       KetQuaNghienCuuID = i.FirstOrDefault().KetQuaNghienCuuID,
                                  //                       LoaiKetQua = i.FirstOrDefault().LoaiKetQua,
                                  //                       DeTaiID = i.FirstOrDefault().DeTaiID,
                                  //                       NhiemVu = i.FirstOrDefault().NhiemVu,
                                  //                       ThoiGian = i.FirstOrDefault().ThoiGian,
                                  //                       TacGia = i.FirstOrDefault().TacGia,
                                  //                       TieuDe = i.FirstOrDefault().TieuDe,
                                  //                       TenTapChiSachHoiThao = i.FirstOrDefault().TenTapChiSachHoiThao,
                                  //                       So = i.FirstOrDefault().So,
                                  //                       Trang = i.FirstOrDefault().Trang,
                                  //                       NhaXuatBan = i.FirstOrDefault().NhaXuatBan,
                                  //                       NguoiTaoID = i.FirstOrDefault().NguoiTaoKQID,
                                  //                       FileDinhKem = new List<FileDinhKemModel>(),
                                  //                   }
                                  //                       ).OrderBy(x => x.KetQuaNghienCuuID).ToList(),
                                  BaiBaoTapChi = (from i in deTais.Where(x => x.ChiTietDeTaiID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTin.BaiBaoTapChi).ToList().GroupBy(x => x.ChiTietDeTaiID)
                                                  select new BaiBaoTapChiModel()
                                                  {
                                                      ChiTietDeTaiID = i.FirstOrDefault().ChiTietDeTaiID,
                                                      DeTai = i.FirstOrDefault().DeTaiID,
                                                      KhoangThoiGian = i.FirstOrDefault().KhoangThoiGian,
                                                      TieuDe = i.FirstOrDefault().TieuDe,
                                                      //TacGia = i.FirstOrDefault().TacGia,
                                                      TenTapChiSachHoiThao = i.FirstOrDefault().TenTapChiSachHoiThao,
                                                      So = i.FirstOrDefault().So,
                                                      Trang = i.FirstOrDefault().Trang,
                                                      NhaXuatBan = i.FirstOrDefault().NhaXuatBan,
                                                      LoaiBaiBao = i.FirstOrDefault().LoaiBaiBao,
                                                      ISSN = i.FirstOrDefault().ISSN,
                                                      NhiemVu = i.FirstOrDefault().NhiemVuBaiBao,
                                                      LoaiNhiemVu = i.FirstOrDefault().LoaiNhiemVu,
                                                      Tap = i.FirstOrDefault().Tap,
                                                      NamDangTai = i.FirstOrDefault().NamDangTai,
                                                      LinkBaiBao = i.FirstOrDefault().LinkBaiBao,
                                                      LinhVucNganhKhoaHoc = i.FirstOrDefault().LinhVucNganhKhoaHoc,
                                                      HeSoAnhHuong = i.FirstOrDefault().HeSoAnhHuong,
                                                      ChiSo = i.FirstOrDefault().ChiSo,
                                                      RankSCIMAG = i.FirstOrDefault().RankSCIMAG,
                                                      DiemTapChi = i.FirstOrDefault().DiemTapChi,
                                                      CapHoiThao = i.FirstOrDefault().CapHoiThao,
                                                      NgayHoiThao = i.FirstOrDefault().NgayHoiThao,
                                                      DiaDiemToChuc = i.FirstOrDefault().DiaDiemToChuc,
                                                      NguoiTaoID = i.FirstOrDefault().NguoiTaoChiTietID,
                                                      FileDinhKem = new List<FileDinhKemModel>(),
                                                      ListTacGia = ((from j in deTais.Where(x => x.ChiTietDeTaiID > 0 && x.TacGiaID > 0 && x.ChiTietDeTaiID == i.FirstOrDefault().ChiTietDeTaiID).ToList().GroupBy(x => x.TacGiaID)
                                                                     select new TacGiaModel()
                                                                     {
                                                                         TacGiaID = j.FirstOrDefault().TacGiaID,
                                                                         ChiTietDeTaiID = j.FirstOrDefault().ChiTietDeTaiID,
                                                                         TenTacGia = j.FirstOrDefault().TenTacGia,
                                                                         CanBoID = j.FirstOrDefault().CanBoTGID,
                                                                         CoQuanID = j.FirstOrDefault().CoQuanTGID,
                                                                     }
                                                       ).ToList())
                                                  }
                                                         ).ToList(),
                                  KetQuaNghienCuuCongBo = (from i in deTais.Where(x => x.ChiTietDeTaiID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.KetQuaNghienCuu).ToList().GroupBy(x => x.ChiTietDeTaiID)
                                                           select new KetQuaNghienCuuNKHModel()
                                                           {
                                                               ChiTietDeTaiID = i.FirstOrDefault().ChiTietDeTaiID,
                                                               DeTai = i.FirstOrDefault().DeTaiID,
                                                               LoaiNhiemVu = i.FirstOrDefault().LoaiNhiemVu,
                                                               NhiemVu = i.FirstOrDefault().NhiemVuBaiBao,
                                                               TieuDe = i.FirstOrDefault().TieuDe,
                                                               KhoangThoiGian = i.FirstOrDefault().KhoangThoiGian,
                                                               NamXuatBan = i.FirstOrDefault().NamXuatBan,
                                                               GhiChu = i.FirstOrDefault().GhiChu,
                                                               //TacGia = i.FirstOrDefault().TacGia,
                                                               //TenTapChiSachHoiThao = i.FirstOrDefault().TenTapChiSachHoiThao,
                                                               //So = i.FirstOrDefault().So,
                                                               //Trang = i.FirstOrDefault().Trang,
                                                               //NhaXuatBan = i.FirstOrDefault().NhaXuatBan,
                                                               NguoiTaoID = i.FirstOrDefault().NguoiTaoChiTietID,
                                                               FileDinhKem = new List<FileDinhKemModel>(),
                                                               ListTacGia = ((from j in deTais.Where(x => x.ChiTietDeTaiID > 0 && x.TacGiaID > 0 && x.ChiTietDeTaiID == i.FirstOrDefault().ChiTietDeTaiID).ToList().GroupBy(x => x.TacGiaID)
                                                                              select new TacGiaModel()
                                                                              {
                                                                                  TacGiaID = j.FirstOrDefault().TacGiaID,
                                                                                  ChiTietDeTaiID = j.FirstOrDefault().ChiTietDeTaiID,
                                                                                  TenTacGia = j.FirstOrDefault().TenTacGia,
                                                                                  CanBoID = j.FirstOrDefault().CanBoTGID,
                                                                                  CoQuanID = j.FirstOrDefault().CoQuanTGID,
                                                                              }
                                                             ).ToList())
                                                           }
                                                         ).ToList(),
                                  SachChuyenKhao = (from i in deTais.Where(x => x.ChiTietDeTaiID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.SachChuyenKhao).ToList().GroupBy(x => x.ChiTietDeTaiID)
                                                    select new SachChuyenKhaoModel()
                                                    {
                                                        ChiTietDeTaiID = i.FirstOrDefault().ChiTietDeTaiID,
                                                        DeTai = i.FirstOrDefault().DeTaiID,
                                                        LoaiNhiemVu = i.FirstOrDefault().LoaiNhiemVu,
                                                        NhiemVu = i.FirstOrDefault().NhiemVuBaiBao,
                                                        TieuDe = i.FirstOrDefault().TieuDe,
                                                        ChuBienID = i.FirstOrDefault().ChuBienID,
                                                        CoQuanChuBienID = i.FirstOrDefault().CoQuanChuBienID,
                                                        NamXuatBan = i.FirstOrDefault().NamXuatBan,
                                                        ISSN = i.FirstOrDefault().ISSN,
                                                        //TacGia = i.FirstOrDefault().TacGia,
                                                        TenTapChiSachHoiThao = i.FirstOrDefault().TenTapChiSachHoiThao,
                                                        So = i.FirstOrDefault().So,
                                                        Trang = i.FirstOrDefault().Trang,
                                                        NhaXuatBan = i.FirstOrDefault().NhaXuatBan,
                                                        NguoiTaoID = i.FirstOrDefault().NguoiTaoChiTietID,
                                                        FileDinhKem = new List<FileDinhKemModel>(),
                                                        ListTacGia = ((from j in deTais.Where(x => x.ChiTietDeTaiID > 0 && x.TacGiaID > 0 && x.ChiTietDeTaiID == i.FirstOrDefault().ChiTietDeTaiID).ToList().GroupBy(x => x.TacGiaID)
                                                                       select new TacGiaModel()
                                                                       {
                                                                           TacGiaID = j.FirstOrDefault().TacGiaID,
                                                                           ChiTietDeTaiID = j.FirstOrDefault().ChiTietDeTaiID,
                                                                           TenTacGia = j.FirstOrDefault().TenTacGia,
                                                                           CanBoID = j.FirstOrDefault().CanBoTGID,
                                                                           CoQuanID = j.FirstOrDefault().CoQuanTGID,
                                                                       }
                                                       ).ToList())
                                                    }
                                                         ).ToList(),
                                  SanPhamDaoTao = (from i in deTais.Where(x => x.ChiTietDeTaiID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTin.SanPhamDaoTao).ToList().GroupBy(x => x.ChiTietDeTaiID)
                                                   select new SanPhamDaoTaoModel()
                                                   {
                                                       ChiTietDeTaiID = i.FirstOrDefault().ChiTietDeTaiID ?? 0,
                                                       DeTai = i.FirstOrDefault().DeTaiID,
                                                       NhiemVu = i.FirstOrDefault().NhiemVuBaiBao,
                                                       LoaiNhiemVu = i.FirstOrDefault().LoaiNhiemVu,
                                                       LoaiDaoTao = i.FirstOrDefault().LoaiDaoTao,
                                                       TenHocVien = i.FirstOrDefault().TenHocVien,
                                                       TenLuanVan = i.FirstOrDefault().TenLuanVan,
                                                       NguoiHuongDan = i.FirstOrDefault().NguoiHuongDan,
                                                       NamBaoVe = i.FirstOrDefault().NamBaoVe,
                                                       NguoiTaoID = i.FirstOrDefault().NguoiTaoChiTietID,

                                                       CoSoDaoTao = i.FirstOrDefault().CoSoDaoTao,
                                                       KhoangThoiGian = i.FirstOrDefault().KhoangThoiGian,
                                                       CapHoiThao = i.FirstOrDefault().CapHoiThao
                                                   }
                                                         ).ToList(),
                              }
                            ).ToList();
                }
                var data = Result.FirstOrDefault();
                var KetQuaNghienCuuDeTai = KetQuaNghienCuu_GetByID(data.DeTaiID);
                if (KetQuaNghienCuuDeTai.Count > 0)
                {
                    if (data.KetQuaNghienCuu == null) data.KetQuaNghienCuu = new List<KetQuaNghienCuuModel>();
                    data.KetQuaNghienCuu = KetQuaNghienCuuDeTai;
                }

                if (data.FileDinhKem != null && data.FileDinhKem.Count > 0)
                {
                    foreach (var item in data.FileDinhKem)
                    {
                        item.FileUrl = item.FileUrl.Replace(@"\\", @"\");
                        item.FileUrl = serverPath + item.FileUrl;
                    }
                }

                var files = GetAllFile(data.DeTaiID);
                if (files.Count > 0)
                {
                    foreach (var item in files)
                    {
                        item.FileUrl = item.FileUrl.Replace(@"\\", @"\");
                        item.FileUrl = serverPath + item.FileUrl;

                        if (item.LoaiFile == (int)EnumLoaiFileDinhKem.SanPhamDeTai)
                        {
                            foreach (var info in data.SanPhamDeTai)
                            {
                                if (item.NghiepVuID == info.ChiTietDeTaiID) info.FileDinhKem.Add(item);
                            }
                        }
                        if (item.LoaiFile == (int)EnumLoaiFileDinhKem.KetQuaChuyenGiao)
                        {
                            foreach (var info in data.KetQuaChuyenGiao)
                            {
                                if (item.NghiepVuID == info.ChiTietDeTaiID) info.FileDinhKem.Add(item);
                            }
                        }
                        if (item.LoaiFile == (int)EnumLoaiFileDinhKem.KetQuaNghienCuu)
                        {
                            foreach (var info in data.KetQuaNghienCuu)
                            {
                                if (item.NghiepVuID == info.KetQuaNghienCuuID) info.FileDinhKem.Add(item);
                            }
                        }
                        if (item.LoaiFile == (int)EnumLoaiFileDinhKem.DanhGiaGiaiDoan)
                        {
                            foreach (var info in data.DanhGiaGiaiDoan)
                            {
                                if (item.NghiepVuID == info.ChiTietDeTaiID) info.FileDinhKem.Add(item);
                            }
                        }
                        if (item.LoaiFile == (int)EnumLoaiFileDinhKem.KetQuaDanhGia)
                        {
                            foreach (var info in data.KetQuaDanhGia)
                            {
                                if (item.NghiepVuID == info.ChiTietDeTaiID) info.FileDinhKem.Add(item);
                            }
                        }
                    }
                }
                #region old
                //Lấy kết quả nghiên cứu đã công bố hoặc đăng ký khác trong lý lịch khoa học vào kết quả nghiên cứu
                //var ketQuaNghienCuu = GetKetQuaNghienCuuByDeTaiID(data.DeTaiID);
                //if (ketQuaNghienCuu.Count > 0)
                //{
                //    if (data.KetQuaNghienCuu == null) data.KetQuaNghienCuu = new List<KetQuaNghienCuuModel>();
                //    data.KetQuaNghienCuu.AddRange(ketQuaNghienCuu);
                //}

                //lấy thông tin bài báo, sách, sản phẩm đào tạo, sản phẩm công bố khác từ lý lịch nhà khoa học
                //var listCT = BaiBaoSachSanPhamNKH(data.DeTaiID);
                //if (listCT.Count > 0)
                //{
                //    foreach (var item in listCT)
                //    {
                //        if (item.LoaiThongTin == EnumLoaiThongTinNhaKhoaHoc.BaiBaoTapChi.GetHashCode())
                //        {
                //            BaiBaoTapChiModel bb = new BaiBaoTapChiModel();
                //            bb.CTNhaKhoaHocID = item.CTNhaKhoaHocID;
                //            bb.DeTai = item.DeTai;
                //            bb.KhoangThoiGian = item.KhoangThoiGian;
                //            bb.TieuDe = item.TieuDe;
                //            bb.TenTapChiSachHoiThao = item.TenTapChiSachHoiThao;
                //            bb.So = item.So;
                //            bb.Trang = item.Trang;
                //            bb.NhaXuatBan = item.NhaXuatBan;
                //            bb.LoaiBaiBao = item.LoaiBaiBao;
                //            bb.ISSN = item.ISSN;
                //            bb.NhiemVu = item.NhiemVuBaiBao;
                //            bb.LoaiNhiemVu = item.LoaiNhiemVu;
                //            bb.Tap = item.Tap;
                //            bb.NamDangTai = item.NamDangTai;
                //            bb.LinkBaiBao = item.LinkBaiBao;
                //            bb.LinhVucNganhKhoaHoc = item.LinhVucNganhKhoaHoc;
                //            bb.HeSoAnhHuong = item.HeSoAnhHuong;
                //            bb.ChiSo = item.ChiSo;
                //            bb.RankSCIMAG = item.RankSCIMAG;
                //            bb.DiemTapChi = item.DiemTapChi;
                //            bb.CapHoiThao = item.CapHoiThao;
                //            bb.NgayHoiThao = item.NgayHoiThao;
                //            bb.DiaDiemToChuc = item.DiaDiemToChuc;
                //            bb.Disable = true;
                //            bb.FileDinhKem = new List<FileDinhKemModel>();
                //            bb.ListTacGia = ((from j in listCT.Where(x => x.CTNhaKhoaHocID > 0 && x.TacGiaID > 0 && x.CTNhaKhoaHocID == item.CTNhaKhoaHocID).ToList().GroupBy(x => x.TacGiaID)
                //                              select new TacGiaModel()
                //                              {
                //                                  TacGiaID = j.FirstOrDefault().TacGiaID,
                //                                  CTNhaKhoaHocID = j.FirstOrDefault().CTNhaKhoaHocID,
                //                                  TenTacGia = j.FirstOrDefault().TenTacGia,
                //                                  CanBoID = j.FirstOrDefault().CanBoTGID,
                //                                  CoQuanID = j.FirstOrDefault().CoQuanTGID,
                //                              }
                //                                       ).ToList());

                //            data.BaiBaoTapChi.Add(bb);
                //        }
                //        else if (item.LoaiThongTin == EnumLoaiThongTinNhaKhoaHoc.KetQuaNghienCuu.GetHashCode())
                //        {
                //            KetQuaNghienCuuNKHModel kq = new KetQuaNghienCuuNKHModel();
                //            kq.CTNhaKhoaHocID = item.CTNhaKhoaHocID;
                //            kq.DeTai = item.DeTai;
                //            kq.LoaiNhiemVu = item.LoaiNhiemVu;
                //            kq.NhiemVu = item.NhiemVuBaiBao;
                //            kq.TieuDe = item.TieuDe;
                //            kq.KhoangThoiGian = item.KhoangThoiGian;
                //            kq.NamXuatBan = item.NamXuatBan;
                //            kq.GhiChu = item.GhiChu;
                //            kq.Disable = true;
                //            kq.FileDinhKem = new List<FileDinhKemModel>();
                //            kq.ListTacGia = ((from j in listCT.Where(x => x.CTNhaKhoaHocID > 0 && x.TacGiaID > 0 && x.CTNhaKhoaHocID == item.CTNhaKhoaHocID).ToList().GroupBy(x => x.TacGiaID)
                //                              select new TacGiaModel()
                //                              {
                //                                  TacGiaID = j.FirstOrDefault().TacGiaID,
                //                                  CTNhaKhoaHocID = j.FirstOrDefault().CTNhaKhoaHocID,
                //                                  TenTacGia = j.FirstOrDefault().TenTacGia,
                //                                  CanBoID = j.FirstOrDefault().CanBoTGID,
                //                                  CoQuanID = j.FirstOrDefault().CoQuanTGID,
                //                              }
                //                                             ).ToList());

                //            data.KetQuaNghienCuuCongBo.Add(kq);
                //        }
                //        else if (item.LoaiThongTin == EnumLoaiThongTinNhaKhoaHoc.SachChuyenKhao.GetHashCode())
                //        {
                //            SachChuyenKhaoModel sck = new SachChuyenKhaoModel();
                //            sck.CTNhaKhoaHocID = item.CTNhaKhoaHocID;
                //            sck.DeTai = item.DeTai;
                //            sck.KhoangThoiGian = item.KhoangThoiGian;
                //            sck.TieuDe = item.TieuDe;
                //            sck.TenTapChiSachHoiThao = item.TenTapChiSachHoiThao;
                //            sck.So = item.So;
                //            sck.Trang = item.Trang;
                //            sck.NhaXuatBan = item.NhaXuatBan;
                //            sck.Disable = true;
                //            sck.FileDinhKem = new List<FileDinhKemModel>();
                //            sck.ListTacGia = ((from j in listCT.Where(x => x.CTNhaKhoaHocID > 0 && x.TacGiaID > 0 && x.CTNhaKhoaHocID == item.CTNhaKhoaHocID).ToList().GroupBy(x => x.TacGiaID)
                //                               select new TacGiaModel()
                //                               {
                //                                   TacGiaID = j.FirstOrDefault().TacGiaID,
                //                                   CTNhaKhoaHocID = j.FirstOrDefault().CTNhaKhoaHocID,
                //                                   TenTacGia = j.FirstOrDefault().TenTacGia,
                //                                   CanBoID = j.FirstOrDefault().CanBoTGID,
                //                                   CoQuanID = j.FirstOrDefault().CoQuanTGID,
                //                               }
                //                                       ).ToList());

                //            data.SachChuyenKhao.Add(sck);
                //        }
                //        else if (item.LoaiThongTin == EnumLoaiThongTinNhaKhoaHoc.SanPhamDaoTao.GetHashCode())
                //        {
                //            SanPhamDaoTaoModel sp = new SanPhamDaoTaoModel();
                //            sp.CTNhaKhoaHocID = item.CTNhaKhoaHocID;
                //            sp.DeTai = item.DeTai;
                //            sp.NhiemVu = item.NhiemVu;
                //            sp.LoaiNhiemVu = item.LoaiNhiemVu;
                //            sp.LoaiDaoTao = item.LoaiDaoTao;
                //            sp.TenHocVien = item.TenHocVien;
                //            sp.TenLuanVan = item.TenLuanVan;
                //            sp.NguoiHuongDan = item.NguoiHuongDan;
                //            sp.NamBaoVe = item.NamBaoVe;
                //            sp.Disable = true;

                //            data.SanPhamDaoTao.Add(sp);
                //        }
                //    }
                //}
                #endregion
                var dt = BaiBaoSachSanPhamNKH(data.DeTaiID);
                if(dt.BaiBaoTapChi != null && dt.BaiBaoTapChi.Count > 0)
                {
                    data.BaiBaoTapChi.AddRange(dt.BaiBaoTapChi);
                }
                if (dt.KetQuaNghienCuuCongBo != null && dt.KetQuaNghienCuuCongBo.Count > 0)
                {
                    data.KetQuaNghienCuuCongBo.AddRange(dt.KetQuaNghienCuuCongBo);
                }
                if (dt.SachChuyenKhao != null && dt.SachChuyenKhao.Count > 0)
                {
                    data.SachChuyenKhao.AddRange(dt.SachChuyenKhao);
                }
                if (dt.SanPhamDaoTao != null && dt.SanPhamDaoTao.Count > 0)
                {
                    data.SanPhamDaoTao.AddRange(dt.SanPhamDaoTao);
                }

                return data;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Thêm mới đề tài
        /// </summary>
        /// <param name="DeXuatDeTaiModel"></param>
        /// <returns></returns>
        public BaseResultModel Insert(DeTaiModel DeTaiModel)
        {
            var Result = new BaseResultModel();
            var coQuanNgoaiTruong = Utils.ConvertToInt32(new SystemConfigDAL().GetByKey("ID_COQUAN_NGOAITRUONG").ConfigValue, 999999999);
            try
            {
                int NamBatDauInt = 0;
                int NamKetThucInt = 0;
                if (DeTaiModel.NamBatDau != null && DeTaiModel.NamBatDau.Length > 5)
                {
                    NamBatDauInt = Utils.ConvertToInt32(DeTaiModel.NamBatDau.Substring(DeTaiModel.NamBatDau.Length - 4), 0);
                }
                if (DeTaiModel.NamKetThuc != null && DeTaiModel.NamKetThuc.Length > 5)
                {
                    NamKetThucInt = Utils.ConvertToInt32(DeTaiModel.NamKetThuc.Substring(DeTaiModel.NamKetThuc.Length - 4), 0);
                }

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("DeTaiID",SqlDbType.Int),
                    new SqlParameter("MaDeTai", SqlDbType.NVarChar),
                    new SqlParameter("TenDeTai", SqlDbType.NVarChar),
                    new SqlParameter("LoaiHinhNghienCuu", SqlDbType.Int),
                    new SqlParameter("LinhVucNghienCuu", SqlDbType.Int),
                    new SqlParameter("LinhVucKinhTeXaHoi", SqlDbType.Int),
                    new SqlParameter("CapQuanLy", SqlDbType.Int),
                    new SqlParameter("NhiemVu",SqlDbType.Int),
                    new SqlParameter("CoQuanChuQuan", SqlDbType.NVarChar),
                    new SqlParameter("DonViQuanLyKhoaHoc", SqlDbType.NVarChar),
                    new SqlParameter("ChuNhiemDeTaiID",SqlDbType.Int),
                    new SqlParameter("NamBatDau",SqlDbType.NVarChar),
                    new SqlParameter("NamKetThuc",SqlDbType.NVarChar),
                    new SqlParameter("KinhPhiDHSP",SqlDbType.Decimal),
                    new SqlParameter("NguonKhac",SqlDbType.Decimal),
                    new SqlParameter("MucTieu",SqlDbType.NVarChar),
                    new SqlParameter("CacNoiDungChinh",SqlDbType.NVarChar),
                    new SqlParameter("SanPhamDangKy",SqlDbType.NVarChar),
                    new SqlParameter("KhaNangUngDung",SqlDbType.NVarChar),
                    new SqlParameter("TrangThai",SqlDbType.Int),
                    new SqlParameter("VaiTroChuNhiemID",SqlDbType.Int),
                    new SqlParameter("NguoiTaoID",SqlDbType.Int),
                    new SqlParameter("DonViPhoiHop",SqlDbType.NVarChar),
                    new SqlParameter("CoQuanChuNhiemID",SqlDbType.Int),
                    new SqlParameter("NamBatDau_Int",SqlDbType.Int),
                    new SqlParameter("NamKetThuc_Int",SqlDbType.Int),
                    new SqlParameter("DeXuatID",SqlDbType.Int),
                };
                parameters[0].Direction = ParameterDirection.Output;
                parameters[0].Size = 8;
                parameters[1].Value = DeTaiModel.MaDeTai ?? Convert.DBNull;
                parameters[2].Value = DeTaiModel.TenDeTai ?? Convert.DBNull;
                parameters[3].Value = DeTaiModel.LoaiHinhNghienCuu ?? Convert.DBNull;
                parameters[4].Value = DeTaiModel.LinhVucNghienCuu ?? Convert.DBNull;
                parameters[5].Value = DeTaiModel.LinhVucKinhTeXaHoi ?? Convert.DBNull;
                parameters[6].Value = DeTaiModel.CapQuanLy ?? Convert.DBNull;
                parameters[7].Value = DeTaiModel.NhiemVu ?? Convert.DBNull;
                parameters[8].Value = DeTaiModel.CoQuanChuQuan ?? Convert.DBNull;
                parameters[9].Value = DeTaiModel.DonViQuanLyKhoaHoc ?? Convert.DBNull;
                parameters[10].Value = DeTaiModel.ChuNhiemDeTaiID ?? Convert.DBNull;
                parameters[11].Value = DeTaiModel.NamBatDau ?? Convert.DBNull;
                parameters[12].Value = DeTaiModel.NamKetThuc ?? Convert.DBNull;
                parameters[13].Value = DeTaiModel.KinhPhiDHSP ?? Convert.DBNull;
                parameters[14].Value = DeTaiModel.NguonKhac ?? Convert.DBNull;
                parameters[15].Value = DeTaiModel.MucTieu ?? Convert.DBNull;
                parameters[16].Value = DBNull.Value;
                parameters[17].Value = DeTaiModel.SanPhamDangKy ?? Convert.DBNull;
                parameters[18].Value = DeTaiModel.KhaNangUngDung ?? Convert.DBNull;
                parameters[19].Value = (int)EnumTrangThaiDeTai.DangThucHien;
                parameters[20].Value = DeTaiModel.VaiTroChuNhiemID ?? Convert.DBNull;
                parameters[21].Value = DeTaiModel.NguoiTaoID ?? Convert.DBNull;
                parameters[22].Value = DeTaiModel.DonViPhoiHop ?? Convert.DBNull;
                parameters[23].Value = DeTaiModel.CoQuanChuNhiemID ?? Convert.DBNull;
                parameters[24].Value = NamBatDauInt != 0 ? NamBatDauInt : Convert.DBNull;
                parameters[25].Value = NamKetThucInt != 0 ? NamKetThucInt : Convert.DBNull;
                parameters[26].Value = DeTaiModel.DeXuatID ?? Convert.DBNull;

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_DeTai_Insert", parameters);
                            int DeTaiID = Utils.ConvertToInt32(parameters[0].Value, 0);
                            if (Result.Status > 0 && DeTaiID > 0)
                            {
                                //insert chủ nhiệm đề tài
                                SqlParameter[] parms_cn = new SqlParameter[]{
                                            new SqlParameter("DeTaiID", SqlDbType.Int),
                                            new SqlParameter("CanBoID", SqlDbType.Int),
                                            new SqlParameter("VaiTro", SqlDbType.Int),
                                            new SqlParameter("LaCanBoTrongTruong", SqlDbType.Int),
                                            new SqlParameter("LaChuNhiemDeTai", SqlDbType.Bit),
                                    };
                                parms_cn[0].Value = DeTaiID;
                                parms_cn[1].Value = DeTaiModel.ChuNhiemDeTaiID;
                                parms_cn[2].Value = DeTaiModel.VaiTroChuNhiemID ?? Convert.DBNull;
                                parms_cn[3].Value = DeTaiModel.CoQuanChuNhiemID != coQuanNgoaiTruong ? 1 : 0;
                                parms_cn[4].Value = true;
                                SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_ThanhVienNghienCuu_Insert", parms_cn);

                                if (DeTaiModel.ThanhVienNghienCuu != null && DeTaiModel.ThanhVienNghienCuu.Count > 0)
                                {
                                    //insert thành viên tham gia nghiên cứu 
                                    foreach (var item in DeTaiModel.ThanhVienNghienCuu)
                                    {
                                        SqlParameter[] parms_tv = new SqlParameter[]{
                                            new SqlParameter("DeTaiID", SqlDbType.Int),
                                            new SqlParameter("CanBoID", SqlDbType.Int),
                                            new SqlParameter("VaiTro", SqlDbType.Int),
                                            new SqlParameter("LaCanBoTrongTruong", SqlDbType.Int),
                                            new SqlParameter("LaChuNhiemDeTai", SqlDbType.Bit),
                                        };

                                        parms_tv[0].Value = DeTaiID;
                                        parms_tv[1].Value = item.CanBoID ?? Convert.DBNull;
                                        parms_tv[2].Value = item.VaiTro ?? Convert.DBNull;
                                        parms_tv[3].Value = item.LaCanBoTrongTruong ?? Convert.DBNull;
                                        parms_tv[4].Value = false;

                                        SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_ThanhVienNghienCuu_Insert", parms_tv);

                                    }
                                }
                                if (DeTaiModel.CacNoiDungChinh != null && DeTaiModel.CacNoiDungChinh.Count > 0)
                                {
                                    //insert nội dung chính 
                                    foreach (var item in DeTaiModel.CacNoiDungChinh)
                                    {
                                        SqlParameter[] parms_nd = new SqlParameter[]{
                                            new SqlParameter("ChiTietDeTaiID", SqlDbType.Int),
                                            new SqlParameter("DeTaiID", SqlDbType.Int),
                                            new SqlParameter("TenNoiDung", SqlDbType.NVarChar),
                                            new SqlParameter("Mota", SqlDbType.NVarChar),
                                            new SqlParameter("LoaiThongTin", SqlDbType.Int),
                                            new SqlParameter("NguoiTaoID", SqlDbType.Int),
                                        };

                                        parms_nd[0].Direction = ParameterDirection.Output;
                                        parms_nd[0].Size = 8;
                                        parms_nd[1].Value = DeTaiID;
                                        parms_nd[2].Value = item.TenNoiDung ?? Convert.DBNull;
                                        parms_nd[3].Value = item.MoTa ?? Convert.DBNull;
                                        parms_nd[4].Value = EnumLoaiThongTin.TienDoThucHien.GetHashCode();
                                        parms_nd[5].Value = DeTaiModel.NguoiTaoID ?? Convert.DBNull;

                                        SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_NoiDungChinh_Insert", parms_nd);

                                    }
                                }
                            }
                            Result.Data = DeTaiID;
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            Result.Status = -1;
                            Result.Message = ConstantLogMessage.API_Error_System;
                            trans.Rollback();
                            throw ex;
                        }
                    }

                    Result.Message = ConstantLogMessage.Alert_Insert_Success("Đề tài");
                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                throw ex;
            }
            if (Result.Status > 0)
            {
                Result.Status = 1;
            }
            return Result;
        }

        /// <summary>
        /// Chỉnh sửa nội dung chính
        /// </summary>
        /// <param name="NoiDungChinhModel"></param>
        /// <returns></returns>
        public BaseResultModel Edit_NoiDungChinh(NoiDungChinhModel NoiDungChinhModel)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parms_nd = new SqlParameter[]{
                    new SqlParameter("ChiTietDeTaiID", SqlDbType.Int),
                    new SqlParameter("DeTaiID", SqlDbType.Int),
                    new SqlParameter("TenNoiDung", SqlDbType.NVarChar),
                    new SqlParameter("Mota", SqlDbType.NVarChar),
                    new SqlParameter("LoaiThongTin", SqlDbType.Int),
                    new SqlParameter("NguoiTaoID", SqlDbType.Int),
                };

                parms_nd[0].Value = NoiDungChinhModel.ChiTietDeTaiID;
                parms_nd[1].Value = NoiDungChinhModel.DeTaiID;
                parms_nd[2].Value = NoiDungChinhModel.TenNoiDung ?? Convert.DBNull;
                parms_nd[3].Value = NoiDungChinhModel.MoTa ?? Convert.DBNull;
                parms_nd[4].Value = EnumLoaiThongTin.TienDoThucHien.GetHashCode();
                parms_nd[5].Value = NoiDungChinhModel.NguoiTaoID ?? Convert.DBNull;

                if (NoiDungChinhModel.ChiTietDeTaiID == null)
                {
                    parms_nd[0].Direction = ParameterDirection.Output;
                    parms_nd[0].Size = 8;
                }

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            if (NoiDungChinhModel.ChiTietDeTaiID == null)
                            {
                                Result.Status = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v1_NoiDungChinh_Insert", parms_nd);
                                Result.Data = Utils.ConvertToInt32(parms_nd[0].Value, 0);
                                Result.Message = ConstantLogMessage.Alert_Insert_Success("nội dung chính");
                            }
                            else
                            {
                                Result.Status = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v1_NoiDungChinh_Update", parms_nd);
                                Result.Data = NoiDungChinhModel.ChiTietDeTaiID;
                                Result.Message = ConstantLogMessage.Alert_Update_Success("nội dung chính");
                            }
                            trans.Commit();
                            Result.Status = 1;
                        }
                        catch
                        {
                            Result.Status = -1;
                            Result.Message = ConstantLogMessage.API_Error_System;
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                throw;
            }
            return Result;
        }

        /// <summary>
        /// Cập nhật trạng thái đề tài
        /// </summary>
        /// <param name="DeTaiModel"></param>
        /// <returns></returns>
        public BaseResultModel Update_TrangThaiDeTai(DeTaiModel DeTaiModel)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("DeTaiID", SqlDbType.Int),
                    new SqlParameter("TrangThai", SqlDbType.Int),
                };
                parameters[0].Value = DeTaiModel.DeTaiID;
                parameters[1].Value = DeTaiModel.TrangThai;

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Result.Status = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v1_DeTai_UpdateTrangThai", parameters);
                            trans.Commit();
                            Result.Status = 1;
                            Result.Message = ConstantLogMessage.Alert_Update_Success("Đề tài");
                        }
                        catch
                        {
                            Result.Status = -1;
                            Result.Message = ConstantLogMessage.API_Error_System;
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                throw;
            }
            return Result;
        }

        /// <summary>
        /// Cập nhật đề tài
        /// </summary>
        /// <param name="DeTaiModel"></param>
        /// <returns></returns>
        public BaseResultModel Update(DeTaiModel DeTaiModel)
        {
            var Result = new BaseResultModel();
            var coQuanNgoaiTruong = Utils.ConvertToInt32(new SystemConfigDAL().GetByKey("ID_COQUAN_NGOAITRUONG").ConfigValue, 999999999);
            var DeTaiOld = GetByID(DeTaiModel.DeTaiID, "");
            if (DeTaiModel.NamBatDau != DeTaiOld.NamBatDau || DeTaiModel.NamKetThuc != DeTaiOld.NamKetThuc)
            {
                DeTaiModel.SoLanGiaHanThucHien = DeTaiOld.SoLanGiaHanThucHien + 1;
            }
            else DeTaiModel.SoLanGiaHanThucHien = DeTaiOld.SoLanGiaHanThucHien;
            try
            {
                int NamBatDauInt = 0;
                int NamKetThucInt = 0;
                if (DeTaiModel.NamBatDau != null && DeTaiModel.NamBatDau.Length > 5)
                {
                    NamBatDauInt = Utils.ConvertToInt32(DeTaiModel.NamBatDau.Substring(DeTaiModel.NamBatDau.Length - 4), 0);
                }
                if (DeTaiModel.NamKetThuc != null && DeTaiModel.NamKetThuc.Length > 5)
                {
                    NamKetThucInt = Utils.ConvertToInt32(DeTaiModel.NamKetThuc.Substring(DeTaiModel.NamKetThuc.Length - 4), 0);
                }

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("DeTaiID",SqlDbType.Int),
                    new SqlParameter("MaDeTai", SqlDbType.NVarChar),
                    new SqlParameter("TenDeTai", SqlDbType.NVarChar),
                    new SqlParameter("LoaiHinhNghienCuu", SqlDbType.Int),
                    new SqlParameter("LinhVucNghienCuu", SqlDbType.Int),
                    new SqlParameter("LinhVucKinhTeXaHoi", SqlDbType.Int),
                    new SqlParameter("CapQuanLy", SqlDbType.Int),
                    new SqlParameter("NhiemVu",SqlDbType.Int),
                    new SqlParameter("CoQuanChuQuan", SqlDbType.NVarChar),
                    new SqlParameter("DonViQuanLyKhoaHoc", SqlDbType.NVarChar),
                    new SqlParameter("ChuNhiemDeTaiID",SqlDbType.Int),
                    new SqlParameter("NamBatDau",SqlDbType.NVarChar),
                    new SqlParameter("NamKetThuc",SqlDbType.NVarChar),
                    new SqlParameter("KinhPhiDHSP",SqlDbType.Decimal),
                    new SqlParameter("NguonKhac",SqlDbType.Decimal),
                    new SqlParameter("MucTieu",SqlDbType.NVarChar),
                    new SqlParameter("CacNoiDungChinh",SqlDbType.NVarChar),
                    new SqlParameter("SanPhamDangKy",SqlDbType.NVarChar),
                    new SqlParameter("KhaNangUngDung",SqlDbType.NVarChar),
                    new SqlParameter("VaiTroChuNhiemID",SqlDbType.Int),
                    new SqlParameter("TrangThai",SqlDbType.Int),
                    new SqlParameter("DonViPhoiHop",SqlDbType.NVarChar),
                    new SqlParameter("CoQuanChuNhiemID",SqlDbType.Int),
                    new SqlParameter("NamBatDau_Int",SqlDbType.Int),
                    new SqlParameter("NamKetThuc_Int",SqlDbType.Int),
                    new SqlParameter("SoLanGiaHanThucHien",SqlDbType.Int),
                };
                parameters[0].Value = DeTaiModel.DeTaiID;
                parameters[1].Value = DeTaiModel.MaDeTai ?? Convert.DBNull;
                parameters[2].Value = DeTaiModel.TenDeTai ?? Convert.DBNull;
                parameters[3].Value = DeTaiModel.LoaiHinhNghienCuu ?? Convert.DBNull;
                parameters[4].Value = DeTaiModel.LinhVucNghienCuu ?? Convert.DBNull;
                parameters[5].Value = DeTaiModel.LinhVucKinhTeXaHoi ?? Convert.DBNull;
                parameters[6].Value = DeTaiModel.CapQuanLy ?? Convert.DBNull;
                parameters[7].Value = DeTaiModel.NhiemVu ?? Convert.DBNull;
                parameters[8].Value = DeTaiModel.CoQuanChuQuan ?? Convert.DBNull;
                parameters[9].Value = DeTaiModel.DonViQuanLyKhoaHoc ?? Convert.DBNull;
                parameters[10].Value = DeTaiModel.ChuNhiemDeTaiID ?? Convert.DBNull;
                parameters[11].Value = DeTaiModel.NamBatDau ?? Convert.DBNull;
                parameters[12].Value = DeTaiModel.NamKetThuc ?? Convert.DBNull;
                parameters[13].Value = DeTaiModel.KinhPhiDHSP ?? Convert.DBNull;
                parameters[14].Value = DeTaiModel.NguonKhac ?? Convert.DBNull;
                parameters[15].Value = DeTaiModel.MucTieu ?? Convert.DBNull;
                parameters[16].Value = DBNull.Value;
                parameters[17].Value = DeTaiModel.SanPhamDangKy ?? Convert.DBNull;
                parameters[18].Value = DeTaiModel.KhaNangUngDung ?? Convert.DBNull;
                parameters[19].Value = DeTaiModel.VaiTroChuNhiemID ?? Convert.DBNull;
                parameters[20].Value = DeTaiModel.TrangThai;
                parameters[21].Value = DeTaiModel.DonViPhoiHop;
                parameters[22].Value = DeTaiModel.CoQuanChuNhiemID;
                parameters[23].Value = NamBatDauInt != 0 ? NamBatDauInt : Convert.DBNull;
                parameters[24].Value = NamKetThucInt != 0 ? NamKetThucInt : Convert.DBNull;
                parameters[25].Value = DeTaiModel.SoLanGiaHanThucHien ?? Convert.DBNull;

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Result.Status = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v1_DeTai_Update", parameters);

                            //Xóa thành viên tham gia cũ
                            SqlParameter[] parms_del = new SqlParameter[]{
                                            new SqlParameter("DeTaiID", SqlDbType.Int),
                                        };
                            parms_del[0].Value = DeTaiModel.DeTaiID;
                            SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_ThanhVienNghienCuu_Delete", parms_del);

                            //insert chủ nhiệm đề tài
                            SqlParameter[] parms_cn = new SqlParameter[]{
                                            new SqlParameter("DeTaiID", SqlDbType.Int),
                                            new SqlParameter("CanBoID", SqlDbType.Int),
                                            new SqlParameter("VaiTro", SqlDbType.Int),
                                            new SqlParameter("LaCanBoTrongTruong", SqlDbType.Int),
                                            new SqlParameter("LaChuNhiemDeTai", SqlDbType.Bit),
                                    };
                            parms_cn[0].Value = DeTaiModel.DeTaiID;
                            parms_cn[1].Value = DeTaiModel.ChuNhiemDeTaiID;
                            parms_cn[2].Value = DeTaiModel.VaiTroChuNhiemID;
                            parms_cn[3].Value = DeTaiModel.CoQuanChuNhiemID != coQuanNgoaiTruong ? 1 : 0;
                            parms_cn[4].Value = true;
                            SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_ThanhVienNghienCuu_Insert", parms_cn);

                            if (DeTaiModel.ThanhVienNghienCuu != null && DeTaiModel.ThanhVienNghienCuu.Count > 0)
                            {
                                //insert thành viên tham gia nghiên cứu 
                                foreach (var item in DeTaiModel.ThanhVienNghienCuu)
                                {
                                    SqlParameter[] parms_tv = new SqlParameter[]{
                                            new SqlParameter("DeTaiID", SqlDbType.Int),
                                            new SqlParameter("CanBoID", SqlDbType.Int),
                                            new SqlParameter("VaiTro", SqlDbType.Int),
                                            new SqlParameter("LaCanBoTrongTruong", SqlDbType.Int),
                                            new SqlParameter("LaChuNhiemDeTai", SqlDbType.Bit),
                                        };

                                    parms_tv[0].Value = DeTaiModel.DeTaiID;
                                    parms_tv[1].Value = item.CanBoID ?? Convert.DBNull;
                                    parms_tv[2].Value = item.VaiTro ?? Convert.DBNull;
                                    parms_tv[3].Value = item.LaCanBoTrongTruong ?? Convert.DBNull;
                                    parms_tv[4].Value = false;

                                    SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_ThanhVienNghienCuu_Insert", parms_tv);
                                }
                            }

                            if (DeTaiModel.CacNoiDungChinh != null && DeTaiModel.CacNoiDungChinh.Count > 0)
                            {
                                //xóa nội dung cũ
                                List<int> listID = new List<int>();
                                foreach (var item in DeTaiModel.CacNoiDungChinh)
                                {
                                    if (item.ChiTietDeTaiID > 0)
                                    {
                                        listID.Add(item.ChiTietDeTaiID ?? 0);
                                    }
                                }

                                listID = listID.Distinct().ToList();
                                var pList = new SqlParameter("@ListID", SqlDbType.Structured);
                                pList.TypeName = "dbo.list_ID";
                                var tbID = new DataTable();
                                tbID.Columns.Add("ID", typeof(string));
                                listID.ForEach(x => tbID.Rows.Add(x));

                                SqlParameter[] parms = new SqlParameter[]{
                                    new SqlParameter("LoaiThongTin", SqlDbType.Int),
                                    pList,
                                    new SqlParameter("DeTaiID", SqlDbType.Int),
                                };
                                parms[0].Value = EnumLoaiThongTin.TienDoThucHien.GetHashCode();
                                parms[1].Value = tbID;
                                parms[2].Value = DeTaiModel.DeTaiID;

                                SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_NoiDungChinh_Delete", parms);

                                //insert nội dung chính 
                                foreach (var item in DeTaiModel.CacNoiDungChinh)
                                {
                                    if (item.ChiTietDeTaiID == null || item.ChiTietDeTaiID == 0)
                                    {
                                        SqlParameter[] parms_nd = new SqlParameter[]{
                                            new SqlParameter("ChiTietDeTaiID", SqlDbType.Int),
                                            new SqlParameter("DeTaiID", SqlDbType.Int),
                                            new SqlParameter("TenNoiDung", SqlDbType.NVarChar),
                                            new SqlParameter("Mota", SqlDbType.NVarChar),
                                            new SqlParameter("LoaiThongTin", SqlDbType.Int),
                                            new SqlParameter("NguoiTaoID", SqlDbType.Int),
                                        };

                                        parms_nd[0].Direction = ParameterDirection.Output;
                                        parms_nd[0].Size = 8;
                                        parms_nd[1].Value = DeTaiModel.DeTaiID;
                                        parms_nd[2].Value = item.TenNoiDung ?? Convert.DBNull;
                                        parms_nd[3].Value = item.MoTa ?? Convert.DBNull;
                                        parms_nd[4].Value = EnumLoaiThongTin.TienDoThucHien.GetHashCode();
                                        parms_nd[5].Value = DeTaiModel.NguoiTaoID ?? Convert.DBNull;

                                        SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_NoiDungChinh_Insert", parms_nd);
                                    }
                                    else
                                    {
                                        SqlParameter[] parms_nd = new SqlParameter[]{
                                            new SqlParameter("ChiTietDeTaiID", SqlDbType.Int),
                                            new SqlParameter("DeTaiID", SqlDbType.Int),
                                            new SqlParameter("TenNoiDung", SqlDbType.NVarChar),
                                            new SqlParameter("Mota", SqlDbType.NVarChar),
                                            new SqlParameter("LoaiThongTin", SqlDbType.Int),
                                            new SqlParameter("NguoiTaoID", SqlDbType.Int),
                                        };

                                        parms_nd[0].Value = item.ChiTietDeTaiID;
                                        parms_nd[1].Value = DeTaiModel.DeTaiID;
                                        parms_nd[2].Value = item.TenNoiDung ?? Convert.DBNull;
                                        parms_nd[3].Value = item.MoTa ?? Convert.DBNull;
                                        parms_nd[4].Value = EnumLoaiThongTin.TienDoThucHien.GetHashCode();
                                        parms_nd[5].Value = DeTaiModel.NguoiTaoID ?? Convert.DBNull;

                                        SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_NoiDungChinh_Update", parms_nd);
                                    }
                                }
                            }

                            trans.Commit();
                            Result.Status = 1;
                            Result.Message = ConstantLogMessage.Alert_Update_Success("Đề tài");
                        }
                        catch
                        {
                            Result.Status = -1;
                            Result.Message = ConstantLogMessage.API_Error_System;
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                throw;
            }
            return Result;
        }

        /// <summary>
        /// Chỉnh sửa thông tin chi tiết đề tài
        /// </summary>
        /// <param name="ThongTinChiTiet"></param>
        /// <returns></returns>
        public BaseResultModel Edit_ThongTinChiTiet(ThongTinChiTietDeTaiModel ThongTinChiTiet)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parms = new SqlParameter[]{
                    new SqlParameter("DeTaiID", SqlDbType.Int),                     //0
                    new SqlParameter("NgayThucHien", SqlDbType.DateTime),           //1
                    new SqlParameter("NoiDungThucHien", SqlDbType.NVarChar),        //2
                    new SqlParameter("KetQuaDatDuoc", SqlDbType.NVarChar),          //3
                    new SqlParameter("TongKinhPhiDuocDuyet", SqlDbType.Decimal),    //4
                    new SqlParameter("TienDoCap", SqlDbType.Decimal),               //5
                    new SqlParameter("TienDoQuyetToan", SqlDbType.Decimal),         //6
                    new SqlParameter("NoiDungDaLam", SqlDbType.NVarChar),           //7
                    new SqlParameter("SanPham", SqlDbType.NVarChar),                //8
                    new SqlParameter("NoiDungChuyenGiao", SqlDbType.NVarChar),      //9
                    new SqlParameter("ThoiGianThucHien", SqlDbType.NVarChar),       //10
                    new SqlParameter("NoiDungDanhGia", SqlDbType.NVarChar),         //11
                    new SqlParameter("LoaiThongTin", SqlDbType.Int),                //12
                    new SqlParameter("ChiTietDeTaiID", SqlDbType.Int),              //13
                    new SqlParameter("NguoiTaoID", SqlDbType.Int),                  //14
                    new SqlParameter("KetQuaThucHien", SqlDbType.NVarChar),         //15
                    new SqlParameter("LoaiKetQua", SqlDbType.Int),                  //16
                    new SqlParameter("XepLoai", SqlDbType.Int),                     //17
                    new SqlParameter("XepLoaiKhac", SqlDbType.NVarChar),            //18
                    new SqlParameter("LoaiNghiemThu", SqlDbType.Int),               //19
                    new SqlParameter("NgayNghiemThu", SqlDbType.DateTime),          //20
                    new SqlParameter("QuyetDinh", SqlDbType.NVarChar),              //21
                    new SqlParameter("TrangThaiTienDo", SqlDbType.Int),             //22
                    new SqlParameter("GhiChu", SqlDbType.NVarChar),                 //23

                    new SqlParameter("CoQuanID", SqlDbType.Int),                    //24
                    new SqlParameter("TieuDe", SqlDbType.NVarChar),                 //25
                    new SqlParameter("TacGia", SqlDbType.NVarChar),                 //26
                    new SqlParameter("TenTapChiSachHoiThao", SqlDbType.NVarChar),   //27
                    new SqlParameter("So", SqlDbType.NVarChar),                     //28
                    new SqlParameter("Trang", SqlDbType.NVarChar),                  //29
                    new SqlParameter("NhaXuatBan", SqlDbType.NVarChar),             //30
                    new SqlParameter("KhoangThoiGian", SqlDbType.NVarChar),         //31
                    new SqlParameter("LoaiBaiBao", SqlDbType.Int),                  //32
                    new SqlParameter("ISSN", SqlDbType.NVarChar),                   //33
                    new SqlParameter("NhiemVu", SqlDbType.Int),                     //34
                    new SqlParameter("LoaiNhiemVu", SqlDbType.Int),                 //35
                    new SqlParameter("TenHoiThao", SqlDbType.NVarChar),             //36                                                        
                    new SqlParameter("Tap", SqlDbType.NVarChar),                    //37
                    new SqlParameter("NamDangTai", SqlDbType.Int),                  //38
                    new SqlParameter("LinkBaiBao", SqlDbType.NVarChar),             //39
                    new SqlParameter("LinhVucNganhKhoaHoc", SqlDbType.Int),         //40
                    new SqlParameter("HeSoAnhHuong", SqlDbType.NVarChar),           //41
                    new SqlParameter("ChiSo", SqlDbType.Int),                       //42
                    new SqlParameter("RankSCIMAG", SqlDbType.Int),                  //43
                    new SqlParameter("DiemTapChi", SqlDbType.Decimal),              //44
                    new SqlParameter("CapHoiThao", SqlDbType.Int),                  //45
                    new SqlParameter("NgayHoiThao", SqlDbType.DateTime),            //46
                    new SqlParameter("DiaDiemToChuc", SqlDbType.NVarChar),          //47
                    new SqlParameter("LoaiDaoTao", SqlDbType.Int),                  //48
                    new SqlParameter("TenHocVien", SqlDbType.NVarChar),             //49
                    new SqlParameter("TenLuanVan", SqlDbType.NVarChar),             //50
                    new SqlParameter("NguoiHuongDan", SqlDbType.NVarChar),          //51
                    new SqlParameter("NamBaoVe", SqlDbType.NVarChar),               //52                                                            
                    new SqlParameter("ChuBienID", SqlDbType.NVarChar),              //53
                    new SqlParameter("CoQuanChuBienID", SqlDbType.NVarChar),        //54
                    new SqlParameter("NamXuatBan", SqlDbType.Int),                  //55
                    new SqlParameter("CoSoDaoTao", SqlDbType.NVarChar),             //56
                };
                parms[0].Value = DBNull.Value;
                parms[1].Value = DBNull.Value;
                parms[2].Value = DBNull.Value;
                parms[3].Value = DBNull.Value;
                parms[4].Value = DBNull.Value;
                parms[5].Value = DBNull.Value;
                parms[6].Value = DBNull.Value;
                parms[7].Value = DBNull.Value;
                parms[8].Value = DBNull.Value;
                parms[9].Value = DBNull.Value;
                parms[10].Value = DBNull.Value;
                parms[11].Value = DBNull.Value;
                parms[12].Value = DBNull.Value;
                parms[13].Value = DBNull.Value;
                parms[14].Value = DBNull.Value;
                parms[15].Value = DBNull.Value;
                parms[16].Value = DBNull.Value;
                parms[17].Value = DBNull.Value;
                parms[18].Value = DBNull.Value;
                parms[19].Value = DBNull.Value;
                parms[20].Value = DBNull.Value;
                parms[21].Value = DBNull.Value;
                parms[22].Value = DBNull.Value;
                parms[23].Value = DBNull.Value;
                parms[24].Value = DBNull.Value;
                parms[25].Value = DBNull.Value;
                parms[26].Value = DBNull.Value;
                parms[27].Value = DBNull.Value;
                parms[28].Value = DBNull.Value;
                parms[29].Value = DBNull.Value;
                parms[30].Value = DBNull.Value;
                parms[31].Value = DBNull.Value;
                parms[32].Value = DBNull.Value;
                parms[33].Value = DBNull.Value;
                parms[34].Value = DBNull.Value;
                parms[35].Value = DBNull.Value;
                parms[36].Value = DBNull.Value;
                parms[37].Value = DBNull.Value;
                parms[38].Value = DBNull.Value;
                parms[39].Value = DBNull.Value;
                parms[40].Value = DBNull.Value;
                parms[41].Value = DBNull.Value;
                parms[42].Value = DBNull.Value;
                parms[43].Value = DBNull.Value;
                parms[44].Value = DBNull.Value;
                parms[45].Value = DBNull.Value;
                parms[46].Value = DBNull.Value;
                parms[47].Value = DBNull.Value;
                parms[48].Value = DBNull.Value;
                parms[49].Value = DBNull.Value;
                parms[50].Value = DBNull.Value;
                parms[51].Value = DBNull.Value;
                parms[52].Value = DBNull.Value;
                parms[53].Value = DBNull.Value;
                parms[54].Value = DBNull.Value;
                parms[55].Value = DBNull.Value;
                parms[56].Value = DBNull.Value;

                if (ThongTinChiTiet.LoaiThongTin == (int)EnumLoaiThongTin.TienDoThucHien)
                {
                    parms[0].Value = ThongTinChiTiet.DeTaiID;
                    parms[1].Value = ThongTinChiTiet.NgayThucHien ?? Convert.DBNull;
                    parms[2].Value = ThongTinChiTiet.NoiDungThucHien ?? Convert.DBNull;
                    parms[3].Value = ThongTinChiTiet.KetQuaDatDuoc ?? Convert.DBNull;
                    parms[4].Value = DBNull.Value;
                    parms[5].Value = DBNull.Value;
                    parms[6].Value = DBNull.Value;
                    parms[7].Value = DBNull.Value;
                    parms[8].Value = DBNull.Value;
                    parms[9].Value = DBNull.Value;
                    parms[10].Value = DBNull.Value;
                    parms[11].Value = DBNull.Value;
                    parms[12].Value = (int)EnumLoaiThongTin.TienDoThucHien;
                    parms[13].Value = ThongTinChiTiet.ChiTietDeTaiID ?? Convert.DBNull;
                    parms[14].Value = ThongTinChiTiet.NguoiTaoID ?? Convert.DBNull;
                    parms[15].Value = DBNull.Value;
                    parms[16].Value = DBNull.Value;
                    parms[17].Value = DBNull.Value;
                    parms[18].Value = DBNull.Value;
                    parms[19].Value = DBNull.Value;
                    parms[20].Value = DBNull.Value;
                    parms[21].Value = DBNull.Value;
                    parms[22].Value = ThongTinChiTiet.TrangThaiTienDo ?? Convert.DBNull;
                    parms[23].Value = ThongTinChiTiet.GhiChu ?? Convert.DBNull;
                }
                else if (ThongTinChiTiet.LoaiThongTin == (int)EnumLoaiThongTin.KinhPhi)
                {
                    parms[0].Value = ThongTinChiTiet.DeTaiID;
                    parms[1].Value = DBNull.Value;
                    parms[2].Value = DBNull.Value;
                    parms[3].Value = DBNull.Value;
                    parms[4].Value = ThongTinChiTiet.TongKinhPhiDuocDuyet ?? Convert.DBNull;
                    parms[5].Value = ThongTinChiTiet.TienDoCap ?? Convert.DBNull;
                    parms[6].Value = ThongTinChiTiet.TienDoQuyetToan ?? Convert.DBNull;
                    parms[7].Value = DBNull.Value;
                    parms[8].Value = DBNull.Value;
                    parms[9].Value = DBNull.Value;
                    parms[10].Value = DBNull.Value;
                    parms[11].Value = DBNull.Value;
                    parms[12].Value = (int)EnumLoaiThongTin.KinhPhi;
                    parms[13].Value = ThongTinChiTiet.ChiTietDeTaiID ?? Convert.DBNull;
                    parms[14].Value = ThongTinChiTiet.NguoiTaoID ?? Convert.DBNull;
                    parms[15].Value = DBNull.Value;
                    parms[16].Value = DBNull.Value;
                    parms[17].Value = DBNull.Value;
                    parms[18].Value = DBNull.Value;
                    parms[19].Value = DBNull.Value;
                    parms[20].Value = DBNull.Value;
                    parms[21].Value = DBNull.Value;
                    parms[22].Value = DBNull.Value;
                    parms[23].Value = DBNull.Value;
                }
                else if (ThongTinChiTiet.LoaiThongTin == (int)EnumLoaiThongTin.SanPhamDeTai)
                {
                    parms[0].Value = ThongTinChiTiet.DeTaiID;
                    parms[1].Value = DBNull.Value;
                    parms[2].Value = DBNull.Value;
                    parms[3].Value = DBNull.Value;
                    parms[4].Value = DBNull.Value;
                    parms[5].Value = DBNull.Value;
                    parms[6].Value = DBNull.Value;
                    parms[7].Value = ThongTinChiTiet.NoiDungDaLam ?? Convert.DBNull;
                    parms[8].Value = ThongTinChiTiet.SanPham ?? Convert.DBNull;
                    parms[9].Value = DBNull.Value;
                    parms[10].Value = DBNull.Value;
                    parms[11].Value = DBNull.Value;
                    parms[12].Value = (int)EnumLoaiThongTin.SanPhamDeTai;
                    parms[13].Value = ThongTinChiTiet.ChiTietDeTaiID ?? Convert.DBNull;
                    parms[14].Value = ThongTinChiTiet.NguoiTaoID ?? Convert.DBNull;
                    parms[15].Value = DBNull.Value;
                    parms[16].Value = DBNull.Value;
                    parms[17].Value = DBNull.Value;
                    parms[18].Value = DBNull.Value;
                    parms[19].Value = DBNull.Value;
                    parms[20].Value = DBNull.Value;
                    parms[21].Value = DBNull.Value;
                    parms[22].Value = DBNull.Value;
                    parms[23].Value = DBNull.Value;
                }
                else if (ThongTinChiTiet.LoaiThongTin == (int)EnumLoaiThongTin.KetQuaChuyenGiao)
                {
                    parms[0].Value = ThongTinChiTiet.DeTaiID;
                    parms[1].Value = DBNull.Value;
                    parms[2].Value = DBNull.Value;
                    parms[3].Value = DBNull.Value;
                    parms[4].Value = DBNull.Value;
                    parms[5].Value = DBNull.Value;
                    parms[6].Value = DBNull.Value;
                    parms[7].Value = DBNull.Value;
                    parms[8].Value = DBNull.Value;
                    parms[9].Value = ThongTinChiTiet.NoiDungChuyenGiao ?? Convert.DBNull;
                    parms[10].Value = DBNull.Value;
                    parms[11].Value = DBNull.Value;
                    parms[12].Value = (int)EnumLoaiThongTin.KetQuaChuyenGiao;
                    parms[13].Value = ThongTinChiTiet.ChiTietDeTaiID ?? Convert.DBNull;
                    parms[14].Value = ThongTinChiTiet.NguoiTaoID ?? Convert.DBNull;
                    parms[15].Value = DBNull.Value;
                    parms[16].Value = DBNull.Value;
                    parms[17].Value = DBNull.Value;
                    parms[18].Value = DBNull.Value;
                    parms[19].Value = DBNull.Value;
                    parms[20].Value = DBNull.Value;
                    parms[21].Value = DBNull.Value;
                    parms[22].Value = DBNull.Value;
                    parms[23].Value = DBNull.Value;
                }
                else if (ThongTinChiTiet.LoaiThongTin == (int)EnumLoaiThongTin.DanhGiaGiaiDoan)
                {
                    parms[0].Value = ThongTinChiTiet.DeTaiID;
                    parms[1].Value = DBNull.Value;
                    parms[2].Value = DBNull.Value;
                    parms[3].Value = DBNull.Value;
                    parms[4].Value = DBNull.Value;
                    parms[5].Value = DBNull.Value;
                    parms[6].Value = DBNull.Value;
                    parms[7].Value = DBNull.Value;
                    parms[8].Value = DBNull.Value;
                    parms[9].Value = DBNull.Value;
                    parms[10].Value = ThongTinChiTiet.ThoiGianThucHien ?? Convert.DBNull;
                    parms[11].Value = ThongTinChiTiet.NoiDungDanhGia ?? Convert.DBNull;
                    parms[12].Value = (int)EnumLoaiThongTin.DanhGiaGiaiDoan;
                    parms[13].Value = ThongTinChiTiet.ChiTietDeTaiID ?? Convert.DBNull;
                    parms[14].Value = ThongTinChiTiet.NguoiTaoID ?? Convert.DBNull;
                    parms[15].Value = ThongTinChiTiet.KetQuaThucHien ?? Convert.DBNull;
                    parms[16].Value = DBNull.Value;
                    parms[17].Value = DBNull.Value;
                    parms[18].Value = DBNull.Value;
                    parms[19].Value = DBNull.Value;
                    parms[20].Value = DBNull.Value;
                    parms[21].Value = DBNull.Value;
                    parms[22].Value = DBNull.Value;
                    parms[23].Value = DBNull.Value;
                }
                else if (ThongTinChiTiet.LoaiThongTin == (int)EnumLoaiThongTin.KetQuaDanhGia)
                {
                    parms[0].Value = ThongTinChiTiet.DeTaiID;
                    parms[1].Value = DBNull.Value;
                    parms[2].Value = DBNull.Value;
                    parms[3].Value = DBNull.Value;
                    parms[4].Value = DBNull.Value;
                    parms[5].Value = DBNull.Value;
                    parms[6].Value = DBNull.Value;
                    parms[7].Value = DBNull.Value;
                    parms[8].Value = DBNull.Value;
                    parms[9].Value = DBNull.Value;
                    parms[10].Value = ThongTinChiTiet.ThoiGianThucHien ?? Convert.DBNull;
                    parms[11].Value = ThongTinChiTiet.NoiDungDanhGia ?? Convert.DBNull;
                    parms[12].Value = (int)EnumLoaiThongTin.KetQuaDanhGia;
                    parms[13].Value = ThongTinChiTiet.ChiTietDeTaiID ?? Convert.DBNull;
                    parms[14].Value = ThongTinChiTiet.NguoiTaoID ?? Convert.DBNull;
                    parms[15].Value = DBNull.Value;
                    parms[16].Value = ThongTinChiTiet.LoaiKetQua ?? Convert.DBNull;
                    parms[17].Value = ThongTinChiTiet.XepLoai ?? Convert.DBNull;
                    parms[18].Value = ThongTinChiTiet.XepLoaiKhac ?? Convert.DBNull;
                    parms[19].Value = ThongTinChiTiet.LoaiNghiemThu ?? Convert.DBNull;
                    parms[20].Value = ThongTinChiTiet.NgayNghiemThu ?? Convert.DBNull;
                    parms[21].Value = ThongTinChiTiet.QuyetDinh ?? Convert.DBNull;
                    parms[22].Value = DBNull.Value;
                    parms[23].Value = DBNull.Value;
                }
                else if (ThongTinChiTiet.LoaiThongTin == (int)EnumLoaiThongTin.BaiBaoTapChi)
                {
                    parms[0].Value = ThongTinChiTiet.DeTaiID;
                    parms[12].Value = (int)EnumLoaiThongTin.BaiBaoTapChi;
                    parms[13].Value = ThongTinChiTiet.ChiTietDeTaiID ?? Convert.DBNull;
                    parms[14].Value = ThongTinChiTiet.NguoiTaoID;
                    parms[24].Value = ThongTinChiTiet.CoQuanID;
                    parms[25].Value = ThongTinChiTiet.TieuDe ?? Convert.DBNull;
                    parms[26].Value = ThongTinChiTiet.TacGia ?? Convert.DBNull;
                    parms[27].Value = ThongTinChiTiet.TenTapChiSachHoiThao ?? Convert.DBNull;
                    parms[28].Value = ThongTinChiTiet.So ?? Convert.DBNull;
                    parms[29].Value = ThongTinChiTiet.Trang ?? Convert.DBNull;
                    parms[30].Value = ThongTinChiTiet.NhaXuatBan ?? Convert.DBNull;
                    parms[31].Value = ThongTinChiTiet.KhoangThoiGian ?? Convert.DBNull;
                    parms[32].Value = ThongTinChiTiet.LoaiBaiBao ?? Convert.DBNull;
                    parms[33].Value = ThongTinChiTiet.ISSN ?? Convert.DBNull;
                    parms[34].Value = ThongTinChiTiet.NhiemVu ?? Convert.DBNull;
                    parms[35].Value = ThongTinChiTiet.LoaiNhiemVu ?? Convert.DBNull;
                    parms[36].Value = ThongTinChiTiet.TenHoiThao ?? Convert.DBNull;
                    parms[37].Value = ThongTinChiTiet.Tap ?? Convert.DBNull;
                    parms[38].Value = ThongTinChiTiet.NamDangTai ?? Convert.DBNull;
                    parms[39].Value = ThongTinChiTiet.LinkBaiBao ?? Convert.DBNull;
                    parms[40].Value = ThongTinChiTiet.LinhVucNganhKhoaHoc ?? Convert.DBNull;
                    parms[41].Value = ThongTinChiTiet.HeSoAnhHuong ?? Convert.DBNull;
                    parms[42].Value = ThongTinChiTiet.ChiSo ?? Convert.DBNull;
                    parms[43].Value = ThongTinChiTiet.RankSCIMAG ?? Convert.DBNull;
                    parms[44].Value = ThongTinChiTiet.DiemTapChi ?? Convert.DBNull;
                    parms[45].Value = ThongTinChiTiet.CapHoiThao ?? Convert.DBNull;
                    parms[46].Value = ThongTinChiTiet.NgayHoiThao ?? Convert.DBNull;
                    parms[47].Value = ThongTinChiTiet.DiaDiemToChuc ?? Convert.DBNull;
                }
                else if (ThongTinChiTiet.LoaiThongTin == (int)EnumLoaiThongTin.KetQuaNghienCuu)
                {
                    parms[0].Value = ThongTinChiTiet.DeTaiID;
                    parms[12].Value = (int)EnumLoaiThongTin.KetQuaNghienCuu;
                    parms[13].Value = ThongTinChiTiet.ChiTietDeTaiID ?? Convert.DBNull;
                    parms[14].Value = ThongTinChiTiet.NguoiTaoID;
                    parms[23].Value = ThongTinChiTiet.GhiChu ?? Convert.DBNull;
                    parms[24].Value = ThongTinChiTiet.CoQuanID;
                    parms[25].Value = ThongTinChiTiet.TieuDe ?? Convert.DBNull;
                    //parms[31].Value = ThongTinChiTiet.NamXuatBan ?? Convert.DBNull;
                    parms[34].Value = ThongTinChiTiet.NhiemVu ?? Convert.DBNull;
                    parms[35].Value = ThongTinChiTiet.LoaiNhiemVu ?? Convert.DBNull;
                    parms[55].Value = ThongTinChiTiet.NamXuatBan ?? Convert.DBNull;
                }
                else if (ThongTinChiTiet.LoaiThongTin == (int)EnumLoaiThongTin.SachChuyenKhao)
                {
                    parms[0].Value = ThongTinChiTiet.DeTaiID;
                    parms[12].Value = (int)EnumLoaiThongTin.SachChuyenKhao;
                    parms[13].Value = ThongTinChiTiet.ChiTietDeTaiID ?? Convert.DBNull;
                    parms[14].Value = ThongTinChiTiet.NguoiTaoID;
                    parms[24].Value = ThongTinChiTiet.CoQuanID;
                    parms[25].Value = ThongTinChiTiet.TieuDe ?? Convert.DBNull;
                    parms[26].Value = ThongTinChiTiet.TacGia ?? Convert.DBNull;
                    parms[27].Value = ThongTinChiTiet.TenTapChiSachHoiThao ?? Convert.DBNull;
                    parms[28].Value = ThongTinChiTiet.So ?? Convert.DBNull;
                    parms[29].Value = ThongTinChiTiet.Trang ?? Convert.DBNull;
                    parms[30].Value = ThongTinChiTiet.NhaXuatBan ?? Convert.DBNull;
                    parms[31].Value = ThongTinChiTiet.KhoangThoiGian ?? Convert.DBNull;
                    parms[33].Value = ThongTinChiTiet.ISSN ?? Convert.DBNull;
                    parms[34].Value = ThongTinChiTiet.NhiemVu ?? Convert.DBNull;
                    parms[35].Value = ThongTinChiTiet.LoaiNhiemVu ?? Convert.DBNull;
                    parms[53].Value = ThongTinChiTiet.ChuBienID ?? Convert.DBNull;
                    parms[54].Value = ThongTinChiTiet.CoQuanChuBienID ?? Convert.DBNull;
                    parms[55].Value = ThongTinChiTiet.NamXuatBan ?? Convert.DBNull;
                }
                else if (ThongTinChiTiet.LoaiThongTin == (int)EnumLoaiThongTin.SanPhamDaoTao)
                {
                    parms[0].Value = ThongTinChiTiet.DeTaiID;
                    parms[12].Value = (int)EnumLoaiThongTin.SanPhamDaoTao;
                    parms[13].Value = ThongTinChiTiet.ChiTietDeTaiID ?? Convert.DBNull;
                    parms[14].Value = ThongTinChiTiet.NguoiTaoID;
                    parms[24].Value = ThongTinChiTiet.CoQuanID;
                    parms[31].Value = ThongTinChiTiet.KhoangThoiGian ?? Convert.DBNull;

                    parms[34].Value = ThongTinChiTiet.NhiemVu ?? Convert.DBNull;
                    parms[35].Value = ThongTinChiTiet.LoaiNhiemVu ?? Convert.DBNull;

                    parms[45].Value = ThongTinChiTiet.CapHoiThao ?? Convert.DBNull;

                    parms[48].Value = ThongTinChiTiet.LoaiDaoTao ?? Convert.DBNull;
                    parms[49].Value = ThongTinChiTiet.TenHocVien ?? Convert.DBNull;
                    parms[50].Value = ThongTinChiTiet.TenLuanVan ?? Convert.DBNull;
                    parms[51].Value = ThongTinChiTiet.NguoiHuongDan ?? Convert.DBNull;
                    parms[52].Value = ThongTinChiTiet.NamBaoVe ?? Convert.DBNull;

                    parms[56].Value = ThongTinChiTiet.CoSoDaoTao ?? Convert.DBNull;
                }

                if (ThongTinChiTiet.ChiTietDeTaiID == null)
                {
                    parms[13].Direction = ParameterDirection.Output;
                    parms[13].Size = 8;
                }
                parms[14].Value = ThongTinChiTiet.NguoiTaoID ?? Convert.DBNull;

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            if (ThongTinChiTiet.ChiTietDeTaiID > 0)
                            {
                                Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_ThongTinChiTietDeTai_Update_New", parms);
                                Result.Message = ConstantLogMessage.Alert_Update_Success("Chi tiết Đề tài");
                                Result.Data = ThongTinChiTiet.ChiTietDeTaiID;
                                //xóa tác giả cũ
                                SqlParameter[] parms_del = new SqlParameter[]{
                                    new SqlParameter("ChiTietDeTaiID", SqlDbType.Int),
                                };
                                parms_del[0].Value = ThongTinChiTiet.ChiTietDeTaiID;
                                SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_TacGia_DeTai_Delete", parms_del);
                                //thêm tác giả
                                if (ThongTinChiTiet.ListTacGia != null && ThongTinChiTiet.ListTacGia.Count > 0)
                                {
                                    foreach (var item in ThongTinChiTiet.ListTacGia)
                                    {
                                        SqlParameter[] parms_tg = new SqlParameter[]{
                                            new SqlParameter("ChiTietDeTaiID", SqlDbType.Int),
                                            new SqlParameter("CanBoID", SqlDbType.Int),
                                            new SqlParameter("CoQuanID", SqlDbType.Int),
                                        };
                                        parms_tg[0].Value = ThongTinChiTiet.ChiTietDeTaiID;
                                        parms_tg[1].Value = item.CanBoID ?? Convert.DBNull;
                                        parms_tg[2].Value = item.CoQuanID ?? Convert.DBNull;

                                        SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_TacGia_DeTai_Insert", parms_tg);
                                    }
                                }
                            }
                            else
                            {
                                Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_ThongTinChiTietDeTai_Insert_New", parms);
                                Result.Message = ConstantLogMessage.Alert_Insert_Success("Chi tiết Đề tài");
                                Result.Data = Utils.ConvertToInt32(parms[13].Value, 0);
                                //thêm tác giả
                                if (ThongTinChiTiet.ListTacGia != null && ThongTinChiTiet.ListTacGia.Count > 0)
                                {
                                    foreach (var item in ThongTinChiTiet.ListTacGia)
                                    {
                                        SqlParameter[] parms_tg = new SqlParameter[]{
                                            new SqlParameter("ChiTietDeTaiID", SqlDbType.Int),
                                            new SqlParameter("CanBoID", SqlDbType.Int),
                                            new SqlParameter("CoQuanID", SqlDbType.Int),
                                        };
                                        parms_tg[0].Value = Result.Data;
                                        parms_tg[1].Value = item.CanBoID ?? Convert.DBNull;
                                        parms_tg[2].Value = item.CoQuanID ?? Convert.DBNull;

                                        SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_TacGia_DeTai_Insert", parms_tg);
                                    }
                                }
                            }
                            trans.Commit();
                            Result.Status = 1;
                        }
                        catch
                        {
                            Result.Status = -1;
                            Result.Message = ConstantLogMessage.API_Error_System;
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                throw;
            }
            return Result;
        }

        /// <summary>
        /// Xóa thông tin chi tiết đề tài theo ID
        /// </summary>
        /// <param name="ThongTinChiTiet"></param>
        /// <returns></returns>
        public BaseResultModel Delete_ThongTinChiTiet(ThongTinChiTietDeTaiModel ThongTinChiTiet)
        {
            var Result = new BaseResultModel();
            SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@ChiTietDeTaiID",SqlDbType.Int)
                };
            parameters[0].Value = ThongTinChiTiet.ChiTietDeTaiID;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, @"v1_ThongTinChiTietDeTai_Delete", parameters);
                        Result.Message = ConstantLogMessage.Alert_Delete_Success("Thông tin chi tiết đề tài");
                        Result.Status = 1;
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        Result.Status = -1;
                        Result.Message = ex.Message;
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// Chỉnh sửa kết quả nghiên cứu
        /// </summary>
        /// <param name="KetQuaNghienCuu"></param>
        /// <returns></returns>
        public BaseResultModel Edit_KetQuaNghienCuu(KetQuaNghienCuuModel KetQuaNghienCuu)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parms = new SqlParameter[]{
                    new SqlParameter("KetQuaNghienCuuID", SqlDbType.Int),
                    new SqlParameter("DeTaiID", SqlDbType.Int),
                    new SqlParameter("LoaiKetQua", SqlDbType.Int),
                    new SqlParameter("NhiemVu", SqlDbType.Int),
                    new SqlParameter("ThoiGian", SqlDbType.NVarChar),
                    new SqlParameter("TacGia", SqlDbType.NVarChar),
                    new SqlParameter("TieuDe", SqlDbType.NVarChar),
                    new SqlParameter("TenTapChiSachHoiThao", SqlDbType.NVarChar),
                    new SqlParameter("So", SqlDbType.NVarChar),
                    new SqlParameter("Trang", SqlDbType.NVarChar),
                    new SqlParameter("NhaXuatBan", SqlDbType.NVarChar),
                    new SqlParameter("NguoiTaoID", SqlDbType.Int),
                };
                parms[0].Value = KetQuaNghienCuu.KetQuaNghienCuuID;
                parms[1].Value = KetQuaNghienCuu.DeTaiID;
                parms[2].Value = KetQuaNghienCuu.LoaiKetQua ?? Convert.DBNull;
                parms[3].Value = KetQuaNghienCuu.NhiemVu ?? Convert.DBNull;
                parms[4].Value = KetQuaNghienCuu.ThoiGian;
                parms[5].Value = KetQuaNghienCuu.TacGia;
                parms[6].Value = KetQuaNghienCuu.TieuDe;
                parms[7].Value = KetQuaNghienCuu.TenTapChiSachHoiThao;
                parms[8].Value = KetQuaNghienCuu.So;
                parms[9].Value = KetQuaNghienCuu.Trang;
                parms[10].Value = KetQuaNghienCuu.NhaXuatBan;
                parms[11].Value = KetQuaNghienCuu.NguoiTaoID;

                if (KetQuaNghienCuu.KetQuaNghienCuuID == null)
                {
                    parms[0].Direction = ParameterDirection.Output;
                    parms[0].Size = 8;
                }

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            if (KetQuaNghienCuu.KetQuaNghienCuuID > 0)
                            {
                                Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_KetQuaNghienCuu_Update", parms);
                                Result.Message = ConstantLogMessage.Alert_Update_Success("kết quả nghiên cứu");
                                Result.Data = KetQuaNghienCuu.KetQuaNghienCuuID;
                            }
                            else
                            {
                                Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_KetQuaNghienCuu_Insert", parms);
                                Result.Message = ConstantLogMessage.Alert_Insert_Success("kết quả nghiên cứu");
                                Result.Data = Utils.ConvertToInt32(parms[0].Value, 0);
                            }
                            trans.Commit();
                            Result.Status = 1;
                        }
                        catch
                        {
                            Result.Status = -1;
                            Result.Message = ConstantLogMessage.API_Error_System;
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                throw;
            }
            return Result;
        }

        /// <summary>
        /// Xóa kết quả nghiên cứu
        /// </summary>
        /// <param name="KetQuaNghienCuu"></param>
        /// <returns></returns>
        public BaseResultModel Delete_KetQuaNghienCuu(KetQuaNghienCuuModel KetQuaNghienCuu)
        {
            var Result = new BaseResultModel();
            SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@KetQuaNghienCuuID",SqlDbType.Int)
                };
            parameters[0].Value = KetQuaNghienCuu.KetQuaNghienCuuID;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, @"v1_KetQuaNghienCuu_Delete", parameters);
                        Result.Message = ConstantLogMessage.Alert_Delete_Success("kết quả nghiên cứu");
                        Result.Status = 1;
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        Result.Status = -1;
                        Result.Message = ex.Message;
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// Lấy kết quả nghiên cứu trong đề tài
        /// </summary>
        /// <param name="DeTaiID"></param>
        /// <returns></returns>
        public List<KetQuaNghienCuuModel> KetQuaNghienCuu_GetByID(int DeTaiID)
        {
            List<KetQuaNghienCuuModel> Result = new List<KetQuaNghienCuuModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@DeTaiID",SqlDbType.Int)
            };
            parameters[0].Value = DeTaiID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_KetQuaNghienCuu_GetByID", parameters))
                {
                    while (dr.Read())
                    {
                        KetQuaNghienCuuModel info = new KetQuaNghienCuuModel();
                        info.NguoiTaoID = Utils.ConvertToInt32(dr["NguoiTaoID"], 0);
                        info.DeTaiID = Utils.ConvertToInt32(dr["DeTaiID"], 0);
                        info.KetQuaNghienCuuID = Utils.ConvertToInt32(dr["KetQuaNghienCuuID"], 0);
                        info.LoaiKetQua = Utils.ConvertToInt32(dr["LoaiKetQua"], 0);
                        info.NhiemVu = Utils.ConvertToInt32(dr["NhiemVu"], 0);
                        info.ThoiGian = Utils.ConvertToString(dr["ThoiGian"], string.Empty);
                        info.TacGia = Utils.ConvertToString(dr["TacGia"], string.Empty);
                        info.TieuDe = Utils.ConvertToString(dr["TieuDe"], string.Empty);
                        info.TenTapChiSachHoiThao = Utils.ConvertToString(dr["TenTapChiSachHoiThao"], string.Empty);
                        info.So = Utils.ConvertToString(dr["So"], string.Empty);
                        info.Trang = Utils.ConvertToString(dr["Trang"], string.Empty);
                        info.NhaXuatBan = Utils.ConvertToString(dr["NhaXuatBan"], string.Empty);
                        info.FileDinhKem = new List<FileDinhKemModel>();
                        Result.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Result;
        }

        /// <summary>
        /// Xóa đề tài
        /// </summary>
        /// <param name="DeTaiModel"></param>
        /// <returns></returns>
        public BaseResultModel Delete(DeTaiModel DeTaiModel)
        {
            var Result = new BaseResultModel();
            SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@DeTaiID",SqlDbType.Int)
                };
            parameters[0].Value = DeTaiModel.DeTaiID;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, @"v1_DeTai_Delete", parameters);
                        Result.Message = ConstantLogMessage.Alert_Delete_Success("đề tài");
                        Result.Status = 1;
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        Result.Status = -1;
                        Result.Message = ex.Message;
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return Result;
        }

        public List<FileDinhKemModel> GetAllFile(int DeTaiID)
        {
            List<FileDinhKemModel> Result = new List<FileDinhKemModel>();

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@DeTaiID",SqlDbType.Int),
            };
            parameters[0].Value = DeTaiID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_DeTai_GetAllFile", parameters))
                {
                    while (dr.Read())
                    {
                        FileDinhKemModel info = new FileDinhKemModel();
                        info.NghiepVuID = Utils.ConvertToInt32(dr["NghiepVuID"], 0);
                        info.FileDinhKemID = Utils.ConvertToInt32(dr["FileDinhKemID"], 0);
                        info.TenFileGoc = Utils.ConvertToString(dr["TenFileGoc"], string.Empty);
                        info.TenFileHeThong = Utils.ConvertToString(dr["TenFileHeThong"], string.Empty);
                        info.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        info.LoaiFile = Utils.ConvertToInt32(dr["LoaiFile"], 0);
                        info.NguoiTaoID = Utils.ConvertToInt32(dr["NguoiTaoID"], 0);
                        info.NgayTao = Utils.ConvertToDateTime(dr["NgayTao"], DateTime.Now);
                        info.NoiDung = Utils.ConvertToString(dr["NoiDung"], string.Empty);
                        Result.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        /// <summary>
        /// Dữ liệu nghiên cứu khoa học
        /// </summary>
        /// <returns></returns>
        public List<DuLieuNghienCuuKhoaHocModel> DuLieuNghienCuuKhoaHoc(List<int> StaffId, int? Year)
        {
            List<CTDuLieuNghienCuuKhoaHocModel> Result = new List<CTDuLieuNghienCuuKhoaHocModel>();

            var pList = new SqlParameter("@ListCanBoID", SqlDbType.Structured);
            pList.TypeName = "dbo.list_ID";
            var tbCanBoID = new DataTable();
            tbCanBoID.Columns.Add("CanBoID", typeof(string));
            StaffId.ForEach(x => tbCanBoID.Rows.Add(x));

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Year",SqlDbType.Int),
                pList
            };
            parameters[0].Value = Year ?? 0;
            parameters[1].Value = tbCanBoID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_DuLieuNghienCuuKhoaHoc", parameters))
                {
                    while (dr.Read())
                    {
                        CTDuLieuNghienCuuKhoaHocModel info = new CTDuLieuNghienCuuKhoaHocModel();
                        info.Id = Utils.ConvertToInt32(dr["Id"], 0);
                        info.YearId = Utils.ConvertToInt32NullAble(dr["YearId"], null);
                        info.StaffId = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        info.ConversionId = Utils.ConvertToInt32(dr["ConversionId"], 0);
                        info.Name = Utils.ConvertToString(dr["Name"], string.Empty);
                        info.Quantity = Utils.ConvertToInt32(dr["Quantity"], 0);
                        info.Members = Utils.ConvertToInt32(dr["Members"], 0);
                        info.StartYear = Utils.ConvertToInt32(dr["StartYear"], 0);
                        info.Desc = "";
                        string MucTieu = Utils.ConvertToString(dr["MucTieu"], string.Empty);
                        if (MucTieu != string.Empty)
                        {
                            info.Desc += "Mục tiêu: " + MucTieu + ". ";
                        }
                        string CacNoiDungChinh = Utils.ConvertToString(dr["CacNoiDungChinh"], string.Empty);
                        if (MucTieu != string.Empty)
                        {
                            info.Desc += "Các nội dung chính: " + CacNoiDungChinh + ". ";
                        }
                        string SanPhamDangKy = Utils.ConvertToString(dr["SanPhamDangKy"], string.Empty);
                        if (MucTieu != string.Empty)
                        {
                            info.Desc += "Sản phẩm đăng ký: " + SanPhamDangKy + ". ";
                        }
                        string KhaNangUngDung = Utils.ConvertToString(dr["KhaNangUngDung"], string.Empty);
                        if (MucTieu != string.Empty)
                        {
                            info.Desc += "Khả năng ứng dụng: " + KhaNangUngDung + ". ";
                        }
                        int NamKetThuc = Utils.ConvertToInt32(dr["NamKetThuc"], 0);
                        info.WorkTime = NamKetThuc - info.StartYear;

                        info.NghiepVuID = Utils.ConvertToInt32(dr["NghiepVuID"], 0);
                        info.DeTaiID = Utils.ConvertToInt32(dr["DeTaiID"], 0);
                        info.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        if (info.ConversionId > 0)
                            Result.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            List<DuLieuNghienCuuKhoaHocModel> listDuLieuNghienCuu = new List<DuLieuNghienCuuKhoaHocModel>();
            listDuLieuNghienCuu = Result.GroupBy(p => p.DeTaiID)
                    .Select(g => new DuLieuNghienCuuKhoaHocModel
                    {
                        Id = g.FirstOrDefault().Id,
                        //YearId = g.FirstOrDefault().YearId,
                        StaffId = g.FirstOrDefault().StaffId,
                        ConversionId = g.FirstOrDefault().ConversionId,
                        Name = g.FirstOrDefault().Name,
                        Quantity = g.FirstOrDefault().Quantity,
                        Members = g.FirstOrDefault().Members,
                        StartYear = g.FirstOrDefault().StartYear,
                        Desc = g.FirstOrDefault().Desc,
                        WorkTime = g.FirstOrDefault().WorkTime,
                        Attach = Result.Where(x => x.DeTaiID == g.Key && x.FileUrl != null && x.FileUrl.Length > 0)
                                        .Select(y => y.FileUrl).ToList()
                    }
                    ).ToList();
            // hoạt động khoa học trong lý lịch khoa học
            var HoatDongKhoaHoc = HoatDongKhoaHocKhac(StaffId, Year);
            if (HoatDongKhoaHoc.Count > 0) listDuLieuNghienCuu.AddRange(HoatDongKhoaHoc);
            //bài báo, sách chuyên khảo - lấy trên controller


            return listDuLieuNghienCuu;
        }

        /// <summary>
        /// hoạt động khoa học khác - trong bảng HoatDongKhoaHoc - khai trên mục hoạt động khoa học khác trong lý lịch khoa học
        /// </summary>
        /// <param name="listCanBoID"></param>
        /// <param name="Year"></param>
        /// <returns></returns>
        public List<DuLieuNghienCuuKhoaHocModel> HoatDongKhoaHocKhac(List<int> listCanBoID, int? Year)
        {
            List<CTDuLieuNghienCuuKhoaHocModel> Result = new List<CTDuLieuNghienCuuKhoaHocModel>();

            var pList = new SqlParameter("@ListCanBoID", SqlDbType.Structured);
            pList.TypeName = "dbo.list_ID";
            var tbCanBoID = new DataTable();
            tbCanBoID.Columns.Add("CanBoID", typeof(string));
            listCanBoID.ForEach(x => tbCanBoID.Rows.Add(x));

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Year",SqlDbType.Int),
                pList
            };
            parameters[0].Value = Year ?? 0;
            parameters[1].Value = tbCanBoID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_HoatDongKhoaHoc", parameters))
                {
                    while (dr.Read())
                    {
                        CTDuLieuNghienCuuKhoaHocModel info = new CTDuLieuNghienCuuKhoaHocModel();
                        //info.Id = Utils.ConvertToInt32(dr["Id"], 0);
                        //info.YearId = Utils.ConvertToInt32NullAble(dr["YearId"], null);
                        info.StaffId = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        info.ConversionId = Utils.ConvertToInt32(dr["NhiemVu"], 0);
                        info.Name = Utils.ConvertToString(dr["Name"], string.Empty);
                        //info.Quantity = Utils.ConvertToInt32(dr["Quantity"], 0);
                        //info.Members = Utils.ConvertToInt32(dr["Members"], 0);
                        //info.StartYear = Utils.ConvertToInt32(dr["StartYear"], 0);
                        info.Desc = Utils.ConvertToString(dr["Description"], string.Empty);

                        //int NamKetThuc = Utils.ConvertToInt32(dr["NamKetThuc"], 0);
                        //info.WorkTime = NamKetThuc - info.StartYear;

                        info.NghiepVuID = Utils.ConvertToInt32(dr["NghiepVuID"], 0);
                        info.HoatDongKhoaHocID = Utils.ConvertToInt32(dr["HoatDongKhoaHocID"], 0);
                        info.CTNhaKhoaHocID = Utils.ConvertToInt32(dr["CTNhaKhoaHocID"], 0);
                        info.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        info.StartYear = Utils.ConvertToInt32(dr["NamThucHien"], 0);

                        Result.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            List<DuLieuNghienCuuKhoaHocModel> listDuLieuNghienCuu = new List<DuLieuNghienCuuKhoaHocModel>();
            listDuLieuNghienCuu = Result.GroupBy(p => p.HoatDongKhoaHocID)
                    .Select(g => new DuLieuNghienCuuKhoaHocModel
                    {
                        Id = g.FirstOrDefault().Id,
                        //YearId = g.FirstOrDefault().YearId,
                        StaffId = g.FirstOrDefault().StaffId,
                        ConversionId = g.FirstOrDefault().ConversionId,
                        Name = g.FirstOrDefault().Name,
                        Quantity = g.FirstOrDefault().Quantity,
                        Members = g.FirstOrDefault().Members,
                        StartYear = g.FirstOrDefault().StartYear,
                        Desc = g.FirstOrDefault().Desc,
                        WorkTime = g.FirstOrDefault().WorkTime,
                        Attach = g.Where(x => x.FileUrl != null && x.FileUrl.Length > 0).Select(y => y.FileUrl).ToList()
                        //Result.Where(x => x.DeTaiID == g.Key && x.FileUrl != null && x.FileUrl.Length > 0).Select(y => y.FileUrl).ToList()
                    }
                    ).ToList();

            return listDuLieuNghienCuu;
        }

        /// <summary>
        /// Lấy kết quả nghiên cứu đã công bố hoặc đăng ký khác trong lý lịch khoa học
        /// </summary>
        /// <param name="DeTaiID"></param>
        /// <returns></returns>
        public List<KetQuaNghienCuuModel> GetKetQuaNghienCuuByDeTaiID(int DeTaiID)
        {
            List<KetQuaNghienCuuModel> Result = new List<KetQuaNghienCuuModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@DeTaiID",SqlDbType.Int)
            };
            parameters[0].Value = DeTaiID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_LyLichKhoaHoc_KQNghienCuu_DeTaiID", parameters))
                {
                    while (dr.Read())
                    {
                        KetQuaNghienCuuModel info = new KetQuaNghienCuuModel();
                        info.DeTaiID = DeTaiID;
                        info.KetQuaNghienCuuID = 0;
                        info.LoaiKetQua = 0;
                        info.NhiemVu = 0;
                        info.ThoiGian = Utils.ConvertToString(dr["KhoangThoiGian"], string.Empty);
                        info.TacGia = Utils.ConvertToString(dr["TacGia"], string.Empty);
                        info.TieuDe = Utils.ConvertToString(dr["TieuDe"], string.Empty);
                        info.TenTapChiSachHoiThao = Utils.ConvertToString(dr["TenTapChiSachHoiThao"], string.Empty);
                        info.So = Utils.ConvertToString(dr["So"], string.Empty);
                        info.Trang = Utils.ConvertToString(dr["Trang"], string.Empty);
                        info.NhaXuatBan = Utils.ConvertToString(dr["NhaXuatBan"], string.Empty);
                        info.FileDinhKem = new List<FileDinhKemModel>();
                        info.Disable = true;
                        Result.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Result;
        }
        public List<DeTaiModel> GetDeTaiByCanBoID(int CanBoID)
        {
            List<DeTaiModel> Result = new List<DeTaiModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CanBoID",SqlDbType.Int)
            };
            parameters[0].Value = CanBoID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_DeTai_GetByCanBoID", parameters))
                {
                    while (dr.Read())
                    {
                        DeTaiModel info = new DeTaiModel();
                        info.DeTaiID = Utils.ConvertToInt32(dr["DeTaiID"], 0);
                        info.TenDeTai = Utils.ConvertToString(dr["TenDeTai"], string.Empty);
                        info.MaDeTai = Utils.ConvertToString(dr["MaDeTai"], string.Empty);
                        info.ChuNhiemDeTaiID = Utils.ConvertToInt32(dr["ChuNhiemDeTaiID"], 0);
                        Result.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Result;
        }


        /// <summary>
        /// Số lượng đề tài đang thực hiện (năm bắt đầu thực hiện = năm hiện tại) của từng cơ quan
        /// </summary>
        /// <returns></returns>
        public List<DanhMucCoQuanDonViModel> SoLuongDeTaiTheoCoQuan()
        {
            List<DanhMucCoQuanDonViModel> Result = new List<DanhMucCoQuanDonViModel>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_DeTai_SLDangThucHien"))
                {
                    while (dr.Read())
                    {
                        DanhMucCoQuanDonViModel info = new DanhMucCoQuanDonViModel();
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanChuNhiemID"], 0);
                        info.SoLuongDeTai = Utils.ConvertToInt32(dr["SoLuongDeTai"], 0);
                        Result.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Result;
        }


        /// <summary>
        /// lấy dữ liệu Bài báo trên tạp chí KHCN, Sách chuyên khảo đã xuất bản, Sản phẩm đào tạo, Kết quả nghiên cứu đã công bố hoặc đăng ký khác,
        /// trả về cho ht quản lý giờ giảng
        /// </summary>
        /// <param name="StaffId"></param>
        /// <param name="Year"></param>
        /// <returns></returns>
        public List<ThongTinCTNhaKhoaHocModel> DuLieuHoatDongKhoHocKhac(List<int> StaffId, int? Year)
        {
            List<ThongTinCTNhaKhoaHocModel> Datas = new List<ThongTinCTNhaKhoaHocModel>();

            var pList = new SqlParameter("@ListCanBoID", SqlDbType.Structured);
            pList.TypeName = "dbo.list_ID";
            var tbCanBoID = new DataTable();
            tbCanBoID.Columns.Add("CanBoID", typeof(string));
            StaffId.ForEach(x => tbCanBoID.Rows.Add(x));

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Year",SqlDbType.Int),
                pList
            };
            parameters[0].Value = Year ?? 0;
            parameters[1].Value = tbCanBoID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_HoatDongKhoaHocKhac", parameters))
                {
                    while (dr.Read())
                    {
                        ThongTinCTNhaKhoaHocModel info = new ThongTinCTNhaKhoaHocModel();
                        info.LoaiThongTin = Utils.ConvertToInt32(dr["LoaiThongTin"], 0);
                        info.CTNhaKhoaHocID = Utils.ConvertToInt32(dr["CTNhaKhoaHocID"], 0);
                        info.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        info.DeTai = Utils.ConvertToInt32(dr["DeTai"], 0);
                        info.LoaiNhiemVu = Utils.ConvertToInt32(dr["LoaiNhiemVu"], 0);
                        info.NhiemVu = Utils.ConvertToInt32(dr["NhiemVu"], 0);
                        info.TacGiaID = Utils.ConvertToInt32(dr["TacGiaID"], 0);
                        info.CoQuanTGID = Utils.ConvertToInt32(dr["CoQuanTGID"], 0);
                        info.FileDinhKemID = Utils.ConvertToInt32(dr["FileDinhKemID"], 0);
                        info.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        info.Url = Utils.ConvertToString(dr["Url"], string.Empty);
                        info.ISSN = Utils.ConvertToString(dr["ISSN"], string.Empty);

                        if (info.LoaiThongTin == 7) // Bài báo, tạp chí
                        {
                            info.NamDangTai = Utils.ConvertToInt32(dr["NamDangTai"], 0);
                            info.TieuDe = Utils.ConvertToString(dr["TieuDe"], string.Empty);
                            info.TenTapChiSachHoiThao = Utils.ConvertToString(dr["TenTapChiSachHoiThao"], string.Empty);
                            info.NhaXuatBan = Utils.ConvertToString(dr["NhaXuatBan"], string.Empty);
                            info.So = Utils.ConvertToString(dr["So"], string.Empty);
                            info.Trang = Utils.ConvertToString(dr["Trang"], string.Empty);
                            info.LinkBaiBao = Utils.ConvertToString(dr["LinkBaiBao"], string.Empty);
                        }
                        else if (info.LoaiThongTin == 8) // kết quả nghiên cứu
                        {
                            info.NamDangTai = Utils.ConvertToInt32(dr["NamXuatBan"], 0);
                            info.TieuDe = Utils.ConvertToString(dr["TieuDe"], string.Empty);
                        }
                        else if (info.LoaiThongTin == 9) // sách chuyên khảo
                        {
                            info.NamDangTai = Utils.ConvertToInt32(dr["NamXuatBan"], 0);
                            info.TieuDe = Utils.ConvertToString(dr["TenTapChiSachHoiThao"], string.Empty);//TenTapChiSachHoiThao
                            info.NhaXuatBan = Utils.ConvertToString(dr["NhaXuatBan"], string.Empty);
                            info.LinkBaiBao = Utils.ConvertToString(dr["LinkBaiBao"], string.Empty);
                            info.ChuBienID = Utils.ConvertToInt32(dr["ChuBienID"], 0);
                            info.CoQuanChuBienID = Utils.ConvertToInt32(dr["CoQuanChuBienID"], 0);
                        }
                        else if (info.LoaiThongTin == 14) // sản phẩm đào tạo
                        {
                            info.NamDangTai = Utils.ConvertToInt32(dr["NamBaoVe"], 0);
                            info.LoaiDaoTao = Utils.ConvertToInt32(dr["LoaiDaoTao"], 0);
                            info.TieuDe = Utils.ConvertToString(dr["TenLuanVan"], string.Empty);//TenLuanVan
                            info.TenHocVien = Utils.ConvertToString(dr["TenHocVien"], string.Empty);
                            info.NguoiHuongDan = Utils.ConvertToString(dr["NguoiHuongDan"], string.Empty);
                        }
                        Datas.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                return Datas;
                throw ex;
            }

            //List<DuLieuNghienCuuKhoaHocModel> listDuLieuNghienCuu = new List<DuLieuNghienCuuKhoaHocModel>();
            //listDuLieuNghienCuu = Datas.GroupBy(p => p.DeTaiID)
            //        .Select(g => new DuLieuNghienCuuKhoaHocModel
            //        {
            //            Id = g.FirstOrDefault().Id,
            //            YearId = g.FirstOrDefault().YearId,
            //            StaffId = g.FirstOrDefault().StaffId,
            //            ConversionId = g.FirstOrDefault().ConversionId,
            //            Name = g.FirstOrDefault().Name,
            //            Quantity = g.FirstOrDefault().Quantity,
            //            Members = g.FirstOrDefault().Members,
            //            StartYear = g.FirstOrDefault().StartYear,
            //            Desc = g.FirstOrDefault().Desc,
            //            WorkTime = g.FirstOrDefault().WorkTime,
            //            Attach = Datas.Where(x => x.DeTaiID == g.Key && x.FileUrl != null && x.FileUrl.Length > 0)
            //                            .Select(y => y.FileUrl).ToList()
            //        }
            //        ).ToList();
            //return listDuLieuNghienCuu;
            List<DuLieuNghienCuuKhoaHocModel> listDuLieuNghienCuu = new List<DuLieuNghienCuuKhoaHocModel>();
            listDuLieuNghienCuu = Datas.GroupBy(p => new { p.CTNhaKhoaHocID })
                    .Select(g => new DuLieuNghienCuuKhoaHocModel
                    {
                        Id = g.FirstOrDefault().LoaiNhiemVu.Value,
                        StaffId = g.FirstOrDefault().CanBoID,
                        ConversionId = g.FirstOrDefault().NhiemVu.Value,
                        Name = g.FirstOrDefault().TieuDe,
                        Quantity = 1,
                        Members = g.Count(x => x.TacGiaID != null),
                        StartYear = g.FirstOrDefault().NamDangTai.Value,
                        //Desc = g.FirstOrDefault().NamDangTai.ToString() + (g.Count(x => x.TacGiaID != null) > 0 ? ", " +  : ""),
                        WorkTime = 0,
                        Attach = g.Where(x => x.FileDinhKemID > 0 && x.FileUrl != null && x.FileUrl.Length > 0).Select(y => y.FileUrl).ToList(),
                    }
                    ).ToList();
            return Datas;
        }

        /// <summary>
        /// Lấy dữ liệu từ lý lịch khoa học
        /// </summary>
        /// <param name="DeTaiID"></param>
        /// <returns></returns>
        public DeTaiModel BaiBaoSachSanPhamNKH(int DeTaiID)
        {
            List<ThongTinCTNhaKhoaHocModel> Datas = new List<ThongTinCTNhaKhoaHocModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@DeTaiID",SqlDbType.Int),
            };
            parameters[0].Value = DeTaiID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_DeTai_ThongTinTuLyLichKhoaHoc", parameters))
                {
                    while (dr.Read())
                    {
                        ThongTinCTNhaKhoaHocModel nhaKhoaHoc = new ThongTinCTNhaKhoaHocModel();
                        nhaKhoaHoc.LoaiThongTin = Utils.ConvertToInt32(dr["LoaiThongTin"], 0);

                        nhaKhoaHoc.CTNhaKhoaHocID = Utils.ConvertToInt32(dr["CTNhaKhoaHocID"], 0);
                        nhaKhoaHoc.KhoangThoiGian = Utils.ConvertToString(dr["KhoangThoiGian"], string.Empty);
                        nhaKhoaHoc.CoSoDaoTao = Utils.ConvertToString(dr["CoSoDaoTao"], string.Empty);
                        //nhaKhoaHoc.ChuyenNganh = Utils.ConvertToString(dr["ChuyenNganh"], string.Empty);
                        //nhaKhoaHoc.HocVi = Utils.ConvertToString(dr["HocVi"], string.Empty);
                        //nhaKhoaHoc.CoQuanCongTac = Utils.ConvertToString(dr["CoQuanCongTacNKH"], string.Empty);
                        //nhaKhoaHoc.CoQuanCongTacCT = Utils.ConvertToString(dr["CoQuanCongTacCT"], string.Empty);
                        //nhaKhoaHoc.DiaChiDienThoai = Utils.ConvertToString(dr["DiaChiDienThoai"], string.Empty);
                        //nhaKhoaHoc.ChucVu = Utils.ConvertToString(dr["ChucVu"], string.Empty);
                        //nhaKhoaHoc.TenNgoaiNgu = Utils.ConvertToString(dr["TenNgoaiNgu"], string.Empty);
                        //nhaKhoaHoc.Doc = Utils.ConvertToString(dr["Doc"], string.Empty);
                        //nhaKhoaHoc.Viet = Utils.ConvertToString(dr["Viet"], string.Empty);
                        //nhaKhoaHoc.Noi = Utils.ConvertToString(dr["Noi"], string.Empty);
                        nhaKhoaHoc.TieuDe = Utils.ConvertToString(dr["TieuDe"], string.Empty);
                        //nhaKhoaHoc.NgayCap = Utils.ConvertToNullableDateTime(dr["NgayCap"], null);
                        //nhaKhoaHoc.SoHieu = Utils.ConvertToString(dr["SoHieu"], string.Empty);
                        //nhaKhoaHoc.TrinhDo = Utils.ConvertToString(dr["TrinhDo"], string.Empty);
                        //nhaKhoaHoc.NoiCap = Utils.ConvertToString(dr["NoiCapNKH"], string.Empty);
                        //nhaKhoaHoc.NoiCapCC = Utils.ConvertToString(dr["NoiCapCC"], string.Empty);
                        //nhaKhoaHoc.TenDuAn = Utils.ConvertToString(dr["TenDuAn"], string.Empty);
                        //nhaKhoaHoc.CoQuanTaiTro = Utils.ConvertToString(dr["CoQuanTaiTro"], string.Empty);
                        //nhaKhoaHoc.VaiTroThamGia = Utils.ConvertToString(dr["VaiTroThamGia"], string.Empty);
                        nhaKhoaHoc.TacGia = Utils.ConvertToString(dr["TacGia"], string.Empty);
                        nhaKhoaHoc.TenTapChiSachHoiThao = Utils.ConvertToString(dr["TenTapChiSachHoiThao"], string.Empty);
                        nhaKhoaHoc.So = Utils.ConvertToString(dr["So"], string.Empty);
                        nhaKhoaHoc.Trang = Utils.ConvertToString(dr["Trang"], string.Empty);
                        nhaKhoaHoc.NhaXuatBan = Utils.ConvertToString(dr["NhaXuatBan"], string.Empty);
                        nhaKhoaHoc.LoaiBaiBao = Utils.ConvertToInt32(dr["LoaiBaiBao"], 0);
                        nhaKhoaHoc.ISSN = Utils.ConvertToString(dr["ISSN"], string.Empty);
                        nhaKhoaHoc.NhiemVuBaiBao = Utils.ConvertToInt32(dr["NhiemVuBaiBao"], 0);
                        nhaKhoaHoc.LoaiNhiemVu = Utils.ConvertToInt32(dr["LoaiNhiemVu"], 0);
                        nhaKhoaHoc.TenHoiThao = Utils.ConvertToString(dr["TenHoiThao"], string.Empty);

                        nhaKhoaHoc.Tap = Utils.ConvertToString(dr["Tap"], string.Empty);
                        nhaKhoaHoc.NamDangTai = Utils.ConvertToInt32(dr["NamDangTai"], 0);
                        nhaKhoaHoc.LinkBaiBao = Utils.ConvertToString(dr["LinkBaiBao"], string.Empty);
                        nhaKhoaHoc.LinhVucNganhKhoaHoc = Utils.ConvertToInt32(dr["LinhVucNganhKhoaHoc"], 0);
                        nhaKhoaHoc.HeSoAnhHuong = Utils.ConvertToString(dr["HeSoAnhHuong"], string.Empty);
                        nhaKhoaHoc.ChiSo = Utils.ConvertToInt32(dr["ChiSo"], 0);
                        nhaKhoaHoc.RankSCIMAG = Utils.ConvertToInt32(dr["RankSCIMAG"], 0);
                        nhaKhoaHoc.DiemTapChi = Utils.ConvertToDecimal(dr["DiemTapChi"], 0);
                        nhaKhoaHoc.CapHoiThao = Utils.ConvertToInt32(dr["CapHoiThao"], 0);
                        nhaKhoaHoc.NgayHoiThao = Utils.ConvertToNullableDateTime(dr["NgayHoiThao"], null);
                        nhaKhoaHoc.DiaDiemToChuc = Utils.ConvertToString(dr["DiaDiemToChuc"], string.Empty);
                        nhaKhoaHoc.LoaiDaoTao = Utils.ConvertToInt32(dr["LoaiDaoTao"], 0);
                        nhaKhoaHoc.TenHocVien = Utils.ConvertToString(dr["TenHocVien"], string.Empty);
                        nhaKhoaHoc.TenLuanVan = Utils.ConvertToString(dr["TenLuanVan"], string.Empty);
                        nhaKhoaHoc.NguoiHuongDan = Utils.ConvertToString(dr["NguoiHuongDan"], string.Empty);
                        nhaKhoaHoc.NamBaoVe = Utils.ConvertToInt32(dr["NamBaoVe"], 0);
                        nhaKhoaHoc.GhiChu = Utils.ConvertToString(dr["GhiChu"], string.Empty);
                        nhaKhoaHoc.ChuBienID = Utils.ConvertToInt32(dr["ChuBienID"], 0);
                        nhaKhoaHoc.CoQuanChuBienID = Utils.ConvertToInt32(dr["CoQuanChuBienID"], 0);
                        nhaKhoaHoc.NamXuatBan = Utils.ConvertToInt32NullAble(dr["NamXuatBan"], null);

                        nhaKhoaHoc.DeCuong = Utils.ConvertToString(dr["DeCuong"], string.Empty);
                        nhaKhoaHoc.DeTai = Utils.ConvertToInt32(dr["DeTai"], 0);
                        nhaKhoaHoc.TenHuongNghienCuuChinh = Utils.ConvertToString(dr["HuongNghienCuuChinh"], string.Empty);
                        //tac gia
                        nhaKhoaHoc.TacGiaID = Utils.ConvertToInt32(dr["TacGiaID"], 0);
                        nhaKhoaHoc.CanBoTGID = Utils.ConvertToInt32(dr["CanBoTGID"], 0);
                        nhaKhoaHoc.CoQuanTGID = Utils.ConvertToInt32(dr["CoQuanTGID"], 0);
                        nhaKhoaHoc.TenTacGia = Utils.ConvertToString(dr["TenTacGia"], string.Empty);
                        //file
                        nhaKhoaHoc.NghiepVuID = Utils.ConvertToInt32(dr["NghiepVuID"], 0);
                        nhaKhoaHoc.FileDinhKemID = Utils.ConvertToInt32(dr["FileDinhKemID"], 0);
                        nhaKhoaHoc.TenFileGoc = Utils.ConvertToString(dr["TenFileGoc"], string.Empty);
                        nhaKhoaHoc.TenFileHeThong = Utils.ConvertToString(dr["TenFileHeThong"], string.Empty);
                        nhaKhoaHoc.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        nhaKhoaHoc.LoaiFile = Utils.ConvertToInt32(dr["LoaiFile"], 0);
                        nhaKhoaHoc.NguoiTaoID = Utils.ConvertToInt32(dr["NguoiTaoID"], 0);
                        nhaKhoaHoc.TenNguoiTao = Utils.ConvertToString(dr["TenNguoiTao"], string.Empty);
                        nhaKhoaHoc.NgayTao = Utils.ConvertToDateTime(dr["NgayTao"], DateTime.Now);
                        nhaKhoaHoc.NoiDungFile = Utils.ConvertToString(dr["NoiDungFile"], string.Empty);

                        Datas.Add(nhaKhoaHoc);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }

            DeTaiModel dt = new DeTaiModel();
            if(Datas.Count > 0)
            {
                var deTais = Datas;
                List<DeTaiModel> Result = new List<DeTaiModel>();
                Result = (from m in deTais
                          group m by m.DeTai into ctt
                          from item in ctt
                          select new DeTaiModel()
                          {
                              DeTaiID = item.DeTai ?? 0,
                              BaiBaoTapChi = (from i in deTais.Where(x => x.CTNhaKhoaHocID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.BaiBaoTapChi).ToList().GroupBy(x => x.CTNhaKhoaHocID)
                                              select new BaiBaoTapChiModel()
                                              {
                                                  CTNhaKhoaHocID = i.FirstOrDefault().CTNhaKhoaHocID,
                                                  DeTai = i.FirstOrDefault().DeTai,
                                                  KhoangThoiGian = i.FirstOrDefault().KhoangThoiGian,
                                                  TieuDe = i.FirstOrDefault().TieuDe,
                                                  //TacGia = i.FirstOrDefault().TacGia,
                                                  TenTapChiSachHoiThao = i.FirstOrDefault().TenTapChiSachHoiThao,
                                                  So = i.FirstOrDefault().So,
                                                  Trang = i.FirstOrDefault().Trang,
                                                  NhaXuatBan = i.FirstOrDefault().NhaXuatBan,
                                                  LoaiBaiBao = i.FirstOrDefault().LoaiBaiBao,
                                                  ISSN = i.FirstOrDefault().ISSN,
                                                  NhiemVu = i.FirstOrDefault().NhiemVuBaiBao,
                                                  LoaiNhiemVu = i.FirstOrDefault().LoaiNhiemVu,
                                                  Tap = i.FirstOrDefault().Tap,
                                                  NamDangTai = i.FirstOrDefault().NamDangTai,
                                                  LinkBaiBao = i.FirstOrDefault().LinkBaiBao,
                                                  LinhVucNganhKhoaHoc = i.FirstOrDefault().LinhVucNganhKhoaHoc,
                                                  HeSoAnhHuong = i.FirstOrDefault().HeSoAnhHuong,
                                                  ChiSo = i.FirstOrDefault().ChiSo,
                                                  RankSCIMAG = i.FirstOrDefault().RankSCIMAG,
                                                  DiemTapChi = i.FirstOrDefault().DiemTapChi,
                                                  CapHoiThao = i.FirstOrDefault().CapHoiThao,
                                                  NgayHoiThao = i.FirstOrDefault().NgayHoiThao,
                                                  DiaDiemToChuc = i.FirstOrDefault().DiaDiemToChuc,
                                                  Disable = true,
                                                  FileDinhKem = new List<FileDinhKemModel>(),
                                                  ListTacGia = ((from j in deTais.Where(x => x.CTNhaKhoaHocID > 0 && x.TacGiaID > 0 && x.CTNhaKhoaHocID == i.FirstOrDefault().CTNhaKhoaHocID).ToList().GroupBy(x => x.TacGiaID)
                                                                 select new TacGiaModel()
                                                                 {
                                                                     TacGiaID = j.FirstOrDefault().TacGiaID,
                                                                     CTNhaKhoaHocID = j.FirstOrDefault().CTNhaKhoaHocID,
                                                                     TenTacGia = j.FirstOrDefault().TenTacGia,
                                                                     CanBoID = j.FirstOrDefault().CanBoTGID,
                                                                     CoQuanID = j.FirstOrDefault().CoQuanTGID,
                                                                 }
                                                   ).ToList())
                                              }
                                                     ).ToList(),
                              KetQuaNghienCuuCongBo = (from i in deTais.Where(x => x.CTNhaKhoaHocID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.KetQuaNghienCuu).ToList().GroupBy(x => x.CTNhaKhoaHocID)
                                                       select new KetQuaNghienCuuNKHModel()
                                                       {
                                                           CTNhaKhoaHocID = i.FirstOrDefault().CTNhaKhoaHocID,
                                                           DeTai = i.FirstOrDefault().DeTai,
                                                           LoaiNhiemVu = i.FirstOrDefault().LoaiNhiemVu,
                                                           NhiemVu = i.FirstOrDefault().NhiemVuBaiBao,
                                                           TieuDe = i.FirstOrDefault().TieuDe,
                                                           KhoangThoiGian = i.FirstOrDefault().KhoangThoiGian,
                                                           NamXuatBan = i.FirstOrDefault().NamXuatBan,
                                                           GhiChu = i.FirstOrDefault().GhiChu,
                                                           Disable = true,
                                                           FileDinhKem = new List<FileDinhKemModel>(),
                                                           ListTacGia = ((from j in deTais.Where(x => x.CTNhaKhoaHocID > 0 && x.TacGiaID > 0 && x.CTNhaKhoaHocID == i.FirstOrDefault().CTNhaKhoaHocID).ToList().GroupBy(x => x.TacGiaID)
                                                                          select new TacGiaModel()
                                                                          {
                                                                              TacGiaID = j.FirstOrDefault().TacGiaID,
                                                                              CTNhaKhoaHocID = j.FirstOrDefault().CTNhaKhoaHocID,
                                                                              TenTacGia = j.FirstOrDefault().TenTacGia,
                                                                              CanBoID = j.FirstOrDefault().CanBoTGID,
                                                                              CoQuanID = j.FirstOrDefault().CoQuanTGID,
                                                                          }
                                                         ).ToList())
                                                       }
                                                     ).ToList(),
                              SachChuyenKhao = (from i in deTais.Where(x => x.CTNhaKhoaHocID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.SachChuyenKhao).ToList().GroupBy(x => x.CTNhaKhoaHocID)
                                                select new SachChuyenKhaoModel()
                                                {
                                                    CTNhaKhoaHocID = i.FirstOrDefault().CTNhaKhoaHocID,
                                                    DeTai = i.FirstOrDefault().DeTai,
                                                    LoaiNhiemVu = i.FirstOrDefault().LoaiNhiemVu,
                                                    NhiemVu = i.FirstOrDefault().NhiemVuBaiBao,
                                                    TieuDe = i.FirstOrDefault().TieuDe,
                                                    ChuBienID = i.FirstOrDefault().ChuBienID,
                                                    CoQuanChuBienID = i.FirstOrDefault().CoQuanChuBienID,
                                                    NamXuatBan = i.FirstOrDefault().NamXuatBan,
                                                    ISSN = i.FirstOrDefault().ISSN,
                                                    TenTapChiSachHoiThao = i.FirstOrDefault().TenTapChiSachHoiThao,
                                                    So = i.FirstOrDefault().So,
                                                    Trang = i.FirstOrDefault().Trang,
                                                    NhaXuatBan = i.FirstOrDefault().NhaXuatBan,
                                                    Disable = true,
                                                    FileDinhKem = new List<FileDinhKemModel>(),
                                                    ListTacGia = ((from j in deTais.Where(x => x.CTNhaKhoaHocID > 0 && x.TacGiaID > 0 && x.CTNhaKhoaHocID == i.FirstOrDefault().CTNhaKhoaHocID).ToList().GroupBy(x => x.TacGiaID)
                                                                   select new TacGiaModel()
                                                                   {
                                                                       TacGiaID = j.FirstOrDefault().TacGiaID,
                                                                       CTNhaKhoaHocID = j.FirstOrDefault().CTNhaKhoaHocID,
                                                                       TenTacGia = j.FirstOrDefault().TenTacGia,
                                                                       CanBoID = j.FirstOrDefault().CanBoTGID,
                                                                       CoQuanID = j.FirstOrDefault().CoQuanTGID,
                                                                   }
                                                   ).ToList())
                                                }
                                                     ).ToList(),
                              SanPhamDaoTao = (from i in deTais.Where(x => x.CTNhaKhoaHocID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.SanPhamDaoTao).ToList().GroupBy(x => x.CTNhaKhoaHocID)
                                               select new SanPhamDaoTaoModel()
                                               {
                                                   CTNhaKhoaHocID = i.FirstOrDefault().CTNhaKhoaHocID ?? 0,
                                                   DeTai = i.FirstOrDefault().DeTai,
                                                   NhiemVu = i.FirstOrDefault().NhiemVuBaiBao,
                                                   LoaiNhiemVu = i.FirstOrDefault().LoaiNhiemVu,
                                                   LoaiDaoTao = i.FirstOrDefault().LoaiDaoTao,
                                                   TenHocVien = i.FirstOrDefault().TenHocVien,
                                                   TenLuanVan = i.FirstOrDefault().TenLuanVan,
                                                   NguoiHuongDan = i.FirstOrDefault().NguoiHuongDan,
                                                   NamBaoVe = i.FirstOrDefault().NamBaoVe,
                                                   CoSoDaoTao = i.FirstOrDefault().CoSoDaoTao,
                                                   KhoangThoiGian = i.FirstOrDefault().KhoangThoiGian,
                                                   CapHoiThao = i.FirstOrDefault().CapHoiThao,
                                                   Disable = true,
                                               }
                                                     ).ToList(),
                          }
                            ).ToList();

                dt = Result.FirstOrDefault();
            }
            
            return dt;
        }
    }
}
