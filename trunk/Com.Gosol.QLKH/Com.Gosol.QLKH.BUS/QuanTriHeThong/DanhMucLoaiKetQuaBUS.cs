using Com.Gosol.QLKH.DAL.DanhMuc;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.DanhMuc;
using Com.Gosol.QLKH.Models.QuanTriHeThong;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Gosol.QLKH.BUS.DanhMuc
{
    public interface IDanhMucLoaiKetQuaBUS
    {
        public BaseResultModel Insert(DanhMucLoaiKetQuaModel item);
        public BaseResultModel InsertMulti(List<DanhMucLoaiKetQuaModel> items);
        public BaseResultModel Update(DanhMucLoaiKetQuaModel item);
        public BaseResultModel Delete(int? id);
        public DanhMucLoaiKetQuaModel GetByID(int id);
        public List<DanhMucLoaiKetQuaModel> GetAll(string keyword);
        public List<DanhMucLoaiKetQuaModel> GetAllAndGroup(string keyword, bool? status);
    }
    public class DanhMucLoaiKetQuaBUS : IDanhMucLoaiKetQuaBUS
    {
        private IDanhMucLoaiKetQuaDAL _DanhMucLoaiKetQuaDAL;
        public DanhMucLoaiKetQuaBUS(IDanhMucLoaiKetQuaDAL danhMucLoaiKetQuaDAL)
        {
            _DanhMucLoaiKetQuaDAL = danhMucLoaiKetQuaDAL;
        }
        public BaseResultModel Delete(int? id)
        {
            return _DanhMucLoaiKetQuaDAL.Delete(id);
        }

        public List<DanhMucLoaiKetQuaModel> GetAll(string keyword)
        {
            return _DanhMucLoaiKetQuaDAL.GetAll(keyword);
        }

        public List<DanhMucLoaiKetQuaModel> GetAllAndGroup(string keyword, bool? status)
        {
            return _DanhMucLoaiKetQuaDAL.GetAllAndGroup(keyword, status);
        }

        public DanhMucLoaiKetQuaModel GetByID(int id)
        {
            return _DanhMucLoaiKetQuaDAL.GetByID(id);
        }

        public BaseResultModel Insert(DanhMucLoaiKetQuaModel item)
        {
            return _DanhMucLoaiKetQuaDAL.Insert(item);
        }

        public BaseResultModel InsertMulti(List<DanhMucLoaiKetQuaModel> items)
        {
            return _DanhMucLoaiKetQuaDAL.InsertMulti(items);
        }

        public BaseResultModel Update(DanhMucLoaiKetQuaModel item)
        {
            return _DanhMucLoaiKetQuaDAL.Update(item);
        }
    }
}
