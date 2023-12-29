using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using NovelTextProcessor;
using WebApi.DTOs;
using WebApi.Models;
using WebApi.Responses;
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

			var entityNamesOfThisNovelAsNovelTextProcessor = ConvertFromModelEntityNameArrayToNovelTextProcessorEntityNameArray(entityNamesOfThisNovel.ToArray()).ToArray();
			Processor processor = new Processor(data.Text, entityNamesOfThisNovelAsNovelTextProcessor);
			await processor.RunAsync();
			return Ok(processor.GetResult());
		}

		private IEnumerable<NovelTextProcessor.Dtos.EntityName> ConvertFromModelEntityNameArrayToNovelTextProcessorEntityNameArray(EntityName[] entityNames)
		{
			for (int i = 0; i < entityNames.Length; i++)
			{
				EntityName? entityName = entityNames[i];
				yield return ConvertFromModelEntityNameToNovelTextProcessorEntityName(entityName);
			}
		}

		private NovelTextProcessor.Dtos.EntityName ConvertFromModelEntityNameToNovelTextProcessorEntityName(EntityName entityName)
		{
			return new NovelTextProcessor.Dtos.EntityName()
			{
				EnglishName = entityName.EnglishName,
				ArabicName = entityName.ArabicName!,
				Gender = entityName.Gender,
			};
		}
	}
}
