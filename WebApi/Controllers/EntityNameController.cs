using Azure;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Models;
using WebApi.Responses;
using WebApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EntityNameController : ControllerBase
	{
		EntityNameService _entityNameService;

		public EntityNameController(EntityNameService entityNameService)
		{
			_entityNameService = entityNameService;
		}

		// POST api/<EntityNameController>
		[HttpPost]
		public IActionResult Create([FromBody] EntityNameDto entityName)
		{
			//TDO First Check if user have acsses on this novel or not
			var entityNameId = _entityNameService.AddEntityName(entityName.EnglishName,entityName.Gender,entityName.NovelId);
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
		public void Update(int id, [FromBody] string NewEnglishName)
		{
			_entityNameService.UpdateEntityName(id, NewEnglishName); //TDO make it need auth and validate if auth id have acsses on this entity or not by Join Novel by novelId
		}

		// DELETE api/<EntityNameController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
			_entityNameService.DeleteEntityName(id); //TDO make it need auth and validate if auth id have acsses on this entity or not
		}
	}
}
