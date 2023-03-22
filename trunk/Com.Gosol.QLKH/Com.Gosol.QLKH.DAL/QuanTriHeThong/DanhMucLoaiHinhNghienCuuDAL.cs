using Com.Gosol.QLKH.DAL.QuanTriHeThong;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.DanhMuc;
using Com.Gosol.QLKH.Models.QuanTriHeThong;
using Com.Gosol.QLKH.Ultilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.Gosol.QLKH.DAL.DanhMuc
{
    public interface IDanhMucLoaiHinhNghienCuuDAL
    {
        public List<DanhMucLoaiHinhNghienCuuModel> GetPagingBySearch(BasePagingParams p, ref int TotalCount);
        public List<DanhMucLoaiHinhNghienCuuModel> GetAllDangSuDung();
        public BaseResultModel Insert(DanhMucLoaiHinhNghienCuuModel DanhMucLoaiHinhNghienCuuModel);
        public BaseResultModel Update(DanhMucLoaiHinhNghienCuuModel DanhMucLoaiHinhNghienCuuModel, int CoQuanID);
        public BaseResultModel Delete(List<int> ListId);
        public DanhMucLoaiHinhNghienCuuModel GetByID(int? Id);
    }
    public class DanhMucLoaiHinhNghienCuuDAL : IDanhMucLoaiHinhNghienCuuDAL
    {
        private const string INSERT = @"v1_DanhMuc_LoaiHinhNghienCuu_Insert";
        private const string UPDATE = @"v1_DanhMuc_LoaiHinhNghienCuu_Update";
        private const string DELETE = @"v1_DanhMuc_LoaiHinhNghienCuu_Delete";
        private const string GETBYID = @"v1_DanhMuc_LoaiHinhNghienCuu_GetByID";
        private const string GETBYNAME = @"v1_DanhMuc_LoaiHinhNghienCuu_GetByName";
        private const string GETLISTPAGING = @"v1_DanhMuc_LoaiHinhNghienCuu_GetPagingBySearch";
        private const string GETAll = @"v1_DanhMuc_LoaiHinhNghienCuu_GetAll";
        // param constant
        private const string PARAM_Id = @"Id";
        private const string PARAM_Name = @"Name";
        private const string PARAM_Status = @"Status";
        private const string PARAM_Note = @"Note";
        // Insert
        public BaseResultModel Insert(DanhMucLoaiHinhNghienCuuModel DanhMucLoaiHinhNghienCuuModel)
        {
            var Result = new BaseResultModel();
            try
            {
                if (DanhMucLoaiHinhNghienCuuModel == null || DanhMucLoaiHinhNghienCuuModel.Name == null || DanhMucLoaiHinhNghienCuuModel.Name.Trim().Length < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Tên loại hình nghiên cứu không được trống";
                }
                else if (DanhMucLoaiHinhNghienCuuModel.Name.Trim().Length > 50)
                {
                    Result.Status = 0;
                    Result.Message = "Tên loại hình nghiên cứu không được quá 50 ký tự";
                }
                else
                {
                    var LoaiHinhNghienCuu = GetByName(DanhMucLoaiHinhNghienCuuModel.Name);
                    if (LoaiHinhNghienCuu != null && LoaiHinhNghienCuu.Id > 0)
                    {
                        Result.Status = 0;
                        Result.Message = "Loại hình nghiên cứu đã tồn tại";
                    }
                    else
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                          {
                            new SqlParameter(PARAM_Name, SqlDbType.NVarChar),
                            new SqlParameter(PARAM_Note, SqlDbType.NVarChar),
                            new SqlParameter(PARAM_Status, SqlDbType.Bit)
                          };
                        parameters[0].Value = DanhMucLoaiHinhNghienCuuModel.Name.Trim();
                        parameters[1].Value = DanhMucLoaiHinhNghienCuuModel.Note.Trim();
                        parameters[2].Value = DanhMucLoaiHinhNghienCuuModel.Status == null ? false : DanhMucLoaiHinhNghienCuuModel.Status;
                        using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                        {
                            conn.Open();
                            using (SqlTransaction trans = conn.BeginTransaction())
                            {
                                try
                                {
                                    Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, INSERT, parameters);
                                    trans.Commit();
                                    Result.Message = ConstantLogMessage.Alert_Insert_Success("danh mục loại hình nghiên cứu");
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
        // Update
        public BaseResultModel Update(DanhMucLoaiHinhNghienCuuModel DanhMucLoaiHinhNghienCuuModel, int CoQuanID)
        {
            var Result = new BaseResultModel();
            try
            {
                int crID;
                if (!int.TryParse(DanhMucLoaiHinhNghienCuuModel.Id.ToString(), out crID) || DanhMucLoaiHinhNghienCuuModel.Id < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Loại hình nghiên cứu không tồn tại";
                }
                else
                {
                    
                    SqlParameter[] parameters = new SqlParameter[]
                     {
                            new SqlParameter(PARAM_Id, SqlDbType.Int),
                            new SqlParameter(PARAM_Name, SqlDbType.NVarChar),
                            new SqlParameter(PARAM_Note, SqlDbType.NVarChar),
                            new SqlParameter(PARAM_Status, SqlDbType.Bit)
                     };

                    parameters[0].Value = DanhMucLoaiHinhNghienCuuModel.Id;
                    parameters[1].Value = DanhMucLoaiHinhNghienCuuModel.Name;
                    parameters[2].Value = DanhMucLoaiHinhNghienCuuModel.Note;
                    parameters[3].Value = DanhMucLoaiHinhNghienCuuModel.Status;

                    using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                    {
                        conn.Open();
                        using (SqlTransaction trans = conn.BeginTransaction())
                        {
                            try
                            {
                                // Cập nhật Role sử dụng
                                Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, UPDATE, parameters);
                                trans.Commit();
                                Result.Message = ConstantLogMessage.Alert_Update_Success("danh mục loại hình nghiên cứu");
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
                    //}
                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                throw;
            }
            return Result;
        }
        public BaseResultModel Delete(List<int> ListId)
        {
            BaseResultModel Result = new BaseResultModel();
            if (ListId.Count <= 0)
            {

                Result.Message = "Vui lòng chọn dữ liệu trước khi xóa";
                Result.Status = 0;
                return Result;
            }
            else
            {
                for (int i = 0; i < ListId.Count; i++)
                {

                    int crID;
                    if (!int.TryParse(ListId[i].ToString(), out crID))
                    {
                        Result.Message = "Loại hình nghiên cứu '" + ListId[i].ToString() + "' không đúng định dạng";
                        Result.Status = 0;
                        return Result;
                    }
                    else
                    {
                        var crObj = GetByID(ListId[i]);
                        if (crObj == null || crObj.Id < 1)
                        {
                            Result.Message = "Id " + GetByID(ListId[i]).Name + " không tồn tại";
                            Result.Status = 0;
                            return Result;
                        }
                        //else if (new HeThongCanBoDAL().GetCanBoById(ListId[i]).Count > 0)
                        //{
                        //    Result.Message = "Trạng thái " + GetByID(ListId[i]).Name + " đang được sử dụng ! Không thể xóa";
                        //    Result.Status = 0;
                        //    return Result;
                        //}
                        else
                        {
                            SqlParameter[] parameters = new SqlParameter[]
                             {
                                new SqlParameter(PARAM_Id, SqlDbType.Int)
                             };
                            parameters[0].Value = ListId[i];
                            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                            {
                                conn.Open();
                                using (SqlTransaction trans = conn.BeginTransaction())
                                {
                                    try
                                    {
                                        int val = 0;
                                        val = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, DELETE, parameters);
                                        trans.Commit();


                                    }
                                    catch
                                    {
                                        Result.Status = -1;
                                        Result.Message = ConstantLogMessage.API_Error_System;
                                        trans.Rollback();
                                        return Result;
                                        throw;
                                    }
                                }
                            }
                        }
                    }
                }
                Result.Status = 1;
                Result.Message = ConstantLogMessage.Alert_Delete_Success("Danh mục Loại hình nghiên cứu");
                return Result;
            }
        }
        public DanhMucLoaiHinhNghienCuuModel GetByID(int? Id)
        {
            DanhMucLoaiHinhNghienCuuModel LoaiHinhNghienCuu = null;
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter(PARAM_Id,SqlDbType.Int)
              };
            parameters[0].Value = Id ?? Convert.DBNull;
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, GETBYID, parameters))
                {
                    while (dr.Read())
                    {
                        LoaiHinhNghienCuu = new DanhMucLoaiHinhNghienCuuModel();
                        LoaiHinhNghienCuu.Id = Utils.ConvertToInt32(dr["Id"], 0);
                        LoaiHinhNghienCuu.Name = Utils.ConvertToString(dr["Name"], string.Empty);
                        LoaiHinhNghienCuu.Note = Utils.ConvertToString(dr["Note"], string.Empty);
                        LoaiHinhNghienCuu.Status = Utils.ConvertToBoolean(dr["Status"], false);
                        break;
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return LoaiHinhNghienCuu;
        }
        public DanhMucLoaiHinhNghienCuuModel GetByName(string Name)
        {
            DanhMucLoaiHinhNghienCuuModel LoaiHinhNghienCuu = new DanhMucLoaiHinhNghienCuuModel();
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter(@"Keyword",SqlDbType.NVarChar)
              };
            parameters[0].Value = Name.Trim();
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, GETBYNAME, parameters))
                {
                    while (dr.Read())
                    {
                        LoaiHinhNghienCuu = new DanhMucLoaiHinhNghienCuuModel();
                        LoaiHinhNghienCuu.Id = Utils.ConvertToInt32(dr["Id"], 0);
                        LoaiHinhNghienCuu.Name = Utils.ConvertToString(dr["Name"], string.Empty);
                        LoaiHinhNghienCuu.Note = Utils.ConvertToString(dr["Note"], string.Empty);
                        LoaiHinhNghienCuu.Status = Utils.ConvertToBoolean(dr["Status"], false);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return LoaiHinhNghienCuu;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="TotalRow"></param>
        /// <returns></returns>
        public List<DanhMucLoaiHinhNghienCuuModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow)
        {
            List<DanhMucLoaiHinhNghienCuuModel> list = new List<DanhMucLoaiHinhNghienCuuModel>();
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("Keyword",SqlDbType.NVarChar),
                new SqlParameter("OrderByName",SqlDbType.NVarChar),
                new SqlParameter("OrderByOption",SqlDbType.NVarChar),
                new SqlParameter("pLimit",SqlDbType.Int),
                new SqlParameter("pOffset",SqlDbType.Int),
                new SqlParameter("TotalRow",SqlDbType.Int),

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
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GETLISTPAGING, parameters))
                {
                    while (dr.Read())
                    {
                        DanhMucLoaiHinhNghienCuuModel item = new DanhMucLoaiHinhNghienCuuModel();
                        item.Id = Utils.ConvertToInt32(dr["Id"], 0);
                        item.Name = Utils.ConvertToString(dr["Name"], string.Empty);
                        item.Note = Utils.ConvertToString(dr["Note"], string.Empty);
                        item.Status = Utils.ConvertToBoolean(dr["Status"], false);
                        list.Add(item);
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
        public List<DanhMucLoaiHinhNghienCuuModel> GetAllDangSuDung()
        {
            List<DanhMucLoaiHinhNghienCuuModel> list = new List<DanhMucLoaiHinhNghienCuuModel>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GETAll))
                {
                    while (dr.Read())
                    {
                        DanhMucLoaiHinhNghienCuuModel item = new DanhMucLoaiHinhNghienCuuModel();
                        item.Id = Utils.ConvertToInt32(dr["Id"], 0);
                        item.Name = Utils.ConvertToString(dr["Name"], string.Empty);
                        item.Note = Utils.ConvertToString(dr["Note"], string.Empty);
                        item.Status = Utils.ConvertToBoolean(dr["Status"], false);
                        if (item.Status)
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
       
    }
}
