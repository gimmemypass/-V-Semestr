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
using _V_Semestr.ViewModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using _V_Semestr.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace _V_Semestr.Controllers
{
    public class HomeController : Controller
    {
        private IPostRepository _postRepo;
        private IFileManager _fileManager;
        private readonly UserManager<User> _userManager;
        private IHubContext<CommentHub> _hubContext;

        public HomeController(
            IPostRepository repo,
            IFileManager fileManager,
            UserManager<User> userManager,
            IHubContext<CommentHub> hubContext
            )

        {
            _postRepo = repo;
            _fileManager = fileManager;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        [Route("/")]
        public IActionResult Index(int pageNumber, string category, string search)
        {
            var vm = _postRepo.GetAllPosts(pageNumber, category, search);
            vm.Posts = vm.Posts.Where(p => p.Shown);
            return View(vm);
        }

        public async Task<IActionResult> Post(int id)
        {
            var currentUser = await GetCurrentUserAsync();
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
            vm.Created = DateTime.Now;
            if (!ModelState.IsValid)
            {
                //return RedirectToAction("Post", new { id = vm.PostId });
                return BadRequest("Неправильно введены данные комментария");
            }
            var comment = new Comment
            {
                Message = vm.Message,
                Created = vm.Created,
                Updated = vm.Created,
                ParentCommentId = vm.ParentCommentId,
                Username = vm.Username,
                PostId = vm.PostId,
                Shown = true,
            };
            _postRepo.AddComment(comment);
            await _postRepo.SaveChangesAsync();
            vm.Id = comment.Id;
            await _hubContext.Clients.Group(vm.PostId.ToString())
                .SendAsync("ReceiveComment", new
                {
                    Message = vm.Message,
                    Created = vm.Created.ToString("dd/MM/yyyy hh:mm:ss"),
                    ParentCommentId = vm.ParentCommentId,
                    Username = vm.Username,
                    PostId = vm.PostId,   
                    Id = vm.Id
                });
            //return RedirectToAction("Post", new { id = vm.PostId });
            return Ok("tutto ok");

        }

        [HttpPost("[controller]/[action]/{connectionId}/{postId}")]
        public async Task<IActionResult> JoinCommentChat(string connectionId, string postId)
        {
            await _hubContext.Groups.AddToGroupAsync(connectionId, postId);
            return Ok();
        }

        [HttpPost("[controller]/[action]/{connectionId}/{postId}")]
        public async Task<IActionResult> LeaveCommentChat(string connectionId, string postId)
        {
            await _hubContext.Groups.RemoveFromGroupAsync(connectionId, postId);
            return Ok();
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
