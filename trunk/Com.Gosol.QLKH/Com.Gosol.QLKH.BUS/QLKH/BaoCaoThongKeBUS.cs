using Com.Gosol.QLKH.DAL.QLKH;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.QLKH;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Gosol.QLKH.BUS.QLKH
{
    public interface IBaoCaoThongKeBUS
    {
        public List<BaoCaoThongKeModel> BCThongKeNhiemVuKhoaHoc(BaoCaoThongKeParams p, List<int> listCanBoID, List<int> listCanBoNuID);
        public List<BCDanhSachNhiemVuKHModel> BCDanhSachNhiemVuKhoaHoc(BaoCaoThongKeParams p, List<int> listCanBoID);
        public List<BCTinhHinhKetQuaModel> BCTinhHinhKetQuaThucHien(BaoCaoThongKeParams p, List<int> listCanBoID);
        public List<BCThongKeHoatDongNghienCuuModel> BCThongKeHoatDongNghienCuu(BaoCaoThongKeParams p, List<int> listCanBoID);
        public List<BCThongKeKetQuaNghienCuuModel> BCThongKeKetQuaNghienCuu(BaoCaoThongKeParams p, List<int> listCanBoID);
        public BaseResultModel CongBoKetQuaNghienCuu(List<BCThongKeKetQuaNghienCuuModel> KetQuaNghienCuuModel);
        public List<BCKeKhaiBaiBaoKhoaHocModel> BCKeKhaiBaiBaoKhoaHoc(BaoCaoThongKeParams p);
    }
    public class BaoCaoThongKeBUS : IBaoCaoThongKeBUS
    {
        private IBaoCaoThongKeDAL _BaoCaoThongKeDAL;
        public BaoCaoThongKeBUS(IBaoCaoThongKeDAL BaoCaoThongKeDAL)
        {
            _BaoCaoThongKeDAL = BaoCaoThongKeDAL;
        }
        public List<BaoCaoThongKeModel> BCThongKeNhiemVuKhoaHoc(BaoCaoThongKeParams p, List<int> listCanBoID, List<int> listCanBoNuID)
        {
            return _BaoCaoThongKeDAL.BCThongKeNhiemVuKhoaHoc(p, listCanBoID, listCanBoNuID);
        }
        public List<BCDanhSachNhiemVuKHModel> BCDanhSachNhiemVuKhoaHoc(BaoCaoThongKeParams p, List<int> listCanBoID)
        {
            return _BaoCaoThongKeDAL.BCDanhSachNhiemVuKhoaHoc(p, listCanBoID);
        }
        public List<BCTinhHinhKetQuaModel> BCTinhHinhKetQuaThucHien(BaoCaoThongKeParams p, List<int> listCanBoID)
        {
            return _BaoCaoThongKeDAL.BCTinhHinhKetQuaThucHien(p, listCanBoID);
        }
        public List<BCThongKeHoatDongNghienCuuModel> BCThongKeHoatDongNghienCuu(BaoCaoThongKeParams p, List<int> listCanBoID)
        {
            return _BaoCaoThongKeDAL.BCThongKeHoatDongNghienCuu(p, listCanBoID);
        }
        public List<BCThongKeKetQuaNghienCuuModel> BCThongKeKetQuaNghienCuu(BaoCaoThongKeParams p, List<int> listCanBoID)
        {
            return _BaoCaoThongKeDAL.BCThongKeKetQuaNghienCuu(p, listCanBoID);
        }
        public List<BCKeKhaiBaiBaoKhoaHocModel> BCKeKhaiBaiBaoKhoaHoc(BaoCaoThongKeParams p)
        {
            return _BaoCaoThongKeDAL.BCKeKhaiBaiBaoKhoaHoc(p);
        }
        public BaseResultModel CongBoKetQuaNghienCuu(List<BCThongKeKetQuaNghienCuuModel> KetQuaNghienCuuModel)
        {
            return _BaoCaoThongKeDAL.CongBoKetQuaNghienCuu(KetQuaNghienCuuModel);
        }
    }
}
