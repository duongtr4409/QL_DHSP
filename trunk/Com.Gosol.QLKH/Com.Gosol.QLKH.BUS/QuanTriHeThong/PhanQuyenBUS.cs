using Com.Gosol.QLKH.DAL.QuanTriHeThong;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.DanhMuc;
using Com.Gosol.QLKH.Models.QuanTriHeThong;
using Com.Gosol.QLKH.Security;
using Com.Gosol.QLKH.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.QLKH.BUS.QuanTriHeThong
{
    public interface IPhanQuyenBUS
    {
        List<ChucNangModel> GetListChucNangByNguoiDungID(int NguoiDungID);
        public BaseResultModel NhomNguoiDung_Insert(NhomNguoiDungModel NhomNguoiDungModel, int NguoiDungID, int CoQuanID);
        public BaseResultModel NhomNguoiDung_GetForUpdate(int? NhomNguoiDungID, int NguoiDungID, int CoQuanID);
        public BaseResultModel NhomNguoiDung_GetChiTietByID(int? NhomNguoiDungID, int NguoiDungID, int CoQuanID);
        public BaseResultModel NhomNguoiDung_Delete(int? NhomNguoiDungID, int CoQuanID, int NguoiDungID);
        public BaseResultModel NhomNguoiDung_Update(NhomNguoiDungModel NhomNguoiDungModel, int NguoiDungID, int CoQuanID);
        public BaseResultModel NguoiDung_NhomNguoiDung_Insert(NguoiDungNhomNguoiDungModel NguoiDungNhomNguoiDungModel);
        public BaseResultModel NguoiDung_NhomNguoiDung_Delete(int? NguoiDungID, int? NhomNguoiDungID, int? CoQuanID);
        public BaseResultModel PhanQuyen_Insert(PhanQuyenModel PhanQuyenModel);
        public BaseResultModel PhanQuyen_InsertMulti(List<PhanQuyenModel> PhanQuyenModel);
        public BaseResultModel PhanQuyen_Delete(int? PhanQuyenID);
        public BaseResultModel PhanQuyen_UpdateMulti(List<PhanQuyenModel> PhanQuyenModel);
        public List<DanhMucCoQuanDonViModel> DanhMucCoQuan_GetAllFoPhanQuyen(int CoQuanID, int NguoiDungID);
        public List<HeThongCanBoModel> HeThongCanBo_GetAllByListCoQuanID(int NhomNguoiDungID, int CoQuanID, int NguoiDungID);
        public List<HeThongNguoiDungModel> HeThongNguoiDung_GetAllByListCoQuanID(int NhomNguoiDungID);
        public List<HeThongNguoiDungModel> HeThong_NguoiDung_GetListBy_NhomNguoiDungID(int NhomNguoiDungID, int NguoiDungID);
        public List<NhomNguoiDungModel> NhomNguoiDung_GetAll(BasePagingParamsForFilter p, int? CoQuanID, int NguoiDungID, ref int TotalRow);
        public List<ChucNangModel> ChucNang_GetQuyenDuocThaoTacTrongNhom(int NhomNguoiDungID, int CoQuanID, int NguoiDungID);
        public List<NhomNguoiDungModel> GetListNhomNguoiDung_ByCoQuanID(int? CoQuanID);
        public List<HeThongNguoiDungModel> GetAllNguoiDung();
        public NhomNguoiDungDetailModel NhomNguoiDung_ChiTiet(int? NhomNguoiDungID);
        public List<NguoiDungModel> PhanQuyen_DanhSachNguoiDungTrongNhom(int NhomNguoiDungID);
        public int CheckQuyen(int NguoiDungID);
        public bool CheckAdmin(int NguoiDungID);
        public List<ChucNangModel> ChucNang_GetQuyenByNhomNguoiDungID(int NhomNguoiDungID);
    }
    public class PhanQuyenBUS : IPhanQuyenBUS
    {
        private IChucNangDAL _ChucNangDAL;
        private IPhanQuyenDAL _PhanQuyenDAL;
        public PhanQuyenBUS(IChucNangDAL ChucNangDAL, IPhanQuyenDAL PhanQuyenDAL)
        {
            _ChucNangDAL = ChucNangDAL;
            _PhanQuyenDAL = PhanQuyenDAL;
        }
        public List<ChucNangModel> GetListChucNangByNguoiDungID(int NguoiDungID)
        {
            List<ChucNangModel> result = _ChucNangDAL.GetListChucNangByNguoiDungID(NguoiDungID);

            return result;
        }


        public BaseResultModel NhomNguoiDung_Insert(NhomNguoiDungModel NhomNguoiDungModel, int NguoiDungID, int CoQuanID)
        {
            return _PhanQuyenDAL.NhomNguoiDung_Insert(NhomNguoiDungModel, NguoiDungID, CoQuanID);
        }

        public BaseResultModel NhomNguoiDung_GetForUpdate(int? NhomNguoiDungID, int NguoiDungID, int CoQuanID)
        {
            return _PhanQuyenDAL.NhomNguoiDung_GetForUpdate(NhomNguoiDungID, NguoiDungID, CoQuanID);
        }

        public BaseResultModel NhomNguoiDung_GetChiTietByID(int? NhomNguoiDungID, int NguoiDungID, int CoQuanID)
        {
            return _PhanQuyenDAL.NhomNguoiDung_GetChiTietByID(NhomNguoiDungID, NguoiDungID, CoQuanID);
        }

        public BaseResultModel NhomNguoiDung_Delete(int? NhomNguoiDungID, int CoQuanID, int NguoiDungID)
        {
            return _PhanQuyenDAL.NhomNguoiDung_Delete(NhomNguoiDungID, CoQuanID, NguoiDungID);
        }

        public BaseResultModel NhomNguoiDung_Update(NhomNguoiDungModel NhomNguoiDungModel, int NguoiDungID, int CoQuanID)
        {
            return _PhanQuyenDAL.NhomNguoiDung_Update(NhomNguoiDungModel, NguoiDungID, CoQuanID);
        }

        public BaseResultModel NguoiDung_NhomNguoiDung_Insert(NguoiDungNhomNguoiDungModel NguoiDungNhomNguoiDungModel)
        {
            return _PhanQuyenDAL.NguoiDung_NhomNguoiDung_Insert(NguoiDungNhomNguoiDungModel);
        }

        public BaseResultModel NguoiDung_NhomNguoiDung_Delete(int? NguoiDungID, int? NhomNguoiDungID, int? CoQuanID)
        {
            return _PhanQuyenDAL.NguoiDung_NhomNguoiDung_Delete(NguoiDungID, NhomNguoiDungID, CoQuanID);
        }

        public BaseResultModel PhanQuyen_Insert(PhanQuyenModel PhanQuyenModel)
        {
            return _PhanQuyenDAL.PhanQuyen_Insert(PhanQuyenModel);
        }
        public BaseResultModel PhanQuyen_InsertMulti(List<PhanQuyenModel> PhanQuyenModel)
        {
            return _PhanQuyenDAL.PhanQuyen_InsertMulti(PhanQuyenModel);
        }

        public BaseResultModel PhanQuyen_Delete(int? PhanQuyenID)
        {
            return _PhanQuyenDAL.PhanQuyen_Delete(PhanQuyenID);
        }

        public BaseResultModel PhanQuyen_UpdateMulti(List<PhanQuyenModel> PhanQuyenModel)
        {
            return _PhanQuyenDAL.PhanQuyen_UpdateMulti(PhanQuyenModel);
        }

        public List<DanhMucCoQuanDonViModel> DanhMucCoQuan_GetAllFoPhanQuyen(int CoQuanID, int NguoiDungID)
        {
            return _PhanQuyenDAL.DanhMucCoQuan_GetAllFoPhanQuyen(CoQuanID, NguoiDungID);
        }

        public List<HeThongCanBoModel> HeThongCanBo_GetAllByListCoQuanID(int NhomNguoiDungID, int CoQuanID, int NguoiDungID)
        {
            return _PhanQuyenDAL.HeThongCanBo_GetAllByListCoQuanID(NhomNguoiDungID, CoQuanID, NguoiDungID);
        }

        public List<HeThongNguoiDungModel> HeThongNguoiDung_GetAllByListCoQuanID(int NhomNguoiDungID)
        {
            return _PhanQuyenDAL.HeThongNguoiDung_GetAllByListCoQuanID(NhomNguoiDungID);
        }

        public List<HeThongNguoiDungModel> HeThong_NguoiDung_GetListBy_NhomNguoiDungID(int NhomNguoiDungID, int NguoiDungID)
        {
            return _PhanQuyenDAL.HeThong_NguoiDung_GetListBy_NhomNguoiDungID(NhomNguoiDungID, NguoiDungID);
        }

        public List<NhomNguoiDungModel> NhomNguoiDung_GetAll(BasePagingParamsForFilter p, int? CoQuanID, int NguoiDungID, ref int TotalRow)
        {
            return _PhanQuyenDAL.NhomNguoiDung_GetAll(p, CoQuanID, NguoiDungID, ref TotalRow);
        }

        public List<ChucNangModel> ChucNang_GetQuyenDuocThaoTacTrongNhom(int NhomNguoiDungID, int CoQuanID, int NguoiDungID)
        {
            return _PhanQuyenDAL.ChucNang_GetQuyenDuocThaoTacTrongNhom(NhomNguoiDungID, CoQuanID, NguoiDungID);
        }

        public List<NhomNguoiDungModel> GetListNhomNguoiDung_ByCoQuanID(int? CoQuanID)
        {
            return _PhanQuyenDAL.GetListNhomNguoiDung_ByCoQuanID(CoQuanID);
        }

        public List<HeThongNguoiDungModel> GetAllNguoiDung()
        {
            return _PhanQuyenDAL.GetAllNguoiDung();
        }

        public NhomNguoiDungDetailModel NhomNguoiDung_ChiTiet(int? NhomNguoiDungID)
        {
            return _PhanQuyenDAL.NhomNguoiDung_ChiTiet(NhomNguoiDungID);
        }

        public List<NguoiDungModel> PhanQuyen_DanhSachNguoiDungTrongNhom(int NhomNguoiDungID)
        {
            return _PhanQuyenDAL.PhanQuyen_DanhSachNguoiDungTrongNhom(NhomNguoiDungID);
        }
        public int CheckQuyen(int NguoiDungID)
        {
            int quyen = EnumQuyenQuanLy.NhaKhoaHoc.GetHashCode();
            var listChucNang = GetListChucNangByNguoiDungID(NguoiDungID);
            Boolean laQuanLy = false;
            Boolean laTruongKhoa = false;
            foreach (var item in listChucNang)
            {
                if (item.ChucNangID == ChucNangEnum.ql_toan_truong.GetHashCode())
                {
                    laQuanLy = true;
                }
                if (item.ChucNangID == ChucNangEnum.ql_don_vi.GetHashCode())
                {
                    laTruongKhoa = true;
                }
            }
            if (laQuanLy)
            {
                quyen = EnumQuyenQuanLy.QuanLy.GetHashCode();
            }
            else if (laTruongKhoa)
            {
                quyen = EnumQuyenQuanLy.TruongKhoa.GetHashCode();
            }
            return quyen;
        }

        public bool CheckAdmin(int NguoiDungID)
        {
            return _PhanQuyenDAL.CheckAdmin(NguoiDungID);
        }

        public List<ChucNangModel> ChucNang_GetQuyenByNhomNguoiDungID(int NhomNguoiDungID)
        {
            return _PhanQuyenDAL.ChucNang_GetQuyenByNhomNguoiDungID(NhomNguoiDungID);
        }
    }
}