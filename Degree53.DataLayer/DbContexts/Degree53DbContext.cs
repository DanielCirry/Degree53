using Degree53.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Degree53.DataLayer.DbContexts
{
    public class Degree53DbContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostDetail> PostDetails { get; set; }
        public Degree53DbContext(DbContextOptions<Degree53DbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>()
                .HasKey(k => k.Id);

            modelBuilder.Entity<PostDetail>()
                .HasKey(k => k.Id);

            modelBuilder.Entity<Post>()
                .HasOne(d => d.PostDetail)
                .WithOne(p => p.Post)
                .HasForeignKey<PostDetail>(fk => fk.PostId);
        }
    }
}
