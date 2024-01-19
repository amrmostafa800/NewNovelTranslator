using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Services;
using WebApi.Validators;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        
        var websiteUrl = builder.Configuration.GetValue<string>("Settings:WebsiteUrl") ?? throw new InvalidOperationException("CORS 'WebsiteUrl' not found.");
        var apiUrl = builder.Configuration.GetValue<string>("Settings:ApiUrl") ?? throw new InvalidOperationException("CORS 'WebsiteUrl' not found.");

        builder.Services.AddIdentityApiEndpoints<CustomIdentityUser>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddCors();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.SameSite = SameSiteMode.None;
            //options.Cookie.Domain = apiUrl.Replace("https://","").Replace("http://","");
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.HttpOnly = true;
        });

        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<NovelService>();
        builder.Services.AddScoped<NovelUserService>();
        builder.Services.AddScoped<NovelSharedService>();
        builder.Services.AddScoped<EntityNameService>();
        builder.Services.AddScoped<IValidator<CreateNovelDto>, NovelValidator>();
        builder.Services.AddScoped<IValidator<NovelUserDto>, NovelUserValidator>();
        builder.Services.AddScoped<IValidator<EntityNameDto>, EntityNameValidator>();

        var app = builder.Build();

        //// Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
        app.UseSwagger();
        app.UseSwaggerUI();
        //}

        app.MapIdentityApi<CustomIdentityUser>();

        app.UseHttpsRedirection();

        app.UseCors(x => x
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(origin => origin == websiteUrl)
            .AllowCredentials()
        );
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}