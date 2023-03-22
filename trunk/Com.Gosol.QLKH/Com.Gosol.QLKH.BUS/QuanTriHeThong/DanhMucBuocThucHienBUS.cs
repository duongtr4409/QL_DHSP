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
    public interface IDanhMucBuocThucHienBUS
    {
        List<DanhMucBuocThucHienModel> GetPagingBySearch(BasePagingParams p, ref int TotalCount);
        public BaseResultModel Insert(DanhMucBuocThucHienModel DanhMucBuocThucHienModel);
        public BaseResultModel Update(DanhMucBuocThucHienModel DanhMucBuocThucHienModel);
        public List<string> Delete(List<int> ListBuocThucHienID);
        public DanhMucBuocThucHienModel GetByID(int BuocThucHienID);
        public List<DanhMucBuocThucHienModel> GetAll();
    }
    public class DanhMucBuocThucHienBUS : IDanhMucBuocThucHienBUS
    {
        private IDanhMucBuocThucHienDAL _DanhMucBuocThucHienDAL;
        public DanhMucBuocThucHienBUS(IDanhMucBuocThucHienDAL danhMucBuocThucHienDAL)
        {
            _DanhMucBuocThucHienDAL = danhMucBuocThucHienDAL;
        }
        public BaseResultModel Insert(DanhMucBuocThucHienModel DanhMucBuocThucHienModel)
        {
            return _DanhMucBuocThucHienDAL.Insert(DanhMucBuocThucHienModel);
        }
        public BaseResultModel Update(DanhMucBuocThucHienModel DanhMucBuocThucHienModel)
        {
            return _DanhMucBuocThucHienDAL.Update(DanhMucBuocThucHienModel);
        }
        public List<string> Delete(List<int> ListBuocThucHienID)
        {
            return _DanhMucBuocThucHienDAL.Delete(ListBuocThucHienID);
        }
        public DanhMucBuocThucHienModel GetByID(int BuocThucHienID)
        {
            return _DanhMucBuocThucHienDAL.GetBuocThucHienByID(BuocThucHienID);
        }
        public List<DanhMucBuocThucHienModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow)
        {
            return _DanhMucBuocThucHienDAL.GetPagingBySearch(p, ref TotalRow);
        }
        public List<DanhMucBuocThucHienModel> GetAll()
        {
            return _DanhMucBuocThucHienDAL.GetAllDangSuDung();
        }
    }
}
