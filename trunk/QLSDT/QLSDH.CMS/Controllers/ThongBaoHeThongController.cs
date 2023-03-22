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
    public class ThongBaoHeThongController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        private Model.TEMIS_systemEntities db = new Model.TEMIS_systemEntities();

        public ThongBaoHeThongController()
        {
        }

        public async Task<ActionResult> Index()
        {
            var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
            List<ThongBao> listThongBao = new List<ThongBao>();
            if (user != null)
            {
                listThongBao = _unitOfWork.GetRepositoryInstance<Model.ThongBao>().GetListByParameter(o => o.MaNCS == user.Username || o.Email == user.Username).OrderByDescending(o => o.Id).ToList();
            }
            ViewBag.ThongBao = listThongBao;

            return View();
        }
    }
}