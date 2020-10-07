using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using _V_Semestr.Models;
using _V_Semestr.Data;
using _V_Semestr.Data.Repository;
using _V_Semestr.Data.FileManager;
using _V_Semestr.Models.Comments;
using _V_Semestr.ViewModel;

namespace _V_Semestr.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private IPostRepository _postRepo;
        private IFileManager _fileManager;

        public HomeController(
            IPostRepository repo,
            IFileManager fileManager)
        {
            _postRepo = repo;
            _fileManager = fileManager;
       //     var comment = new MainComment();
        }

        [Route("/")]
        public IActionResult Index(int pageNumber, string? category)
        {
            if (pageNumber < 1)
                return RedirectToAction("Index", new { pageNumber = 1, category });
            var posts = String.IsNullOrEmpty(category) ?
                _postRepo.GetAllPosts(pageNumber) :
                _postRepo.GetAllPosts(category);
            return View(posts);
        }

        public IActionResult Post(int id)
        {
            var post = _postRepo.GetPost(id);
            return View(post);
        }

        [HttpGet("/Image/{image}")]
        [ResponseCache(CacheProfileName = "Monthly")]
        public IActionResult Image(string image)
        {
            var mime = image.Substring(image.LastIndexOf('.') + 1);
            return new FileStreamResult(_fileManager.ImageStream(image), $"image/{mime}");
        }
        
        [HttpPost]
        public async Task<IActionResult> Comment(CommentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Post",new { id = vm.PostId });
            }
            var post = _postRepo.GetPost(vm.PostId);
            if(vm.MainCommentId == 0)
            {
                post.MainComments = post.MainComments ?? new List<MainComment>();
                post.MainComments.Add(new MainComment
                {
                    Message = vm.Message,
                    Created = DateTime.Now,
                });
                _postRepo.UpdatePost(post);
            }
            else
            {
                var comment = new SubComment
                {
                    MainCommentId = vm.MainCommentId,
                    Message = vm.Message,
                    Created = DateTime.Now,
                };
                _postRepo.AddSubComment(comment); 
            }
            await _postRepo.SaveChangesAsync(); 
            return RedirectToAction("Post",new { id = vm.PostId });
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
