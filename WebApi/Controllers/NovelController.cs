using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Responses;
using WebApi.Services;
using WebApp.Extensions;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NovelController : ControllerBase
	{
		private readonly NovelService _novelService;

		public NovelController(NovelService novelService)
		{
			_novelService = novelService;
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
				return new ErrorResponse()
				{
					Description = "Novel Id Not Exist"
				};
			}
			return Ok(novel);
		}

		// POST api/<NovelController>
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Create([FromBody] string novelName)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value.ToInt();

			var novelUserId = await _novelService.AddNovel(novelName, userId);
			if (novelUserId == 0)
			{
				return new ErrorResponse()
				{
					Description = "You Already Own Novel With Same Name"
				};
			}
			return Ok("Created");
		}

		// DELETE api/<NovelController>/5
		[HttpDelete("{id}")]
		[Authorize]
		public async Task<IActionResult> Delete(int id)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value.ToInt();
			var novel = _novelService.GetById(id);

			if (novel == null) // check if novel not exist
			{
				return new ErrorResponse()
				{
					Description = "No Novel With This Id"
				};
			}

			if (novel.UserId != userId) //check if current User Have Permission To Delete This NovelClone
			{
				return new ErrorResponse()
				{
					Description = "You Cant Delete Novel Without Have Permission On It"
				};
			}

			var isDeleted = await _novelService.DeleteNovel(id);
			if (!isDeleted)
			{
				return new ErrorResponse()
				{
					Description = "Unknown Error"
				};
			}
			return Ok("Deleted");
		}
	}
}
