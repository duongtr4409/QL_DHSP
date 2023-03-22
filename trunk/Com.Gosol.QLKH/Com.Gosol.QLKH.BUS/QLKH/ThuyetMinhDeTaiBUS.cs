using Com.Gosol.QLKH.DAL.QLKH;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.QLKH;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Gosol.QLKH.BUS.QLKH
{
    public interface IThuyetMinhDeTaiBUS
    {
        public List<DeXuatThuyetMinhModel> GetPagingBySearch(BasePagingParams p, int CapQuanLy);
        public BaseResultModel InsertThuyetMinh(ThuyetMinhDeTaiModel ThuyetMinhDeTai);
        public BaseResultModel UpdateThuyetMinh(ThuyetMinhDeTaiModel ThuyetMinhDeTai);
        public ThuyetMinhDeTaiModel GetByID(int ThuyetMinhID);
        public List<ThuyetMinhDeTaiModel> GetAllThuyetMinhByDeXuatID(int DeXuatID);
        public BaseResultModel Delete_ThuyetMinh(int ThuyetMinhID);
        public BaseResultModel DuyetThuyetMinh(int ThuyetMinhID, int DeXuatID);
    }
    public class ThuyetMinhDeTaiBUS : IThuyetMinhDeTaiBUS
    {
        private IThuyetMinhDeTaiDAL _ThuyetMinhDeTaiDAL;
        public ThuyetMinhDeTaiBUS(IThuyetMinhDeTaiDAL ThuyetMinhDeTaiDAL)
        {
            _ThuyetMinhDeTaiDAL = ThuyetMinhDeTaiDAL;
        }
        public List<DeXuatThuyetMinhModel> GetPagingBySearch(BasePagingParams p, int CapQuanLy)
        {
            return _ThuyetMinhDeTaiDAL.GetPagingBySearch(p, CapQuanLy);
        }
        public BaseResultModel InsertThuyetMinh(ThuyetMinhDeTaiModel ThuyetMinhDeTai)
        {
            return _ThuyetMinhDeTaiDAL.InsertThuyetMinh(ThuyetMinhDeTai);
        }
        public BaseResultModel UpdateThuyetMinh(ThuyetMinhDeTaiModel ThuyetMinhDeTai)
        {
            return _ThuyetMinhDeTaiDAL.UpdateThuyetMinh(ThuyetMinhDeTai);
        }
        public ThuyetMinhDeTaiModel GetByID(int ThuyetMinhID)
        {
            return _ThuyetMinhDeTaiDAL.GetByID(ThuyetMinhID);
        }
        public List<ThuyetMinhDeTaiModel> GetAllThuyetMinhByDeXuatID(int DeXuatID)
        {
            return _ThuyetMinhDeTaiDAL.GetAllThuyetMinhByDeXuatID(DeXuatID);
        }
        public BaseResultModel Delete_ThuyetMinh(int ThuyetMinhID)
        {
            return _ThuyetMinhDeTaiDAL.Delete_ThuyetMinh(ThuyetMinhID);
        }
        public BaseResultModel DuyetThuyetMinh(int ThuyetMinhID, int DeXuatID)
        {
            return _ThuyetMinhDeTaiDAL.DuyetThuyetMinh(ThuyetMinhID, DeXuatID);
        }
    }
}
