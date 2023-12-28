namespace WebApi.DTOs
{
	public class NovelDto
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public required string Name { get; set; }
		public required string UserName { get; set; }
	}
}
