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
    public interface IDanhMucLinhVucBUS
    {
        public BaseResultModel Insert(DanhMucLinhVucModel item);
        public BaseResultModel InsertMulti(List<DanhMucLinhVucModel> items);
        public BaseResultModel Update(DanhMucLinhVucModel item);
        public BaseResultModel Delete(int? id);
        public DanhMucLinhVucModel GetByID(int id);
        public List<DanhMucLinhVucModel> GetAll(int? type, string keyword, bool? status);
        public List<DanhMucLinhVucModel> GetAllAndGroup(int? type, string keyword, bool? status);
    }
    public class DanhMucLinhVucBUS : IDanhMucLinhVucBUS
    {
        private IDanhMucLinhVucDAL _DanhMucLinhVucDAL;
        public DanhMucLinhVucBUS(IDanhMucLinhVucDAL danhMucLinhVucDAL)
        {
            _DanhMucLinhVucDAL = danhMucLinhVucDAL;
        }
        public BaseResultModel Delete(int? id)
        {
            return _DanhMucLinhVucDAL.Delete(id);
        }

        public List<DanhMucLinhVucModel> GetAll(int? type, string keyword, bool? status)
        {
            return _DanhMucLinhVucDAL.GetAll(type, keyword, status);
        }

        public List<DanhMucLinhVucModel> GetAllAndGroup(int? type, string keyword, bool? status)
        {
            return _DanhMucLinhVucDAL.GetAllAndGroup(type, keyword, status);
        }

        public DanhMucLinhVucModel GetByID(int id)
        {
            return _DanhMucLinhVucDAL.GetByID(id);
        }

        public BaseResultModel Insert(DanhMucLinhVucModel item)
        {
            return _DanhMucLinhVucDAL.Insert(item);
        }

        public BaseResultModel InsertMulti(List<DanhMucLinhVucModel> items)
        {
            return _DanhMucLinhVucDAL.InsertMulti(items);
        }

        public BaseResultModel Update(DanhMucLinhVucModel item)
        {
            return _DanhMucLinhVucDAL.Update(item);
        }
    }
}
