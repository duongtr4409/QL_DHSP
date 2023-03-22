using Com.Gosol.QLKH.DAL.QLKH;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.QLKH;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Gosol.QLKH.BUS.QLKH
{
    public interface IHoiDongBUS
    {
        public List<HoiDongModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow, List<int> listCanBo);
        public HoiDongModel GetByID(int HoiDongID);
        public BaseResultModel Edit_HoiDong(HoiDongModel HoiDongModel);
        public BaseResultModel Delete_HoiDong(HoiDongModel HoiDongModel);
        public BaseResultModel Insert_DanhGiaHoiDong(List<DanhSachHoiDongDanhGiaModel> DSHoiDongDanhGiaModel);
        public List<DanhSachHoiDongDanhGiaModel> GetByHoiDongID(int HoiDongID, string Keyword, int CapQuanLy, int CanBoID);
    }
    public class HoiDongBUS : IHoiDongBUS
    {
        private IHoiDongDAL _HoiDongDAL;
        public HoiDongBUS(IHoiDongDAL HoiDongDAL)
        {
            _HoiDongDAL = HoiDongDAL;
        }

        public List<HoiDongModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow, List<int> listCanBo)
        {
            return _HoiDongDAL.GetPagingBySearch(p, ref TotalRow, listCanBo);
        }
        public HoiDongModel GetByID(int HoiDongID)
        {
            return _HoiDongDAL.GetByID(HoiDongID);
        }
        public BaseResultModel Edit_HoiDong(HoiDongModel HoiDongModel)
        {
            return _HoiDongDAL.Edit_HoiDong(HoiDongModel);
        }
        public BaseResultModel Delete_HoiDong(HoiDongModel HoiDongModel)
        {
            return _HoiDongDAL.Delete_HoiDong(HoiDongModel);
        }
        public BaseResultModel Insert_DanhGiaHoiDong(List<DanhSachHoiDongDanhGiaModel> DSHoiDongDanhGiaModel)
        {
            return _HoiDongDAL.Insert_DanhGiaHoiDong(DSHoiDongDanhGiaModel);
        }
        public List<DanhSachHoiDongDanhGiaModel> GetByHoiDongID(int HoiDongID, string Keyword, int CapQuanLy, int CanBoID)
        {
            return _HoiDongDAL.GetByHoiDongID(HoiDongID, Keyword, CapQuanLy, CanBoID);
        }
    }
}
