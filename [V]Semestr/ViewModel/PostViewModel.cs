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
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; } 
        public string Desciption { get; set; } 
        public string Tags { get; set; } 
        public string CategoryId { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public string CurrentImage { get; set; } 
        public IFormFile CoverImagePath { get; set; } = null;

    }
}
