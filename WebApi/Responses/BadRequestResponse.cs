namespace WebApi.Responses;

public class BadRequestResponse : Response
{
    protected override int StatusCode => 400;
    public override string Status => "Error";
}