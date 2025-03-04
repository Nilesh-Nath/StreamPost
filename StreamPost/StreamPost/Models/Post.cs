using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace StreamPost.Models
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string PublishedDate {  get; set; }
        public int LikeNumber {  get; set; }
        public int CommentNumber { get; set; }
        public string Id { get; set; }
        [ForeignKey("Id")]
        public User User { get; set; }
        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public Category Category { get; set; }
        public string? FeaturedImage { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
