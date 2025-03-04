using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StreamPost.Models;
using StreamPost.ViewModels;

namespace StreamPost.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public LoginController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) { return View(model); }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user == null)
            {
                ModelState.AddModelError("message", "Email not confirmed yet");
                return View(model);
            }

            //if (user != null)
            //{
            //    if (!await _userManager.IsEmailConfirmedAsync(user))
            //    {
            //        ModelState.AddModelError("", "Please confirm your email before logging in.");
            //        return View(model);
            //    }
            //}

            var password = await _userManager.CheckPasswordAsync(user, model.Password);
            if(password == false)
            {
                ModelState.AddModelError("message", "Invalid credentials");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid Login Attempt!");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login","Login");
        }
    }
}
