using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
			//TDO First Check if user have acsses on this novel or not
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value.ToInt();
			var novelUserIdOfThisEntity = _novelService.GetById(entityName.NovelId)?.UserId;
			if (userId != novelUserIdOfThisEntity)
			{
				return new ErrorResponse()
				{
					Description = "You Dont Have Permission On This Novel",
				};
			}

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
		public async Task Update(int id, [FromBody] string NewEnglishName,char gender)
		{
			await _entityNameService.UpdateEntityName(id, NewEnglishName, gender); //TDO make it need auth and validate if auth id have acsses on this entity or not by Join Novel by novelId
		}

		// DELETE api/<EntityNameController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
			_entityNameService.DeleteEntityName(id); //TDO make it need auth and validate if auth id have acsses on this entity or not
		}
	}
}
