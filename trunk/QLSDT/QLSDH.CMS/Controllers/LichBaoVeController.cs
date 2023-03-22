using CoreAPI.Entity;
using Excel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TEMIS.CMS.Common;
using TEMIS.CMS.Models;
using TEMIS.CMS.Repository;
using TEMIS.Model;

namespace TEMIS.CMS.Areas.Admin.Controllers
{
    public class LichBaoVeController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();

        public LichBaoVeController()
        {
        }

        public async Task<ActionResult> Index()
        {
            List<KhoaHoc> listKhoaHoc = new List<KhoaHoc>();
            listKhoaHoc = _unitOfWork.GetRepositoryInstance<KhoaHoc>().GetAllRecords().OrderByDescending(o => o.Id).ToList();
            ViewBag.KhoaHoc = listKhoaHoc;

            List<int> listNam = new List<int>();
            int curentyear = DateTime.Now.Year;
            int year = 0;
            for (int i = 0; i < 20; i++)
            {
                year = curentyear - i;
                listNam.Add(year);
            }
            ViewBag.NamBaoVe = listNam;

            return View();
        }

        public async Task<ActionResult> LoadData(int nam = 0, string khoahoc = "", string keysearch = "")
        {
            try
            {
                List<LichBaoVe> list_data = new List<LichBaoVe>();

                //if (nam > 0 && khoahoc != "" && keysearch != "")
                //{
                //    list_data = _unitOfWork.GetRepositoryInstance<LichBaoVe>().GetListByParameter(o => o.NgayBaoVe.Value.Year == nam && o.KhoaHoc == khoahoc && o.TenNCS.Contains(keysearch)).OrderByDescending(x => x.Id).ToList();
                //}
                //else if (nam > 0 && khoahoc != "" && keysearch == "")
                //{
                //    list_data = _unitOfWork.GetRepositoryInstance<LichBaoVe>().GetListByParameter(o => o.NgayBaoVe.Value.Year == nam && o.KhoaHoc == khoahoc).OrderByDescending(x => x.Id).ToList();
                //}
                //else if (nam > 0 && khoahoc == "" && keysearch != "")
                //{
                //    list_data = _unitOfWork.GetRepositoryInstance<LichBaoVe>().GetListByParameter(o => o.NgayBaoVe.Value.Year == nam && o.TenNCS.Contains(keysearch)).OrderByDescending(x => x.Id).ToList();
                //}
                //else if (nam == 0 && khoahoc != "" && keysearch != "")
                //{
                //    list_data = _unitOfWork.GetRepositoryInstance<LichBaoVe>().GetListByParameter(o => o.KhoaHoc == khoahoc && o.TenNCS.Contains(keysearch)).OrderByDescending(x => x.Id).ToList();
                //}
                //else
                //{
                //    list_data = _unitOfWork.GetRepositoryInstance<LichBaoVe>().GetAllRecords().OrderByDescending(x => x.Id).Take(100).ToList();
                //}

                if (nam > 0)
                {
                    list_data = _unitOfWork.GetRepositoryInstance<LichBaoVe>().GetListByParameter(o => o.NgayBaoVe.Value.Year == nam && o.KhoaHoc.Contains(khoahoc)  && o.TenNCS.Contains(keysearch)).OrderByDescending(x => x.Id).ToList();
                }
                else
                {
                    list_data = _unitOfWork.GetRepositoryInstance<LichBaoVe>().GetListByParameter(o =>  o.KhoaHoc.Contains(khoahoc) && o.TenNCS.Contains(keysearch)).OrderByDescending(x => x.Id).ToList();
                }

                return PartialView("_PartialLichBaoVe", list_data);
            }
            catch (Exception ex)
            {
                string mss = ex.Message;
                throw;
            }
        }
    }
}