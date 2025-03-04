using StreamPost.Models;

namespace StreamPost.ViewModels
{
    public class HomeViewModel
    {
        public User user { get; set; }
        public List<Category> categories { get; set; }
        public List<Post> posts { get; set; }
        public Post latestpost { get; set; }
        public Post TechPosts { get; set; }
        public List<Post> MostLikes { get; set; }
        public List<Post> Sports {get;set;}
        public Post Politics {get;set;}
        public List<Post> Health { get; set; }
        public List<Post> Entertainment { get; set; }
        public Post Social { get; set; }

        public int LikeNumber { get; set; }
        public int PostNumber { get; set; }
    }
}
