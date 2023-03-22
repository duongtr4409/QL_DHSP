using Com.Gosol.QLKH.DAL.QuanTriHeThong;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.DanhMuc;
using Com.Gosol.QLKH.Models.QuanTriHeThong;
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

namespace Com.Gosol.QLKH.DAL.QuanTriHeThong
{
    public interface IDanhMucBuocThucHienDAL
    {
        public List<DanhMucBuocThucHienModel> GetPagingBySearch(BasePagingParams p, ref int TotalCount);
        public BaseResultModel Insert(DanhMucBuocThucHienModel DanhMucChucVuModel);
        public BaseResultModel Update(DanhMucBuocThucHienModel DanhMucChucVuModel);
        public List<string> Delete(List<int> ListChucVuID);
        public DanhMucBuocThucHienModel GetBuocThucHienByID(int BuocThucHienID);
        public List<DanhMucBuocThucHienModel> GetAllDangSuDung();
    }
    public class DanhMucBuocThucHienDAL : IDanhMucBuocThucHienDAL
    {
        // param constant
        private const string PARAM_BUOCTHUCHIENID = "@BuocThucHienID";
        private const string PARAM_TENBUOCTHUCHIEN = "@TenBuocThucHien";
        private const string PARAM_GHICHU = "@GhiChu";
        private const string PARAM_TRANGTHAISUDUNG = "@TrangThaiSuDung";
        private const string PARAM_THUTUHIENTHI = "@ThuTuHienThi";
        private const string PARAM_MABUOCTHUCHIEN = "@MaBuocThucHien";
        private const string PARAM_KEYWORD = "@Keyword";

