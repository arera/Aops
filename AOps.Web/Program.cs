using AOps.Infrastructure.DependencyInjection;
using Serilog;
using FluentValidation;
using AOps.Application;
using Microsoft.AspNetCore.Authentication.Cookies;



var builder = WebApplication.CreateBuilder(args);
//Configure Serilog from configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
   builder.Host.UseSerilog();

//builder.Services.RegisterInfra(builder.Configuration); // IConfiguration

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Index";        // Redirect for unauthenticated users
        options.LogoutPath = "/Auth/Logout";      // Optional logout endpoint
        options.AccessDeniedPath = "/Home/Index"; // Redirect when [Authorize] fails (403)
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Session timeout

        // ?? Disable appending ReturnUrl
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                // Redirect to custom login without ReturnUrl
                context.Response.Redirect("/Home/Index");
                return Task.CompletedTask;
            },
            OnRedirectToAccessDenied = context =>
            {
                // Redirect on 403 forbidden
                context.Response.Redirect("/Home/Index");
                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Optional: Session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);


builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterInfra(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseHealthChecks("/health");
app.UseRouting();
app.UseAuthentication();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
