using _V_Semestr.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _V_Semestr.ViewModel
{
    public class PanelViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
