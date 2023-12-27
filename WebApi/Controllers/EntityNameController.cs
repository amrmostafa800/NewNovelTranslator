using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NovelTextProcessor.Dtos;
using System.Security.Claims;
using WebApi.DTOs;
using WebApi.Models;
using WebApi.Responses;
using WebApi.Services;
using WebApp.Extensions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EntityNameController : ControllerBase //TDO need many edits
	{
		EntityNameService _entityNameService;
		NovelService _novelService;

		public EntityNameController(EntityNameService entityNameService, NovelService novelService)
		{
			_entityNameService = entityNameService;
			_novelService = novelService;
		}

		// POST api/<EntityNameController>
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Create([FromBody] EntityNameDto entityName)
		{
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

			var entityNameId = await _entityNameService.AddEntityName(entityName.EnglishName,entityName.Gender,entityName.NovelId);
			if (entityNameId != 0) // If Add Not Failed
			{
				return Ok($"Created Id:{entityNameId}");
			}
			return new ErrorResponse()
			{
				Description = "Unknown Error",
			};
		}

		// PUT api/<EntityNameController>/5
		[HttpPut("{id}")]
		[Authorize]
		public async Task<IActionResult> Update(int id, [FromBody] string NewEnglishName,char gender) //TDO i think i will make only gender EditAble Here Later To Dont Broke Replace Bycouse Replace i will order by length (if i dont it will bug when have name first only in line and other line with first last) so better make only gender editAble or ReOrderBy every edit (i will look at it when Finsh NLP NER Controller)
		{
			//Check For permission
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value.ToInt();
			if (!_entityNameService.CheckIfNovelUserIdsOfThisEntityNameEqualThisNovelUserId(id, userId))
			{
				return new ErrorResponse()
				{
					Description = "You Dont Have Permission On This Novel",
				};
			}

			await _entityNameService.UpdateEntityName(id, NewEnglishName, gender);
			return Ok("Edited");
		}

		// DELETE api/<EntityNameController>/5
		[HttpDelete("{id}")]
		[Authorize]
		public IActionResult Delete(int id)
		{
			//Check For permission
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value.ToInt();
			if (!_entityNameService.CheckIfNovelUserIdsOfThisEntityNameEqualThisNovelUserId(id, userId))
			{
				return new ErrorResponse()
				{
					Description = "You Dont Have Permission On This Novel",
				};
			}

			_entityNameService.DeleteEntityName(id);
			return NoContent();
		}

		private bool _IsUserHaveAcsses(int novelUserId)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value.ToInt();
			if (userId != novelUserId)
			{
				return false;
			}
			return true;
		}
	}
}
