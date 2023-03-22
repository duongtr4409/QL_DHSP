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
    public interface IDanhMucLoaiKetQuaDAL
    {
        public BaseResultModel Insert(DanhMucLoaiKetQuaModel item);
        public BaseResultModel InsertMulti(List<DanhMucLoaiKetQuaModel> items);
        public BaseResultModel Update(DanhMucLoaiKetQuaModel item);
        public BaseResultModel Delete(int? id);
        public DanhMucLoaiKetQuaModel GetByID(int id);
        public List<DanhMucLoaiKetQuaModel> GetAll(string keyword);
        public List<DanhMucLoaiKetQuaModel> GetAllAndGroup(string keyword, bool? status);
    }
    public class DanhMucLoaiKetQuaDAL : IDanhMucLoaiKetQuaDAL
    {
        //private const string INSERT = @"v1_DanhMuc_LoaiKetQua_Insert";
        //private const string UPDATE = @"v1_DanhMuc_LoaiKetQua_Update";
        //private const string DELETE = @"v1_DanhMuc_LoaiKetQua_Delete";
        //private const string GETBYID = @"v1_DanhMuc_LoaiKetQua_GetByID";
        //private const string GETBYNAME = @"v1_DanhMuc_LoaiKetQua_GetByName";
        //private const string GETLISTPAGING = @"v1_DanhMuc_LoaiKetQua_GetPagingBySearch";
        //private const string GETAll = @"v1_DanhMuc_LoaiKetQua_GetAll";
        //// param constant
        //private const string PARAM_LoaiKetQuaID = @"LoaiKetQuaID";
        //private const string PARAM_TenLoaiKetQua = @"TenLoaiKetQua";
        //private const string PARAM_TrangThaiSuDung = @"TrangThaiSuDung";
        //private const string PARAM_GhiChu = @"GhiChu";
        // Insert
        public BaseResultModel Insert(DanhMucLoaiKetQuaModel item)
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
                       };
                    parameters[0].Value = item.Name.Trim();
                    parameters[1].Value = item.ParentId ?? Convert.DBNull;
                    parameters[2].Value = item.MappingId ?? Convert.DBNull;
                    parameters[3].Value = item.Status;
                    using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                    {
                        conn.Open();
                        using (SqlTransaction trans = conn.BeginTransaction())
                        {
                            try
                            {
                                Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_LoaiKetQua_Insert", parameters);
                                trans.Commit();
                                Result.Message = ConstantLogMessage.Alert_Insert_Success("danh mục loại kết quả");
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
        public BaseResultModel InsertMulti(List<DanhMucLoaiKetQuaModel> items)
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
                                    };
                                    parameters[0].Value = item.Name.Trim();
                                    parameters[1].Value = item.ParentId ?? Convert.DBNull;
                                    parameters[2].Value = item.MappingId ?? Convert.DBNull;
                                    parameters[3].Value = item.Status;
                                    Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_LoaiKetQua_Insert", parameters);
                                }
                                trans.Commit();
                                Result.Message = ConstantLogMessage.Alert_Insert_Success("Danh mục loại kết quả");
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

        public BaseResultModel Update(DanhMucLoaiKetQuaModel item)
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
                else if (item.Id == null || item.Id < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng chọn dữ liệu trước khi thao tác";
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
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                                new SqlParameter("Id", SqlDbType.Int),
                                new SqlParameter("Name", SqlDbType.NVarChar),
                                new SqlParameter("ParentId", SqlDbType.Int),
                                new SqlParameter("MappingId", SqlDbType.Int),
                                new SqlParameter("Status", SqlDbType.Bit),
                        };
                        parameters[0].Value = item.Id;
                        parameters[1].Value = item.Name.Trim();
                        parameters[2].Value = item.ParentId ?? Convert.DBNull;
                        parameters[3].Value = item.MappingId ?? Convert.DBNull;
                        parameters[4].Value = item.Status;
                        using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                        {
                            conn.Open();
                            using (SqlTransaction trans = conn.BeginTransaction())
                            {
                                try
                                {
                                    Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_LoaiKetQua_Update", parameters);
                                    trans.Commit();
                                    Result.Message = ConstantLogMessage.Alert_Update_Success("danh mục loại kết quả");
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
        /// xóa loại kết quả
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
            if (checkUse(crObj.Id))
            {
                Result.Message = "Dữ liệu đã được sử dụng, không thể xóa!";
                Result.Status = 0;
                return Result;
            }
            var lstChildren = GetByParentId(id.Value);
            if (lstChildren != null && lstChildren.Count > 0)
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
                        val = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_DanhMuc_LoaiKetQua_Delete", parameters);
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
            Result.Message = ConstantLogMessage.Alert_Delete_Success("Danh mục loại kết quả");
            return Result;
        }

        /// <summary>
        /// lấy thông tin loại kết quả theo id
        /// </summary>
        /// <param name="LoaiKetQuaID"></param>
        /// <returns></returns>
        public DanhMucLoaiKetQuaModel GetByID(int LoaiKetQuaID)
        {
            if (LoaiKetQuaID <= 0 || LoaiKetQuaID == null)
            {
                return new DanhMucLoaiKetQuaModel();
            }
            DanhMucLoaiKetQuaModel LoaiKetQua = new DanhMucLoaiKetQuaModel();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("Id",SqlDbType.Int)
            };
            parameters[0].Value = LoaiKetQuaID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_LoaiKetQua_GetByID", parameters))
                {
                    while (dr.Read())
                    {
                        LoaiKetQua.Id = Utils.ConvertToInt32(dr["Id"], 0);
                        LoaiKetQua.Name = Utils.ConvertToString(dr["Name"], string.Empty);
                        LoaiKetQua.ParentId = Utils.ConvertToInt32(dr["ParentId"], 0);
                        LoaiKetQua.MappingId = Utils.ConvertToInt32(dr["MappingId"], 0);
                        LoaiKetQua.Status = Utils.ConvertToBoolean(dr["Status"], false);
                        break;
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return LoaiKetQua;
        }


        /// <summary>
        /// ds con trong parenid
        /// </summary>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        public List<DanhMucLoaiKetQuaModel> GetByParentId(int ParentId)
        {
            List<DanhMucLoaiKetQuaModel> result = new List<DanhMucLoaiKetQuaModel>();

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("parentId",SqlDbType.Int)
            };
            parameters[0].Value = ParentId;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_LoaiKetQua_GetByParentID", parameters))
                {
                    while (dr.Read())
                    {
                        DanhMucLoaiKetQuaModel item = new DanhMucLoaiKetQuaModel();
                        item.Id = Utils.ConvertToInt32(dr["Id"], 0);
                        item.Name = Utils.ConvertToString(dr["Name"], string.Empty);
                        item.ParentId = Utils.ConvertToInt32(dr["ParentId"], 0);
                        item.MappingId = Utils.ConvertToInt32(dr["MappingId"], 0);
                        item.Status = Utils.ConvertToBoolean(dr["Status"], false);
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
        /// tìm kiếm theo tên trong cây của parenid
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        public List<DanhMucLoaiKetQuaModel> GetByNameAndParentId(string name, int? ParentId)
        {
            List<DanhMucLoaiKetQuaModel> result = new List<DanhMucLoaiKetQuaModel>();
            if (name == null || name == "" || name.Length < 1)
            {
                return new List<DanhMucLoaiKetQuaModel>();
            }
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("name",SqlDbType.NVarChar),
                new SqlParameter("parentId",SqlDbType.Int)
            };
            parameters[0].Value = name;
            parameters[1].Value = ParentId ?? 0;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_LoaiKetQua_GetByNameAndParentId", parameters))
                {
                    while (dr.Read())
                    {
                        DanhMucLoaiKetQuaModel item = new DanhMucLoaiKetQuaModel();
                        item.Id = Utils.ConvertToInt32(dr["Id"], 0);
                        item.Name = Utils.ConvertToString(dr["Name"], string.Empty);
                        item.ParentId = Utils.ConvertToInt32(dr["ParentId"], 0);
                        item.MappingId = Utils.ConvertToInt32(dr["MappingId"], 0);
                        item.Status = Utils.ConvertToBoolean(dr["Status"], false);
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
        /// danh sách tất cả loại kết quả, phẳng
        /// </summary>
        /// <returns></returns>
        public List<DanhMucLoaiKetQuaModel> GetAll(string keyword)
        {
            List<DanhMucLoaiKetQuaModel> list = new List<DanhMucLoaiKetQuaModel>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_LoaiKetQua_GetAll"))
                {
                    while (dr.Read())
                    {
                        DanhMucLoaiKetQuaModel item = new DanhMucLoaiKetQuaModel();
                        item.Id = Utils.ConvertToInt32(dr["Id"], 0);
                        item.Name = Utils.ConvertToString(dr["Name"], string.Empty);
                        item.ParentId = Utils.ConvertToInt32(dr["ParentId"], 0);
                        item.MappingId = Utils.ConvertToInt32(dr["MappingId"], 0);
                        item.Status = Utils.ConvertToBoolean(dr["Status"], false);
                        if (keyword != null && item.Name.ToLower().Contains(keyword.ToLower()))
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
        /// danh sách tất cả loại kết quả,
        ///  gom hình cây (cha - con)
        /// </summary>
        /// <returns></returns>
        public List<DanhMucLoaiKetQuaModel> GetAllAndGroup(string keyword, bool? status)
        {
            var result = new List<DanhMucLoaiKetQuaModel>();
            try
            {
                result = GetAll(keyword);
                if (status != null)
                {
                    result = result.Where(x => x.Status == status.Value).ToList();
                }
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
        bool checkUse(int LoaiKetQuaID)
        {
            int loaiketquaid = 0;
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter(@"Id", SqlDbType.Int)
            };
            parameters[0].Value = LoaiKetQuaID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, "v1_DanhMuc_LoaiKetQua_CheckSuDung", parameters))
                {
                    while (dr.Read())
                    {
                        loaiketquaid = Utils.ConvertToInt32(dr["LoaiKetQuaID"], 0);
                        break;
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return loaiketquaid > 0;
        }

    }
}
