using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreamPost.DataAccessLayer;
using StreamPost.Models;
using StreamPost.ViewModels;

namespace StreamPost.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly StreamPostDataAccess _dataAccess;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public ProfileController(StreamPostDataAccess dataAccess,SignInManager<User> signInManager,UserManager<User> userManager)
        {
            _dataAccess = dataAccess;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var categories =  _dataAccess.categories.ToList();
            var user = await _signInManager.UserManager.GetUserAsync(User);

            var totalPosts = _dataAccess.posts.Count(p => p.Id == user.Id);
            var totalLikes = _dataAccess.posts
                                .Where(p => p.Id == user.Id)
                                .Sum(p => p.LikeNumber);
            var posts = _dataAccess.posts.Where(p => p.Id == user.Id).ToList();

            var model = new HomeViewModel
            {
                categories = categories,
                user = user,
                PostNumber = totalPosts,
                LikeNumber = totalLikes,
                posts = posts
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
        {
            var user = await _signInManager.UserManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.DateOfBirth = model.DateOfBirth;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Profile","Profile");
            }
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error.Description); 
                    ModelState.AddModelError("", error.Description);
                }
            }


            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return RedirectToAction("Profile", "Profile");
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePassword(EditPasswordViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found." });
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
            if (!isPasswordCorrect)
            {
                return Json(new { success = false, field = "CurrentPassword", message = "Current password is incorrect." });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return Json(new { success = false, message = "Validation failed.", errors = errors });
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                return Json(new { success = true, message = "Password updated successfully." });
            }
            else
            {
                var errorDict = result.Errors
                    .GroupBy(e => string.Empty)
                    .ToDictionary(
                        g => "Password",
                        g => g.Select(e => e.Description).ToArray()
                    );

                return Json(new { success = false, message = "Failed to update password.", errors = errorDict });
            }
        }
    }
}
