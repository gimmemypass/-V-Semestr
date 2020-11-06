using _V_Semestr.Models.Identity;
using _V_Semestr.Services.Email;
using _V_Semestr.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _V_Semestr.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        private IEmailService _emailService;

        public AuthController(
           SignInManager<User> signInManager,
           UserManager<User> userManager,
           IEmailService emailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
        } 
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var result = await _signInManager.PasswordSignInAsync(vm.UserName, vm.Password, false, false);

            if (!result.Succeeded)
            {
                return View(vm);
            }
            var user = await _userManager.FindByNameAsync(vm.UserName);
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (isAdmin)
            {
                return RedirectToAction("Index", "Panel"); 
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home"); 
        }

        [HttpGet]
        public IActionResult Register ()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var user = new User
            {
                UserName = vm.UserName,
                Email = vm.Email,
                FirstName = vm.FirstName,
                SecondName = vm.SecondName
                
            };
            var result = await _userManager.CreateAsync(user, vm.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                //await _emailService.SendEmail(user.Email, "Welcom", "Thank you for registering!");
                return RedirectToAction("Index", "Home");
            }
            return View(vm);
        }

    }
}
