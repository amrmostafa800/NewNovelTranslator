
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Services;
using WebApi.Validators;

namespace WebApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(connectionString));

			builder.Services.AddIdentityApiEndpoints<CustomIdentityUser>()
			.AddEntityFrameworkStores<ApplicationDbContext>();

			builder.Services.AddCors();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
				//options.Cookie.Domain = ""; // need to set later maybe
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.HttpOnly = true;
            });

            builder.Services.AddAuthorization();
			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddScoped<NovelService>();
			builder.Services.AddScoped<EntityNameService>();
			builder.Services.AddScoped<IValidator<CreateNovelDto>, NovelValidator>();
			builder.Services.AddScoped<IValidator<EntityNameDto>, EntityNameValidator>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.MapIdentityApi<CustomIdentityUser>();

			app.UseHttpsRedirection();

            app.UseCors(x => x
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials()
            );
            app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
