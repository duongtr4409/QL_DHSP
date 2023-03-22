using QLSDH.API.Common;
using QLSDH.API.Models;
using QLSDH.API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TEMIS.Model;
namespace QLSDH.API.Controllers
{
    [RoutePrefix("api/v1/teachers")]
    public class TeacherController : ApiController
    {
        private ApiResponse response;
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();

        [Route("getListTeachers")]
        public async Task<IHttpActionResult> GetTeacherById(int departmentId = 0, int yearId = 0)
        {
            var strmess = string.Empty;
            List<ResponseTeacher> result = new List<ResponseTeacher>();
            try
            {
                var re = Request;
                var hearder = re.Headers;
                if (hearder.Contains("token"))
                {
                    string token = hearder.GetValues("token").First();
                    var checktoken = _unitOfWork.GetRepositoryInstance<SysSetting>().GetFirstOrDefaultByParameter(o => o.SKey == "TOKEN");
                    if(checktoken == null)
                    {
                        strmess = "token fail";
                        return Ok(new ApiResponse(ApiResponseCode.VALIDATOR_CODE, null, strmess));
                    }
                    else
                    {
                        if(token == checktoken.Value)
                        {
                            var listGiangvien = _unitOfWork.GetRepositoryInstance<HocPhan_GiangVien>().GetListByParameter(x => x.Status == true && x.KhoaId == departmentId && x.YearId == yearId).ToList();
                            if (listGiangvien.Count > 0)
                            {
                                foreach (var item in listGiangvien)
                                {
                                    ResponseTeacher teacher = new ResponseTeacher
                                    {
                                        Id = item.Id,
                                        YearId = item.YearId.Value,
                                        GradeId = item.GradeId.Value,
                                        ConversionId = item.ConversionId.Value,
                                        Departmentid = item.Departmentid.Value,
                                        ForDepartmentId = item.ForDepartmentId.Value,
                                        Name = item.Name,
                                        LessionTime = (float)item.LessionTime,
                                        TeachingTime = item.TeachingTime,
                                        Class = item.Class,
                                        Size = item.Size.Value,
                                        Paid = item.Paid.Value,
                                        Course = item.Course,
                                        Desc = item.Desc,
                                        Invited = item.Invited.Value,
                                        Specializing = item.Specializing,
                                        InvitedPartner = item.InvitedPartner,
                                        LinkedPartner = item.LinkedPartner,
                                        SubjectName = item.SubjectName,
                                        InvitedDegreeId = item.InvitedDegreeId.Value,
                                        StaffId = item.StaffId.Value,
                                        SemesterId = item.SemesterId.Value
                                    };
                                    result.Add(teacher);
                                }
                            }
                            strmess = "true";
                            return Ok(new ApiResponse(ApiResponseCode.SUCCESS_CODE, result, strmess));
                        }
                        else
                        {
                            strmess = "invalid token";
                            return Ok(new ApiResponse(ApiResponseCode.VALIDATOR_CODE, null, strmess));
                        }
                    }
                }
                else
                {
                    strmess = "invalid token";
                    return Ok(new ApiResponse(ApiResponseCode.VALIDATOR_CODE, null, strmess));
                }
            }
            catch (Exception ex)
            {
                strmess = "false";
                return Ok(new ApiResponse(ApiResponseCode.EXCEPTION_CODE, null, strmess));
            }


        }
    }
}
