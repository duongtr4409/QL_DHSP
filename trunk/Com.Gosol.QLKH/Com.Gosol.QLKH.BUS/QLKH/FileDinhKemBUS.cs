using Com.Gosol.QLKH.DAL.DanhMuc;
using Com.Gosol.QLKH.DAL.QLKH;
using Com.Gosol.QLKH.DAL.QuanTriHeThong;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.DanhMuc;
using Com.Gosol.QLKH.Models.QLKH;
using Com.Gosol.QLKH.Models.QuanTriHeThong;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Gosol.QLKH.BUS.QLKH
{
    public interface IFileDinhKemBUS
    {
        public BaseResultModel Insert(FileDinhKemModel FileDinhKemModel);
        public BaseResultModel Delete(int LoaiFile, int NghiepVuID);
        public BaseResultModel Delete_FileDinhKemID(int FileDinhKemID);
        public FileDinhKemModel GetByID(int FileDinhKemID);
        public List<FileDinhKemModel> GetByLoaiFileAndNghiepVuIDAndCoQuanID(int LoaiFile, int NghiepVuID, int CoQuanID);
    }
    public class FileDinhKemBUS : IFileDinhKemBUS
    {
        private IFileDinhKemDAL _FileDinhKemDAL;
        public FileDinhKemBUS(IFileDinhKemDAL fileDinhKemDAL)
        {
            _FileDinhKemDAL = fileDinhKemDAL;
        }
        public BaseResultModel Insert(FileDinhKemModel FileDinhKemModel)
        {
            return _FileDinhKemDAL.Insert_FileDinhKem(FileDinhKemModel);
        }
        public BaseResultModel Delete(int LoaiFile, int NghiepVuID)
        {
            return _FileDinhKemDAL.Delete_FileDinhKem(LoaiFile, NghiepVuID);
        }
        public FileDinhKemModel GetByID(int FileDinhKemID)
        {
            return _FileDinhKemDAL.GetByID(FileDinhKemID);
        }
        public BaseResultModel Delete_FileDinhKemID(int FileDinhKemID)
        {
            return _FileDinhKemDAL.Delete_FileDinhKemID(FileDinhKemID);
        }
        public List<FileDinhKemModel> GetByLoaiFileAndNghiepVuIDAndCoQuanID(int LoaiFile, int NghiepVuID, int CoQuanID)
        {
            return _FileDinhKemDAL.GetByLoaiFileAndNghiepVuIDAndCoQuanID(LoaiFile, NghiepVuID, CoQuanID);
        }
    }
}
