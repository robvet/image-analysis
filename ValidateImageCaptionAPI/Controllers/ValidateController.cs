using Azure;
using ImageCaptionServices;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ValidateImageCaptionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidateController : ControllerBase
    {
        private readonly ILogger<ValidateController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IImageCaptionOrchestrator _imageCaptionService;

        public ValidateController(IConfiguration configuration, IImageCaptionOrchestrator imageCaptionService)
        {
            _configuration = configuration;
            _imageCaptionService = imageCaptionService;
        }

        // GET: api/<ValuesController>
        [HttpGet("caption-image")]
        public async Task<IActionResult> CaptionImage(string imageName)
        {
            string response = null;

            response = await _imageCaptionService.OrchestrateAsync(imageName);
          
            if (response == null)
              return BadRequest($"Null Completion returned to endpoint");
        
            return Ok(response);
        }

        // GET: api/<ValuesController>
        [HttpGet("enhance-description")]
        public async Task<IActionResult> EnhanceDescription()
        {
            return Ok() ;
        }



        //// GET api/<ValuesController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<ValuesController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<ValuesController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<ValuesController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
