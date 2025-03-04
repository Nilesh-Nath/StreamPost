using StreamPost.Models;

namespace StreamPost.ViewModels
{
    public class CreatePostViewModel
    {
        public string title { get; set; }
        public string? featuredImage { get; set; }
        public string content { get; set; }
        public int category { get; set; }
    }
}
