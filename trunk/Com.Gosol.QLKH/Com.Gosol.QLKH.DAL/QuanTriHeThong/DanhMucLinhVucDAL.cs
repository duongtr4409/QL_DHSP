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
using System.Net.Http.Headers;
using System.Text;

namespace Com.Gosol.QLKH.DAL.QuanTriHeThong
{
    public interface IDanhMucLinhVucDAL
    {
        public BaseResultModel Insert(DanhMucLinhVucModel item);
        public BaseResultModel InsertMulti(List<DanhMucLinhVucModel> items);
        public BaseResultModel Update(DanhMucLinhVucModel item);
        public BaseResultModel Delete(int? id);
        public DanhMucLinhVucModel GetByID(int id);
        public List<DanhMucLinhVucModel> GetAll(int? type, string keyword, bool? status);
        public List<DanhMucLinhVucModel> GetAllAndGroup(int? type, string keyword, bool? status);
    }
    public class DanhMucLinhVucDAL : IDanhMucLinhVucDAL
    {
        public BaseResultModel Insert(DanhMucLinhVucModel item)
        {
            var Result = new BaseResultModel();
            try
            {
                if (item == null || item.Name == null || item.Name.Trim().Length < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Tên không được trống";
                    return Result;
                }
                else if (item.Code == null || item.Code.Length < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Mã không được trống";
                    return Result;
                }
                else
                {
                    if (item.ParentId != null && item.ParentId > 0)
                    {
                        var prItem = GetByID(item.ParentId.Value);
                        if (prItem == null || prItem.Id < 1)
                        {
                            Result.Status = 0;
                            Result.Message = "ParenId không tồn tại";
                            return Result;
                        }
                    }
                    var crData = GetByCode(item.Code.Trim());
                    if (crData != null && crData.Count > 0)
                    {
                        Result.Status = 0;
                        Result.Message = "Mã đã tồn tại";
                        return Result;
                    }
                    var itemInDB = GetByNameAndParentId(item.Name, item.ParentId);
                    if (itemInDB != null && itemInDB.Count > 0)
                    {
                        Result.Status = 0;
                        Result.Message = "Tên đã tồn tại";
                        return Result;
                    }
                    SqlParameter[] parameters = new SqlParameter[]
                       {
                         new SqlParameter("Name", SqlDbType.NVarChar),
                         new SqlParameter("ParentId", SqlDbType.Int),
                         new SqlParameter("MappingId", SqlDbType.Int),
                         new SqlParameter("Status", SqlDbType.Bit),
                         new SqlParameter("Code", SqlDbType.NVarChar),
                         new SqlParameter("Type", SqlDbType.TinyInt),
                       };
                    parameters[0].Value = item.Name.Trim();
                    parameters[1].Value = item.ParentId ?? Convert.DBNull;
                    parameters[2].Value = item.MappingId ?? Convert.DBNull;
                    parameters[3].Value = item.Status;
                    parameters[4].Value = item.Code ?? Convert.DBNull;
                    parameters[5].Value = item.Type;
                    using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                    {
                        conn.Open();
                        using (SqlTransaction trans = conn.BeginTransaction())
                        {
                            try
                            {
                                Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_LinhVuc_Insert", parameters);
                                trans.Commit();
                                Result.Message = ConstantLogMessage.Alert_Insert_Success("danh mục lĩnh vực");
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
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                throw ex;
            }
            return Result;
        }

        /// <summary>
        /// thêm nhiều dòng
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public BaseResultModel InsertMulti(List<DanhMucLinhVucModel> items)
        {
            var Result = new BaseResultModel();
            try
            {
                if (items == null || items.Count < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Không có dữ liệu";
                    return Result;
                }
                else
                {
                    foreach (var item in items)
                    {
                        if (item.Code == null || item.Code.Length < 1)
                        {
                            Result.Status = 0;
                            Result.Message = "Mã không được trống";
                            return Result;
                        }
                        if (item == null || item.Name == null || item.Name.Trim().Length < 1)
                        {
                            Result.Status = 0;
                            Result.Message = "Tên không được trống";
                            return Result;
                        }
                        var itemCodeInDB = GetByCode(item.Code);
                        if (itemCodeInDB != null && itemCodeInDB.Count > 0)
                        {
                            Result.Status = 0;
                            Result.Message = "Mã đã tồn tại";
                            return Result;
                        }
                        //if (items.Where(x => x.Name == item.Name && x != item).ToList().Count > 0)
                        //{
                        //    Result.Status = 0;
                        //    Result.Message = "Name không được trùng!";
                        //    return Result;
                        //}
                        var itemInDB = GetByNameAndParentId(item.Name, item.ParentId);
                        if (itemInDB != null && itemInDB.Count > 0)
                        {
                            Result.Status = 0;
                            Result.Message = item.Name + " đã tồn tại";
                            return Result;
                        }
                        if (item.ParentId != null && item.ParentId > 0)
                        {
                            var prItem = GetByID(item.ParentId.Value);
                            if (prItem == null || prItem.Id < 1)
                            {
                                Result.Status = 0;
                                Result.Message = "ParenId không tồn tại";
                                return Result;
                            }
                        }
                    }
                    using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                    {
                        conn.Open();
                        using (SqlTransaction trans = conn.BeginTransaction())
                        {
                            try
                            {
                                foreach (var item in items)
                                {
                                    SqlParameter[] parameters = new SqlParameter[]
                                    {
                                        new SqlParameter("Name", SqlDbType.NVarChar),
                                        new SqlParameter("ParentId", SqlDbType.Int),
                                        new SqlParameter("MappingId", SqlDbType.Int),
                                        new SqlParameter("Status", SqlDbType.Bit),
                                        new SqlParameter("Code", SqlDbType.NVarChar),
                                        new SqlParameter("Type", SqlDbType.TinyInt),
                                    };
                                    parameters[0].Value = item.Name.Trim();
                                    parameters[1].Value = item.ParentId ?? Convert.DBNull;
                                    parameters[2].Value = item.MappingId ?? Convert.DBNull;
                                    parameters[3].Value = item.Status;
                                    parameters[4].Value = item.Code ?? Convert.DBNull;
                                    parameters[5].Value = item.Type;
                                    Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_LinhVuc_Insert", parameters);
                                }
                                trans.Commit();
                                Result.Message = ConstantLogMessage.Alert_Insert_Success("Danh mục lĩnh vực");
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
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                throw ex;
            }
            return Result;
        }

        public BaseResultModel Update(DanhMucLinhVucModel item)
        {
            var Result = new BaseResultModel();
            try
            {
                if (item.Id == null || item.Id < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng chọn dữ liệu trước khi thao tác";
                    return Result;
                }
                else if (item == null || item.Name == null || item.Name.Trim().Length < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Tên không được trống";
                    return Result;
                }
                else if (item == null || item.Code == null || item.Code.Trim().Length < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Mã không được trống";
                    return Result;
                }
                else
                {
                    var crObj = GetByID(item.Id);
                    if (crObj == null || crObj.Id < 1)
                    {
                        Result.Status = 0;
                        Result.Message = "Dữ liệu không tồn tại";
                        return Result;
                    }
                    else
                    {
                        var itemInDB = GetByNameAndParentId(item.Name, item.ParentId);
                        if (itemInDB != null && itemInDB.Where(x => x.Id != item.Id).ToList().Count > 0)
                        {
                            Result.Status = 0;
                            Result.Message = "Tên đã tồn tại";
                            return Result;
                        }
                        var itemCodeInDB = GetByCode(item.Code);
                        if (itemCodeInDB != null && itemCodeInDB.Where(x => x.Id != item.Id).ToList().Count > 0)
                        {
                            Result.Status = 0;
                            Result.Message = "Mã đã tồn tại";
                            return Result;
                        }

                        SqlParameter[] parameters = new SqlParameter[]
                        {
                            new SqlParameter("Id", SqlDbType.Int),
                            new SqlParameter("Name", SqlDbType.NVarChar),
                            new SqlParameter("ParentId", SqlDbType.Int),
                            new SqlParameter("MappingId", SqlDbType.Int),
                            new SqlParameter("Status", SqlDbType.Bit),
                            new SqlParameter("Code", SqlDbType.NVarChar),
                            new SqlParameter("Type", SqlDbType.TinyInt),
                        };
                        parameters[0].Value = item.Id;
                        parameters[1].Value = item.Name.Trim();
                        parameters[2].Value = item.ParentId ?? Convert.DBNull;
                        parameters[3].Value = item.MappingId ?? Convert.DBNull;
                        parameters[4].Value = item.Status;
                        parameters[5].Value = item.Code ?? Convert.DBNull;
                        parameters[6].Value = item.Type;
                        using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                        {
                            conn.Open();
                            using (SqlTransaction trans = conn.BeginTransaction())
                            {
                                try
                                {
                                    Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_LinhVuc_Update", parameters);
                                    trans.Commit();
                                    Result.Message = ConstantLogMessage.Alert_Update_Success("danh mục lĩnh vực");
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

        /// <summary>
        /// xóa lĩnh vực
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResultModel Delete(int? id)
        {
            BaseResultModel Result = new BaseResultModel();
            if (id == null || id < 1)
            {
                Result.Message = "Vui lòng chọn dữ liệu trước khi xóa";
                Result.Status = 0;
                return Result;
            }
            var crObj = GetByID(id.Value);
            if (crObj == null || crObj.Id < 1)
            {
                Result.Message = "Dữ liệu không tồn tại";
                Result.Status = 0;
                return Result;
            }
            var lstChilren = GetByParentID(id.Value);
            if (lstChilren != null && lstChilren.Count > 0)
            {
                Result.Message = "Dữ liệu đã được sử dụng, không thể xóa!";
                Result.Status = 0;
                return Result;
            }
            SqlParameter[] parameters = new SqlParameter[]
             {
                                new SqlParameter("Id", SqlDbType.Int)
             };
            parameters[0].Value = id;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        int val = 0;
                        val = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_DanhMuc_LinhVuc_Delete", parameters);
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
            Result.Status = 1;
            Result.Message = ConstantLogMessage.Alert_Delete_Success("Danh mục lĩnh vực");
            return Result;

        }

        /// <summary>
        /// lấy thông tin lĩnh vực theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DanhMucLinhVucModel GetByID(int id)
        {
            if (id <= 0 || id == null)
            {
                return new DanhMucLinhVucModel();
            }
            DanhMucLinhVucModel item = new DanhMucLinhVucModel();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("Id",SqlDbType.Int)
            };
            parameters[0].Value = id;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_LinhVuc_GetByID", parameters))
                {
                    while (dr.Read())
                    {
                        item.Id = Utils.ConvertToInt32(dr["Id"], 0);
                        item.Name = Utils.ConvertToString(dr["Name"], string.Empty);
                        item.ParentId = Utils.ConvertToInt32(dr["ParentId"], 0);
                        item.MappingId = Utils.ConvertToInt32(dr["MappingId"], 0);
                        item.Status = Utils.ConvertToBoolean(dr["Status"], false);
                        item.Type = Utils.ConvertToInt32(dr["Type"], 0);
                        item.Code = Utils.ConvertToString(dr["Code"], string.Empty);
                        break;
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return item;
        }

        public List<DanhMucLinhVucModel> GetByNameAndParentId(string name, int? ParentId)
        {
            List<DanhMucLinhVucModel> result = new List<DanhMucLinhVucModel>();
            if (name == null || name == "" || name.Length < 1)
            {
                return new List<DanhMucLinhVucModel>();
            }
            DanhMucLinhVucModel item = new DanhMucLinhVucModel();
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("name",SqlDbType.NVarChar),
                new SqlParameter("parentId",SqlDbType.Int)
           };
            parameters[0].Value = name;
            parameters[1].Value = ParentId ?? 0;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_LinhVuc_GetByNameAndParentId", parameters))
                {
                    while (dr.Read())
                    {
                        item.Id = Utils.ConvertToInt32(dr["Id"], 0);
                        item.Name = Utils.ConvertToString(dr["Name"], string.Empty);
                        item.ParentId = Utils.ConvertToInt32(dr["ParentId"], 0);
                        item.MappingId = Utils.ConvertToInt32(dr["MappingId"], 0);
                        item.Status = Utils.ConvertToBoolean(dr["Status"], false);
                        item.Type = Utils.ConvertToInt32(dr["Type"], 0);
                        item.Code = Utils.ConvertToString(dr["Code"], string.Empty);
                        result.Add(item);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return result;
        }

        public List<DanhMucLinhVucModel> GetByCode(string Code)
        {
            List<DanhMucLinhVucModel> result = new List<DanhMucLinhVucModel>();
            SqlParameter[] parameters = new SqlParameter[]
         {
                new SqlParameter("Code",SqlDbType.VarChar)
         };
            parameters[0].Value = Code;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_LinhVuc_GetByCode", parameters))
                {
                    while (dr.Read())
                    {
                        DanhMucLinhVucModel item = new DanhMucLinhVucModel();

                        item.Id = Utils.ConvertToInt32(dr["Id"], 0);
                        item.Name = Utils.ConvertToString(dr["Name"], string.Empty);
                        item.ParentId = Utils.ConvertToInt32(dr["ParentId"], 0);
                        item.MappingId = Utils.ConvertToInt32(dr["MappingId"], 0);
                        item.Status = Utils.ConvertToBoolean(dr["Status"], false);
                        item.Type = Utils.ConvertToInt32(dr["Type"], 0);
                        item.Code = Utils.ConvertToString(dr["Code"], string.Empty);
                        result.Add(item);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// danh sách tất cả lĩnh vực, phẳng
        /// </summary>
        /// <returns></returns>
        public List<DanhMucLinhVucModel> GetAll(int? type, string keyword, bool? status)
        {
            List<DanhMucLinhVucModel> list = new List<DanhMucLinhVucModel>();
            SqlParameter[] parameters = new SqlParameter[]
             {
                   //new SqlParameter("keyword",SqlDbType.NVarChar),
                   new SqlParameter("type",SqlDbType.Int),
                   new SqlParameter("status",SqlDbType.Bit)
             };
            //parameters[0].Value = keyword ?? "";
            parameters[0].Value = type ?? 0;
            parameters[1].Value = status ?? Convert.DBNull;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_LinhVuc_GetAll", parameters))
                {
                    while (dr.Read())
                    {
                        DanhMucLinhVucModel item = new DanhMucLinhVucModel();
                        item.Id = Utils.ConvertToInt32(dr["Id"], 0);
                        item.Name = Utils.ConvertToString(dr["Name"], string.Empty);
                        item.ParentId = Utils.ConvertToInt32(dr["ParentId"], 0);
                        item.MappingId = Utils.ConvertToInt32(dr["MappingId"], 0);
                        item.Status = Utils.ConvertToBoolean(dr["Status"], false);
                        item.Type = Utils.ConvertToInt32(dr["Type"], 0);
                        item.Code = Utils.ConvertToString(dr["Code"], string.Empty);
                        if (keyword != null && (item.Name.ToLower().Contains(keyword.ToLower()) || item.Code.ToLower().Contains(keyword.ToLower())))
                            item.Highlight = 1;
                        list.Add(item);
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

        /// <summary>
        /// danh sách tất cả lĩnh vực,
        ///  gom hình cây (cha - con)
        /// </summary>
        /// <returns></returns>
        public List<DanhMucLinhVucModel> GetAllAndGroup(int? type, string keyword, bool? status)
        {
            var result = new List<DanhMucLinhVucModel>();
            try
            {
                result = GetAll(type, keyword, status);
                result.ForEach(x => x.Children = result.Where(i => i.ParentId == x.Id).ToList());
                result.RemoveAll(x => x.ParentId > 0);
            }
            catch (Exception ex)
            {
                return result;
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// lấy danh sách con
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public List<DanhMucLinhVucModel> GetByParentID(int ParentID)
        {
            if (ParentID <= 0 || ParentID == null)
            {
                return new List<DanhMucLinhVucModel>();
            }
            List<DanhMucLinhVucModel> result = new List<DanhMucLinhVucModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("ParentId",SqlDbType.Int)
            };
            parameters[0].Value = ParentID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_LinhVuc_GetByParentID", parameters))
                {
                    while (dr.Read())
                    {
                        DanhMucLinhVucModel item = new DanhMucLinhVucModel();
                        item.Id = Utils.ConvertToInt32(dr["Id"], 0);
                        item.Name = Utils.ConvertToString(dr["Name"], string.Empty);
                        item.ParentId = Utils.ConvertToInt32(dr["ParentId"], 0);
                        item.MappingId = Utils.ConvertToInt32(dr["MappingId"], 0);
                        item.Status = Utils.ConvertToBoolean(dr["Status"], false);
                        item.Type = Utils.ConvertToInt32(dr["Type"], 0);
                        item.Code = Utils.ConvertToString(dr["Code"], string.Empty);
                        result.Add(item);
                    }
                    dr.Close();
                }
            }
            catch
            {
                return result;
                throw;
            }
            return result;
        }
    }
}
