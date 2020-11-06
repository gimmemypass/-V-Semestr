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
using System.Security.Claims;
using _V_Semestr.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace _V_Semestr.Controllers
{
    public class HomeController : Controller
    {
        private IPostRepository _postRepo;
        private IFileManager _fileManager;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(
            IPostRepository repo,
            IFileManager fileManager,
            UserManager<IdentityUser> userManager)
        {
            _postRepo = repo;
            _fileManager = fileManager;
            _userManager = userManager;
        }

        private Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        [Route("/")]
        public IActionResult Index(int pageNumber, string category, string search)
        {
            var vm = _postRepo.GetAllPosts(pageNumber, category, search);
            vm.Posts = vm.Posts.Where(p => p.Shown);
            return View(vm);
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
            var user = await GetCurrentUserAsync();
            vm.Username = user.UserName;
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Post", new { id = vm.PostId });
            }
            var comment = new Comment
            {
                Message = vm.Message,
                Created = DateTime.Now,
                Updated = DateTime.Now,
                ParentCommentId = vm.ParentCommentId,
                Username = vm.Username,
                PostId = vm.PostId,
                Shown = false,
            };
            //var post = _postRepo.GetPost(vm.PostId);
            //post.Comments = post.Comments ?? new List<Comment>();
            //post.Comments.Add(comment);
            //_postRepo.UpdatePost(post);
            _postRepo.AddComment(comment);
            await _postRepo.SaveChangesAsync();
            return RedirectToAction("Post", new { id = vm.PostId });

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