        public BaseResultModel Insert(DanhMucBuocThucHienModel DanhMucBuocThucHienModel)
        {
            var Result = new BaseResultModel();
            try
            {
                if (DanhMucBuocThucHienModel == null || DanhMucBuocThucHienModel.TenBuocThucHien == null || DanhMucBuocThucHienModel.TenBuocThucHien.Trim().Length < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Tên bước thực hiện không được trống";
                    return Result;
                }
                else if (DanhMucBuocThucHienModel.MaBuocThucHien == null || DanhMucBuocThucHienModel.MaBuocThucHien.Trim().Length < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Mã bước thực hiện không được trống";
                    return Result;
                }
                else
                {
                    //Kiểm tra tên trùng
                    var crBuocThucHien = GetBuocThucHienByName(DanhMucBuocThucHienModel.TenBuocThucHien);
                    if (crBuocThucHien != null && crBuocThucHien.BuocThucHienID > 0)
                    {
                        Result.Status = 0;
                        Result.Message = "Bước thực hiện đã tồn tại";
                        return Result;
                    }
                    //Kiểm tra mã trùng
                    var crBuocThucHienMa = GetBuocThucHienByName(DanhMucBuocThucHienModel.MaBuocThucHien);
                    if (crBuocThucHienMa != null && crBuocThucHienMa.BuocThucHienID > 0)
                    {
                        Result.Status = 0;
                        Result.Message = "Mã bước thực hiện đã tồn tại";
                        return Result;
                    }
                    else
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                           {
                            new SqlParameter(PARAM_TENBUOCTHUCHIEN, SqlDbType.NVarChar),
                            new SqlParameter(PARAM_THUTUHIENTHI, SqlDbType.Int),
                            new SqlParameter(PARAM_GHICHU, SqlDbType.NVarChar),
                            new SqlParameter(PARAM_TRANGTHAISUDUNG, SqlDbType.Bit),
                            new SqlParameter(PARAM_MABUOCTHUCHIEN, SqlDbType.NVarChar),

                           };
                        parameters[0].Value = DanhMucBuocThucHienModel.TenBuocThucHien.Trim();
                        parameters[1].Value = DanhMucBuocThucHienModel.ThuTuHienThi;
                        parameters[2].Value = DanhMucBuocThucHienModel.GhiChu;
                        parameters[3].Value = DanhMucBuocThucHienModel.TrangThaiSuDung;
                        parameters[4].Value = DanhMucBuocThucHienModel.MaBuocThucHien;
                        using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                        {
                            conn.Open();
                            using (SqlTransaction trans = conn.BeginTransaction())
                            {
                                try
                                {
                                    Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_BuocThucHien_Insert", parameters);
                                    trans.Commit();
                                    Result.Message = ConstantLogMessage.Alert_Insert_Success("danh mục bước thực hiện");
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
        public BaseResultModel Update(DanhMucBuocThucHienModel DanhMucBuocThucHienModel)
        {
            var Result = new BaseResultModel();
            try
            {
                if (DanhMucBuocThucHienModel == null || DanhMucBuocThucHienModel.TenBuocThucHien == null || DanhMucBuocThucHienModel.TenBuocThucHien.Trim().Length < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Tên bước thực hiện không được trống";
                    return Result;
                }
                else if (DanhMucBuocThucHienModel.MaBuocThucHien == null || DanhMucBuocThucHienModel.MaBuocThucHien.Trim().Length < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Mã bước thực hiện không được trống";
                    return Result;
                }
                else
                {
                    var crObj = GetBuocThucHienByID(DanhMucBuocThucHienModel.BuocThucHienID);
                    var objDouble = GetBuocThucHienByName(DanhMucBuocThucHienModel.TenBuocThucHien.Trim());
                    var objBuocThucHienByMa = GetBuocThucHienByName(DanhMucBuocThucHienModel.MaBuocThucHien.Trim());
                    if (crObj == null || crObj.BuocThucHienID < 1)
                    {
                        Result.Status = 0;
                        Result.Message = "Bước thực hiện không tồn tại";
                        return Result;
                    }
                    else if (objDouble != null && objDouble.BuocThucHienID > 0 && objDouble.BuocThucHienID != DanhMucBuocThucHienModel.BuocThucHienID)
                    {
                        Result.Status = 0;
                        Result.Message = "Bước thực hiện đã tồn tại";
                        return Result;
                    }
                    else if (objBuocThucHienByMa != null && objBuocThucHienByMa.BuocThucHienID > 0 && objBuocThucHienByMa.BuocThucHienID != DanhMucBuocThucHienModel.BuocThucHienID)
                    {
                        Result.Status = 0;
                        Result.Message = "Bước thực hiện đã tồn tại";
                        return Result;
                    }
                    else
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                                new SqlParameter(PARAM_BUOCTHUCHIENID, SqlDbType.Int),
                                new SqlParameter(PARAM_TENBUOCTHUCHIEN, SqlDbType.NVarChar),
                                new SqlParameter(PARAM_THUTUHIENTHI, SqlDbType.Int),
                                new SqlParameter(PARAM_GHICHU, SqlDbType.NVarChar),
                                new SqlParameter(PARAM_TRANGTHAISUDUNG, SqlDbType.Bit),
                                new SqlParameter(PARAM_MABUOCTHUCHIEN, SqlDbType.NVarChar),
                        };

                        parameters[0].Value = DanhMucBuocThucHienModel.BuocThucHienID;
                        parameters[1].Value = DanhMucBuocThucHienModel.TenBuocThucHien.Trim();
                        parameters[2].Value = DanhMucBuocThucHienModel.ThuTuHienThi;
                        parameters[3].Value = DanhMucBuocThucHienModel.GhiChu;
                        parameters[4].Value = DanhMucBuocThucHienModel.TrangThaiSuDung;
                        parameters[5].Value = DanhMucBuocThucHienModel.MaBuocThucHien;
                        using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                        {
                            conn.Open();
                            using (SqlTransaction trans = conn.BeginTransaction())
                            {
                                try
                                {
                                    Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_BuocThucHien_Update", parameters);
                                    trans.Commit();
                                    Result.Message = ConstantLogMessage.Alert_Update_Success("danh mục bước thực hiện");
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
        public List<string> Delete(List<int> ListBuocThucHienID)
        {
            List<string> dic = new List<string>();
            string message = "";
            if (ListBuocThucHienID.Count <= 0)
            {
                message = "Vui lòng chọn dữ liệu trước khi xóa!";
                dic.Add(message);
            }
            else
            {
                for (int i = 0; i < ListBuocThucHienID.Count; i++)
                {
                    if (GetBuocThucHienByID(ListBuocThucHienID[i]) != null)
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                            new SqlParameter(PARAM_BUOCTHUCHIENID, SqlDbType.Int)
                        };
                        parameters[0].Value = ListBuocThucHienID[i];
                        using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                        {
                            conn.Open();
                            using (SqlTransaction trans = conn.BeginTransaction())
                            {
                                try
                                {
                                    var val = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_BuocThucHien_Delete", parameters);
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
        public List<DanhMucBuocThucHienModel> GetAllDangSuDung()
        {
            List<DanhMucBuocThucHienModel> list = new List<DanhMucBuocThucHienModel>();
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_DanhMuc_BuocThucHien_GetAll"))
                {
                    while (dr.Read())
                    {
                        DanhMucBuocThucHienModel item = new DanhMucBuocThucHienModel();
                        item.BuocThucHienID = Utils.ConvertToInt32(dr["BuocThucHienID"], 0);
                        item.TenBuocThucHien = Utils.ConvertToString(dr["TenBuocThucHien"], string.Empty);
                        item.ThuTuHienThi = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        item.GhiChu = Utils.ConvertToString(dr["GhiChu"], string.Empty);
                        item.TrangThaiSuDung = Utils.ConvertToBoolean(dr["TrangThaiSuDung"], false);
                        item.MaBuocThucHien = Utils.ConvertToString(dr["MaBuocThucHien"], string.Empty);
                        if (item.TrangThaiSuDung)
                        {
                            list.Add(item);
                        }
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
        public DanhMucBuocThucHienModel GetBuocThucHienByID(int BuocThucHienID)
        {
            if (BuocThucHienID <= 0 || BuocThucHienID == null)
            {
                return new DanhMucBuocThucHienModel();
            }
            DanhMucBuocThucHienModel buocthuchien = null;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(PARAM_BUOCTHUCHIENID,SqlDbType.Int)
            };
            parameters[0].Value = BuocThucHienID;
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_BuocThucHien_GetByID", parameters))
                {
                    while (dr.Read())
                    {
                        buocthuchien = new DanhMucBuocThucHienModel();
                        buocthuchien.BuocThucHienID = Utils.ConvertToInt32(dr["BuocThucHienID"], 0);
                        buocthuchien.TenBuocThucHien = Utils.ConvertToString(dr["TenBuocThucHien"], string.Empty);
                        buocthuchien.GhiChu = Utils.ConvertToString(dr["GhiChu"], string.Empty);
                        buocthuchien.ThuTuHienThi = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        buocthuchien.TrangThaiSuDung = Utils.ConvertToBoolean(dr["TrangThaiSuDung"], false);
                        buocthuchien.MaBuocThucHien = Utils.ConvertToString(dr["MaBuocThucHien"], string.Empty);
                        break;
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return buocthuchien;
        }
        //Get by TenBuocThucHien or MaBuocThucHien
        public DanhMucBuocThucHienModel GetBuocThucHienByName(string TenBuocThucHien)
        {
            if (string.IsNullOrEmpty(TenBuocThucHien))
            {
                return new DanhMucBuocThucHienModel();
            }
            DanhMucBuocThucHienModel buocthuchien = new DanhMucBuocThucHienModel();
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter(PARAM_KEYWORD,SqlDbType.NVarChar)
              };
            parameters[0].Value = TenBuocThucHien.Trim();
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_BuocThucHien_GetByName", parameters))
                {
                    while (dr.Read())
                    {
                        buocthuchien.BuocThucHienID = Utils.ConvertToInt32(dr["BuocThucHienID"], 0);
                        buocthuchien.TenBuocThucHien = Utils.ConvertToString(dr["TenBuocThucHien"], string.Empty);
                        buocthuchien.GhiChu = Utils.ConvertToString(dr["GhiChu"], string.Empty);
                        buocthuchien.ThuTuHienThi = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        buocthuchien.TrangThaiSuDung = Utils.ConvertToBoolean(dr["TrangThaiSuDung"], false);
                        buocthuchien.MaBuocThucHien = Utils.ConvertToString(dr["MaBuocThucHien"], string.Empty);
                        break;
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return buocthuchien;
        }
        public List<DanhMucBuocThucHienModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow)
        {
            List<DanhMucBuocThucHienModel> list = new List<DanhMucBuocThucHienModel>();
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

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_BuocThucHien_GetPagingBySearch", parameters))
                {
                    while (dr.Read())
                    {
                        DanhMucBuocThucHienModel buocthuchien = new DanhMucBuocThucHienModel();
                        buocthuchien.BuocThucHienID = Utils.ConvertToInt32(dr["BuocThucHienID"], 0);
                        buocthuchien.TenBuocThucHien = Utils.ConvertToString(dr["TenBuocThucHien"], string.Empty);
                        buocthuchien.ThuTuHienThi = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        buocthuchien.GhiChu = Utils.ConvertToString(dr["GhiChu"], string.Empty);
                        buocthuchien.TrangThaiSuDung = Utils.ConvertToBoolean(dr["TrangThaiSuDung"], false);
                        buocthuchien.MaBuocThucHien = Utils.ConvertToString(dr["MaBuocThucHien"], string.Empty);
                        list.Add(buocthuchien);
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
