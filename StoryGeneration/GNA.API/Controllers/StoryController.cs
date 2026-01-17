using System.Threading.Tasks;
using GNA.API.Interfaces;
using GNA.API.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GNA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        private readonly IStoryService _storyService;

        public StoryController(IStoryService storyService)
        { 
            _storyService = storyService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromBody] StoryRequest request)
        {
            if (request == null)
                return BadRequest("İstek boş geldi.");

            var result = await _storyService.HikayeUret(request);

            if (!result.Basarili)
                return StatusCode(500, result.HataMesaji);

            return Ok(result);
        }
    }
}
