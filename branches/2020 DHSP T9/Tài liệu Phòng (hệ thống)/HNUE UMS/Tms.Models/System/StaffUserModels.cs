using System.ComponentModel.DataAnnotations;

namespace Ums.Models.System
{
    public class StaffUserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        [Required(ErrorMessage = "Tên đăng nhập không được bỏ trống")]
        public string Username { get; set; }
        public int StaffId { get; set; }
        public bool IsAdmin { get; set; }
        [EmailAddress(ErrorMessage = "Nhập sai định dạng email")]
        public string Email { get; set; }
    }
}