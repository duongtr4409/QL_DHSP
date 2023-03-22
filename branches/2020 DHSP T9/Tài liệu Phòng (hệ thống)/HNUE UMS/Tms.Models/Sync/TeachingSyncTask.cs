using System.Collections.Generic;

namespace Ums.Models.Sync
{
    public class TeachingSyncModel
    {
        public string Data { get; set; }
    }

    public class PostGraduatedSyncModel
    {
        public List<TeachingSyncTask> Result { get; set; } = new List<TeachingSyncTask>();
    }

    public class TeachingSyncTask
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int GradeId { get; set; }
        public string Grade { get; set; }
        public int ConversionId { get; set; }
        public string Conversion { get; set; }
        public int DepartmentId { get; set; }
        public int ForDepartmentId { get; set; }
        public string ForDepartment { get; set; }
        public string Department { get; set; }
        public string Name { get; set; }
        public double LessionTime { get; set; }
        public string TeachingTime { get; set; }
        public string Class { get; set; }
        public int Size { get; set; }
        public string Paid { get; set; }
        public string Course { get; set; }
        public string Desc { get; set; }
        public string Invited { get; set; }
        public string InvitedLecturer { get; set; }
        public string Specializing { get; set; }
        public string InvitedPartner { get; set; }
        public string LinkedPartner { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public int StaffId { get; set; }
        public string Lecturer { get; set; }
        public int InvitedDegreeId { get; set; }
        public int SemesterId { get; set; }
        public string SyncId { get; set; }
        public bool IsOk => YearId > 0 && GradeId > 0 && ConversionId > 0 && DepartmentId > 0 && ForDepartmentId > 0 && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Lecturer);
        public string Message
        {
            get
            {
                var m = "";
                if (YearId == 0)
                {
                    m += "[Chưa có ID năm học]<br/>";
                }
                if (GradeId == 0)
                {
                    m += "[Chưa có ID hệ đào tạo]<br/>";
                }
                if (ConversionId == 0)
                {
                    m += "[Chưa có ID quy đổi nhiệm vụ]<br/>";
                }
                if (DepartmentId == 0)
                {
                    m += "[Chưa có ID khoa đào tạo]<br/>";
                }
                if (ForDepartmentId == 0)
                {
                    m += "[Chưa có ID khoa được đào tạo]<br/>";
                }
                if (string.IsNullOrEmpty(Name))
                {
                    m += "[Chưa có tên nhiệm vụ]<br/>";
                }
                if (string.IsNullOrEmpty(Lecturer))
                {
                    m += "[Chưa có giảng viên]";
                }
                return m;
            }
        }
    }

    public class StandardLogin
    {
        public string Token { get; set; }
    }

    public class StandardTask
    {
        public string Ma_khoa { get; set; }
        public string Ten_khoa { get; set; }
        public string Ma_he { get; set; }
        public string Ten_he { get; set; }
        public string Ky_hieu { get; set; }
        public string Ten_mon { get; set; }
        public string Ten_lop { get; set; }
        public string Ma_cb { get; set; }
        public string Ho_ten { get; set; }
        public string Hoc_ham { get; set; }
        public string Sy_so { get; set; }
        public string So_tiet { get; set; }
        public string Loai_tiet { get; set; }
        public string Loai_lop { get; set; }
    }
}
