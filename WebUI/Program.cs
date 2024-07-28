using Data;
using Data.Entities;
using Data.Extensions;
using Data.Interfaces;
using Data.Services;
using JobScheduler;
using JobScheduler.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<IdContext>(n =>
    n.UseSqlServer(builder.Configuration.GetConnectionString("Db"),
    r => r.CommandTimeout((int)TimeSpan.FromMinutes(15).TotalSeconds)));
builder.Services.AddDbContext<Db>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Db"),
        opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(15).TotalSeconds)));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoleManager<RoleManager<ApplicationRole>>()
    .AddEntityFrameworkStores<IdContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/\\";
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
});


builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.Cookie.Name = "NicksonAssessment";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    options.LoginPath = "/Identity/Account/Login";
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.SlidingExpiration = true;
});


builder.Services.AddScoped<IMapperService, MapperService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IRepoService, RepoService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
                o.TokenLifespan = TimeSpan.FromMinutes(10));



builder.Services.AddSingleton<IJobFactory, JobFactory>();
builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
builder.Services.AddSingleton<JobRunner>();
builder.Services.AddHostedService<JobHostedService>();
builder.Services.AddScoped<EmailSenderJob>();
builder.Services.AddSingleton(new JobSchedule(
    jobType: typeof(EmailSenderJob),
    cronExpression: "0 0/1 * * * ?")); //every 10 seconds

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "ProjectArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

//using (var n = app.Services.CreateScope())
//{
//    var roleManager = n.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
//    var roles = new[] { "Admin", "Guest" };
//    //check role exist and create it if it doesn't exist
//    foreach (var role in roles)
//    {
//        if (!await roleManager.RoleExistsAsync(role))
//            await roleManager.CreateAsync(new ApplicationRole(role));
//    }
//}

app.Run();
