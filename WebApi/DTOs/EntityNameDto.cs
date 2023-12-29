using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs
{
	public class EntityNameDto
	{
		public required EntityNameDetails[] EntityNames { get; set; }
		public required int NovelId { get; set; }
	}

	public class EntityNameDetails
	{
		public required string EnglishName { get; set; }

		[AllowedValues('M', 'F')]
		public required char Gender { get; set; }
	}
}
