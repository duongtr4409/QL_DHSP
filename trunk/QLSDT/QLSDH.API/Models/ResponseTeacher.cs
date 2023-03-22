using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLSDH.API.Models
{
    public class ResponseTeacher
    {
        //ID Nhiệm vụ giảng dạy
        public int Id { get; set; }
        //ID Năm học
        public int YearId { get; set; }
        //ID Hệ đào tạo
        public int GradeId { get; set; }
        //ID Quy đổi
        public int ConversionId { get; set; }
        //ID Đơn vị giảng dạy
        public int Departmentid { get; set; }
        //ID Đơn vị được giảng dạy
        public int ForDepartmentId { get; set; }
        //Tên nhiệm vụ giảng dạy
        public string Name { get; set; }
        //Thời lượng giảng dạy (tiết)
        public float LessionTime { get; set; }
        //Lịch giảng dạy
        public string TeachingTime { get; set; }
        //Tên lớp
        public string Class { get; set; }
        //Sĩ số lớp
        public int Size { get; set; }
        //Giờ giảng được trả tiền trực tiếp
        public bool Paid { get; set; }
        //Khoá
        public string Course { get; set; }
        //Mô tả
        public string Desc { get; set; }
        //Mời giảng
        public bool Invited { get; set; }
        //Chuyên ngành
        public string Specializing { get; set; }
        //Đơn vị mời giảng
        public string InvitedPartner { get; set; }
        //Đơn vị liên kết
        public string LinkedPartner { get; set; }
        //Tên môn học
        public string SubjectName { get; set; }
        //ID Học hàm học vị của giảng viên được mời
        public int InvitedDegreeId { get; set; }
        //ID Cán bộ giảng dạy (Nếu mời giảng thì = 0)
        public int StaffId { get; set; }
        //ID Học kỳ
        public int SemesterId { get; set; }

    }
}