using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace StreamPost.Models
{
    public class Like
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LikeID { get; set; }

        [NotNull]
        public string Id { get; set; }
        [ForeignKey("Id")]
        public User User { get; set; }
        [NotNull]
        public int PostID { get; set; }
        [ForeignKey("PostID")]
        public Post Post { get; set; }
    }
}
