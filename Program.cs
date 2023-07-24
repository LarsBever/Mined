using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Mined.DataAccess.Data;
using Mined.DataAccess.Repository;
using Mined.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<MinedDbContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("MinedDBConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<MinedDbContext>();

//Register Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
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

app.MapControllerRoute(
    name: "default",
    pattern: "{model=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
