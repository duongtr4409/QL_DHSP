using Hnue.Helper;
using System.Web.Mvc;
using Ums.App.Base;
using Ums.App.Security;
using Ums.Models.Common;
using Ums.Services.Students;

namespace Ums.Website.Controllers
{
    public class StudentController : BaseController
    {
        private readonly IStandardStudentService _standardStudentService;

        public StudentController(IStandardStudentService standardStudentService)
        {
            _standardStudentService = standardStudentService;
        }

        [Function("STANDARD_STUDENT_REPORT")]
        public ActionResult Report(int facultyId = 0, string course = "", string year = "")
        {
            ViewBag.Year = year;
            ViewBag.FacultyId = facultyId;
            ViewBag.Course = course;
            ViewBag.Years = _standardStudentService.GetYears();
            ViewBag.Faculties = _standardStudentService.GetFaculties();
            ViewBag.Courses = _standardStudentService.GetCourses();
            return View();
        }

        [Function("STANDARD_STUDENT_REPORT")]
        public object GetReport(TableModel model, int facultyId = 0, string course = "", string year = "")
        {
            var s = _standardStudentService.GetDiemNamHoc(year, course, facultyId, model.Start, model.Pagesize, model.Draw);
            return s.ToJson();
        }
    }
}