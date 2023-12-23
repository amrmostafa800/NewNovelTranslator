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

        public DbSet<NovelName> NovelNames { get; set; }
        public DbSet<NovelClone> NovelClones { get; set; }
        public DbSet<NovelUser> NovelUsers { get; set; }
    }
}
