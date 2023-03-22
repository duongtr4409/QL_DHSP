using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Gosol.QLKH.Models.QLKH
{
    public class FileDinhKemModel
    {
        public int FileDinhKemID { get; set; }
        public string TenFileGoc { get; set; }
        public string TenFileHeThong { get; set; }
        public int LoaiFile { get; set; } //Loại file đính kèm
        public string FileUrl { get; set; }
        public DateTime? NgayTao { get; set; }
        public string NgayTaoStr { get; set; }
        public Boolean? CoBaoMat { get; set; }
        public int? NguoiTaoID { get; set; }
        public string TenNguoiTao { get; set; }
        public int? ThongTinVaoRaID { get; set; }
        public string Base64File { get; set; }
        //public int? FileVanBanOrNhiemVu { get; set; }
        public int LoaiUpload { get; set; } // 1 - scan (Base64File is not null); 2 - chụp ảnh
        public int NghiepVuID { get; set; }
        public int? CoQuanID { get; set; }
        public string NoiDung { get; set; }
        public string FolderPath { get; set; }
    }
}
