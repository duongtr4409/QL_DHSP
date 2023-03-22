using Com.Gosol.QLKH.DAL.QLKH;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.DanhMuc;
using Com.Gosol.QLKH.Models.QLKH;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Gosol.QLKH.BUS.QLKH
{
    public interface IDeTaiBUS
    {
        public List<DeTaiModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow, int LinhVucNghienCuu, int LinhVucKinhTeXaHoi, int CapQuanLy, int TrangThai, int CanBoID, List<int> listCanBoID);
        public DeTaiModel GetByID(int DeTaiID, string serverPath);
        public BaseResultModel Insert(DeTaiModel DeTaiModel);
        public BaseResultModel Update(DeTaiModel DeTaiModel);
        public BaseResultModel Update_TrangThaiDeTai(DeTaiModel DeTaiModel);
        public BaseResultModel Edit_ThongTinChiTiet(ThongTinChiTietDeTaiModel ChiTiet);
        public BaseResultModel Delete_ThongTinChiTiet(ThongTinChiTietDeTaiModel ThongTinChiTiet);
        public BaseResultModel Edit_KetQuaNghienCuu(KetQuaNghienCuuModel KetQuaNghienCuu);
        public BaseResultModel Delete_KetQuaNghienCuu(KetQuaNghienCuuModel KetQuaNghienCuu);
        public BaseResultModel Delete(DeTaiModel DeTaiModel);
        public List<DuLieuNghienCuuKhoaHocModel> DuLieuNghienCuuKhoaHoc(List<int> StaffId, int? Year);
        public List<DeTaiModel> GetDeTaiByCanBoID(int CanBoID);
        public List<KetQuaNghienCuuModel> GetKetQuaNghienCuuByDeTaiID(int DeTaiID);
        public List<DanhMucCoQuanDonViModel> SoLuongDeTaiTheoCoQuan();
        public List<ThongTinCTNhaKhoaHocModel> DuLieuHoatDongKhoHocKhac(List<int> StaffId, int? Year);
    }
    public class DeTaiBUS : IDeTaiBUS
    {
        private IDeTaiDAL _DeTaiDAL;
        public DeTaiBUS(IDeTaiDAL DeTaiDAL)
        {
            _DeTaiDAL = DeTaiDAL;
        }

        public List<DeTaiModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow, int LinhVucNghienCuu, int LinhVucKinhTeXaHoi, int CapQuanLy, int TrangThai, int CanBoID, List<int> listCanBoID)
        {
            return _DeTaiDAL.GetPagingBySearch(p, ref TotalRow, LinhVucNghienCuu, LinhVucKinhTeXaHoi, CapQuanLy, TrangThai, CanBoID, listCanBoID);
        }
        public DeTaiModel GetByID(int DeTaiID, string serverPath)
        {
            return _DeTaiDAL.GetByID(DeTaiID, serverPath);
        }
        public BaseResultModel Insert(DeTaiModel DeTaiModel)
        {
            return _DeTaiDAL.Insert(DeTaiModel);
        }
        public BaseResultModel Update(DeTaiModel DeTaiModel)
        {
            return _DeTaiDAL.Update(DeTaiModel);
        }
        public BaseResultModel Update_TrangThaiDeTai(DeTaiModel DeTaiModel)
        {
            return _DeTaiDAL.Update_TrangThaiDeTai(DeTaiModel);
        }
        public BaseResultModel Edit_ThongTinChiTiet(ThongTinChiTietDeTaiModel ChiTiet)
        {
            return _DeTaiDAL.Edit_ThongTinChiTiet(ChiTiet);
        }
        public BaseResultModel Delete_ThongTinChiTiet(ThongTinChiTietDeTaiModel ChiTiet)
        {
            return _DeTaiDAL.Delete_ThongTinChiTiet(ChiTiet);
        }
        public BaseResultModel Edit_KetQuaNghienCuu(KetQuaNghienCuuModel KetQuaNghienCuu)
        {
            return _DeTaiDAL.Edit_KetQuaNghienCuu(KetQuaNghienCuu);
        }
        public BaseResultModel Delete_KetQuaNghienCuu(KetQuaNghienCuuModel KetQuaNghienCuu)
        {
            return _DeTaiDAL.Delete_KetQuaNghienCuu(KetQuaNghienCuu);
        }
        public BaseResultModel Delete(DeTaiModel DeTaiModel)
        {
            return _DeTaiDAL.Delete(DeTaiModel);
        }
        public List<DuLieuNghienCuuKhoaHocModel> DuLieuNghienCuuKhoaHoc(List<int> StaffId, int? Year)
        {
            return _DeTaiDAL.DuLieuNghienCuuKhoaHoc(StaffId, Year);
        }

        public List<DeTaiModel> GetDeTaiByCanBoID(int CanBoID)
        {
            return _DeTaiDAL.GetDeTaiByCanBoID(CanBoID);
        }
        public List<KetQuaNghienCuuModel> GetKetQuaNghienCuuByDeTaiID(int DeTaiID)
        {
            return _DeTaiDAL.GetKetQuaNghienCuuByDeTaiID(DeTaiID);
        }

        public List<DanhMucCoQuanDonViModel> SoLuongDeTaiTheoCoQuan()
        {
            return _DeTaiDAL.SoLuongDeTaiTheoCoQuan();
        }

        public List<ThongTinCTNhaKhoaHocModel> DuLieuHoatDongKhoHocKhac(List<int> StaffId, int? Year)
        {
            return _DeTaiDAL.DuLieuHoatDongKhoHocKhac(StaffId, Year);
        }
    }
}
