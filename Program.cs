using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Mined.DataAccess.Data;
using Mined.DataAccess.Repository;
using Mined.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Mined.Utility;
using Mined.Models;
using Microsoft.AspNet.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<MinedDbContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("MinedDBConnection")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<MinedDbContext>().AddDefaultTokenProviders();
builder.Services.AddSingleton<IEmailSender, EmailSender>();

//Seed Admin account
//builder.Services.AddDefaultIdentity<IdentityUser>().AddRoles<IdentityRole>()
//            .AddEntityFrameworkStores<MinedDbContext>();
//builder.Services.AddSingleton<SeedAdminAccount, SeedAdminAccount>();
//builder.SeedAdminAccount.SeedUsers(IdentityUser);

//Register Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Add session 
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{model=Home}/{action=Index}/{id?}");

app.MapRazorPages();

//using (var scope = app.Services.CreateScope())
//{
//	var services = scope.ServiceProvider;
//	var userManager = services.GetRequiredService(Usermanager<userManager>);
//	var admin = await userManager.FindByEmailAsync("admin@admin.com");
//	if (admin != null)
//	{
//		if (!await userManager.IsInRoleAsync(admin, "Admin"))
//			await userManager.AddToRoleAsync(admin, "Admin");
//	}
//}

app.Run();
