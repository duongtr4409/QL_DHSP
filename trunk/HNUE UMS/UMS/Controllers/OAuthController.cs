using System.Linq;
using System.Web.Mvc;
using Hnue.Helper;
using Mvc.JQuery.DataTables.DynamicLinq;
using Ums.App.Base;
using Ums.App.Security;
using Ums.Core.Domain.OAuth;
using Ums.Core.Domain.System;
using Ums.Models.Common;
using Ums.Services.OAuth;

namespace Ums.Website.Controllers
{
    public class OAuthController : BaseController
    {
        private readonly IOAuthApplicationService _applicationService;
        public OAuthController(IOAuthApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        #region APPLICATION
        public ActionResult Application()
        {
            return View();
        }

        [Function("OAUTH_APPLICATION")]
        public object GetApplication(TableModel model)
        {
            var lst = _applicationService.Gets().OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        public ActionResult ApplicationEdit(int id = 0)
        {
            var m = new OAuthApplication
            {
                Id = id,
                Token = Common.RandomString(128) + Common.GetTimeString()
            };
            if (id == 0) return View(m);
            var f = _applicationService.Get(id);
            return View(f);
        }

        [Function("OAUTH_APPLICATION")]
        [HttpPost]
        public ActionResult ApplicationEdit(OAuthApplication m)
        {
            _applicationService.InsertOrUpdate(m);
            return IframeScript;
        }

        [Function("OAUTH_APPLICATION")]
        public ActionResult ApplicationDelete(int id)
        {
            _applicationService.Delete(id);
            return RedirectToAction("Application");
        }
        #endregion
    }
}