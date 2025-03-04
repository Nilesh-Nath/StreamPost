using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StreamPost.Models;

namespace StreamPost.DataAccessLayer
{
    public class StreamPostDataAccess : IdentityDbContext<User>
    {
        public StreamPostDataAccess(DbContextOptions<StreamPostDataAccess> options) : base(options){ }
        public DbSet<Post> posts { get; set; }
        public DbSet<Comment> comments { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Like> likes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
