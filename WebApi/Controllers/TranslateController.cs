using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NovelTextProcessor;
using WebApi.DTOs;
using WebApi.ObjectsMapper;
using WebApi.Services;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TranslateController : ControllerBase
	{
		private readonly EntityNameService _entityNameService;

		public TranslateController(EntityNameService entityNameService)
		{
			_entityNameService = entityNameService;
		}

		[HttpPost("{novelId}")]
		public async Task<IActionResult> Translate(int novelId, [FromBody] TranslateDto data)
		{
			var entityNamesOfThisNovel = _entityNameService.GetEntityNamesByNovelId(novelId);

			if (entityNamesOfThisNovel.IsNullOrEmpty())
			{
				return BadRequest("Novel Id Not Exist");
			}

			var entityNamesOfThisNovelAsNovelTextProcessor = EntityNameMapper.ConvertFromModelEntityNameArrayToNovelTextProcessorEntityNameArray(entityNamesOfThisNovel.ToArray()).ToArray();
			Processor processor = new Processor(data.Text, entityNamesOfThisNovelAsNovelTextProcessor);
			await processor.RunAsync();
			return Ok(processor.GetResult());
		}
	}
}
