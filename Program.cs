using Microsoft.EntityFrameworkCore;
using WebApplication1.Areas.Identity.Data;
using WebApplication1.Data;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("WebApplication1ContextConnection") ?? throw new InvalidOperationException("Connection string 'WebApplication1ContextConnection' not found.");

builder.Services.AddDbContext<WebApplication1Context>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<WebApplication1User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<WebApplication1Context>();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<TaskService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "RetrieveListItems",
    pattern: "List/RetrieveListItems/{listId}",
    defaults: new { controller = "List", action = "RetrieveListItems" });

app.MapRazorPages();

app.Run();
