using WebApi.Data;

namespace WebApi.Models
{
    public class NovelUsers
    {
        public int Id { get; set; }
        public int novelId { get; set; }
        public int userId { get; set; }

        public Novel? Novel { get; set; }
        public CustomIdentityUser? User { get; set; }
    }
}
