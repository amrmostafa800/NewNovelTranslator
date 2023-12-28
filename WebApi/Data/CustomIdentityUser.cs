using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Data
{
	public class CustomIdentityUser : IdentityUser<int>
	{
		[Column(TypeName = "VARCHAR"), MaxLength(256)]
		public override string? Email { get => base.Email; set => base.Email = value; }

		[Column(TypeName = "VARCHAR"), MaxLength(256)]
		public override string? UserName { get => base.UserName; set => base.UserName = value; }

		[Column(TypeName = "VARCHAR"), MaxLength(256)]
		public override string? NormalizedUserName { get => base.NormalizedUserName; set => base.NormalizedUserName = value; }

		[Column(TypeName = "VARCHAR"), MaxLength(256)]
		public override string? NormalizedEmail { get => base.NormalizedEmail; set => base.NormalizedEmail = value; }

		[Column(TypeName = "VARCHAR"), MaxLength(256)]
		public override string? PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }

		[Column(TypeName = "VARCHAR"), MaxLength(256)]
		public override string? PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }

		[Column(TypeName = "VARCHAR"), MaxLength(256)]
		public override string? ConcurrencyStamp { get => base.ConcurrencyStamp; set => base.ConcurrencyStamp = value; }

		[Column(TypeName = "VARCHAR"), MaxLength(256)]
		public override string? SecurityStamp { get => base.SecurityStamp; set => base.SecurityStamp = value; }
	}
}
