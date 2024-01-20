using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Responses;

public class OkResponse : Response
{
    protected override int StatusCode => 200;
    public override string Status => "Success";
}