using System.ComponentModel.DataAnnotations;

namespace Ums.Models.System
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Username { get; set; }
        public int StaffId { get; set; }
        public bool IsAdmin { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}