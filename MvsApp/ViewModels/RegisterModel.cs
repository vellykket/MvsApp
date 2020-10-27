using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MvsApplication.ViewModels
{
    public class RegisterModel
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Недопустимая длина имени")]
        public string Firstname { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Недопустимая длина фамилии")]
        public string Lastname { get; set; }
        public string BalanceName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Remote(action: "VerifyEmail", controller: "Users", ErrorMessage = "Email уже используется")]    
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}