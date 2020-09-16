using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using _V_Semestr.Models;
using _V_Semestr.Data;

namespace _V_Semestr.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private AppDbContext _ctx;
        public HomeController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult Edit()
        {
            return View(new Post());
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Post post)
        {
            _ctx.Posts.Add(post);
            await _ctx.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult NewCategory()
        {
            return View(new Category());
        }
        [HttpPost]
        public async Task<IActionResult> NewCategory(Category category)
        {
            _ctx.Categories.Add(category);
            await _ctx.SaveChangesAsync();
            return RedirectToAction("Index");
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
