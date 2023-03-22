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
    public interface IDanhMucCapDeTaiBUS
    {
        public BaseResultModel Insert(DanhMucCapDeTaiModel item);
        public BaseResultModel InsertMulti(List<DanhMucCapDeTaiModel> items);
        public BaseResultModel Update(DanhMucCapDeTaiModel item);
        public BaseResultModel Delete(int? id);
        public DanhMucCapDeTaiModel GetByID(int id);
        public List<DanhMucCapDeTaiModel> GetAll(string keyword);
        public List<DanhMucCapDeTaiModel> GetAllAndGroup(string keyword, bool? status);
    }
    public class DanhMucCapDeTaiBUS : IDanhMucCapDeTaiBUS
    {
        private IDanhMucCapDeTaiDAL _DanhMucCapDeTaiDAL;
        public DanhMucCapDeTaiBUS(IDanhMucCapDeTaiDAL danhMucCapDeTaiDAL)
        {
            _DanhMucCapDeTaiDAL = danhMucCapDeTaiDAL;
        }
        public BaseResultModel Delete(int? id)
        {
            return _DanhMucCapDeTaiDAL.Delete(id);
        }

        public List<DanhMucCapDeTaiModel> GetAll(string keyword)
        {
            return _DanhMucCapDeTaiDAL.GetAll( keyword);
        }

        public List<DanhMucCapDeTaiModel> GetAllAndGroup(string keyword, bool? status)
        {
            return _DanhMucCapDeTaiDAL.GetAllAndGroup(keyword, status);
        }

        public DanhMucCapDeTaiModel GetByID(int id)
        {
            return _DanhMucCapDeTaiDAL.GetByID(id);
        }

        public BaseResultModel Insert(DanhMucCapDeTaiModel item)
        {
            return _DanhMucCapDeTaiDAL.Insert(item);
        }

        public BaseResultModel InsertMulti(List<DanhMucCapDeTaiModel> items)
        {
            return _DanhMucCapDeTaiDAL.InsertMulti(items);
        }

        public BaseResultModel Update(DanhMucCapDeTaiModel item)
        {
            return _DanhMucCapDeTaiDAL.Update(item);
        }
    }
}
