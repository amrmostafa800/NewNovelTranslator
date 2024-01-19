using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Extensions;
using WebApi.Responses;
using WebApi.Services;
using WebApi.Validators;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NovelUserController : ControllerBase
{
    private readonly NovelService _novelService;
    private readonly NovelUserService _novelUserService;
    private readonly IValidator<NovelUserDto> _novelUserValidator;

    public NovelUserController(NovelService novelService, NovelUserService novelUserService,
        IValidator<NovelUserDto> novelUserValidator)
    {
        _novelService = novelService;
        _novelUserService = novelUserService;
        _novelUserValidator = novelUserValidator;
    }

    
    [HttpGet("{novelId}")]
    [Authorize]
    public IActionResult GetNovelUserByNovelId(int novelId)
    {
        return Ok(_novelUserService.GetNovelUsersByNovelId(novelId));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddNovelUser([FromBody] NovelUserDto novelUser)
    {
        var validationResult = await _novelUserValidator.ValidateAsync(novelUser);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        //Check If CurrentUser Have Permission To Novel
        var novel = _novelService.GetById(novelUser.NovelId);

        if (novel == null) // check if novel not exist
        {
            return new BadRequestResponse
            {
                Description = "No Novel With This Id"
            };
        }

        if (novel.UserId != User.GetCurrentUserId())
        {
            return new BadRequestResponse
            {
                Description = "You Cant Add Novel User If You Not Owner Of This Novel"
            };
        }

        //Add NovelUser
        if (await _novelUserService.AddNovelUser(novelUser.NovelId, novelUser.UserName))
        {
            return new OkResponse
            {
                Description = "Added"
            };
        }

        return new BadRequestResponse
        {
            Description = "User Already Have Permission On This Novel"
        };
    }
    
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> RemoveNovelUser([FromBody] NovelUserDto novelUser)
    {
        var validationResult = await _novelUserValidator.ValidateAsync(novelUser);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        //Check If CurrentUser Have Permission To Novel
        var novel = _novelService.GetById(novelUser.NovelId);

        if (novel == null) // check if novel not exist
        {
            return new BadRequestResponse
            {
                Description = "No Novel With This Id"
            };
        }

        if (novel.UserId != User.GetCurrentUserId()) //check if current User Have Permission To Delete This NovelUser
        {
            return new BadRequestResponse
            {
                Description = "You Cant Remove Novel User If You Not Owner Of This Novel"
            };
        }

        if (novel.UserName == novelUser.UserName)
        {
            return new BadRequestResponse
            {
                Description = "You Cant Remove Yourself From Novel"
            };
        }

        //Remove NovelUser
        if (await _novelUserService.RemoveNovelUser(novelUser.NovelId, novelUser.UserName))
        {
            return new OkResponse
            {
                Description = "Removed"
            };
        }

        return new BadRequestResponse
        {
            Description = "User Already Dont Have Permission On This Novel"
        };
    }
}