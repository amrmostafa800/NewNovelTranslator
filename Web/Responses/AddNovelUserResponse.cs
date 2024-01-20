namespace Web.Responses;

public class AddNovelUserResponse
{
    public required string status { get; set; }
    public required string description { get; set; }
    public int? id { get; set; }
}