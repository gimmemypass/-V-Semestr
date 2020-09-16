using System;
using System.Collections.Generic;

namespace _V_Semestr.Models
{
    public class Category : Model
    {
        public string Name { get; set;}
        public string Description { get; set; }
        public IList<Post> Posts { get; set; }
    }
}
