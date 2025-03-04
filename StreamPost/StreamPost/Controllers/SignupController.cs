using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StreamPost.DataAccessLayer;
using StreamPost.Models;
using StreamPost.Services;
using StreamPost.ViewModels;

namespace StreamPost.Controllers
{
    public class SignupController : Controller
    {
        private readonly StreamPostDataAccess _dataAccess;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        public SignupController(StreamPostDataAccess dataAccess,SignInManager<User> signInManager, UserManager<User> userManager,IEmailService emailService)
        {
            _dataAccess = dataAccess;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }
        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupViewModel model)
        {
            if (!ModelState.IsValid) { return View(model); }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email","Email already in use!");
                return View(model);
            }

            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth
            };

            var result = await _userManager.CreateAsync(user,model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            //if (result.Succeeded)
            //{
            //    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //    var confirmationLink = Url.Action("ConfirmEmail", "Signup", new {token,Email = model.Email},Request.Scheme);

            //    await _emailService.SendEmailAsync(user.Email, "Confirm Your Email",$"Click <a href='{confirmationLink}'>here</a> to confirm your email and proceed to login to your streapost account..👍😊");

            //    return RedirectToAction("ConfirmationLink", "Messages");
            //}

            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
                Console.WriteLine($"Error: {error.Description}");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            if (token == null || email == null)
            {
                return View("Error");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return View("Error");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Login", "Login");
            }

            return RedirectToAction("Error","Message");
        }
    }
}
