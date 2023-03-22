using Com.Gosol.QLKH.DAL.QuanTriHeThong;
using Com.Gosol.QLKH.DAL.QLKH;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.QLKH;
using Com.Gosol.QLKH.Ultilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;

namespace Com.Gosol.QLKH.DAL.QLKH
{
    public interface IThuyetMinhDeTaiDAL
    {
        public List<DeXuatThuyetMinhModel> GetPagingBySearch(BasePagingParams p, int CapQuanLy);
        public BaseResultModel InsertThuyetMinh(ThuyetMinhDeTaiModel ThuyetMinhDeTai);
        public BaseResultModel UpdateThuyetMinh(ThuyetMinhDeTaiModel ThuyetMinhDeTai);
        public ThuyetMinhDeTaiModel GetByID(int ThuyetMinhID);
        public List<ThuyetMinhDeTaiModel> GetAllThuyetMinhByDeXuatID(int DeXuatID);
        public BaseResultModel Delete_ThuyetMinh(int ThuyetMinhID);
        public BaseResultModel DuyetThuyetMinh(int ThuyetMinhID, int DeXuatID);
    }
    public class ThuyetMinhDeTaiDAL : IThuyetMinhDeTaiDAL
    {
        public List<DeXuatThuyetMinhModel> GetPagingBySearch(BasePagingParams p, int CapQuanLy)
        {
            List<DeXuatThuyetMinhModel> Result = new List<DeXuatThuyetMinhModel>();
            List<ThuyetMinhDeTaiModel> ThuyetMinhDeTai = new List<ThuyetMinhDeTaiModel>();
            SqlParameter[] parameters = new SqlParameter[]
                      {
                        new SqlParameter("@Keyword",SqlDbType.NVarChar),
                        new SqlParameter("@CapQuanLy",SqlDbType.NVarChar)
                      };
            parameters[0].Value = p.Keyword == null ? "" : p.Keyword.Trim();
            parameters[1].Value = CapQuanLy;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_ThuyetMinh_GetDanhSachDeXuatThuyetMinh", parameters))
                {
                    while (dr.Read())
                    {
                        ThuyetMinhDeTaiModel thuyetminh = new ThuyetMinhDeTaiModel();
                        thuyetminh.DeXuatID = Utils.ConvertToInt32(dr["DeXuatID"], 0);
                        thuyetminh.MaDeXuat = Utils.ConvertToString(dr["MaDeXuat"], string.Empty);
                        thuyetminh.TenDeXuat = Utils.ConvertToString(dr["TenDeXuat"], string.Empty);
                        thuyetminh.ThuyetMinhID = Utils.ConvertToInt32(dr["ThuyetMinhID"], 0);
                        thuyetminh.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        thuyetminh.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        thuyetminh.TrangThai = Utils.ConvertToInt32(dr["TrangThai"], 0);
                        thuyetminh.FileThuyetMinh = new FileDinhKemModel();
                        thuyetminh.FileThuyetMinh.FileDinhKemID = Utils.ConvertToInt32(dr["FileDinhKemID"], 0);
                        thuyetminh.FileThuyetMinh.TenFileGoc = Utils.ConvertToString(dr["TenFileGoc"], string.Empty);
                        thuyetminh.FileThuyetMinh.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        ThuyetMinhDeTai.Add(thuyetminh);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            var listDeXuat = ThuyetMinhDeTai.Where(x => x.ThuyetMinhID == 0).ToList();
            foreach (var item in listDeXuat)
            {
                DeXuatThuyetMinhModel dexuat = new DeXuatThuyetMinhModel();
                dexuat.DeXuatID = item.DeXuatID;
                dexuat.TenDeXuat = item.TenDeXuat;
                dexuat.MaDeXuat = item.MaDeXuat;
                var listThuyetMinh = ThuyetMinhDeTai.Where(x => x.ThuyetMinhID > 0 && x.DeXuatID == item.DeXuatID).ToList();
                dexuat.ListThuyetMinh = listThuyetMinh;
                if (dexuat.ListThuyetMinh.Count > 0)
                {
                    int ThuyetMinhID = dexuat.ListThuyetMinh[0].ThuyetMinhID;
                    dexuat.DuyetDeXuat = CheckDuyetDeXuatByThuyetMinhID(ThuyetMinhID);
                }
                else
                {
                    dexuat.DuyetDeXuat = false;
                }
                Result.Add(dexuat);
            }

            return Result;
        }

        public BaseResultModel InsertThuyetMinh(ThuyetMinhDeTaiModel ThuyetMinhDeTai)
        {
            var Result = new BaseResultModel();
            if (ThuyetMinhDeTai.DeXuatID > 0)
            {

                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("DeXuatID",SqlDbType.Int),
                new SqlParameter("CoQuanID",SqlDbType.Int),
                new SqlParameter("CanBoID",SqlDbType.Int)
                };

                parameters[0].Value = ThuyetMinhDeTai.DeXuatID;
                parameters[1].Value = ThuyetMinhDeTai.CoQuanID;
                parameters[2].Value = ThuyetMinhDeTai.CanBoID;

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Result.Data = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "v1_ThuyetMinh_Insert", parameters);
                            trans.Commit();
                            Result.Status = 1;
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
            else
            {
                Result.Status = 0;
                Result.Message = "Không có đề xuất đề tài thuyết minh";
            }
            return Result;
        }

        public BaseResultModel UpdateThuyetMinh(ThuyetMinhDeTaiModel ThuyetMinhDeTai)
        {
            var Result = new BaseResultModel();
            ThuyetMinhDeTaiModel thuyetminh = GetByID(ThuyetMinhDeTai.ThuyetMinhID);
            bool checkDuyet = CheckDuyetDeXuatByThuyetMinhID(ThuyetMinhDeTai.ThuyetMinhID);
            if (checkDuyet)
            {
                Result.Status = 0;
                Result.Message = "Đề xuất đã được duyệt, không thể sửa";
                return Result;
            }
            if (thuyetminh.ThuyetMinhID < 0)
            {
                Result.Status = 0;
                Result.Message = "Không có thuyết minh đề tài";
                return Result;
            }
            else if (thuyetminh.DeXuatID != ThuyetMinhDeTai.DeXuatID)
            {
                Result.Status = 0;
                Result.Message = "Không có thuyết minh đề tài";
                return Result;
            }
            else
            {

                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("ThuyetMinhID", SqlDbType.Int),
                new SqlParameter("CoQuanID", SqlDbType.Int),
                new SqlParameter("CanBoID", SqlDbType.Int)
                };

                parameters[0].Value = ThuyetMinhDeTai.ThuyetMinhID;
                parameters[1].Value = ThuyetMinhDeTai.CoQuanID;
                parameters[2].Value = ThuyetMinhDeTai.CanBoID;

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Result.Data = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v1_ThuyetMinh_Update", parameters);
                            trans.Commit();
                            Result.Status = 1;
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
            return Result;
        }

        public ThuyetMinhDeTaiModel GetByID(int ThuyetMinhID)
        {
            var ThuyetMinh = new ThuyetMinhDeTaiModel();

            if (ThuyetMinhID > 0)
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("ThuyetMinhID",SqlDbType.Int)
                };

                parameters[0].Value = ThuyetMinhID;

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, @"v1_ThuyetMinh_GetByID", parameters))
                {
                    while (dr.Read())
                    {
                        ThuyetMinh.ThuyetMinhID = Utils.ConvertToInt32(dr["ThuyetMinhID"], 0);
                        ThuyetMinh.DeXuatID = Utils.ConvertToInt32(dr["DeXuatID"], 0);
                        ThuyetMinh.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        ThuyetMinh.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        ThuyetMinh.FileThuyetMinh = new FileDinhKemModel();
                        ThuyetMinh.FileThuyetMinh.TenFileGoc = Utils.ConvertToString(dr["TenFileGoc"], string.Empty);
                        ThuyetMinh.FileThuyetMinh.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        ThuyetMinh.FileThuyetMinh.FileDinhKemID = Utils.ConvertToInt32(dr["FileDinhKemID"], 0);
                    }
                }
            }
            return ThuyetMinh;
        }
        /// <summary>
        /// Kiểm tra đề xuất đã được duyệt hay chưa
        /// </summary>
        /// <param name="ThuyetMinhID"></param>
        /// <returns></returns>
        public bool CheckDuyetDeXuatByThuyetMinhID(int ThuyetMinhID)
        {
            List<ThuyetMinhDeTaiModel> ListThuyetMinh = GetAllThuyetMinhByThuyetMinhID(ThuyetMinhID);
            bool flag = false;
            foreach (var item in ListThuyetMinh)
            {
                if (item.TrangThai > 0)
                {
                    flag = true;
                }
            }
            return flag;
        }
        /// <summary>
        /// Lấy danh sách thuyết minh của một đề xuất theo ThuyetMinhID
        /// </summary>
        /// <param name="ThuyetMinhID"></param>
        /// <returns></returns>
        public List<ThuyetMinhDeTaiModel> GetAllThuyetMinhByThuyetMinhID(int ThuyetMinhID)
        {
            ThuyetMinhDeTaiModel ThuyetMinh = GetByID(ThuyetMinhID);
            int DeXuatID = ThuyetMinh.DeXuatID;
            List<ThuyetMinhDeTaiModel> ListThuyetMinh = GetAllThuyetMinhByDeXuatID(DeXuatID);
            return ListThuyetMinh;
        }
        /// <summary>
        /// Lấy danh sách thuyết minh của một đề xuất theo DeXuatID
        /// </summary>
        /// <param name="ThuyetMinhID"></param>
        /// <returns></returns>
        public List<ThuyetMinhDeTaiModel> GetAllThuyetMinhByDeXuatID(int DeXuatID)
        {
            List<ThuyetMinhDeTaiModel> ListThuyetMinh = new List<ThuyetMinhDeTaiModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter("DeXuatID",SqlDbType.Int)
            };
            parameters[0].Value = DeXuatID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, @"v1_ThuyetMinh_GetAllByDeXuatID", parameters))
            {
                while (dr.Read())
                {
                    ThuyetMinhDeTaiModel thuyetminh = new ThuyetMinhDeTaiModel();
                    thuyetminh.ThuyetMinhID = Utils.ConvertToInt32(dr["ThuyetMinhID"], 0);
                    thuyetminh.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                    thuyetminh.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                    thuyetminh.TrangThai = Utils.ConvertToInt32(dr["TrangThai"], 0);
                    thuyetminh.FileThuyetMinh = new FileDinhKemModel();
                    thuyetminh.FileThuyetMinh.TenFileGoc = Utils.ConvertToString(dr["TenFileGoc"], string.Empty);
                    thuyetminh.FileThuyetMinh.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                    thuyetminh.FileThuyetMinh.FileDinhKemID = Utils.ConvertToInt32(dr["FileDinhKemID"], 0);
                    ListThuyetMinh.Add(thuyetminh);
                }
            }
            return ListThuyetMinh;
        }
        public BaseResultModel Delete_ThuyetMinh(int ThuyetMinhID)
        {
            var Result = new BaseResultModel();
            bool DuyetDeXuat = CheckDuyetDeXuatByThuyetMinhID(ThuyetMinhID);
            if (DuyetDeXuat)
            {
                Result.Status = 0;
                Result.Message = "Đề xuất đã duyệt, không thể xóa";
                return Result;
            }
            SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@ThuyetMinhID",SqlDbType.Int)
                };
            parameters[0].Value = ThuyetMinhID;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, @"v1_ThuyetMinh_Delete", parameters);
                        Result.Message = ConstantLogMessage.Alert_Delete_Success("thuyết minh đề tài");
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
        public BaseResultModel DuyetThuyetMinh(int ThuyetMinhID, int DeXuatID)
        {
            var Result = new BaseResultModel();
            ThuyetMinhDeTaiModel ThuyetMinhModel = GetByID(ThuyetMinhID);
            bool DuyetDeXuat = CheckDuyetDeXuatByThuyetMinhID(ThuyetMinhID);
            if (DuyetDeXuat)
            {
                Result.Status = 0;
                Result.Message = "Đề xuất đã có thuyết minh được duyệt";
                return Result;
            }
            SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("ThuyetMinhID", SqlDbType.Int),
                new SqlParameter("DeXuatID", SqlDbType.Int),
                };

            parameters[0].Value = ThuyetMinhID;
            parameters[1].Value = DeXuatID;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        Result.Data = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v1_ThuyetMinh_DuyetThuyetMinh", parameters);

                        //Chuyển đề xuất thành đề tài
                        int resultDuyet = Utils.ConvertToInt32(Result.Data, 0);
                        if (resultDuyet > 0)
                        {
                            DeXuatDeTaiModel DeXuatModel = new DeXuatDeTaiDAL().GetByID(DeXuatID);
                            //Lấy thông tin đề xuất chuyển sang màn hình nhiệm vụ nghiên cứu
                            DeTaiModel dt = new DeTaiModel();
                            dt.MaDeTai = DeXuatModel.MaDeXuat;
                            dt.TenDeTai = DeXuatModel.TenDeXuat;
                            dt.LinhVucNghienCuu = DeXuatModel.LinhVucNghienCuu;
                            dt.LinhVucKinhTeXaHoi = DeXuatModel.LinhVucKinhTeXaHoi;
                            dt.CapQuanLy = DeXuatModel.CapQuanLy;
                            dt.NhiemVu = DeXuatModel.CapQuanLy;
                            dt.ChuNhiemDeTaiID = ThuyetMinhModel.CanBoID;
                            dt.CoQuanChuNhiemID = ThuyetMinhModel.CoQuanID;
                            dt.NamBatDau = DeXuatModel.ThoiGianThucHienTu;
                            dt.NamKetThuc = DeXuatModel.ThoiGianThucHienDen;
                            dt.KinhPhiDHSP = DeXuatModel.KinhPhiDuKien;
                            dt.NguoiTaoID = DeXuatModel.NguoiDeXuat;
                            dt.MucTieu = DeXuatModel.MucTieu;
                            dt.SanPhamDangKy = DeXuatModel.SanPham;
                            dt.CoQuanID = ThuyetMinhModel.CoQuanID;
                            var detai = new DeTaiDAL().Insert(dt);
                            int DeTaiID = Utils.ConvertToInt32(detai.Data, 0);
                            if (DeTaiID > 0 && ThuyetMinhModel.FileThuyetMinh != null && ThuyetMinhModel.FileThuyetMinh.FileDinhKemID > 0)
                            {
                                //FileDinhKemModel file = ThuyetMinhModel.FileThuyetMinh;
                                FileDinhKemModel file = ThuyetMinhModel.FileThuyetMinh;
                                string fileBaseUrl = file.FileUrl;
                                file.NghiepVuID = DeTaiID;
                                file.LoaiFile = EnumLoaiFileDinhKem.DeTai.GetHashCode();
                                file.FolderPath = nameof(EnumLoaiFileDinhKem.DeTai);
                                file.FileUrl = file.FileUrl.Replace(nameof(EnumLoaiFileDinhKem.ThuyetMinhDeTai), nameof(EnumLoaiFileDinhKem.DeTai));
                                new FileDinhKemDAL().Insert_FileDinhKem(file);
                                try
                                {
                                    File.Copy(fileBaseUrl, file.FileUrl, true);
                                }
                                catch (Exception)
                                {
                                    //throw;
                                }
                            }
                        }
                        trans.Commit();
                        Result.Status = 1;
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
            return Result;
        }
        public bool CheckDuyetDeXuatByDeXuatID(int DeXuatID)
        {
            List<ThuyetMinhDeTaiModel> ListThuyetMinh = GetAllThuyetMinhByDeXuatID(DeXuatID);
            bool flag = false;
            foreach (var item in ListThuyetMinh)
            {
                if (item.TrangThai > 0)
                {
                    flag = true;
                }
            }
            return flag;
        }
    }
}
