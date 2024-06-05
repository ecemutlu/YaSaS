using DataAccessLayer.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using UserInterface.Controllers;
using UserInterface.Data;
using UserInterface.Models;
using UserInterface.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("conn");

// DbContext'ler için SQL Server bağlantısı kuruluyor
builder.Services.AddDbContext<mydbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));

// Eklenen HttpClient servisi
builder.Services.AddHttpClient<CityTownService>(client =>
{
	client.BaseAddress = new Uri("https://turkiyeapi.dev/"); // Base address ayarlanıyor
});
builder.Services.AddHttpClient<CityController>(client =>
{
    client.BaseAddress = new Uri("https://turkiyeapi.dev/");
});


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Identity servisleri CustomUser ve IdentityRole ile yapılandırılıyor
builder.Services.AddIdentity<CustomUser, IdentityRole>()
	.AddDefaultTokenProviders()
	.AddEntityFrameworkStores<ApplicationDbContext>();

// MVC ve Razor Pages servisleri ekleniyor
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddMvc();


// Kimlik doğrulama yapılandırması ekleniyor
builder.Services.AddAuthentication(options =>  // Cookie tabanlı kimlik doğrulama için varsayılan yapılandırma
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
})
.AddCookie(options =>  // Cookie ayarları
{
    options.LoginPath = "/Account/Login"; // Kullanıcı yetkisiz bir sayfaya erişmeye çalışırsa yönlendirilecek sayfa
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Cookie'nin geçerlilik süresi
});














var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

	app.UseMigrationsEndPoint();

}
else
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Kimlik doğrulama ve yetkilendirme middleware'leri ekleniyor
app.UseAuthentication();
app.UseAuthorization();

#pragma warning disable ASP0014 // Suggest using top level route registrations
app.UseEndpoints(endpoints =>
{
	endpoints.MapControllerRoute(
	  name: "areas",
	  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
	);
});
#pragma warning restore ASP0014 // Suggest using top level route registrations

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

