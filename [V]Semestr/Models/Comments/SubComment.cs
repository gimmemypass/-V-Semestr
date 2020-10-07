using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _V_Semestr.Models.Comments
{
    public class SubComment : Comment
    {
       public virtual int MainCommentId { get; set; } 
    }
}
