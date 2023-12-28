using WebApi.Data;

namespace WebApi.Models
{
	public class NovelUser
	{
		public int Id { get; set; }
		public int NovelId { get; set; }
		public int UserId { get; set; }

		public Novel? Novel { get; set; }
		public CustomIdentityUser? User { get; set; }
	}
}
