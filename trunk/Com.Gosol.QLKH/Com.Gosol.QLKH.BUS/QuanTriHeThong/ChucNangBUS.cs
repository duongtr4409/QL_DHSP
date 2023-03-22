using Com.Gosol.QLKH.DAL.QuanTriHeThong;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.DanhMuc;
using Com.Gosol.QLKH.Models.QuanTriHeThong;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.QLKH.BUS.QuanTriHeThong
{
    public interface IChucNangBUS
    {
        public List<ChucNangModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow);
    }
    public class ChucNangBUS : IChucNangBUS
    {
        private IChucNangDAL _ChucNangDAL;
        public ChucNangBUS(IChucNangDAL ChucNangDAL)
        {
            _ChucNangDAL = ChucNangDAL;
        }
       
        public List<ChucNangModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow)
        {
            return _ChucNangDAL.GetPagingBySearch(p, ref TotalRow);
        }
    }
}