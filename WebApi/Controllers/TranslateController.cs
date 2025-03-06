using Microsoft.AspNetCore.Mvc;
using NovelTextProcessor;
using WebApi.DTOs;
using WebApi.ObjectsMapper;
using WebApi.Services;

namespace WebApi.Controllers;

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
        //Can Check Here If Novel Exist or Not But This Will Not Happen If Person Call Api Using Website Normal
        var entityNamesOfThisNovel = _entityNameService.GetEntityNamesByNovelId(novelId, true);

        if (entityNamesOfThisNovel.Count == 0)
        {
            return BadRequest("Please Add At Last One Entity Name Before Translate");
        }

        var entityNamesOfThisNovelAsNovelTextProcessor = EntityNameMapper
            .ConvertFromModelEntityNameArrayToNovelTextProcessorEntityNameArray(entityNamesOfThisNovel.ToArray())
            .ToArray();

        var processor = new Processor(data.Text, entityNamesOfThisNovelAsNovelTextProcessor);
        await processor.RunAsync();
        return Ok(processor.GetResult().Replace("لله ", "***").Replace("الله", "***")); // TDO : need edit soon
    }
}