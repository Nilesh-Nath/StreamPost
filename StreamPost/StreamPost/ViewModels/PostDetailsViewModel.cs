using StreamPost.Models;

namespace StreamPost.ViewModels
{
    public class PostDetailsViewModel
    {
        public List<Category> categories { get; set; }
        public Post Post { get; set; }
        public List<Post> MostLikes { get; set; }
        public string UserID { get; set; }
        public bool IsLiked { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
