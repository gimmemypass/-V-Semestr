using _V_Semestr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _V_Semestr.ViewModel
{
    public class IndexViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
        public bool NextPage { get; set; }
        public int PageNumber { get; set; }
        public string Category { get; set; }
    }
}
