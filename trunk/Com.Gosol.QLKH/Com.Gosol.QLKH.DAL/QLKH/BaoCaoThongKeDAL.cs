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

    public interface IBaoCaoThongKeDAL
    {
        public List<BaoCaoThongKeModel> BCThongKeNhiemVuKhoaHoc(BaoCaoThongKeParams p, List<int> listCanBoID, List<int> listCanBoNuID);
        public List<BCDanhSachNhiemVuKHModel> BCDanhSachNhiemVuKhoaHoc(BaoCaoThongKeParams p, List<int> listCanBoID);
        public List<BCTinhHinhKetQuaModel> BCTinhHinhKetQuaThucHien(BaoCaoThongKeParams p, List<int> listCanBoID);
        public List<BCThongKeHoatDongNghienCuuModel> BCThongKeHoatDongNghienCuu(BaoCaoThongKeParams p, List<int> listCanBoID);
        public List<BCThongKeKetQuaNghienCuuModel> BCThongKeKetQuaNghienCuu(BaoCaoThongKeParams p, List<int> listCanBoID);
        public BaseResultModel CongBoKetQuaNghienCuu(List<BCThongKeKetQuaNghienCuuModel> KetQuaNghienCuuModel);
        public List<BCKeKhaiBaiBaoKhoaHocModel> BCKeKhaiBaiBaoKhoaHoc(BaoCaoThongKeParams p);
    }
    public class BaoCaoThongKeDAL : IBaoCaoThongKeDAL
    {
        /// <summary>
        /// Báo cáo thống kê nhiệm vụ khoa học và công nghệ
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public List<BaoCaoThongKeModel> BCThongKeNhiemVuKhoaHoc(BaoCaoThongKeParams p, List<int> listCanBoID, List<int> listCanBoNuID)
        {
            List<BCThongKeNhiemVuKHModel> Result = new List<BCThongKeNhiemVuKHModel>();
            var pListCanBoNu = new SqlParameter("@ListCanBoNuID", SqlDbType.Structured);
            pListCanBoNu.TypeName = "dbo.list_ID";
            var tbCanBoNuID = new DataTable();
            tbCanBoNuID.Columns.Add("CapID", typeof(string));
            listCanBoNuID.ForEach(x => tbCanBoNuID.Rows.Add(x));

            var pList = new SqlParameter("@ListCanBoID", SqlDbType.Structured);
            pList.TypeName = "dbo.list_ID";
            var tbCanBoID = new DataTable();
            tbCanBoID.Columns.Add("CanBoID", typeof(string));
            listCanBoID.ForEach(x => tbCanBoID.Rows.Add(x));

            SqlParameter[] parameters = new SqlParameter[]
            {
                 new SqlParameter("@NamBatDau",SqlDbType.Int),
                 new SqlParameter("@NamKetThuc",SqlDbType.Int),
                 new SqlParameter("@TenNhiemVu",SqlDbType.NVarChar),
                 new SqlParameter("@MaNhiemVu",SqlDbType.NVarChar),
                 new SqlParameter("@LoaiHinhNghienCuu",SqlDbType.Int),
                 new SqlParameter("@LinhVucNghienCuu",SqlDbType.Int),
                 new SqlParameter("@LinhVucKinhTeXaHoi",SqlDbType.Int),
                 new SqlParameter("@CapQuanLy",SqlDbType.Int),
                 pList,
                 pListCanBoNu,
            };
            parameters[0].Value = p.NamBatDau ?? DateTime.Now.Year;
            parameters[1].Value = p.NamKetThuc ?? DateTime.Now.Year;
            parameters[2].Value = p.TenNhiemVu ?? "";
            parameters[3].Value = p.MaNhiemVu ?? "";
            parameters[4].Value = p.LoaiHinhNghienCuu ?? Convert.DBNull;
            parameters[5].Value = p.LinhVucNghienCuu ?? Convert.DBNull;
            parameters[6].Value = p.LinhVucKinhTeXaHoi ?? Convert.DBNull;
            parameters[7].Value = p.CapQuanLy ?? Convert.DBNull;
            parameters[8].Value = tbCanBoID;
            parameters[9].Value = tbCanBoNuID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_BCThongKeNhiemVuKH", parameters))
                {
                    while (dr.Read())
                    {
                        BCThongKeNhiemVuKHModel info = new BCThongKeNhiemVuKHModel();
                        info.ID = Utils.ConvertToInt32(dr["ID"], 0);
                        info.NghiepVuID = Utils.ConvertToInt32(dr["NghiepVuID"], 0);
                        info.HangMuc = Utils.ConvertToString(dr["HangMuc"], string.Empty);
                        info.MaSo = Utils.ConvertToString(dr["MaSo"], string.Empty);
                        info.TongSo = Utils.ConvertToInt32(dr["TongSo"], 0);
                        info.SoChuNhiemLaNu = Utils.ConvertToInt32(dr["SoChuNhiemLaNu"], 0);
                        info.SoPheDuyet = Utils.ConvertToInt32(dr["SoPheDuyet"], 0);
                        info.SoChuyenTiep = Utils.ConvertToInt32(dr["SoChuyenTiep"], 0);
                        info.SoDuocNghiemThu = Utils.ConvertToInt32(dr["SoDuocNghiemThu"], 0);
                        info.SoDaUngDung = Utils.ConvertToInt32(dr["SoDaUngDung"], 0);
                        Result.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            List<BaoCaoThongKeModel> data = new List<BaoCaoThongKeModel>();
            BaoCaoThongKeModel tongNhiemVu = new BaoCaoThongKeModel();
            tongNhiemVu.ID = 1;
            tongNhiemVu.HangMuc = "Tổng số nhiệm vụ KH&CN";
            tongNhiemVu.NhiemVuKhoaHoc = new List<BCThongKeNhiemVuKHModel>();
            BaoCaoThongKeModel capQuanLy = new BaoCaoThongKeModel();
            capQuanLy.ID = 2;
            capQuanLy.HangMuc = "Chia theo cấp quản lý";
            capQuanLy.NhiemVuKhoaHoc = new List<BCThongKeNhiemVuKHModel>();
            BaoCaoThongKeModel linhVucNghienCuu = new BaoCaoThongKeModel();
            linhVucNghienCuu.ID = 3;
            linhVucNghienCuu.HangMuc = "Chia theo lĩnh vực nghiên cứu";
            linhVucNghienCuu.NhiemVuKhoaHoc = new List<BCThongKeNhiemVuKHModel>();
            BaoCaoThongKeModel kinhTeXaHoi = new BaoCaoThongKeModel();
            kinhTeXaHoi.ID = 4;
            kinhTeXaHoi.HangMuc = "Chia theo mục tiêu kinh tế - xã hội";
            kinhTeXaHoi.NhiemVuKhoaHoc = new List<BCThongKeNhiemVuKHModel>();
            foreach (var item in Result)
            {
                if(item.ID == 1)
                {
                    tongNhiemVu.NhiemVuKhoaHoc.Add(item);
                    if (item.HangMuc == "SoDeTai") item.HangMuc = "Số đề tài/ đề án KHCN";
                    if (item.HangMuc == "SoDuAn") item.HangMuc = "Số dự án KH&CN";
                }
                else if (item.ID == 2)
                {
                    //capQuanLy.NhiemVuKhoaHoc.Add(item);
                }
                else if (item.ID == 3)
                {
                    //linhVucNghienCuu.NhiemVuKhoaHoc.Add(item);
                }
                else if (item.ID == 4)
                {
                    //kinhTeXaHoi.NhiemVuKhoaHoc.Add(item);
                }
            }
            data.Add(tongNhiemVu);

            //số liệu theo cấp 
            var dmCap = new DanhMucCapDeTaiDAL().GetAll("");
            List<BCThongKeNhiemVuKHModel> listSoLieuTheoCap = Result.Where(x => x.ID == 2).ToList();
            if(listSoLieuTheoCap != null && listSoLieuTheoCap.Count > 0)
            {
                List<DanhMucCapDeTaiModel> listCapID = new List<DanhMucCapDeTaiModel>();
                foreach (var item in dmCap)
                {
                    if (item.ParentId == null || item.ParentId == 0) listCapID.Add(item);
                }
                foreach (var item in listCapID)
                {
                    List<DanhMucCapDeTaiModel> listCap = new List<DanhMucCapDeTaiModel>();
                    listCap.Add(item);
                    for (int i = 0; i < listCap.Count; i++)
                    {
                        foreach (var dm in dmCap)
                        {
                            if (dm.ParentId == listCap[i].Id)
                            {
                                listCap.Add(dm);
                            }
                        }
                    }

                    List<BCThongKeNhiemVuKHModel> listSL = new List<BCThongKeNhiemVuKHModel>();
                    foreach (var sl in listSoLieuTheoCap)
                    {
                        foreach (var dm in listCap)
                        {
                            if (sl.NghiepVuID == dm.Id)
                            {
                                listSL.Add(sl);
                            }
                        }
                    }
                    if (listSL.Count > 0)
                    {
                        BCThongKeNhiemVuKHModel ThongKeModel = new BCThongKeNhiemVuKHModel();
                        ThongKeModel.HangMuc = item.Name;
                        ThongKeModel.TongSo = listSL.Sum(x => x.TongSo);
                        ThongKeModel.SoChuNhiemLaNu = listSL.Sum(x => x.SoChuNhiemLaNu);
                        ThongKeModel.SoPheDuyet = listSL.Sum(x => x.SoPheDuyet);
                        ThongKeModel.SoChuyenTiep = listSL.Sum(x => x.SoChuyenTiep);
                        ThongKeModel.SoDuocNghiemThu = listSL.Sum(x => x.SoDuocNghiemThu);
                        ThongKeModel.SoDaUngDung = listSL.Sum(x => x.SoDaUngDung);
                        capQuanLy.NhiemVuKhoaHoc.Add(ThongKeModel);
                    }
                    else
                    {
                        BCThongKeNhiemVuKHModel ThongKeModel = new BCThongKeNhiemVuKHModel();
                        ThongKeModel.HangMuc = item.Name;
                        ThongKeModel.TongSo = 0;
                        ThongKeModel.SoChuNhiemLaNu = 0;
                        ThongKeModel.SoPheDuyet = 0;
                        ThongKeModel.SoChuyenTiep = 0;
                        ThongKeModel.SoDuocNghiemThu = 0;
                        ThongKeModel.SoDaUngDung = 0;
                        capQuanLy.NhiemVuKhoaHoc.Add(ThongKeModel);
                    }
                }
            }     

            data.Add(capQuanLy);

            //số liệu theo lĩnh vực
            var dmLinhVuc = new DanhMucLinhVucDAL().GetAll(1, "", true);
            List<BCThongKeNhiemVuKHModel> listSLTheoLinhVucNghienCuu = Result.Where(x => x.ID == 3).ToList();
            if (listSLTheoLinhVucNghienCuu != null && listSLTheoLinhVucNghienCuu.Count > 0)
            {
                List<DanhMucLinhVucModel> listLinhVucNghienCuu = new List<DanhMucLinhVucModel>();
                foreach (var item in dmLinhVuc)
                {
                    if (item.ParentId == null || item.ParentId == 0) listLinhVucNghienCuu.Add(item);
                }
                foreach (var item in listLinhVucNghienCuu)
                {
                    List<DanhMucLinhVucModel> listLV = new List<DanhMucLinhVucModel>();
                    listLV.Add(item);
                    for (int i = 0; i < listLV.Count; i++)
                    {
                        foreach (var dm in dmLinhVuc)
                        {
                            if (dm.ParentId == listLV[i].Id)
                            {
                                listLV.Add(dm);
                            }
                        }
                    }

                    List<BCThongKeNhiemVuKHModel> listSL = new List<BCThongKeNhiemVuKHModel>();
                    foreach (var sl in listSLTheoLinhVucNghienCuu)
                    {
                        foreach (var dm in listLV)
                        {
                            if (sl.NghiepVuID == dm.Id)
                            {
                                listSL.Add(sl);
                            }
                        }
                    }
                    if (listSL.Count > 0)
                    {
                        BCThongKeNhiemVuKHModel ThongKeModel = new BCThongKeNhiemVuKHModel();
                        ThongKeModel.HangMuc = item.Name;
                        ThongKeModel.TongSo = listSL.Sum(x => x.TongSo);
                        ThongKeModel.SoChuNhiemLaNu = listSL.Sum(x => x.SoChuNhiemLaNu);
                        ThongKeModel.SoPheDuyet = listSL.Sum(x => x.SoPheDuyet);
                        ThongKeModel.SoChuyenTiep = listSL.Sum(x => x.SoChuyenTiep);
                        ThongKeModel.SoDuocNghiemThu = listSL.Sum(x => x.SoDuocNghiemThu);
                        ThongKeModel.SoDaUngDung = listSL.Sum(x => x.SoDaUngDung);
                        linhVucNghienCuu.NhiemVuKhoaHoc.Add(ThongKeModel);
                    }
                    else
                    {
                        BCThongKeNhiemVuKHModel ThongKeModel = new BCThongKeNhiemVuKHModel();
                        ThongKeModel.HangMuc = item.Name;
                        ThongKeModel.TongSo = 0;
                        ThongKeModel.SoChuNhiemLaNu = 0;
                        ThongKeModel.SoPheDuyet = 0;
                        ThongKeModel.SoChuyenTiep = 0;
                        ThongKeModel.SoDuocNghiemThu = 0;
                        ThongKeModel.SoDaUngDung = 0;
                        linhVucNghienCuu.NhiemVuKhoaHoc.Add(ThongKeModel);
                    }
                }
            }       

            data.Add(linhVucNghienCuu);

            //Số liệu theo lĩnh vuc kinh tế - xã hội
            var dmLinhVucKTXH = new DanhMucLinhVucDAL().GetAll(2, "", true);
            List<BCThongKeNhiemVuKHModel> listSLTheoLinhVucKTXH = Result.Where(x => x.ID == 4).ToList();
            if(listSLTheoLinhVucKTXH != null && listSLTheoLinhVucKTXH.Count > 0)
            {
                List<DanhMucLinhVucModel> listLinhVucKTXH = new List<DanhMucLinhVucModel>();
                foreach (var item in dmLinhVucKTXH)
                {
                    if (item.ParentId == null || item.ParentId == 0) listLinhVucKTXH.Add(item);
                }
                foreach (var item in listLinhVucKTXH)
                {
                    List<DanhMucLinhVucModel> listLV = new List<DanhMucLinhVucModel>();
                    listLV.Add(item);
                    for (int i = 0; i < listLV.Count; i++)
                    {
                        foreach (var dm in dmLinhVucKTXH)
                        {
                            if (dm.ParentId == listLV[i].Id)
                            {
                                listLV.Add(dm);
                            }
                        }
                    }

                    List<BCThongKeNhiemVuKHModel> listSL = new List<BCThongKeNhiemVuKHModel>();
                    foreach (var sl in listSLTheoLinhVucKTXH)
                    {
                        foreach (var dm in listLV)
                        {
                            if (sl.NghiepVuID == dm.Id)
                            {
                                listSL.Add(sl);
                            }
                        }
                    }
                    if (listSL.Count > 0)
                    {
                        BCThongKeNhiemVuKHModel ThongKeModel = new BCThongKeNhiemVuKHModel();
                        ThongKeModel.HangMuc = item.Name;
                        ThongKeModel.TongSo = listSL.Sum(x => x.TongSo);
                        ThongKeModel.SoChuNhiemLaNu = listSL.Sum(x => x.SoChuNhiemLaNu);
                        ThongKeModel.SoPheDuyet = listSL.Sum(x => x.SoPheDuyet);
                        ThongKeModel.SoChuyenTiep = listSL.Sum(x => x.SoChuyenTiep);
                        ThongKeModel.SoDuocNghiemThu = listSL.Sum(x => x.SoDuocNghiemThu);
                        ThongKeModel.SoDaUngDung = listSL.Sum(x => x.SoDaUngDung);
                        kinhTeXaHoi.NhiemVuKhoaHoc.Add(ThongKeModel);
                    }
                    else
                    {
                        BCThongKeNhiemVuKHModel ThongKeModel = new BCThongKeNhiemVuKHModel();
                        ThongKeModel.HangMuc = item.Name;
                        ThongKeModel.TongSo = 0;
                        ThongKeModel.SoChuNhiemLaNu = 0;
                        ThongKeModel.SoPheDuyet = 0;
                        ThongKeModel.SoChuyenTiep = 0;
                        ThongKeModel.SoDuocNghiemThu = 0;
                        ThongKeModel.SoDaUngDung = 0;
                        kinhTeXaHoi.NhiemVuKhoaHoc.Add(ThongKeModel);
                    }
                }
            }
            
            data.Add(kinhTeXaHoi);

            return data;
        }

        /// <summary>
        /// Báo cáo danh sách nhiệm vụ khoa học và công nghệ
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public List<BCDanhSachNhiemVuKHModel> BCDanhSachNhiemVuKhoaHoc(BaoCaoThongKeParams p, List<int> listCanBoID)
        {
            List<BCDanhSachNhiemVuKHModel> Result = new List<BCDanhSachNhiemVuKHModel>();

            var pList = new SqlParameter("@ListCanBoID", SqlDbType.Structured);
            pList.TypeName = "dbo.list_ID";
            var tbCanBoID = new DataTable();
            tbCanBoID.Columns.Add("CanBoID", typeof(string));
            listCanBoID.ForEach(x => tbCanBoID.Rows.Add(x));

            SqlParameter[] parameters = new SqlParameter[]
            {
                 new SqlParameter("@NamBatDau",SqlDbType.Int),
                 new SqlParameter("@NamKetThuc",SqlDbType.Int),
                 new SqlParameter("@TenNhiemVu",SqlDbType.NVarChar),
                 new SqlParameter("@MaNhiemVu",SqlDbType.NVarChar),
                 new SqlParameter("@LoaiHinhNghienCuu",SqlDbType.Int),
                 new SqlParameter("@LinhVucNghienCuu",SqlDbType.Int),
                 new SqlParameter("@LinhVucKinhTeXaHoi",SqlDbType.Int),
                 new SqlParameter("@CapQuanLy",SqlDbType.Int),
                 pList,
            };
            parameters[0].Value = p.NamBatDau ?? DateTime.Now.Year;
            parameters[1].Value = p.NamKetThuc ?? DateTime.Now.Year;
            parameters[2].Value = p.TenNhiemVu ?? "";
            parameters[3].Value = p.MaNhiemVu ?? "";
            parameters[4].Value = p.LoaiHinhNghienCuu ?? Convert.DBNull;
            parameters[5].Value = p.LinhVucNghienCuu ?? Convert.DBNull;
            parameters[6].Value = p.LinhVucKinhTeXaHoi ?? Convert.DBNull;
            parameters[7].Value = p.CapQuanLy ?? Convert.DBNull;
            parameters[8].Value = tbCanBoID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_BCDanhSachNhiemVuKhoaHoc", parameters))
                {
                    while (dr.Read())
                    {
                        BCDanhSachNhiemVuKHModel info = new BCDanhSachNhiemVuKHModel();
                        info.NhiemVuID = Utils.ConvertToInt32(dr["NhiemVuID"], 0);
                        info.TenNhiemVu = Utils.ConvertToString(dr["TenNhiemVu"], string.Empty);
                        info.ChuNhiemDeTaiID = Utils.ConvertToInt32(dr["ChuNhiemDeTaiID"], 0);                    
                        info.MucTieu = Utils.ConvertToString(dr["MucTieu"], string.Empty);
                        info.KetQuaSanPham = Utils.ConvertToString(dr["SanPhamDangKy"], string.Empty);
                        string NamBatDau = Utils.ConvertToString(dr["NamBatDau"], string.Empty);
                        string NamKetThuc = Utils.ConvertToString(dr["NamKetThuc"], string.Empty);
                        info.ThoiGianThucHien = NamBatDau + " - " + NamKetThuc;
                        info.NSNN = Utils.ConvertToDecimal(dr["KinhPhiDHSP"], 0);
                        info.NguonKhac = Utils.ConvertToDecimal(dr["NguonKhac"], 0);
                        info.TongKinhPhi = info.NSNN + info.NguonKhac;
                        info.NSNNDaCap = Utils.ConvertToDecimal(dr["NSNNDaCap"], 0);
                        info.NguonKhacDaCap = Utils.ConvertToDecimal(dr["NguonKhacDaCap"], 0);
                        info.TongDaCap = info.NSNNDaCap + info.NguonKhacDaCap;
                        info.NSNN_NamHienTai = Utils.ConvertToDecimal(dr["NSNN_NamHienTai"], 0);
                        info.NguonKhac_NamHienTai = Utils.ConvertToDecimal(dr["NguonKhac_NamHienTai"], 0);
                        info.Tong_NamHienTai = info.NSNN_NamHienTai + info.NguonKhac_NamHienTai;
                        info.GhiChu = Utils.ConvertToString(dr["GhiChu"], string.Empty);
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

        /// <summary>
        /// Báo cáo tình hình và kết quả thực hiện nhiệm vụ khoa học và công nghệ
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public List<BCTinhHinhKetQuaModel> BCTinhHinhKetQuaThucHien(BaoCaoThongKeParams p, List<int> listCanBoID)
        {
            List<ChiTietTinhHinhKetQuaThucHienModel> Result = new List<ChiTietTinhHinhKetQuaThucHienModel>();

            var dmCap = new DanhMucCapDeTaiDAL().GetAll("");
            List<int> listCapID = new List<int>();
            List<int> listCapNhaNuocID = new List<int>();
            List<int> listCapNganhID = new List<int>();

            var capNhaNuoc = new SystemConfigDAL().GetByKey("CAP_NHA_NUOC").ConfigValue;
            if (capNhaNuoc != null && capNhaNuoc.Length > 0)
            {
                var cap = capNhaNuoc.Split(',');
                foreach (var item in cap)
                {
                    int id = Utils.ConvertToInt32(item, 0);
                    if (id > 0) listCapNhaNuocID.Add(id);
                }
            }
            var capBoNganh = new SystemConfigDAL().GetByKey("CAP_BO_NGANH").ConfigValue;
            if (capBoNganh != null && capBoNganh.Length > 0)
            {
                var cap = capBoNganh.Split(',');
                foreach (var item in cap)
                {
                    int id = Utils.ConvertToInt32(item, 0);
                    if (id > 0) listCapNganhID.Add(id);
                }
            }
            //listCapNhaNuocID.Add(capNhaNuoc);
            //listCapNganhID.Add(capBoNganh);

            for (int i = 0; i < listCapNhaNuocID.Count; i++)
            {
                foreach (var dm in dmCap)
                {
                    if (dm.ParentId == listCapNhaNuocID[i])
                    {
                        listCapNhaNuocID.Add(dm.Id);
                    }
                }
            }

            for (int i = 0; i < listCapNganhID.Count; i++)
            {
                foreach (var dm in dmCap)
                {
                    if (dm.ParentId == listCapNganhID[i])
                    {
                        listCapNganhID.Add(dm.Id);
                    }
                }
            }
            if(listCapNhaNuocID.Count > 0) listCapID.AddRange(listCapNhaNuocID);
            if (listCapNganhID.Count > 0) listCapID.AddRange(listCapNganhID);
            listCapID = listCapID.Distinct().ToList();
            var pListCap = new SqlParameter("@ListCapID", SqlDbType.Structured);
            pListCap.TypeName = "dbo.list_ID";
            var tbCapID = new DataTable();
            tbCapID.Columns.Add("CapID", typeof(string));
            listCapID.ForEach(x => tbCapID.Rows.Add(x));

            var pList = new SqlParameter("@ListCanBoID", SqlDbType.Structured);
            pList.TypeName = "dbo.list_ID";
            var tbCanBoID = new DataTable();
            tbCanBoID.Columns.Add("CanBoID", typeof(string));
            listCanBoID.ForEach(x => tbCanBoID.Rows.Add(x));

            SqlParameter[] parameters = new SqlParameter[]
            {
                 new SqlParameter("@NamBatDau",SqlDbType.Int),
                 new SqlParameter("@NamKetThuc",SqlDbType.Int),
                 new SqlParameter("@TenNhiemVu",SqlDbType.NVarChar),
                 new SqlParameter("@MaNhiemVu",SqlDbType.NVarChar),
                 new SqlParameter("@LoaiHinhNghienCuu",SqlDbType.Int),
                 new SqlParameter("@LinhVucNghienCuu",SqlDbType.Int),
                 new SqlParameter("@LinhVucKinhTeXaHoi",SqlDbType.Int),
                 new SqlParameter("@CapQuanLy",SqlDbType.Int),
                 pList,
                 pListCap,
            };
            parameters[0].Value = p.NamBatDau ?? DateTime.Now.Year;
            parameters[1].Value = p.NamKetThuc ?? DateTime.Now.Year;
            parameters[2].Value = p.TenNhiemVu ?? "";
            parameters[3].Value = p.MaNhiemVu ?? "";
            parameters[4].Value = p.LoaiHinhNghienCuu ?? Convert.DBNull;
            parameters[5].Value = p.LinhVucNghienCuu ?? Convert.DBNull;
            parameters[6].Value = p.LinhVucKinhTeXaHoi ?? Convert.DBNull;
            parameters[7].Value = p.CapQuanLy ?? Convert.DBNull;
            parameters[8].Value = tbCanBoID;
            parameters[9].Value = tbCapID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_BCTinhHinhKetQuaThucHien", parameters))
                {
                    while (dr.Read())
                    {
                        ChiTietTinhHinhKetQuaThucHienModel info = new ChiTietTinhHinhKetQuaThucHienModel();
                        info.CapQuanLy = Utils.ConvertToInt32(dr["CapQuanLy"], 0);
                        info.ChuNhiemDeTaiID = Utils.ConvertToInt32(dr["ChuNhiemDeTaiID"], 0);
                        info.TenCapQuanLy = Utils.ConvertToString(dr["Name"], string.Empty);
                        info.NhiemVuID = Utils.ConvertToInt32(dr["NhiemVuID"], 0);
                        info.TenNhiemVu = Utils.ConvertToString(dr["TenNhiemVu"], string.Empty);
                        info.DonViChuTri = Utils.ConvertToString(dr["DonViChuTri"], string.Empty);
                        string NamBatDau = Utils.ConvertToString(dr["NamBatDau"], string.Empty);
                        string NamKetThuc = Utils.ConvertToString(dr["NamKetThuc"], string.Empty);
                        info.ThoiGianThucHien = NamBatDau + " - " + NamKetThuc;
                        info.KinhPhiNSNN = Utils.ConvertToDecimal(dr["KinhPhiDHSP"], 0);
                        info.KinhPhiNSNN = info.KinhPhiNSNN / 1000000;
                        info.NguonKhac = Utils.ConvertToDecimal(dr["NguonKhac"], 0);
                        info.NguonKhac = info.NguonKhac / 1000000;
                        info.SoDaCap = Utils.ConvertToDecimal(dr["SoDaCap"], 0);
                        info.KetQuaDatDuoc = Utils.ConvertToString(dr["KetQuaDatDuoc"], string.Empty);
                        info.GhiChu = Utils.ConvertToString(dr["GhiChu"], string.Empty);
                        Result.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            List<BCTinhHinhKetQuaModel> BCTinhHinhKetQua = new List<BCTinhHinhKetQuaModel>();
            BCTinhHinhKetQuaModel nhaNuoc = new BCTinhHinhKetQuaModel();
            int capNhaNuocID = listCapNhaNuocID[0];
            nhaNuoc.CapQuanLy = capNhaNuocID;
            //nhaNuoc.TenCapQuanLy = "Cấp nhà nước";
            var nn = dmCap.FirstOrDefault(x => x.Id == capNhaNuocID);
            if (nn != null)
            {
                nhaNuoc.TenCapQuanLy = nn.Name;
            }
            else nhaNuoc.TenCapQuanLy = "";
            nhaNuoc.NhiemVuKhoaHoc = new List<ChiTietTinhHinhKetQuaThucHienModel>();
            foreach (var item in Result)
            {
                foreach (var id in listCapNhaNuocID)
                {
                    if(item.CapQuanLy == id)
                    {
                        nhaNuoc.NhiemVuKhoaHoc.Add(item);
                    }
                }
            }
            BCTinhHinhKetQua.Add(nhaNuoc);

            BCTinhHinhKetQuaModel boNganh = new BCTinhHinhKetQuaModel();
            int capBoNganhID = listCapNganhID[0];
            boNganh.CapQuanLy = capBoNganhID;
            //boNganh.TenCapQuanLy = "Cấp bộ ngành";
            var bn = dmCap.FirstOrDefault(x => x.Id == capBoNganhID);
            if (bn != null)
            {
                boNganh.TenCapQuanLy = bn.Name;
            }
            else boNganh.TenCapQuanLy = "";
            boNganh.NhiemVuKhoaHoc = new List<ChiTietTinhHinhKetQuaThucHienModel>();
            foreach (var item in Result)
            {
                foreach (var id in listCapNganhID)
                {
                    if (item.CapQuanLy == id)
                    {
                        boNganh.NhiemVuKhoaHoc.Add(item);
                    }
                }
            }
            BCTinhHinhKetQua.Add(boNganh);

            return BCTinhHinhKetQua;
        }

        /// <summary>
        /// Báo cáo thống kê các hoạt động nghiên cứu khoa học, chuyển giao công nghệ
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public List<BCThongKeHoatDongNghienCuuModel> BCThongKeHoatDongNghienCuu(BaoCaoThongKeParams p, List<int> listCanBoID)
        {
            List<BCThongKeHoatDongNghienCuuModel> Result = new List<BCThongKeHoatDongNghienCuuModel>();

            var pList = new SqlParameter("@ListCanBoID", SqlDbType.Structured);
            pList.TypeName = "dbo.list_ID";
            var tbCanBoID = new DataTable();
            tbCanBoID.Columns.Add("CanBoID", typeof(string));
            listCanBoID.ForEach(x => tbCanBoID.Rows.Add(x));

            SqlParameter[] parameters = new SqlParameter[]
            {
                 new SqlParameter("@NamBatDau",SqlDbType.Int),
                 new SqlParameter("@NamKetThuc",SqlDbType.Int),
                 new SqlParameter("@TenNhiemVu",SqlDbType.NVarChar),
                 new SqlParameter("@MaNhiemVu",SqlDbType.NVarChar),
                 new SqlParameter("@LoaiHinhNghienCuu",SqlDbType.Int),
                 new SqlParameter("@LinhVucNghienCuu",SqlDbType.Int),
                 new SqlParameter("@LinhVucKinhTeXaHoi",SqlDbType.Int),
                 new SqlParameter("@CapQuanLy",SqlDbType.Int),
                 pList,
                 //pListBaiBao,
                 //pListSach,
                 //pListDaoTao,
                 //pListSanPham,
            };
            parameters[0].Value = p.NamBatDau ?? DateTime.Now.Year;
            parameters[1].Value = p.NamKetThuc ?? DateTime.Now.Year;
            parameters[2].Value = p.TenNhiemVu ?? "";
            parameters[3].Value = p.MaNhiemVu ?? "";
            parameters[4].Value = p.LoaiHinhNghienCuu ?? Convert.DBNull;
            parameters[5].Value = p.LinhVucNghienCuu ?? Convert.DBNull;
            parameters[6].Value = p.LinhVucKinhTeXaHoi ?? Convert.DBNull;
            parameters[7].Value = p.CapQuanLy ?? Convert.DBNull;
            parameters[8].Value = tbCanBoID;
            //parameters[9].Value = tbBaiBao;
            //parameters[10].Value = tbSach;
            //parameters[11].Value = tbDaoTao;
            //parameters[12].Value = tbSanPham;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_BCThongKeHoatDongNghienCuu", parameters))
                {
                    while (dr.Read())
                    {
                        BCThongKeHoatDongNghienCuuModel info = new BCThongKeHoatDongNghienCuuModel();
                        info.MaDeTai = Utils.ConvertToString(dr["MaDeTai"], string.Empty);
                        info.TenNhiemVu = Utils.ConvertToString(dr["TenDeTai"], string.Empty);
                        info.NguoiChuTriIDStr = Utils.ConvertToString(dr["NguoiChuTriIDStr"], string.Empty);   
                        info.DoiTac = Utils.ConvertToString(dr["DoiTac"], string.Empty);
                        string NamBatDau = Utils.ConvertToString(dr["NamBatDau"], string.Empty);
                        string NamKetThuc = Utils.ConvertToString(dr["NamKetThuc"], string.Empty);
                        info.ThoiGianThucHien = NamBatDau + " - " + NamKetThuc;
                        int TrangThai = Utils.ConvertToInt32(dr["TrangThai"], 0);
                        if(TrangThai == (int)EnumTrangThaiDeTai.ChuaThucHien)
                        {
                            info.TrangThaiThucHien = "Chưa thực hiện";
                        }
                        else if (TrangThai == (int)EnumTrangThaiDeTai.DangThucHien)
                        {
                            info.TrangThaiThucHien = "Đang thực hiện";
                        }
                        else if (TrangThai == (int)EnumTrangThaiDeTai.NghiemThu)
                        {
                            info.TrangThaiThucHien = "Nghiệm thu";
                        }
                        else if (TrangThai == (int)EnumTrangThaiDeTai.ThanhLy)
                        {
                            info.TrangThaiThucHien = "Thanh lý";
                        }
                        info.KinhPhiThucHien = Utils.ConvertToDecimal(dr["KinhPhiThucHien"], 0);
                        info.KinhPhiThucHien = info.KinhPhiThucHien / 1000000;
                        info.BaiBao = Utils.ConvertToString(dr["BaiBao"], string.Empty);
                        info.SachChuyenKhao = Utils.ConvertToString(dr["SachChuyenKhao"], string.Empty);
                        info.DaoTao = Utils.ConvertToString(dr["DaoTao"], string.Empty);
                        info.SanPhamKhac = Utils.ConvertToString(dr["SanPhamKhac"], string.Empty);
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

        /// <summary>
        /// Báo cáo thông kê kết quả nghiên cứu
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public List<BCThongKeKetQuaNghienCuuModel> BCThongKeKetQuaNghienCuu(BaoCaoThongKeParams p, List<int> listCanBoID)
        {
            List<BCThongKeKetQuaNghienCuuModel> Result = new List<BCThongKeKetQuaNghienCuuModel>();

            var pList = new SqlParameter("@ListCanBoID", SqlDbType.Structured);
            pList.TypeName = "dbo.list_ID";
            var tbCanBoID = new DataTable();
            tbCanBoID.Columns.Add("CanBoID", typeof(string));
            listCanBoID.ForEach(x => tbCanBoID.Rows.Add(x));

            SqlParameter[] parameters = new SqlParameter[]
            {
                 new SqlParameter("@NamBatDau",SqlDbType.Int),
                 new SqlParameter("@NamKetThuc",SqlDbType.Int),
                 new SqlParameter("@TenNhiemVu",SqlDbType.NVarChar),
                 new SqlParameter("@MaNhiemVu",SqlDbType.NVarChar),
                 new SqlParameter("@LoaiHinhNghienCuu",SqlDbType.Int),
                 new SqlParameter("@LinhVucNghienCuu",SqlDbType.Int),
                 new SqlParameter("@LinhVucKinhTeXaHoi",SqlDbType.Int),
                 new SqlParameter("@CapQuanLy",SqlDbType.Int),
                 pList,
                 //new SqlParameter("@CanBoID",SqlDbType.Int),
            };
            parameters[0].Value = p.NamBatDau ?? DateTime.Now.Year;
            parameters[1].Value = p.NamKetThuc ?? DateTime.Now.Year;
            parameters[2].Value = p.TenNhiemVu ?? "";
            parameters[3].Value = p.MaNhiemVu ?? "";
            parameters[4].Value = p.LoaiHinhNghienCuu ?? Convert.DBNull;
            parameters[5].Value = p.LinhVucNghienCuu ?? Convert.DBNull;
            parameters[6].Value = p.LinhVucKinhTeXaHoi ?? Convert.DBNull;
            parameters[7].Value = p.CapQuanLy ?? Convert.DBNull;
            parameters[8].Value = tbCanBoID;
            //parameters[8].Value = p.CanBoID ?? Convert.DBNull;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_BCThongKeKetQuaNghienCuu", parameters))
                {
                    while (dr.Read())
                    {
                        BCThongKeKetQuaNghienCuuModel info = new BCThongKeKetQuaNghienCuuModel();
                        info.KetQuaNghienCuuID = Utils.ConvertToInt32(dr["KetQuaNghienCuuID"], 0);
                        info.CTNhaKhoaHocID = Utils.ConvertToInt32(dr["CTNhaKhoaHocID"], 0);
                        info.TacGia = Utils.ConvertToString(dr["TacGia"], string.Empty);
                        info.TenCongTrinhKhoaHoc = Utils.ConvertToString(dr["TenDeTai"], string.Empty);
                        info.TenTapChiSachHoiThao = Utils.ConvertToString(dr["TenTapChiSachHoiThao"], string.Empty);
                        info.So = Utils.ConvertToString(dr["So"], string.Empty);
                        info.Trang = Utils.ConvertToString(dr["Trang"], string.Empty);
                        info.NhaXuatBan = Utils.ConvertToString(dr["NhaXuatBan"], string.Empty);
                        info.ThoiGian = Utils.ConvertToString(dr["ThoiGian"], string.Empty);  
                        info.CongBo = Utils.ConvertToBoolean(dr["CongBo"], false);  
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

        /// <summary>
        /// Báo cáo kê khai bài báo khoa học
        /// </summary>
        /// <param name="p"></param>
        /// <param name="listCanBoID"></param>
        /// <returns></returns>
        public List<BCKeKhaiBaiBaoKhoaHocModel> BCKeKhaiBaiBaoKhoaHoc(BaoCaoThongKeParams p)
        {
            List<BCKeKhaiBaiBaoKhoaHocModel> Result = new List<BCKeKhaiBaiBaoKhoaHocModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                 new SqlParameter("@NamBatDau",SqlDbType.Int),
                 new SqlParameter("@NamKetThuc",SqlDbType.Int),
            };
            parameters[0].Value = p.NamBatDau ?? DateTime.Now.Year;   
            parameters[1].Value = p.NamKetThuc ?? DateTime.Now.Year;   

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_BCKeKhaiBaiBaoKhoaHoc", parameters))
                {
                    while (dr.Read())
                    {
                        BCKeKhaiBaiBaoKhoaHocModel info = new BCKeKhaiBaiBaoKhoaHocModel();
                        info.LoaiBaiBao = Utils.ConvertToInt32(dr["LoaiBaiBao"], 0);                    
                        info.TenBaiBao = Utils.ConvertToString(dr["TenBaiBao"], string.Empty);
                        info.MaDeTai = Utils.ConvertToString(dr["MaDeTai"], string.Empty);
                        info.TenCacTacGia = Utils.ConvertToString(dr["TenCacTacGia"], string.Empty);
                        info.TenTapChiHoiThao = Utils.ConvertToString(dr["TenTapChiHoiThao"], string.Empty);
                        info.HeSoAnhHuong = Utils.ConvertToString(dr["HeSoAnhHuong"], string.Empty);
                        info.ISI_SCOPUS = Utils.ConvertToString(dr["ISI_SCOPUS"], string.Empty);
                        if(info.ISI_SCOPUS == EnumChiSo.ISI.GetHashCode().ToString())
                        {
                            info.ISI_SCOPUS = "ISI";
                        }
                        else if (info.ISI_SCOPUS == EnumChiSo.SCOPUS.GetHashCode().ToString())
                        {
                            info.ISI_SCOPUS = "SCOPUS";
                        }
                        else info.ISI_SCOPUS = "";
                        info.Rank_SCIMAGO = Utils.ConvertToString(dr["Rank_SCIMAGO"], string.Empty);
                        if (info.Rank_SCIMAGO == EnumRank.Q1.GetHashCode().ToString())
                        {
                            info.Rank_SCIMAGO = "Q1";
                        }
                        else if (info.Rank_SCIMAGO == EnumRank.Q2.GetHashCode().ToString())
                        {
                            info.Rank_SCIMAGO = "Q2";
                        }
                        else if (info.Rank_SCIMAGO == EnumRank.Q3.GetHashCode().ToString())
                        {
                            info.Rank_SCIMAGO = "Q3";
                        }
                        else if (info.Rank_SCIMAGO == EnumRank.Q4.GetHashCode().ToString())
                        {
                            info.Rank_SCIMAGO = "Q4";
                        }
                        else info.Rank_SCIMAGO = "";
                        info.DiemTapChi = Utils.ConvertToString(dr["DiemTapChi"], string.Empty);
                        info.CapHoiThao = Utils.ConvertToInt32(dr["CapHoiThao"], 0);
                        if (info.CapHoiThao == EnumCapHoiThao.QuocTe.GetHashCode())
                        {
                            info.LoaiHoiThao = "Quốc tế";
                        }
                        else if (info.CapHoiThao == EnumCapHoiThao.QuocGia.GetHashCode())
                        {
                            info.LoaiHoiThao = "Quốc gia";
                        }
                        else if (info.CapHoiThao == EnumCapHoiThao.TrongNuoc.GetHashCode())
                        {
                            info.LoaiHoiThao = "Trường";
                        }

                        info.ISSN = Utils.ConvertToString(dr["ISSN"], string.Empty);
                        info.Tap = Utils.ConvertToString(dr["Tap"], string.Empty);
                        info.So = Utils.ConvertToString(dr["So"], string.Empty);
                        info.NamDangTai = Utils.ConvertToString(dr["NamDangTai"], string.Empty);
                        info.TuTrangDenTrang = Utils.ConvertToString(dr["TuTrangDenTrang"], string.Empty);
                        info.LinkBaiBao = Utils.ConvertToString(dr["LinkBaiBao"], string.Empty);
                        info.LinhVucNganhKhoaHoc = Utils.ConvertToString(dr["LinhVucNganhKhoaHoc"], string.Empty);
                      
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

        /// <summary>
        /// Công bố kết quả nghiên cứu
        /// </summary>
        /// <param name="KetQuaNghienCuuModel"></param>
        /// <returns></returns>
        public BaseResultModel CongBoKetQuaNghienCuu(List<BCThongKeKetQuaNghienCuuModel> KetQuaNghienCuuModel)
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
                            foreach (var item in KetQuaNghienCuuModel)
                            {
                                if(item.KetQuaNghienCuuID > 0)
                                {
                                    SqlParameter[] parameters = new SqlParameter[]
                                    {
                                        new SqlParameter("KetQuaNghienCuuID", SqlDbType.Int),
                                        new SqlParameter("CongBo", SqlDbType.Bit),
                                    };
                                    parameters[0].Value = item.KetQuaNghienCuuID;
                                    parameters[1].Value = item.CongBo;
                                    SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v1_CongBoKetQuaNghienCuu", parameters);
                                }
                                else if (item.CTNhaKhoaHocID > 0)
                                {
                                    SqlParameter[] parameters = new SqlParameter[]
                                    {
                                        new SqlParameter("CTNhaKhoaHocID", SqlDbType.Int),
                                        new SqlParameter("CongBo", SqlDbType.Bit),
                                    };
                                    parameters[0].Value = item.CTNhaKhoaHocID;
                                    parameters[1].Value = item.CongBo;
                                    SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v1_CongBoThongTinNhaKhoaHoc", parameters);
                                }
                            }
                            trans.Commit();
                            Result.Status = 1;
                            Result.Message = ConstantLogMessage.Alert_Update_Success("kết quả");
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
    }
}
