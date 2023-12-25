using WebApi.Data;

namespace WebApi.Models
{
    public class Novel
    {
        public int Id { get; set; }
        public int NovelNameId { get; set; }

        public NovelName? NovelName { get; set; }
    }
}
