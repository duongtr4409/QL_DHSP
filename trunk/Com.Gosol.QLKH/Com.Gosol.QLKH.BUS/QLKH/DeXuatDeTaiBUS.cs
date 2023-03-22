using Com.Gosol.QLKH.DAL.QLKH;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.QLKH;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Gosol.QLKH.BUS.QLKH
{
    public interface IDeXuatDeTaiBUS
    {
        public List<DeXuatDeTaiModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow, int LinhVucNghienCuu, int LinhVucKinhTeXaHoi, int CapQuanLy, int TrangThaiID, int CanBoID, int CoQuanID, Boolean LaQuanLy, int CanBoIDLogin);
        public DeXuatDeTaiModel GetByID(int DeXuatID);
        public BaseResultModel Insert(DeXuatDeTaiModel DeTaiModel);
        public BaseResultModel Update(DeXuatDeTaiModel DeTaiModel);
        public BaseResultModel Delete(DeXuatDeTaiModel DeTaiModel);
        public BaseResultModel Update_TrangThaiDeTai(LichSuDuyetDeXuatModel DeTaiModel);
        public List<DeXuatDeTaiModel> DanhSachDeXuatDaGuiFilter(int CoQuanID, int CanBoID, int LinhVucID, string Keyword, bool isCount);
        public List<LichSuChinhSuaDeXuatModel> GetLichSuDeXuat(int DeXuatID);
        public BaseResultModel Update_DeXuatLog(int DeXuatID, int? CanBoID, int? CoQuanID);
        public List<DeXuatDeTaiChiTiet> GetListQuanLy(int CoQuanID);
        public DeTaiModel GetDeTaiByLSDeXuatID(int ID);
    }
    public class DeXuatDeTaiBUS : IDeXuatDeTaiBUS
    {
        private IDeXuatDeTaiDAL _DeXuatDeTaiDAL;
        public DeXuatDeTaiBUS(IDeXuatDeTaiDAL DeXuatDeTaiDAL)
        {
            _DeXuatDeTaiDAL = DeXuatDeTaiDAL;
        }

        public List<DeXuatDeTaiModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow, int LinhVucNghienCuu, int LinhVucKinhTeXaHoi, int CapQuanLy, int TrangThaiID, int CanBoID, int CoQuanID, Boolean LaQuanLy, int CanBoIDLogin)
        {
            return _DeXuatDeTaiDAL.GetPagingBySearch(p, ref TotalRow, LinhVucNghienCuu, LinhVucKinhTeXaHoi, CapQuanLy, TrangThaiID, CanBoID, CoQuanID, LaQuanLy, CanBoIDLogin);
        }
        public DeXuatDeTaiModel GetByID(int DeXuatID)
        {
            return _DeXuatDeTaiDAL.GetByID(DeXuatID);
        }
        public BaseResultModel Insert(DeXuatDeTaiModel DeTaiModel)
        {
            return _DeXuatDeTaiDAL.Insert(DeTaiModel);
        }
        public BaseResultModel Update(DeXuatDeTaiModel DeTaiModel)
        {
            return _DeXuatDeTaiDAL.Update(DeTaiModel);
        }
        public BaseResultModel Delete(DeXuatDeTaiModel DeTaiModel)
        {
            return _DeXuatDeTaiDAL.Delete(DeTaiModel);
        }
        public BaseResultModel Update_TrangThaiDeTai(LichSuDuyetDeXuatModel DeTaiModel)
        {
            return _DeXuatDeTaiDAL.Update_TrangThaiDeTai(DeTaiModel);
        }
        public List<DeXuatDeTaiModel> DanhSachDeXuatDaGuiFilter(int CoQuanID, int CanBoID, int LinhVucID, string Keyword, bool isCount)
        {
            return _DeXuatDeTaiDAL.DanhSachDeXuatDaGuiFilter(CoQuanID, CanBoID, LinhVucID, Keyword, isCount);
        }
        public List<LichSuChinhSuaDeXuatModel> GetLichSuDeXuat(int DeXuatID)
        {
            return _DeXuatDeTaiDAL.GetLichSuDeXuat(DeXuatID);
        }
        public BaseResultModel Update_DeXuatLog(int DeXuatID, int? CanBoID, int? CoQuanID)
        {
            return _DeXuatDeTaiDAL.Update_DeXuatLog(DeXuatID, CanBoID, CoQuanID);
        }
        public List<DeXuatDeTaiChiTiet> GetListQuanLy(int CoQuanID)
        {
            return _DeXuatDeTaiDAL.GetListQuanLy(CoQuanID);
        }
        public DeTaiModel GetDeTaiByLSDeXuatID(int ID)
        {
            return _DeXuatDeTaiDAL.GetDeTaiByLSDeXuatID(ID);
        }
    }
}
