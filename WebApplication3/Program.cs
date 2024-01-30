using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using WebApplication3.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AuthDbContext>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opts =>
{
	opts.Lockout.AllowedForNewUsers = true;
	opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);
	opts.Lockout.MaxFailedAccessAttempts = 3;
}).AddEntityFrameworkStores<AuthDbContext>(); 

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", option =>
{
    option.Cookie.Name = "MyCookieAuth";
});
builder.Services.AddHttpContextAccessor();


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

app.UseSession();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();


app.Run();
