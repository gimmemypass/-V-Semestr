using _V_Semestr.Models.Comments;
using System;
using System.Collections.Generic;

namespace _V_Semestr.Models
{
    public class Post : Model
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Desciption { get; set; }
        public string Tags { get; set; }
        public string CoverImagePath { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<MainComment> MainComments { get; set; }
        public DateTime PostedOn { get; set; }

        //public int Views{get;set;} = 0;
        //теги
    }
}
