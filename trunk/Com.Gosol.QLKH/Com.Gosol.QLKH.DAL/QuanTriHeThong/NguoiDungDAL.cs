using Com.Gosol.QLKH.DAL.DanhMuc;
using Com.Gosol.QLKH.Models.QuanTriHeThong;
using Com.Gosol.QLKH.Security;
using Com.Gosol.QLKH.Ultilities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.QLKH.DAL.QuanTriHeThong
{
    public interface INguoiDungDAL
    {
        NguoiDungModel GetInfoByLogin(string UserName, string Password);
    }
    public class NguoiDungDAL : INguoiDungDAL
    {
        public NguoiDungModel GetInfoByLogin(string UserName, string Password)
        {

            NguoiDungModel user = null;
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                return user;
            }
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(@"TenNguoiDung",SqlDbType.NVarChar),
                new SqlParameter(@"MatKhau",SqlDbType.NVarChar),
            };

            parameters[0].Value = UserName.Trim();
            parameters[1].Value = Password.Trim();
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_HT_NguoiDung_GetByLogin", parameters))
                {
                    if (dr.Read())
                    {
                        user = new NguoiDungModel();
                        user.TenNguoiDung = Utils.ConvertToString(dr["TenNguoiDung"], "");
                        user.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], "");
                        user.NguoiDungID = Utils.ConvertToInt32(dr["NguoiDungID"], 0);
                        user.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        //user.TenNguoiDung = Utils.ConvertToString(dr["TenNguoiDung"], "");
                        user.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        user.TrangThai = Utils.ConvertToInt32(dr["TrangThai"], 0);
                        //user.CapCoQuan = Utils.ConvertToInt32(dr["CapID"], 0);
                        user.VaiTro = Utils.ConvertToInt32(dr["VaiTro"], 0);
                        user.AnhHoSo = Utils.ConvertToString(dr["AnhHoSo"], "");
                        //user.QuanLyThanNhan = user.VaiTro;
                        // nếu người dùng có quyền quản lý cán bộ thì có quyền quản lý thân nhân
                        //var QuyenCuaCanBo = new ChucNangDAL().GetListChucNangByNguoiDungID(user.NguoiDungID);
                        //if (QuyenCuaCanBo.Any(x => x.ChucNangID == ChucNangEnum.HeThong_QuanLy_CanBo.GetHashCode())) user.QuanLyThanNhan = EnumVaiTroCanBo.LanhDao.GetHashCode();
                    }
                    dr.Close();
                }

                //if (user != null)
                //{
                //    var CoQuanSuDungPhanMem = new DanhMucCoQuanDonViDAL().GetCoQuanSuDungPhanMem_By_CoQuanDangNhap(user.CoQuanID);
                //    user.CoQuanSuDungPhanMem = CoQuanSuDungPhanMem.CoQuanID;
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return user;
        }


    }
}
