using System;
using System.Collections.Generic;

namespace _V_Semestr.Models
{
    public class Category : Model
    {
        public Category()
        {
            Posts = new List<Post>();
        }

        public string Name { get; set;}
        public string Description { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
