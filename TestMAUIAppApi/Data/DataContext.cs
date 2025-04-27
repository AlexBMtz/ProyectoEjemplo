using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;

namespace TestMAUIAppApi.Data
{
    public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<User>(options)
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Editorial> Editorials { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Student> Students { get; set; }
    }
}
