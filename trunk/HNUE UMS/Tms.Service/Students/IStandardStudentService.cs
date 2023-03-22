using System.Collections.Generic;
using Ums.Core.Domain.Students;
using Ums.Models.Common;

namespace Ums.Services.Students
{
    public interface IStandardStudentService
    {
        StandardStudent GetStudent(string username);
        List<StandardFaculty> GetFaculties();
        TableResult<DiemNamHoc> GetDiemNamHoc(string nam_hoc, string khoa_hoc, int id_khoa, int start, int pagesize, int draw);
        string[] GetYears();
        string[] GetCourses();
        TableResult<StandardStudent> GetStudents(int start = 0, int pagesize = 30, int draw = 0);
    }
}