using Com.Gosol.QLKH.DAL.QuanTriHeThong;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.DanhMuc;
using Com.Gosol.QLKH.Models.QuanTriHeThong;
using Com.Gosol.QLKH.Models.QLKH;
using Com.Gosol.QLKH.Ultilities;
using MySql.Data.MySqlClient;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Com.Gosol.QLKH.DAL.QuanTriHeThong
{
    public interface IDanhMucBieuMauDAL
    {
        public List<DanhMucBieuMauModel> GetPagingBySearch(BasePagingParams p, ref int TotalCount);
        public BaseResultModel Insert(DanhMucBieuMauModel DanhMucChucVuModel);
        public BaseResultModel Update(DanhMucBieuMauModel DanhMucChucVuModel);
        public List<string> Delete(List<int> ListChucVuID);
        public DanhMucBieuMauModel GetBieuMauByID(int BieuMauID);
        public List<DanhMucBieuMauModel> GetAll();
    }
    public class DanhMucBieuMauDAL : IDanhMucBieuMauDAL
    {
        // param constant
        private const string PARAM_BIEUMAUID = "@BieuMauID";
        private const string PARAM_TENBIEUMAU = "@TenBieuMau";
        private const string PARAM_GHICHU = "@GhiChu";
        private const string PARAM_THUTUHIENTHI = "@ThuTuHienThi";
        private const string PARAM_KEYWORD = "@Keyword";

        public BaseResultModel Insert(DanhMucBieuMauModel DanhMucBieuMauModel)
        {
            var Result = new BaseResultModel();
            try
            {
                if (DanhMucBieuMauModel == null || DanhMucBieuMauModel.TenBieuMau == null || DanhMucBieuMauModel.TenBieuMau.Trim().Length < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Tên biểu mẫu không được trống";
                    return Result;
                }
                else
                {
                    //Kiểm tra tên trùng
                    var crBieuMau = GetBieuMauByName(DanhMucBieuMauModel.TenBieuMau);
                    if (crBieuMau != null && crBieuMau.BieuMauID > 0)
                    {
                        Result.Status = 0;
                        Result.Message = "Biểu mẫu đã tồn tại";
                        return Result;
                    }
                    else
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                           {
                            new SqlParameter(PARAM_TENBIEUMAU, SqlDbType.NVarChar),
                            new SqlParameter(PARAM_THUTUHIENTHI, SqlDbType.Int),
                            new SqlParameter(PARAM_GHICHU, SqlDbType.NVarChar),
                             new SqlParameter("NgayCapNhat", SqlDbType.DateTime),
                            new SqlParameter("NguoiCapNhat", SqlDbType.Int),
                           };
                        parameters[0].Value = DanhMucBieuMauModel.TenBieuMau.Trim();
                        parameters[1].Value = DanhMucBieuMauModel.ThuTuHienThi;
                        parameters[2].Value = DanhMucBieuMauModel.GhiChu ?? Convert.DBNull;
                        parameters[3].Value = DateTime.Now;
                        parameters[4].Value = DanhMucBieuMauModel.NguoiCapNhat ?? Convert.DBNull;
                        using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                        {
                            conn.Open();
                            using (SqlTransaction trans = conn.BeginTransaction())
                            {
                                try
                                {
                                    var val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, @"v1_DanhMuc_BieuMau_Insert", parameters);
                                    int BieuMauID = Utils.ConvertToInt32(val, 0);
                                    trans.Commit();
                                    Result.Status = BieuMauID;
                                    Result.Message = ConstantLogMessage.Alert_Insert_Success("danh mục biểu mẫu");
                                }
                                catch (Exception ex)
                                {
                                    Result.Status = -1;
                                    Result.Message = ConstantLogMessage.API_Error_System;
                                    trans.Rollback();
                                    throw ex;
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                throw ex;
            }
            return Result;
        }
        public BaseResultModel Update(DanhMucBieuMauModel DanhMucBieuMauModel)
        {
            var Result = new BaseResultModel();
            try
            {
                if (DanhMucBieuMauModel == null || DanhMucBieuMauModel.TenBieuMau == null || DanhMucBieuMauModel.TenBieuMau.Trim().Length < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Tên biểu mẫu không được trống";
                    return Result;
                }
                else
                {
                    var crObj = GetBieuMauByID(DanhMucBieuMauModel.BieuMauID);
                    var objDouble = GetBieuMauByName(DanhMucBieuMauModel.TenBieuMau.Trim());
                    if (crObj == null || crObj.BieuMauID < 1)
                    {
                        Result.Status = 0;
                        Result.Message = "Biểu mẫu không tồn tại";
                        return Result;
                    }
                    else if (objDouble != null && objDouble.BieuMauID > 0 && objDouble.BieuMauID != DanhMucBieuMauModel.BieuMauID)
                    {
                        Result.Status = 0;
                        Result.Message = "Biểu mẫu đã tồn tại";
                        return Result;
                    }
                    else
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                                new SqlParameter(PARAM_BIEUMAUID, SqlDbType.Int),
                                new SqlParameter(PARAM_TENBIEUMAU, SqlDbType.NVarChar),
                                new SqlParameter(PARAM_THUTUHIENTHI, SqlDbType.Int),
                                new SqlParameter(PARAM_GHICHU, SqlDbType.NVarChar),
                                new SqlParameter("NgayCapNhat", SqlDbType.DateTime),
                                new SqlParameter("NguoiCapNhat", SqlDbType.Int),
                        };

                        parameters[0].Value = DanhMucBieuMauModel.BieuMauID;
                        parameters[1].Value = DanhMucBieuMauModel.TenBieuMau.Trim();
                        parameters[2].Value = DanhMucBieuMauModel.ThuTuHienThi;
                        parameters[3].Value = DanhMucBieuMauModel.GhiChu;
                        parameters[4].Value = DateTime.Now;
                        parameters[5].Value = DanhMucBieuMauModel.NguoiCapNhat ?? Convert.DBNull;
                        using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                        {
                            conn.Open();
                            using (SqlTransaction trans = conn.BeginTransaction())
                            {
                                try
                                {
                                    Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_BieuMau_Update", parameters);
                                    trans.Commit();
                                    Result.Message = ConstantLogMessage.Alert_Update_Success("danh mục biểu mẫu");
                                    return Result;
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
                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                throw ex;
            }
        }

        [Obsolete]
        public List<string> Delete(List<int> ListBieuMauID)
        {
            List<string> dic = new List<string>();
            string message = "";
            if (ListBieuMauID.Count <= 0)
            {
                message = "Vui lòng chọn dữ liệu trước khi xóa!";
                dic.Add(message);
            }
            else
            {
                for (int i = 0; i < ListBieuMauID.Count; i++)
                {
                    if (GetBieuMauByID(ListBieuMauID[i]) != null)
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                            new SqlParameter(PARAM_BIEUMAUID, SqlDbType.Int)
                        };
                        parameters[0].Value = ListBieuMauID[i];
                        using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                        {
                            conn.Open();
                            using (SqlTransaction trans = conn.BeginTransaction())
                            {
                                try
                                {
                                    var val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, @"v1_DanhMuc_BieuMau_Delete", parameters);
                                    trans.Commit();
                                }
                                catch
                                {
                                    trans.Rollback();
                                    throw;
                                }
                            }
                        }
                    }
                }

            }
            return dic;
        }
        //Get All Đang sử dụng
        public List<DanhMucBieuMauModel> GetAll()
        {
            List<DanhMucBieuMauModel> list = new List<DanhMucBieuMauModel>();
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_DanhMuc_BieuMau_GetAll"))
                {
                    while (dr.Read())
                    {
                        DanhMucBieuMauModel item = new DanhMucBieuMauModel();
                        item.BieuMauID = Utils.ConvertToInt32(dr["BieuMauID"], 0);
                        item.TenBieuMau = Utils.ConvertToString(dr["TenBieuMau"], string.Empty);
                        item.ThuTuHienThi = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        item.GhiChu = Utils.ConvertToString(dr["GhiChu"], string.Empty);
                        item.NgayCapNhat = Utils.ConvertToNullableDateTime(dr["NgayCapNhat"], null);
                        item.NguoiCapNhat = Utils.ConvertToInt32NullAble(dr["NguoiCapNhat"], null);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        //Get By ID
        public DanhMucBieuMauModel GetBieuMauByID(int BieuMauID)
        {
            if (BieuMauID <= 0 || BieuMauID == null)
            {
                return new DanhMucBieuMauModel();
            }
            DanhMucBieuMauModel BieuMau = null;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(PARAM_BIEUMAUID,SqlDbType.Int)
            };
            parameters[0].Value = BieuMauID;
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_BieuMau_GetByID", parameters))
                {
                    while (dr.Read())
                    {
                        BieuMau = new DanhMucBieuMauModel();
                        BieuMau.BieuMauID = Utils.ConvertToInt32(dr["BieuMauID"], 0);
                        BieuMau.TenBieuMau = Utils.ConvertToString(dr["TenBieuMau"], string.Empty);
                        BieuMau.GhiChu = Utils.ConvertToString(dr["GhiChu"], string.Empty);
                        BieuMau.ThuTuHienThi = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        BieuMau.FileDinhKem = new FileDinhKemModel();
                        BieuMau.FileDinhKem.TenFileGoc = Utils.ConvertToString(dr["TenFileGoc"], string.Empty);
                        BieuMau.FileDinhKem.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        BieuMau.FileDinhKem.FileDinhKemID = Utils.ConvertToInt32(dr["FileDinhKemID"], 0);
                        BieuMau.NgayCapNhat = Utils.ConvertToNullableDateTime(dr["NgayCapNhat"], null);
                        BieuMau.NguoiCapNhat = Utils.ConvertToInt32NullAble(dr["NguoiCapNhat"], null);
                        break;
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return BieuMau;
        }
        //Get by TenBieuMau or MaBieuMau
        public DanhMucBieuMauModel GetBieuMauByName(string TenBieuMau)
        {
            if (string.IsNullOrEmpty(TenBieuMau))
            {
                return new DanhMucBieuMauModel();
            }
            DanhMucBieuMauModel BieuMau = new DanhMucBieuMauModel();
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter(PARAM_KEYWORD,SqlDbType.NVarChar)
              };
            parameters[0].Value = TenBieuMau.Trim();
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_BieuMau_GetByName", parameters))
                {
                    while (dr.Read())
                    {
                        BieuMau.BieuMauID = Utils.ConvertToInt32(dr["BieuMauID"], 0);
                        BieuMau.TenBieuMau = Utils.ConvertToString(dr["TenBieuMau"], string.Empty);
                        BieuMau.GhiChu = Utils.ConvertToString(dr["GhiChu"], string.Empty);
                        BieuMau.ThuTuHienThi = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        BieuMau.NgayCapNhat = Utils.ConvertToNullableDateTime(dr["NgayCapNhat"], null);
                        BieuMau.NguoiCapNhat = Utils.ConvertToInt32NullAble(dr["NguoiCapNhat"], null);
                        break;
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return BieuMau;
        }
        public List<DanhMucBieuMauModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow)
        {
            List<DanhMucBieuMauModel> list = new List<DanhMucBieuMauModel>();
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("@Keyword",SqlDbType.NVarChar),
                new SqlParameter("@OrderByName",SqlDbType.NVarChar),
                new SqlParameter("@OrderByOption",SqlDbType.NVarChar),
                new SqlParameter("@pLimit",SqlDbType.Int),
                new SqlParameter("@pOffset",SqlDbType.Int),
                new SqlParameter("@TotalRow",SqlDbType.Int)

              };
            parameters[0].Value = p.Keyword == null ? "" : p.Keyword.Trim();
            parameters[1].Value = p.OrderByName;
            parameters[2].Value = p.OrderByOption;
            parameters[3].Value = p.Limit;
            parameters[4].Value = p.Offset;
            parameters[5].Direction = ParameterDirection.Output;
            parameters[5].Size = 8;
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_BieuMau_GetPagingBySearch", parameters))
                {
                    while (dr.Read())
                    {
                        DanhMucBieuMauModel BieuMau = new DanhMucBieuMauModel();
                        BieuMau.BieuMauID = Utils.ConvertToInt32(dr["BieuMauID"], 0);
                        BieuMau.TenBieuMau = Utils.ConvertToString(dr["TenBieuMau"], string.Empty);
                        BieuMau.ThuTuHienThi = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        BieuMau.GhiChu = Utils.ConvertToString(dr["GhiChu"], string.Empty);
                        BieuMau.FileDinhKem = new FileDinhKemModel();
                        BieuMau.FileDinhKem.TenFileGoc = Utils.ConvertToString(dr["TenFileGoc"], string.Empty);
                        BieuMau.FileDinhKem.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        BieuMau.FileDinhKem.FileDinhKemID = Utils.ConvertToInt32(dr["FileDinhKemID"], 0);
                        BieuMau.NgayCapNhat = Utils.ConvertToNullableDateTime(dr["NgayCapNhat"], null);
                        BieuMau.NguoiCapNhat = Utils.ConvertToInt32NullAble(dr["NguoiCapNhat"], null);
                        list.Add(BieuMau);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parameters[5].Value, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
    }
}
