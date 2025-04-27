using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;
using SharedModels.Models.Enums;

namespace TestMAUIAppApi.Data
{
    public class Seeder
    {
        public static async Task Seed(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, DataContext context)
        {
            if (!context.Roles.Any())
                await AddRoleAsync(roleManager, context);


            if (!context.Users.Any())
                await AddUserAsync(userManager, context);

            if (!context.Authors.Any())
                await AddAuthorAsync(new Author { FirstName = "J.K.", LastName = "Rowling" }, context);

            if (!context.Editorials.Any())
                await AddEditorialAsync(new Editorial { Name = "Panini" }, context);

            if (!context.Books.Any())
            {
                var author = await context.Authors.FirstOrDefaultAsync();
                var editorial = await context.Editorials.FirstOrDefaultAsync();
                await AddBookAsync(author, editorial, context);
            }


            await context.SaveChangesAsync();

        }

        private static async Task AddBookAsync(Author? author, Editorial? editorial, DataContext context)
        {
            Book book = new()
            {
                Name = "Harry Potter",
                PublishingYear = "1992",
                Author = author,
                Editorial = editorial

            };
            await context.Books.AddAsync(book);
        }

        private static async Task AddEditorialAsync(Editorial editorial, DataContext context)
        {
            await context.Editorials.AddAsync(editorial);
            await context.SaveChangesAsync();
        }

        private static async Task AddAuthorAsync(Author author, DataContext context)
        {
            await context.Authors.AddAsync(author);
            await context.SaveChangesAsync();
        }

        private static async Task AddUserAsync(UserManager<User> userManager, DataContext context)
        {
            var user = await userManager.FindByEmailAsync("");
            if (user == null)
            {
                user = new()
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    Email = "admin@gmail.com",
                    UserName = "admin@gmail.com"

                };

                var result = await userManager.CreateAsync(user, "Admin1234!");
                if (result.Succeeded)
                {
                    // Ensure role is added after user creation
                    await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                }
            }
        }

        private static async Task AddRoleAsync(RoleManager<IdentityRole> roleManager, DataContext context)
        {
            foreach (var role in Enum.GetValues<Roles>())
            {
                if (!await roleManager.RoleExistsAsync(role.ToString()))
                {
                    await roleManager.CreateAsync(new IdentityRole(role.ToString()));
                }
            }

        }
    }
}
