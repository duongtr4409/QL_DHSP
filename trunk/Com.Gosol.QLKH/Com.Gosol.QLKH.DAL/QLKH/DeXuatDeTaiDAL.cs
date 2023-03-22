using Com.Gosol.QLKH.DAL.QuanTriHeThong;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.QLKH;
using Com.Gosol.QLKH.Security;
using Com.Gosol.QLKH.Ultilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace Com.Gosol.QLKH.DAL.QLKH
{
    public interface IDeXuatDeTaiDAL
    {
        public List<DeXuatDeTaiModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow, int LinhVucNghienCuu, int LinhVucKinhTeXaHoi, int CapQuanLy, int TrangThai, int CanBoID, int CoQuanID, Boolean LaQuanLy, int CanBoIDLogin);
        public DeXuatDeTaiModel GetByID(int DeXuatID);
        public BaseResultModel Insert(DeXuatDeTaiModel DeTaiModel);
        public BaseResultModel Update(DeXuatDeTaiModel DeTaiModel);
        public BaseResultModel Update_TrangThaiDeTai(LichSuDuyetDeXuatModel DeTaiModel);
        public List<DeXuatDeTaiModel> DanhSachDeXuatDaGuiFilter(int CoQuanID, int CanBoID, int LinhVucID, string Keyword, bool isCount);
        public BaseResultModel Delete(DeXuatDeTaiModel DeXuatDeTaiModel);
        public List<LichSuChinhSuaDeXuatModel> GetLichSuDeXuat(int DeXuatID);
        public BaseResultModel Update_DeXuatLog(int DeXuatID, int? CanBoID, int? CoQuanID);
        public List<DeXuatDeTaiChiTiet> GetListQuanLy(int CoQuanID);
        public DeTaiModel GetDeTaiByLSDeXuatID(int DeXuatID);
    }
    public class DeXuatDeTaiDAL : IDeXuatDeTaiDAL
    {
        /// <summary>
        /// Lấy danh sách đề xuất đề tài
        /// </summary>
        /// <param name="p"></param>
        /// <param name="TotalRow"></param>
        /// <param name="LinhVuc"></param>
        /// <param name="CapDeTai"></param>
        /// <param name="TrangThai"></param>
        /// <param name="CanBoID"></param>
        /// <returns></returns>
        public List<DeXuatDeTaiModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow, int LinhVucNghienCuu, int LinhVucKinhTeXaHoi, int CapQuanLy, int TrangThai, int CanBoID, int CoQuanID, Boolean LaQuanLy, int CanBoIDLogin)
        {
            List<DeXuatDeTaiChiTiet> Result = new List<DeXuatDeTaiChiTiet>();

            SqlParameter[] parameters = new SqlParameter[]
                      {
                        new SqlParameter("@Keyword",SqlDbType.NVarChar),
                        new SqlParameter("@OrderByName",SqlDbType.NVarChar),
                        new SqlParameter("@OrderByOption",SqlDbType.NVarChar),
                        new SqlParameter("@pLimit",SqlDbType.Int),
                        new SqlParameter("@pOffset",SqlDbType.Int),
                        new SqlParameter("@TotalRow",SqlDbType.Int),
                        new SqlParameter("@LinhVucNghienCuu",SqlDbType.Int),
                        new SqlParameter("@LinhVucKinhTeXaHoi",SqlDbType.Int),
                        new SqlParameter("@CapQuanLy",SqlDbType.Int),
                        new SqlParameter("@TrangThai",SqlDbType.Int),
                        new SqlParameter("@CanBoID",SqlDbType.Int),
                        new SqlParameter("@CoQuanID",SqlDbType.Int),
                        new SqlParameter("@LaQuanLy",SqlDbType.Bit),
                        new SqlParameter("@CanBoIDLogin",SqlDbType.Int),
                      };
            parameters[0].Value = p.Keyword == null ? "" : p.Keyword.Trim();
            parameters[1].Value = p.OrderByName;
            parameters[2].Value = p.OrderByOption;
            parameters[3].Value = p.Limit;
            parameters[4].Value = p.Offset;
            parameters[5].Direction = ParameterDirection.Output;
            parameters[5].Size = 8;
            parameters[6].Value = LinhVucNghienCuu;
            parameters[7].Value = LinhVucKinhTeXaHoi;
            parameters[8].Value = CapQuanLy;
            parameters[9].Value = TrangThai;
            parameters[10].Value = CanBoID;
            parameters[11].Value = CoQuanID;
            parameters[12].Value = LaQuanLy;
            parameters[13].Value = CanBoIDLogin;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_DeXuatDeTai_GetPagingBySearch", parameters))
                {
                    while (dr.Read())
                    {
                        DeXuatDeTaiChiTiet deTai = new DeXuatDeTaiChiTiet();
                        deTai.DeXuatID = Utils.ConvertToInt32(dr["DeXuatID"], 0);
                        deTai.TenDeXuat = Utils.ConvertToString(dr["TenDeXuat"], string.Empty);
                        deTai.MaDeXuat = Utils.ConvertToString(dr["MaDeXuat"], string.Empty);
                        deTai.LinhVucNghienCuu = Utils.ConvertToInt32(dr["LinhVucNghienCuu"], 0);
                        deTai.TenLinhVucNghienCuu = Utils.ConvertToString(dr["TenLinhVucNghienCuu"], string.Empty);
                        deTai.LinhVucKinhTeXaHoi = Utils.ConvertToInt32(dr["LinhVucKinhTeXaHoi"], 0);
                        deTai.TenLinhVucKinhTeXaHoi = Utils.ConvertToString(dr["TenLinhVucKinhTeXaHoi"], string.Empty);
                        deTai.CapQuanLy = Utils.ConvertToInt32(dr["CapQuanLy"], 0);
                        deTai.TenCapQuanLy = Utils.ConvertToString(dr["TenCapQuanLy"], string.Empty);
                        deTai.NguoiDeXuat = Utils.ConvertToInt32(dr["NguoiDeXuat"], 0);
                        deTai.TenNguoiDeXuat = Utils.ConvertToString(dr["TenNguoiDeXuat"], string.Empty);
                        deTai.TinhCapThiet = Utils.ConvertToString(dr["TinhCapThiet"], string.Empty);
                        deTai.MucTieu = Utils.ConvertToString(dr["MucTieu"], string.Empty);
                        deTai.SanPham = Utils.ConvertToString(dr["SanPham"], string.Empty);
                        deTai.KinhPhiDuKien = Utils.ConvertToDecimal(dr["KinhPhiDuKien"], 0);
                        deTai.TrangThai = Utils.ConvertToInt32(dr["TrangThai"], 0);
                        deTai.TenTrangThai = Utils.ConvertToString(dr["TenTrangThai"], string.Empty);
                        deTai.NgayThucHien = Utils.ConvertToDateTime(dr["NgayThucHien"], DateTime.Now);
                        deTai.ThoiGianNghienCuu = Utils.ConvertToInt32(dr["ThoiGianNghienCuu"], 0);

                        deTai.NgayDeXuat = Utils.ConvertToNullableDateTime(dr["NgayDeXuat"], null);
                        deTai.NoiDung = Utils.ConvertToString(dr["NoiDung"], string.Empty);
                        deTai.DiaChiUngDung = Utils.ConvertToString(dr["DiaChiUngDung"], string.Empty);
                        deTai.ThoiGianThucHienTu = Utils.ConvertToString(dr["ThoiGianThucHienTu"], string.Empty);
                        deTai.ThoiGianThucHienDen = Utils.ConvertToString(dr["ThoiGianThucHienDen"], string.Empty);
                        deTai.ThuocChuongTrinh = Utils.ConvertToString(dr["ThuocChuongTrinh"], string.Empty);

                        deTai.NghiepVuID = Utils.ConvertToInt32(dr["NghiepVuID"], 0);
                        deTai.FileDinhKemID = Utils.ConvertToInt32(dr["FileDinhKemID"], 0);
                        deTai.TenFileGoc = Utils.ConvertToString(dr["TenFileGoc"], string.Empty);
                        deTai.TenFileHeThong = Utils.ConvertToString(dr["TenFileHeThong"], string.Empty);
                        deTai.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        deTai.LoaiFile = Utils.ConvertToInt32(dr["LoaiFile"], 0);
                        deTai.NguoiTaoID = Utils.ConvertToInt32(dr["NguoiTaoID"], 0);
                        deTai.TenNguoiTao = Utils.ConvertToString(dr["TenNguoiTao"], string.Empty);
                        deTai.NgayTao = Utils.ConvertToDateTime(dr["NgayTao"], DateTime.Now);
                        deTai.NoiDungFile = Utils.ConvertToString(dr["NoiDungFile"], string.Empty);

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
            List<DeXuatDeTaiModel> Data = new List<DeXuatDeTaiModel>();
            if (Result.Count > 0)
            {
                Data = Result.GroupBy(p => p.DeXuatID)
                    .Select(g => new DeXuatDeTaiModel
                    {
                        DeXuatID = g.Key,
                        TenDeXuat = g.FirstOrDefault().TenDeXuat,
                        MaDeXuat = g.FirstOrDefault().MaDeXuat,
                        LinhVucNghienCuu = g.FirstOrDefault().LinhVucNghienCuu,
                        TenLinhVucNghienCuu = g.FirstOrDefault().TenLinhVucNghienCuu,
                        LinhVucKinhTeXaHoi = g.FirstOrDefault().LinhVucKinhTeXaHoi,
                        TenLinhVucKinhTeXaHoi = g.FirstOrDefault().TenLinhVucKinhTeXaHoi,
                        CapQuanLy = g.FirstOrDefault().CapQuanLy,
                        TenCapQuanLy = g.FirstOrDefault().TenCapQuanLy,
                        NguoiDeXuat = g.FirstOrDefault().NguoiDeXuat,
                        TenNguoiDeXuat = g.FirstOrDefault().TenNguoiDeXuat,
                        TinhCapThiet = g.FirstOrDefault().TinhCapThiet,
                        MucTieu = g.FirstOrDefault().MucTieu,
                        SanPham = g.FirstOrDefault().SanPham,
                        KinhPhiDuKien = g.FirstOrDefault().KinhPhiDuKien,
                        TrangThai = g.FirstOrDefault().TrangThai,
                        TenTrangThai = g.FirstOrDefault().TenTrangThai,
                        NgayThucHien = g.FirstOrDefault().NgayThucHien,
                        ThoiGianNghienCuu = g.FirstOrDefault().ThoiGianNghienCuu,
                        NgayDeXuat = g.FirstOrDefault().NgayDeXuat,
                        NoiDung = g.FirstOrDefault().NoiDung,
                        DiaChiUngDung = g.FirstOrDefault().DiaChiUngDung,
                        ThoiGianThucHienTu = g.FirstOrDefault().ThoiGianThucHienTu,
                        ThoiGianThucHienDen = g.FirstOrDefault().ThoiGianThucHienDen,
                        ThuocChuongTrinh = g.FirstOrDefault().ThuocChuongTrinh,
                        FileDinhKem = Result.Where(x => x.DeXuatID == g.Key && x.FileDinhKemID > 0)
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
                    ).OrderByDescending(x => x.DeXuatID).ToList();
            }

            return Data;
        }

        /// <summary>
        /// Lấy chi tiết đề xuất đề tài
        /// </summary>
        /// <param name="DeTaiID"></param>
        /// <returns></returns>
        public DeXuatDeTaiModel GetByID(int DeXuatID)
        {
            List<DeXuatDeTaiModel> Result = new List<DeXuatDeTaiModel>();
            List<DeXuatDeTaiChiTiet> deXuatDeTais = new List<DeXuatDeTaiChiTiet>();

            SqlParameter[] parameters = new SqlParameter[]
            {
                  new SqlParameter("@DeXuatID",SqlDbType.Int)
            };
            parameters[0].Value = DeXuatID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_DeXuatDeTai_GetByID", parameters))
                {
                    while (dr.Read())
                    {
                        DeXuatDeTaiChiTiet deTai = new DeXuatDeTaiChiTiet();
                        deTai.DeXuatID = Utils.ConvertToInt32(dr["DeXuatID"], 0);
                        deTai.TenDeXuat = Utils.ConvertToString(dr["TenDeXuat"], string.Empty);
                        deTai.MaDeXuat = Utils.ConvertToString(dr["MaDeXuat"], string.Empty);
                        deTai.LinhVucNghienCuu = Utils.ConvertToInt32(dr["LinhVucNghienCuu"], 0);
                        deTai.LinhVucKinhTeXaHoi = Utils.ConvertToInt32(dr["LinhVucKinhTeXaHoi"], 0);
                        deTai.CapQuanLy = Utils.ConvertToInt32(dr["CapQuanLy"], 0);
                        deTai.NguoiDeXuat = Utils.ConvertToInt32(dr["NguoiDeXuat"], 0);
                        deTai.TinhCapThiet = Utils.ConvertToString(dr["TinhCapThiet"], string.Empty);
                        deTai.MucTieu = Utils.ConvertToString(dr["MucTieu"], string.Empty);
                        deTai.SanPham = Utils.ConvertToString(dr["SanPham"], string.Empty);
                        deTai.KinhPhiDuKien = Utils.ConvertToDecimal(dr["KinhPhiDuKien"], 0);
                        deTai.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        deTai.ThoiGianNghienCuu = Utils.ConvertToInt32(dr["ThoiGianNghienCuu"], 0);
                        deTai.TrangThai = Utils.ConvertToInt32(dr["TrangThai"], 0);

                        deTai.NgayDeXuat = Utils.ConvertToNullableDateTime(dr["NgayDeXuat"], null);
                        deTai.NoiDung = Utils.ConvertToString(dr["NoiDung"], string.Empty);
                        deTai.DiaChiUngDung = Utils.ConvertToString(dr["DiaChiUngDung"], string.Empty);
                        deTai.ThoiGianThucHienTu = Utils.ConvertToString(dr["ThoiGianThucHienTu"], string.Empty);
                        deTai.ThoiGianThucHienDen = Utils.ConvertToString(dr["ThoiGianThucHienDen"], string.Empty);
                        deTai.ThuocChuongTrinh = Utils.ConvertToString(dr["ThuocChuongTrinh"], string.Empty);

                        deTai.NghiepVuID = Utils.ConvertToInt32(dr["NghiepVuID"], 0);
                        deTai.FileDinhKemID = Utils.ConvertToInt32(dr["FileDinhKemID"], 0);
                        deTai.TenFileGoc = Utils.ConvertToString(dr["TenFileGoc"], string.Empty);
                        deTai.TenFileHeThong = Utils.ConvertToString(dr["TenFileHeThong"], string.Empty);
                        deTai.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        deTai.LoaiFile = Utils.ConvertToInt32(dr["LoaiFile"], 0);
                        deTai.NguoiTaoID = Utils.ConvertToInt32(dr["NguoiTaoID"], 0);
                        deTai.CoQuanTaoID = Utils.ConvertToInt32(dr["CoQuanTaoID"], 0);
                        deTai.TenNguoiTao = Utils.ConvertToString(dr["TenNguoiTao"], string.Empty);
                        deTai.NgayTao = Utils.ConvertToDateTime(dr["NgayTao"], DateTime.Now);
                        deTai.NoiDungFile = Utils.ConvertToString(dr["NoiDungFile"], string.Empty);
                        //thong tin xet duyet
                        deTai.DuyetDeXuatID = Utils.ConvertToInt32(dr["DuyetDeXuatID"], 0);
                        deTai.TrangThaiDuyet = Utils.ConvertToInt32(dr["TrangThaiDuyet"], 0);
                        deTai.NgayThucHienDuyet = Utils.ConvertToDateTime(dr["NgayThucHienDuyet"], DateTime.Now);
                        deTai.NoiDungDuyet = Utils.ConvertToString(dr["NoiDungDuyet"], string.Empty);
                        deTai.TenNguoiThucHien = Utils.ConvertToString(dr["TenNguoiThucHien"], string.Empty);
                        deTai.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        deXuatDeTais.Add(deTai);
                    }
                    dr.Close();
                }
                if (deXuatDeTais.Count > 0)
                {
                    Result = (from m in deXuatDeTais
                              group m by m.DeXuatID into ctt
                              from item in ctt
                              select new DeXuatDeTaiModel()
                              {
                                  DeXuatID = item.DeXuatID,
                                  TenDeXuat = item.TenDeXuat,
                                  MaDeXuat = item.MaDeXuat,
                                  LinhVucNghienCuu = item.LinhVucNghienCuu,
                                  LinhVucKinhTeXaHoi = item.LinhVucKinhTeXaHoi,
                                  CapQuanLy = item.CapQuanLy,
                                  NguoiDeXuat = item.NguoiDeXuat,
                                  CoQuanID = item.CoQuanID,
                                  ThoiGianNghienCuu = item.ThoiGianNghienCuu,
                                  TinhCapThiet = item.TinhCapThiet,
                                  MucTieu = item.MucTieu,
                                  SanPham = item.SanPham,
                                  KinhPhiDuKien = item.KinhPhiDuKien,
                                  TrangThai = item.TrangThai,
                                  NgayDeXuat = item.NgayDeXuat,
                                  NoiDung = item.NoiDung,
                                  DiaChiUngDung = item.DiaChiUngDung,
                                  ThoiGianThucHienTu = item.ThoiGianThucHienTu,
                                  ThoiGianThucHienDen = item.ThoiGianThucHienDen,
                                  ThuocChuongTrinh = item.ThuocChuongTrinh,
                                  FileDinhKem = (from i in deXuatDeTais.Where(x => x.FileDinhKemID > 0 && x.LoaiFile == (int)EnumLoaiFileDinhKem.DeXuatDeTai).ToList().GroupBy(x => x.FileDinhKemID)
                                                 select new FileDinhKemModel()
                                                 {
                                                     NghiepVuID = i.FirstOrDefault().DeXuatID,
                                                     FileDinhKemID = i.FirstOrDefault().FileDinhKemID,
                                                     TenFileHeThong = i.FirstOrDefault().TenFileHeThong,
                                                     TenFileGoc = i.FirstOrDefault().TenFileGoc,
                                                     LoaiFile = i.FirstOrDefault().LoaiFile,
                                                     FileUrl = i.FirstOrDefault().FileUrl,
                                                     NoiDung = i.FirstOrDefault().NoiDungFile,
                                                     NgayTao = i.FirstOrDefault().NgayTao,
                                                     NguoiTaoID = i.FirstOrDefault().NguoiTaoID,
                                                     TenNguoiTao = i.FirstOrDefault().TenNguoiTao,
                                                     CoQuanID = i.FirstOrDefault().CoQuanTaoID,
                                                 }
                                                         ).ToList(),
                                  ThongTinXetDuyetDeTai = (from i in deXuatDeTais.Where(x => x.DuyetDeXuatID > 0).ToList().GroupBy(x => x.DuyetDeXuatID)
                                                           select new LichSuDuyetDeXuatModel()
                                                           {
                                                               DuyetDeXuatID = i.FirstOrDefault().DuyetDeXuatID,
                                                               DeXuatID = i.FirstOrDefault().DeXuatID,
                                                               TrangThai = i.FirstOrDefault().TrangThaiDuyet,
                                                               NgayThucHien = i.FirstOrDefault().NgayThucHienDuyet,
                                                               NoiDung = i.FirstOrDefault().NoiDungDuyet,
                                                               TenNguoiThucHien = i.FirstOrDefault().TenNguoiThucHien,
                                                               CanBoID = i.FirstOrDefault().CanBoID,
                                                           }
                                                         ).ToList(),
                              }
                       ).ToList();
                }
                var ct = Result.FirstOrDefault();
                if (ct.ThongTinXetDuyetDeTai.Count > 0)
                {
                    foreach (var item in ct.ThongTinXetDuyetDeTai)
                    {
                        item.FileXetDuyet = new FileDinhKemDAL().GetByLoaiFileAndNghiepVuID((int)EnumLoaiFileDinhKem.DuyetDeXuat, item.DuyetDeXuatID);
                    }
                }
                ct.ThongTinChinhSuaDeXuat = GetLichSuDeXuat(ct.DeXuatID);
                if (ct.TrangThai == (int)EnumTrangThaiDeXuat.DaDuyet)
                {
                    foreach (var item in ct.ThongTinXetDuyetDeTai)
                    {
                        if (item.TrangThai == (int)EnumTrangThaiDeXuat.DaDuyet)
                        {
                            ct.FileThuyetMinh = item.FileXetDuyet;
                            break;
                        }
                    }
                }
                return ct;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Lịch sử chỉnh sửa đề xuất
        /// </summary>
        /// <param name="DeXuatID"></param>
        /// <returns></returns>
        public List<LichSuChinhSuaDeXuatModel> GetLichSuDeXuat(int DeXuatID)
        {
            List<DeXuatLog> deXuatLogs = new List<DeXuatLog>();
            List<LichSuChinhSuaDeXuatModel> Result = new List<LichSuChinhSuaDeXuatModel>();

            SqlParameter[] parameters = new SqlParameter[]
            {
                  new SqlParameter("@DeXuatID",SqlDbType.Int)
            };
            parameters[0].Value = DeXuatID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_DeXuatLog_GetByDeXuatID", parameters))
                {
                    while (dr.Read())
                    {
                        DeXuatLog deXuat = new DeXuatLog();
                        deXuat.DeXuatID = Utils.ConvertToInt32(dr["DeXuatID"], 0);
                        deXuat.TenDeXuat = Utils.ConvertToString(dr["TenDeXuat"], string.Empty);
                        deXuat.TenDeXuatEdit = Utils.ConvertToString(dr["TenDeXuatEdit"], string.Empty);
                        deXuat.MaDeXuat = Utils.ConvertToString(dr["MaDeXuat"], string.Empty);
                        deXuat.MaDeXuatEdit = Utils.ConvertToString(dr["MaDeXuatEdit"], string.Empty);
                        deXuat.LinhVucNghienCuu = Utils.ConvertToInt32(dr["LinhVucNghienCuu"], 0);
                        deXuat.LinhVucNghienCuuEdit = Utils.ConvertToInt32(dr["LinhVucNghienCuuEdit"], 0);
                        deXuat.TenLinhVucNghienCuu = Utils.ConvertToString(dr["TenLinhVucNghienCuu"], string.Empty);
                        deXuat.TenLinhVucNghienCuuEdit = Utils.ConvertToString(dr["TenLinhVucNghienCuuEdit"], string.Empty);
                        deXuat.LinhVucKinhTeXaHoi = Utils.ConvertToInt32(dr["LinhVucKinhTeXaHoi"], 0);
                        deXuat.LinhVucKinhTeXaHoiEdit = Utils.ConvertToInt32(dr["LinhVucKinhTeXaHoiEdit"], 0);
                        deXuat.TenLinhVucKinhTeXaHoi = Utils.ConvertToString(dr["TenLinhVucKinhTeXaHoi"], string.Empty);
                        deXuat.TenLinhVucKinhTeXaHoiEdit = Utils.ConvertToString(dr["TenLinhVucKinhTeXaHoiEdit"], string.Empty);
                        deXuat.CapQuanLy = Utils.ConvertToInt32(dr["CapQuanLy"], 0);
                        deXuat.CapQuanLyEdit = Utils.ConvertToInt32(dr["CapQuanLyEdit"], 0);
                        deXuat.TenCapQuanLy = Utils.ConvertToString(dr["TenCapQuanLy"], string.Empty);
                        deXuat.TenCapQuanLyEdit = Utils.ConvertToString(dr["TenCapQuanLyEdit"], string.Empty);
                        deXuat.NguoiDeXuat = Utils.ConvertToInt32(dr["NguoiDeXuat"], 0);
                        deXuat.NguoiDeXuatEdit = Utils.ConvertToInt32(dr["NguoiDeXuatEdit"], 0);
                        deXuat.TenNguoiDeXuat = Utils.ConvertToString(dr["TenNguoiDeXuat"], string.Empty);
                        deXuat.TenNguoiDeXuatEdit = Utils.ConvertToString(dr["TenNguoiDeXuatEdit"], string.Empty);
                        deXuat.TinhCapThiet = Utils.ConvertToString(dr["TinhCapThiet"], string.Empty);
                        deXuat.TinhCapThietEdit = Utils.ConvertToString(dr["TinhCapThietEdit"], string.Empty);
                        deXuat.MucTieu = Utils.ConvertToString(dr["MucTieu"], string.Empty);
                        deXuat.MucTieuEdit = Utils.ConvertToString(dr["MucTieuEdit"], string.Empty);
                        deXuat.SanPham = Utils.ConvertToString(dr["SanPham"], string.Empty);
                        deXuat.SanPhamEdit = Utils.ConvertToString(dr["SanPhamEdit"], string.Empty);
                        deXuat.KinhPhiDuKien = Utils.ConvertToDecimal(dr["KinhPhiDuKien"], 0);
                        deXuat.KinhPhiDuKienEdit = Utils.ConvertToDecimal(dr["KinhPhiDuKienEdit"], 0);
                        deXuat.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        deXuat.CoQuanIDEdit = Utils.ConvertToInt32(dr["CoQuanIDEdit"], 0);
                        deXuat.ThoiGianNghienCuu = Utils.ConvertToInt32(dr["ThoiGianNghienCuu"], 0);
                        deXuat.ThoiGianNghienCuuEdit = Utils.ConvertToInt32(dr["ThoiGianNghienCuuEdit"], 0);

                        deXuat.NgayDeXuat = Utils.ConvertToNullableDateTime(dr["NgayDeXuat"], null);
                        deXuat.NgayDeXuatEdit = Utils.ConvertToNullableDateTime(dr["NgayDeXuatEdit"], null);
                        deXuat.NoiDung = Utils.ConvertToString(dr["NoiDung"], string.Empty);
                        deXuat.NoiDungEdit = Utils.ConvertToString(dr["NoiDungEdit"], string.Empty);
                        deXuat.DiaChiUngDung = Utils.ConvertToString(dr["DiaChiUngDung"], string.Empty);
                        deXuat.DiaChiUngDungEdit = Utils.ConvertToString(dr["DiaChiUngDungEdit"], string.Empty);
                        deXuat.ThoiGianThucHienTu = Utils.ConvertToString(dr["ThoiGianThucHienTu"], string.Empty);
                        deXuat.ThoiGianThucHienTuEdit = Utils.ConvertToString(dr["ThoiGianThucHienTuEdit"], string.Empty);
                        deXuat.ThoiGianThucHienDen = Utils.ConvertToString(dr["ThoiGianThucHienDen"], string.Empty);
                        deXuat.ThoiGianThucHienDenEdit = Utils.ConvertToString(dr["ThoiGianThucHienDenEdit"], string.Empty);
                        deXuat.ThuocChuongTrinh = Utils.ConvertToString(dr["ThuocChuongTrinh"], string.Empty);
                        deXuat.ThuocChuongTrinhEdit = Utils.ConvertToString(dr["ThuocChuongTrinhEdit"], string.Empty);
                        deXuat.ThoiGianThucHien = deXuat.ThoiGianThucHienTu + "-" + deXuat.ThoiGianThucHienDen;
                        deXuat.ThoiGianThucHienEdit = deXuat.ThoiGianThucHienTuEdit + "-" + deXuat.ThoiGianThucHienDenEdit;

                        deXuat.TrangThai = Utils.ConvertToInt32(dr["TrangThai"], 0);
                        deXuat.FileID = Utils.ConvertToString(dr["FileID"], string.Empty);
                        deXuat.FileIDEdit = Utils.ConvertToString(dr["FileIDEdit"], string.Empty);
                        deXuat.TrangThai = Utils.ConvertToInt32(dr["TrangThai"], 0);

                        deXuat.ID = Utils.ConvertToInt32(dr["ID"], 0);
                        deXuat.NgayChinhSua = Utils.ConvertToNullableDateTime(dr["NgayChinhSua"], null);
                        deXuat.CanBoChinhSuaID = Utils.ConvertToInt32(dr["CanBoChinhSuaID"], 0);
                        deXuat.CoQuanChinhSuaID = Utils.ConvertToInt32(dr["CoQuanChinhSuaID"], 0);
                        deXuatLogs.Add(deXuat);
                    }
                    dr.Close();
                }
                foreach (var deXuat in deXuatLogs)
                {
                    if (deXuat.CanBoChinhSuaID > 0)
                    {
                        LichSuChinhSuaDeXuatModel ls = new LichSuChinhSuaDeXuatModel();
                        ls.ID = deXuat.ID;
                        ls.DeXuatID = deXuat.DeXuatID;
                        ls.CanBoID = deXuat.CanBoChinhSuaID;
                        ls.CoQuanID = deXuat.CoQuanChinhSuaID;
                        ls.NgayChinhSua = deXuat.NgayChinhSua;
                        ls.NoiDungChinhSua = new List<NoiDungChinhSua>();
                        ls.Data = new List<DeXuatDeTaiModel>();

                        if (deXuat.TenDeXuat != deXuat.TenDeXuatEdit)
                        {
                            NoiDungChinhSua nd = new NoiDungChinhSua();
                            nd.key = "TenDeXuat";
                            nd.label = "Tên đề xuất";
                            ls.NoiDungChinhSua.Add(nd);
                        }
                        if (deXuat.MaDeXuat != deXuat.MaDeXuatEdit)
                        {
                            NoiDungChinhSua nd = new NoiDungChinhSua();
                            nd.key = "MaDeXuat";
                            nd.label = "Mã đề xuất";
                            ls.NoiDungChinhSua.Add(nd);
                        }
                        if (deXuat.LinhVucNghienCuu != deXuat.LinhVucNghienCuuEdit)
                        {
                            NoiDungChinhSua nd = new NoiDungChinhSua();
                            nd.key = "LinhVucNghienCuu";
                            nd.label = "Lĩnh vực nghiên cứu KHCN";
                            ls.NoiDungChinhSua.Add(nd);
                        }
                        if (deXuat.LinhVucKinhTeXaHoi != deXuat.LinhVucKinhTeXaHoiEdit)
                        {
                            NoiDungChinhSua nd = new NoiDungChinhSua();
                            nd.key = "LinhVucKinhTeXaHoi";
                            nd.label = "Lĩnh vực kinh tế - xã hội";
                            ls.NoiDungChinhSua.Add(nd);
                        }

                        if (deXuat.CapQuanLy != deXuat.CapQuanLyEdit)
                        {
                            NoiDungChinhSua nd = new NoiDungChinhSua();
                            nd.key = "CapQuanLy";
                            nd.label = "Cấp quản lý";
                            ls.NoiDungChinhSua.Add(nd);
                        }

                        if (deXuat.NguoiDeXuat != deXuat.NguoiDeXuatEdit)
                        {
                            NoiDungChinhSua nd = new NoiDungChinhSua();
                            nd.key = "NguoiDeXuat";
                            nd.label = "Người đề xuất";
                            ls.NoiDungChinhSua.Add(nd);
                        }

                        if (deXuat.TinhCapThiet != deXuat.TinhCapThietEdit)
                        {
                            NoiDungChinhSua nd = new NoiDungChinhSua();
                            nd.key = "TinhCapThiet";
                            nd.label = "Tính cấp thiết";
                            ls.NoiDungChinhSua.Add(nd);
                        }

                        if (deXuat.MucTieu != deXuat.MucTieuEdit)
                        {
                            NoiDungChinhSua nd = new NoiDungChinhSua();
                            nd.key = "MucTieu";
                            nd.label = "Mục tiêu";
                            ls.NoiDungChinhSua.Add(nd);
                        }

                        if (deXuat.SanPham != deXuat.SanPhamEdit)
                        {
                            NoiDungChinhSua nd = new NoiDungChinhSua();
                            nd.key = "SanPham";
                            nd.label = "Sản phẩm";
                            ls.NoiDungChinhSua.Add(nd);
                        }

                        if (deXuat.KinhPhiDuKien != deXuat.KinhPhiDuKienEdit)
                        {
                            NoiDungChinhSua nd = new NoiDungChinhSua();
                            nd.key = "KinhPhiDuKien";
                            nd.label = "Kinh phí dự kiến";
                            ls.NoiDungChinhSua.Add(nd);
                        }

                        if (deXuat.ThoiGianNghienCuu != deXuat.ThoiGianNghienCuuEdit)
                        {
                            NoiDungChinhSua nd = new NoiDungChinhSua();
                            nd.key = "ThoiGianNghienCuu";
                            nd.label = "Thời gian nghiên cứu";
                            ls.NoiDungChinhSua.Add(nd);
                        }

                        DateTime NgayDeXuat = deXuat.NgayDeXuat ?? DateTime.Now;
                        DateTime NgayDeXuatEdit = deXuat.NgayDeXuatEdit ?? DateTime.Now;
                        if (NgayDeXuat.ToString("yyyyMMdd") != NgayDeXuatEdit.ToString("yyyyMMdd"))
                        {
                            NoiDungChinhSua nd = new NoiDungChinhSua();
                            nd.key = "NgayDeXuat";
                            nd.label = "Ngày đề xuất";
                            ls.NoiDungChinhSua.Add(nd);
                        }

                        if (deXuat.NoiDung != deXuat.NoiDungEdit)
                        {
                            NoiDungChinhSua nd = new NoiDungChinhSua();
                            nd.key = "NoiDung";
                            nd.label = "Nội dung";
                            ls.NoiDungChinhSua.Add(nd);
                        }

                        if (deXuat.DiaChiUngDung != deXuat.DiaChiUngDungEdit)
                        {
                            NoiDungChinhSua nd = new NoiDungChinhSua();
                            nd.key = "DiaChiUngDung";
                            nd.label = "Địa chỉ ứng dụng";
                            ls.NoiDungChinhSua.Add(nd);
                        }

                        if ((deXuat.ThoiGianThucHienTu != deXuat.ThoiGianThucHienTuEdit) || (deXuat.ThoiGianThucHienDen != deXuat.ThoiGianThucHienDenEdit))
                        {
                            NoiDungChinhSua nd = new NoiDungChinhSua();
                            nd.key = "ThoiGianThucHien";
                            nd.label = "Thời gian thực hiện";
                            ls.NoiDungChinhSua.Add(nd);
                        }

                        //if (deXuat.ThoiGianThucHienDen != deXuat.ThoiGianThucHienDenEdit)
                        //{
                        //    NoiDungChinhSua nd = new NoiDungChinhSua();
                        //    nd.key = "ThoiGianThucHienDen";
                        //    nd.label = "Thời gian thực hiện";
                        //    ls.NoiDungChinhSua.Add(nd);
                        //}

                        if (deXuat.ThuocChuongTrinh != deXuat.ThuocChuongTrinhEdit)
                        {
                            NoiDungChinhSua nd = new NoiDungChinhSua();
                            nd.key = "ThuocChuongTrinh";
                            nd.label = "Thuộc chương trình";
                            ls.NoiDungChinhSua.Add(nd);
                        }

                        if (deXuat.FileID != deXuat.FileIDEdit)
                        {
                            NoiDungChinhSua nd = new NoiDungChinhSua();
                            nd.key = "FileDinhKem";
                            nd.label = "File đính kèm";
                            ls.NoiDungChinhSua.Add(nd);
                        }

                        if (ls.NoiDungChinhSua.Count > 0)
                        {
                            var listAllFile = new FileDinhKemDAL().GetByLoaiFileAndNghiepVuID((int)EnumLoaiFileDinhKem.DeXuatDeTai, deXuat.DeXuatID);

                            DeXuatDeTaiModel dx = new DeXuatDeTaiModel();
                            dx.DeXuatID = deXuat.DeXuatID;
                            dx.MaDeXuat = deXuat.MaDeXuat;
                            dx.TenDeXuat = deXuat.TenDeXuat;
                            dx.LinhVucNghienCuu = deXuat.LinhVucNghienCuu;
                            dx.TenLinhVucNghienCuu = deXuat.TenLinhVucNghienCuu;
                            dx.LinhVucKinhTeXaHoi = deXuat.LinhVucKinhTeXaHoi;
                            dx.TenLinhVucKinhTeXaHoi = deXuat.TenLinhVucKinhTeXaHoi;
                            dx.CapQuanLy = deXuat.CapQuanLy;
                            dx.TenCapQuanLy = deXuat.TenCapQuanLy;
                            dx.NguoiDeXuat = deXuat.NguoiDeXuat;
                            dx.CoQuanID = deXuat.CoQuanID;
                            dx.TenNguoiDeXuat = deXuat.TenNguoiDeXuat;
                            dx.TinhCapThiet = deXuat.TinhCapThiet;
                            dx.MucTieu = deXuat.MucTieu;
                            dx.SanPham = deXuat.SanPham;
                            dx.KinhPhiDuKien = deXuat.KinhPhiDuKien;
                            dx.ThoiGianNghienCuu = deXuat.ThoiGianNghienCuu;
                            dx.NgayDeXuat = deXuat.NgayDeXuat;
                            dx.NoiDung = deXuat.NoiDung;
                            dx.DiaChiUngDung = deXuat.DiaChiUngDung;
                            dx.ThoiGianThucHien = deXuat.ThoiGianThucHien;
                            dx.ThoiGianThucHienTu = deXuat.ThoiGianThucHienTu;
                            dx.ThoiGianThucHienDen = deXuat.ThoiGianThucHienDen;
                            dx.ThuocChuongTrinh = deXuat.ThuocChuongTrinh;
                            dx.FileDinhKem = new List<FileDinhKemModel>();
                            if (deXuat.FileID != null && deXuat.FileID.Length > 0)
                            {
                                var ListFileID = deXuat.FileID.Split(',');
                                for (int i = 0; i < ListFileID.Length; i++)
                                {
                                    foreach (var item in listAllFile)
                                    {
                                        if (Utils.ConvertToInt32(ListFileID[i], 0) == item.FileDinhKemID)
                                        {
                                            dx.FileDinhKem.Add(item);
                                            break;
                                        }
                                    }
                                }
                            }
                            ls.Data.Add(dx);

                            DeXuatDeTaiModel dxEdit = new DeXuatDeTaiModel();
                            dxEdit.DeXuatID = deXuat.DeXuatID;
                            dxEdit.MaDeXuat = deXuat.MaDeXuatEdit;
                            dxEdit.TenDeXuat = deXuat.TenDeXuatEdit;
                            dxEdit.LinhVucNghienCuu = deXuat.LinhVucNghienCuuEdit;
                            dxEdit.TenLinhVucNghienCuu = deXuat.TenLinhVucNghienCuuEdit;
                            dxEdit.LinhVucKinhTeXaHoi = deXuat.LinhVucKinhTeXaHoiEdit;
                            dxEdit.TenLinhVucKinhTeXaHoi = deXuat.TenLinhVucKinhTeXaHoiEdit;
                            dxEdit.CapQuanLy = deXuat.CapQuanLyEdit;
                            dxEdit.TenCapQuanLy = deXuat.TenCapQuanLyEdit;
                            dxEdit.NguoiDeXuat = deXuat.NguoiDeXuatEdit;
                            dxEdit.CoQuanID = deXuat.CoQuanIDEdit;
                            dxEdit.TenNguoiDeXuat = deXuat.TenNguoiDeXuatEdit;
                            dxEdit.TinhCapThiet = deXuat.TinhCapThietEdit;
                            dxEdit.MucTieu = deXuat.MucTieuEdit;
                            dxEdit.SanPham = deXuat.SanPhamEdit;
                            dxEdit.KinhPhiDuKien = deXuat.KinhPhiDuKienEdit;
                            dxEdit.ThoiGianNghienCuu = deXuat.ThoiGianNghienCuuEdit;
                            dxEdit.NgayDeXuat = deXuat.NgayDeXuatEdit;
                            dxEdit.NoiDung = deXuat.NoiDungEdit;
                            dxEdit.DiaChiUngDung = deXuat.DiaChiUngDungEdit;
                            dxEdit.ThoiGianThucHien = deXuat.ThoiGianThucHienEdit;
                            dxEdit.ThoiGianThucHienTu = deXuat.ThoiGianThucHienTuEdit;
                            dxEdit.ThoiGianThucHienDen = deXuat.ThoiGianThucHienDenEdit;
                            dxEdit.ThuocChuongTrinh = deXuat.ThuocChuongTrinhEdit;
                            dxEdit.FileDinhKem = new List<FileDinhKemModel>();
                            if (deXuat.FileIDEdit != null && deXuat.FileIDEdit.Length > 0)
                            {
                                var ListFileID = deXuat.FileIDEdit.Split(',');
                                for (int i = 0; i < ListFileID.Length; i++)
                                {
                                    foreach (var item in listAllFile)
                                    {
                                        if (Utils.ConvertToInt32(ListFileID[i], 0) == item.FileDinhKemID)
                                        {
                                            dxEdit.FileDinhKem.Add(item);
                                            break;
                                        }
                                    }
                                }
                            }
                            ls.Data.Add(dxEdit);
                        }
                        Result.Add(ls);
                    }
                }

                return Result;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Thêm mới đề xuất đề tài
        /// </summary>
        /// <param name="DeTaiModel"></param>
        /// <returns></returns>
        public BaseResultModel Insert(DeXuatDeTaiModel DeXuatDeTaiModel)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("MaDeXuat", SqlDbType.NVarChar),
                    new SqlParameter("TenDeXuat", SqlDbType.NVarChar),
                    new SqlParameter("LinhVucNghienCuu", SqlDbType.Int),
                    new SqlParameter("LinhVucKinhTeXaHoi", SqlDbType.Int),
                    new SqlParameter("CapQuanLy", SqlDbType.Int),
                    new SqlParameter("NguoiDeXuat",SqlDbType.Int),
                    new SqlParameter("TinhCapThiet", SqlDbType.NVarChar),
                    new SqlParameter("MucTieu", SqlDbType.NVarChar),
                    new SqlParameter("SanPham",SqlDbType.NVarChar),
                    new SqlParameter("KinhPhiDuKien",SqlDbType.Decimal),
                    new SqlParameter("TrangThai",SqlDbType.Int),
                    new SqlParameter("DeTaiID",SqlDbType.Int),
                    new SqlParameter("CoQuanID",SqlDbType.Int),
                    new SqlParameter("ThoiGianNghienCuu",SqlDbType.Int),

                    new SqlParameter("NgayDeXuat",SqlDbType.DateTime),
                    new SqlParameter("NoiDung",SqlDbType.NVarChar),
                    new SqlParameter("DiaChiUngDung",SqlDbType.NVarChar),
                    new SqlParameter("ThoiGianThucHienTu",SqlDbType.NVarChar),
                    new SqlParameter("ThoiGianThucHienDen",SqlDbType.NVarChar),
                    new SqlParameter("ThuocChuongTrinh",SqlDbType.NVarChar),
                };
                parameters[0].Value = DeXuatDeTaiModel.MaDeXuat;
                parameters[1].Value = DeXuatDeTaiModel.TenDeXuat;
                parameters[2].Value = DeXuatDeTaiModel.LinhVucNghienCuu;
                parameters[3].Value = DeXuatDeTaiModel.LinhVucKinhTeXaHoi;
                parameters[4].Value = DeXuatDeTaiModel.CapQuanLy;
                parameters[5].Value = DeXuatDeTaiModel.NguoiDeXuat;
                parameters[6].Value = DeXuatDeTaiModel.TinhCapThiet;
                parameters[7].Value = DeXuatDeTaiModel.MucTieu;
                parameters[8].Value = DeXuatDeTaiModel.SanPham;
                parameters[9].Value = DeXuatDeTaiModel.KinhPhiDuKien;
                parameters[10].Value = (int)EnumTrangThaiDeXuat.ChuaGui;
                parameters[11].Direction = ParameterDirection.Output;
                parameters[11].Size = 8;
                parameters[12].Value = DeXuatDeTaiModel.CoQuanID;
                parameters[13].Value = DeXuatDeTaiModel.ThoiGianNghienCuu;
                parameters[14].Value = DeXuatDeTaiModel.NgayDeXuat ?? DateTime.Now;
                parameters[15].Value = DeXuatDeTaiModel.NoiDung ?? Convert.DBNull;
                parameters[16].Value = DeXuatDeTaiModel.DiaChiUngDung ?? Convert.DBNull;
                parameters[17].Value = DeXuatDeTaiModel.ThoiGianThucHienTu ?? Convert.DBNull;
                parameters[18].Value = DeXuatDeTaiModel.ThoiGianThucHienDen ?? Convert.DBNull;
                parameters[19].Value = DeXuatDeTaiModel.ThuocChuongTrinh ?? Convert.DBNull;

                if (DeXuatDeTaiModel.MaDeXuat == null) parameters[0].Value = DBNull.Value;
                if (DeXuatDeTaiModel.TenDeXuat == null) parameters[1].Value = DBNull.Value;
                if (DeXuatDeTaiModel.LinhVucNghienCuu == null) parameters[2].Value = DBNull.Value;
                if (DeXuatDeTaiModel.LinhVucKinhTeXaHoi == null) parameters[3].Value = DBNull.Value;
                if (DeXuatDeTaiModel.CapQuanLy == null) parameters[4].Value = DBNull.Value;
                if (DeXuatDeTaiModel.NguoiDeXuat == null) parameters[5].Value = DBNull.Value;
                if (DeXuatDeTaiModel.TinhCapThiet == null) parameters[6].Value = DBNull.Value;
                if (DeXuatDeTaiModel.MucTieu == null) parameters[7].Value = DBNull.Value;
                if (DeXuatDeTaiModel.SanPham == null) parameters[8].Value = DBNull.Value;
                if (DeXuatDeTaiModel.KinhPhiDuKien == null) parameters[9].Value = DBNull.Value;
                if (DeXuatDeTaiModel.CoQuanID == null) parameters[12].Value = DBNull.Value;
                if (DeXuatDeTaiModel.ThoiGianNghienCuu == null) parameters[13].Value = DBNull.Value;

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_DeXuatDeTai_Insert", parameters);
                            int DeTaiID = Utils.ConvertToInt32(parameters[11].Value, 0);
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
        /// Cập nhật thông tin đề xuất đề tài
        /// </summary>
        /// <param name="DeTaiModel"></param>
        /// <returns></returns>
        public BaseResultModel Update(DeXuatDeTaiModel DeXuatDeTaiModel)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("DeXuatID",SqlDbType.Int),
                    new SqlParameter("MaDeXuat", SqlDbType.NVarChar),
                    new SqlParameter("TenDeXuat", SqlDbType.NVarChar),
                    new SqlParameter("LinhVucNghienCuu", SqlDbType.Int),
                    new SqlParameter("LinhVucKinhTeXaHoi", SqlDbType.Int),
                    new SqlParameter("CapQuanLy", SqlDbType.Int),
                    new SqlParameter("NguoiDeXuat",SqlDbType.Int),
                    new SqlParameter("TinhCapThiet", SqlDbType.NVarChar),
                    new SqlParameter("MucTieu", SqlDbType.NVarChar),
                    new SqlParameter("SanPham",SqlDbType.NVarChar),
                    new SqlParameter("KinhPhiDuKien",SqlDbType.Decimal),
                    new SqlParameter("CoQuanID",SqlDbType.Int),
                    new SqlParameter("ThoiGianNghienCuu",SqlDbType.Int),
                    new SqlParameter("CanBoChinhSuaID",SqlDbType.Int),
                    new SqlParameter("CoQuanChinhSuaID",SqlDbType.Int),
                    new SqlParameter("NgayChinhSua",SqlDbType.DateTime),

                    new SqlParameter("NgayDeXuat",SqlDbType.DateTime),
                    new SqlParameter("NoiDung",SqlDbType.NVarChar),
                    new SqlParameter("DiaChiUngDung",SqlDbType.NVarChar),
                    new SqlParameter("ThoiGianThucHienTu",SqlDbType.NVarChar),
                    new SqlParameter("ThoiGianThucHienDen",SqlDbType.NVarChar),
                    new SqlParameter("ThuocChuongTrinh",SqlDbType.NVarChar),
                };
                parameters[0].Value = DeXuatDeTaiModel.DeXuatID;
                parameters[1].Value = DeXuatDeTaiModel.MaDeXuat ?? Convert.DBNull;
                parameters[2].Value = DeXuatDeTaiModel.TenDeXuat ?? Convert.DBNull;
                parameters[3].Value = DeXuatDeTaiModel.LinhVucNghienCuu ?? Convert.DBNull;
                parameters[4].Value = DeXuatDeTaiModel.LinhVucKinhTeXaHoi ?? Convert.DBNull;
                parameters[5].Value = DeXuatDeTaiModel.CapQuanLy ?? Convert.DBNull;
                parameters[6].Value = DeXuatDeTaiModel.NguoiDeXuat ?? Convert.DBNull;
                parameters[7].Value = DeXuatDeTaiModel.TinhCapThiet ?? Convert.DBNull;
                parameters[8].Value = DeXuatDeTaiModel.MucTieu ?? Convert.DBNull;
                parameters[9].Value = DeXuatDeTaiModel.SanPham ?? Convert.DBNull;
                parameters[10].Value = DeXuatDeTaiModel.KinhPhiDuKien ?? Convert.DBNull;
                parameters[11].Value = DeXuatDeTaiModel.CoQuanID ?? Convert.DBNull;
                parameters[12].Value = DeXuatDeTaiModel.ThoiGianNghienCuu ?? Convert.DBNull;
                parameters[13].Value = DeXuatDeTaiModel.CanBoChinhSuaID ?? Convert.DBNull;
                parameters[14].Value = DeXuatDeTaiModel.CoQuanChinhSuaID ?? Convert.DBNull;
                parameters[15].Value = DateTime.Now;
                parameters[16].Value = DeXuatDeTaiModel.NgayDeXuat ?? DateTime.Now;
                parameters[17].Value = DeXuatDeTaiModel.NoiDung ?? Convert.DBNull;
                parameters[18].Value = DeXuatDeTaiModel.DiaChiUngDung ?? Convert.DBNull;
                parameters[19].Value = DeXuatDeTaiModel.ThoiGianThucHienTu ?? Convert.DBNull;
                parameters[20].Value = DeXuatDeTaiModel.ThoiGianThucHienDen ?? Convert.DBNull;
                parameters[21].Value = DeXuatDeTaiModel.ThuocChuongTrinh ?? Convert.DBNull;

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Result.Status = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v1_DeXuatDeTai_Update", parameters);

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
        /// Cập nhật trạng thái đề xuất đề tài + insert lịch sử duyệt
        /// </summary>
        /// <param name="DeTaiModel"></param>
        /// <returns></returns>
        public BaseResultModel Update_TrangThaiDeTai(LichSuDuyetDeXuatModel DeTaiModel)
        {
            var Result = new BaseResultModel();

            DeXuatDeTaiModel DeXuatModel = GetByID(DeTaiModel.DeXuatID);
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("DeXuatID", SqlDbType.Int),
                    new SqlParameter("TrangThai", SqlDbType.Int),
                    new SqlParameter("NgayThucHien", SqlDbType.DateTime),
                    new SqlParameter("NoiDung", SqlDbType.NVarChar),
                    new SqlParameter("CanBoID", SqlDbType.Int),
                    new SqlParameter("DuyetDeXuatID", SqlDbType.Int),
                    new SqlParameter("CoQuanID", SqlDbType.Int),
                };
                parameters[0].Value = DeTaiModel.DeXuatID;
                parameters[1].Value = DeTaiModel.TrangThai;
                parameters[2].Value = DeTaiModel.NgayThucHien;
                parameters[3].Value = DeTaiModel.NoiDung;
                parameters[4].Value = DeTaiModel.CanBoID;
                parameters[5].Direction = ParameterDirection.Output;
                parameters[5].Size = 8;
                parameters[6].Value = DeTaiModel.CoQuanID;

                if (DeTaiModel.NoiDung == null || DeTaiModel.NoiDung == "")
                {
                    parameters[3].Value = DBNull.Value;
                }
                if (DeTaiModel.NgayThucHien == null || DeTaiModel.NgayThucHien == DateTime.MinValue)
                {
                    parameters[2].Value = DateTime.Now;
                }

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Result.Status = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v1_DeXuatDeTai_UpdateTrangThai", parameters);

                            if (DeTaiModel.TrangThai == EnumTrangThaiDeXuat.ChuaDuyet.GetHashCode())
                            {
                                // insert vào bảng DeXuatLog khi nhấn gửi đề xuất
                                DeXuatModel.TrangThai = DeTaiModel.TrangThai;
                                Insert_DeXuatLog(DeXuatModel);
                            }

                            if (DeTaiModel.TrangThai == EnumTrangThaiDeXuat.DaDuyet.GetHashCode())
                            {
                                Boolean laCapTruong = false;
                                var listCap = new DanhMucCapDeTaiDAL().GetAll(null);
                                foreach (var item in listCap)
                                {
                                    if (DeXuatModel.CapQuanLy > 0 && DeXuatModel.CapQuanLy == item.Id && item.Type == EnumCapQuanLy.CapTruong.GetHashCode())
                                    {
                                        laCapTruong = true;
                                        break;
                                    }
                                }
                                if (laCapTruong)
                                {
                                    //Lấy thông tin đề xuất chuyển sang màn hình nhiệm vụ nghiên cứu
                                    DeTaiModel dt = new DeTaiModel();
                                    dt.MaDeTai = DeXuatModel.MaDeXuat;
                                    dt.TenDeTai = DeXuatModel.TenDeXuat;
                                    dt.LinhVucNghienCuu = DeXuatModel.LinhVucNghienCuu;
                                    dt.LinhVucKinhTeXaHoi = DeXuatModel.LinhVucKinhTeXaHoi;
                                    dt.CapQuanLy = DeXuatModel.CapQuanLy;
                                    dt.NhiemVu = DeXuatModel.CapQuanLy;
                                    dt.ChuNhiemDeTaiID = DeXuatModel.NguoiDeXuat;
                                    dt.CoQuanChuNhiemID = DeXuatModel.CoQuanID;
                                    dt.NamBatDau = DeXuatModel.ThoiGianThucHienTu;
                                    dt.NamKetThuc = DeXuatModel.ThoiGianThucHienDen;
                                    dt.KinhPhiDHSP = DeXuatModel.KinhPhiDuKien;
                                    dt.NguoiTaoID = DeXuatModel.NguoiDeXuat;
                                    dt.CoQuanID = DeXuatModel.CoQuanID;
                                    dt.DeXuatID = DeXuatModel.DeXuatID;
                                    dt.MucTieu = DeXuatModel.MucTieu;
                                    dt.SanPhamDangKy = DeXuatModel.SanPham;
                                    var detai = new DeTaiDAL().Insert(dt);
                                    int DeTaiID = Utils.ConvertToInt32(detai.Data, 0);
                                    //lấy file cuối cùng nhà khoa học đính kèm chuyển sang đề tài
                                    if(DeTaiID > 0 && DeXuatModel.FileDinhKem != null && DeXuatModel.FileDinhKem.Count > 0)
                                    {
                                        int FileDinhKemID = DeXuatModel.FileDinhKem.Select(x => x.FileDinhKemID).Max();
                                        foreach (var item in DeXuatModel.FileDinhKem)
                                        {
                                            if (item.FileDinhKemID == FileDinhKemID)
                                            {
                                                string url = item.FileUrl;
                                                FileDinhKemModel file = new FileDinhKemModel();
                                                file.FileDinhKemID = item.FileDinhKemID;
                                                file.NghiepVuID = DeTaiID;
                                                file.TenFileGoc = item.TenFileGoc;
                                                file.TenFileHeThong = item.TenFileHeThong;
                                                file.NgayTao = item.NgayTao;
                                                file.NguoiTaoID = item.NguoiTaoID;
                                                file.CoQuanID = item.CoQuanID;
                                                file.NoiDung = item.NoiDung;
                                                file.LoaiFile = EnumLoaiFileDinhKem.DeTai.GetHashCode();
                                                file.FolderPath = nameof(EnumLoaiFileDinhKem.DeTai);
                                                file.FileUrl = item.FileUrl.Replace(nameof(EnumLoaiFileDinhKem.DeXuatDeTai), nameof(EnumLoaiFileDinhKem.DeTai));
                                                new FileDinhKemDAL().Insert_FileDinhKem(file);
                                                try
                                                {
                                                    File.Copy(url, file.FileUrl, true);
                                                }
                                                catch (Exception)
                                                {
                                                    //throw;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            trans.Commit();
                            int DuyetDeXuatID = Utils.ConvertToInt32(parameters[5].Value, 0);
                            Result.Data = DuyetDeXuatID;
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
        /// Thêm mới vào bảng DeXuatLog khi nhấn gửi đề xuất
        /// </summary>
        /// <param name="DeXuatDeTaiModel"></param>
        /// <returns></returns>
        public BaseResultModel Insert_DeXuatLog(DeXuatDeTaiModel DeXuatDeTaiModel)
        {
            var Result = new BaseResultModel();
            string listFileID = "";
            if (DeXuatDeTaiModel.FileDinhKem != null && DeXuatDeTaiModel.FileDinhKem.Count > 0)
            {
                foreach (var item in DeXuatDeTaiModel.FileDinhKem)
                {
                    listFileID += item.FileDinhKemID + ",";
                }
            }

            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("MaDeXuat", SqlDbType.NVarChar),
                    new SqlParameter("TenDeXuat", SqlDbType.NVarChar),
                    new SqlParameter("LinhVucNghienCuu", SqlDbType.Int),
                    new SqlParameter("LinhVucKinhTeXaHoi", SqlDbType.Int),
                    new SqlParameter("CapQuanLy", SqlDbType.Int),
                    new SqlParameter("NguoiDeXuat",SqlDbType.Int),
                    new SqlParameter("TinhCapThiet", SqlDbType.NVarChar),
                    new SqlParameter("MucTieu", SqlDbType.NVarChar),
                    new SqlParameter("SanPham",SqlDbType.NVarChar),
                    new SqlParameter("KinhPhiDuKien",SqlDbType.Decimal),
                    new SqlParameter("TrangThai",SqlDbType.Int),
                    new SqlParameter("DeTaiID",SqlDbType.Int),
                    new SqlParameter("CoQuanID",SqlDbType.Int),
                    new SqlParameter("ThoiGianNghienCuu",SqlDbType.Int),
                    new SqlParameter("DeXuatID",SqlDbType.Int),
                    new SqlParameter("FileID",SqlDbType.NVarChar),

                    new SqlParameter("NgayDeXuat",SqlDbType.DateTime),
                    new SqlParameter("NoiDung",SqlDbType.NVarChar),
                    new SqlParameter("DiaChiUngDung",SqlDbType.NVarChar),
                    new SqlParameter("ThoiGianThucHienTu",SqlDbType.NVarChar),
                    new SqlParameter("ThoiGianThucHienDen",SqlDbType.NVarChar),
                    new SqlParameter("ThuocChuongTrinh",SqlDbType.NVarChar),
                };
                parameters[0].Value = DeXuatDeTaiModel.MaDeXuat;
                parameters[1].Value = DeXuatDeTaiModel.TenDeXuat;
                parameters[2].Value = DeXuatDeTaiModel.LinhVucNghienCuu;
                parameters[3].Value = DeXuatDeTaiModel.LinhVucKinhTeXaHoi;
                parameters[4].Value = DeXuatDeTaiModel.CapQuanLy;
                parameters[5].Value = DeXuatDeTaiModel.NguoiDeXuat;
                parameters[6].Value = DeXuatDeTaiModel.TinhCapThiet;
                parameters[7].Value = DeXuatDeTaiModel.MucTieu;
                parameters[8].Value = DeXuatDeTaiModel.SanPham;
                parameters[9].Value = DeXuatDeTaiModel.KinhPhiDuKien;
                parameters[10].Value = DeXuatDeTaiModel.TrangThai;
                parameters[11].Direction = ParameterDirection.Output;
                parameters[11].Size = 8;
                parameters[12].Value = DeXuatDeTaiModel.CoQuanID;
                parameters[13].Value = DeXuatDeTaiModel.ThoiGianNghienCuu;
                parameters[14].Value = DeXuatDeTaiModel.DeXuatID;
                parameters[15].Value = listFileID;
                parameters[16].Value = DeXuatDeTaiModel.NgayDeXuat ?? DateTime.Now;
                parameters[17].Value = DeXuatDeTaiModel.NoiDung ?? Convert.DBNull;
                parameters[18].Value = DeXuatDeTaiModel.DiaChiUngDung ?? Convert.DBNull;
                parameters[19].Value = DeXuatDeTaiModel.ThoiGianThucHienTu ?? Convert.DBNull;
                parameters[20].Value = DeXuatDeTaiModel.ThoiGianThucHienDen ?? Convert.DBNull;
                parameters[21].Value = DeXuatDeTaiModel.ThuocChuongTrinh ?? Convert.DBNull;

                if (DeXuatDeTaiModel.MaDeXuat == null) parameters[0].Value = DBNull.Value;
                if (DeXuatDeTaiModel.TenDeXuat == null) parameters[1].Value = DBNull.Value;
                if (DeXuatDeTaiModel.LinhVucNghienCuu == null) parameters[2].Value = DBNull.Value;
                if (DeXuatDeTaiModel.LinhVucKinhTeXaHoi == null) parameters[3].Value = DBNull.Value;
                if (DeXuatDeTaiModel.CapQuanLy == null) parameters[4].Value = DBNull.Value;
                if (DeXuatDeTaiModel.NguoiDeXuat == null) parameters[5].Value = DBNull.Value;
                if (DeXuatDeTaiModel.TinhCapThiet == null) parameters[6].Value = DBNull.Value;
                if (DeXuatDeTaiModel.MucTieu == null) parameters[7].Value = DBNull.Value;
                if (DeXuatDeTaiModel.SanPham == null) parameters[8].Value = DBNull.Value;
                if (DeXuatDeTaiModel.KinhPhiDuKien == null) parameters[9].Value = DBNull.Value;
                if (DeXuatDeTaiModel.CoQuanID == null) parameters[12].Value = DBNull.Value;
                if (DeXuatDeTaiModel.ThoiGianNghienCuu == null) parameters[13].Value = DBNull.Value;
                if (listFileID == "") parameters[15].Value = DBNull.Value;

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_DeXuatLog_Insert", parameters);
                            int DeTaiID = Utils.ConvertToInt32(parameters[11].Value, 0);
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

                    Result.Message = ConstantLogMessage.Alert_Insert_Success("Đề xuất log");
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
        /// Cập nhập DeXuatLog
        /// </summary>
        /// <param name="DeXuatID"></param>
        /// <returns></returns>
        public BaseResultModel Update_DeXuatLog(int DeXuatID, int? CanBoID, int? CoQuanID)
        {
            var Result = new BaseResultModel();
            DeXuatDeTaiModel DeXuatDeTaiModel = GetByID(DeXuatID);
            string listFileID = "";
            if (DeXuatDeTaiModel.FileDinhKem != null && DeXuatDeTaiModel.FileDinhKem.Count > 0)
            {
                foreach (var item in DeXuatDeTaiModel.FileDinhKem)
                {
                    listFileID += item.FileDinhKemID + ",";
                }
            }

            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("MaDeXuat", SqlDbType.NVarChar),
                    new SqlParameter("TenDeXuat", SqlDbType.NVarChar),
                    new SqlParameter("LinhVucNghienCuu", SqlDbType.Int),
                    new SqlParameter("LinhVucKinhTeXaHoi", SqlDbType.Int),
                    new SqlParameter("CapQuanLy", SqlDbType.Int),
                    new SqlParameter("NguoiDeXuat",SqlDbType.Int),
                    new SqlParameter("TinhCapThiet", SqlDbType.NVarChar),
                    new SqlParameter("MucTieu", SqlDbType.NVarChar),
                    new SqlParameter("SanPham",SqlDbType.NVarChar),
                    new SqlParameter("KinhPhiDuKien",SqlDbType.Decimal),
                    new SqlParameter("TrangThai",SqlDbType.Int),
                    new SqlParameter("FileID",SqlDbType.NVarChar),
                    new SqlParameter("CoQuanID",SqlDbType.Int),
                    new SqlParameter("ThoiGianNghienCuu",SqlDbType.Int),
                    new SqlParameter("DeXuatID",SqlDbType.Int),
                    new SqlParameter("CanBoChinhSuaID",SqlDbType.Int),
                    new SqlParameter("CoQuanChinhSuaID",SqlDbType.Int),
                    new SqlParameter("NgayChinhSua",SqlDbType.DateTime),
                    new SqlParameter("NgayDeXuat",SqlDbType.DateTime),
                    new SqlParameter("NoiDung",SqlDbType.NVarChar),
                    new SqlParameter("DiaChiUngDung",SqlDbType.NVarChar),
                    new SqlParameter("ThoiGianThucHienTu",SqlDbType.NVarChar),
                    new SqlParameter("ThoiGianThucHienDen",SqlDbType.NVarChar),
                    new SqlParameter("ThuocChuongTrinh",SqlDbType.NVarChar),
                };
                parameters[0].Value = DeXuatDeTaiModel.MaDeXuat;
                parameters[1].Value = DeXuatDeTaiModel.TenDeXuat;
                parameters[2].Value = DeXuatDeTaiModel.LinhVucNghienCuu;
                parameters[3].Value = DeXuatDeTaiModel.LinhVucKinhTeXaHoi;
                parameters[4].Value = DeXuatDeTaiModel.CapQuanLy;
                parameters[5].Value = DeXuatDeTaiModel.NguoiDeXuat;
                parameters[6].Value = DeXuatDeTaiModel.TinhCapThiet;
                parameters[7].Value = DeXuatDeTaiModel.MucTieu;
                parameters[8].Value = DeXuatDeTaiModel.SanPham;
                parameters[9].Value = DeXuatDeTaiModel.KinhPhiDuKien;
                parameters[10].Value = DeXuatDeTaiModel.TrangThai;
                parameters[11].Value = listFileID;
                parameters[12].Value = DeXuatDeTaiModel.CoQuanID;
                parameters[13].Value = DeXuatDeTaiModel.ThoiGianNghienCuu;
                parameters[14].Value = DeXuatDeTaiModel.DeXuatID;
                parameters[15].Value = CanBoID ?? Convert.DBNull;
                parameters[16].Value = CoQuanID ?? Convert.DBNull;
                parameters[17].Value = DateTime.Now;
                parameters[18].Value = DeXuatDeTaiModel.NgayDeXuat ?? DateTime.Now;
                parameters[19].Value = DeXuatDeTaiModel.NoiDung ?? Convert.DBNull;
                parameters[20].Value = DeXuatDeTaiModel.DiaChiUngDung ?? Convert.DBNull;
                parameters[21].Value = DeXuatDeTaiModel.ThoiGianThucHienTu ?? Convert.DBNull;
                parameters[22].Value = DeXuatDeTaiModel.ThoiGianThucHienDen ?? Convert.DBNull;
                parameters[23].Value = DeXuatDeTaiModel.ThuocChuongTrinh ?? Convert.DBNull;

                if (DeXuatDeTaiModel.MaDeXuat == null) parameters[0].Value = DBNull.Value;
                if (DeXuatDeTaiModel.TenDeXuat == null) parameters[1].Value = DBNull.Value;
                if (DeXuatDeTaiModel.LinhVucNghienCuu == null) parameters[2].Value = DBNull.Value;
                if (DeXuatDeTaiModel.LinhVucKinhTeXaHoi == null) parameters[3].Value = DBNull.Value;
                if (DeXuatDeTaiModel.CapQuanLy == null) parameters[4].Value = DBNull.Value;
                if (DeXuatDeTaiModel.NguoiDeXuat == null) parameters[5].Value = DBNull.Value;
                if (DeXuatDeTaiModel.TinhCapThiet == null) parameters[6].Value = DBNull.Value;
                if (DeXuatDeTaiModel.MucTieu == null) parameters[7].Value = DBNull.Value;
                if (DeXuatDeTaiModel.SanPham == null) parameters[8].Value = DBNull.Value;
                if (DeXuatDeTaiModel.KinhPhiDuKien == null) parameters[9].Value = DBNull.Value;
                if (listFileID == "") parameters[11].Value = DBNull.Value;
                if (DeXuatDeTaiModel.CoQuanID == null) parameters[12].Value = DBNull.Value;
                if (DeXuatDeTaiModel.ThoiGianNghienCuu == null) parameters[13].Value = DBNull.Value;


                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_DeXuatLog_Update", parameters);
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

                    Result.Message = ConstantLogMessage.Alert_Insert_Success("Đề xuất log");
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
        /// Dùng cho chức năng hiển thị danh sách đề xuất đã gửi tại màn hình đơn vị nghiên cứu
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public List<DeXuatDeTaiModel> DanhSachDeXuatDaGuiFilter(int CoQuanID, int CanBoID, int LinhVucID, string Keyword, bool isCount)
        {
            var Result = new List<DeXuatDeTaiModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                    new SqlParameter("@Keyword", SqlDbType.NVarChar),
                    new SqlParameter("@LinhVucID",SqlDbType.Int),
                    new SqlParameter("@CoQuanID", SqlDbType.Int)
            };
            parameters[0].Value = Keyword;
            parameters[1].Value = LinhVucID;
            parameters[2].Value = CoQuanID;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_DeXuat_GetListDaGuiFilter", parameters))
                        {
                            while (dr.Read())
                            {
                                DeXuatDeTaiModel dexuat = new DeXuatDeTaiModel();
                                dexuat.DeXuatID = Utils.ConvertToInt32(dr["DeXuatID"], 0);
                                dexuat.MaDeXuat = Utils.ConvertToString(dr["MaDeXuat"], string.Empty);
                                dexuat.TenDeXuat = Utils.ConvertToString(dr["TenDeXuat"], string.Empty);
                                dexuat.TenLinhVucNghienCuu = Utils.ConvertToString(dr["TenLinhVucNghienCuu"], string.Empty);
                                dexuat.TenLinhVucKinhTeXaHoi = Utils.ConvertToString(dr["TenLinhVucKinhTeXaHoi"], string.Empty);
                                dexuat.CapQuanLy = Utils.ConvertToInt32(dr["CapQuanLy"], 0);
                                dexuat.TenCapQuanLy = Utils.ConvertToString(dr["TenCapQuanLy"], string.Empty);
                                dexuat.NguoiDeXuat = Utils.ConvertToInt32(dr["NguoiDeXuat"], 0);
                                dexuat.TrangThai = Utils.ConvertToInt32(dr["TrangThai"], 0);
                                if (dexuat.TrangThai == EnumTrangThaiDeXuat.ChuaDuyet.GetHashCode())
                                {
                                    dexuat.TenTrangThai = "Chưa duyệt";
                                }
                                else if (dexuat.TrangThai == EnumTrangThaiDeXuat.DuyetPhaiSua.GetHashCode())
                                {
                                    dexuat.TenTrangThai = "Duyệt phải sửa";
                                }
                                else if (dexuat.TrangThai == EnumTrangThaiDeXuat.DaDuyet.GetHashCode())
                                {
                                    dexuat.TenTrangThai = "Đã duyệt";
                                }
                                else if (dexuat.TrangThai == EnumTrangThaiDeXuat.KhongDuyet.GetHashCode())
                                {
                                    dexuat.TenTrangThai = "Không duyệt";
                                }
                                if (!isCount)
                                {
                                    dexuat.FileDinhKem = new FileDinhKemDAL().GetByLoaiFileAndNghiepVuID(EnumLoaiFileDinhKem.DeXuatDeTai.GetHashCode(), dexuat.DeXuatID);
                                }
                                Result.Add(dexuat);
                            }
                            dr.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            List<DeXuatDeTaiModel> listFilter = Result;
            if (CanBoID > 0)
            {
                listFilter = Result.Where(item => item.NguoiDeXuat == CanBoID).ToList();
            }
            return listFilter;
        }

        /// <summary>
        /// Xóa đề xuất đề tài
        /// </summary>
        /// <param name="DeXuatDeTaiModel"></param>
        /// <returns></returns>
        public BaseResultModel Delete(DeXuatDeTaiModel DeXuatDeTaiModel)
        {
            var Result = new BaseResultModel();
            var deXuat = GetByID(DeXuatDeTaiModel.DeXuatID);
            if (deXuat != null && deXuat.TrangThai == (int)EnumTrangThaiDeXuat.ChuaGui)
            {
                SqlParameter[] parameters = new SqlParameter[]
                    {
                    new SqlParameter("@DeXuatID",SqlDbType.Int)
                    };
                parameters[0].Value = DeXuatDeTaiModel.DeXuatID;

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, @"v1_DeXuatDeTai_Delete", parameters);
                            Result.Message = ConstantLogMessage.Alert_Delete_Success("đề xuất");
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
            }
            else
            {
                Result.Status = -1;
                Result.Message = "Đề xuất đã duyệt không được xóa";
            }

            return Result;
        }

        /// <summary>
        /// Lấy danh sách quản lý cho thông báo đề xuất
        /// </summary>
        /// <param name="CoQuanID"></param>
        /// <returns></returns>
        public List<DeXuatDeTaiChiTiet> GetListQuanLy(int CoQuanID)
        {
            List<DeXuatDeTaiChiTiet> Result = new List<DeXuatDeTaiChiTiet>();

            SqlParameter[] parameters = new SqlParameter[]
            {
              new SqlParameter("@CoQuanID",SqlDbType.Int),
              new SqlParameter("@ChucNangID",SqlDbType.Int),
            };
            parameters[0].Value = CoQuanID;
            parameters[1].Value = ChucNangEnum.ql_toan_truong.GetHashCode();

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_DeXuatDeTai_GetListQuanLy", parameters))
                {
                    while (dr.Read())
                    {
                        DeXuatDeTaiChiTiet info = new DeXuatDeTaiChiTiet();
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        info.NguoiDungID = Utils.ConvertToInt32(dr["NguoiDungID"], 0);
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


        public DeTaiModel GetDeTaiByLSDeXuatID(int ID)
        {
            DeTaiModel dt = new DeTaiModel();

            SqlParameter[] parameters = new SqlParameter[]
            {
              new SqlParameter("@ID",SqlDbType.Int),
            };
            parameters[0].Value = ID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_DeXuatDeTai_GetDeTaiByDeXuatID", parameters))
                {
                    while (dr.Read())
                    {
                        dt.DeTaiID = Utils.ConvertToInt32(dr["DeTaiID"], 0);
                        dt.DeXuatID = Utils.ConvertToInt32(dr["DeXuatID"], 0);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
    }
}
