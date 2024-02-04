using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using WebApplication3.Model;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

// Inject IWebHostEnvironment
var env = builder.Environment;

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AuthDbContext>();

// Account Lockout
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opts =>
{
    opts.Lockout.AllowedForNewUsers = true;
    opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(10);
    opts.Lockout.MaxFailedAccessAttempts = 3;
}).AddEntityFrameworkStores<AuthDbContext>();

builder.Services.ConfigureApplicationCookie(Config =>
{
    Config.LoginPath = "/Login";
});

// Interact with current HTTP request and Response for Session
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Used to store session data in a distributed way
builder.Services.AddDistributedMemoryCache();

// Create Session & Session Lockout
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
});

// Cookie Authentication
builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", option =>
{
    option.Cookie.Name = "MyCookieAuth";
});

// Inject IWebHostEnvironment
builder.Services.AddSingleton<IWebHostEnvironment>(env);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!env.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseStatusCodePagesWithRedirects("/errors/{0}");

app.UseSession();


app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
