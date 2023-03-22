using System.ComponentModel.DataAnnotations;

namespace Ums.Models.Security
{
    public class LoginModel
    {
        [Required]
        public string Type { get; set; }
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Capcha { get; set; }
    }
    public class RegisterModel
    {
        public string Capcha { get; set; }
        public string UserKey { get; set; }
        public string UserType { get; set; } = "GUEST";
        public string Avatar { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
