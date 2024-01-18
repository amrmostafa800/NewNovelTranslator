using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Extensions;
using WebApi.Responses;
using WebApi.Services;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NovelUser : ControllerBase
{
    private readonly NovelService _novelService;
    private readonly NovelUserService _novelUserService;

    public NovelUser(NovelService novelService, NovelUserService novelUserService)
    {
        _novelService = novelService;
        _novelUserService = novelUserService;
    }

    [HttpPost("AddNovelUser")]
    [Authorize]
    public async Task<IActionResult> AddNovelUser([FromBody] AddNovelUserDto addNovelUser)
    {
        if (string.IsNullOrEmpty(addNovelUser.UserName))
        {
            return new BadRequestResponse
            {
                Description = "UserName Cannot Been Null Or Empty"
            };
        }

        //Check If CurrentUser Have Permission To Novel
        var novel = _novelService.GetById(addNovelUser.NovelId);

        if (novel == null) // check if novel not exist
        {
            return new BadRequestResponse
            {
                Description = "No Novel With This Id"
            };
        }

        if (novel.UserId != User.GetCurrentUserId()) //check if current User Have Permission To Delete This Novel
        {
            return new BadRequestResponse
            {
                Description = "You Cant Add Novel User If You Not Owner Of It"
            };
        }

        if (await _novelUserService.AddNovelUser(addNovelUser.NovelId, addNovelUser.UserName))
        {
            return new OkResponse
            {
                Description = "Added"
            };
        }

        return new BadRequestResponse
        {
            Description = "You Already Have Permission On This Novel"
        };
    }
}