using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Core.Domain.Students;

namespace Ums.Services.Students
{
    public interface IStandardStudentService
    {
        StandardStudent GetStudent(string username);
        List<StandardFaculty> GetFaculties();
        List<DiemNamHoc> GetDiemNamHoc(string nam_hoc, string khoa_hoc, string id_khoa);
        string[] GetYears();
        string[] GetCourses();
    }
}