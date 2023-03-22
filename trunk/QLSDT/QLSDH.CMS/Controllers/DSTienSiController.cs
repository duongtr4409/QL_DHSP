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
    public class DSTienSiController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        private Model.TEMIS_systemEntities db = new Model.TEMIS_systemEntities();

        public DSTienSiController()
        {
        }
        // GET: Admin/GiangVien
        public async Task<ActionResult> Index()
        {
            //ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");

            //var lstVaiTro = _unitOfWork.GetRepositoryInstance<Model.AspNetRole>().GetAllRecords();
            //ViewBag.ListVaiTro = new SelectList(lstVaiTro, "Id", "Name");

            List<Model.GiangVien> lstGV = new List<Model.GiangVien>();
         
            lstGV = _unitOfWork.GetRepositoryInstance<Model.GiangVien>().GetAllRecords().ToList();

            return View();
        }
    }
}