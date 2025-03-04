using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreamPost.DataAccessLayer;
using StreamPost.ViewModels;

namespace StreamPost.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly StreamPostDataAccess _dataAccess;
        public CategoryController(StreamPostDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        [HttpGet]
        public IActionResult Category(int id)
        {
            var categories = _dataAccess.categories.ToList();
            var posts = _dataAccess.posts
                        .OrderByDescending(p => p.PublishedDate)
                        .Where(p => p.CategoryID == id)
                        .Include(p => p.User)
                        .Include(p => p.Category)
                        .ToList();
           
            var category = _dataAccess.categories.Find(id);

            var categoryModel = new CategoryViewModel
            {
                categories = categories,
                Posts = posts,
                CategoryName = category
            };

            return View(categoryModel);
        }


        [HttpPost]
        public async Task<IActionResult> CustomCategory(string query)
        {
            var categories = _dataAccess.categories.ToList();

            var Posts = _dataAccess.posts
                        .Include(p=>p.User)
                        .Where(p => EF.Functions.Like(p.Title, $"%{query}%"))
                        .ToList();

            
            var model = new CategoryViewModel
            {
                categories = categories,
                Posts = Posts
            };

            return View(model);
        }
    }
}
