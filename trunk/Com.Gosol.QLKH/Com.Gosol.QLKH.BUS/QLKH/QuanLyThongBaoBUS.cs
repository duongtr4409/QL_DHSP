using Com.Gosol.QLKH.DAL.QLKH;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.QLKH;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Gosol.QLKH.BUS.QLKH
{
    public interface IQuanLyThongBaoBUS
    {
        public List<QuanLyThongBaoModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow);
        public BaseResultModel Edit_ThongBao(QuanLyThongBaoModel ThongBaoModel);
        public QuanLyThongBaoModel GetByID(int ThongBaoID);
        public BaseResultModel Delete_ThongBao(QuanLyThongBaoModel ThongBaoModel);
        public List<ChiTietHienThiThongBaoModel> GetDSThongBaoHienThi(BasePagingParams p, ref int TotalRow, int CanBoID, int CoQuanID);
        public BaseResultModel Update_TrangThaiTatThongBao(List<DoiTuongThongBaoModel> DoiTuongThongBao);
        public List<DoiTuongDeTaiModel> GetDS_DoiTuongThongBaoTheoCap(int? CapQuanLy, int? NamBatDau);
    }
    public class QuanLyThongBaoBUS : IQuanLyThongBaoBUS
    {
        private IQuanLyThongBaoDAL _QuanLyThongBaoDAL;
        public QuanLyThongBaoBUS(IQuanLyThongBaoDAL QuanLyThongBaoDAL)
        {
            _QuanLyThongBaoDAL = QuanLyThongBaoDAL;
        }

        public List<QuanLyThongBaoModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow)
        {
            return _QuanLyThongBaoDAL.GetPagingBySearch(p, ref TotalRow);
        }
        public QuanLyThongBaoModel GetByID(int ThongBaoID)
        {
            return _QuanLyThongBaoDAL.GetByID(ThongBaoID);
        }
        public BaseResultModel Edit_ThongBao(QuanLyThongBaoModel ThongBaoModel)
        {
            return _QuanLyThongBaoDAL.Edit_ThongBao(ThongBaoModel);
        }
        public BaseResultModel Delete_ThongBao(QuanLyThongBaoModel ThongBaoModel)
        {
            return _QuanLyThongBaoDAL.Delete_ThongBao(ThongBaoModel);
        }
        public List<ChiTietHienThiThongBaoModel> GetDSThongBaoHienThi(BasePagingParams p, ref int TotalRow, int CanBoID, int CoQuanID)
        {
            return _QuanLyThongBaoDAL.GetDSThongBaoHienThi(p, ref TotalRow, CanBoID, CoQuanID);
        }
        public BaseResultModel Update_TrangThaiTatThongBao(List<DoiTuongThongBaoModel> DoiTuongThongBao)
        {
            return _QuanLyThongBaoDAL.Update_TrangThaiTatThongBao(DoiTuongThongBao);
        }
        public List<DoiTuongDeTaiModel> GetDS_DoiTuongThongBaoTheoCap(int? CapQuanLy, int? NamBatDau)
        {
            return _QuanLyThongBaoDAL.GetDS_DoiTuongThongBaoTheoCap(CapQuanLy, NamBatDau);
        }
    }
}
