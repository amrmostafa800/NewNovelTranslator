using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Enums;
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
    private readonly IValidator<AddNovelUserDto> _novelUserValidator;

    public NovelUserController(NovelService novelService, NovelUserService novelUserService,
        IValidator<AddNovelUserDto> novelUserValidator)
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
    public async Task<IActionResult> AddNovelUser([FromBody] AddNovelUserDto novelUser)
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
        var addResult = await _novelUserService.AddNovelUser(novelUser.NovelId, novelUser.UserName);

        switch (addResult)
        {
            case EAddNovelUserResult.Success:
                return new OkResponse
                {
                    Description = "Added"
                };
            
            case EAddNovelUserResult.AlreadyOwnPermission:
                return new BadRequestResponse
                {
                    Description = "User Already Have Permission On This Novel"
                };
            
            case EAddNovelUserResult.UsernameNotExist:
                return new BadRequestResponse
                {
                    Description = "This Username Not Exist"
                };
            
            default:
                return new BadRequestResponse
                {
                    Description = "Unknown Error"
                };
        }
        
    }
    
    [HttpDelete("{novelUserId}")]
    [Authorize]
    public async Task<IActionResult> RemoveNovelUser(int novelUserId)
    {
        var novelUser = await _novelUserService.GetNovelUser(novelUserId);
        
        if (novelUser?.UserId != User.GetCurrentUserId()) //check if current User Have Permission To Delete This NovelUser
        {
            return new BadRequestResponse
            {
                Description = "You Cant Remove Novel User If You Not Owner Of This Novel"
            };
        }

        //Remove NovelUser
        var removeResult = await _novelUserService.RemoveNovelUserByNovelUserId(novelUserId, novelUser.UserId);

        switch (removeResult)
        {
            case ERemoveNovelUserResult.Success:
                return new OkResponse
                {
                    Description = "Removed"
                };
            
            case ERemoveNovelUserResult.AlreadyDontOwnPermission:
                return new BadRequestResponse
                {
                    Description = "User Already Dont Have Permission On This Novel"
                };
            
            case ERemoveNovelUserResult.OwnerTryRemoveItself:
                return new BadRequestResponse
                {
                    Description = "You Cant Remove Yourself"
                };
            
            case ERemoveNovelUserResult.ThisNovelUserIdNotExist:
                return new BadRequestResponse
                {
                    Description = "There is No NovelUser With This Id"
                };
            
            default:
                return new BadRequestResponse
                {
                    Description = "Unknown Error"
                };
        }
        
    }
}