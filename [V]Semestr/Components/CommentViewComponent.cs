using _V_Semestr.Data.Repository;
using _V_Semestr.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _V_Semestr.Components
{
    public class CommentViewComponent : ViewComponent
    {
        private IPostRepository _postRepo;

        public CommentViewComponent(IPostRepository postRepo)
        {
            _postRepo = postRepo;
        }

        public IViewComponentResult Invoke(CommentViewModel vm)
        {
            var view = View("Default", vm);
            return view;
        }
    }
}
