using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Responses;
using WebApi.Services;
using WebApi.Extensions;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NovelController : ControllerBase //TDO use DataProtectionProvider to create protector to encrypt ID - Try make code more clean
{
    private readonly NovelService _novelService;
    private readonly NovelSharedService _novelSharedService;
    private readonly IValidator<CreateNovelDto> _novelValidator;

    public NovelController(NovelService novelService, IValidator<CreateNovelDto> createNovelValidator, NovelSharedService novelSharedService)
    {
        _novelService = novelService;
        _novelValidator = createNovelValidator;
        _novelSharedService = novelSharedService;
    }

    // GET: api/<NovelController>
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_novelService.GetAllNovels());
    }

    // GET api/<NovelController>/5
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var novel = _novelService.GetById(id);

        if (novel == null)
        {
            return new BadRequestResponse
            {
                Description = "Novel Id Not Exist"
            };
        }
        
        return Ok(new
        {
            novel.Id,
            novel.Name,
            OwnerUserId = novel.UserId,
        });
    }

    // POST api/<NovelController>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateNovelDto novelDto)
    {
        var validationResult = await _novelValidator.ValidateAsync(novelDto);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value.ToInt();

        var novelUserId = await _novelService.AddNovel(novelDto.NovelName, userId);
        
        if (novelUserId == 0)
        {
            return new BadRequestResponse
            {
                Description = "You Already Own Novel With Same Name"
            };
        }

        return new OkResponse
        {
            Description = "Created"
        };
    }

    // DELETE api/<NovelController>/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var novel = _novelService.GetById(id);

        if (novel == null) // check if novel not exist
        {
            return new BadRequestResponse
            {
                Description = "No Novel With This Id"
            };
        }

        if (novel.UserId != User.GetCurrentUserId()) //check if current User Have Permission To Delete This Novel (Only Owner Can Delete Novel (first NovelUser is the Owner and is who create the novel))
        {
            return new BadRequestResponse
            {
                Description = "You Cant Delete Novel Not Created By You"
            };
        }

        var isDeleted = await _novelService.DeleteNovel(id);
        
        if (!isDeleted)
        {
            return new BadRequestResponse
            {
                Description = "Unknown Error"
            };
        }
        
        return new OkResponse
        {
            Description = "Deleted"
        };
    }

    [HttpPost("CheckIfOwnPermissionOnNovel")]
    public async Task<IActionResult> CheckIfOwnPermissionOnNovel(CheckForPermissionDto checkForPermission)
    {
        var result = await _novelSharedService.IsUserOwnThisNovel(checkForPermission.NovelId, checkForPermission.UserId);
        
        if (result)
        {
            return new OkResponse
            {
                Description = "true"
            };
        }

        return new OkResponse()
        {
            Description = "False"
        };
    }
}