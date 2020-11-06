using _V_Semestr.Data.FileManager;
using _V_Semestr.Data.Repository;
using _V_Semestr.Models;
using _V_Semestr.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private IPostRepository _postRepo;
        private ICategoryRepository _catRepo;
        private IFileManager _fileManager;

        public PanelController(
            IPostRepository blogRepo,
            ICategoryRepository cat,
            IFileManager fileManager 
            )
        {
            _postRepo = blogRepo;
            _catRepo = cat;
            _fileManager = fileManager;
        }
        public IActionResult Index()
        {

            var vm = new PanelViewModel
            {
                Posts = _postRepo.GetAllPosts(),
                Categories = _catRepo.GetAllCategories()
            };
            return View(vm);
        }

        
        [HttpGet]
        public IActionResult EditPost(int? id)
        {
            var vm = new PostViewModel();
            
            if(id != null)
            {
                var post = _postRepo.GetPost((int)id);
                vm.Categories = from cat in _catRepo.GetAllCategories()
                    select new SelectListItem 
                    {
                        Selected = (cat.Id == post.Category.Id),
                        Text = cat.Name, 
                        Value = cat.Id.ToString()
                    };
                vm.Id = post.Id;
                vm.Title = post.Title;
                vm.Content = post.Content;
                vm.Desciption = post.Desciption;
                vm.Tags = post.Tags;
                vm.CurrentImage = post.CoverImagePath;
            }
            else
            {
                vm.Categories = from cat in _catRepo.GetAllCategories()
                    select new SelectListItem 
                    {
                        Text = cat.Name, 
                        Value = cat.Id.ToString()
                    };

            }
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> EditPost(PostViewModel vm)
        {
            var post = new Post
            {
                Id = vm.Id,
                Title = vm.Title,
                Content = vm.Content,
                Desciption = vm.Desciption,
                //Category = _catRepo.GetCategory(int.Parse(vm.CategoryId)),
                CategoryId = int.Parse(vm.CategoryId),
                Tags = vm.Tags
            };
            if(vm.CoverImagePath == null)
            {
                post.CoverImagePath = vm.CurrentImage; 
            }
            else
            {
                if (!string.IsNullOrEmpty(vm.CurrentImage))
                    _fileManager.RemoveImage(vm.CurrentImage);
                post.CoverImagePath = await _fileManager.SaveImage(vm.CoverImagePath); 
            }
            if (post.Id > 0)
            {
                _postRepo.UpdatePost(post);
            }
            else
            {
                _postRepo.AddPost(post);
            }
            if (await _postRepo.SaveChangesAsync())
                return RedirectToAction("Index");
            else return View(post);
        }

        [HttpGet]
        public async Task<IActionResult> RemovePost(int id)
        {
            _postRepo.RemovePost(id);
            await _postRepo.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditCategory(int? id)
        {
            var vm = new CategoryViewModel();
            if(id != null)
            {
                var cat = _catRepo.GetCategory((int)id);
                vm.Id = cat.Id;
                vm.Name = cat.Name;
                vm.Description = cat.Description;
                //vm.Posts = cat.Posts;
            }
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> EditCategory(CategoryViewModel vm)
        {
            var cat = new Category
            {
                Id = vm.Id,
                Name = vm.Name,
                Description = vm.Description,
            };
            if (cat.Id > 0)
                _catRepo.UpdateCategory(cat);
            else
                _catRepo.AddCategory(cat);
            if (await _catRepo.SaveChangesAsync())
                return RedirectToAction("Index");
            else return View(cat);
        }

        [HttpGet]
        public async Task<IActionResult> RemoveCategory(int id)
        {
            _catRepo.RemoveCategory(id);
            await _catRepo.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ChangeShowingPost(int id)
        {
            var post = _postRepo.GetPost(id);
            post.Shown = !post.Shown;
            _postRepo.UpdatePost(post);
            await _postRepo.SaveChangesAsync();
            return RedirectToAction("Index");
        }


    }
}
