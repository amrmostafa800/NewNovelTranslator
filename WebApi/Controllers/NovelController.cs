﻿using FluentValidation;
using FluentValidation.Results;
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
	public class NovelController : ControllerBase
	{
		private readonly NovelService _novelService;
		private readonly IValidator<CreateNovelDto> _createNovelValidator;

		public NovelController(NovelService novelService, IValidator<CreateNovelDto> createNovelValidator)
		{
			_novelService = novelService;
			_createNovelValidator = createNovelValidator;
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
		public async Task<IActionResult> Create([FromBody] CreateNovelDto novelDto)
		{
			ValidationResult validationResult = await _createNovelValidator.ValidateAsync(novelDto);

			if (!validationResult.IsValid)
			{
				return BadRequest(validationResult.Errors);
			}

			var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value.ToInt();

			var novelUserId = await _novelService.AddNovel(novelDto.NovelName, userId);
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

			if (novel.UserId != userId) //check if current User Have Permission To Delete This Novel
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
