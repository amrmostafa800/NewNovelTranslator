using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.DTOs;
using WebApi.Responses;
using WebApi.Services;
using WebApp.Extensions;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EntityNameController : ControllerBase //TDO need many edits - use DataProtectionProvider to create protector to encrypt ID
	{
		EntityNameService _entityNameService;
		NovelService _novelService;

		public EntityNameController(EntityNameService entityNameService, NovelService novelService)
		{
			_entityNameService = entityNameService;
			_novelService = novelService;
		}

		// POST api/<EntityNameController>
		[HttpPost()]
		[Authorize]
		public async Task<IActionResult> CreateMany([FromBody] EntityNameDto entityName)
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
			var entityNamesAddResult = await _entityNameService.AddManyEntityNames(entityName);
			if (entityNamesAddResult) // If Add Not Failed
			{
				return Ok($"Created Id:{entityNamesAddResult}");
			}
			return new ErrorResponse()
			{
				Description = "One Of EntityNames Or More Exist",
			};
		}

		// PUT api/<EntityNameController>/5
		[HttpPut("{id}")]
		[Authorize]
		public async Task<IActionResult> Update(int id, [FromBody] string NewEnglishName, char gender) //TDO i think i will make only gender EditAble Here Later To Dont Broke Replace Bycouse Replace i will order by length (if i dont it will bug when have name first only in line and other line with first last) so better make only gender editAble or ReOrderBy every edit (i will look at it when Finsh NLP NER Controller)
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

			_entityNameService.DeleteEntityName(id);
			return NoContent();
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
