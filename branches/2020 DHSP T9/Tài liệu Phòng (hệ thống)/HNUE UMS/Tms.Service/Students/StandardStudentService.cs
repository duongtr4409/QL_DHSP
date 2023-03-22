using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Ums.Core.Domain.Students;
using Ums.Models.Common;
using Ums.Services.System;

namespace Ums.Services.Students
{
    public class StandardStudentService : IStandardStudentService
    {
        private readonly ISettingService _settingService;
        public readonly IDbConnection Db;
        public StandardStudentService(ISettingService settingService)
        {
            _settingService = settingService;
            Db = new SqlConnection(_settingService.GetValue("HNUE.UNISOFT.DB.CONNECTION"));
        }

        public TableResult<StandardStudent> GetStudents(int start = 0, int pagesize = 30, int draw = 0)
        {
            var lst = Db.Query<StandardStudent>("select sv.id_sv as id, sv.ma_sv as Username, ds.mat_khau as [Password], sv.ngay_sinh as Birthday, sv.Ho_ten as Name, sv.Dienthoai_canhan as Phone, sv.Email, sv.NoiO_hiennay as Address from svHoSoSinhVien sv left join svDanhSach ds on sv.id_sv=ds.id_sv order by sv.id_sv desc OFFSET @start ROWS FETCH NEXT @pagesize ROWS ONLY", new { start, pagesize });
            var count = Db.QuerySingle<int>("select count(sv.id_sv) from svHoSoSinhVien sv left join svDanhSach ds on sv.id_sv=ds.id_sv");
            return lst.ToList().ToTableResult(count, draw);
        }

        public StandardStudent GetStudent(string username)
        {
            return Db.QuerySingle<StandardStudent>("select sv.id_sv as id, sv.ma_sv as Username, ds.mat_khau as [Password], sv.ngay_sinh as Birthday, sv.Ho_ten as Name, sv.Dienthoai_canhan as Phone, sv.Email, sv.NoiO_hiennay as Address from svHoSoSinhVien sv left join svDanhSach ds on sv.id_sv=ds.id_sv where sv.ma_sv =@ma_sv", new { ma_sv = username });
        }

        public string[] GetYears()
        {
            return Db.Query<string>("select distinct nam_hoc from svDiemNamHoc order by nam_hoc desc").ToArray();
        }

        public string[] GetCourses()
        {
            return Db.Query<string>("select distinct khoa_hoc from svLop order by khoa_hoc desc").ToArray();
        }

        public List<StandardFaculty> GetFaculties()
        {
            return Db.Query<StandardFaculty>("select id_khoa as id, ten_khoa as name from svkhoa order by ten_khoa").ToList();
        }

        public TableResult<DiemNamHoc> GetDiemNamHoc(string nam_hoc = "", string khoa_hoc = "", int id_khoa = 0, int start = 0, int pagesize = 30, int draw = 0)
        {
            var data = Db.Query<DiemNamHoc>(@"SELECT
		                                   sv.Ho_ten,
                                            sv.Ma_sv
                                          ,Nam_hoc
                                          ,TBCHT4
                                          ,TBCHT4_tich_luy
                                          ,Tong_so_hoc_trinh
                                          ,Tong_so_hoc_trinh_tich_luy
                                          ,Tong_so_hoc_trinh_thi_lai
                                          ,Tong_so_hoc_trinh_hoc_lai
                                          ,Tong_so_mon
                                          ,Tong_so_mon_thi_lai
                                          ,Tong_so_mon_hoc_lai
                                          ,xlht.Xep_loai as xep_loai_hoc_tap
                                          ,xltl.Xep_loai as xep_loai_tich_luy
                                      FROM svDiemNamHoc as diem inner join svHoSoSinhVien as sv on diem.ID_sv=sv.ID_sv
                                      inner join svXepLoaiHocTap as xlht on diem.ID_xep_loai4=xlht.ID_xep_loai
                                      inner join svXepLoaiHocTap as xltl on diem.ID_xep_loai_tich_luy4=xltl.ID_xep_loai
                                      inner join svDanhSach as ds on diem.ID_sv=ds.ID_sv
                                      where nam_hoc = @nam_hoc
                                      order by sv.Ho_ten
                                      OFFSET @start ROWS FETCH NEXT @pagesize ROWS ONLY", new { nam_hoc, khoa_hoc, id_khoa, start, pagesize }).ToList();
            var count = Db.QuerySingle<int>(@"SELECT count(*) FROM svDiemNamHoc where nam_hoc = @nam_hoc", new { nam_hoc });
            return data.ToTableResult(count, draw);
        }
    }
}
