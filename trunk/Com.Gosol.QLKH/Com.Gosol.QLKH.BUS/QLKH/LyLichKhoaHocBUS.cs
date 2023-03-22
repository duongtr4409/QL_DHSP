using Com.Gosol.QLKH.DAL.QLKH;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.DanhMuc;
using Com.Gosol.QLKH.Models.QLKH;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Gosol.QLKH.BUS.QLKH
{
    public interface ILyLichKhoaHocBUS
    {
        public List<NhaKhoaHocModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow, int QuyenQuanLy, string Alphabet);
        public NhaKhoaHocModel GetByID(int CanBoID, string serverPath, int? CoQuanID);
        public BaseResultModel Insert(NhaKhoaHocModel NhaKhoaHocModel);
        public BaseResultModel Update(NhaKhoaHocModel NhaKhoaHocModel);
        public BaseResultModel Delete(NhaKhoaHocModel NhaKhoaHocModel);
        public BaseResultModel Delete_ThongTinChiTiet(ThongTinCTNhaKhoaHocModel ThongTinChiTiet);
        public BaseResultModel Edit_ThongTinChiTiet(ThongTinCTNhaKhoaHocModel ThongTinChiTiet);
        public BaseResultModel Edit_HoatDongKhoaHoc(HoatDongKhoaHocModel HoatDongKhoaHoc);
        public BaseResultModel Delete_HoatDongKhoaHoc(HoatDongKhoaHocModel HoatDongKhoaHoc);
        public List<HoatDongKhoaHocModel> HoatDongKhoaHoc_GetByCanBoID(int CanBoID, int CoQuanID);
        public List<DuAnDeTaiModel> GetDuAnDeTaiByCanBoID(int CanBoID);
        public NhaKhoaHocModel GetThongTinNhaKhoaHoc_DeTai(int CanBoID, Boolean LaCanBoTrongTruong);
        public List<NguoiGioiThieuModel> GetNguoiGioiThieu(int CanBoID, int CoQuanID);
        public BaseResultModel UpdateURL(NhaKhoaHocModel NhaKhoaHocModel);
        public string GetUrlCanBoTrongTruong(int CanBoID, int CoQuanID);
        public List<DanhMucCoQuanDonViModel> SoLuongBaiBaoVaSach(int Nam);
        public NhaKhoaHocModel GetThongTinTuNhaKhoaHocKhac(int CanBoID, int CoQuanID);
        public NhaKhoaHocModel GetThongTinTuDeTai(int CanBoID, int CoQuanID);
    }
    public class LyLichKhoaHocBUS : ILyLichKhoaHocBUS
    {
        private ILyLichKhoaHocDAL _LyLichKhoaHocDAL;
        public LyLichKhoaHocBUS(ILyLichKhoaHocDAL LyLichKhoaHocDAL)
        {
            _LyLichKhoaHocDAL = LyLichKhoaHocDAL;
        }

        public List<NhaKhoaHocModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow, int QuyenQuanLy, string Alphabet)
        {
            return _LyLichKhoaHocDAL.GetPagingBySearch(p, ref TotalRow, QuyenQuanLy, Alphabet);
        }
        public NhaKhoaHocModel GetByID(int CanBoID, string serverPath, int? CoQuanID)
        {
            return _LyLichKhoaHocDAL.GetByID(CanBoID, serverPath, CoQuanID);
        }
        public BaseResultModel Insert(NhaKhoaHocModel NhaKhoaHocModel)
        {
            return _LyLichKhoaHocDAL.Insert(NhaKhoaHocModel);
        }
        public BaseResultModel Update(NhaKhoaHocModel NhaKhoaHocModel)
        {
            return _LyLichKhoaHocDAL.Update(NhaKhoaHocModel);
        }
        public BaseResultModel Delete(NhaKhoaHocModel NhaKhoaHocModel)
        {
            return _LyLichKhoaHocDAL.Delete(NhaKhoaHocModel);
        }
        public BaseResultModel Delete_ThongTinChiTiet(ThongTinCTNhaKhoaHocModel ThongTinChiTiet)
        {
            return _LyLichKhoaHocDAL.Delete_ThongTinChiTiet(ThongTinChiTiet);
        }
        public BaseResultModel Edit_ThongTinChiTiet(ThongTinCTNhaKhoaHocModel ThongTinChiTiet)
        {
            return _LyLichKhoaHocDAL.Edit_ThongTinChiTiet(ThongTinChiTiet);
        }
        public BaseResultModel Delete_HoatDongKhoaHoc(HoatDongKhoaHocModel HoatDongKhoaHoc)
        {
            return _LyLichKhoaHocDAL.Delete_HoatDongKhoaHoc(HoatDongKhoaHoc);
        }
        public BaseResultModel Edit_HoatDongKhoaHoc(HoatDongKhoaHocModel HoatDongKhoaHoc)
        {
            return _LyLichKhoaHocDAL.Edit_HoatDongKhoaHoc(HoatDongKhoaHoc);
        }
        public List<HoatDongKhoaHocModel> HoatDongKhoaHoc_GetByCanBoID(int CanBoID, int CoQuanID)
        {
            return _LyLichKhoaHocDAL.HoatDongKhoaHoc_GetByCanBoID(CanBoID, CoQuanID);
        }
        public List<DuAnDeTaiModel> GetDuAnDeTaiByCanBoID(int CanBoID)
        {
            return _LyLichKhoaHocDAL.GetDuAnDeTaiByCanBoID(CanBoID);
        }
        public NhaKhoaHocModel GetThongTinNhaKhoaHoc_DeTai(int CanBoID, Boolean LaCanBoTrongTruong)
        {
            return _LyLichKhoaHocDAL.GetThongTinNhaKhoaHoc_DeTai(CanBoID, LaCanBoTrongTruong);
        }
        public List<NguoiGioiThieuModel> GetNguoiGioiThieu(int CanBoID, int CoQuanID)
        {
            return _LyLichKhoaHocDAL.GetNguoiGioiThieu(CanBoID, CoQuanID);
        }
        public BaseResultModel UpdateURL(NhaKhoaHocModel NhaKhoaHocModel)
        {
            return _LyLichKhoaHocDAL.UpdateURL(NhaKhoaHocModel);
        }
        public string GetUrlCanBoTrongTruong(int CanBoID, int CoQuanID)
        {
            return _LyLichKhoaHocDAL.GetUrlCanBoTrongTruong(CanBoID, CoQuanID);
        }

        public List<DanhMucCoQuanDonViModel> SoLuongBaiBaoVaSach(int Nam)
        {
            return _LyLichKhoaHocDAL.SoLuongBaiBaoVaSach(Nam);
        }
        public NhaKhoaHocModel GetThongTinTuNhaKhoaHocKhac(int CanBoID, int CoQuanID)
        {
            return _LyLichKhoaHocDAL.GetThongTinTuNhaKhoaHocKhac(CanBoID, CoQuanID);
        }
        public NhaKhoaHocModel GetThongTinTuDeTai(int CanBoID, int CoQuanID)
        {
            return _LyLichKhoaHocDAL.GetThongTinTuDeTai(CanBoID, CoQuanID);
        }
    }
}
