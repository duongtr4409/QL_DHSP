using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using Ums.App;
using Ums.App.Base;
using Ums.Core.Entities.Shared;
using Ums.Services.Organize;
using Ums.Services.System;

namespace Ums.Website.Areas.Api_v2.Controllers
{
    public class OrganizeController : BaseApiController
    {
        private readonly ISystemLogService _systemLogService;
        private readonly IDepartmentService _departmentService;
        public OrganizeController()
        {
            _systemLogService = UnityConfig.Resolve<ISystemLogService>();
            _departmentService = UnityConfig.Resolve<IDepartmentService>();
        }

        public JsonResult<ApiResponse> GetDepartmentTypes()
        {
            var data = new List<object>
            {
                new{Id=1, Name="Khoa, Bộ môn trực thuộc"},
                new{Id=2, Name="Phòng ban chức năng, Trung tâm nghiệp vụ"},
                new{Id=4, Name="Trường trực thuộc"},
                new{Id=5, Name="Trung tâm, Viện nghiên cứu"},
                new{Id=3, Name="Khác"}
            };
            _systemLogService.LogAudit("API Get department type");
            return Json(data.CreateResponse());
        }

        public JsonResult<ApiResponse> GetDepartments(int typeId = 0)
        {
            var lst = _departmentService.Gets();
            if (typeId > 0)
            {
                lst = lst.Where(i => i.TypeId == typeId);
            }
            var data = lst.Select(i => new
            {
                i.Id,
                i.Name,
                i.Address,
                i.Tel,
                i.Email
            }).OrderBy(i => i.Id).ToList();
            _systemLogService.LogAudit("API Get all department with typeId = " + typeId);
            return Json(data.CreateResponse());
        }
        public JsonResult<ApiResponse> GetFaculties()
        {
            var lst = _departmentService.Gets().Where(i => i.TypeId == 1)
            .Select(i => new
            {
                i.Id,
                i.Name,
                i.Address,
                i.Tel,
                i.Email
            }).OrderBy(i => i.Id).ToList();
            _systemLogService.LogAudit("API Get all faculty");
            return Json(lst.CreateResponse());
        }
        public JsonResult<ApiResponse> GetDivisions()
        {
            var lst = _departmentService.Gets().Where(i => i.TypeId == 2)
            .Select(i => new
            {
                i.Id,
                i.Name,
                i.Address,
                i.Tel,
                i.Email
            }).OrderBy(i => i.Id).ToList();
            _systemLogService.LogAudit("API Get all division");
            return Json(lst.CreateResponse());
        }
        public JsonResult<ApiResponse> GetSchools()
        {
            var lst = _departmentService.Gets().Where(i => i.TypeId == 4)
            .Select(i => new
            {
                i.Id,
                i.Name,
                i.Address,
                i.Tel,
                i.Email
            }).OrderBy(i => i.Id).ToList();
            _systemLogService.LogAudit("API Get all school");
            return Json(lst.CreateResponse());
        }
        public JsonResult<ApiResponse> GetInstitues()
        {
            var lst = _departmentService.Gets().Where(i => i.TypeId == 5)
            .Select(i => new
            {
                i.Id,
                i.Name,
                i.Address,
                i.Tel,
                i.Email
            }).OrderBy(i => i.Id).ToList();
            _systemLogService.LogAudit("API Get all institues");
            return Json(lst.CreateResponse());
        }
        public JsonResult<ApiResponse> GetCenters()
        {
            var lst = _departmentService.Gets().Where(i => i.TypeId == 5)
            .Select(i => new
            {
                i.Id,
                i.Name,
                i.Address,
                i.Tel,
                i.Email
            }).OrderBy(i => i.Id).ToList();
            _systemLogService.LogAudit("API Get all centers");
            return Json(lst.CreateResponse());
        }
    }
}
