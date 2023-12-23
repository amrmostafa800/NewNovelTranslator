namespace WebApi.Responses
{
	public class ErrorResponse : Response
	{
		protected override int statusCode => 400;
		public override string? Type => "Error";
	}
}
