
using BillingApp.Data;
using BillingApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using MediatR;
using BillingApp.Web.Middleware;
using BillingApp.Handlers.Authentication.Handlers;
using BillingApp.Handlers.Users.Handlers;
using BillingApp.Handlers.Products.Handlers;
using BillingApp.Web.Services;
using DinkToPdf.Contracts;
using DinkToPdf;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Serilog configuration
builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
});


//pdf services
//builder.Services.AddSingleton<IConverter, BasicConverter>();
//builder.Services.AddSingleton<PdfService>();
//builder.Services.AddSingleton<ViewRenderService>();

// Database connection
builder.Services.AddDbContext<BillingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<BillingDbContext>()
    .AddDefaultTokenProviders();

// Authentication & Authorization
builder.Services.AddAuthentication()
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireSuperAdminRole", policy =>
        policy.RequireRole("SuperAdmin"));
});

// Session Configuration
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// MediatR for CQRS
builder.Services.AddMediatR(typeof(RegisterUserCommandHandler).Assembly);
builder.Services.AddMediatR(typeof(LoginUserHandler).Assembly);
builder.Services.AddMediatR(typeof(GetProductByIdHandler).Assembly);
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetProductByIdHandler).Assembly));



var app = builder.Build();

// Seed SuperAdmin & Roles
await DbSeeder.SeedRolesAndSuperAdmin(app.Services);

// Configure Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSerilogRequestLogging(); // Logging Middleware

app.UseSession(); // Ensure session is initialized before authentication
app.UseMiddleware<SessionRoleMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

// Default Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();

//using BillingApp.Data;
//using BillingApp.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Serilog;
//using MediatR;
//using BillingApp.Web.Middleware;
//using BillingApp.Handlers.Authentication.Handlers;
//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllersWithViews();

////serilogger
//builder.Host.UseSerilog((context, config) =>
//{
//    config.ReadFrom.Configuration(context.Configuration);
//});

////database
//builder.Services.AddDbContext<BillingDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

////configure identity
//builder.Services.AddIdentity<User, IdentityRole>()
//    .AddEntityFrameworkStores<BillingDbContext>()
//    .AddDefaultTokenProviders();

////authentication
//builder.Services.AddAuthentication()
//    .AddCookie("Cookies", options =>
//    {
//        options.LoginPath = "/Auth/Login";
//        options.AccessDeniedPath = "/Auth/AccessDenied";
//    });

////session
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(30);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.LoginPath = "/Account/Login";
//    options.AccessDeniedPath = "/Account/AccessDenied";
//});

////mediators
//builder.Services.AddMediatR(typeof(Program).Assembly);
//builder.Services.AddMediatR(typeof(LoginUserHandler).Assembly);

////sessions
//builder.Services.AddDistributedMemoryCache();

////authorization
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("RequireSuperAdminRole", policy =>
//        policy.RequireRole("SuperAdmin"));
//});

//var app = builder.Build();
//await DbSeeder.SeedRolesAndSuperAdmin(app.Services);

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseSession(); // Ensure session is before authentication
//app.UseMiddleware<SessionRoleMiddleware>();
//app.UseAuthentication();
//app.UseAuthorization();
//app.UseSerilogRequestLogging();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Auth}/{action=Login}/{id?}");

//app.Run();




//using BillingApp.Data;
//using BillingApp.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Serilog;
//using MediatR;
//using BillingApp.Web.Middleware;
//using BillingApp.Handlers.Authentication.Handlers;
//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllersWithViews();

////serilogger
//builder.Host.UseSerilog((context, config) =>
//{
//    config.ReadFrom.Configuration(context.Configuration);
//});

////database
//builder.Services.AddDbContext<BillingDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

////configure identity
//builder.Services.AddIdentity<User, IdentityRole>()
//    .AddEntityFrameworkStores<BillingDbContext>()
//    .AddDefaultTokenProviders();

////authentication
//builder.Services.AddAuthentication()
//    .AddCookie("Cookies", options =>
//    {
//        options.LoginPath = "/Auth/Login"; // Redirect to Login page if not authenticated
//        options.AccessDeniedPath = "/Auth/AccessDenied"; // Redirect to Access Denied page
//    });
////session
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(30);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.LoginPath = "/Account/Login";
//    options.AccessDeniedPath = "/Account/AccessDenied";
//});

////mediators
////builder.Services.AddMediatR(typeof(CreateUserHandler).Assembly);
//builder.Services.AddMediatR(typeof(Program).Assembly);
//builder.Services.AddMediatR(typeof(LoginUserHandler).Assembly);


////sessions
//builder.Services.AddDistributedMemoryCache();

////authorization
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("RequireSuperAdminRole", policy =>
//        policy.RequireRole("SuperAdmin"));
//});


//var app = builder.Build();
//await DbSeeder.SeedRolesAndSuperAdmin(app.Services);

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();
//app.UseSession();
//app.UseMiddleware<SessionRoleMiddleware>();
//app.UseAuthentication();
//app.UseAuthorization();
//app.UseSerilogRequestLogging();



//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Auth}/{action=Login}/{id?}");

//app.Run();


