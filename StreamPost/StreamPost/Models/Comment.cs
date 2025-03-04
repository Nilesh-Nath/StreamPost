using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StreamPost.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentID { get; set; }
        [Required]
        public string CommentText { get; set; }
        public string Id { get; set; }
        [ForeignKey("Id")]
        public User User { get; set; }
        public int PostID { get; set; }
        [ForeignKey("PostID")]
        public Post Post { get; set; }
    }
}
