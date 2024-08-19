using BusinessLogic.DataBasesContext;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;
//using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:TokenKey"]);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddScoped<IVacationService, VacationService>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddDbContext<VacationManagerDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddAuthentication(options =>
{
    //this doesnt seem to be working at all...
    //i have to manually specify the scheme in everyendpoint
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],  // Ustawienie Issuer
        ValidAudience = builder.Configuration["Jwt:Audience"],  // Ustawienie Audience
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            var result = JsonSerializer.Serialize(new { message = context.Exception.Message });
            logger.LogInformation(result);
            return context.Response.WriteAsync(result);
        },
        OnTokenValidated = context =>
        {
            // Logowanie sukcesu walidacji tokenu
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Token validated successfully.");
            return Task.CompletedTask;
        }
    };


});

builder.Services.AddIdentity<User, IdentityRole>(o =>
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

//cors policy

builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientAPI",
        policy =>
        {
            var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
            policy
                .WithOrigins(allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseExceptionHandler(appError => {
//    appError.Run(async context => {
//        context.Response.StatusCode = 500;
//        context.Response.ContentType = "application/json";

//        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
//        if (contextFeature is not null)
//        {
//            await context.Response.WriteAsJsonAsync(new
//            {
//                context.Response.StatusCode,
//                Message = contextFeature.Error.Message

//            }.ToString());
//        }
//    });
//});

app.UseCors("ClientAPI");

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();



app.Run();
