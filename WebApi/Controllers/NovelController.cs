using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Models;
using WebApi.Responses;
using WebApi.Services;
using WebApp.Extensions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
		public async Task<IActionResult> Get(int id)
		{
			var novel = await _novelService.GetById(id);
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
		public async Task<IActionResult> Delete(int id)
		{
			var result = await _novelService.DeleteNovel(id);
			if (!result)
			{
				return new ErrorResponse()
				{
					Description = "No Novel With This Id"
				};
			}
			return Ok("Deleted");
		}
	}
}
