namespace WebApi.DTOs
{
	public class AddNovelUserDto
	{
        public int NovelId { get; set; }
        public required string UserName { get; set; }
    }
}
