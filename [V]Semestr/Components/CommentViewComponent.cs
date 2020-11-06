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
            var comments = _postRepo.GetCommentsByPostId(vm.PostId);
            foreach(var c in comments)
            {
                if(c.ParentCommentId == vm.Id)
                {
                    vm.Children.Add(new CommentViewModel
                    {
                        Id = c.Id,
                        PostId = c.PostId,
                        ParentCommentId = c.ParentCommentId,
                        Message = c.Message,
                        Username = c.Username,
                        Created = c.Created
                    });
                }
            }
            var view = View("Default", vm);
            return view;
        }
    }
}
