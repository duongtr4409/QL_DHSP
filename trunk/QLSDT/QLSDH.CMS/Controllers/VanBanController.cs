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
    public class VanBanController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        private Model.TEMIS_systemEntities db = new Model.TEMIS_systemEntities();

        public VanBanController()
        {
        }

        // GET: Admin/GiangVien
        public async Task<ActionResult> Index()
        {
            List<ChuyenMucVanBan> listChuyenMuc = _unitOfWork.GetRepositoryInstance<Model.ChuyenMucVanBan>().GetListByParameter(o => o.Id < 4).ToList();
            ViewBag.ChuyenMuc = listChuyenMuc;
            List<Model.VanBan> lstVanBan = new List<Model.VanBan>();
            lstVanBan = _unitOfWork.GetRepositoryInstance<Model.VanBan>().GetAllRecords().ToList();

            return View(lstVanBan);
        }
    }
}