using AuthRazor.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Threading.Tasks;

namespace AuthRazor.Identity
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                if (userManager == null)
                {
                    await roleManager.CreateAsync(new IdentityRole("User"));
                    await roleManager.CreateAsync(new IdentityRole("Admin"));

                    var admin = new IdentityUser { UserName = "admin@htl.at", Email = "admin@htl.at" };
                    var user = new IdentityUser { UserName = "user@htl.at", Email = "user@htl.at" };

                    var result = await userManager.CreateAsync(admin, "admin");
                    await userManager.CreateAsync(user, "user");

                    await userManager.AddToRoleAsync(user, "User");
                    await userManager.AddToRolesAsync(admin, new[] { "User", "Admin" });
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}