using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
	public class NovelName
	{
		public int Id { get; set; }

		[Column(TypeName = "VARCHAR"), MaxLength(256)]
		public required string novelName { get; set; }
	}
}
