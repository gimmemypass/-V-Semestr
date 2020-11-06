using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _V_Semestr.ViewModel
{
    public class LoginViewModel
    {
       [Required(ErrorMessage ="Поле не может быть пустым")]
       public string UserName { get; set; } 

       [Required(ErrorMessage ="Поле не может быть пустым")]
       [DataType(DataType.Password)]
       public string Password { get; set; } 
    }
}
