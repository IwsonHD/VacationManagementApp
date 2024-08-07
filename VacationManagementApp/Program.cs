using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.DataBasesContext;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IVacationService, VacationService>();
builder.Services.AddTransient<IEmailService, EmailService>();  
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddDbContext<VacationManagerDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    options => options.MigrationsAssembly("BusinessLogic")
    ));
//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(30);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});
builder.Services.AddIdentity<User, IdentityRole>( o =>
{
    o.Password.RequireDigit = false;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequireUppercase = false;
    o.Password.RequiredLength = 0;
    o.Password.RequireLowercase = false;
    o.Password.RequireUppercase = false;

})
    .AddEntityFrameworkStores<VacationManagerDbContext>()
    .AddDefaultTokenProviders();



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
    pattern: "{controller=Home}/{action=Index}/{id?}");


using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Employee", "Employer" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

app.Run();
