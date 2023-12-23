using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data
{
	public class ApplicationDbContext : IdentityDbContext<CustomIdentityUser, IdentityRole<int>, int>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
			base(options)
		{
		}

        public DbSet<Novel> Novels { get; set; }
        public DbSet<NovelUsers> NovelUsers { get; set; }
    }
}
