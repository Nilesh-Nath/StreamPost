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
    public class CreateBlogController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly StreamPostDataAccess _dataAccess;
        public CreateBlogController(StreamPostDataAccess dataAccess,SignInManager<User> signInManager)
        {
            _dataAccess = dataAccess;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> CreateBlog()
        {

            var categories = _dataAccess.categories.ToList();
            var modelView = new HomeViewModel
            {
                user = _signInManager.UserManager.GetUserAsync(User).Result,
                categories = categories
            };
            return View(modelView);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlog(CreatePostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                    ModelState.AddModelError("", error.ErrorMessage);
                }
            }

            if (ModelState.IsValid)
            {
                var blog = new Post
                {
                    Title = model.title,
                    Description = model.content,
                    PublishedDate = DateTime.Now.Date.ToString("yyyy-MM-dd"),
                    LikeNumber = 0,
                    CommentNumber = 0,
                    Id = _signInManager.UserManager.GetUserId(User),
                    CategoryID = model.category,
                    FeaturedImage = model.featuredImage
                };

                _dataAccess.posts.Add(blog);

                await _dataAccess.SaveChangesAsync();

                return RedirectToAction("Index","Home");
            }
            var categories = _dataAccess.categories.ToList();
            var homeViewModel = new HomeViewModel
            {
                user = await _signInManager.UserManager.GetUserAsync(User),
                categories = categories
            };

            return View(homeViewModel);
        }
    }
}
