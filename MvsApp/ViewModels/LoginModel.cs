using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MvsApplication.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не указан Email")]
        [Remote(action: "VerifyLogin", controller: "UsersValidation", ErrorMessage = "Email не верный")]
        public string Email { get; set; }
         
        [Required(ErrorMessage = "Не указан пароль")]
        [Remote(action: "VerifyLogin", controller: "UsersValidation", ErrorMessage = "Password не верный")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}