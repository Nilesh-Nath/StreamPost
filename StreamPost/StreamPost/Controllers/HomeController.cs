using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StreamPost.Models;
using StreamPost.DataAccessLayer;
using StreamPost.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace StreamPost.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly StreamPostDataAccess _dataAccess;
        public HomeController(ILogger<HomeController> logger,SignInManager<User> signInManager, StreamPostDataAccess dataAccess)
        {
            _logger = logger;
            _signInManager = signInManager;
            _dataAccess = dataAccess;
        }

        [HttpGet]

        public async Task<IActionResult> Index()
        {
            var user = await _signInManager.UserManager.GetUserAsync(User);
            
            if (user == null)
            {
                _logger.LogWarning("User is signed in but returned null from UserManager.");
                return RedirectToAction("Login", "Login");
            }

            var categories = _dataAccess.categories.ToList();

            var latestPost = _dataAccess.posts
                .OrderByDescending(p => p.PostID) 
                .Include(p => p.User)
                .Include(p => p.Category)
                .FirstOrDefault(p => !string.IsNullOrEmpty(p.FeaturedImage)) 
                ?? _dataAccess.posts
                    .Where(p => !string.IsNullOrEmpty(p.FeaturedImage))
                    .OrderByDescending(p => p.PostID)
                    .Include(p => p.User)
                    .Include(p => p.Category)
                    .FirstOrDefault();

            var posts = _dataAccess.posts
                .OrderByDescending(p => p.PostID)
                .Include(p => p.User)
                .Include(p => p.Category)
                .Where(p => string.IsNullOrEmpty(p.FeaturedImage)) 
                .Take(4) 
                .ToList();

            var techPosts = _dataAccess.posts
                .OrderByDescending(p => p.PostID)
                .Include(p => p.User)
                .Include(p => p.Category)
                .Where(p => p.CategoryID == 1)
                .Where(p => !string.IsNullOrEmpty(p.FeaturedImage))
                .FirstOrDefault() ??
                _dataAccess.posts
                .Where(p => !string.IsNullOrEmpty(p.FeaturedImage))
                .Where(p => p.CategoryID == 1)
                .Include(p => p.User)
                .Include(p => p.Category)
                .FirstOrDefault();

            var mostLikes = _dataAccess.posts
                            .Include(p => p.User)
                            .Include(p => p.Category)
                            .Where(p => !string.IsNullOrEmpty(p.FeaturedImage))
                            .OrderByDescending(p => p.LikeNumber)
                            .Take(2)
                            .ToList() ??
                            _dataAccess.posts
                            .Where(p => !string.IsNullOrEmpty(p.FeaturedImage))
                            .OrderByDescending(p => p.LikeNumber)
                            .Include(p => p.User)
                            .Include(p => p.Category)
                            .Take(2)
                            .ToList();


            var sports = _dataAccess.posts
                .OrderByDescending(p => p.PostID)
                .Include(p => p.User)
                .Include(p => p.Category)
                .Where(p => p.CategoryID == 2)
                .Where(p => !string.IsNullOrEmpty(p.FeaturedImage))
                .Take(3)
                .ToList() ??
                _dataAccess.posts
                .Where(p => !string.IsNullOrEmpty(p.FeaturedImage))
                .Where(p => p.CategoryID == 2)
                .Include(p => p.User)
                .Include(p => p.Category)
                .Take(3)
                .ToList();

            var politics = _dataAccess.posts
                .OrderByDescending(p => p.PostID)
                .Include(p => p.User)
                .Include(p => p.Category)
                .Where(p => p.CategoryID == 3)
                .Where(p => !string.IsNullOrEmpty(p.FeaturedImage))
                .FirstOrDefault() ??
                _dataAccess.posts
                .Where(p => !string.IsNullOrEmpty(p.FeaturedImage))
                .Where(p => p.CategoryID == 3)
                .Include(p => p.User)
                .Include(p => p.Category)
                .FirstOrDefault();

            var social = _dataAccess.posts
                .OrderByDescending(p => p.PostID)
                .Include(p => p.User)
                .Include(p => p.Category)
                .Where(p => p.CategoryID == 4)
                .Where(p => !string.IsNullOrEmpty(p.FeaturedImage))
                .FirstOrDefault() ??
                _dataAccess.posts
                .Where(p => !string.IsNullOrEmpty(p.FeaturedImage))
                .Where(p => p.CategoryID == 4)
                .Include(p => p.User)
                .Include(p => p.Category)
                .FirstOrDefault();

            var entertainment = _dataAccess.posts
                .OrderByDescending(p => p.PostID)
                .Include(p => p.User)
                .Include(p => p.Category)
                .Where(p => p.CategoryID == 5)
                .Where(p => !string.IsNullOrEmpty(p.FeaturedImage))
                .Take(2)
                .ToList() ??
                _dataAccess.posts
                .Where(p => !string.IsNullOrEmpty(p.FeaturedImage))
                .Where(p => p.CategoryID == 5)
                .Include(p => p.User)
                .Include(p => p.Category)
                .Take(2)
                .ToList();

            var health = _dataAccess.posts
                .OrderByDescending(p => p.PostID)
                .Include(p => p.User)
                .Include(p => p.Category)
                .Where(p => p.CategoryID == 6)
                .Where(p => !string.IsNullOrEmpty(p.FeaturedImage))
                .Take(3)
                .ToList() ??
                _dataAccess.posts
                .Where(p => !string.IsNullOrEmpty(p.FeaturedImage))
                .Where(p => p.CategoryID == 6)
                .Include(p => p.User)
                .Include(p => p.Category)
                .Take(3)
                .ToList();


            if (_signInManager.IsSignedIn(User))
            {
                var viewModel = new HomeViewModel
                {
                    user = user,
                    categories = categories,
                    latestpost = latestPost,
                    posts = posts,
                    TechPosts = techPosts,
                    MostLikes = mostLikes,
                    Sports = sports,
                    Politics = politics,
                    Health = health,
                    Entertainment = entertainment,
                    Social = social
                };
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpGet]
        public async Task<IActionResult> PostDetails(int id)
        {

            var categories = _dataAccess.categories.ToList();
            var post =  _dataAccess.posts.Include(p=>p.User).Include(p=>p.Category).FirstOrDefault(p => p.PostID == id);
            var userID = _signInManager.UserManager.GetUserId(User);
            var IsLiked = _dataAccess.likes.Any(l => l.Id == userID && l.PostID == id);

            if (post == null)
            {
                return NotFound();
            }
            
            var categoryId = post.CategoryID;
            var mostLikes = _dataAccess.posts
                            .OrderByDescending(p => p.PostID)
                            .Include(p => p.User)
                            .Include(p => p.Category)
                            .Where(p => p.CategoryID == categoryId && p.PostID != id)
                            .Where(p => !string.IsNullOrEmpty(p.FeaturedImage))
                            .Take(2)
                            .ToList() ?? _dataAccess.posts
                            .Where(p => !string.IsNullOrEmpty(p.FeaturedImage))
                            .Where(p => p.CategoryID == categoryId && p.PostID != id)
                            .Include(p => p.User)
                            .Include(p => p.Category)
                            .Take(2)
                            .ToList();

            var comments = _dataAccess.comments
                            .OrderByDescending(c => c.CommentID)
                            .Where(c=>c.PostID == id)
                            .Include(c => c.User)
                            .Include(c => c.Post)
                            .Take(4)
                            .ToList();


            var postDetailViewModel = new PostDetailsViewModel
            {
                categories = categories,
                Post = post,
                MostLikes = mostLikes,
                UserID = userID,
                IsLiked = IsLiked,
                Comments = comments
            };

            return View(postDetailViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> LikePost(int PostId)
        {
            var Id = _signInManager.UserManager.GetUserId(User);
            if (Id == null)
            {
                return NotFound();
            }

            var post =  _dataAccess.posts.Find(PostId);
            if (post == null)
            {
                return Json(new { success = false, message = "Post not found" });
            }

            bool isLikedAlready = _dataAccess.likes.Any(l => l.PostID == PostId && l.Id == Id);

            if (isLikedAlready)
            {
                var like = _dataAccess.likes.FirstOrDefault(l => l.PostID == PostId && l.Id == Id);
                _dataAccess.likes.Remove(like);
                post.LikeNumber -= 1;
            }
            else
            {
                var model = new Like { PostID = PostId, Id = Id };
                _dataAccess.likes.Add(model);
                post.LikeNumber += 1;
            }

            _dataAccess.posts.Update(post);
            await _dataAccess.SaveChangesAsync();

            var likeNumber = _dataAccess.posts.Find(PostId).LikeNumber;

            return Json(new { success = true, isLiked = !isLikedAlready , likeNumber });
        }


        [HttpPost]
        public async Task<IActionResult> CommentPost(int PostID, string Id, string CommentText)
        {
            var post = _dataAccess.posts.Find(PostID);
            if (post == null)
            {
                return NotFound();
            }

            var commentModel = new Comment
            {
                CommentText = CommentText,
                Id = Id,
                PostID = PostID
            };

            post.CommentNumber += 1;
            _dataAccess.Update(post);
            _dataAccess.comments.Add(commentModel);
            await _dataAccess.SaveChangesAsync();

            var commentNumber = _dataAccess.posts.Find(PostID).CommentNumber;
            return new JsonResult(new
            {
                success = true,
                message = "Commented!",
                commentNumber
            });
        }

        [HttpGet]
        public IActionResult GetComments(int postID)
        {
            var comments = _dataAccess.comments
                .Where(c => c.PostID == postID)
                .OrderByDescending(c => c.CommentID)
                .Select(c => new
                {
                    userName = c.User.UserName,
                    commentText = c.CommentText,
                    user = c.User.Id
                })
                .ToList();

            var posts = _dataAccess.posts.Include(p=>p.User).FirstOrDefault(p => p.PostID == postID);
            if(posts == null)
            {
                return NotFound();
            }
            var postAuthor = posts.User.Id;

            return Json(new { success = true, comments , postAuthor });
        }


        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            var results = _dataAccess.posts
                .Where(p => p.Title.Contains(query))
                .Select(p => new { p.PostID, p.Title })
                .ToList();

            return Json(results);
        }

        public async Task<IActionResult> DeletePost(int PostID)
        {
            var post = _dataAccess.posts.Find(PostID);

            if(post == null)
            {
                return NotFound();
            }

            _dataAccess.posts.Remove(post);
            await _dataAccess.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> EditPost(int id)
        {
            var Categories = _dataAccess.categories.ToList();
            var post = _dataAccess.posts.Find(id);
            if(post == null)
            {
                return NotFound();
            }
            var model = new EditPostViewModel
            {
                categories = Categories,
                Post = post
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(EditPostViewModel model)
        {
            var Post = _dataAccess.posts.FirstOrDefault(p => p.PostID == model.PostID);

            if(Post == null)
            {
                return NotFound();
            }

            Post.Title = model.Title;
            Post.Description = model.Content;
            Post.FeaturedImage = model.FeaturedImage ?? Post.FeaturedImage;
            Post.CategoryID = model.CategoryID;

            _dataAccess.Update(Post);
            await _dataAccess.SaveChangesAsync();
            
            return RedirectToAction("PostDetails", "Home", new {id = Post.PostID});
        }

    }
}
