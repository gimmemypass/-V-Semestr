using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _V_Semestr.Models.Comments
{
    public class MainComment : Comment
    {
        public virtual List<SubComment> SubComments { get; set; }    
    }
}
