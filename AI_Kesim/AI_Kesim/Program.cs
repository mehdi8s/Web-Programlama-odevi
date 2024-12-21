using AI_Kesim.Data;
using AI_Kesim.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<UserDetails, IdentityRole>(options =>
{
    options.Password.RequireDigit = false; // Rakam gereksinimi
    options.Password.RequiredLength = 3; // Minimum uzunluk
    options.Password.RequireNonAlphanumeric = false; // Özel karakter gereksinimi
    options.Password.RequireUppercase = false; // Büyük harf gereksinimi
    options.Password.RequireLowercase = false; // Küçük harf gereksinimi
}).AddEntityFrameworkStores<ApplicationDbContext>()
   .AddDefaultUI()
  .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
