using Com.Gosol.QLKH.DAL.QuanTriHeThong;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.DanhMuc;
using Com.Gosol.QLKH.Models.QLKH;
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
    public interface IDanhMucCapDeTaiDAL
    {
        public BaseResultModel Insert(DanhMucCapDeTaiModel item);
        public BaseResultModel InsertMulti(List<DanhMucCapDeTaiModel> items);
        public BaseResultModel Update(DanhMucCapDeTaiModel item);
        public BaseResultModel Delete(int? id);
        public DanhMucCapDeTaiModel GetByID(int id);
        public List<DanhMucCapDeTaiModel> GetAll(string keyword);
        public List<DanhMucCapDeTaiModel> GetAllAndGroup(string keyword, bool? status);
    }
    public class DanhMucCapDeTaiDAL : IDanhMucCapDeTaiDAL
    {
        public BaseResultModel Insert(DanhMucCapDeTaiModel item)
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
                                new SqlParameter("Type", SqlDbType.Int),
                       };
                    parameters[0].Value = item.Name.Trim();
                    parameters[1].Value = item.ParentId ?? Convert.DBNull;
                    parameters[2].Value = item.MappingId ?? Convert.DBNull;
                    parameters[3].Value = item.Status;
                    parameters[4].Value = item.Type ?? Convert.DBNull;

                    using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                    {
                        conn.Open();
                        using (SqlTransaction trans = conn.BeginTransaction())
                        {
                            try
                            {
                                Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_CapDeTai_Insert", parameters);
                                trans.Commit();
                                Result.Message = ConstantLogMessage.Alert_Insert_Success("danh mục cấp đề tài");
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
        public BaseResultModel InsertMulti(List<DanhMucCapDeTaiModel> items)
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
                                        new SqlParameter("Type", SqlDbType.Int),
                                    };
                                    parameters[0].Value = item.Name.Trim();
                                    parameters[1].Value = item.ParentId ?? Convert.DBNull;
                                    parameters[2].Value = item.MappingId ?? Convert.DBNull;
                                    parameters[3].Value = item.Status;
                                    parameters[4].Value = item.Type ?? Convert.DBNull;
                                    Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_CapDeTai_Insert", parameters);
                                }
                                trans.Commit();
                                Result.Message = ConstantLogMessage.Alert_Insert_Success("Danh mục cấp đề tài");
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

        public BaseResultModel Update(DanhMucCapDeTaiModel item)
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
                                new SqlParameter("Type", SqlDbType.Int),
                        };
                        parameters[0].Value = item.Id;
                        parameters[1].Value = item.Name.Trim();
                        parameters[2].Value = item.ParentId ?? Convert.DBNull;
                        parameters[3].Value = item.MappingId ?? Convert.DBNull;
                        parameters[4].Value = item.Status;
                        parameters[5].Value = item.Type ?? Convert.DBNull;
                        using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                        {
                            conn.Open();
                            using (SqlTransaction trans = conn.BeginTransaction())
                            {
                                try
                                {
                                    Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_CapDeTai_Update", parameters);
                                    trans.Commit();
                                    Result.Message = ConstantLogMessage.Alert_Update_Success("danh mục cấp đề tài");
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
        /// xóa cấp đề tài
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
                        val = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_DanhMuc_CapDeTai_Delete", parameters);
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
            Result.Message = ConstantLogMessage.Alert_Delete_Success("Danh mục Cấp đề tài");
            return Result;
        }

        /// <summary>
        /// lấy thông tin cấp đề tài theo id
        /// </summary>
        /// <param name="CapDeTaiID"></param>
        /// <returns></returns>
        public DanhMucCapDeTaiModel GetByID(int CapDeTaiID)
        {
            if (CapDeTaiID <= 0 || CapDeTaiID == null)
            {
                return new DanhMucCapDeTaiModel();
            }
            DanhMucCapDeTaiModel CapDeTai = new DanhMucCapDeTaiModel();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("Id",SqlDbType.Int)
            };
            parameters[0].Value = CapDeTaiID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_CapDeTai_GetByID", parameters))
                {
                    while (dr.Read())
                    {
                        CapDeTai.Id = Utils.ConvertToInt32(dr["Id"], 0);
                        CapDeTai.Name = Utils.ConvertToString(dr["Name"], string.Empty);
                        CapDeTai.ParentId = Utils.ConvertToInt32(dr["ParentId"], 0);
                        CapDeTai.MappingId = Utils.ConvertToInt32(dr["MappingId"], 0);
                        CapDeTai.Status = Utils.ConvertToBoolean(dr["Status"], false);
                        CapDeTai.Type = Utils.ConvertToInt32(dr["Type"], 0);
                        break;
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return CapDeTai;
        }


        /// <summary>
        /// ds con trong parenid
        /// </summary>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        public List<DanhMucCapDeTaiModel> GetByParentId(int ParentId)
        {
            List<DanhMucCapDeTaiModel> result = new List<DanhMucCapDeTaiModel>();

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("parentId",SqlDbType.Int)
            };
            parameters[0].Value = ParentId;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_CapDeTai_GetByParentID", parameters))
                {
                    while (dr.Read())
                    {
                        DanhMucCapDeTaiModel item = new DanhMucCapDeTaiModel();
                        item.Id = Utils.ConvertToInt32(dr["Id"], 0);
                        item.Name = Utils.ConvertToString(dr["Name"], string.Empty);
                        item.ParentId = Utils.ConvertToInt32(dr["ParentId"], 0);
                        item.MappingId = Utils.ConvertToInt32(dr["MappingId"], 0);
                        item.Status = Utils.ConvertToBoolean(dr["Status"], false);
                        item.Type = Utils.ConvertToInt32(dr["Type"], 0);
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
        public List<DanhMucCapDeTaiModel> GetByNameAndParentId(string name, int? ParentId)
        {
            List<DanhMucCapDeTaiModel> result = new List<DanhMucCapDeTaiModel>();
            if (name == null || name == "" || name.Length < 1)
            {
                return new List<DanhMucCapDeTaiModel>();
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
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_CapDeTai_GetByNameAndParentId", parameters))
                {
                    while (dr.Read())
                    {
                        DanhMucCapDeTaiModel item = new DanhMucCapDeTaiModel();
                        item.Id = Utils.ConvertToInt32(dr["Id"], 0);
                        item.Name = Utils.ConvertToString(dr["Name"], string.Empty);
                        item.ParentId = Utils.ConvertToInt32(dr["ParentId"], 0);
                        item.MappingId = Utils.ConvertToInt32(dr["MappingId"], 0);
                        item.Status = Utils.ConvertToBoolean(dr["Status"], false);
                        item.Type = Utils.ConvertToInt32(dr["Type"], 0);
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
        /// danh sách tất cả cấp đề tài, phẳng
        /// </summary>
        /// <returns></returns>
        public List<DanhMucCapDeTaiModel> GetAll(string keyword)
        {
            List<DanhMucCapDeTaiModel> list = new List<DanhMucCapDeTaiModel>();
            // SqlParameter[] parameters = new SqlParameter[]
            //{
            //     new SqlParameter("keyword",SqlDbType.NVarChar)
            //};
            // parameters[0].Value = keyword ?? "";
            try
            {
                var vaiTroChuNhiemIdsKey = new SystemConfigDAL().GetByKey("VaiTroChuNhiemIds").ConfigValue;
                var vaiTroChuNhiemIds = new List<CategorieModel>();
                if (vaiTroChuNhiemIdsKey != null && vaiTroChuNhiemIdsKey.Length > 0)
                {
                    var query = vaiTroChuNhiemIdsKey.Split(';');
                    foreach (var item in query)
                    {
                        var qr1 = item.Trim().Split('-');
                        var it = new CategorieModel();
                        it.ParentId = Utils.ConvertToInt32(qr1[0], 0);
                        it.Id = Utils.ConvertToInt32(qr1[1], 0);
                        vaiTroChuNhiemIds.Add(it);
                    }
                }
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_CapDeTai_GetAll"))
                {
                    while (dr.Read())
                    {
                        DanhMucCapDeTaiModel item = new DanhMucCapDeTaiModel();
                        item.Id = Utils.ConvertToInt32(dr["Id"], 0);
                        item.Name = Utils.ConvertToString(dr["Name"], string.Empty);
                        item.ParentId = Utils.ConvertToInt32(dr["ParentId"], 0);
                        item.MappingId = Utils.ConvertToInt32(dr["MappingId"], 0);
                        item.Status = Utils.ConvertToBoolean(dr["Status"], false);
                        item.Type = Utils.ConvertToInt32(dr["Type"], 0);
                        if (keyword != null && item.Name.ToLower().Contains(keyword.ToLower()))
                            item.Highlight = 1;
                        if (item.MappingId > 0 && vaiTroChuNhiemIds != null && vaiTroChuNhiemIds.Count > 0)
                        {
                            var vtnv = vaiTroChuNhiemIds.FirstOrDefault(x => x.ParentId == item.MappingId);
                            if (vtnv != null && vtnv.Id > 0)
                                item.VaiTroChuNhiemID = vaiTroChuNhiemIds.FirstOrDefault(x => x.ParentId == item.MappingId).Id;
                        }
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
        /// danh sách tất cả cấp đề tài,
        ///  gom hình cây (cha - con)
        /// </summary>
        /// <returns></returns>
        public List<DanhMucCapDeTaiModel> GetAllAndGroup(string keyword, bool? status)
        {
            var result = new List<DanhMucCapDeTaiModel>();
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


    }
}
