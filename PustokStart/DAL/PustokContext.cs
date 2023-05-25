using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PustokStart.Models;

namespace PustokStart.DAL
{
    public class PustokContext:IdentityDbContext
    {
        public PustokContext(DbContextOptions<PustokContext> options):base(options)
        {
            
        }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }  
        public DbSet<BookImage> BookImages { get; set; }
        public DbSet<BookTags> BookTags { get; set; }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<Feature>Features { get; set; }
        public DbSet<Setting>Settings{ get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<BookComment> BookComments { get; set; }    
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem>OrderItems { get; set; } 




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasKey(x => x.Id);
            modelBuilder.Entity<Setting>().HasKey(x => x.Key);
            modelBuilder.Entity<SubCategory>().Property(x=> x.Id).UseIdentityColumn();
            modelBuilder.Entity<SubCategory>().HasKey(x => new {x.Name, x.CategoryId});
            base.OnModelCreating(modelBuilder);
        }

    }
}
