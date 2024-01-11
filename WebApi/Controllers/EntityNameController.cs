using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NovelTextProcessor;
using System.Security.Claims;
using WebApi.DTOs;
using WebApi.ObjectsMapper;
using WebApi.Responses;
using WebApi.Services;
using WebApp.Extensions;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EntityNameController : ControllerBase //TDO use DataProtectionProvider to create protector to encrypt ID
	{
		private readonly EntityNameService _entityNameService;
		private readonly NovelService _novelService;
		private readonly IValidator<EntityNameDto> _EntityNameValidator;
		private readonly Document_NLP document_NLP;

		public EntityNameController(EntityNameService entityNameService, NovelService novelService, IValidator<EntityNameDto> entityNameValidator)
		{
			_entityNameService = entityNameService;
			_novelService = novelService;
			_EntityNameValidator = entityNameValidator;
			document_NLP = new Document_NLP();
		}

		[HttpGet("{id}")]
		public IActionResult GetEntityNameById(int id) 
		{
			var entityNames = _entityNameService.GetEntityNamesByNovelId(id);

            return Ok(entityNames.Select(e => new 
			{
				e.Id,
				e.EnglishName,
				e.ArabicName,
				e.Gender
			}));
		}

		// POST api/<EntityNameController>
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> CreateMany([FromBody] EntityNameDto entityName)
		{
			//Validate
			var validationResult = await _EntityNameValidator.ValidateAsync(entityName);
			if (!validationResult.IsValid)
			{
				return BadRequest(validationResult.Errors);
			}
			//Check if user have acsses on this novel or not
			var novelUserIdOfThisEntity = _novelService.GetById(entityName.NovelId)!.UserId;
			if (!_IsUserHaveAcsses(novelUserIdOfThisEntity))
			{
				return new ErrorResponse()
				{
					Description = "You Dont Have Permission On This Novel",
				};
			}

			//Add To database
			var entityNamesAddResult = await _entityNameService.AddManyEntityNames(entityName);
			if (entityNamesAddResult) // If Add Not Failed
			{
				return NoContent();
			}
			return new ErrorResponse()
			{
				Description = "One Of EntityNames Or More Exist",
			};
		}

		// PUT api/<EntityNameController>/5
		//TDO i think i will make only gender EditAble Here Later To Dont Broke Replace Bycouse Replace i will order by length (if i dont it will bug when have firstName only in line and other line with firstName lastName) so better make only gender editAble or ReOrderBy every edit (i will look at it when Finsh NLP NER Controller(Extract Names By Ai))
		[HttpPut("{id}")]
		[Authorize]
		public async Task<IActionResult> Update(int id, [FromBody] string NewEnglishName, char gender)
		{
			//Check For permission
			var userId = _GetCurrentUserId();
			if (!_entityNameService.CheckIfNovelUserIdsOfThisEntityNameEqualThisNovelUserId(id, userId))
			{
				return new ErrorResponse()
				{
					Description = "You Dont Have Permission On This Novel",
				};
			}
			//Update EntityName
			await _entityNameService.UpdateEntityName(id, NewEnglishName, gender);
			return Ok("Edited");
		}

		// DELETE api/<EntityNameController>/5
		[HttpDelete("{id}")]
		[Authorize]
		public IActionResult Delete(int id)
		{
			//Check For permission
			var userId = _GetCurrentUserId();
			if (!_entityNameService.CheckIfNovelUserIdsOfThisEntityNameEqualThisNovelUserId(id, userId))
			{
				return new ErrorResponse()
				{
					Description = "You Dont Have Permission On This Novel",
				};
			}
			//Delete EntityName
			_entityNameService.DeleteEntityName(id);
			return NoContent();
		}

		[HttpPost("ExtractEntityNames")]
		[Authorize]
		public async Task<IActionResult> ExtractEntityNamesFromText([FromBody] ExtractEntityNameDto extractEntityName)
		{
			await document_NLP.RunAsync(extractEntityName.Text);
			return Ok(document_NLP.ExtractEntityNames().OrderByDescending(e => e.Length));
		}

		private bool _IsUserHaveAcsses(int novelUserId)
		{
			var userId = _GetCurrentUserId();
			if (userId != novelUserId)
			{
				return false;
			}
			return true;
		}

		private int _GetCurrentUserId() => User.FindFirst(ClaimTypes.NameIdentifier)!.Value.ToInt();
	}
}
