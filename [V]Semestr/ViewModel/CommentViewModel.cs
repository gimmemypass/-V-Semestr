using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _V_Semestr.ViewModel
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        [Required]
        public int PostId { get; set; } 
        [Required]
        public int ParentCommentId { get; set; }
        [Required]
        public string Message { get; set; }
        public string Username { get; set; }
        public List<CommentViewModel> Children { get; set; } = new List<CommentViewModel>();
        public DateTime Created { get; set; }
    }
}
