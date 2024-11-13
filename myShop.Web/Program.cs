using Microsoft.EntityFrameworkCore;
using myShop.DataAccess.Data;
using myShop.DataAccess.Implementation;
using myShop.Entities.Repository;
using Microsoft.AspNetCore.Identity;
using Utilities;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;

namespace myShop.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>
             (options => options.UseSqlServer(builder.Configuration.GetConnectionString("constr")));

            builder.Services.AddIdentity<IdentityUser,IdentityRole>()
                .AddDefaultUI()
                .AddEntityFrameworkStores<AppDbContext>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IEmailSender, EmailSender>();
           // builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddRazorPages();
            builder.Services.Configure<StripeData>(builder.Configuration.GetSection("Stripe"));
            
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();


			app.UseAuthentication();
            app.UseAuthorization();
            app.MapRazorPages();

            app.MapControllerRoute(
               name: "customer",
               pattern: "{controller=Home}/{action=Index}/{id?}",
               defaults: new { area = "Customer" });

            app.MapControllerRoute(
               name: "default",
               pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


            app.Run();
        }
    }
}
