using Com.Gosol.QLKH.DAL.QuanTriHeThong;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.DanhMuc;
using Com.Gosol.QLKH.Models.QLKH;
using Com.Gosol.QLKH.Models.QuanTriHeThong;
using Com.Gosol.QLKH.Ultilities;
using Com.Gosol.TDNV.Ultilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.Gosol.QLKH.DAL.QLKH
{
    public interface ILyLichKhoaHocDAL
    {
        public List<NhaKhoaHocModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow, int QuyenQuanLy, string Alphabet);
        public NhaKhoaHocModel GetByID(int CanBoID, string serverPath, int? CoQuanID);
        public BaseResultModel Insert(NhaKhoaHocModel NhaKhoaHocModel);
        public BaseResultModel Edit_ThongTinChiTiet(ThongTinCTNhaKhoaHocModel ThongTinChiTiet);
        public BaseResultModel Delete_ThongTinChiTiet(ThongTinCTNhaKhoaHocModel ThongTinChiTiet);
        public BaseResultModel Edit_HoatDongKhoaHoc(HoatDongKhoaHocModel HoatDongKhoaHoc);
        public BaseResultModel Delete_HoatDongKhoaHoc(HoatDongKhoaHocModel HoatDongKhoaHoc);
        public List<HoatDongKhoaHocModel> HoatDongKhoaHoc_GetByCanBoID(int CanBoID, int CoQuanID);
        public BaseResultModel Update(NhaKhoaHocModel NhaKhoaHocModel);
        public BaseResultModel Delete(NhaKhoaHocModel NhaKhoaHocModel);
        public List<DuAnDeTaiModel> GetDuAnDeTaiByCanBoID(int CanBoID);
        public NhaKhoaHocModel GetThongTinNhaKhoaHoc_DeTai(int CanBoID, Boolean LaCanBoTrongTruong);
        public List<NguoiGioiThieuModel> GetNguoiGioiThieu(int CanBoID, int CoQuanID);
        public BaseResultModel UpdateURL(NhaKhoaHocModel NhaKhoaHocModel);
        public string GetUrlCanBoTrongTruong(int CanBoID, int CoQuanID);
        public List<DanhMucCoQuanDonViModel> SoLuongBaiBaoVaSach(int Nam);
        public NhaKhoaHocModel GetThongTinTuNhaKhoaHocKhac(int CanBoID, int CoQuanID);
        public NhaKhoaHocModel GetThongTinTuDeTai(int CanBoID, int CoQuanID);
    }
    public class LyLichKhoaHocDAL : ILyLichKhoaHocDAL
    {
        /// <summary>
        /// Lấy danh sách nhà khoa học
        /// </summary>
        /// <param name="p"></param>
        /// <param name="TotalRow"></param>
        /// <param name="QuyenQuanLy"></param>
        /// <param name="Alphabet"></param>
        /// <returns></returns>
        public List<NhaKhoaHocModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow, int QuyenQuanLy, string Alphabet)
        {
            List<NhaKhoaHocModel> Result = new List<NhaKhoaHocModel>();

            SqlParameter[] parameters = new SqlParameter[]
                      {
                        new SqlParameter("@Keyword",SqlDbType.NVarChar),
                        new SqlParameter("@OrderByName",SqlDbType.NVarChar),
                        new SqlParameter("@OrderByOption",SqlDbType.NVarChar),
                        new SqlParameter("@pLimit",SqlDbType.Int),
                        new SqlParameter("@pOffset",SqlDbType.Int),
                        new SqlParameter("@TotalRow",SqlDbType.Int),
                        new SqlParameter("@QuyenQuanLy",SqlDbType.Int),
                        new SqlParameter("@Alphabet",SqlDbType.NVarChar),
                      };
            parameters[0].Value = p.Keyword == null ? "" : p.Keyword.Trim();
            parameters[1].Value = p.OrderByName;
            parameters[2].Value = p.OrderByOption;
            parameters[3].Value = p.Limit;
            parameters[4].Value = p.Offset;
            parameters[5].Direction = ParameterDirection.Output;
            parameters[5].Size = 8;
            parameters[6].Value = QuyenQuanLy;
            parameters[7].Value = Alphabet ?? Convert.DBNull;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_LyLichKhoaHoc_GetPagingBySearch", parameters))
                {
                    while (dr.Read())
                    {
                        NhaKhoaHocModel nhaKhoaHoc = new NhaKhoaHocModel();
                        nhaKhoaHoc.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        nhaKhoaHoc.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        nhaKhoaHoc.MaCB = Utils.ConvertToString(dr["MaCB"], string.Empty);
                        nhaKhoaHoc.NgaySinh = Utils.ConvertToNullableDateTime(dr["NgaySinh"], null);
                        nhaKhoaHoc.GioiTinh = Utils.ConvertToInt32(dr["GioiTinh"], 0);
                        nhaKhoaHoc.CoQuanCongTac = Utils.ConvertToString(dr["CoQuanCongTac"], string.Empty);
                        nhaKhoaHoc.DiaChiCoQuan = Utils.ConvertToString(dr["DiaChiCoQuan"], string.Empty);
                        nhaKhoaHoc.PhongBanID = Utils.ConvertToInt32(dr["PhongBanID"], 0);
                        nhaKhoaHoc.Email = Utils.ConvertToString(dr["Email"], string.Empty);
                        nhaKhoaHoc.DienThoai = Utils.ConvertToString(dr["DienThoai"], string.Empty);
                        nhaKhoaHoc.DienThoaiDiDong = Utils.ConvertToString(dr["DienThoaiDiDong"], string.Empty);
                        nhaKhoaHoc.Fax = Utils.ConvertToString(dr["Fax"], string.Empty);
                        nhaKhoaHoc.TrangThaiID = Utils.ConvertToInt32(dr["TrangThaiID"], 0);
                        nhaKhoaHoc.LaChuyenGia = Utils.ConvertToBoolean(dr["LaChuyenGia"], false);
                        nhaKhoaHoc.Url = Utils.ConvertToString(dr["Url"], string.Empty);
                        nhaKhoaHoc.ChucDanhKhoaHocIDStr = Utils.ConvertToString(dr["ChucDanhKhoaHocIDStr"], string.Empty);
                        nhaKhoaHoc.ChucDanhHanhChinhIDStr = Utils.ConvertToString(dr["ChucDanhHanhChinhIDStr"], string.Empty);
                        nhaKhoaHoc.AnhHoSo = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        nhaKhoaHoc.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        if (nhaKhoaHoc.ChucDanhKhoaHocIDStr != string.Empty)
                        {
                            var cdkh = nhaKhoaHoc.ChucDanhKhoaHocIDStr.Split(',');
                            if (cdkh != null && cdkh.Length > 0)
                            {
                                nhaKhoaHoc.ChucDanhKhoaHoc = new List<int>();
                                foreach (var item in cdkh)
                                {
                                    var id = Utils.ConvertToInt32(item, 0);
                                    if (id > 0) nhaKhoaHoc.ChucDanhKhoaHoc.Add(id);
                                }
                            }
                        }
                        if (nhaKhoaHoc.ChucDanhHanhChinhIDStr != string.Empty)
                        {
                            var cdhc = nhaKhoaHoc.ChucDanhHanhChinhIDStr.Split(',');
                            if (cdhc != null && cdhc.Length > 0)
                            {
                                nhaKhoaHoc.ChucDanhHanhChinh = new List<int>();
                                foreach (var item in cdhc)
                                {
                                    var id = Utils.ConvertToInt32(item, 0);
                                    if (id > 0) nhaKhoaHoc.ChucDanhHanhChinh.Add(id);
                                }
                            }
                        }

                        Result.Add(nhaKhoaHoc);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parameters[5].Value, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        /// <summary>
        /// Thêm mới nhà khoa học
        /// </summary>
        /// <param name="NhaKhoaHocModel"></param>
        /// <returns></returns>
        public BaseResultModel Insert(NhaKhoaHocModel NhaKhoaHocModel)
        {
            var Result = new BaseResultModel();
            string ChucDanhKhoaHocIDStr = "";
            string ChucDanhHanhChinhIDStr = "";
            if (NhaKhoaHocModel.ChucDanhKhoaHoc != null && NhaKhoaHocModel.ChucDanhKhoaHoc.Count > 0)
            {
                foreach (var item in NhaKhoaHocModel.ChucDanhKhoaHoc)
                {
                    ChucDanhKhoaHocIDStr += item.ToString() + ",";
                }
            }
            if (NhaKhoaHocModel.ChucDanhHanhChinh != null && NhaKhoaHocModel.ChucDanhHanhChinh.Count > 0)
            {
                foreach (var item in NhaKhoaHocModel.ChucDanhHanhChinh)
                {
                    ChucDanhHanhChinhIDStr += item.ToString() + ",";
                }
            }
            if (NhaKhoaHocModel.Email == null || NhaKhoaHocModel.Email.Length < 1)
            {
                Result.Status = 0;
                Result.Message = "Email không được trống";
                return Result;
            }
            var crCanBoByEmail = new HeThongCanBoDAL().GetCanBoByEmail(NhaKhoaHocModel.Email.Trim());
            if (crCanBoByEmail != null && crCanBoByEmail.CanBoID > 0)
            {
                Result.Status = 0;
                Result.Message = "Email đã tồn tại trên hệ thống";
                return Result;
            }
            var crNguoiDungByEmail = new HeThongNguoiDungDAL().GetByEmail(NhaKhoaHocModel.Email.Trim());
            if (crNguoiDungByEmail != null && crNguoiDungByEmail.CanBoID > 0)
            {
                Result.Status = 0;
                Result.Message = "Email đã tồn tại trên hệ thống";
                return Result;
            }
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("CanBoID",SqlDbType.Int),
                    new SqlParameter("TenCanBo", SqlDbType.NVarChar),
                    new SqlParameter("MaCB", SqlDbType.NVarChar),
                    new SqlParameter("NgaySinh", SqlDbType.DateTime),
                    new SqlParameter("GioiTinh", SqlDbType.Int),
                    new SqlParameter("CoQuanCongTac", SqlDbType.NVarChar),
                    new SqlParameter("DiaChiCoQuan", SqlDbType.NVarChar),
                    new SqlParameter("PhongBanID",SqlDbType.Int),
                    new SqlParameter("Email", SqlDbType.NVarChar),
                    new SqlParameter("DienThoai", SqlDbType.NVarChar),
                    new SqlParameter("DienThoaiDiDong",SqlDbType.NVarChar),
                    new SqlParameter("Fax",SqlDbType.NVarChar),
                    new SqlParameter("TrangThaiID",SqlDbType.Int),
                    new SqlParameter("LaChuyenGia",SqlDbType.Bit),
                    new SqlParameter("Url",SqlDbType.NVarChar),
                    new SqlParameter("ChucDanhKhoaHocIDStr",SqlDbType.NVarChar),
                    new SqlParameter("ChucDanhHanhChinhIDStr",SqlDbType.NVarChar),
                    new SqlParameter("CoQuanID",SqlDbType.Int),
                };
                parameters[0].Direction = ParameterDirection.Output;
                parameters[0].Size = 8;
                parameters[1].Value = NhaKhoaHocModel.TenCanBo ?? Convert.DBNull;
                parameters[2].Value = NhaKhoaHocModel.MaCB ?? Convert.DBNull;
                parameters[3].Value = NhaKhoaHocModel.NgaySinh ?? Convert.DBNull;
                parameters[4].Value = NhaKhoaHocModel.GioiTinh ?? Convert.DBNull;
                parameters[5].Value = NhaKhoaHocModel.CoQuanCongTac ?? Convert.DBNull;
                parameters[6].Value = NhaKhoaHocModel.DiaChiCoQuan ?? Convert.DBNull;
                parameters[7].Value = NhaKhoaHocModel.PhongBanID ?? Convert.DBNull;
                parameters[8].Value = NhaKhoaHocModel.Email ?? Convert.DBNull;
                parameters[9].Value = NhaKhoaHocModel.DienThoai ?? Convert.DBNull;
                parameters[10].Value = NhaKhoaHocModel.DienThoaiDiDong ?? Convert.DBNull;
                parameters[11].Value = NhaKhoaHocModel.Fax ?? Convert.DBNull;
                parameters[12].Value = NhaKhoaHocModel.TrangThaiID ?? Convert.DBNull;
                parameters[13].Value = NhaKhoaHocModel.LaChuyenGia ?? Convert.DBNull;
                parameters[14].Value = NhaKhoaHocModel.Url ?? Convert.DBNull;
                parameters[15].Value = ChucDanhKhoaHocIDStr ?? Convert.DBNull;
                parameters[16].Value = ChucDanhHanhChinhIDStr ?? Convert.DBNull;
                parameters[17].Value = Utils.ConvertToInt32(new SystemConfigDAL().GetByKey("ID_COQUAN_NGOAITRUONG").ConfigValue, 999999999);


                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_LyLichKhoaHoc_Insert", parameters);
                            int CanBoID = Utils.ConvertToInt32(parameters[0].Value, 0);
                            Result.Data = CanBoID;
                            //URL 
                            SqlParameter[] parms_url = new SqlParameter[]{
                                new SqlParameter("CanBoID", SqlDbType.Int),
                                new SqlParameter("CoQuanID", SqlDbType.Int),
                                new SqlParameter("Url", SqlDbType.NVarChar),
                                new SqlParameter("LoaiThongTin", SqlDbType.Int),
                            };
                            parms_url[0].Value = CanBoID;
                            parms_url[1].Value = Utils.ConvertToInt32(new SystemConfigDAL().GetByKey("ID_COQUAN_NGOAITRUONG").ConfigValue, 999999999);
                            parms_url[2].Value = NhaKhoaHocModel.Url ?? Convert.DBNull;
                            parms_url[3].Value = EnumLoaiThongTinNhaKhoaHoc.Url.GetHashCode();

                            SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_URL_Insert", parms_url);
                            //thêm ngươi giới thiệu
                            if (NhaKhoaHocModel.NguoiGioiThieu != null && NhaKhoaHocModel.NguoiGioiThieu.Count > 0)
                            {
                                foreach (var item in NhaKhoaHocModel.NguoiGioiThieu)
                                {
                                    SqlParameter[] parms_gt = new SqlParameter[]{
                                        new SqlParameter("CanBoID", SqlDbType.Int),
                                        new SqlParameter("CoQuanID", SqlDbType.Int),
                                        new SqlParameter("NguoiGioiThieuID", SqlDbType.Int),
                                        new SqlParameter("CoQuanGioiThieuID", SqlDbType.Int),
                                        new SqlParameter("Link", SqlDbType.NVarChar),
                                    };
                                    parms_gt[0].Value = CanBoID;
                                    parms_gt[1].Value = Utils.ConvertToInt32(new SystemConfigDAL().GetByKey("ID_COQUAN_NGOAITRUONG").ConfigValue, 999999999);
                                    parms_gt[2].Value = item.NguoiGioiThieuID ?? Convert.DBNull;
                                    parms_gt[3].Value = item.CoQuanGioiThieuID ?? Convert.DBNull;
                                    parms_gt[4].Value = item.Link ?? Convert.DBNull;

                                    SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_NguoiGioiThieu_Insert", parms_gt);
                                }
                            }
                            trans.Commit();
                            // thêm người dùng - username = email
                            if (CanBoID > 0)
                            {
                                var mess = "";
                                int NguoiDungID = 0;
                                var nguoiDung = new HeThongNguoiDungModel();
                                nguoiDung.CanBoID = CanBoID;
                                nguoiDung.TenNguoiDung = NhaKhoaHocModel.Email.TrimEnd();
                                var matKhauMacDinh = new SystemConfigDAL().GetByKey("MatKhau_MacDinh").ConfigValue;
                                nguoiDung.MatKhau = Cryptor.EncryptPasswordUser(NhaKhoaHocModel.Email.Trim().ToLower(), matKhauMacDinh ?? "123456");
                                nguoiDung.CanBoID = CanBoID;
                                nguoiDung.TrangThai = 1;
                                var insertNguoiDung = new HeThongNguoiDungDAL().Insert(nguoiDung, ref mess, ref NguoiDungID);
                                if (insertNguoiDung <= 0)
                                {
                                    Result.Status = 0;
                                    Result.Message = "Không thể tạo người dùng";
                                    trans.Rollback();
                                    return Result;
                                }
                                // có thể thêm luôn nhóm người dùng ở đây
                            }
                        }
                        catch (Exception ex)
                        {
                            Result.Status = -1;
                            Result.Message = ConstantLogMessage.API_Error_System;
                            trans.Rollback();
                            throw ex;
                        }
                    }

                    Result.Message = ConstantLogMessage.Alert_Insert_Success("Nhà khoa học");
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
        /// Lấy thông tin chi tiết nhà khoa học
        /// </summary>
        /// <param name="CanBoID"></param>
        /// <returns></returns>
        public NhaKhoaHocModel GetByID(int CanBoID, string serverPath, int? CoQuanID)
        {
            List<NhaKhoaHocModel> Result = new List<NhaKhoaHocModel>();
            List<ThongTinCTNhaKhoaHocModel> thongTinCTNhaKhoaHocs = new List<ThongTinCTNhaKhoaHocModel>();

            var scf = new SystemConfigDAL();
            var cqNgoaiTruongID = Utils.ConvertToInt32(scf.GetByKey("ID_COQUAN_NGOAITRUONG").ConfigValue, 999999999);
            SqlParameter[] parameters = new SqlParameter[]
            {
                  new SqlParameter("@CanBoID",SqlDbType.Int),
                  new SqlParameter("@CoQuanID",SqlDbType.Int),
            };
            parameters[0].Value = CanBoID;
            parameters[1].Value = CoQuanID ?? Convert.DBNull;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_LyLichKhoaHoc_GetByID", parameters))
                {
                    while (dr.Read())
                    {
                        ThongTinCTNhaKhoaHocModel nhaKhoaHoc = new ThongTinCTNhaKhoaHocModel();
                        // thông tin cơ bản
                        nhaKhoaHoc.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        nhaKhoaHoc.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        nhaKhoaHoc.MaCB = Utils.ConvertToString(dr["MaCB"], string.Empty);
                        nhaKhoaHoc.NgaySinh = Utils.ConvertToNullableDateTime(dr["NgaySinh"], null);
                        nhaKhoaHoc.GioiTinh = Utils.ConvertToInt32(dr["GioiTinh"], 0);
                        nhaKhoaHoc.CoQuanCongTac = Utils.ConvertToString(dr["CoQuanCongTac"], string.Empty);
                        nhaKhoaHoc.DiaChiCoQuan = Utils.ConvertToString(dr["DiaChiCoQuan"], string.Empty);
                        nhaKhoaHoc.PhongBanID = Utils.ConvertToInt32(dr["PhongBanID"], 0);
                        nhaKhoaHoc.Email = Utils.ConvertToString(dr["Email"], string.Empty);
                        nhaKhoaHoc.DienThoai = Utils.ConvertToString(dr["DienThoai"], string.Empty);
                        nhaKhoaHoc.DienThoaiDiDong = Utils.ConvertToString(dr["DienThoaiDiDong"], string.Empty);
                        nhaKhoaHoc.Fax = Utils.ConvertToString(dr["Fax"], string.Empty);
                        nhaKhoaHoc.TrangThaiID = Utils.ConvertToInt32(dr["TrangThaiID"], 0);
                        nhaKhoaHoc.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        nhaKhoaHoc.LaChuyenGia = Utils.ConvertToBoolean(dr["LaChuyenGia"], false);
                        nhaKhoaHoc.Url = Utils.ConvertToString(dr["Url"], string.Empty);
                        nhaKhoaHoc.LoaiThongTin = Utils.ConvertToInt32(dr["LoaiThongTin"], 0);
                        nhaKhoaHoc.ChucDanhKhoaHocIDStr = Utils.ConvertToString(dr["ChucDanhKhoaHocIDStr"], string.Empty);
                        nhaKhoaHoc.ChucDanhHanhChinhIDStr = Utils.ConvertToString(dr["ChucDanhHanhChinhIDStr"], string.Empty);
                        if (nhaKhoaHoc.ChucDanhKhoaHocIDStr != string.Empty)
                        {
                            var cdkh = nhaKhoaHoc.ChucDanhKhoaHocIDStr.Split(',');
                            if (cdkh != null && cdkh.Length > 0)
                            {
                                nhaKhoaHoc.ChucDanhKhoaHoc = new List<int>();
                                foreach (var item in cdkh)
                                {
                                    var id = Utils.ConvertToInt32(item, 0);
                                    if (id > 0) nhaKhoaHoc.ChucDanhKhoaHoc.Add(id);
                                }
                            }
                        }
                        if (nhaKhoaHoc.ChucDanhHanhChinhIDStr != string.Empty)
                        {
                            var cdhc = nhaKhoaHoc.ChucDanhHanhChinhIDStr.Split(',');
                            if (cdhc != null && cdhc.Length > 0)
                            {
                                nhaKhoaHoc.ChucDanhHanhChinh = new List<int>();
                                foreach (var item in cdhc)
                                {
                                    var id = Utils.ConvertToInt32(item, 0);
                                    if (id > 0) nhaKhoaHoc.ChucDanhHanhChinh.Add(id);
                                }
                            }
                        }
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
                        //thong tin ct
                        nhaKhoaHoc.CTNhaKhoaHocID = Utils.ConvertToInt32(dr["CTNhaKhoaHocID"], 0);
                        nhaKhoaHoc.KhoangThoiGian = Utils.ConvertToString(dr["KhoangThoiGian"], string.Empty);
                        nhaKhoaHoc.CoSoDaoTao = Utils.ConvertToString(dr["CoSoDaoTao"], string.Empty);
                        nhaKhoaHoc.ChuyenNganh = Utils.ConvertToString(dr["ChuyenNganh"], string.Empty);
                        nhaKhoaHoc.HocVi = Utils.ConvertToString(dr["HocVi"], string.Empty);
                        nhaKhoaHoc.CoQuanCongTac = Utils.ConvertToString(dr["CoQuanCongTacNKH"], string.Empty);
                        nhaKhoaHoc.CoQuanCongTacCT = Utils.ConvertToString(dr["CoQuanCongTacCT"], string.Empty);
                        nhaKhoaHoc.DiaChiDienThoai = Utils.ConvertToString(dr["DiaChiDienThoai"], string.Empty);
                        nhaKhoaHoc.ChucVu = Utils.ConvertToString(dr["ChucVu"], string.Empty);
                        nhaKhoaHoc.TenNgoaiNgu = Utils.ConvertToString(dr["TenNgoaiNgu"], string.Empty);
                        nhaKhoaHoc.Doc = Utils.ConvertToString(dr["Doc"], string.Empty);
                        nhaKhoaHoc.Viet = Utils.ConvertToString(dr["Viet"], string.Empty);
                        nhaKhoaHoc.Noi = Utils.ConvertToString(dr["Noi"], string.Empty);
                        nhaKhoaHoc.TieuDe = Utils.ConvertToString(dr["TieuDe"], string.Empty);
                        nhaKhoaHoc.NgayCap = Utils.ConvertToNullableDateTime(dr["NgayCap"], null);
                        nhaKhoaHoc.SoHieu = Utils.ConvertToString(dr["SoHieu"], string.Empty);
                        nhaKhoaHoc.TrinhDo = Utils.ConvertToString(dr["TrinhDo"], string.Empty);
                        nhaKhoaHoc.NoiCap = Utils.ConvertToString(dr["NoiCapNKH"], string.Empty);
                        nhaKhoaHoc.NoiCapCC = Utils.ConvertToString(dr["NoiCapCC"], string.Empty);
                        nhaKhoaHoc.TenDuAn = Utils.ConvertToString(dr["TenDuAn"], string.Empty);
                        nhaKhoaHoc.CoQuanTaiTro = Utils.ConvertToString(dr["CoQuanTaiTro"], string.Empty);
                        nhaKhoaHoc.VaiTroThamGia = Utils.ConvertToString(dr["VaiTroThamGia"], string.Empty);
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
                        //hoạt động khoa học
                        nhaKhoaHoc.HoatDongKhoaHocID = Utils.ConvertToInt32(dr["HoatDongKhoaHocID"], 0);
                        nhaKhoaHoc.NhiemVu = Utils.ConvertToInt32(dr["NhiemVu"], 0);
                        nhaKhoaHoc.HoatDongKH = Utils.ConvertToString(dr["HoatDongKhoaHoc"], string.Empty);
                        nhaKhoaHoc.PublicCV = Utils.ConvertToBoolean(dr["PublicCV"], false);
                        nhaKhoaHoc.NamThucHien = Utils.ConvertToInt32(dr["NamThucHien"], 0);
                        
                        //người giới thiệu
                        nhaKhoaHoc.ID = Utils.ConvertToInt32(dr["ID"], 0);
                        nhaKhoaHoc.NguoiGioiThieuID = Utils.ConvertToInt32(dr["NguoiGioiThieuID"], 0);
                        nhaKhoaHoc.CoQuanGioiThieuID = Utils.ConvertToInt32(dr["CoQuanGioiThieuID"], 0);
                        nhaKhoaHoc.TenNguoiGioiThieu = Utils.ConvertToString(dr["TenNguoiGioiThieu"], string.Empty);
                        nhaKhoaHoc.Link = Utils.ConvertToString(dr["Link"], string.Empty);
                        //tac gia
                        nhaKhoaHoc.TacGiaID = Utils.ConvertToInt32(dr["TacGiaID"], 0);
                        nhaKhoaHoc.CanBoTGID = Utils.ConvertToInt32(dr["CanBoTGID"], 0);
                        nhaKhoaHoc.CoQuanTGID = Utils.ConvertToInt32(dr["CoQuanTGID"], 0);
                        nhaKhoaHoc.TenTacGia = Utils.ConvertToString(dr["TenTacGia"], string.Empty);

                        thongTinCTNhaKhoaHocs.Add(nhaKhoaHoc);
                    }
                    dr.Close();
                }

                if (thongTinCTNhaKhoaHocs.Count > 0)
                {
                    Result = (from m in thongTinCTNhaKhoaHocs
                              group m by m.CanBoID into ctt
                              from item in ctt
                              select new NhaKhoaHocModel()
                              {
                                  CanBoID = item.CanBoID,
                                  TenCanBo = item.TenCanBo,
                                  MaCB = item.MaCB,
                                  NgaySinh = item.NgaySinh,
                                  GioiTinh = item.GioiTinh,
                                  CoQuanCongTac = item.CoQuanCongTac,
                                  DiaChiCoQuan = item.DiaChiCoQuan,
                                  PhongBanID = item.PhongBanID,
                                  ChucDanhKhoaHocIDStr = item.ChucDanhKhoaHocIDStr,
                                  ChucDanhKhoaHoc = item.ChucDanhKhoaHoc,
                                  ChucDanhHanhChinhIDStr = item.ChucDanhHanhChinhIDStr,
                                  ChucDanhHanhChinh = item.ChucDanhHanhChinh,
                                  Email = item.Email,
                                  DienThoai = item.DienThoai,
                                  DienThoaiDiDong = item.DienThoaiDiDong,
                                  Fax = item.Fax,
                                  TrangThaiID = item.TrangThaiID,
                                  LaChuyenGia = item.LaChuyenGia,
                                  Url = item.Url,
                                  CoQuanID = item.CoQuanID,
                                  LaCanBoTrongTruong = (item.CoQuanID == 0 || item.CoQuanID == cqNgoaiTruongID) ? false : true,
                                  AnhHoSo = (from i in thongTinCTNhaKhoaHocs.Where(x => x.LoaiFile == EnumLoaiFileDinhKem.AnhDaiDien.GetHashCode())
                                             select i.FileUrl).FirstOrDefault()
                                            ,
                                  FileDinhKem = (from i in thongTinCTNhaKhoaHocs.Where(x => x.FileDinhKemID > 0 && x.LoaiFile == EnumLoaiFileDinhKem.LyLich.GetHashCode()).ToList().GroupBy(x => x.FileDinhKemID)
                                                 select new FileDinhKemModel()
                                                 {
                                                     NghiepVuID = i.FirstOrDefault().CanBoID,
                                                     FileDinhKemID = i.FirstOrDefault().FileDinhKemID,
                                                     TenFileHeThong = i.FirstOrDefault().TenFileHeThong,
                                                     TenFileGoc = i.FirstOrDefault().TenFileGoc,
                                                     LoaiFile = i.FirstOrDefault().LoaiFile,
                                                     FileUrl = i.FirstOrDefault().FileUrl,
                                                     NoiDung = i.FirstOrDefault().NoiDungFile,
                                                     NgayTao = i.FirstOrDefault().NgayTao,
                                                     NguoiTaoID = i.FirstOrDefault().NguoiTaoID,
                                                 }
                                                         ).ToList(),
                                  NguoiGioiThieu = (from i in thongTinCTNhaKhoaHocs.Where(x => x.ID > 0).ToList().GroupBy(x => x.ID)
                                                    select new NguoiGioiThieuModel()
                                                    {
                                                        ID = i.FirstOrDefault().ID,
                                                        NguoiGioiThieuID = i.FirstOrDefault().NguoiGioiThieuID,
                                                        CoQuanGioiThieuID = i.FirstOrDefault().CoQuanGioiThieuID,
                                                        TenNguoiGioiThieu = i.FirstOrDefault().TenNguoiGioiThieu,
                                                        Link = i.FirstOrDefault().Link,
                                                    }
                                                         ).ToList(),
                                  QuaTrinhDaoTao = (from i in thongTinCTNhaKhoaHocs.Where(x => x.CTNhaKhoaHocID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.QuaTrinhDaoTao).ToList().GroupBy(x => x.CTNhaKhoaHocID)
                                                    select new QuaTrinhDaoTaoModel()
                                                    {
                                                        CTNhaKhoaHocID = i.FirstOrDefault().CTNhaKhoaHocID,
                                                        KhoangThoiGian = i.FirstOrDefault().KhoangThoiGian,
                                                        CoSoDaoTao = i.FirstOrDefault().CoSoDaoTao,
                                                        ChuyenNganh = i.FirstOrDefault().ChuyenNganh,
                                                        HocVi = i.FirstOrDefault().HocVi,
                                                    }
                                                         ).ToList(),
                                  QuaTrinhCongTac = (from i in thongTinCTNhaKhoaHocs.Where(x => x.CTNhaKhoaHocID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.QuaTrinhCongTac).ToList().GroupBy(x => x.CTNhaKhoaHocID)
                                                     select new QuaTrinhCongTacModel()
                                                     {
                                                         CTNhaKhoaHocID = i.FirstOrDefault().CTNhaKhoaHocID,
                                                         CoQuanCongTac = i.FirstOrDefault().CoQuanCongTacCT,
                                                         DiaChiDienThoai = i.FirstOrDefault().DiaChiDienThoai,
                                                         KhoangThoiGian = i.FirstOrDefault().KhoangThoiGian,
                                                         ChucVu = i.FirstOrDefault().ChucVu,
                                                     }
                                                         ).ToList(),
                                  NgoaiNgu = (from i in thongTinCTNhaKhoaHocs.Where(x => x.CTNhaKhoaHocID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.NgoaiNgu).ToList().GroupBy(x => x.CTNhaKhoaHocID)
                                              select new NgoaiNguModel()
                                              {
                                                  CTNhaKhoaHocID = i.FirstOrDefault().CTNhaKhoaHocID,
                                                  TenNgoaiNgu = i.FirstOrDefault().TenNgoaiNgu,
                                                  Doc = i.FirstOrDefault().Doc,
                                                  Viet = i.FirstOrDefault().Viet,
                                                  Noi = i.FirstOrDefault().Noi,
                                              }
                                                         ).ToList(),
                                  VanBangChungChi = (from i in thongTinCTNhaKhoaHocs.Where(x => x.CTNhaKhoaHocID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.VanBangChungChi).ToList().GroupBy(x => x.CTNhaKhoaHocID)
                                                     select new VanBangChungChiModel()
                                                     {
                                                         CTNhaKhoaHocID = i.FirstOrDefault().CTNhaKhoaHocID,
                                                         TieuDe = i.FirstOrDefault().TieuDe,
                                                         NgayCap = i.FirstOrDefault().NgayCap,
                                                         SoHieu = i.FirstOrDefault().SoHieu,
                                                         TrinhDo = i.FirstOrDefault().TrinhDo,
                                                         NoiCap = i.FirstOrDefault().NoiCapCC,
                                                     }
                                                         ).ToList(),
                                  GiaiThuongKhoaHoc = (from i in thongTinCTNhaKhoaHocs.Where(x => x.CTNhaKhoaHocID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.GiaiThuongKhoaHoc).ToList().GroupBy(x => x.CTNhaKhoaHocID)
                                                       select new GiaiThuongKhoaHocModel()
                                                       {
                                                           CTNhaKhoaHocID = i.FirstOrDefault().CTNhaKhoaHocID,
                                                           TieuDe = i.FirstOrDefault().TieuDe,
                                                           KhoangThoiGian = i.FirstOrDefault().KhoangThoiGian,
                                                       }
                                                         ).ToList(),
                                  HuongNghienCuuChinh = (from i in thongTinCTNhaKhoaHocs.Where(x => x.CTNhaKhoaHocID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.HuongNghienCuuChinh).ToList().GroupBy(x => x.CTNhaKhoaHocID)
                                                         select new HuongNghienCuuChinhModel()
                                                         {
                                                             CTNhaKhoaHocID = i.FirstOrDefault().CTNhaKhoaHocID,
                                                             TenHuongNghienCuuChinh = i.FirstOrDefault().TenHuongNghienCuuChinh
                                                         }
                                                         ).FirstOrDefault(),

                                  DuAnDeTai = (from i in thongTinCTNhaKhoaHocs.Where(x => x.CTNhaKhoaHocID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.DuAnDeTai).ToList().GroupBy(x => x.CTNhaKhoaHocID)
                                               select new DuAnDeTaiModel()
                                               {
                                                   CTNhaKhoaHocID = i.FirstOrDefault().CTNhaKhoaHocID,
                                                   TenDuAn = i.FirstOrDefault().TenDuAn,
                                                   KhoangThoiGian = i.FirstOrDefault().KhoangThoiGian,
                                                   CoQuanTaiTro = i.FirstOrDefault().CoQuanTaiTro,
                                                   VaiTroThamGia = i.FirstOrDefault().VaiTroThamGia,
                                               }
                                                         ).ToList(),
                                  BaiBaoTapChi = (from i in thongTinCTNhaKhoaHocs.Where(x => x.CTNhaKhoaHocID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.BaiBaoTapChi).ToList().GroupBy(x => x.CTNhaKhoaHocID)
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
                                                      FileDinhKem = new List<FileDinhKemModel>(),
                                                      ListTacGia = ((from j in thongTinCTNhaKhoaHocs.Where(x => x.CTNhaKhoaHocID > 0 && x.TacGiaID > 0 && x.CTNhaKhoaHocID == i.FirstOrDefault().CTNhaKhoaHocID).ToList().GroupBy(x => x.TacGiaID)
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
                                  BaoCaoKhoaHoc = (from i in thongTinCTNhaKhoaHocs.Where(x => x.CTNhaKhoaHocID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.BaoCaoKhoaHoc).ToList().GroupBy(x => x.CTNhaKhoaHocID)
                                                   select new BaoCaoKhoaHocModel()
                                                   {
                                                       CTNhaKhoaHocID = i.FirstOrDefault().CTNhaKhoaHocID,
                                                       TenHoiThao = i.FirstOrDefault().TenHoiThao,
                                                       KhoangThoiGian = i.FirstOrDefault().KhoangThoiGian,
                                                       TieuDe = i.FirstOrDefault().TieuDe,
                                                       ListTacGia = ((from j in thongTinCTNhaKhoaHocs.Where(x => x.CTNhaKhoaHocID > 0 && x.TacGiaID > 0 && x.CTNhaKhoaHocID == i.FirstOrDefault().CTNhaKhoaHocID).ToList().GroupBy(x => x.TacGiaID)
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
                                  KetQuaNghienCuu = (from i in thongTinCTNhaKhoaHocs.Where(x => x.CTNhaKhoaHocID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.KetQuaNghienCuu).ToList().GroupBy(x => x.CTNhaKhoaHocID)
                                                     select new KetQuaNghienCuuNKHModel()
                                                     {
                                                         CTNhaKhoaHocID = i.FirstOrDefault().CTNhaKhoaHocID,
                                                         DeTai = i.FirstOrDefault().DeTai,
                                                         LoaiNhiemVu = i.FirstOrDefault().LoaiNhiemVu,
                                                         NhiemVu = i.FirstOrDefault().NhiemVu,
                                                         TieuDe = i.FirstOrDefault().TieuDe,
                                                         NamXuatBan = i.FirstOrDefault().NamXuatBan,
                                                         GhiChu = i.FirstOrDefault().GhiChu,
                                                         //TacGia = i.FirstOrDefault().TacGia,
                                                         //TenTapChiSachHoiThao = i.FirstOrDefault().TenTapChiSachHoiThao,
                                                         //So = i.FirstOrDefault().So,
                                                         //Trang = i.FirstOrDefault().Trang,
                                                         //NhaXuatBan = i.FirstOrDefault().NhaXuatBan,
                                                         FileDinhKem = new List<FileDinhKemModel>(),
                                                         ListTacGia = ((from j in thongTinCTNhaKhoaHocs.Where(x => x.CTNhaKhoaHocID > 0 && x.TacGiaID > 0 && x.CTNhaKhoaHocID == i.FirstOrDefault().CTNhaKhoaHocID).ToList().GroupBy(x => x.TacGiaID)
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
                                  SachChuyenKhao = (from i in thongTinCTNhaKhoaHocs.Where(x => x.CTNhaKhoaHocID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.SachChuyenKhao).ToList().GroupBy(x => x.CTNhaKhoaHocID)
                                                    select new SachChuyenKhaoModel()
                                                    {
                                                        CTNhaKhoaHocID = i.FirstOrDefault().CTNhaKhoaHocID,
                                                        DeTai = i.FirstOrDefault().DeTai,
                                                        LoaiNhiemVu = i.FirstOrDefault().LoaiNhiemVu,
                                                        NhiemVu = i.FirstOrDefault().NhiemVu,
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
                                                        FileDinhKem = new List<FileDinhKemModel>(),
                                                        ListTacGia = ((from j in thongTinCTNhaKhoaHocs.Where(x => x.CTNhaKhoaHocID > 0 && x.TacGiaID > 0 && x.CTNhaKhoaHocID == i.FirstOrDefault().CTNhaKhoaHocID).ToList().GroupBy(x => x.TacGiaID)
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
                                  CacMonGiangDay = (from i in thongTinCTNhaKhoaHocs.Where(x => x.CTNhaKhoaHocID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.CacMonGiangDay).ToList().GroupBy(x => x.CTNhaKhoaHocID)
                                                    select new CacMonGiangDayModel()
                                                    {
                                                        CTNhaKhoaHocID = i.FirstOrDefault().CTNhaKhoaHocID,
                                                        TieuDe = i.FirstOrDefault().TieuDe,
                                                        DeCuong = i.FirstOrDefault().DeCuong,
                                                        FileDinhKem = new List<FileDinhKemModel>(),
                                                    }
                                                         ).ToList(),
                                  HoatDongKhoaHoc = (from i in thongTinCTNhaKhoaHocs.Where(x => x.HoatDongKhoaHocID > 0).ToList().GroupBy(x => x.HoatDongKhoaHocID)
                                                     select new HoatDongKhoaHocModel()
                                                     {
                                                         HoatDongKhoaHocID = i.FirstOrDefault().HoatDongKhoaHocID,
                                                         CanBoID = i.FirstOrDefault().CanBoID,
                                                         NhiemVu = i.FirstOrDefault().NhiemVu,
                                                         HoatDongKhoaHoc = i.FirstOrDefault().HoatDongKH,
                                                         PublicCV = i.FirstOrDefault().PublicCV,
                                                         FileDinhKem = new List<FileDinhKemModel>(),
                                                     }
                                                         ).ToList(),
                                  SanPhamDaoTao = (from i in thongTinCTNhaKhoaHocs.Where(x => x.CTNhaKhoaHocID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.SanPhamDaoTao).ToList().GroupBy(x => x.CTNhaKhoaHocID)
                                                   select new SanPhamDaoTaoModel()
                                                   {
                                                       CTNhaKhoaHocID = i.FirstOrDefault().CTNhaKhoaHocID,
                                                       DeTai = i.FirstOrDefault().DeTai,
                                                       NhiemVu = i.FirstOrDefault().NhiemVu,
                                                       LoaiNhiemVu = i.FirstOrDefault().LoaiNhiemVu,
                                                       LoaiDaoTao = i.FirstOrDefault().LoaiDaoTao,
                                                       TenHocVien = i.FirstOrDefault().TenHocVien,
                                                       TenLuanVan = i.FirstOrDefault().TenLuanVan,
                                                       NguoiHuongDan = i.FirstOrDefault().NguoiHuongDan,
                                                       NamBaoVe = i.FirstOrDefault().NamBaoVe,
                                                       CoSoDaoTao = i.FirstOrDefault().CoSoDaoTao,
                                                       KhoangThoiGian = i.FirstOrDefault().KhoangThoiGian,
                                                       CapHoiThao = i.FirstOrDefault().CapHoiThao
                                                   }
                                                         ).ToList(),
                                  //SanPhamKhac = (from i in thongTinCTNhaKhoaHocs.Where(x => x.CTNhaKhoaHocID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.SanPhamKhac).ToList().GroupBy(x => x.CTNhaKhoaHocID)
                                  //                 select new SanPhamKhacModel()
                                  //                 {
                                  //                     CTNhaKhoaHocID = i.FirstOrDefault().CTNhaKhoaHocID,
                                  //                     DeTai = i.FirstOrDefault().DeTai,
                                  //                     NhiemVu = i.FirstOrDefault().NhiemVu,
                                  //                     LoaiNhiemVu = i.FirstOrDefault().LoaiNhiemVu,
                                  //                     TieuDe = i.FirstOrDefault().TieuDe,
                                  //                     KhoangThoiGian = i.FirstOrDefault().KhoangThoiGian,
                                  //                     GhiChu = i.FirstOrDefault().GhiChu,
                                  //                     ListTacGia = ((from j in thongTinCTNhaKhoaHocs.Where(x => x.CTNhaKhoaHocID > 0 && x.TacGiaID > 0 && x.CTNhaKhoaHocID == i.FirstOrDefault().CTNhaKhoaHocID).ToList().GroupBy(x => x.TacGiaID)
                                  //                                    select new TacGiaModel()
                                  //                                    {
                                  //                                        TacGiaID = j.FirstOrDefault().TacGiaID,
                                  //                                        CTNhaKhoaHocID = j.FirstOrDefault().CTNhaKhoaHocID,
                                  //                                        TenTacGia = j.FirstOrDefault().TenTacGia,
                                  //                                        CanBoID = j.FirstOrDefault().CanBoTGID,
                                  //                                        CoQuanID = j.FirstOrDefault().CoQuanTGID,
                                  //                                    }
                                  //                     ).ToList())
                                  //                 }
                                  //                       ).ToList(),
                              }
                            ).ToList();
                }
                var data = new NhaKhoaHocModel();
                data = Result.FirstOrDefault() ?? new NhaKhoaHocModel();
                if (data != null && data.FileDinhKem != null && data.FileDinhKem.Count > 0)
                {
                    foreach (var item in data.FileDinhKem)
                    {
                        item.FileUrl = item.FileUrl.Replace(@"\\", @"\");
                        item.FileUrl = serverPath + item.FileUrl;
                    }
                }

                var files = GetAllFile(data.CanBoID);
                if (files.Count > 0)
                {
                    foreach (var item in files)
                    {
                        item.FileUrl = item.FileUrl.Replace(@"\\", @"\");
                        item.FileUrl = serverPath + item.FileUrl;

                        if (item.LoaiFile == (int)EnumLoaiFileDinhKem.BaiBaoTapChi)
                        {
                            foreach (var info in data.BaiBaoTapChi)
                            {
                                if (item.NghiepVuID == info.CTNhaKhoaHocID) info.FileDinhKem.Add(item);
                            }
                        }
                        if (item.LoaiFile == (int)EnumLoaiFileDinhKem.KetQuaNghienCuu)
                        {
                            foreach (var info in data.KetQuaNghienCuu)
                            {
                                if (item.NghiepVuID == info.CTNhaKhoaHocID) info.FileDinhKem.Add(item);
                            }
                        }
                        if (item.LoaiFile == (int)EnumLoaiFileDinhKem.SachChuyenKhao)
                        {
                            foreach (var info in data.SachChuyenKhao)
                            {
                                if (item.NghiepVuID == info.CTNhaKhoaHocID) info.FileDinhKem.Add(item);
                            }
                        }
                        if (item.LoaiFile == (int)EnumLoaiFileDinhKem.CacMonGiangDay)
                        {
                            foreach (var info in data.CacMonGiangDay)
                            {
                                if (item.NghiepVuID == info.CTNhaKhoaHocID) info.FileDinhKem.Add(item);
                            }
                        }
                        if (item.LoaiFile == (int)EnumLoaiFileDinhKem.HoatDongKhoaHoc)
                        {
                            foreach (var info in data.HoatDongKhoaHoc)
                            {
                                if (item.NghiepVuID == info.HoatDongKhoaHocID) info.FileDinhKem.Add(item);
                            }
                        }
                    }
                }
                #region old
                //var listCT = GetChiTietNhaKhoaHoc(data.CanBoID, data.CoQuanID);
                //if (listCT.Count > 0)
                //{
                //    foreach (var item in listCT)
                //    {
                //        if (item.LoaiThongTin == EnumLoaiThongTin.BaiBaoTapChi.GetHashCode() || item.LoaiThongTin == EnumLoaiThongTinNhaKhoaHoc.BaiBaoTapChi.GetHashCode())
                //        {
                //            BaiBaoTapChiModel bb = new BaiBaoTapChiModel();
                //            bb.CTNhaKhoaHocID = item.CTNhaKhoaHocID;
                //            bb.ChiTietDeTaiID = item.ChiTietDeTaiID;
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
                //        else if (item.LoaiThongTin == EnumLoaiThongTin.SachChuyenKhao.GetHashCode() || item.LoaiThongTin == EnumLoaiThongTinNhaKhoaHoc.SachChuyenKhao.GetHashCode())
                //        {
                //            SachChuyenKhaoModel sck = new SachChuyenKhaoModel();
                //            sck.CTNhaKhoaHocID = item.CTNhaKhoaHocID;
                //            sck.ChiTietDeTaiID = item.ChiTietDeTaiID;
                //            sck.DeTai = item.DeTai;
                //            sck.KhoangThoiGian = item.KhoangThoiGian;
                //            sck.TieuDe = item.TieuDe;
                //            sck.TenTapChiSachHoiThao = item.TenTapChiSachHoiThao;
                //            sck.So = item.So;
                //            sck.Trang = item.Trang;
                //            sck.NhaXuatBan = item.NhaXuatBan;
                //            sck.FileDinhKem = new List<FileDinhKemModel>();
                //            sck.ListTacGia = ((from j in thongTinCTNhaKhoaHocs.Where(x => x.CTNhaKhoaHocID > 0 && x.TacGiaID > 0 && x.CTNhaKhoaHocID == item.CTNhaKhoaHocID).ToList().GroupBy(x => x.TacGiaID)
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
                //        else if (item.LoaiThongTin == EnumLoaiThongTin.SanPhamDaoTao.GetHashCode() || item.LoaiThongTin == EnumLoaiThongTinNhaKhoaHoc.SanPhamDaoTao.GetHashCode())
                //        {
                //            SanPhamDaoTaoModel sp = new SanPhamDaoTaoModel();
                //            sp.CTNhaKhoaHocID = item.CTNhaKhoaHocID;
                //            sp.ChiTietDeTaiID = item.ChiTietDeTaiID;
                //            sp.DeTai = item.DeTai;
                //            sp.NhiemVu = item.NhiemVu;
                //            sp.LoaiNhiemVu = item.LoaiNhiemVu;
                //            sp.LoaiDaoTao = item.LoaiDaoTao;
                //            sp.TenHocVien = item.TenHocVien;
                //            sp.TenLuanVan = item.TenLuanVan;
                //            sp.NguoiHuongDan = item.NguoiHuongDan;
                //            sp.NamBaoVe = item.NamBaoVe;

                //            data.SanPhamDaoTao.Add(sp);
                //        }
                //    }
                //}
                #endregion
  
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Chỉnh sửa thông tin chi tiết nhà khoa học
        /// </summary>
        /// <param name="ThongTinChiTiet"></param>
        /// <returns></returns>
        public BaseResultModel Edit_ThongTinChiTiet(ThongTinCTNhaKhoaHocModel ThongTinChiTiet)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parms = new SqlParameter[]{
                    new SqlParameter("CTNhaKhoaHocID", SqlDbType.Int),          //0
                    new SqlParameter("CanBoID", SqlDbType.Int),                 //1
                    new SqlParameter("KhoangThoiGian", SqlDbType.NVarChar),     //2
                    new SqlParameter("CoSoDaoTao", SqlDbType.NVarChar),         //3
                    new SqlParameter("ChuyenNganh", SqlDbType.NVarChar),        //4
                    new SqlParameter("HocVi", SqlDbType.NVarChar),              //5
                    new SqlParameter("CoQuanCongTac", SqlDbType.NVarChar),      //6
                    new SqlParameter("DiaChiDienThoai", SqlDbType.NVarChar),    //7
                    new SqlParameter("ChucVu", SqlDbType.NVarChar),             //8
                    new SqlParameter("TenNgoaiNgu", SqlDbType.NVarChar),        //9
                    new SqlParameter("Doc", SqlDbType.NVarChar),                //10
                    new SqlParameter("Viet", SqlDbType.NVarChar),               //11
                    new SqlParameter("Noi", SqlDbType.NVarChar),                //12
                    new SqlParameter("TieuDe", SqlDbType.NVarChar),             //13
                    new SqlParameter("NgayCap", SqlDbType.DateTime),            //14
                    new SqlParameter("SoHieu", SqlDbType.NVarChar),             //15
                    new SqlParameter("TrinhDo", SqlDbType.NVarChar),            //16
                    new SqlParameter("NoiCap", SqlDbType.NVarChar),             //17
                    new SqlParameter("TenDuAn", SqlDbType.NVarChar),            //18
                    new SqlParameter("CoQuanTaiTro", SqlDbType.NVarChar),       //19
                    new SqlParameter("VaiTroThamGia", SqlDbType.NVarChar),      //20
                    new SqlParameter("TacGia", SqlDbType.NVarChar),             //21
                    new SqlParameter("TenTapChiSachHoiThao", SqlDbType.NVarChar),//22
                    new SqlParameter("So", SqlDbType.NVarChar),                 //23
                    new SqlParameter("Trang", SqlDbType.NVarChar),              //24
                    new SqlParameter("NhaXuatBan", SqlDbType.NVarChar),         //25
                    new SqlParameter("DeCuong", SqlDbType.NVarChar),            //26
                    new SqlParameter("DeTai", SqlDbType.Int),                   //27
                    new SqlParameter("LoaiThongTin", SqlDbType.Int),            //28
                    new SqlParameter("HuongNghienCuuChinh", SqlDbType.NVarChar),//29
                    new SqlParameter("CoQuanID", SqlDbType.Int),                //30
                    //bổ sung lần 1
                    new SqlParameter("LoaiBaiBao", SqlDbType.Int),              //31
                    new SqlParameter("ISSN", SqlDbType.NVarChar),               //32
                    new SqlParameter("NhiemVu", SqlDbType.Int),                 //33
                    new SqlParameter("LoaiNhiemVu", SqlDbType.Int),             //34
                    new SqlParameter("TenHoiThao", SqlDbType.NVarChar),         //35
                    //bổ sung lần 2
                    new SqlParameter("Tap", SqlDbType.NVarChar),                //36
                    new SqlParameter("NamDangTai", SqlDbType.Int),              //37
                    new SqlParameter("LinkBaiBao", SqlDbType.NVarChar),         //38
                    new SqlParameter("LinhVucNganhKhoaHoc", SqlDbType.Int),     //39
                    new SqlParameter("HeSoAnhHuong", SqlDbType.NVarChar),       //40
                    new SqlParameter("ChiSo", SqlDbType.Int),                   //41
                    new SqlParameter("RankSCIMAG", SqlDbType.Int),              //42
                    new SqlParameter("DiemTapChi", SqlDbType.Decimal),          //43
                    new SqlParameter("CapHoiThao", SqlDbType.Int),              //44
                    new SqlParameter("NgayHoiThao", SqlDbType.DateTime),        //45
                    new SqlParameter("DiaDiemToChuc", SqlDbType.NVarChar),      //46
                    new SqlParameter("LoaiDaoTao", SqlDbType.Int),              //47
                    new SqlParameter("TenHocVien", SqlDbType.NVarChar),         //48
                    new SqlParameter("TenLuanVan", SqlDbType.NVarChar),         //49
                    new SqlParameter("NguoiHuongDan", SqlDbType.NVarChar),      //50
                    new SqlParameter("NamBaoVe", SqlDbType.NVarChar),           //51
                    new SqlParameter("GhiChu", SqlDbType.NVarChar),             //52
                    new SqlParameter("ChuBienID", SqlDbType.NVarChar),          //53
                    new SqlParameter("CoQuanChuBienID", SqlDbType.NVarChar),    //54
                    new SqlParameter("NamXuatBan", SqlDbType.Int),              //55
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

                if (ThongTinChiTiet.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.QuaTrinhDaoTao)
                {
                    parms[0].Value = ThongTinChiTiet.CTNhaKhoaHocID ?? Convert.DBNull;
                    parms[1].Value = ThongTinChiTiet.CanBoID;
                    parms[2].Value = ThongTinChiTiet.KhoangThoiGian ?? Convert.DBNull;
                    parms[3].Value = ThongTinChiTiet.CoSoDaoTao ?? Convert.DBNull;
                    parms[4].Value = ThongTinChiTiet.ChuyenNganh ?? Convert.DBNull;
                    parms[5].Value = ThongTinChiTiet.HocVi ?? Convert.DBNull;
                    //parms[6].Value = DBNull.Value;
                    //parms[7].Value = DBNull.Value;
                    //parms[8].Value = DBNull.Value;
                    //parms[9].Value = DBNull.Value;
                    //parms[10].Value = DBNull.Value;
                    //parms[11].Value = DBNull.Value;
                    //parms[12].Value = DBNull.Value;
                    //parms[13].Value = DBNull.Value;
                    //parms[14].Value = DBNull.Value;
                    //parms[15].Value = DBNull.Value;
                    //parms[16].Value = DBNull.Value;
                    //parms[17].Value = DBNull.Value;
                    //parms[18].Value = DBNull.Value;
                    //parms[19].Value = DBNull.Value;
                    //parms[20].Value = DBNull.Value;
                    //parms[21].Value = DBNull.Value;
                    //parms[22].Value = DBNull.Value;
                    //parms[23].Value = DBNull.Value;
                    //parms[24].Value = DBNull.Value;
                    //parms[25].Value = DBNull.Value;
                    //parms[26].Value = DBNull.Value;
                    //parms[27].Value = DBNull.Value;
                    parms[28].Value = (int)EnumLoaiThongTinNhaKhoaHoc.QuaTrinhDaoTao;
                    //parms[29].Value = DBNull.Value;
                    parms[30].Value = ThongTinChiTiet.CoQuanID;
                }
                else if (ThongTinChiTiet.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.QuaTrinhCongTac)
                {
                    parms[0].Value = ThongTinChiTiet.CTNhaKhoaHocID ?? Convert.DBNull;
                    parms[1].Value = ThongTinChiTiet.CanBoID;
                    parms[2].Value = ThongTinChiTiet.KhoangThoiGian ?? Convert.DBNull;
                    //parms[3].Value = DBNull.Value;
                    //parms[4].Value = DBNull.Value;
                    parms[5].Value = DBNull.Value;
                    parms[6].Value = ThongTinChiTiet.CoQuanCongTac ?? Convert.DBNull;
                    parms[7].Value = ThongTinChiTiet.DiaChiDienThoai ?? Convert.DBNull;
                    parms[8].Value = ThongTinChiTiet.ChucVu ?? Convert.DBNull;
                    //parms[9].Value = DBNull.Value;
                    //parms[10].Value = DBNull.Value;
                    //parms[11].Value = DBNull.Value;
                    //parms[12].Value = DBNull.Value;
                    //parms[13].Value = DBNull.Value;
                    //parms[14].Value = DBNull.Value;
                    //parms[15].Value = DBNull.Value;
                    //parms[16].Value = DBNull.Value;
                    //parms[17].Value = DBNull.Value;
                    //parms[18].Value = DBNull.Value;
                    //parms[19].Value = DBNull.Value;
                    //parms[20].Value = DBNull.Value;
                    //parms[21].Value = DBNull.Value;
                    //parms[22].Value = DBNull.Value;
                    //parms[23].Value = DBNull.Value;
                    //parms[24].Value = DBNull.Value;
                    //parms[25].Value = DBNull.Value;
                    //parms[26].Value = DBNull.Value;
                    //parms[27].Value = DBNull.Value;
                    parms[28].Value = (int)EnumLoaiThongTinNhaKhoaHoc.QuaTrinhCongTac;
                    //parms[29].Value = DBNull.Value;
                    parms[30].Value = ThongTinChiTiet.CoQuanID;
                }
                else if (ThongTinChiTiet.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.NgoaiNgu)
                {
                    parms[0].Value = ThongTinChiTiet.CTNhaKhoaHocID ?? Convert.DBNull;
                    parms[1].Value = ThongTinChiTiet.CanBoID;
                    //parms[2].Value = DBNull.Value;
                    //parms[3].Value = DBNull.Value;
                    //parms[4].Value = DBNull.Value;
                    //parms[5].Value = DBNull.Value;
                    //parms[6].Value = DBNull.Value;
                    //parms[7].Value = DBNull.Value;
                    //parms[8].Value = DBNull.Value;
                    parms[9].Value = ThongTinChiTiet.TenNgoaiNgu ?? Convert.DBNull;
                    parms[10].Value = ThongTinChiTiet.Doc ?? Convert.DBNull;
                    parms[11].Value = ThongTinChiTiet.Viet ?? Convert.DBNull;
                    parms[12].Value = ThongTinChiTiet.Noi ?? Convert.DBNull;
                    //parms[13].Value = DBNull.Value;
                    //parms[14].Value = DBNull.Value;
                    //parms[15].Value = DBNull.Value;
                    //parms[16].Value = DBNull.Value;
                    //parms[17].Value = DBNull.Value;
                    //parms[18].Value = DBNull.Value;
                    //parms[19].Value = DBNull.Value;
                    //parms[20].Value = DBNull.Value;
                    //parms[21].Value = DBNull.Value;
                    //parms[22].Value = DBNull.Value;
                    //parms[23].Value = DBNull.Value;
                    //parms[24].Value = DBNull.Value;
                    //parms[25].Value = DBNull.Value;
                    //parms[26].Value = DBNull.Value;
                    //parms[27].Value = DBNull.Value;
                    parms[28].Value = (int)EnumLoaiThongTinNhaKhoaHoc.NgoaiNgu;
                    //parms[29].Value = DBNull.Value;
                    parms[30].Value = ThongTinChiTiet.CoQuanID;
                }
                else if (ThongTinChiTiet.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.VanBangChungChi)
                {
                    parms[0].Value = ThongTinChiTiet.CTNhaKhoaHocID ?? Convert.DBNull;
                    parms[1].Value = ThongTinChiTiet.CanBoID;
                    //parms[2].Value = DBNull.Value;
                    //parms[3].Value = DBNull.Value;
                    //parms[4].Value = DBNull.Value;
                    //parms[5].Value = DBNull.Value;
                    //parms[6].Value = DBNull.Value;
                    //parms[7].Value = DBNull.Value;
                    //parms[8].Value = DBNull.Value;
                    //parms[9].Value = DBNull.Value;
                    //parms[10].Value = DBNull.Value;
                    //parms[11].Value = DBNull.Value;
                    //parms[12].Value = DBNull.Value;
                    parms[13].Value = ThongTinChiTiet.TieuDe ?? Convert.DBNull;
                    parms[14].Value = ThongTinChiTiet.NgayCap ?? Convert.DBNull;
                    parms[15].Value = ThongTinChiTiet.SoHieu ?? Convert.DBNull;
                    parms[16].Value = ThongTinChiTiet.TrinhDo ?? Convert.DBNull;
                    parms[17].Value = ThongTinChiTiet.NoiCap ?? Convert.DBNull;
                    //parms[18].Value = DBNull.Value;
                    //parms[19].Value = DBNull.Value;
                    //parms[20].Value = DBNull.Value;
                    //parms[21].Value = DBNull.Value;
                    //parms[22].Value = DBNull.Value;
                    //parms[23].Value = DBNull.Value;
                    //parms[24].Value = DBNull.Value;
                    //parms[25].Value = DBNull.Value;
                    //parms[26].Value = DBNull.Value;
                    //parms[27].Value = DBNull.Value;
                    parms[28].Value = (int)EnumLoaiThongTinNhaKhoaHoc.VanBangChungChi;
                    //parms[29].Value = DBNull.Value;
                    parms[30].Value = ThongTinChiTiet.CoQuanID;
                }
                else if (ThongTinChiTiet.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.GiaiThuongKhoaHoc)
                {
                    parms[0].Value = ThongTinChiTiet.CTNhaKhoaHocID ?? Convert.DBNull;
                    parms[1].Value = ThongTinChiTiet.CanBoID;
                    parms[2].Value = ThongTinChiTiet.KhoangThoiGian ?? Convert.DBNull;
                    //parms[3].Value = DBNull.Value;
                    //parms[4].Value = DBNull.Value;
                    //parms[5].Value = DBNull.Value;
                    //parms[6].Value = DBNull.Value;
                    //parms[7].Value = DBNull.Value;
                    //parms[8].Value = DBNull.Value;
                    //parms[9].Value = DBNull.Value;
                    //parms[10].Value = DBNull.Value;
                    //parms[11].Value = DBNull.Value;
                    //parms[12].Value = DBNull.Value;
                    parms[13].Value = ThongTinChiTiet.TieuDe ?? Convert.DBNull;
                    //parms[14].Value = DBNull.Value;
                    //parms[15].Value = DBNull.Value;
                    //parms[16].Value = DBNull.Value;
                    //parms[17].Value = DBNull.Value;
                    //parms[18].Value = DBNull.Value;
                    //parms[19].Value = DBNull.Value;
                    //parms[20].Value = DBNull.Value;
                    //parms[21].Value = DBNull.Value;
                    //parms[22].Value = DBNull.Value;
                    //parms[23].Value = DBNull.Value;
                    //parms[24].Value = DBNull.Value;
                    //parms[25].Value = DBNull.Value;
                    //parms[26].Value = DBNull.Value;
                    //parms[27].Value = DBNull.Value;
                    parms[28].Value = (int)EnumLoaiThongTinNhaKhoaHoc.GiaiThuongKhoaHoc;
                    //parms[29].Value = DBNull.Value;
                    parms[30].Value = ThongTinChiTiet.CoQuanID;
                }
                else if (ThongTinChiTiet.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.DuAnDeTai)
                {
                    parms[0].Value = ThongTinChiTiet.CTNhaKhoaHocID ?? Convert.DBNull;
                    parms[1].Value = ThongTinChiTiet.CanBoID;
                    parms[2].Value = ThongTinChiTiet.KhoangThoiGian ?? Convert.DBNull;
                    //parms[3].Value = DBNull.Value;
                    //parms[4].Value = DBNull.Value;
                    //parms[5].Value = DBNull.Value;
                    //parms[6].Value = DBNull.Value;
                    //parms[7].Value = DBNull.Value;
                    //parms[8].Value = DBNull.Value;
                    //parms[9].Value = DBNull.Value;
                    //parms[10].Value = DBNull.Value;
                    //parms[11].Value = DBNull.Value;
                    //parms[12].Value = DBNull.Value;
                    //parms[13].Value = DBNull.Value;
                    //parms[14].Value = DBNull.Value;
                    //parms[15].Value = DBNull.Value;
                    //parms[16].Value = DBNull.Value;
                    //parms[17].Value = DBNull.Value;
                    parms[18].Value = ThongTinChiTiet.TenDuAn ?? Convert.DBNull;
                    parms[19].Value = ThongTinChiTiet.CoQuanTaiTro ?? Convert.DBNull;
                    parms[20].Value = ThongTinChiTiet.VaiTroThamGia ?? Convert.DBNull;
                    //parms[21].Value = DBNull.Value;
                    //parms[22].Value = DBNull.Value;
                    //parms[23].Value = DBNull.Value;
                    //parms[24].Value = DBNull.Value;
                    //parms[25].Value = DBNull.Value;
                    //parms[26].Value = DBNull.Value;
                    //parms[27].Value = DBNull.Value;
                    parms[28].Value = (int)EnumLoaiThongTinNhaKhoaHoc.DuAnDeTai;
                    //parms[29].Value = DBNull.Value;
                    parms[30].Value = ThongTinChiTiet.CoQuanID;
                }
                else if (ThongTinChiTiet.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.BaiBaoTapChi)
                {
                    parms[0].Value = ThongTinChiTiet.CTNhaKhoaHocID ?? Convert.DBNull;
                    parms[1].Value = ThongTinChiTiet.CanBoID;
                    parms[2].Value = ThongTinChiTiet.KhoangThoiGian ?? Convert.DBNull;
                    //parms[3].Value = DBNull.Value;
                    //parms[4].Value = DBNull.Value;
                    //parms[5].Value = DBNull.Value;
                    //parms[6].Value = DBNull.Value;
                    //parms[7].Value = DBNull.Value;
                    //parms[8].Value = DBNull.Value;
                    //parms[9].Value = DBNull.Value;
                    //parms[10].Value = DBNull.Value;
                    //parms[11].Value = DBNull.Value;
                    //parms[12].Value = DBNull.Value;
                    parms[13].Value = ThongTinChiTiet.TieuDe ?? Convert.DBNull;
                    //parms[14].Value = DBNull.Value;
                    //parms[15].Value = DBNull.Value;
                    //parms[16].Value = DBNull.Value;
                    //parms[17].Value = DBNull.Value;
                    //parms[18].Value = DBNull.Value;
                    //parms[19].Value = DBNull.Value;
                    //parms[20].Value = DBNull.Value;
                    parms[21].Value = ThongTinChiTiet.TacGia ?? Convert.DBNull;
                    parms[22].Value = ThongTinChiTiet.TenTapChiSachHoiThao ?? Convert.DBNull;
                    parms[23].Value = ThongTinChiTiet.So ?? Convert.DBNull;
                    parms[24].Value = ThongTinChiTiet.Trang ?? Convert.DBNull;
                    parms[25].Value = ThongTinChiTiet.NhaXuatBan ?? Convert.DBNull;
                    //parms[26].Value = DBNull.Value;
                    parms[27].Value = ThongTinChiTiet.DeTai ?? Convert.DBNull;
                    parms[28].Value = (int)EnumLoaiThongTinNhaKhoaHoc.BaiBaoTapChi;
                    //parms[29].Value = DBNull.Value;
                    parms[30].Value = ThongTinChiTiet.CoQuanID;
                    parms[31].Value = ThongTinChiTiet.LoaiBaiBao ?? Convert.DBNull;
                    parms[32].Value = ThongTinChiTiet.ISSN ?? Convert.DBNull;
                    parms[33].Value = ThongTinChiTiet.NhiemVu ?? Convert.DBNull;
                    parms[34].Value = ThongTinChiTiet.LoaiNhiemVu ?? Convert.DBNull;
                    parms[35].Value = ThongTinChiTiet.TenHoiThao ?? Convert.DBNull;
                    parms[36].Value = ThongTinChiTiet.Tap ?? Convert.DBNull;
                    parms[37].Value = ThongTinChiTiet.NamDangTai ?? Convert.DBNull;
                    parms[38].Value = ThongTinChiTiet.LinkBaiBao ?? Convert.DBNull;
                    parms[39].Value = ThongTinChiTiet.LinhVucNganhKhoaHoc ?? Convert.DBNull;
                    parms[40].Value = ThongTinChiTiet.HeSoAnhHuong ?? Convert.DBNull;
                    parms[41].Value = ThongTinChiTiet.ChiSo ?? Convert.DBNull;
                    parms[42].Value = ThongTinChiTiet.RankSCIMAG ?? Convert.DBNull;
                    parms[43].Value = ThongTinChiTiet.DiemTapChi ?? Convert.DBNull;
                    parms[44].Value = ThongTinChiTiet.CapHoiThao ?? Convert.DBNull;
                    parms[45].Value = ThongTinChiTiet.NgayHoiThao ?? Convert.DBNull;
                    parms[46].Value = ThongTinChiTiet.DiaDiemToChuc ?? Convert.DBNull;

                }
                else if (ThongTinChiTiet.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.KetQuaNghienCuu)
                {
                    parms[0].Value = ThongTinChiTiet.CTNhaKhoaHocID ?? Convert.DBNull;
                    parms[1].Value = ThongTinChiTiet.CanBoID;
                    parms[2].Value = ThongTinChiTiet.NamXuatBan ?? Convert.DBNull;
                    //parms[3].Value = DBNull.Value;
                    //parms[4].Value = DBNull.Value;
                    //parms[5].Value = DBNull.Value;
                    //parms[6].Value = DBNull.Value;
                    //parms[7].Value = DBNull.Value;
                    //parms[8].Value = DBNull.Value;
                    //parms[9].Value = DBNull.Value;
                    //parms[10].Value = DBNull.Value;
                    //parms[11].Value = DBNull.Value;
                    //parms[12].Value = DBNull.Value;
                    parms[13].Value = ThongTinChiTiet.TieuDe ?? Convert.DBNull;
                    //parms[14].Value = DBNull.Value;
                    //parms[15].Value = DBNull.Value;
                    //parms[16].Value = DBNull.Value;
                    //parms[17].Value = DBNull.Value;
                    //parms[18].Value = DBNull.Value;
                    //parms[19].Value = DBNull.Value;
                    //parms[20].Value = DBNull.Value;
                    //parms[21].Value = ThongTinChiTiet.TacGia ?? Convert.DBNull;
                    //parms[22].Value = ThongTinChiTiet.TenTapChiSachHoiThao ?? Convert.DBNull;
                    //parms[23].Value = ThongTinChiTiet.So ?? Convert.DBNull;
                    //parms[24].Value = ThongTinChiTiet.Trang ?? Convert.DBNull;
                    //parms[25].Value = ThongTinChiTiet.NhaXuatBan ?? Convert.DBNull;
                    //parms[26].Value = DBNull.Value;
                    parms[27].Value = ThongTinChiTiet.DeTai ?? Convert.DBNull;
                    parms[28].Value = (int)EnumLoaiThongTinNhaKhoaHoc.KetQuaNghienCuu;
                    //parms[29].Value = DBNull.Value;
                    parms[30].Value = ThongTinChiTiet.CoQuanID;
                    //parms[31].Value = ThongTinChiTiet.LoaiBaiBao ?? Convert.DBNull;
                    //parms[32].Value = ThongTinChiTiet.ISSN ?? Convert.DBNull;
                    parms[33].Value = ThongTinChiTiet.NhiemVu ?? Convert.DBNull;
                    parms[34].Value = ThongTinChiTiet.LoaiNhiemVu ?? Convert.DBNull;
                    //parms[35].Value = ThongTinChiTiet.TenHoiThao?? Convert.DBNull;
                    //parms[36].Value = ThongTinChiTiet.Tap ?? Convert.DBNull;
                    //parms[37].Value = ThongTinChiTiet.NamDangTai ?? Convert.DBNull;
                    //parms[38].Value = ThongTinChiTiet.LinkBaiBao ?? Convert.DBNull;
                    //parms[39].Value = ThongTinChiTiet.LinhVucNganhKhoaHoc ?? Convert.DBNull;
                    //parms[40].Value = ThongTinChiTiet.HeSoAnhHuong ?? Convert.DBNull;
                    //parms[41].Value = ThongTinChiTiet.ChiSo ?? Convert.DBNull;
                    //parms[42].Value = ThongTinChiTiet.RankSCIMAG ?? Convert.DBNull;
                    //parms[43].Value = ThongTinChiTiet.DiemTapChi ?? Convert.DBNull;
                    //parms[44].Value = ThongTinChiTiet.CapHoiThao ?? Convert.DBNull;
                    //parms[45].Value = ThongTinChiTiet.NgayHoiThao ?? Convert.DBNull;
                    //parms[46].Value = ThongTinChiTiet.DiaDiemToChuc ?? Convert.DBNull;
                    //parms[47].Value = ThongTinChiTiet.LoaiDaoTao ?? Convert.DBNull;
                    //parms[48].Value = ThongTinChiTiet.TenHocVien ?? Convert.DBNull;
                    //parms[49].Value = ThongTinChiTiet.TenLuanVan ?? Convert.DBNull;
                    //parms[50].Value = ThongTinChiTiet.NguoiHuongDan ?? Convert.DBNull;
                    //parms[51].Value = ThongTinChiTiet.NamBaoVe ?? Convert.DBNull;
                    parms[52].Value = ThongTinChiTiet.GhiChu ?? Convert.DBNull;
                    //parms[53].Value = ThongTinChiTiet.ChuBienID ?? Convert.DBNull;
                    //parms[54].Value = ThongTinChiTiet.CoQuanChuBienID ?? Convert.DBNull;
                    parms[55].Value = ThongTinChiTiet.NamXuatBan ?? Convert.DBNull;
                }
                else if (ThongTinChiTiet.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.SachChuyenKhao)
                {
                    parms[0].Value = ThongTinChiTiet.CTNhaKhoaHocID ?? Convert.DBNull;
                    parms[1].Value = ThongTinChiTiet.CanBoID;
                    parms[2].Value = ThongTinChiTiet.KhoangThoiGian ?? Convert.DBNull;
                    //parms[3].Value = DBNull.Value;
                    //parms[4].Value = DBNull.Value;
                    //parms[5].Value = DBNull.Value;
                    //parms[6].Value = DBNull.Value;
                    //parms[7].Value = DBNull.Value;
                    //parms[8].Value = DBNull.Value;
                    //parms[9].Value = DBNull.Value;
                    //parms[10].Value = DBNull.Value;
                    //parms[11].Value = DBNull.Value;
                    //parms[12].Value = DBNull.Value;
                    parms[13].Value = ThongTinChiTiet.TieuDe ?? Convert.DBNull;
                    //parms[14].Value = DBNull.Value;
                    //parms[15].Value = DBNull.Value;
                    //parms[16].Value = DBNull.Value;
                    //parms[17].Value = DBNull.Value;
                    //parms[18].Value = DBNull.Value;
                    //parms[19].Value = DBNull.Value;
                    //parms[20].Value = DBNull.Value;
                    parms[21].Value = ThongTinChiTiet.TacGia ?? Convert.DBNull;
                    parms[22].Value = ThongTinChiTiet.TenTapChiSachHoiThao ?? Convert.DBNull;
                    parms[23].Value = ThongTinChiTiet.So ?? Convert.DBNull;
                    parms[24].Value = ThongTinChiTiet.Trang ?? Convert.DBNull;
                    parms[25].Value = ThongTinChiTiet.NhaXuatBan ?? Convert.DBNull;
                    //parms[26].Value = DBNull.Value;
                    parms[27].Value = ThongTinChiTiet.DeTai ?? Convert.DBNull;
                    parms[28].Value = (int)EnumLoaiThongTinNhaKhoaHoc.SachChuyenKhao;
                    //parms[29].Value = DBNull.Value;
                    parms[30].Value = ThongTinChiTiet.CoQuanID;
                    parms[32].Value = ThongTinChiTiet.ISSN ?? Convert.DBNull;
                    parms[33].Value = ThongTinChiTiet.NhiemVu ?? Convert.DBNull;
                    parms[34].Value = ThongTinChiTiet.LoaiNhiemVu ?? Convert.DBNull;

                    parms[53].Value = ThongTinChiTiet.ChuBienID ?? Convert.DBNull;
                    parms[54].Value = ThongTinChiTiet.CoQuanChuBienID ?? Convert.DBNull;
                    parms[55].Value = ThongTinChiTiet.NamXuatBan ?? Convert.DBNull;
                }
                else if (ThongTinChiTiet.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.CacMonGiangDay)
                {
                    parms[0].Value = ThongTinChiTiet.CTNhaKhoaHocID ?? Convert.DBNull;
                    parms[1].Value = ThongTinChiTiet.CanBoID;
                    //parms[2].Value = DBNull.Value;
                    //parms[3].Value = DBNull.Value;
                    //parms[4].Value = DBNull.Value;
                    //parms[5].Value = DBNull.Value;
                    //parms[6].Value = DBNull.Value;
                    //parms[7].Value = DBNull.Value;
                    //parms[8].Value = DBNull.Value;
                    //parms[9].Value = DBNull.Value;
                    //parms[10].Value = DBNull.Value;
                    //parms[11].Value = DBNull.Value;
                    //parms[12].Value = DBNull.Value;
                    parms[13].Value = ThongTinChiTiet.TieuDe ?? Convert.DBNull;
                    //parms[14].Value = DBNull.Value;
                    //parms[15].Value = DBNull.Value;
                    //parms[16].Value = DBNull.Value;
                    //parms[17].Value = DBNull.Value;
                    //parms[18].Value = DBNull.Value;
                    //parms[19].Value = DBNull.Value;
                    //parms[20].Value = DBNull.Value;
                    //parms[21].Value = DBNull.Value;
                    //parms[22].Value = DBNull.Value;
                    //parms[23].Value = DBNull.Value;
                    //parms[24].Value = DBNull.Value;
                    //parms[25].Value = DBNull.Value;
                    parms[26].Value = ThongTinChiTiet.DeCuong ?? Convert.DBNull;
                    //parms[27].Value = DBNull.Value;
                    parms[28].Value = (int)EnumLoaiThongTinNhaKhoaHoc.CacMonGiangDay;
                    //parms[29].Value = DBNull.Value;
                    parms[30].Value = ThongTinChiTiet.CoQuanID;
                }
                else if (ThongTinChiTiet.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.HuongNghienCuuChinh)
                {
                    parms[0].Value = ThongTinChiTiet.CTNhaKhoaHocID ?? Convert.DBNull;
                    parms[1].Value = ThongTinChiTiet.CanBoID;
                    //parms[2].Value = DBNull.Value;
                    //parms[3].Value = DBNull.Value;
                    //parms[4].Value = DBNull.Value;
                    //parms[5].Value = DBNull.Value;
                    //parms[6].Value = DBNull.Value;
                    //parms[7].Value = DBNull.Value;
                    //parms[8].Value = DBNull.Value;
                    //parms[9].Value = DBNull.Value;
                    //parms[10].Value = DBNull.Value;
                    //parms[11].Value = DBNull.Value;
                    //parms[12].Value = DBNull.Value;
                    //parms[13].Value = DBNull.Value;
                    //parms[14].Value = DBNull.Value;
                    //parms[15].Value = DBNull.Value;
                    //parms[16].Value = DBNull.Value;
                    //parms[17].Value = DBNull.Value;
                    //parms[18].Value = DBNull.Value;
                    //parms[19].Value = DBNull.Value;
                    //parms[20].Value = DBNull.Value;
                    //parms[21].Value = DBNull.Value;
                    //parms[22].Value = DBNull.Value;
                    //parms[23].Value = DBNull.Value;
                    //parms[24].Value = DBNull.Value;
                    //parms[25].Value = DBNull.Value;
                    //parms[26].Value = DBNull.Value;
                    //parms[27].Value = DBNull.Value;
                    parms[28].Value = (int)EnumLoaiThongTinNhaKhoaHoc.HuongNghienCuuChinh;
                    parms[29].Value = ThongTinChiTiet.TenHuongNghienCuuChinh;
                    parms[30].Value = ThongTinChiTiet.CoQuanID;
                }
                else if (ThongTinChiTiet.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.BaoCaoKhoaHoc)
                {
                    parms[0].Value = ThongTinChiTiet.CTNhaKhoaHocID ?? Convert.DBNull;
                    parms[1].Value = ThongTinChiTiet.CanBoID;
                    parms[2].Value = ThongTinChiTiet.KhoangThoiGian ?? Convert.DBNull;
                    //parms[3].Value = DBNull.Value;
                    //parms[4].Value = DBNull.Value;
                    //parms[5].Value = DBNull.Value;
                    //parms[6].Value = DBNull.Value;
                    //parms[7].Value = DBNull.Value;
                    //parms[8].Value = DBNull.Value;
                    //parms[9].Value = DBNull.Value;
                    //parms[10].Value = DBNull.Value;
                    //parms[11].Value = DBNull.Value;
                    //parms[12].Value = DBNull.Value;
                    parms[13].Value = ThongTinChiTiet.TieuDe ?? Convert.DBNull;
                    //parms[14].Value = DBNull.Value;
                    //parms[15].Value = DBNull.Value;
                    //parms[16].Value = DBNull.Value;
                    //parms[17].Value = DBNull.Value;
                    //parms[18].Value = DBNull.Value;
                    //parms[19].Value = DBNull.Value;
                    //parms[20].Value = DBNull.Value;
                    parms[21].Value = ThongTinChiTiet.TacGia ?? Convert.DBNull;
                    //parms[22].Value = DBNull.Value;
                    //parms[23].Value = DBNull.Value;
                    //parms[24].Value = DBNull.Value;
                    //parms[25].Value = DBNull.Value;
                    //parms[26].Value = DBNull.Value;
                    //parms[27].Value = DBNull.Value;
                    //parms[28].Value = (int)EnumLoaiThongTinNhaKhoaHoc.BaoCaoKhoaHoc;
                    //parms[29].Value = ThongTinChiTiet.TenHuongNghienCuuChinh;
                    parms[30].Value = ThongTinChiTiet.CoQuanID;
                    parms[32].Value = ThongTinChiTiet.ISSN ?? Convert.DBNull;
                    parms[35].Value = ThongTinChiTiet.TenHoiThao ?? Convert.DBNull;
                }
                else if (ThongTinChiTiet.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.SanPhamDaoTao)
                {
                    parms[0].Value = ThongTinChiTiet.CTNhaKhoaHocID ?? Convert.DBNull;
                    parms[1].Value = ThongTinChiTiet.CanBoID;
                    parms[2].Value = ThongTinChiTiet.KhoangThoiGian ?? Convert.DBNull;
                    parms[3].Value = ThongTinChiTiet.CoSoDaoTao;
                    //parms[4].Value = DBNull.Value;
                    //parms[5].Value = DBNull.Value;
                    //parms[6].Value = DBNull.Value;
                    //parms[7].Value = DBNull.Value;
                    //parms[8].Value = DBNull.Value;
                    //parms[9].Value = DBNull.Value;
                    //parms[10].Value = DBNull.Value;
                    //parms[11].Value = DBNull.Value;
                    //parms[12].Value = DBNull.Value;
                    //parms[13].Value = ThongTinChiTiet.TieuDe ?? Convert.DBNull;
                    //parms[14].Value = DBNull.Value;
                    //parms[15].Value = DBNull.Value;
                    //parms[16].Value = DBNull.Value;
                    //parms[17].Value = DBNull.Value;
                    //parms[18].Value = DBNull.Value;
                    //parms[19].Value = DBNull.Value;
                    //parms[20].Value = DBNull.Value;
                    //parms[21].Value = ThongTinChiTiet.TacGia ?? Convert.DBNull;
                    //parms[22].Value = ThongTinChiTiet.TenTapChiSachHoiThao ?? Convert.DBNull;
                    //parms[23].Value = ThongTinChiTiet.So ?? Convert.DBNull;
                    //parms[24].Value = ThongTinChiTiet.Trang ?? Convert.DBNull;
                    //parms[25].Value = ThongTinChiTiet.NhaXuatBan ?? Convert.DBNull;
                    //parms[26].Value = DBNull.Value;
                    parms[27].Value = ThongTinChiTiet.DeTai ?? Convert.DBNull;
                    parms[28].Value = (int)EnumLoaiThongTinNhaKhoaHoc.SanPhamDaoTao;
                    //parms[29].Value = DBNull.Value;
                    parms[30].Value = ThongTinChiTiet.CoQuanID;
                    //parms[31].Value = ThongTinChiTiet.LoaiBaiBao ?? Convert.DBNull;
                    //parms[32].Value = ThongTinChiTiet.ISSN ?? Convert.DBNull;
                    parms[33].Value = ThongTinChiTiet.NhiemVu ?? Convert.DBNull;
                    parms[34].Value = ThongTinChiTiet.LoaiNhiemVu ?? Convert.DBNull;
                    //parms[35].Value = ThongTinChiTiet.TenHoiThao?? Convert.DBNull;
                    //parms[36].Value = ThongTinChiTiet.Tap ?? Convert.DBNull;
                    //parms[37].Value = ThongTinChiTiet.NamDangTai ?? Convert.DBNull;
                    //parms[38].Value = ThongTinChiTiet.LinkBaiBao ?? Convert.DBNull;
                    //parms[39].Value = ThongTinChiTiet.LinhVucNganhKhoaHoc ?? Convert.DBNull;
                    //parms[40].Value = ThongTinChiTiet.HeSoAnhHuong ?? Convert.DBNull;
                    //parms[41].Value = ThongTinChiTiet.ChiSo ?? Convert.DBNull;
                    //parms[42].Value = ThongTinChiTiet.RankSCIMAG ?? Convert.DBNull;
                    //parms[43].Value = ThongTinChiTiet.DiemTapChi ?? Convert.DBNull;
                    parms[44].Value = ThongTinChiTiet.CapHoiThao ?? Convert.DBNull;
                    //parms[45].Value = ThongTinChiTiet.NgayHoiThao ?? Convert.DBNull;
                    //parms[46].Value = ThongTinChiTiet.DiaDiemToChuc ?? Convert.DBNull;
                    parms[47].Value = ThongTinChiTiet.LoaiDaoTao ?? Convert.DBNull;
                    parms[48].Value = ThongTinChiTiet.TenHocVien ?? Convert.DBNull;
                    parms[49].Value = ThongTinChiTiet.TenLuanVan ?? Convert.DBNull;
                    parms[50].Value = ThongTinChiTiet.NguoiHuongDan ?? Convert.DBNull;
                    parms[51].Value = ThongTinChiTiet.NamBaoVe ?? Convert.DBNull;
                }

                if (ThongTinChiTiet.CTNhaKhoaHocID == null)
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
                            if (ThongTinChiTiet.CTNhaKhoaHocID > 0)
                            {
                                Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_ThongTinCTNhaKhoaHoc_Update", parms);
                                Result.Message = ConstantLogMessage.Alert_Update_Success("Chi tiết Đề tài");
                                Result.Data = ThongTinChiTiet.CTNhaKhoaHocID;
                                //xóa tác giả cũ
                                SqlParameter[] parms_del = new SqlParameter[]{
                                    new SqlParameter("CTNhaKhoaHocID", SqlDbType.Int),
                                };
                                parms_del[0].Value = ThongTinChiTiet.CTNhaKhoaHocID;
                                SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_TacGia_Delete", parms_del);
                                //thêm tác giả
                                if (ThongTinChiTiet.ListTacGia != null && ThongTinChiTiet.ListTacGia.Count > 0)
                                {
                                    foreach (var item in ThongTinChiTiet.ListTacGia)
                                    {
                                        SqlParameter[] parms_tg = new SqlParameter[]{
                                            new SqlParameter("CTNhaKhoaHocID", SqlDbType.Int),
                                            new SqlParameter("CanBoID", SqlDbType.Int),
                                            new SqlParameter("CoQuanID", SqlDbType.Int),
                                        };
                                        parms_tg[0].Value = ThongTinChiTiet.CTNhaKhoaHocID;
                                        parms_tg[1].Value = item.CanBoID ?? Convert.DBNull;
                                        parms_tg[2].Value = item.CoQuanID ?? Convert.DBNull;

                                        SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_TacGia_Insert", parms_tg);
                                    }
                                }
                            }
                            else
                            {
                                Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_ThongTinCTNhaKhoaHoc_Insert", parms);
                                Result.Message = ConstantLogMessage.Alert_Insert_Success("Chi tiết Đề tài");
                                Result.Data = Utils.ConvertToInt32(parms[0].Value, 0);
                                //thêm tác giả
                                if (ThongTinChiTiet.ListTacGia != null && ThongTinChiTiet.ListTacGia.Count > 0)
                                {
                                    foreach (var item in ThongTinChiTiet.ListTacGia)
                                    {
                                        SqlParameter[] parms_tg = new SqlParameter[]{
                                            new SqlParameter("CTNhaKhoaHocID", SqlDbType.Int),
                                            new SqlParameter("CanBoID", SqlDbType.Int),
                                            new SqlParameter("CoQuanID", SqlDbType.Int),
                                        };
                                        parms_tg[0].Value = Result.Data;
                                        parms_tg[1].Value = item.CanBoID ?? Convert.DBNull;
                                        parms_tg[2].Value = item.CoQuanID ?? Convert.DBNull;

                                        SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_TacGia_Insert", parms_tg);
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
        /// Cập nhật thông tin nhà khoa học
        /// </summary>
        /// <param name="NhaKhoaHocModel"></param>
        /// <returns></returns>
        public BaseResultModel Update(NhaKhoaHocModel NhaKhoaHocModel)
        {
            var Result = new BaseResultModel();
            string ChucDanhKhoaHocIDStr = "";
            string ChucDanhHanhChinhIDStr = "";
            if (NhaKhoaHocModel.ChucDanhKhoaHoc != null && NhaKhoaHocModel.ChucDanhKhoaHoc.Count > 0)
            {
                foreach (var item in NhaKhoaHocModel.ChucDanhKhoaHoc)
                {
                    ChucDanhKhoaHocIDStr += item.ToString() + ",";
                }
            }
            if (NhaKhoaHocModel.ChucDanhHanhChinh != null && NhaKhoaHocModel.ChucDanhHanhChinh.Count > 0)
            {
                foreach (var item in NhaKhoaHocModel.ChucDanhHanhChinh)
                {
                    ChucDanhHanhChinhIDStr += item.ToString() + ",";
                }
            }

            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("CanBoID",SqlDbType.Int),
                    new SqlParameter("TenCanBo", SqlDbType.NVarChar),
                    new SqlParameter("MaCB", SqlDbType.NVarChar),
                    new SqlParameter("NgaySinh", SqlDbType.DateTime),
                    new SqlParameter("GioiTinh", SqlDbType.Int),
                    new SqlParameter("CoQuanCongTac", SqlDbType.NVarChar),
                    new SqlParameter("DiaChiCoQuan", SqlDbType.NVarChar),
                    new SqlParameter("PhongBanID",SqlDbType.Int),
                    new SqlParameter("Email", SqlDbType.NVarChar),
                    new SqlParameter("DienThoai", SqlDbType.NVarChar),
                    new SqlParameter("DienThoaiDiDong",SqlDbType.NVarChar),
                    new SqlParameter("Fax",SqlDbType.NVarChar),
                    new SqlParameter("TrangThaiID",SqlDbType.Int),
                    new SqlParameter("LaChuyenGia",SqlDbType.Bit),
                    new SqlParameter("Url",SqlDbType.NVarChar),
                    new SqlParameter("ChucDanhKhoaHocIDStr",SqlDbType.NVarChar),
                    new SqlParameter("ChucDanhHanhChinhIDStr",SqlDbType.NVarChar),
                    //new SqlParameter("HuongNghienCuuChinh",SqlDbType.NVarChar),
                };
                parameters[0].Value = NhaKhoaHocModel.CanBoID;
                parameters[1].Value = NhaKhoaHocModel.TenCanBo ?? Convert.DBNull;
                parameters[2].Value = NhaKhoaHocModel.MaCB ?? Convert.DBNull;
                parameters[3].Value = NhaKhoaHocModel.NgaySinh ?? Convert.DBNull;
                parameters[4].Value = NhaKhoaHocModel.GioiTinh ?? Convert.DBNull;
                parameters[5].Value = NhaKhoaHocModel.CoQuanCongTac ?? Convert.DBNull;
                parameters[6].Value = NhaKhoaHocModel.DiaChiCoQuan ?? Convert.DBNull;
                parameters[7].Value = NhaKhoaHocModel.PhongBanID ?? Convert.DBNull;
                parameters[8].Value = NhaKhoaHocModel.Email ?? Convert.DBNull;
                parameters[9].Value = NhaKhoaHocModel.DienThoai ?? Convert.DBNull;
                parameters[10].Value = NhaKhoaHocModel.DienThoaiDiDong ?? Convert.DBNull;
                parameters[11].Value = NhaKhoaHocModel.Fax ?? Convert.DBNull;
                parameters[12].Value = NhaKhoaHocModel.TrangThaiID ?? Convert.DBNull;
                parameters[13].Value = NhaKhoaHocModel.LaChuyenGia ?? Convert.DBNull;
                parameters[14].Value = NhaKhoaHocModel.Url ?? Convert.DBNull;
                parameters[15].Value = ChucDanhKhoaHocIDStr ?? Convert.DBNull;
                parameters[16].Value = ChucDanhHanhChinhIDStr ?? Convert.DBNull;
                //parameters[17].Value = NhaKhoaHocModel.HuongNghienCuuChinh ?? Convert.DBNull;

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Result.Status = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v1_LyLichKhoaHoc_Update", parameters);
                            //URL 
                            if (NhaKhoaHocModel.Url != null && NhaKhoaHocModel.Url != "")
                            {
                                SqlParameter[] parms_url = new SqlParameter[]{
                                new SqlParameter("CanBoID", SqlDbType.Int),
                                new SqlParameter("CoQuanID", SqlDbType.Int),
                                new SqlParameter("Url", SqlDbType.NVarChar),
                                new SqlParameter("LoaiThongTin", SqlDbType.Int),
                            };
                                parms_url[0].Value = NhaKhoaHocModel.CanBoID;
                                parms_url[1].Value = NhaKhoaHocModel.CoQuanID;
                                parms_url[2].Value = NhaKhoaHocModel.Url ?? Convert.DBNull;
                                parms_url[3].Value = EnumLoaiThongTinNhaKhoaHoc.Url.GetHashCode();

                                SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_URL_Insert", parms_url);
                            }
                            //Xóa ngươi giới thiệu cũ
                            SqlParameter[] parms_del = new SqlParameter[]{
                                new SqlParameter("CanBoID", SqlDbType.Int),
                                new SqlParameter("CoQuanID", SqlDbType.Int),
                            };
                            parms_del[0].Value = NhaKhoaHocModel.CanBoID;
                            parms_del[1].Value = NhaKhoaHocModel.CoQuanID;
                            SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_NguoiGioiThieu_Delete", parms_del);
                            //thêm ngươi giới thiệu
                            if (NhaKhoaHocModel.NguoiGioiThieu != null && NhaKhoaHocModel.NguoiGioiThieu.Count > 0)
                            {
                                foreach (var item in NhaKhoaHocModel.NguoiGioiThieu)
                                {
                                    SqlParameter[] parms_gt = new SqlParameter[]{
                                        new SqlParameter("CanBoID", SqlDbType.Int),
                                        new SqlParameter("CoQuanID", SqlDbType.Int),
                                        new SqlParameter("NguoiGioiThieuID", SqlDbType.Int),
                                        new SqlParameter("CoQuanGioiThieuID", SqlDbType.Int),
                                        new SqlParameter("Link", SqlDbType.NVarChar),
                                    };
                                    parms_gt[0].Value = NhaKhoaHocModel.CanBoID;
                                    parms_gt[1].Value = NhaKhoaHocModel.CoQuanID;
                                    parms_gt[2].Value = item.NguoiGioiThieuID ?? Convert.DBNull;
                                    parms_gt[3].Value = item.CoQuanGioiThieuID ?? Convert.DBNull;
                                    parms_gt[4].Value = item.Link ?? Convert.DBNull;

                                    SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_NguoiGioiThieu_Insert", parms_gt);
                                }
                            }
                            #region chuc danh
                            //if (Result.Status > 0 && NhaKhoaHocModel.CanBoID > 0)
                            //{
                            //    if (NhaKhoaHocModel.ChucDanhKhoaHoc != null && NhaKhoaHocModel.ChucDanhKhoaHoc.Count > 0)
                            //    {
                            //        //Xóa chức danh cũ
                            //        SqlParameter[] parms_del = new SqlParameter[]{
                            //            new SqlParameter("CanBoID", SqlDbType.Int),
                            //        };
                            //        parms_del[0].Value = NhaKhoaHocModel.CanBoID;
                            //        SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_ChucDanh_Delete", parms_del);
                            //        //insert chức danh khoa học 
                            //        foreach (var item in NhaKhoaHocModel.ChucDanhKhoaHoc)
                            //        {
                            //            SqlParameter[] parms_cd = new SqlParameter[]{
                            //                new SqlParameter("CanBoID", SqlDbType.Int),
                            //                new SqlParameter("ChucDanhID", SqlDbType.Int),
                            //                new SqlParameter("TenChucDanh", SqlDbType.NVarChar),
                            //                new SqlParameter("LoaiChucDanh", SqlDbType.Int),
                            //            };

                            //            parms_cd[0].Value = NhaKhoaHocModel.CanBoID;
                            //            parms_cd[1].Value = item.ChucDanhID;
                            //            parms_cd[2].Value = item.TenChucDanh;
                            //            parms_cd[3].Value = (int)EnumChucDanh.KhoaHoc;

                            //            SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_ChucDanh_Insert", parms_cd);

                            //        }
                            //    }

                            //    if (NhaKhoaHocModel.ChucDanhHanhChinh != null && NhaKhoaHocModel.ChucDanhHanhChinh.Count > 0)
                            //    {
                            //        //insert chức danh hành chính
                            //        foreach (var item in NhaKhoaHocModel.ChucDanhHanhChinh)
                            //        {
                            //            SqlParameter[] parms_cd = new SqlParameter[]{
                            //                new SqlParameter("CanBoID", SqlDbType.Int),
                            //                new SqlParameter("ChucDanhID", SqlDbType.Int),
                            //                new SqlParameter("TenChucDanh", SqlDbType.NVarChar),
                            //                new SqlParameter("LoaiChucDanh", SqlDbType.Int),
                            //            };

                            //            parms_cd[0].Value = NhaKhoaHocModel.CanBoID;
                            //            parms_cd[1].Value = item.ChucDanhID;
                            //            parms_cd[2].Value = item.TenChucDanh;
                            //            parms_cd[3].Value = (int)EnumChucDanh.HanhChinh;

                            //            SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_ChucDanh_Insert", parms_cd);

                            //        }
                            //    }
                            //}
                            #endregion
                            trans.Commit();
                            Result.Status = 1;
                            Result.Message = ConstantLogMessage.Alert_Update_Success("Nhà khoa học");
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
        /// Cập nhật thông tin url
        /// </summary>
        /// <param name="NhaKhoaHocModel"></param>
        /// <returns></returns>
        public BaseResultModel UpdateURL(NhaKhoaHocModel NhaKhoaHocModel)
        {
            var Result = new BaseResultModel();

            try
            {
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            if (NhaKhoaHocModel.Url != null && NhaKhoaHocModel.Url != "")
                            {
                                SqlParameter[] parms_url = new SqlParameter[]{
                                new SqlParameter("CanBoID", SqlDbType.Int),
                                new SqlParameter("CoQuanID", SqlDbType.Int),
                                new SqlParameter("Url", SqlDbType.NVarChar),
                                new SqlParameter("LoaiThongTin", SqlDbType.Int),
                            };
                                parms_url[0].Value = NhaKhoaHocModel.CanBoID;
                                parms_url[1].Value = NhaKhoaHocModel.CoQuanID;
                                parms_url[2].Value = NhaKhoaHocModel.Url ?? Convert.DBNull;
                                parms_url[3].Value = EnumLoaiThongTinNhaKhoaHoc.Url.GetHashCode();

                                SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_URL_Insert", parms_url);
                            }

                            //Xóa ngươi giới thiệu cũ
                            SqlParameter[] parms_del = new SqlParameter[]{
                                new SqlParameter("CanBoID", SqlDbType.Int),
                                new SqlParameter("CoQuanID", SqlDbType.Int),
                            };
                            parms_del[0].Value = NhaKhoaHocModel.CanBoID;
                            parms_del[1].Value = NhaKhoaHocModel.CoQuanID;
                            SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_NguoiGioiThieu_Delete", parms_del);
                            //thêm ngươi giới thiệu
                            if (NhaKhoaHocModel.NguoiGioiThieu != null && NhaKhoaHocModel.NguoiGioiThieu.Count > 0)
                            {
                                foreach (var item in NhaKhoaHocModel.NguoiGioiThieu)
                                {
                                    SqlParameter[] parms_gt = new SqlParameter[]{
                                        new SqlParameter("CanBoID", SqlDbType.Int),
                                        new SqlParameter("CoQuanID", SqlDbType.Int),
                                        new SqlParameter("NguoiGioiThieuID", SqlDbType.Int),
                                        new SqlParameter("CoQuanGioiThieuID", SqlDbType.Int),
                                        new SqlParameter("Link", SqlDbType.NVarChar),
                                    };
                                    parms_gt[0].Value = NhaKhoaHocModel.CanBoID;
                                    parms_gt[1].Value = NhaKhoaHocModel.CoQuanID;
                                    parms_gt[2].Value = item.NguoiGioiThieuID ?? Convert.DBNull;
                                    parms_gt[3].Value = item.CoQuanGioiThieuID ?? Convert.DBNull;
                                    parms_gt[4].Value = item.Link ?? Convert.DBNull;

                                    SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_NguoiGioiThieu_Insert", parms_gt);
                                }
                            }

                            trans.Commit();
                            Result.Status = 1;
                            Result.Message = ConstantLogMessage.Alert_Update_Success("Nhà khoa học");
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
        /// Xóa thông tin chi tiết nhà khoa học theo ID
        /// </summary>
        /// <param name="ThongTinChiTiet"></param>
        /// <returns></returns>
        public BaseResultModel Delete_ThongTinChiTiet(ThongTinCTNhaKhoaHocModel ThongTinChiTiet)
        {
            var Result = new BaseResultModel();
            SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@CTNhaKhoaHocID",SqlDbType.Int)
                };
            parameters[0].Value = ThongTinChiTiet.CTNhaKhoaHocID;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, @"v1_ThongTinCTNhaKhoaHoc_Delete", parameters);
                        Result.Message = ConstantLogMessage.Alert_Delete_Success("Thông tin chi tiết nhà khoa học");
                        Result.Status = 1;
                        trans.Commit();
                        int NghiepVuID = Utils.ConvertToInt32(ThongTinChiTiet.CTNhaKhoaHocID, 0);
                        new FileDinhKemDAL().Delete_FileDinhKem(ThongTinChiTiet.LoaiFile, NghiepVuID);
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
        /// Xóa hoạt động khoa học
        /// </summary>
        /// <param name="HoatDongKhoaHoc"></param>
        /// <returns></returns>
        public BaseResultModel Delete_HoatDongKhoaHoc(HoatDongKhoaHocModel HoatDongKhoaHoc)
        {
            var Result = new BaseResultModel();
            SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@HoatDongKhoaHocID",SqlDbType.Int)
                };
            parameters[0].Value = HoatDongKhoaHoc.HoatDongKhoaHocID;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, @"v1_HoatDongKhoaHoc_Delete", parameters);
                        Result.Message = ConstantLogMessage.Alert_Delete_Success("hoạt động khoa học");
                        Result.Status = 1;
                        trans.Commit();
                        //int NghiepVuID = Utils.ConvertToInt32(HoatDongKhoaHoc.CTNhaKhoaHocID, 0);
                        //new FileDinhKemDAL().Delete_FileDinhKem(HoatDongKhoaHoc.LoaiFile, NghiepVuID);
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
        /// Chỉnh sửa hoạt động khoa học
        /// </summary>
        /// <param name="HoatDongKhoaHoc"></param>
        /// <returns></returns>
        public BaseResultModel Edit_HoatDongKhoaHoc(HoatDongKhoaHocModel HoatDongKhoaHoc)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parms = new SqlParameter[]{
                    new SqlParameter("HoatDongKhoaHocID", SqlDbType.Int),
                    new SqlParameter("CanBoID", SqlDbType.Int),
                    new SqlParameter("NhiemVu", SqlDbType.Int),
                    new SqlParameter("HoatDongKhoaHoc", SqlDbType.NVarChar),
                    new SqlParameter("PublicCV", SqlDbType.Bit),
                    new SqlParameter("CoQuanID", SqlDbType.Int),
                    new SqlParameter("NamThucHien", SqlDbType.Int),
                };
                parms[0].Value = HoatDongKhoaHoc.HoatDongKhoaHocID ?? Convert.DBNull;
                parms[1].Value = HoatDongKhoaHoc.CanBoID;
                parms[2].Value = HoatDongKhoaHoc.NhiemVu ?? Convert.DBNull;
                parms[3].Value = HoatDongKhoaHoc.HoatDongKhoaHoc ?? Convert.DBNull;
                parms[4].Value = HoatDongKhoaHoc.PublicCV ?? Convert.DBNull;
                parms[5].Value = HoatDongKhoaHoc.CoQuanID ?? Convert.DBNull;
                parms[6].Value = HoatDongKhoaHoc.NamThucHien ?? DateTime.Now.Year;

                if (HoatDongKhoaHoc.HoatDongKhoaHocID == null)
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
                            if (HoatDongKhoaHoc.HoatDongKhoaHocID > 0)
                            {
                                Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_HoatDongKhoaHoc_Update", parms);
                                Result.Message = ConstantLogMessage.Alert_Update_Success("hoạt động khoa học");
                                Result.Data = HoatDongKhoaHoc.HoatDongKhoaHocID;
                            }
                            else
                            {
                                Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_HoatDongKhoaHoc_Insert", parms);
                                Result.Message = ConstantLogMessage.Alert_Insert_Success("hoạt động khoa học");
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
        /// Lấy hoạt động khoa học theo CanBoID và CoQuanID
        /// </summary>
        /// <param name="CanBoID"></param>
        /// <param name="CoQuanID"></param>
        /// <returns></returns>
        public List<HoatDongKhoaHocModel> HoatDongKhoaHoc_GetByCanBoID(int CanBoID, int CoQuanID)
        {
            List<HoatDongKhoaHocModel> Result = new List<HoatDongKhoaHocModel>();
            List<ChiTietHoatDongKhoaHocModel> Data = new List<ChiTietHoatDongKhoaHocModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CanBoID",SqlDbType.Int),
                new SqlParameter("@CoQuanID",SqlDbType.Int),
            };
            parameters[0].Value = CanBoID;
            parameters[1].Value = CoQuanID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_HoatDongKhoaHoc_GetByCanBoID", parameters))
                {
                    while (dr.Read())
                    {
                        ChiTietHoatDongKhoaHocModel info = new ChiTietHoatDongKhoaHocModel();
                        info.HoatDongKhoaHocID = Utils.ConvertToInt32(dr["HoatDongKhoaHocID"], 0);
                        info.NhiemVu = Utils.ConvertToInt32(dr["NhiemVu"], 0);
                        info.HoatDongKhoaHoc = Utils.ConvertToString(dr["HoatDongKhoaHoc"], string.Empty);
                        info.PublicCV = Utils.ConvertToBoolean(dr["PublicCV"], false);
                        //file
                        info.NghiepVuID = Utils.ConvertToInt32(dr["NghiepVuID"], 0);
                        info.FileDinhKemID = Utils.ConvertToInt32(dr["FileDinhKemID"], 0);
                        info.TenFileGoc = Utils.ConvertToString(dr["TenFileGoc"], string.Empty);
                        info.TenFileHeThong = Utils.ConvertToString(dr["TenFileHeThong"], string.Empty);
                        info.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        info.LoaiFile = Utils.ConvertToInt32(dr["LoaiFile"], 0);
                        info.NguoiTaoID = Utils.ConvertToInt32(dr["NguoiTaoID"], 0);
                        //info.TenNguoiTao = Utils.ConvertToString(dr["TenNguoiTao"], string.Empty);
                        info.NgayTao = Utils.ConvertToDateTime(dr["NgayTao"], DateTime.Now);
                        info.NamThucHien = Utils.ConvertToInt32(dr["NamThucHien"], 0);

                        Data.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (Data.Count > 0)
            {
                Result = Data.GroupBy(p => p.HoatDongKhoaHocID)
                    .Select(g => new HoatDongKhoaHocModel
                    {
                        HoatDongKhoaHocID = g.Key,
                        NhiemVu = g.FirstOrDefault().NhiemVu,
                        HoatDongKhoaHoc = g.FirstOrDefault().HoatDongKhoaHoc,
                        PublicCV = g.FirstOrDefault().PublicCV,
                        NamThucHien = g.FirstOrDefault().NamThucHien,
                        FileDinhKem = Data.Where(x => x.NghiepVuID == g.Key && x.FileDinhKemID > 0)
                                        .Select(y => new FileDinhKemModel
                                        {
                                            FileDinhKemID = y.FileDinhKemID,
                                            NghiepVuID = g.Key ?? 0,
                                            TenFileGoc = y.TenFileGoc,
                                            TenFileHeThong = y.TenFileHeThong,
                                            FileUrl = y.FileUrl,
                                            LoaiFile = y.LoaiFile,
                                            NguoiTaoID = y.NguoiTaoID,
                                            NgayTao = y.NgayTao,
                                            
                                        }
                                        ).ToList()
                    }
                    ).ToList();
            }


            return Result;
        }

        public BaseResultModel Delete(NhaKhoaHocModel NhaKhoaHocModel)
        {
            var Result = new BaseResultModel();
            var listDeTai = new DeTaiDAL().GetDeTaiByCanBoID(NhaKhoaHocModel.CanBoID);
            if (listDeTai != null && listDeTai.Count > 0)
            {
                Result.Message = "Cán bộ có trong nhiệm vụ nghiên cứu không được xóa!";
                Result.Status = 0;
                return Result;
            }

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CanBoID",SqlDbType.Int)
            };
            parameters[0].Value = NhaKhoaHocModel.CanBoID;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, @"v1_LyLichKhoaHoc_Delete", parameters);
                        Result.Message = ConstantLogMessage.Alert_Delete_Success("nhà khoa học");
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

        public List<FileDinhKemModel> GetAllFile(int CanBoID)
        {
            List<FileDinhKemModel> Result = new List<FileDinhKemModel>();

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CanBoID",SqlDbType.Int),
            };
            parameters[0].Value = CanBoID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_LyLichKhoaHoc_GetAllFile", parameters))
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
        /// Lấy danh sách người giới thiệu
        /// </summary>
        /// <param name="CanBoID"></param>
        /// <param name="CoQuanID"></param>
        /// <returns></returns>
        public List<NguoiGioiThieuModel> GetNguoiGioiThieu(int CanBoID, int CoQuanID)
        {
            List<NguoiGioiThieuModel> Result = new List<NguoiGioiThieuModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CanBoID",SqlDbType.Int),
                new SqlParameter("@CoQuanID",SqlDbType.Int)
            };
            parameters[0].Value = CanBoID;
            parameters[1].Value = CoQuanID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_NguoiGioiThieu_GetByCanBoID", parameters))
                {
                    while (dr.Read())
                    {
                        NguoiGioiThieuModel info = new NguoiGioiThieuModel();
                        info.ID = Utils.ConvertToInt32(dr["ID"], 0);
                        info.NguoiGioiThieuID = Utils.ConvertToInt32(dr["NguoiGioiThieuID"], 0);
                        info.CoQuanGioiThieuID = Utils.ConvertToInt32(dr["CoQuanGioiThieuID"], 0);
                        info.TenNguoiGioiThieu = Utils.ConvertToString(dr["TenNguoiGioiThieu"], string.Empty);
                        info.Link = Utils.ConvertToString(dr["Link"], string.Empty);
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
        /// Lấy thông tin url cán bộ trong trường
        /// </summary>
        /// <param name="CanBoID"></param>
        /// <param name="CoQuanID"></param>
        /// <returns></returns>
        public string GetUrlCanBoTrongTruong(int CanBoID, int CoQuanID)
        {
            string Result = "";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CanBoID",SqlDbType.Int),
                new SqlParameter("@CoQuanID",SqlDbType.Int),
                new SqlParameter("@LoaiThongTin",SqlDbType.Int),
            };
            parameters[0].Value = CanBoID;
            parameters[1].Value = CoQuanID;
            parameters[2].Value = EnumLoaiThongTinNhaKhoaHoc.Url.GetHashCode();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_URL_GetByCanBoID", parameters))
                {
                    while (dr.Read())
                    {
                        Result = Utils.ConvertToString(dr["Url"], string.Empty);
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

        public List<DuAnDeTaiModel> GetDuAnDeTaiByCanBoID(int CanBoID)
        {
            List<DuAnDeTaiModel> Result = new List<DuAnDeTaiModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CanBoID",SqlDbType.Int)
            };
            parameters[0].Value = CanBoID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_DuAnDeTai_GetByCanBoID", parameters))
                {
                    while (dr.Read())
                    {
                        DuAnDeTaiModel info = new DuAnDeTaiModel();
                        info.VaiTro = Utils.ConvertToInt32(dr["VaiTro"], 0);
                        info.TenDuAn = Utils.ConvertToString(dr["TenDuAn"], string.Empty);
                        info.KhoangThoiGian = Utils.ConvertToString(dr["KhoangThoiGian"], string.Empty);
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

        /// <summary>
        /// lấy các kết quả đã nhập ở chức năng Nghiên cứu nhiệm vụ để hiển thị trong lý lịch khoa học
        /// </summary>
        /// <param name="CanBoID"></param>
        /// <param name="LaCanBoTrongTruong"></param>
        /// <returns></returns>
        public NhaKhoaHocModel GetThongTinNhaKhoaHoc_DeTai(int CanBoID, Boolean LaCanBoTrongTruong)
        {
            List<int> LoaiKQ = new List<int>();
            List<int> LoaiKQ_BaiBao = new List<int>();
            List<int> LoaiKQ_SachChuyenKhao = new List<int>();
            List<int> LoaiKQ_SanPhamKhac = new List<int>();

            var loaiKQ_BaiBao = new SystemConfigDAL().GetByKey("LOAI_KQ_BAI_BAO").ConfigValue;
            if (loaiKQ_BaiBao != null && loaiKQ_BaiBao.Length > 0)
            {
                var loai = loaiKQ_BaiBao.Split(',');
                foreach (var item in loai)
                {
                    int id = Utils.ConvertToInt32(item, 0);
                    if (id > 0) LoaiKQ_BaiBao.Add(id);
                }
            }
            var loaiKQ_SachChuyenKhao = new SystemConfigDAL().GetByKey("LOAI_KQ_SACH_CHUYEN_KHAO").ConfigValue;
            if (loaiKQ_SachChuyenKhao != null && loaiKQ_SachChuyenKhao.Length > 0)
            {
                var loai = loaiKQ_SachChuyenKhao.Split(',');
                foreach (var item in loai)
                {
                    int id = Utils.ConvertToInt32(item, 0);
                    if (id > 0) LoaiKQ_SachChuyenKhao.Add(id);
                }
            }
            var loaiKQ_DaoTao = new SystemConfigDAL().GetByKey("LOAI_KQ_DAO_TAO").ConfigValue;
            if (loaiKQ_DaoTao != null && loaiKQ_DaoTao.Length > 0)
            {
                var loai = loaiKQ_DaoTao.Split(',');
                foreach (var item in loai)
                {
                    int id = Utils.ConvertToInt32(item, 0);
                    if (id > 0) LoaiKQ_SanPhamKhac.Add(id);
                }
            }
            var loaiKQ_SanPhamKhac = new SystemConfigDAL().GetByKey("LOAI_KQ_SAN_PHAM_KHAC").ConfigValue;
            if (loaiKQ_SanPhamKhac != null && loaiKQ_SanPhamKhac.Length > 0)
            {
                var loai = loaiKQ_SanPhamKhac.Split(',');
                foreach (var item in loai)
                {
                    int id = Utils.ConvertToInt32(item, 0);
                    if (id > 0) LoaiKQ_SanPhamKhac.Add(id);
                }
            }
            LoaiKQ.AddRange(LoaiKQ_BaiBao);
            LoaiKQ.AddRange(LoaiKQ_SachChuyenKhao);
            LoaiKQ.AddRange(LoaiKQ_SanPhamKhac);
            LoaiKQ = LoaiKQ.Distinct().ToList();

            var pList = new SqlParameter("@ListLoaiKQID", SqlDbType.Structured);
            pList.TypeName = "dbo.list_ID";
            var tbLoaiKQID = new DataTable();
            tbLoaiKQID.Columns.Add("CanBoID", typeof(string));
            LoaiKQ.ForEach(x => tbLoaiKQID.Rows.Add(x));

            NhaKhoaHocModel nhaKhoaHoc = new NhaKhoaHocModel();
            List<KetQuaNghienCuuModel> Result = new List<KetQuaNghienCuuModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CanBoID",SqlDbType.Int),
                new SqlParameter("@LaCanBoTrongTruong",SqlDbType.Bit),
                pList
            };
            parameters[0].Value = CanBoID;
            parameters[1].Value = LaCanBoTrongTruong;
            parameters[2].Value = tbLoaiKQID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_GetThongTinNhaKhoaHoc_DeTai", parameters))
                {
                    while (dr.Read())
                    {
                        KetQuaNghienCuuModel info = new KetQuaNghienCuuModel();
                        info.DeTaiID = Utils.ConvertToInt32(dr["DeTaiID"], 0);
                        info.LoaiKetQua = Utils.ConvertToInt32(dr["LoaiKetQua"], 0);
                        info.ThoiGian = Utils.ConvertToString(dr["ThoiGian"], string.Empty);
                        info.TacGia = Utils.ConvertToString(dr["TacGia"], string.Empty);
                        info.TieuDe = Utils.ConvertToString(dr["TieuDe"], string.Empty);
                        info.TenTapChiSachHoiThao = Utils.ConvertToString(dr["TenTapChiSachHoiThao"], string.Empty);
                        info.So = Utils.ConvertToString(dr["So"], string.Empty);
                        info.Trang = Utils.ConvertToString(dr["Trang"], string.Empty);
                        info.NhaXuatBan = Utils.ConvertToString(dr["NhaXuatBan"], string.Empty);
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

            nhaKhoaHoc.BaiBaoTapChi = new List<BaiBaoTapChiModel>();
            nhaKhoaHoc.SachChuyenKhao = new List<SachChuyenKhaoModel>();
            nhaKhoaHoc.KetQuaNghienCuu = new List<KetQuaNghienCuuNKHModel>();
            foreach (var item in Result)
            {
                foreach (var bb in LoaiKQ_BaiBao)
                {
                    if (item.LoaiKetQua == bb)
                    {
                        BaiBaoTapChiModel info = new BaiBaoTapChiModel();
                        info.DeTai = item.DeTaiID;
                        info.KhoangThoiGian = item.ThoiGian;
                        //info.TacGia = item.TacGia;
                        info.TieuDe = item.TieuDe;
                        info.TenTapChiSachHoiThao = item.TenTapChiSachHoiThao;
                        info.So = item.So;
                        info.Trang = item.Trang;
                        info.NhaXuatBan = item.NhaXuatBan;
                        info.Disable = true;
                        nhaKhoaHoc.BaiBaoTapChi.Add(info);
                    }
                }
                foreach (var s in LoaiKQ_SachChuyenKhao)
                {
                    if (item.LoaiKetQua == s)
                    {
                        SachChuyenKhaoModel info = new SachChuyenKhaoModel();
                        info.DeTai = item.DeTaiID;
                        info.KhoangThoiGian = item.ThoiGian;
                        //info.TacGia = item.TacGia;
                        info.TieuDe = item.TieuDe;
                        info.TenTapChiSachHoiThao = item.TenTapChiSachHoiThao;
                        info.So = item.So;
                        info.Trang = item.Trang;
                        info.NhaXuatBan = item.NhaXuatBan;
                        info.Disable = true;
                        nhaKhoaHoc.SachChuyenKhao.Add(info);
                    }
                }
                foreach (var sp in LoaiKQ_SanPhamKhac)
                {
                    if (item.LoaiKetQua == sp)
                    {
                        KetQuaNghienCuuNKHModel info = new KetQuaNghienCuuNKHModel();
                        info.DeTai = item.DeTaiID;
                        info.KhoangThoiGian = item.ThoiGian;
                        info.TacGia = item.TacGia;
                        info.TieuDe = item.TieuDe;
                        info.TenTapChiSachHoiThao = item.TenTapChiSachHoiThao;
                        info.So = item.So;
                        info.Trang = item.Trang;
                        info.NhaXuatBan = item.NhaXuatBan;
                        info.Disable = true;
                        nhaKhoaHoc.KetQuaNghienCuu.Add(info);
                    }
                }
            }

            return nhaKhoaHoc;
        }

        /// <summary>
        /// Lấy danh sách chi tiết nhà khoa hoc có tác giả là CanBoID
        /// </summary>
        /// <param name="CanBoID"></param>
        /// <param name="CoQuanID"></param>
        /// <returns></returns>
        public NhaKhoaHocModel GetThongTinTuNhaKhoaHocKhac(int CanBoID, int CoQuanID)
        {
            List<ThongTinCTNhaKhoaHocModel> Result = new List<ThongTinCTNhaKhoaHocModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CanBoID",SqlDbType.Int),
                new SqlParameter("@CoQuanID",SqlDbType.Int),
            };
            parameters[0].Value = CanBoID;
            parameters[1].Value = CoQuanID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_LyLichKhoaHoc_GetChiTietNhaKhoaHoc", parameters))
                {
                    while (dr.Read())
                    {
                        ThongTinCTNhaKhoaHocModel info = new ThongTinCTNhaKhoaHocModel();
                        info.CTNhaKhoaHocID = Utils.ConvertToInt32(dr["CTNhaKhoaHocID"], 0);
                        info.LoaiThongTin = Utils.ConvertToInt32(dr["LoaiThongTin"], 0);
                        info.KhoangThoiGian = Utils.ConvertToString(dr["KhoangThoiGian"], string.Empty);
                  
                        info.TieuDe = Utils.ConvertToString(dr["TieuDe"], string.Empty);
                        //info.NgayCap = Utils.ConvertToNullableDateTime(dr["NgayCap"], null);
                        //info.SoHieu = Utils.ConvertToString(dr["SoHieu"], string.Empty);
                        //info.TrinhDo = Utils.ConvertToString(dr["TrinhDo"], string.Empty);
                        //info.NoiCap = Utils.ConvertToString(dr["NoiCapNKH"], string.Empty);
                        //info.NoiCapCC = Utils.ConvertToString(dr["NoiCapCC"], string.Empty);
                        //info.TenDuAn = Utils.ConvertToString(dr["TenDuAn"], string.Empty);
                        //info.CoQuanTaiTro = Utils.ConvertToString(dr["CoQuanTaiTro"], string.Empty);
                        //info.VaiTroThamGia = Utils.ConvertToString(dr["VaiTroThamGia"], string.Empty);
                        info.TacGia = Utils.ConvertToString(dr["TacGia"], string.Empty);
                        info.TenTapChiSachHoiThao = Utils.ConvertToString(dr["TenTapChiSachHoiThao"], string.Empty);
                        info.So = Utils.ConvertToString(dr["So"], string.Empty);
                        info.Trang = Utils.ConvertToString(dr["Trang"], string.Empty);
                        info.NhaXuatBan = Utils.ConvertToString(dr["NhaXuatBan"], string.Empty);
                        info.LoaiBaiBao = Utils.ConvertToInt32(dr["LoaiBaiBao"], 0);
                        info.ISSN = Utils.ConvertToString(dr["ISSN"], string.Empty);
                        info.NhiemVuBaiBao = Utils.ConvertToInt32(dr["NhiemVuBaiBao"], 0);
                        info.LoaiNhiemVu = Utils.ConvertToInt32(dr["LoaiNhiemVu"], 0);
                        info.TenHoiThao = Utils.ConvertToString(dr["TenHoiThao"], string.Empty);

                        info.Tap = Utils.ConvertToString(dr["Tap"], string.Empty);
                        info.NamDangTai = Utils.ConvertToInt32(dr["NamDangTai"], 0);
                        info.LinkBaiBao = Utils.ConvertToString(dr["LinkBaiBao"], string.Empty);
                        info.LinhVucNganhKhoaHoc = Utils.ConvertToInt32(dr["LinhVucNganhKhoaHoc"], 0);
                        info.HeSoAnhHuong = Utils.ConvertToString(dr["HeSoAnhHuong"], string.Empty);
                        info.ChiSo = Utils.ConvertToInt32(dr["ChiSo"], 0);
                        info.RankSCIMAG = Utils.ConvertToInt32(dr["RankSCIMAG"], 0);
                        info.DiemTapChi = Utils.ConvertToDecimal(dr["DiemTapChi"], 0);
                        info.CapHoiThao = Utils.ConvertToInt32(dr["CapHoiThao"], 0);
                        info.NgayHoiThao = Utils.ConvertToNullableDateTime(dr["NgayHoiThao"], null);
                        info.DiaDiemToChuc = Utils.ConvertToString(dr["DiaDiemToChuc"], string.Empty);
                        info.LoaiDaoTao = Utils.ConvertToInt32(dr["LoaiDaoTao"], 0);
                        info.TenHocVien = Utils.ConvertToString(dr["TenHocVien"], string.Empty);
                        info.TenLuanVan = Utils.ConvertToString(dr["TenLuanVan"], string.Empty);
                        info.NguoiHuongDan = Utils.ConvertToString(dr["NguoiHuongDan"], string.Empty);
                        info.NamBaoVe = Utils.ConvertToInt32(dr["NamBaoVe"], 0);
                        info.GhiChu = Utils.ConvertToString(dr["GhiChu"], string.Empty);
                        info.NamXuatBan = Utils.ConvertToInt32NullAble(dr["NamXuatBan"], null);

                        info.DeCuong = Utils.ConvertToString(dr["DeCuong"], string.Empty);
                        info.DeTai = Utils.ConvertToInt32(dr["DeTai"], 0);
                        info.TenHuongNghienCuuChinh = Utils.ConvertToString(dr["HuongNghienCuuChinh"], string.Empty);

                        info.TacGiaID = Utils.ConvertToInt32(dr["TacGiaID"], 0);
                        info.CanBoTGID = Utils.ConvertToInt32(dr["CanBoTGID"], 0);
                        info.CoQuanTGID = Utils.ConvertToInt32(dr["CoQuanTGID"], 0);
                        info.TenTacGia = Utils.ConvertToString(dr["TenTacGia"], string.Empty);

                        Result.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            List<NhaKhoaHocModel> nkh = new List<NhaKhoaHocModel>();
            var thongTinCTNhaKhoaHocs = Result;
            if (thongTinCTNhaKhoaHocs.Count > 0)
            {
                nkh = (from m in thongTinCTNhaKhoaHocs
                          group m by m.CanBoID into ctt
                          from item in ctt
                          select new NhaKhoaHocModel()
                          {
                              CanBoID = item.CanBoID,
                              BaiBaoTapChi = (from i in thongTinCTNhaKhoaHocs.Where(x => x.CTNhaKhoaHocID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.BaiBaoTapChi).ToList().GroupBy(x => x.CTNhaKhoaHocID)
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
                                                  ListTacGia = ((from j in thongTinCTNhaKhoaHocs.Where(x => x.CTNhaKhoaHocID > 0 && x.TacGiaID > 0 && x.CTNhaKhoaHocID == i.FirstOrDefault().CTNhaKhoaHocID).ToList().GroupBy(x => x.TacGiaID)
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
                              KetQuaNghienCuu = (from i in thongTinCTNhaKhoaHocs.Where(x => x.CTNhaKhoaHocID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.KetQuaNghienCuu).ToList().GroupBy(x => x.CTNhaKhoaHocID)
                                                 select new KetQuaNghienCuuNKHModel()
                                                 {
                                                     CTNhaKhoaHocID = i.FirstOrDefault().CTNhaKhoaHocID,
                                                     DeTai = i.FirstOrDefault().DeTai,
                                                     LoaiNhiemVu = i.FirstOrDefault().LoaiNhiemVu,
                                                     NhiemVu = i.FirstOrDefault().NhiemVu,
                                                     TieuDe = i.FirstOrDefault().TieuDe,
                                                     NamXuatBan = i.FirstOrDefault().NamXuatBan,
                                                     GhiChu = i.FirstOrDefault().GhiChu,
                                                     Disable = true,
                                                     FileDinhKem = new List<FileDinhKemModel>(),
                                                     ListTacGia = ((from j in thongTinCTNhaKhoaHocs.Where(x => x.CTNhaKhoaHocID > 0 && x.TacGiaID > 0 && x.CTNhaKhoaHocID == i.FirstOrDefault().CTNhaKhoaHocID).ToList().GroupBy(x => x.TacGiaID)
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
                              SachChuyenKhao = (from i in thongTinCTNhaKhoaHocs.Where(x => x.CTNhaKhoaHocID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTinNhaKhoaHoc.SachChuyenKhao).ToList().GroupBy(x => x.CTNhaKhoaHocID)
                                                select new SachChuyenKhaoModel()
                                                {
                                                    CTNhaKhoaHocID = i.FirstOrDefault().CTNhaKhoaHocID,
                                                    DeTai = i.FirstOrDefault().DeTai,
                                                    LoaiNhiemVu = i.FirstOrDefault().LoaiNhiemVu,
                                                    NhiemVu = i.FirstOrDefault().NhiemVu,
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
                                                    Disable = true,
                                                    FileDinhKem = new List<FileDinhKemModel>(),
                                                    ListTacGia = ((from j in thongTinCTNhaKhoaHocs.Where(x => x.CTNhaKhoaHocID > 0 && x.TacGiaID > 0 && x.CTNhaKhoaHocID == i.FirstOrDefault().CTNhaKhoaHocID).ToList().GroupBy(x => x.TacGiaID)
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
                          }
                        ).ToList();
            }
            var data = new NhaKhoaHocModel();
            data = nkh.FirstOrDefault() ?? new NhaKhoaHocModel();

            return data;
        }

        /// <summary>
        /// Lấy thông tin bài báo, sách, sản phẩm từ đề tài
        /// </summary>
        /// <param name="CanBoID"></param>
        /// <param name="CoQuanID"></param>
        /// <returns></returns>
        public NhaKhoaHocModel GetThongTinTuDeTai(int CanBoID, int CoQuanID)
        {
            List<ThongTinCTNhaKhoaHocModel> Result = new List<ThongTinCTNhaKhoaHocModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CanBoID",SqlDbType.Int),
                new SqlParameter("@CoQuanID",SqlDbType.Int),
            };
            parameters[0].Value = CanBoID;
            parameters[1].Value = CoQuanID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_LyLichKhoaHoc_GetChiTietThongTinDeTai", parameters))
                {
                    while (dr.Read())
                    {
                        ThongTinCTNhaKhoaHocModel info = new ThongTinCTNhaKhoaHocModel();
                        info.ChiTietDeTaiID = Utils.ConvertToInt32(dr["ChiTietDeTaiID"], 0);
                        info.LoaiThongTin = Utils.ConvertToInt32(dr["LoaiThongTin"], 0);
                        info.KhoangThoiGian = Utils.ConvertToString(dr["KhoangThoiGian"], string.Empty);

                        info.TieuDe = Utils.ConvertToString(dr["TieuDe"], string.Empty);
                        info.TacGia = Utils.ConvertToString(dr["TacGia"], string.Empty);
                        info.TenTapChiSachHoiThao = Utils.ConvertToString(dr["TenTapChiSachHoiThao"], string.Empty);
                        info.So = Utils.ConvertToString(dr["So"], string.Empty);
                        info.Trang = Utils.ConvertToString(dr["Trang"], string.Empty);
                        info.NhaXuatBan = Utils.ConvertToString(dr["NhaXuatBan"], string.Empty);
                        info.LoaiBaiBao = Utils.ConvertToInt32(dr["LoaiBaiBao"], 0);
                        info.ISSN = Utils.ConvertToString(dr["ISSN"], string.Empty);
                        info.NhiemVuBaiBao = Utils.ConvertToInt32(dr["NhiemVuBaiBao"], 0);
                        info.LoaiNhiemVu = Utils.ConvertToInt32(dr["LoaiNhiemVu"], 0);
                        info.TenHoiThao = Utils.ConvertToString(dr["TenHoiThao"], string.Empty);

                        info.Tap = Utils.ConvertToString(dr["Tap"], string.Empty);
                        info.NamDangTai = Utils.ConvertToInt32(dr["NamDangTai"], 0);
                        info.LinkBaiBao = Utils.ConvertToString(dr["LinkBaiBao"], string.Empty);
                        info.LinhVucNganhKhoaHoc = Utils.ConvertToInt32(dr["LinhVucNganhKhoaHoc"], 0);
                        info.HeSoAnhHuong = Utils.ConvertToString(dr["HeSoAnhHuong"], string.Empty);
                        info.ChiSo = Utils.ConvertToInt32(dr["ChiSo"], 0);
                        info.RankSCIMAG = Utils.ConvertToInt32(dr["RankSCIMAG"], 0);
                        info.DiemTapChi = Utils.ConvertToDecimal(dr["DiemTapChi"], 0);
                        info.CapHoiThao = Utils.ConvertToInt32(dr["CapHoiThao"], 0);
                        info.NgayHoiThao = Utils.ConvertToNullableDateTime(dr["NgayHoiThao"], null);
                        info.DiaDiemToChuc = Utils.ConvertToString(dr["DiaDiemToChuc"], string.Empty);
                        info.LoaiDaoTao = Utils.ConvertToInt32(dr["LoaiDaoTao"], 0);
                        info.TenHocVien = Utils.ConvertToString(dr["TenHocVien"], string.Empty);
                        info.TenLuanVan = Utils.ConvertToString(dr["TenLuanVan"], string.Empty);
                        info.NguoiHuongDan = Utils.ConvertToString(dr["NguoiHuongDan"], string.Empty);
                        info.NamBaoVe = Utils.ConvertToInt32(dr["NamBaoVe"], 0);
                        info.GhiChu = Utils.ConvertToString(dr["GhiChu"], string.Empty);

                        info.NamXuatBan = Utils.ConvertToInt32NullAble(dr["NamXuatBan"], null);
                        //info.DeCuong = Utils.ConvertToString(dr["DeCuong"], string.Empty);
                        //info.DeTai = Utils.ConvertToInt32(dr["DeTai"], 0);
                        //info.TenHuongNghienCuuChinh = Utils.ConvertToString(dr["HuongNghienCuuChinh"], string.Empty);

                        info.TacGiaID = Utils.ConvertToInt32(dr["TacGiaID"], 0);
                        info.CanBoTGID = Utils.ConvertToInt32(dr["CanBoTGID"], 0);
                        info.CoQuanTGID = Utils.ConvertToInt32(dr["CoQuanTGID"], 0);
                        info.TenTacGia = Utils.ConvertToString(dr["TenTacGia"], string.Empty);

                        Result.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            List<NhaKhoaHocModel> nkh = new List<NhaKhoaHocModel>();
            var thongTinCTNhaKhoaHocs = Result;
            if (thongTinCTNhaKhoaHocs.Count > 0)
            {
                nkh = (from m in thongTinCTNhaKhoaHocs
                       group m by m.CanBoID into ctt
                       from item in ctt
                       select new NhaKhoaHocModel()
                       {
                           CanBoID = item.CanBoID,
                           BaiBaoTapChi = (from i in thongTinCTNhaKhoaHocs.Where(x => x.ChiTietDeTaiID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTin.BaiBaoTapChi).ToList().GroupBy(x => x.ChiTietDeTaiID)
                                           select new BaiBaoTapChiModel()
                                           {
                                               ChiTietDeTaiID = i.FirstOrDefault().ChiTietDeTaiID,
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
                                               ListTacGia = ((from j in thongTinCTNhaKhoaHocs.Where(x => x.ChiTietDeTaiID > 0 && x.TacGiaID > 0 && x.ChiTietDeTaiID == i.FirstOrDefault().ChiTietDeTaiID).ToList().GroupBy(x => x.TacGiaID)
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
                           KetQuaNghienCuu = (from i in thongTinCTNhaKhoaHocs.Where(x => x.ChiTietDeTaiID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTin.KetQuaNghienCuu).ToList().GroupBy(x => x.ChiTietDeTaiID)
                                              select new KetQuaNghienCuuNKHModel()
                                              {
                                                  ChiTietDeTaiID = i.FirstOrDefault().ChiTietDeTaiID,
                                                  DeTai = i.FirstOrDefault().DeTai,
                                                  LoaiNhiemVu = i.FirstOrDefault().LoaiNhiemVu,
                                                  NhiemVu = i.FirstOrDefault().NhiemVu,
                                                  TieuDe = i.FirstOrDefault().TieuDe,
                                                  NamXuatBan = i.FirstOrDefault().NamXuatBan,
                                                  GhiChu = i.FirstOrDefault().GhiChu,
                                                  Disable = true,
                                                  FileDinhKem = new List<FileDinhKemModel>(),
                                                  ListTacGia = ((from j in thongTinCTNhaKhoaHocs.Where(x => x.ChiTietDeTaiID > 0 && x.TacGiaID > 0 && x.ChiTietDeTaiID == i.FirstOrDefault().ChiTietDeTaiID).ToList().GroupBy(x => x.TacGiaID)
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
                           SachChuyenKhao = (from i in thongTinCTNhaKhoaHocs.Where(x => x.ChiTietDeTaiID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTin.SachChuyenKhao).ToList().GroupBy(x => x.ChiTietDeTaiID)
                                             select new SachChuyenKhaoModel()
                                             {
                                                 ChiTietDeTaiID = i.FirstOrDefault().ChiTietDeTaiID,
                                                 DeTai = i.FirstOrDefault().DeTai,
                                                 LoaiNhiemVu = i.FirstOrDefault().LoaiNhiemVu,
                                                 NhiemVu = i.FirstOrDefault().NhiemVu,
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
                                                 Disable = true,
                                                 FileDinhKem = new List<FileDinhKemModel>(),
                                                 ListTacGia = ((from j in thongTinCTNhaKhoaHocs.Where(x => x.ChiTietDeTaiID > 0 && x.TacGiaID > 0 && x.ChiTietDeTaiID == i.FirstOrDefault().ChiTietDeTaiID).ToList().GroupBy(x => x.TacGiaID)
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
                           SanPhamDaoTao = (from i in thongTinCTNhaKhoaHocs.Where(x => x.ChiTietDeTaiID > 0 && x.LoaiThongTin == (int)EnumLoaiThongTin.SanPhamDaoTao).ToList().GroupBy(x => x.ChiTietDeTaiID)
                                            select new SanPhamDaoTaoModel()
                                            {
                                                ChiTietDeTaiID = i.FirstOrDefault().ChiTietDeTaiID ?? 0,
                                                DeTai = i.FirstOrDefault().DeTai,
                                                NhiemVu = i.FirstOrDefault().NhiemVuBaiBao,
                                                LoaiNhiemVu = i.FirstOrDefault().LoaiNhiemVu,
                                                LoaiDaoTao = i.FirstOrDefault().LoaiDaoTao,
                                                TenHocVien = i.FirstOrDefault().TenHocVien,
                                                TenLuanVan = i.FirstOrDefault().TenLuanVan,
                                                NguoiHuongDan = i.FirstOrDefault().NguoiHuongDan,
                                                NamBaoVe = i.FirstOrDefault().NamBaoVe,
                                                Disable = true,
                                            }
                                                     ).ToList(),
                       }
                        ).ToList();
            }
            var data = new NhaKhoaHocModel();
            data = nkh.FirstOrDefault() ?? new NhaKhoaHocModel();

            return data;
        }

        /// <summary>
        /// Đếm số lượng bài báo, sách đã xuất bản của từng cơ quan theo năm
        /// </summary>
        /// <param name="Nam"></param>
        /// <returns></returns>
        public List<DanhMucCoQuanDonViModel> SoLuongBaiBaoVaSach(int Nam)
        {

            List<DanhMucCoQuanDonViModel> Result = new List<DanhMucCoQuanDonViModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Nam",SqlDbType.Int),
            };
            parameters[0].Value = Nam;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_ThongTinCTNhaKhoaHoc_SLBaiBaoAndSach_By_Nam", parameters))
                {
                    while (dr.Read())
                    {
                        DanhMucCoQuanDonViModel info = new DanhMucCoQuanDonViModel();
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.SoLuongBaiBao = Utils.ConvertToInt32(dr["SoLuongBaiBao"], 0);
                        info.SoLuongSach = Utils.ConvertToInt32(dr["SoLuongSach"], 0);

                        Result.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                return new List<DanhMucCoQuanDonViModel>();
                throw ex;
            }

            return Result;
        }


    }
}
