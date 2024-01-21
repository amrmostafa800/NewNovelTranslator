namespace WebApi.Responses;

public class OkResponse : Response
{
    protected override int StatusCode => 200;
    public override string Status => "Success";
}