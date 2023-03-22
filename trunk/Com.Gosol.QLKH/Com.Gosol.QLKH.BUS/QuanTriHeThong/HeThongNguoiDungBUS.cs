﻿using Com.Gosol.QLKH.DAL.DanhMuc;
using Com.Gosol.QLKH.DAL.QuanTriHeThong;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.DanhMuc;
using Com.Gosol.QLKH.Models.QuanTriHeThong;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Gosol.QLKH.BUS.QuanTriHeThong
{
    public interface IHeThongNguoidungBUS
    {
        public int Insert(HeThongNguoiDungModel HeThongNguoiDungModel, ref string Message, ref int NguoiDungID);
        public int Update(HeThongNguoiDungModel HeThongNguoiDungModel, ref string Message);
        public List<string> Delete(List<int> ListNguoiDungID,ref int Status);
        public HeThongNguoiDungModel GetByID(int NguoiDungID);
        public List<object> GetPagingBySearch(BasePagingParams p, ref int TotalRow, int? CoQuanID, int? TrangThai);
        public Dictionary<int, string> ResetPassword(int NguoiDungID);
        public NguoiDungModel GetByIDForPhanQuyen(int NguoiDungID);
        public BaseResultModel SendMail(string TenDangNhap,string Url);
       
        public BaseResultModel ChangePassword(int NguoiDungID, string OldPassword, string NewPassword);
        public List<HeThongNguoiDungModelPartial> HeThong_NguoiDung_GetListBy_NhomNguoiDungID(int NhomNguoiDungID);
        public BaseResultModel CheckMaMail(string Ma);
        public BaseResultModel UpdateNguoiDung(string TenDangNhap, string MatKhauMoi);
        public List<HeThongNguoiDungModel> GetAll();
    }
    public class HeThongNguoidungBUS : IHeThongNguoidungBUS
    {
        private IHeThongNguoiDungDAL _HeThongNguoidungDAL;
        public HeThongNguoidungBUS(IHeThongNguoiDungDAL heThongNguoidungDAL)
        {
            this._HeThongNguoidungDAL = heThongNguoidungDAL;
        }
        public int Insert(HeThongNguoiDungModel HeThongNguoiDungModel, ref string Message, ref int NguoiDungID)
        {
            return _HeThongNguoidungDAL.Insert(HeThongNguoiDungModel, ref Message, ref  NguoiDungID);
        }
        public int Update(HeThongNguoiDungModel HeThongNguoiDungModel, ref string Message)
        {
            return _HeThongNguoidungDAL.Update(HeThongNguoiDungModel, ref Message);
        }
        public List<string> Delete(List<int> ListNguoiDungID, ref int Status)
        {
            return _HeThongNguoidungDAL.Delete(ListNguoiDungID,ref Status);
        }
        public HeThongNguoiDungModel GetByID(int NguoiDungID)
        {
            return _HeThongNguoidungDAL.GetByID(NguoiDungID);
        }
        public List<object> GetPagingBySearch(BasePagingParams p, ref int TotalRow, int? CoQuanID, int? TrangThai)
        {
            return _HeThongNguoidungDAL.GetPagingBySearch(p, ref TotalRow, CoQuanID, TrangThai);
        }
        public Dictionary<int, string> ResetPassword(int NguoiDungID)
        {
            return _HeThongNguoidungDAL.ResetPassword(NguoiDungID);
        }
        public NguoiDungModel GetByIDForPhanQuyen(int NguoiDungID)
        {
            return _HeThongNguoidungDAL.GetByIDForPhanQuyen(NguoiDungID);
        }
        public BaseResultModel SendMail(string TenDangNhap, string Url)
        {
            return _HeThongNguoidungDAL.SendMail(TenDangNhap,Url);
        }

       
        public BaseResultModel ChangePassword(int NguoiDungID, string OldPassword, string NewPassword)
        {
            return _HeThongNguoidungDAL.ChangePassword(NguoiDungID,  OldPassword, NewPassword);
        }

        public List<HeThongNguoiDungModelPartial> HeThong_NguoiDung_GetListBy_NhomNguoiDungID(int NhomNguoiDungID)
        {
            return _HeThongNguoidungDAL.HeThong_NguoiDung_GetListBy_NhomNguoiDungID(NhomNguoiDungID);
        }
        public BaseResultModel CheckMaMail(string Ma)
        {
            return  _HeThongNguoidungDAL.CheckMaMail(Ma);
        }
        public BaseResultModel UpdateNguoiDung(string TenDangNhap, string MatKhauMoi)
        {
            return _HeThongNguoidungDAL.UpdateNguoiDung(TenDangNhap,MatKhauMoi);
        }

        public List<HeThongNguoiDungModel> GetAll()
        {
            return _HeThongNguoidungDAL.GetAll();
        }
    }
}
