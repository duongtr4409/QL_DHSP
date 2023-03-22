using Com.Gosol.QLKH.DAL.DanhMuc;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.DanhMuc;
using Com.Gosol.QLKH.Models.QuanTriHeThong;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Gosol.QLKH.BUS.DanhMuc
{
    public interface IDanhMucLoaiHinhNghienCuuBUS
    {
        List<DanhMucLoaiHinhNghienCuuModel> GetPagingBySearch(BasePagingParams p, ref int TotalCount);
        List<DanhMucLoaiHinhNghienCuuModel> GetAllDangSuDung();
        public BaseResultModel Insert(DanhMucLoaiHinhNghienCuuModel DanhMucLoaiHinhNghienCuuModel);
        public BaseResultModel Update(DanhMucLoaiHinhNghienCuuModel DanhMucLoaiHinhNghienCuuModel, int CoQuanID);
        public BaseResultModel Delete(List<int> ListLoaiHinhNghienCuuID);
        public DanhMucLoaiHinhNghienCuuModel GetByID(int LoaiHinhNghienCuuID);
    }
    public class DanhMucLoaiHinhNghienCuuBUS : IDanhMucLoaiHinhNghienCuuBUS
    {
        private IDanhMucLoaiHinhNghienCuuDAL _DanhMucLoaiHinhNghienCuuDAL;
        public DanhMucLoaiHinhNghienCuuBUS(IDanhMucLoaiHinhNghienCuuDAL DanhMucLoaiHinhNghienCuuDAL)
        {
            _DanhMucLoaiHinhNghienCuuDAL = DanhMucLoaiHinhNghienCuuDAL;
        }
        public BaseResultModel Insert(DanhMucLoaiHinhNghienCuuModel DanhMucLoaiHinhNghienCuuModel)
        {
            return _DanhMucLoaiHinhNghienCuuDAL.Insert(DanhMucLoaiHinhNghienCuuModel);
        }
        public BaseResultModel Update(DanhMucLoaiHinhNghienCuuModel DanhMucLoaiHinhNghienCuuModel, int CoQuanID)
        {
            return _DanhMucLoaiHinhNghienCuuDAL.Update(DanhMucLoaiHinhNghienCuuModel, CoQuanID);
        }
        public BaseResultModel Delete(List<int> ListLoaiHinhNghienCuuID)
        {
            return _DanhMucLoaiHinhNghienCuuDAL.Delete(ListLoaiHinhNghienCuuID);
        }
        public DanhMucLoaiHinhNghienCuuModel GetByID(int LoaiHinhNghienCuuID)
        {
            return _DanhMucLoaiHinhNghienCuuDAL.GetByID(LoaiHinhNghienCuuID);
        }
        public List<DanhMucLoaiHinhNghienCuuModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow)
        {
            return _DanhMucLoaiHinhNghienCuuDAL.GetPagingBySearch(p, ref TotalRow);
        }
        public List<DanhMucLoaiHinhNghienCuuModel> GetAllDangSuDung()
        {
            return _DanhMucLoaiHinhNghienCuuDAL.GetAllDangSuDung();
        }
    }
}
