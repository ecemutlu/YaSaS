//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using DataAccessLayer;


//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<mydbContext>();

////Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

////builder.Services.AddDbContext<mydbContext>(options =>
////	options.UseSqlServer(connectionString));

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();



//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//	app.UseMigrationsEndPoint();
//}
//else
//{
//	app.UseExceptionHandler("/Home/Error");
//	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//	app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllerRoute(
//	name: "default",
//	pattern: "{controller=Home}/{action=Index}/{id?}");
//app.MapRazorPages();

//app.Run();
using DataAccessLayer.Context;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YaSaS_UserInterface.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<mydbContext>(
    options => options.UseSqlServer
    (builder.Configuration.GetConnectionString("conn")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<mydbContext>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//      name: "areas",
//      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
//    );
//});

app.Run();

