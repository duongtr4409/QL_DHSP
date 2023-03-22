using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.QLKH;
using Com.Gosol.QLKH.Ultilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Com.Gosol.QLKH.DAL.QLKH
{
    public interface IFileDinhKemDAL
    {
        public BaseResultModel Insert_FileDinhKem(FileDinhKemModel FileDinhKemModel);
        public List<FileDinhKemModel> GetBy_ThongTinRaVaoID(int ThongTinRaVaoID);
        public FileDinhKemModel GetByID(int FileDinhKemID);
        public BaseResultModel Delete_FileDinhKem(int LoaiFile, int NghiepVuID);
        public BaseResultModel Delete_FileDinhKemID(int FileDinhKemID);
        public List<FileDinhKemModel> GetByLoaiFileAndNghiepVuIDAndCoQuanID(int LoaiFile, int NghiepVuID, int CoQuanID);
    }
    public class FileDinhKemDAL : IFileDinhKemDAL
    {
        #region Get
        public List<FileDinhKemModel> GetBy_ThongTinRaVaoID(int ThongTinRaVaoID)
        {
            List<FileDinhKemModel> Result = new List<FileDinhKemModel>();
            //if (fileDinhKemModel == null || fileDinhKemModel.Base64File == null || fileDinhKemModel.Base64File.Length < 1)
            //{
            //    Result.Status = 0;
            //    Result.Message = "Vui lòng chọn file ảnh chân dung";
            //    return Result;
            //}
            SqlParameter[] parameters = new SqlParameter[]
            {
                  new SqlParameter("@ThongTinRaVaoID",SqlDbType.Int)
            };
            parameters[0].Value = ThongTinRaVaoID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_NV_FileDinhKem_GetBy_ThongTinRaVaoID", parameters))
                {
                    while (dr.Read())
                    {
                        var crFile = new FileDinhKemModel();
                        crFile.ThongTinVaoRaID = Utils.ConvertToInt32(dr["ThongTinVaoRaID"], 0);
                        crFile.FileDinhKemID = Utils.ConvertToInt32(dr["FileDinhKemID"], 0);
                        crFile.TenFileHeThong = Utils.ConvertToString(dr["TenFileHeThong"], string.Empty);
                        crFile.LoaiFile = Utils.ConvertToInt32(dr["LoaiFile"], 0);
                        Result.Add(crFile);
                    }
                    dr.Close();
                }
                return Result;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Insert
        public BaseResultModel Insert(FileDinhKemModel fileDinhKemModel)
        {
            var Result = new BaseResultModel();
            if (fileDinhKemModel == null || fileDinhKemModel.Base64File == null || fileDinhKemModel.Base64File.Length < 1)
            {
                Result.Status = 0;
                Result.Message = "Vui lòng chọn file ảnh chân dung";
                return Result;
            }
            SqlParameter[] parameters = new SqlParameter[]
                {
                            new SqlParameter("@TenFileGoc",SqlDbType.NVarChar),
                            new SqlParameter("@TenFileHeThong",SqlDbType.NVarChar),
                            new SqlParameter("@LoaiFile",SqlDbType.Int),
                            new SqlParameter("@FileUrl",SqlDbType.NVarChar),
                            new SqlParameter("@NgayTao",SqlDbType.DateTime),
                            new SqlParameter("@CoBaoMat",SqlDbType.Bit),
                            new SqlParameter("@NguoiTaoID",SqlDbType.Int),
                            new SqlParameter("@ThongTinVaoRaID",SqlDbType.Int)

                };
            parameters[0].Value = fileDinhKemModel.TenFileGoc ?? Convert.DBNull;
            parameters[1].Value = fileDinhKemModel.TenFileHeThong ?? Convert.DBNull;
            parameters[2].Value = fileDinhKemModel.LoaiFile;
            parameters[3].Value = fileDinhKemModel.FileUrl ?? Convert.DBNull;
            parameters[4].Value = fileDinhKemModel.NgayTao ?? Convert.DBNull;
            parameters[5].Value = fileDinhKemModel.CoBaoMat ?? Convert.DBNull;
            parameters[6].Value = fileDinhKemModel.NguoiTaoID ?? Convert.DBNull;
            parameters[7].Value = fileDinhKemModel.ThongTinVaoRaID ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_NV_FileDinhKemAnh_Insert", parameters);
                        Result.Message = ConstantLogMessage.Alert_Insert_Success("Thêm file đính kèm");
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        Result.Message = ex.Message;
                        trans.Rollback();
                        throw;
                    }
                }
            }
            return Result;
        }
        #endregion

        #region Update
        #endregion

        #region Delete
        //public List<string> Delete(int FileDinhKemID)
        //{

        //    List<string> dic = new List<string>();

        //    SqlParameter[] parameters = new SqlParameter[]
        //                  {new SqlParameter("@FileDinhKemID", SqlDbType.Int)};
        //    parameters[0].Value = FileDinhKemID;
        //    using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
        //    {
        //        conn.Open();
        //        using (SqlTransaction trans = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                Utils.ConvertToInt32(SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_NV_FileDinhKem_Delete", parameters), 0);
        //                trans.Commit();
        //            }
        //            catch (Exception ex)
        //            {
        //                dic.Add(ex.Message);
        //                trans.Rollback();
        //                throw;
        //            }
        //        }
        //    }
        //    return dic;
        //}
        #endregion

        public BaseResultModel Insert_FileDinhKem(FileDinhKemModel fileDinhKemModel)
        {
            var Result = new BaseResultModel();
            if (fileDinhKemModel == null)
            {
                Result.Status = 0;
                Result.Message = "Chưa chọn file đính kèm";
                return Result;
            }
            SqlParameter[] parameters = new SqlParameter[]
                {
                            new SqlParameter("@TenFileGoc",SqlDbType.NVarChar),
                            new SqlParameter("@TenFileHeThong",SqlDbType.NVarChar),
                            new SqlParameter("@LoaiFile",SqlDbType.Int),
                            new SqlParameter("@FileUrl",SqlDbType.NVarChar),
                            new SqlParameter("@NgayTao",SqlDbType.DateTime),
                            new SqlParameter("@NguoiTaoID",SqlDbType.Int),
                            new SqlParameter("@NghiepVuID",SqlDbType.Int),
                            new SqlParameter("@NoiDung",SqlDbType.NVarChar),
                            new SqlParameter("@CoQuanID",SqlDbType.Int),
                };
            parameters[0].Value = fileDinhKemModel.TenFileGoc ?? Convert.DBNull;
            parameters[1].Value = fileDinhKemModel.TenFileHeThong ?? Convert.DBNull;
            parameters[2].Value = fileDinhKemModel.LoaiFile;
            parameters[3].Value = fileDinhKemModel.FileUrl ?? Convert.DBNull;
            parameters[4].Value = fileDinhKemModel.NgayTao ?? Convert.DBNull;
            parameters[5].Value = fileDinhKemModel.NguoiTaoID ?? Convert.DBNull;
            parameters[6].Value = fileDinhKemModel.NghiepVuID;
            parameters[7].Value = fileDinhKemModel.NoiDung ?? Convert.DBNull;
            parameters[8].Value = fileDinhKemModel.CoQuanID ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        Result.Status = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, @"v1_NV_FileDinhKem_Insert", parameters);
                        Result.Message = ConstantLogMessage.Alert_Insert_Success("Thêm file đính kèm");
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        Result.Message = ex.Message;
                        trans.Rollback();
                        throw;
                    }
                }
            }
            return Result;
        }

        public BaseResultModel Delete_FileDinhKem(int LoaiFile, int NghiepVuID)
        {
            var Result = new BaseResultModel();
            List<FileDinhKemModel> ListFile = GetByLoaiFileAndNghiepVuID(LoaiFile, NghiepVuID);
            if (ListFile.Count > 0)
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                            new SqlParameter("@LoaiFile",SqlDbType.NVarChar),
                            new SqlParameter("@NghiepVuID",SqlDbType.NVarChar)
                };
                parameters[0].Value = LoaiFile;
                parameters[1].Value = NghiepVuID;

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, @"v1_NV_FileDinhKem_Delete", parameters);
                            Result.Message = ConstantLogMessage.Alert_Delete_Success("Xóa file đính kèm");
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
                //Xóa file trong thư mục sau khi xóa trên db thành công
                if (Result.Status > 0)
                {
                    foreach (FileDinhKemModel fileInfo in ListFile)
                    {
                        DeleteFileByPath(fileInfo.FileUrl);
                    }
                }
            }
            else
            {
                Result.Status = 1;
            }
            return Result;
        }

        public BaseResultModel Delete_FileDinhKemID(int FileDinhKemID)
        {
            var Result = new BaseResultModel();
            var file = GetByID(FileDinhKemID);
            if(file.LoaiFile == EnumLoaiFileDinhKem.DeXuatDeTai.GetHashCode())
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@FileDinhKemID",SqlDbType.Int)
                };
                parameters[0].Value = FileDinhKemID;

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, @"v1_NV_FileDinhKem_UpdateDeleteFile", parameters);
                            Result.Message = ConstantLogMessage.Alert_Delete_Success("Xóa file đính kèm");
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
            else if(file.FileDinhKemID > 0)
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@FileDinhKemID",SqlDbType.Int)
                };
                parameters[0].Value = FileDinhKemID;

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, @"v1_NV_FileDinhKem_DeleteByFileDinhKemID", parameters);
                            Result.Message = ConstantLogMessage.Alert_Delete_Success("Xóa file đính kèm");
                            Result.Status = 1;
                            trans.Commit();
                            DeleteFileByPath(file.FileUrl);
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
                Result.Status = 1;
            }


            return Result;
        }

        public FileDinhKemModel GetByID(int FileDinhKemID)
        {
            FileDinhKemModel Result = new FileDinhKemModel();

            SqlParameter[] parameters = new SqlParameter[]
            {
                  new SqlParameter("@FileDinhKemID",SqlDbType.Int)
            };
            parameters[0].Value = FileDinhKemID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_FileDinhKem_GetByID", parameters))
                {
                    while (dr.Read())
                    {
                        Result.NghiepVuID = Utils.ConvertToInt32(dr["NghiepVuID"], 0);
                        Result.FileDinhKemID = Utils.ConvertToInt32(dr["FileDinhKemID"], 0);
                        Result.TenFileGoc = Utils.ConvertToString(dr["TenFileGoc"], string.Empty);
                        Result.TenFileHeThong = Utils.ConvertToString(dr["TenFileHeThong"], string.Empty);
                        Result.LoaiFile = Utils.ConvertToInt32(dr["LoaiFile"], 0);
                        Result.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                    }
                    dr.Close();
                }

                return Result;
            }
            catch
            {
                throw;
            }
        }

        public List<FileDinhKemModel> GetByLoaiFileAndNghiepVuID(int LoaiFile, int NghiepVuID)
        {
            List<FileDinhKemModel> ListFileDinhKem = new List<FileDinhKemModel>();
            SqlParameter[] parameters = new SqlParameter[]
                {
                            new SqlParameter("@LoaiFile",SqlDbType.NVarChar),
                            new SqlParameter("@NghiepVuID",SqlDbType.NVarChar)
                };
            parameters[0].Value = LoaiFile;
            parameters[1].Value = NghiepVuID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_NV_FileDinhKem_GetBy_LoaiFileAndNghiepVuID", parameters))
                {
                    while (dr.Read())
                    {
                        FileDinhKemModel fileInfo = new FileDinhKemModel();
                        fileInfo.FileDinhKemID = Utils.ConvertToInt32(dr["FileDinhKemID"], 0);
                        fileInfo.TenFileGoc = Utils.ConvertToString(dr["TenFileGoc"], string.Empty);
                        fileInfo.TenFileHeThong = Utils.ConvertToString(dr["TenFileHeThong"], string.Empty);
                        fileInfo.LoaiFile = Utils.ConvertToInt32(dr["LoaiFile"], 0);
                        fileInfo.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        fileInfo.NguoiTaoID = Utils.ConvertToInt32(dr["NguoiTaoID"], 0);
                        fileInfo.TenNguoiTao = Utils.ConvertToString(dr["TenNguoiTao"], string.Empty);
                        fileInfo.NgayTao = Utils.ConvertToDateTime(dr["NgayTao"], DateTime.Now);
                        fileInfo.NoiDung = Utils.ConvertToString(dr["NoiDung"], string.Empty);
                        ListFileDinhKem.Add(fileInfo);
                    }
                    dr.Close();
                }
                return ListFileDinhKem;
            }
            catch
            {
                throw;
            }
        }

        public List<FileDinhKemModel> GetByLoaiFileAndNghiepVuIDAndCoQuanID(int LoaiFile, int NghiepVuID, int CoQuanID)
        {
            List<FileDinhKemModel> ListFileDinhKem = new List<FileDinhKemModel>();
            SqlParameter[] parameters = new SqlParameter[]
                {
                            new SqlParameter("@LoaiFile",SqlDbType.NVarChar),
                            new SqlParameter("@NghiepVuID",SqlDbType.NVarChar),
                            new SqlParameter("@CoQuanID",SqlDbType.NVarChar),
                };
            parameters[0].Value = LoaiFile;
            parameters[1].Value = NghiepVuID;
            parameters[2].Value = CoQuanID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_NV_FileDinhKem_GetBy_LoaiFile_NghiepVuID_CoQuanID", parameters))
                {
                    while (dr.Read())
                    {
                        FileDinhKemModel fileInfo = new FileDinhKemModel();
                        fileInfo.FileDinhKemID = Utils.ConvertToInt32(dr["FileDinhKemID"], 0);
                        fileInfo.TenFileGoc = Utils.ConvertToString(dr["TenFileGoc"], string.Empty);
                        fileInfo.TenFileHeThong = Utils.ConvertToString(dr["TenFileHeThong"], string.Empty);
                        fileInfo.LoaiFile = Utils.ConvertToInt32(dr["LoaiFile"], 0);
                        fileInfo.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        fileInfo.NguoiTaoID = Utils.ConvertToInt32(dr["NguoiTaoID"], 0);
                        fileInfo.TenNguoiTao = Utils.ConvertToString(dr["TenNguoiTao"], string.Empty);
                        fileInfo.NgayTao = Utils.ConvertToDateTime(dr["NgayTao"], DateTime.Now);
                        fileInfo.NoiDung = Utils.ConvertToString(dr["NoiDung"], string.Empty);
                        ListFileDinhKem.Add(fileInfo);
                    }
                    dr.Close();
                }
                return ListFileDinhKem;
            }
            catch
            {
                throw;
            }
        }

        public bool DeleteFileByPath(string Path)
        {
            try
            {
                if (File.Exists(Path))
                {
                    File.Delete(Path);
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }

        public List<FileDinhKemModel> GetFileDinhKemByListNghiepVu(List<int> listNghiepVuID, int LoaiFile)
        {
            List<FileDinhKemModel> Result = new List<FileDinhKemModel>();
            var pList = new SqlParameter("@ListID", SqlDbType.Structured);
            pList.TypeName = "dbo.list_ID";
            var tbNghiepVuID = new DataTable();
            tbNghiepVuID.Columns.Add("ID", typeof(string));
            listNghiepVuID.ForEach(x => tbNghiepVuID.Rows.Add(x));

            SqlParameter[] parameters = new SqlParameter[]
            {
                pList,
                new SqlParameter("@LoaiFile",SqlDbType.Int),
            };
            parameters[0].Value = tbNghiepVuID;
            parameters[1].Value = LoaiFile;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_FileDinhKem_GetByListNghiepVuID", parameters))
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
    }
}
