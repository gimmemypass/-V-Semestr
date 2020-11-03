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
    public class PaginatorViewComponent : ViewComponent
    {
        private IPostRepository _postRepository;

        public PaginatorViewComponent(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public IViewComponentResult Invoke(IndexViewModel vm)
        {
            return View("Default", vm);
        } 
    }
}
