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
        }

        [Route("/")]
        public IActionResult Index(string? category)
        {
            var posts = String.IsNullOrEmpty(category) ? _postRepo.GetAllPosts() : _postRepo.GetAllPosts(category);
            return View(posts);
        }

        public IActionResult Post(int id)
        {
            var post = _postRepo.GetPost(id);
            return View(post);
        }

        [HttpGet("/Image/{image}")]
        public IActionResult Image(string image)
        {
            var mime = image.Substring(image.LastIndexOf('.') + 1);
            return new FileStreamResult(_fileManager.ImageStream(image), $"image/{mime}");
        }
        

//        [HttpGet]
        //public IActionResult NewCategory()
        //{
        //    return View(new Category());
        //}
        //[HttpPost]
        //public async Task<IActionResult> NewCategory(Category category)
        //{
        //    _ctx.Categories.Add(category);
        //    await _ctx.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

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
