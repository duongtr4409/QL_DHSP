using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.QLKH;
using Com.Gosol.QLKH.Ultilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.Gosol.QLKH.DAL.QLKH
{
    public interface IHoiDongDAL
    {
        public List<HoiDongModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow, List<int> listCanBo);
        public HoiDongModel GetByID(int HoiDongID);
        public BaseResultModel Edit_HoiDong(HoiDongModel HoiDongModel);  
        public BaseResultModel Delete_HoiDong(HoiDongModel HoiDongModel);
        public BaseResultModel Insert_DanhGiaHoiDong(List<DanhSachHoiDongDanhGiaModel> DSHoiDongDanhGiaModel);
        public List<DanhSachHoiDongDanhGiaModel> GetByHoiDongID(int HoiDongID, string Keyword, int CapQuanLy, int CanBoID);
    }
    public class HoiDongDAL : IHoiDongDAL
    {
        /// <summary>
        /// Lấy danh sách hội đồng
        /// </summary>
        /// <param name="p"></param>
        /// <param name="TotalRow"></param>
        /// <returns></returns>
        public List<HoiDongModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow, List<int> listCanBo)
        {
            List<HoiDongModel> Result = new List<HoiDongModel>();
            List<ChiTietHoiDongModel> chiTietHoiDongs = new List<ChiTietHoiDongModel>();

            var pList = new SqlParameter("@ListCanBoID", SqlDbType.Structured);
            pList.TypeName = "dbo.list_ID";
            var tbCanBoID = new DataTable();
            var tmp = listCanBo;
            tbCanBoID.Columns.Add("CanBoID", typeof(string));
            listCanBo.ForEach(x => tbCanBoID.Rows.Add(x));

            SqlParameter[] parameters = new SqlParameter[]
                      {
                        new SqlParameter("@Keyword",SqlDbType.NVarChar),
                        new SqlParameter("@OrderByName",SqlDbType.NVarChar),
                        new SqlParameter("@OrderByOption",SqlDbType.NVarChar),
                        new SqlParameter("@pLimit",SqlDbType.Int),
                        new SqlParameter("@pOffset",SqlDbType.Int),
                        new SqlParameter("@TotalRow",SqlDbType.Int),
                        pList,    
                      };
            parameters[0].Value = p.Keyword == null ? "" : p.Keyword.Trim();
            parameters[1].Value = p.OrderByName;
            parameters[2].Value = p.OrderByOption;
            parameters[3].Value = p.Limit;
            parameters[4].Value = p.Offset;
            parameters[5].Direction = ParameterDirection.Output;
            parameters[5].Size = 8;
            parameters[6].Value = tbCanBoID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_HoiDong_GetPagingBySearch", parameters))
                {
                    while (dr.Read())
                    {
                        ChiTietHoiDongModel info = new ChiTietHoiDongModel();
                        info.HoiDongID = Utils.ConvertToInt32(dr["HoiDongID"], 0);
                        info.ChiTietHoiDongID = Utils.ConvertToInt32(dr["ChiTietHoiDongID"], 0);
                        info.TenHoiDong = Utils.ConvertToString(dr["TenHoiDong"], string.Empty);
                        info.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        info.VaiTro = Utils.ConvertToString(dr["VaiTro"], string.Empty);      
                        chiTietHoiDongs.Add(info);
                    }
                    dr.Close();
                }
                Result = chiTietHoiDongs.GroupBy(p => p.HoiDongID)
                    .Select(g => new HoiDongModel
                            {
                                HoiDongID = g.Key,
                                TenHoiDong = g.FirstOrDefault().TenHoiDong,
                                ThanhVienHoiDong = chiTietHoiDongs.Where(x => x.HoiDongID == g.Key && x.ChiTietHoiDongID > 0)
                                        .Select(y => new ThanhVienHoiDongModel
                                            { 
                                                ChiTietHoiDongID = y.ChiTietHoiDongID,
                                                HoiDongID = g.Key,
                                                CanBoID = y.CanBoID,
                                                VaiTro = y.VaiTro,
                                            }
                                        ).ToList()
                            }
                    ).ToList();

                TotalRow = Utils.ConvertToInt32(parameters[5].Value, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        /// <summary>
        /// Chi tiết hội đồng
        /// </summary>
        /// <param name="HoiDongID"></param>
        /// <returns></returns>
        public HoiDongModel GetByID(int HoiDongID)
        {
            List<HoiDongModel> Result = new List<HoiDongModel>();
            List<ChiTietHoiDongModel> chiTietHoiDongs = new List<ChiTietHoiDongModel>();

            SqlParameter[] parameters = new SqlParameter[]
            {
                  new SqlParameter("@HoiDongID",SqlDbType.Int)
            };
            parameters[0].Value = HoiDongID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HoiDong_GetByID", parameters))
                {
                    while (dr.Read())
                    {
                        ChiTietHoiDongModel info = new ChiTietHoiDongModel();
                        info.HoiDongID = Utils.ConvertToInt32(dr["HoiDongID"], 0);
                        info.ChiTietHoiDongID = Utils.ConvertToInt32(dr["ChiTietHoiDongID"], 0);
                        info.TenHoiDong = Utils.ConvertToString(dr["TenHoiDong"], string.Empty);
                        info.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        info.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        info.VaiTro = Utils.ConvertToString(dr["VaiTro"], string.Empty);
                        info.DonViCongTac = Utils.ConvertToString(dr["DonViCongTac"], string.Empty);
                        chiTietHoiDongs.Add(info);
                    }
                    dr.Close();
                }

                if (chiTietHoiDongs.Count > 0)
                {
                    Result = (from m in chiTietHoiDongs
                              group m by m.CanBoID into ctt
                              from item in ctt
                              select new HoiDongModel()
                              {
                                  HoiDongID = item.HoiDongID,
                                  TenHoiDong = item.TenHoiDong,
                                  ThanhVienHoiDong = (from i in chiTietHoiDongs.Where(x => x.ChiTietHoiDongID > 0).ToList().GroupBy(x => x.ChiTietHoiDongID)
                                                 select new ThanhVienHoiDongModel()
                                                 {
                                                     ChiTietHoiDongID = i.FirstOrDefault().CanBoID,
                                                     CanBoID = i.FirstOrDefault().CanBoID,
                                                     VaiTro = i.FirstOrDefault().VaiTro,
                                                     TenCanBo = i.FirstOrDefault().TenCanBo,   
                                                     DonViCongTac = i.FirstOrDefault().DonViCongTac,
                                                 }
                                  ).ToList(),
                              }
                            ).ToList();
                }

                return Result.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Chinh sửa thông tin hội đồng
        /// </summary>
        /// <param name="HoiDongModel"></param>
        /// <returns></returns>
        public BaseResultModel Edit_HoiDong(HoiDongModel HoiDongModel)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parms = new SqlParameter[]{
                    new SqlParameter("HoiDongID", SqlDbType.Int),          
                    new SqlParameter("TenHoiDong", SqlDbType.NVarChar),
                };
                parms[0].Value = HoiDongModel.HoiDongID ?? Convert.DBNull;
                parms[1].Value = HoiDongModel.TenHoiDong ?? Convert.DBNull;

                if (HoiDongModel.HoiDongID == null)
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
                            if (HoiDongModel.HoiDongID > 0)
                            {
                                Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_HoiDong_Update", parms);
                                if(Result.Status > 0 && HoiDongModel.ThanhVienHoiDong != null && HoiDongModel.ThanhVienHoiDong.Count > 0)
                                {
                                    //Xóa thành viên cũ
                                    SqlParameter[] parms_del = new SqlParameter[]{
                                        new SqlParameter("HoiDongID", SqlDbType.Int),                                   
                                    };
                                    parms_del[0].Value = HoiDongModel.HoiDongID;
                                    SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_ThanhVienHoiDong_Delete", parms_del);
                                    //Insert thành viên hội đồng    
                                    foreach (var item in HoiDongModel.ThanhVienHoiDong)
                                    {
                                        SqlParameter[] parms_tv = new SqlParameter[]{
                                            new SqlParameter("HoiDongID", SqlDbType.Int),
                                            new SqlParameter("CanBoID", SqlDbType.Int),
                                            new SqlParameter("VaiTro", SqlDbType.NVarChar),
                                        };
                                        parms_tv[0].Value = HoiDongModel.HoiDongID;
                                        parms_tv[1].Value = item.CanBoID;
                                        parms_tv[2].Value = item.VaiTro ?? Convert.DBNull;

                                        SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_ThanhVienHoiDong_Insert", parms_tv);
                                    }       
                                }
                                Result.Message = ConstantLogMessage.Alert_Update_Success("hội đồng");
                                Result.Data = HoiDongModel.HoiDongID;
                            }
                            else
                            {
                                Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_HoiDong_Insert", parms);
                                int HoiDongID = Utils.ConvertToInt32(parms[0].Value, 0);
                                if (HoiDongModel.ThanhVienHoiDong != null && HoiDongModel.ThanhVienHoiDong.Count > 0)
                                {
                                    //Insert thành viên hội đồng
                                    foreach (var item in HoiDongModel.ThanhVienHoiDong)
                                    {
                                        SqlParameter[] parms_tv = new SqlParameter[]{
                                            new SqlParameter("HoiDongID", SqlDbType.Int),
                                            new SqlParameter("CanBoID", SqlDbType.Int),
                                            new SqlParameter("VaiTro", SqlDbType.NVarChar),
                                        };
                                        parms_tv[0].Value = HoiDongID;
                                        parms_tv[1].Value = item.CanBoID;
                                        parms_tv[2].Value = item.VaiTro ?? Convert.DBNull;

                                        SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_ThanhVienHoiDong_Insert", parms_tv);
                                    }
                                }
                                Result.Message = ConstantLogMessage.Alert_Insert_Success("hội đồng");
                                Result.Data = HoiDongID;
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
        /// Xóa hội đồng
        /// </summary>
        /// <param name="HoiDongModel"></param>
        /// <returns></returns>
        public BaseResultModel Delete_HoiDong(HoiDongModel HoiDongModel)
        {
            var Result = new BaseResultModel();
            SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@HoiDongID",SqlDbType.Int)
                };
            parameters[0].Value = HoiDongModel.HoiDongID;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, @"v1_HoiDong_Delete", parameters);
                        Result.Message = ConstantLogMessage.Alert_Delete_Success("hội đồng");
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

        /// <summary>
        /// Lưu danh sách hội đồng dánh giá đề xuất
        /// </summary>
        /// <param name="DSHoiDongDanhGiaModel"></param>
        /// <returns></returns>
        public BaseResultModel Insert_DanhGiaHoiDong(List<DanhSachHoiDongDanhGiaModel> DSHoiDongDanhGiaModel)
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
                            //Xóa danh sách cũ
                            SqlParameter[] parms_del = new SqlParameter[]{
                                        new SqlParameter("HoiDongID", SqlDbType.Int),
                                    };
                            parms_del[0].Value = DSHoiDongDanhGiaModel[0].HoiDongID;
                            SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_DanhSachHoiDongDanhGia_Delete", parms_del);
                            //Insert danh sách đánh giá  
                            foreach (var item in DSHoiDongDanhGiaModel)
                            {
                                if(item.DeXuatID != null)
                                {
                                    SqlParameter[] parms_tv = new SqlParameter[]{
                                            new SqlParameter("HoiDongID", SqlDbType.Int),
                                            new SqlParameter("DeXuatID", SqlDbType.Int),
                                        };
                                    parms_tv[0].Value = item.HoiDongID ?? Convert.DBNull;
                                    parms_tv[1].Value = item.DeXuatID ?? Convert.DBNull;

                                    SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_DanhSachHoiDongDanhGia_Insert", parms_tv);
                                }
                            }

                            Result.Message = ConstantLogMessage.Alert_Update_Success("danh sách đánh giá");
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
        /// Lấy danh sách đánh giá theo HoiDongID
        /// </summary>
        /// <param name="HoiDongID"></param>
        /// <returns></returns>
        public List<DanhSachHoiDongDanhGiaModel> GetByHoiDongID(int HoiDongID, string Keyword, int CapQuanLy, int CanBoID)
        {
            List<DanhSachHoiDongDanhGiaModel> Result = new List<DanhSachHoiDongDanhGiaModel>();

            SqlParameter[] parameters = new SqlParameter[]
            {
                  new SqlParameter("@HoiDongID",SqlDbType.Int),
                  new SqlParameter("@Keyword",SqlDbType.NVarChar),
                  new SqlParameter("@CapQuanLy",SqlDbType.Int),
                  new SqlParameter("@CanBoID",SqlDbType.Int),
            };
            parameters[0].Value = HoiDongID;
            parameters[1].Value = Keyword == null ? "" : Keyword.Trim();
            parameters[2].Value = CapQuanLy;
            parameters[3].Value = CanBoID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_DanhSachHoiDongDanhGia_GetByID", parameters))
                {
                    while (dr.Read())
                    {
                        DanhSachHoiDongDanhGiaModel info = new DanhSachHoiDongDanhGiaModel();
                        info.DeXuatID = Utils.ConvertToInt32(dr["DeXuatID"], 0);     
                        info.TenDeXuat = Utils.ConvertToString(dr["TenDeXuat"], string.Empty);
                        info.MaDeXuat = Utils.ConvertToString(dr["MaDeXuat"], string.Empty);
                        info.LinhVucNghienCuu = Utils.ConvertToInt32(dr["LinhVucNghienCuu"], 0);
                        info.TenLinhVucNghienCuu = Utils.ConvertToString(dr["TenLinhVucNghienCuu"], string.Empty);
                        info.LinhVucKinhTeXaHoi = Utils.ConvertToInt32(dr["LinhVucKinhTeXaHoi"], 0);
                        info.TenLinhVucKinhTeXaHoi = Utils.ConvertToString(dr["TenLinhVucKinhTeXaHoi"], string.Empty);
                        info.CapQuanLy = Utils.ConvertToInt32(dr["CapQuanLy"], 0);
                        info.TenCapQuanLy = Utils.ConvertToString(dr["TenCapQuanLy"], string.Empty);
                        info.NguoiDeXuat = Utils.ConvertToInt32(dr["NguoiDeXuat"], 0);
                        info.TenNguoiDeXuat = Utils.ConvertToString(dr["TenNguoiDeXuat"], string.Empty);
                        info.ID = Utils.ConvertToInt32(dr["ID"], 0);
                        info.HoiDongID = Utils.ConvertToInt32(dr["HoiDongID"], 0);
                        if(info.HoiDongID > 0)
                        {
                            info.DanhGia = true;
                        }
                        else info.DanhGia = false;
                        Result.Add(info);
                    }
                    dr.Close();
                }

                return Result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
