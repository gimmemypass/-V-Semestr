using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _V_Semestr.Models
{
    public class Comment : Model
    {
        public string Message { get; set; } 
        public int ParentCommentId { get; set; }
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
        public string Username { get; set; }
    }
}
