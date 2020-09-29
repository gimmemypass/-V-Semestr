using _V_Semestr.Data.FileManager;
using _V_Semestr.Data.Repository;
using _V_Semestr.Models;
using _V_Semestr.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _V_Semestr.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PanelController : Controller
    {
        private IRepository _repo;
        private IFileManager _fileManager;

        public PanelController(
            IRepository repo,
            IFileManager fileManager 
            )
        {
            _repo = repo;
            _fileManager = fileManager;
        }
        public IActionResult Index()
        {
            
            var posts = _repo.GetAllPosts();
            return View(posts);
        }

        
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id == null)
                return View(new PostViewModel());
            else
            {
                var post = _repo.GetPost((int)id);
                return View(new PostViewModel
                {
                    Id = post.Id, 
                    Title = post.Title,
                    Content = post.Content,
                    Desciption = post.Desciption
                }) ;
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(PostViewModel vm)
        {
            var post = new Post
            {
                Id = vm.Id,
                Title = vm.Title,
                Content = vm.Content,
                Desciption = vm.Desciption,
                CoverImagePath = await _fileManager.SaveImage(vm.CoverImagePath)
            };
            if (post.Id > 0)
                _repo.UpdatePost(post);
            else
                _repo.AddPost(post);
            if (await _repo.SaveChangesAsync())
                return RedirectToAction("Index");
            else return View(post);
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            _repo.RemovePost(id);
            await _repo.SaveChangesAsync();
            return RedirectToAction("Index");
        }


    }
}
