using Com.Gosol.QLKH.DAL.DanhMuc;
using Com.Gosol.QLKH.DAL.QuanTriHeThong;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.DanhMuc;
using Com.Gosol.QLKH.Models.QuanTriHeThong;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Gosol.QLKH.BUS.QuanTriHeThong
{
    public interface IDanhMucBieuMauBUS
    {
        List<DanhMucBieuMauModel> GetPagingBySearch(BasePagingParams p, ref int TotalCount);
        public BaseResultModel Insert(DanhMucBieuMauModel DanhMucBieuMauModel);
        public BaseResultModel Update(DanhMucBieuMauModel DanhMucBieuMauModel);
        public List<string> Delete(List<int> ListBieuMauID);
        public DanhMucBieuMauModel GetByID(int BieuMauID);
        public List<DanhMucBieuMauModel> GetAll();
    }
    public class DanhMucBieuMauBUS : IDanhMucBieuMauBUS
    {
        private IDanhMucBieuMauDAL _DanhMucBieuMauDAL;
        public DanhMucBieuMauBUS(IDanhMucBieuMauDAL danhMucBieuMauDAL)
        {
            _DanhMucBieuMauDAL = danhMucBieuMauDAL;
        }
        public BaseResultModel Insert(DanhMucBieuMauModel DanhMucBieuMauModel)
        {
            return _DanhMucBieuMauDAL.Insert(DanhMucBieuMauModel);
        }
        public BaseResultModel Update(DanhMucBieuMauModel DanhMucBieuMauModel)
        {
            return _DanhMucBieuMauDAL.Update(DanhMucBieuMauModel);
        }
        public List<string> Delete(List<int> ListBieuMauID)
        {
            return _DanhMucBieuMauDAL.Delete(ListBieuMauID);
        }
        public DanhMucBieuMauModel GetByID(int BieuMauID)
        {
            return _DanhMucBieuMauDAL.GetBieuMauByID(BieuMauID);
        }
        public List<DanhMucBieuMauModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow)
        {
            return _DanhMucBieuMauDAL.GetPagingBySearch(p, ref TotalRow);
        }
        public List<DanhMucBieuMauModel> GetAll()
        {
            return _DanhMucBieuMauDAL.GetAll();
        }
    }
}
