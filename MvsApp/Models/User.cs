using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvsApplication.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Недопустимая длина имени")]
        public string Firstname { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Недопустимая длина фамилии")]
        public string Lastname { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Remote(action: "VerifyEmail", controller: "Users", ErrorMessage = "Email уже используется")]    
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        public virtual ICollection<Account> Accounts { get; set; }

        public override string ToString()
        {
            return $"Name - {Firstname}, LatsName - {Lastname}";
        }
    }
}