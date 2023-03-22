using Com.Gosol.QLKH.DAL.QuanTriHeThong;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.QLKH;
using Com.Gosol.QLKH.Models.QuanTriHeThong;
using Com.Gosol.QLKH.Ultilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.Gosol.QLKH.DAL.QLKH
{
    public interface IQuanLyThongBaoDAL
    {
        public List<QuanLyThongBaoModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow);
        public BaseResultModel Edit_ThongBao(QuanLyThongBaoModel ThongBaoModel);
        public QuanLyThongBaoModel GetByID(int ThongBaoID);
        public BaseResultModel Delete_ThongBao(QuanLyThongBaoModel ThongBaoModel);
        public List<ChiTietHienThiThongBaoModel> GetDSThongBaoHienThi(BasePagingParams p, ref int TotalRow, int CanBoID, int CoQuanID);
        public BaseResultModel Update_TrangThaiTatThongBao(List<DoiTuongThongBaoModel> DoiTuongThongBao);
        public List<DoiTuongDeTaiModel> GetDS_DoiTuongThongBaoTheoCap(int? CapQuanLy, int? NamBatDau);
    }
    public class QuanLyThongBaoDAL : IQuanLyThongBaoDAL
    {
        /// <summary>
        /// Lấy danh sách thông báo
        /// </summary>
        /// <param name="p"></param>
        /// <param name="TotalRow"></param>
        /// <returns></returns>
        public List<QuanLyThongBaoModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow)
        {
            List<QuanLyThongBaoModel> thongBaos = new List<QuanLyThongBaoModel>();
            List<ChiTietThongBaoModel> chiTietThongBaos = new List<ChiTietThongBaoModel>();

            SqlParameter[] parameters = new SqlParameter[]
                      {
                        new SqlParameter("@Keyword",SqlDbType.NVarChar),
                        new SqlParameter("@OrderByName",SqlDbType.NVarChar),
                        new SqlParameter("@OrderByOption",SqlDbType.NVarChar),
                        new SqlParameter("@pLimit",SqlDbType.Int),
                        new SqlParameter("@pOffset",SqlDbType.Int),
                        new SqlParameter("@TotalRow",SqlDbType.Int),
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
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_QuanLyThongBao_GetPagingBySearch", parameters))
                {
                    while (dr.Read())
                    {
                        ChiTietThongBaoModel info = new ChiTietThongBaoModel();
                        info.ThongBaoID = Utils.ConvertToInt32(dr["ThongBaoID"], 0);
                        info.TenThongBao = Utils.ConvertToString(dr["TenThongBao"], string.Empty);
                        info.NoiDung = Utils.ConvertToString(dr["NoiDung"], string.Empty);
                        info.ThoiGianBatDau = Utils.ConvertToDateTime(dr["ThoiGianBatDau"], DateTime.MinValue);
                        info.ThoiGianKetThuc = Utils.ConvertToDateTime(dr["ThoiGianKetThuc"], DateTime.MinValue);
                        info.HienThi = Utils.ConvertToBoolean(dr["HienThi"], false);
                        info.CapQuanLy = Utils.ConvertToInt32(dr["CapQuanLy"], 0);
                        info.NamBatDau = Utils.ConvertToInt32(dr["NamBatDau"], 0);

                        info.DoiTuongID = Utils.ConvertToInt32(dr["DoiTuongID"], 0);
                        info.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);

                        info.FileDinhKemID = Utils.ConvertToInt32(dr["FileDinhKemID"], 0);
                        if (info.FileDinhKemID > 0)
                        {
                            info.NghiepVuID = Utils.ConvertToInt32(dr["NghiepVuID"], 0);
                            info.TenFileGoc = Utils.ConvertToString(dr["TenFileGoc"], string.Empty);
                            info.TenFileHeThong = Utils.ConvertToString(dr["TenFileHeThong"], string.Empty);
                            info.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                            info.LoaiFile = Utils.ConvertToInt32(dr["LoaiFile"], 0);
                            info.NguoiTaoID = Utils.ConvertToInt32(dr["NguoiTaoID"], 0);
                            info.TenNguoiTao = Utils.ConvertToString(dr["TenNguoiTao"], string.Empty);
                            info.NgayTao = Utils.ConvertToDateTime(dr["NgayTao"], DateTime.Now);
                            info.NoiDungFile = Utils.ConvertToString(dr["NoiDungFile"], string.Empty);
                        }

                        chiTietThongBaos.Add(info);
                    }
                    dr.Close();
                }

                TotalRow = Utils.ConvertToInt32(parameters[5].Value, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            thongBaos = chiTietThongBaos.GroupBy(p => p.ThongBaoID)
                    .Select(g => new QuanLyThongBaoModel
                    {
                        ThongBaoID = g.Key,
                        TenThongBao = g.FirstOrDefault().TenThongBao,
                        NoiDung = g.FirstOrDefault().NoiDung,
                        ThoiGianBatDau = g.FirstOrDefault().ThoiGianBatDau,
                        ThoiGianKetThuc = g.FirstOrDefault().ThoiGianKetThuc,
                        HienThi = g.FirstOrDefault().HienThi,
                        CapQuanLy = g.FirstOrDefault().CapQuanLy,
                        TenCapQuanLy = g.FirstOrDefault().TenCapQuanLy,
                        NamBatDau = g.FirstOrDefault().NamBatDau,
                        DoiTuongThongBao = chiTietThongBaos.Where(x => x.ThongBaoID == g.Key && x.DoiTuongID > 0).GroupBy(x => x.DoiTuongID)
                                        .Select(y => new DoiTuongThongBaoModel
                                        {
                                            DoiTuongID = y.FirstOrDefault().DoiTuongID,
                                            ThongBaoID = g.Key,
                                            CanBoID = y.FirstOrDefault().CanBoID,
                                            CoQuanID = y.FirstOrDefault().CoQuanID,
                                        }
                                        ).ToList(),
                        FileDinhKem = chiTietThongBaos.Where(x => x.ThongBaoID == g.Key && x.FileDinhKemID > 0).GroupBy(x => x.FileDinhKemID)
                                        .Select(y => new FileDinhKemModel
                                        {
                                            NghiepVuID = g.Key ?? 0,
                                            FileDinhKemID = y.FirstOrDefault().FileDinhKemID,
                                            TenFileHeThong = y.FirstOrDefault().TenFileHeThong,
                                            TenFileGoc = y.FirstOrDefault().TenFileGoc,
                                            LoaiFile = y.FirstOrDefault().LoaiFile,
                                            FileUrl = y.FirstOrDefault().FileUrl,
                                            NoiDung = y.FirstOrDefault().NoiDungFile,
                                            NgayTao = y.FirstOrDefault().NgayTao,
                                            NguoiTaoID = y.FirstOrDefault().NguoiTaoID,
                                            TenNguoiTao = y.FirstOrDefault().TenNguoiTao,
                                        }
                                        ).ToList()
                    }
                    ).OrderByDescending(x => x.ThongBaoID).ToList();

            return thongBaos;
        }

        /// <summary>
        /// Chỉnh sửa thông báo
        /// </summary>
        /// <param name="ThongBaoModel"></param>
        /// <returns></returns>
        public BaseResultModel Edit_ThongBao(QuanLyThongBaoModel ThongBaoModel)
        {
            var Result = new BaseResultModel();
            //if ((ThongBaoModel.CapQuanLy != null && ThongBaoModel.CapQuanLy > 0) || (ThongBaoModel.NamBatDau != null && ThongBaoModel.NamBatDau > 0))
            //{
            //    //ThongBaoModel.DoiTuongThongBao = GetDoiTuongThongBao(ThongBaoModel.CapQuanLy, ThongBaoModel.NamBatDau);

            //    //if (ThongBaoModel.DoiTuongThongBao == null || ThongBaoModel.DoiTuongThongBao.Count == 0)
            //    //{
            //    //    ThongBaoModel.DoiTuongThongBao = GetDoiTuongThongBao(ThongBaoModel.CapQuanLy, ThongBaoModel.NamBatDau);
            //    //}
            //    //else
            //    //{
            //    //    var listDT = GetDoiTuongThongBao(ThongBaoModel.CapQuanLy, ThongBaoModel.NamBatDau);
            //    //    for (int i = 0; i < listDT.Count; i++)
            //    //    {
            //    //        Boolean check = true;
            //    //        for (int j = 0; j < ThongBaoModel.DoiTuongThongBao.Count; j++)
            //    //        {
            //    //            if (ThongBaoModel.DoiTuongThongBao[j].CanBoID == listDT[i].CanBoID && ThongBaoModel.DoiTuongThongBao[j].CoQuanID == listDT[i].CoQuanID)
            //    //            {
            //    //                check = false;
            //    //                break;
            //    //            }
            //    //        }
            //    //        if (check)
            //    //        {
            //    //            ThongBaoModel.DoiTuongThongBao.Add(listDT[i]);
            //    //        }
            //    //    }
            //    //}
            //}
            try
            {
                SqlParameter[] parms = new SqlParameter[]{
                    new SqlParameter("ThongBaoID", SqlDbType.Int),
                    new SqlParameter("TenThongBao", SqlDbType.NVarChar),
                    new SqlParameter("NoiDung", SqlDbType.NVarChar),
                    new SqlParameter("ThoiGianBatDau", SqlDbType.DateTime),
                    new SqlParameter("ThoiGianKetThuc", SqlDbType.DateTime),
                    new SqlParameter("HienThi", SqlDbType.Bit),
                    new SqlParameter("LoaiThongBao", SqlDbType.Int),
                    new SqlParameter("CapQuanLy", SqlDbType.Int),
                    new SqlParameter("NamBatDau", SqlDbType.Int),
                };
                parms[0].Value = ThongBaoModel.ThongBaoID ?? Convert.DBNull;
                parms[1].Value = ThongBaoModel.TenThongBao ?? Convert.DBNull;
                parms[2].Value = ThongBaoModel.NoiDung ?? Convert.DBNull;
                parms[3].Value = ThongBaoModel.ThoiGianBatDau ?? Convert.DBNull;
                parms[4].Value = ThongBaoModel.ThoiGianKetThuc ?? Convert.DBNull;
                parms[5].Value = ThongBaoModel.HienThi ?? Convert.DBNull;
                parms[6].Value = ThongBaoModel.LoaiThongBao ?? Convert.DBNull;
                parms[7].Value = ThongBaoModel.CapQuanLy ?? Convert.DBNull;
                parms[8].Value = ThongBaoModel.NamBatDau ?? Convert.DBNull;

                if (ThongBaoModel.ThongBaoID == null)
                {
                    parms[0].Direction = ParameterDirection.Output;
                    parms[0].Size = 8;
                }

                SqlParameter[] parms_log = new SqlParameter[]{
                    new SqlParameter("ID", SqlDbType.Int),
                    new SqlParameter("ThongBaoID", SqlDbType.Int),
                    new SqlParameter("TenThongBao", SqlDbType.NVarChar),
                    new SqlParameter("NoiDung", SqlDbType.NVarChar),
                    new SqlParameter("ThoiGianBatDau", SqlDbType.DateTime),
                    new SqlParameter("ThoiGianKetThuc", SqlDbType.DateTime),
                    new SqlParameter("HienThi", SqlDbType.Bit),
                    new SqlParameter("LoaiThongBao", SqlDbType.Int),
                    new SqlParameter("CapQuanLy", SqlDbType.Int),
                    new SqlParameter("NamBatDau", SqlDbType.Int),
                    new SqlParameter("CanBoID", SqlDbType.Int),
                    new SqlParameter("CoQuanID", SqlDbType.Int),
                    new SqlParameter("NgayChinhSua", SqlDbType.DateTime),
                };
                parms_log[0].Direction = ParameterDirection.Output;
                parms_log[0].Size = 8;
                parms_log[1].Value = ThongBaoModel.ThongBaoID ?? Convert.DBNull;
                parms_log[2].Value = ThongBaoModel.TenThongBao ?? Convert.DBNull;
                parms_log[3].Value = ThongBaoModel.NoiDung ?? Convert.DBNull;
                parms_log[4].Value = ThongBaoModel.ThoiGianBatDau ?? Convert.DBNull;
                parms_log[5].Value = ThongBaoModel.ThoiGianKetThuc ?? Convert.DBNull;
                parms_log[6].Value = ThongBaoModel.HienThi ?? Convert.DBNull;
                parms_log[7].Value = ThongBaoModel.LoaiThongBao ?? Convert.DBNull;
                parms_log[8].Value = ThongBaoModel.CapQuanLy ?? Convert.DBNull;
                parms_log[9].Value = ThongBaoModel.NamBatDau ?? Convert.DBNull;
                parms_log[10].Value = ThongBaoModel.CanBoID ?? Convert.DBNull;
                parms_log[11].Value = ThongBaoModel.CoQuanID ?? Convert.DBNull;
                parms_log[12].Value = DateTime.Now;

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            if (ThongBaoModel.ThongBaoID > 0)
                            {
                                Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_QuanLyThongBao_Update", parms);

                                //Xóa đối tượng thông báo cũ
                                SqlParameter[] parms_del = new SqlParameter[]{
                                    new SqlParameter("ThongBaoID", SqlDbType.Int),
                                };
                                parms_del[0].Value = ThongBaoModel.ThongBaoID;
                                SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_DoiTuongThongBao_Delete", parms_del);
                                if (Result.Status > 0 && ThongBaoModel.DoiTuongThongBao != null && ThongBaoModel.DoiTuongThongBao.Count > 0)
                                {
                                    //Insert đối tượng thông báo
                                    foreach (var item in ThongBaoModel.DoiTuongThongBao)
                                    {
                                        SqlParameter[] parms_tv = new SqlParameter[]{
                                            new SqlParameter("ThongBaoID", SqlDbType.Int),
                                            new SqlParameter("CanBoID", SqlDbType.Int),
                                            new SqlParameter("CoQuanID", SqlDbType.Int),
                                        };
                                        parms_tv[0].Value = ThongBaoModel.ThongBaoID;
                                        parms_tv[1].Value = item.CanBoID ?? Convert.DBNull;
                                        parms_tv[2].Value = item.CoQuanID ?? Convert.DBNull;

                                        SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_DoiTuongThongBao_Insert", parms_tv);
                                    }
                                }
                                Result.Message = ConstantLogMessage.Alert_Update_Success("thông báo");
                                Result.Data = ThongBaoModel.ThongBaoID;
                            }
                            else
                            {
                                Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_QuanLyThongBao_Insert", parms);
                                int ThongBaoID = Utils.ConvertToInt32(parms[0].Value, 0);
                                if (ThongBaoModel.DoiTuongThongBao != null && ThongBaoModel.DoiTuongThongBao.Count > 0)
                                {
                                    //Insert đối tượng thông báo
                                    foreach (var item in ThongBaoModel.DoiTuongThongBao)
                                    {
                                        SqlParameter[] parms_tv = new SqlParameter[]{
                                            new SqlParameter("ThongBaoID", SqlDbType.Int),
                                            new SqlParameter("CanBoID", SqlDbType.Int),
                                            new SqlParameter("CoQuanID", SqlDbType.Int),
                                        };
                                        parms_tv[0].Value = ThongBaoID;
                                        parms_tv[1].Value = item.CanBoID ?? Convert.DBNull;
                                        parms_tv[2].Value = item.CoQuanID ?? Convert.DBNull;

                                        SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_DoiTuongThongBao_Insert", parms_tv);
                                    }
                                }
                                Result.Message = ConstantLogMessage.Alert_Insert_Success("thông báo");
                                Result.Data = ThongBaoID;
                            }
                            //insert log
                            parms_log[1].Value = Result.Data;
                            SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_QuanLyThongBaoLog_Insert", parms_log);
                            int ID = Utils.ConvertToInt32(parms_log[0].Value, 0);
                            if (ThongBaoModel.DoiTuongThongBao != null && ThongBaoModel.DoiTuongThongBao.Count > 0)
                            {
                                //Insert đối tượng thông báo
                                foreach (var item in ThongBaoModel.DoiTuongThongBao)
                                {
                                    SqlParameter[] parms_tv = new SqlParameter[]{
                                            new SqlParameter("ID", SqlDbType.Int),
                                            new SqlParameter("ThongBaoID", SqlDbType.Int),
                                            new SqlParameter("CanBoID", SqlDbType.Int),
                                            new SqlParameter("CoQuanID", SqlDbType.Int),
                                        };
                                    parms_tv[0].Value = ID;
                                    parms_tv[1].Value = Result.Data;
                                    parms_tv[2].Value = item.CanBoID ?? Convert.DBNull;
                                    parms_tv[3].Value = item.CoQuanID ?? Convert.DBNull;

                                    SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_DoiTuongThongBaoLog_Insert", parms_tv);
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
        /// Chi tiết thông báo
        /// </summary>
        /// <param name="ThongBaoID"></param>
        /// <returns></returns>
        public QuanLyThongBaoModel GetByID(int ThongBaoID)
        {
            List<QuanLyThongBaoModel> Result = new List<QuanLyThongBaoModel>();
            List<ChiTietThongBaoModel> chiTiets = new List<ChiTietThongBaoModel>();

            SqlParameter[] parameters = new SqlParameter[]
            {
                  new SqlParameter("@ThongBaoID",SqlDbType.Int)
            };
            parameters[0].Value = ThongBaoID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_QuanLyThongBao_GetByID", parameters))
                {
                    while (dr.Read())
                    {
                        ChiTietThongBaoModel info = new ChiTietThongBaoModel();
                        //info.ThongBaoID = Utils.ConvertToInt32(dr["ThongBaoID"], 0);
                        //info.TenThongBao = Utils.ConvertToString(dr["TenThongBao"], string.Empty);
                        //info.NoiDung = Utils.ConvertToString(dr["NoiDung"], string.Empty);
                        //info.ThoiGianBatDau = Utils.ConvertToDateTime(dr["ThoiGianBatDau"], DateTime.MinValue);
                        //info.ThoiGianKetThuc = Utils.ConvertToDateTime(dr["ThoiGianKetThuc"], DateTime.MinValue);
                        //info.HienThi = Utils.ConvertToBoolean(dr["HienThi"], false);
                        //info.CapQuanLy = Utils.ConvertToInt32(dr["CapQuanLy"], 0);
                        //info.TenCapQuanLy = Utils.ConvertToString(dr["TenCapQuanLy"], string.Empty);
                        //info.NamBatDau = Utils.ConvertToInt32(dr["NamBatDau"], 0);

                        //info.DoiTuongID = Utils.ConvertToInt32(dr["DoiTuongID"], 0);
                        //info.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        //info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);

                        //info.FileDinhKemID = Utils.ConvertToInt32(dr["FileDinhKemID"], 0);
                        //if (info.FileDinhKemID > 0)
                        //{
                        //    info.NghiepVuID = Utils.ConvertToInt32(dr["NghiepVuID"], 0);
                        //    info.TenFileGoc = Utils.ConvertToString(dr["TenFileGoc"], string.Empty);
                        //    info.TenFileHeThong = Utils.ConvertToString(dr["TenFileHeThong"], string.Empty);
                        //    info.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        //    info.LoaiFile = Utils.ConvertToInt32(dr["LoaiFile"], 0);
                        //    info.NguoiTaoID = Utils.ConvertToInt32(dr["NguoiTaoID"], 0);
                        //    info.TenNguoiTao = Utils.ConvertToString(dr["TenNguoiTao"], string.Empty);
                        //    info.NgayTao = Utils.ConvertToDateTime(dr["NgayTao"], DateTime.Now);
                        //    info.NoiDungFile = Utils.ConvertToString(dr["NoiDungFile"], string.Empty);
                        //}

                        info.ID = Utils.ConvertToInt32(dr["ID"], 0);
                        info.TenThongBaoLog = Utils.ConvertToString(dr["TenThongBaoLog"], string.Empty);
                        info.NoiDungLog = Utils.ConvertToString(dr["NoiDungLog"], string.Empty);
                        info.ThoiGianBatDauLog = Utils.ConvertToDateTime(dr["ThoiGianBatDauLog"], DateTime.MinValue);
                        info.ThoiGianKetThucLog = Utils.ConvertToDateTime(dr["ThoiGianKetThucLog"], DateTime.MinValue);
                        info.HienThiLog = Utils.ConvertToBoolean(dr["HienThiLog"], false);
                        info.CapQuanLyLog = Utils.ConvertToInt32(dr["CapQuanLyLog"], 0);
                        info.TenCapQuanLyLog = Utils.ConvertToString(dr["TenCapQuanLyLog"], string.Empty);
                        info.NamBatDauLog = Utils.ConvertToInt32(dr["NamBatDauLog"], 0);
                        info.CanBoChinhSuaID = Utils.ConvertToInt32(dr["CanBoChinhSuaID"], 0);
                        info.CoQuanChinhSuaID = Utils.ConvertToInt32(dr["CoQuanChinhSuaID"], 0);
                        info.NgayChinhSua = Utils.ConvertToNullableDateTime(dr["NgayChinhSua"], null);
                        //info.TenNguoiChinhSua = Utils.ConvertToString(dr["TenNguoiChinhSua"], string.Empty);

                        info.DoiTuongIDLog = Utils.ConvertToInt32(dr["DoiTuongIDLog"], 0);
                        info.CanBoIDLog = Utils.ConvertToInt32(dr["CanBoIDLog"], 0);
                        info.CoQuanIDLog = Utils.ConvertToInt32(dr["CoQuanIDLog"], 0);
                        chiTiets.Add(info);
                    }
                    dr.Close();
                }

                if (chiTiets.Count > 0)
                {
                    Result = (from m in chiTiets
                              group m by m.ThongBaoID into ctt
                              from item in ctt
                              select new QuanLyThongBaoModel()
                              {
                                  ThongBaoID = item.ThongBaoID,
                                  TenThongBao = item.TenThongBao,
                                  NoiDung = item.NoiDung,
                                  ThoiGianBatDau = item.ThoiGianBatDau,
                                  ThoiGianKetThuc = item.ThoiGianKetThuc,
                                  HienThi = item.HienThi,
                                  CapQuanLy = item.CapQuanLy,
                                  TenCapQuanLy = item.TenCapQuanLy,
                                  NamBatDau = item.NamBatDau,
                                  DoiTuongThongBao = (from i in chiTiets.Where(x => x.DoiTuongID > 0).ToList().GroupBy(x => x.DoiTuongID)
                                                      select new DoiTuongThongBaoModel()
                                                      {
                                                          DoiTuongID = i.FirstOrDefault().DoiTuongID,
                                                          ThongBaoID = i.FirstOrDefault().ThongBaoID,
                                                          CanBoID = i.FirstOrDefault().CanBoID,
                                                          CoQuanID = i.FirstOrDefault().CoQuanID,
                                                      }
                                  ).ToList(),
                                  FileDinhKem = (from i in chiTiets.Where(x => x.FileDinhKemID > 0).ToList().GroupBy(x => x.FileDinhKemID)
                                                 select new FileDinhKemModel
                                                 {
                                                     NghiepVuID = i.FirstOrDefault().NghiepVuID,
                                                     FileDinhKemID = i.FirstOrDefault().FileDinhKemID,
                                                     TenFileHeThong = i.FirstOrDefault().TenFileHeThong,
                                                     TenFileGoc = i.FirstOrDefault().TenFileGoc,
                                                     LoaiFile = i.FirstOrDefault().LoaiFile,
                                                     FileUrl = i.FirstOrDefault().FileUrl,
                                                     NoiDung = i.FirstOrDefault().NoiDung,
                                                     NgayTao = i.FirstOrDefault().NgayTao,
                                                     NguoiTaoID = i.FirstOrDefault().NguoiTaoID,
                                                     TenNguoiTao = i.FirstOrDefault().TenNguoiTao,
                                                 }
                                  ).ToList(),
                                  LichSuChinhSuaThongBao = (from i in chiTiets.Where(x => x.ID > 0).ToList().GroupBy(x => x.ID)
                                                            select new LichSuChinhSuaThongBaoModel()
                                                            {
                                                                ID = i.FirstOrDefault().ID,
                                                                ThongBaoID = i.FirstOrDefault().ThongBaoID,
                                                                TenThongBao = i.FirstOrDefault().TenThongBaoLog,
                                                                NoiDung = i.FirstOrDefault().NoiDungLog,
                                                                ThoiGianBatDau = i.FirstOrDefault().ThoiGianBatDauLog,
                                                                ThoiGianKetThuc = i.FirstOrDefault().ThoiGianKetThucLog,
                                                                HienThi = i.FirstOrDefault().HienThiLog,
                                                                CapQuanLy = i.FirstOrDefault().CapQuanLyLog,
                                                                TenCapQuanLy = i.FirstOrDefault().TenCapQuanLyLog,
                                                                NamBatDau = i.FirstOrDefault().NamBatDauLog,
                                                                CanBoID = i.FirstOrDefault().CanBoChinhSuaID,
                                                                CoQuanID = i.FirstOrDefault().CoQuanChinhSuaID,
                                                                NgayChinhSua = i.FirstOrDefault().NgayChinhSua,
                                                                TenNguoiChinhSua = i.FirstOrDefault().TenNguoiChinhSua,
                                                                DoiTuongThongBao = (from j in chiTiets.Where(x => x.DoiTuongIDLog > 0 && x.ID == i.FirstOrDefault().ID).ToList().GroupBy(x => x.DoiTuongIDLog)
                                                                                    select new DoiTuongThongBaoModel()
                                                                                    {
                                                                                        DoiTuongID = j.FirstOrDefault().DoiTuongIDLog,
                                                                                        ThongBaoID = j.FirstOrDefault().ThongBaoID,
                                                                                        CanBoID = j.FirstOrDefault().CanBoIDLog,
                                                                                        CoQuanID = j.FirstOrDefault().CoQuanIDLog,
                                                                                    }
                                                                      ).ToList(),
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
        /// Xóa thông báo
        /// </summary>
        /// <param name="ThongBaoModel"></param>
        /// <returns></returns>
        public BaseResultModel Delete_ThongBao(QuanLyThongBaoModel ThongBaoModel)
        {
            var Result = new BaseResultModel();
            SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@ThongBaoID",SqlDbType.Int)
                };
            parameters[0].Value = ThongBaoModel.ThongBaoID;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, @"v1_QuanLyThongBao_Delete", parameters);
                        Result.Message = ConstantLogMessage.Alert_Delete_Success("thông báo");
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
        /// Danh sách các thông báo hiển thị màn hình người dùng
        /// </summary>
        /// <param name="p"></param>
        /// <param name="TotalRow"></param>
        /// <param name="CanBoID"></param>
        /// <param name="CoQuanID"></param>
        /// <returns></returns>
        public List<ChiTietHienThiThongBaoModel> GetDSThongBaoHienThi(BasePagingParams p, ref int TotalRow, int CanBoID, int CoQuanID)
        {
            List<ChiTietHienThiThongBaoModel> thongBaos = new List<ChiTietHienThiThongBaoModel>();
            List<ChiTietThongBaoModel> chiTietThongBaos = new List<ChiTietThongBaoModel>();

            SqlParameter[] parameters = new SqlParameter[]
                      {
                        new SqlParameter("@Keyword",SqlDbType.NVarChar),
                        new SqlParameter("@CanBoID",SqlDbType.Int),
                        new SqlParameter("@CoQuanID",SqlDbType.Int),
                        new SqlParameter("@pLimit",SqlDbType.Int),
                        new SqlParameter("@pOffset",SqlDbType.Int),
                        new SqlParameter("@TotalRow",SqlDbType.Int),
                      };
            parameters[0].Value = p.Keyword == null ? "" : p.Keyword.Trim();
            parameters[1].Value = CanBoID;
            parameters[2].Value = CoQuanID;
            parameters[3].Value = p.Limit;
            parameters[4].Value = p.Offset;
            parameters[5].Direction = ParameterDirection.Output;
            parameters[5].Size = 8;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_QuanLyThongBao_HienThi", parameters))
                {
                    while (dr.Read())
                    {
                        ChiTietThongBaoModel info = new ChiTietThongBaoModel();
                        info.ThongBaoID = Utils.ConvertToInt32(dr["ThongBaoID"], 0);
                        info.TenThongBao = Utils.ConvertToString(dr["TenThongBao"], string.Empty);
                        info.NoiDung = Utils.ConvertToString(dr["NoiDung"], string.Empty);
                        info.ThoiGianBatDau = Utils.ConvertToDateTime(dr["ThoiGianBatDau"], DateTime.MinValue);
                        info.ThoiGianKetThuc = Utils.ConvertToDateTime(dr["ThoiGianKetThuc"], DateTime.MinValue);
                        info.HienThi = Utils.ConvertToBoolean(dr["HienThi"], false);

                        info.DoiTuongID = Utils.ConvertToInt32(dr["DoiTuongID"], 0);
                        info.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);

                        info.FileDinhKemID = Utils.ConvertToInt32(dr["FileDinhKemID"], 0);
                        if (info.FileDinhKemID > 0)
                        {
                            info.NghiepVuID = Utils.ConvertToInt32(dr["NghiepVuID"], 0);
                            info.TenFileGoc = Utils.ConvertToString(dr["TenFileGoc"], string.Empty);
                            info.TenFileHeThong = Utils.ConvertToString(dr["TenFileHeThong"], string.Empty);
                            info.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                            info.LoaiFile = Utils.ConvertToInt32(dr["LoaiFile"], 0);
                            info.NguoiTaoID = Utils.ConvertToInt32(dr["NguoiTaoID"], 0);
                            info.TenNguoiTao = Utils.ConvertToString(dr["TenNguoiTao"], string.Empty);
                            info.NgayTao = Utils.ConvertToDateTime(dr["NgayTao"], DateTime.Now);
                            info.NoiDungFile = Utils.ConvertToString(dr["NoiDungFile"], string.Empty);
                        }

                        chiTietThongBaos.Add(info);
                    }
                    dr.Close();
                }

                TotalRow = Utils.ConvertToInt32(parameters[5].Value, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            thongBaos = chiTietThongBaos.GroupBy(p => p.ThongBaoID)
                 .Select(g => new ChiTietHienThiThongBaoModel
                 {
                     ThongBaoID = g.Key,
                     TenThongBao = g.FirstOrDefault().TenThongBao,
                     NoiDung = g.FirstOrDefault().NoiDung,
                     ThoiGianBatDau = g.FirstOrDefault().ThoiGianBatDau,
                     ThoiGianKetThuc = g.FirstOrDefault().ThoiGianKetThuc,
                     HienThi = g.FirstOrDefault().HienThi,
                     DoiTuongID = g.FirstOrDefault().DoiTuongID,
                     CanBoID = g.FirstOrDefault().CanBoID,
                     CoQuanID = g.FirstOrDefault().CoQuanID,
                     FileDinhKem = chiTietThongBaos.Where(x => x.ThongBaoID == g.Key && x.FileDinhKemID > 0).GroupBy(x => x.FileDinhKemID)
                                     .Select(y => new FileDinhKemModel
                                     {
                                         NghiepVuID = g.Key ?? 0,
                                         FileDinhKemID = y.FirstOrDefault().FileDinhKemID,
                                         TenFileHeThong = y.FirstOrDefault().TenFileHeThong,
                                         TenFileGoc = y.FirstOrDefault().TenFileGoc,
                                         LoaiFile = y.FirstOrDefault().LoaiFile,
                                         FileUrl = y.FirstOrDefault().FileUrl,
                                         NoiDung = y.FirstOrDefault().NoiDungFile,
                                         NgayTao = y.FirstOrDefault().NgayTao,
                                         NguoiTaoID = y.FirstOrDefault().NguoiTaoID,
                                         TenNguoiTao = y.FirstOrDefault().TenNguoiTao,
                                     }
                                     ).ToList()
                 }
                 ).OrderByDescending(x => x.ThongBaoID).ToList();

            return thongBaos;
        }

        /// <summary>
        /// Tắt thông báo đã hiển thị
        /// </summary>
        /// <param name="DoiTuongThongBao"></param>
        /// <returns></returns>
        public BaseResultModel Update_TrangThaiTatThongBao(List<DoiTuongThongBaoModel> DoiTuongThongBao)
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
                            foreach (var item in DoiTuongThongBao)
                            {
                                SqlParameter[] parameters = new SqlParameter[]
                                {
                                    new SqlParameter("DoiTuongID", SqlDbType.Int),
                                    new SqlParameter("TatThongBao", SqlDbType.Bit),
                                };
                                parameters[0].Value = item.DoiTuongID;
                                parameters[1].Value = true;

                                Result.Status = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v1_DoiTuongThongBao_TatThongBao", parameters);
                            }

                            trans.Commit();
                            Result.Status = 1;
                            Result.Message = ConstantLogMessage.Alert_Update_Success("thông báo");
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
        /// Lấy đối tượng thông báo theo cấp quản lý và năm bắt đầu đề tài
        /// </summary>
        /// <param name="CapQuanLy"></param>
        /// <param name="NamBatDau"></param>
        /// <returns></returns>
        public List<DoiTuongThongBaoModel> GetDoiTuongThongBao(int? CapQuanLy, int? NamBatDau)
        {
            List<DoiTuongThongBaoModel> Result = new List<DoiTuongThongBaoModel>();

            //var dmCap = new DanhMucCapDeTaiDAL().GetAll("");
            //List<DanhMucCapDeTaiModel> listCap = new List<DanhMucCapDeTaiModel>();
            //foreach (var item in dmCap)
            //{
            //    if (item.Id == CapQuanLy) listCap.Add(item);
            //}

            //for (int i = 0; i < listCap.Count; i++)
            //{
            //    foreach (var dm in dmCap)
            //    {
            //        if (!listCap.Select(x => x.Id).ToList().Contains(dm.Id))
            //        {
            //            listCap.Add(dm);
            //        }
            //    }
            //}
            var dmCap = new DanhMucCapDeTaiDAL().GetAll("");
            List<DanhMucCapDeTaiModel> listCap = new List<DanhMucCapDeTaiModel>();
            if (CapQuanLy != null && CapQuanLy > 0)
            {
                foreach (var item in dmCap)
                {
                    if (item.Id == CapQuanLy) listCap.Add(item);
                }

                for (int i = 0; i < listCap.Count; i++)
                {
                    foreach (var dm in dmCap)
                    {
                        if (dm.ParentId == listCap[i].Id)
                        {
                            if (!listCap.Select(x => x.Id).ToList().Contains(dm.Id))
                            {
                                listCap.Add(dm);
                            }
                        }
                    }
                }
            }

            var listCapID = listCap.Select(x => x.Id).Distinct().ToList();
            var pListCap = new SqlParameter("@ListCapID", SqlDbType.Structured);
            pListCap.TypeName = "dbo.list_ID";
            var tbCapID = new DataTable();
            tbCapID.Columns.Add("CapID", typeof(string));
            listCapID.ForEach(x => tbCapID.Rows.Add(x));

            SqlParameter[] parameters = new SqlParameter[]
            {
                pListCap,
                new SqlParameter("@NamBatDau",SqlDbType.Int)
            };
            parameters[0].Value = tbCapID;
            parameters[1].Value = NamBatDau ?? 0;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_QuanLyThongBao_GetDoiTuongThongBao_New", parameters))
                {
                    while (dr.Read())
                    {
                        DoiTuongThongBaoModel info = new DoiTuongThongBaoModel();
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanChuNhiemID"], 0);
                        info.CanBoID = Utils.ConvertToInt32(dr["ChuNhiemDeTaiID"], 0);
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

        public List<DoiTuongDeTaiModel> GetDS_DoiTuongThongBaoTheoCap(int? CapQuanLy, int? NamBatDau)
        {
            var dmCap = new DanhMucCapDeTaiDAL().GetAll("");
            List<DanhMucCapDeTaiModel> listCap = new List<DanhMucCapDeTaiModel>();
            if (CapQuanLy != null && CapQuanLy > 0)
            {
                foreach (var item in dmCap)
                {
                    if (item.Id == CapQuanLy) listCap.Add(item);
                }

                for (int i = 0; i < listCap.Count; i++)
                {
                    foreach (var dm in dmCap)
                    {
                        if (dm.ParentId == listCap[i].Id)
                        {
                            if (!listCap.Select(x => x.Id).ToList().Contains(dm.Id))
                            {
                                listCap.Add(dm);
                            }
                        }
                    }
                }
            }


            var listCapID = listCap.Select(x => x.Id).Distinct().ToList();
            var pListCap = new SqlParameter("@ListCapID", SqlDbType.Structured);
            pListCap.TypeName = "dbo.list_ID";
            var tbCapID = new DataTable();
            tbCapID.Columns.Add("CapID", typeof(string));
            listCapID.ForEach(x => tbCapID.Rows.Add(x));

            List<DoiTuongDeTaiModel> Result = new List<DoiTuongDeTaiModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                pListCap,
                new SqlParameter("@NamBatDau",SqlDbType.Int)
            };
            parameters[0].Value = tbCapID;
            parameters[1].Value = NamBatDau ?? 0;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_QuanLyThongBao_GetDoiTuongThongBao_New", parameters))
                {
                    while (dr.Read())
                    {
                        DoiTuongDeTaiModel info = new DoiTuongDeTaiModel();
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanChuNhiemID"], 0);
                        info.CanBoID = Utils.ConvertToInt32(dr["ChuNhiemDeTaiID"], 0);
                        info.CapQuanLy = Utils.ConvertToInt32(dr["NhiemVu"], 0);
                        if (!Result.Select(x => x.CanBoID).ToList().Contains(info.CanBoID))
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
