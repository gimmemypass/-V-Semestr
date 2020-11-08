using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _V_Semestr.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Поле не может быть пустым")]
        public string UserName { get; set; } 

        [Required(ErrorMessage = "Поле не может быть пустым" )]
        [DataType(DataType.EmailAddress, ErrorMessage ="Неверно указан Email" )]
        public string Email { get; set; } 

        [Required(ErrorMessage ="Поле не может быть пустым")]
        [DataType(DataType.Password, ErrorMessage ="Неверный формат пароля(минимум длина - 6)")]
        public string Password { get; set; } 

        [Required(ErrorMessage = "Поле не может быть пустым")]
        [DataType(DataType.Password, ErrorMessage ="Неверный формат пароля(минимум длина - 6)")]
        [Compare("Password", ErrorMessage = "Пароли должны совпадать")]
        public string ConfirmPassword { get; set; } 

        [Required(ErrorMessage ="Поле не может быть пустым")]
        public string FirstName { get; set; }

        [Required(ErrorMessage ="Поле не может быть пустым")]
        public string SecondName { get; set; }

    }
}
