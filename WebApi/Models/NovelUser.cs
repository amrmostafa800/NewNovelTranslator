using WebApi.Data;

namespace WebApi.Models
{
	public class NovelUser
	{
		public int Id { get; set; }
		public int NovelCloneId { get; set; }
        public int UserId { get; set; }

		public Novel? NovelClone { get; set; }
		public CustomIdentityUser? User { get; set; }
	}
}
