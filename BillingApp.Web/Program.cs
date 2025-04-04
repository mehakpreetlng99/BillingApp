
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
using BillingApp.Mappings;

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
builder.Services.AddAutoMapper(typeof(MappingProfile)); // Or your mapping profile
builder.Services.AddMediatR (typeof(GetUsersByRoleQueryHandler).Assembly);
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

