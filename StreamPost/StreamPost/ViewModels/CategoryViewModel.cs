using StreamPost.Models;

namespace StreamPost.ViewModels
{
    public class CategoryViewModel
    {
        public List<Post> Posts { get; set; }
        public List<Category> categories { get; set; }
        public Category CategoryName { get; set; }
        public int CategoryCount { get; set; }
    }
}
