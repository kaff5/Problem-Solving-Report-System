using Microsoft.EntityFrameworkCore;
using WEBBACK2.Models.UserDir;
using WEBBACK2.Models.TopicDir;
using WEBBACK2.Models.RoleDir;
using WEBBACK2.Models.TaskDir;

namespace WEBBACK2.Models.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Task1> Tasks { get; set; }
        public DbSet<Solution> Solutions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => u.userName)
                .IsUnique();

            builder.Entity<User>()
                .HasOne<Role>()
                .WithMany()
                .HasForeignKey(p => p.roleId)
                .HasPrincipalKey(t => t.roleId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Task1>()
                .HasOne<Topic>()
                .WithMany()
                .HasForeignKey(p => p.topicId)
                .HasPrincipalKey(t => t.id).OnDelete(DeleteBehavior.Cascade);


            builder.Entity<Topic>()
                .HasOne<Topic>()
                .WithMany()
                .HasForeignKey(p => p.parentId)
                .HasPrincipalKey(t => t.id);


            builder.Entity<Solution>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.authorId)
                .HasPrincipalKey(t => t.userId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Solution>()
                .HasOne<Task1>()
                .WithMany()
                .HasForeignKey(p => p.taskId)
                .HasPrincipalKey(t => t.id).OnDelete(DeleteBehavior.Cascade);
        }
    }
}