using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
	public class EntityName
	{
		public int Id { get; set; }

		[Column(TypeName = "VARCHAR"), MaxLength(256)]
		public required string EnglishName { get; set; }

		public string? ArabicName { get; set; }

		[AllowedValues('M','F')]
		public required char Gender { get; set; }

        public int NovelId { get; set; }

        public Novel? Novel { get; set; }
    }
}
