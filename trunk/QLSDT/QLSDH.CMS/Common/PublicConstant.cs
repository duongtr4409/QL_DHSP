namespace TEMIS.CMS.Common
{
    public class PublicConstant
    {
        public const string ROLE_ADMINSTRATOR = "Administrator";//1
        public const string ROLE_GIANG_VIEN_HD = "Giảng viên hướng dẫn"; // 2
        public const string ROLE_TRUONG_KHOA_DT = "Trưởng khoa đào tạo"; //3
        public const string ROLE_CB_PHONG_SDH = "Cán bộ quản lý phòng SĐH"; //4
        public const string ROLE_NCS = "Nghiên cứu sinh"; // 5
        public const string ROLE_CB_PHONG_TAI_CHINH = "Cán bộ phòng Tài Chính"; // 6
        public const string ROLE_CB_THU_VIEN = "Cán bộ Thư Viện"; // 7
        public const string TaiLieuTuyenSinh = "TaiLieu";

        public const string LOGIN_INFO = "LOGIN_INFO";
        public const string ROLE_INFO = "ROLE_INFO";

        public const string BM_GIANGVIEN = "Giảng viên";
        public const string BM_TEXT = "Text";
        public const string BM_NUMBER = "Số";
        public const string BM_DATETIME = "Ngày tháng";
        public const string BM_BANG = "Bảng danh sách";

        //Trạng thái tuyển sinh
        public const int STT_CHODUYET = 0;
        public const int STT_DUYET = 1;
        public const int STT_XETTUYEN = 2;
        public const int STT_TRUOT = 3;

        //Trạng thái hồ sơ tuyển sinh
        public const int STT_DANGKYMOI = 0;
        public const int STT_DANGXULY = 1;
        public const int STT_DADUYET = 2;
        public const int STT_HUY = 3;

        //Trạng thái Trường tt NCS
        public const int STT_NCS_TAILAI = -1;
        public const int STT_NCS_CHODUYET = 0;
        public const int STT_NCS_DADUYET = 1;

        //Trạng thái học phí
        public const int CHUA_NOP = -1;
        public const int CHO_DUYET = 0;
        public const int DA_NOP = 1;
        public const int NOP_LOI = 2;

        //Vai trò biểu mẫu
        public const string VT_CHUTICH = "Chủ tịch Hội đồng";
        public const string VT_PHOCHUTICH = "Phó chủ tịch HĐ";
        public const string VT_UVTHUONGTRUC = "Ủy viên thường trực";
        public const string VT_UV = "Ủy viên";
        public const string VT_UVTHUONGTRUC_TK = "UVTT, Trưởng ban";


        // Mật khẩu mặc định tạo tài khoản
        public const string PASSWORD_DEFAULT = "123456";

        //Loại trường thông tin trong mục thông tin
        public const string DM_UPLOAD = "Upload";
        public const string DM_CHECKBOX = "Checkbox";
        public const string DM_TEXT = "Text";

        //Loại học phần tương ứng 
        public const int HP1 = 1; // học phần 1
        public const int HP2 = 2;
        public const int HP3 = 3;
        public const int HP4 = 4;

        //status đợt tuyển sinh
        public const int DANGTUYENSINH = 1; // đang tuyển sinh mở đợt đó
        public const int DONGTUYENSINH = 0; // kết thúc tuyển sinh k mở đc 
    }

    public enum CMS_STATUS
    {
        ACTIVE = 1,
        PENDING = 2,
        DELETED = 3
    }
    public enum CMS_IMAGE_TYPE
    {
        ARTICLE_TYPE = 0,
        PRODUCT_TYPE
    }
}