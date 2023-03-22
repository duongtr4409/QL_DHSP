using Com.Gosol.QLKH.DAL.DanhMuc;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.DanhMuc;
using Com.Gosol.QLKH.Models.QuanTriHeThong;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Gosol.QLKH.BUS.DanhMuc
{
    public interface IDanhMucTrangThaiBUS
    {
        List<DanhMucTrangThaiModel> GetPagingBySearch(BasePagingParams p, ref int TotalCount);
        public BaseResultModel Insert(DanhMucTrangThaiModel DanhMucTrangThaiModel);
        public BaseResultModel Update(DanhMucTrangThaiModel DanhMucTrangThaiModel, int CoQuanID);
        public BaseResultModel Delete(List<int> ListTrangThaiID);
        public DanhMucTrangThaiModel GetByID(int TrangThaiID);
        public List<DanhMucTrangThaiModel> GetAll();
    }
    public class DanhMucTrangThaiBUS : IDanhMucTrangThaiBUS
    {
        private IDanhMucTrangThaiDAL _DanhMucTrangThaiDAL;
        public DanhMucTrangThaiBUS(IDanhMucTrangThaiDAL DanhMucTrangThaiDAL)
        {
            _DanhMucTrangThaiDAL = DanhMucTrangThaiDAL;
        }
        public BaseResultModel Insert(DanhMucTrangThaiModel DanhMucTrangThaiModel)
        {
            return _DanhMucTrangThaiDAL.Insert(DanhMucTrangThaiModel);
        }
        public BaseResultModel Update(DanhMucTrangThaiModel DanhMucTrangThaiModel, int CoQuanID)
        {
            return _DanhMucTrangThaiDAL.Update(DanhMucTrangThaiModel, CoQuanID);
        }
        public BaseResultModel Delete(List<int> ListTrangThaiID)
        {
            return _DanhMucTrangThaiDAL.Delete(ListTrangThaiID);
        }
        public DanhMucTrangThaiModel GetByID(int TrangThaiID)
        {
            return _DanhMucTrangThaiDAL.GetByID(TrangThaiID);
        }
        public List<DanhMucTrangThaiModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow)
        {
            return _DanhMucTrangThaiDAL.GetPagingBySearch(p, ref TotalRow);
        }
        public List<DanhMucTrangThaiModel> GetAll()
        {
            return _DanhMucTrangThaiDAL.GetAllDangSuDung();
        }
    }
}
