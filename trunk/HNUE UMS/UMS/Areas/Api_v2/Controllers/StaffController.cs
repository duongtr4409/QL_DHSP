using System.Linq;
using System.Web.Http.Results;
using Ums.App;
using Ums.App.Base;
using Ums.Core.Entities.Shared;
using Ums.Services.Data;
using Ums.Services.Personnel;
using Ums.Services.System;

namespace Ums.Website.Areas.Api_v2.Controllers
{
    public class StaffController : BaseApiController
    {
        private readonly ISystemLogService _systemLogService;
        private readonly IPositionService _positionService;
        private readonly IDegreeService _degreeService;
        private readonly ITitleService _titleService;
        private readonly IStaffService _staffService;
        public StaffController()
        {
            _systemLogService = UnityConfig.Resolve<ISystemLogService>();
            _positionService = UnityConfig.Resolve<IPositionService>();
            _degreeService = UnityConfig.Resolve<IDegreeService>();
            _titleService = UnityConfig.Resolve<ITitleService>();
            _staffService = UnityConfig.Resolve<IStaffService>();
        }

        public JsonResult<ApiResponse> GetTitles()
        {
            var data = _titleService.Gets().Select(i => new { i.Id, i.Name }).OrderBy(i => i.Id).ToList();
            _systemLogService.LogAudit("API Get all titles");
            return Json(data.CreateResponse());
        }

        public JsonResult<ApiResponse> GetPositions()
        {
            var data = _positionService.Gets().Select(i => new { i.Id, i.Name }).OrderBy(i => i.Id).ToList();
            _systemLogService.LogAudit("API Get all positions");
            return Json(data.CreateResponse());
        }

        public JsonResult<ApiResponse> GetDegrees()
        {
            var data = _degreeService.Gets().Select(i => new { i.Id, i.Name }).OrderBy(i => i.Id).ToList();
            _systemLogService.LogAudit("API Get all degrees");
            return Json(data.CreateResponse());
        }

        public JsonResult<ApiResponse> GetStave(int departmentId = 0, int degreeId = 0, string keyword = "", int page = 1, int pagesize = 30, string startName = "")
        {
            var lst = _staffService.Gets(departmentId: departmentId, keyword: keyword, degreeId: degreeId);
            if (!string.IsNullOrEmpty(startName))
            {
                startName = startName.Trim().ToLower();
                var ids = lst.Where(i => i.Name.Contains(" " + startName)).Select(i => new { i.Id, i.Name }).ToList().Where(i => i.Name.Split(' ').Last().ToLower().StartsWith(startName)).Select(i => i.Id).ToArray();
                lst = lst.Where(i => ids.Contains(i.Id));
            }
            var data = lst.OrderBy(i => i.Id).Skip((page - 1) * pagesize).Take(pagesize).ToList().Select(i => new
            {
                i.Id,
                i.Name,
                i.TitleId,
                i.Code,
                i.DegreeId,
                i.DepartmentId,
                i.Gender,
                Department = i.Department?.Name,
                i.TeachingInId,
                PositionIds = i.Positions.Select(j => j.PositionId).ToArray(),
                i.IsRetired,
                Email = i.Accounts.FirstOrDefault(j => !j.IsDeleted)?.Email ?? "",
                i.Birthday,
                i.IsMoved,
                i.IsProbation
            }).OrderBy(i => i.Id).ToList();
            _systemLogService.LogAudit("API Get all staff");
            return Json(data.CreateResponse(total: lst.Count()));
        }

        public JsonResult<ApiResponse> GetStaff(int id)
        {
            var staff = _staffService.Get(id);
            if (staff == null)
            {
                return Json(false.CreateResponse(_message: "Không tìm thấy thông tin cán bộ!"));
            }
            _systemLogService.LogAudit("API Get staff staff info: " + staff.Name);
            return Json(new
            {
                staff.Id,
                staff.Name,
                staff.Code,
                staff.Gender,
                staff.Birthday,
                staff.TitleId,
                staff.DegreeId,
                staff.IsMoved,
                staff.IsRetired,
                staff.IsProbation,
                staff.DepartmentId,
                staff.TeachingInId,
                Email = staff.Accounts?.FirstOrDefault(j => !j.IsDeleted)?.Email ?? "",
                PositionIds = staff.Positions?.Select(i => i.PositionId).ToArray()
            }.CreateResponse());
        }
    }
};