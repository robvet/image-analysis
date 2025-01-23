using Azure;
using DescrptionEnhancementService.DescrptionEnhancementServices.Contracts;
using ImageCaptionService.Contracts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Contracts;

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
        private readonly IDescriptionOrchestrator _descriptionOrchestrator;

        public ValidateController(IConfiguration configuration, 
                                  IImageCaptionOrchestrator imageCaptionService,
                                  IDescriptionOrchestrator descriptionOrchestrator)
        {
            _configuration = configuration;
            _imageCaptionService = imageCaptionService;
            _descriptionOrchestrator = descriptionOrchestrator;
        }

        // GET: api/<ValuesController>
        [HttpGet("caption-image")]
        public async Task<IActionResult> CaptionImage(string imageName)
        {
            var response = await _imageCaptionService.OrchestrateAsync(imageName);
          
            if (response == null)
                return BadRequest($"Image Not Found");

            string json = JsonConvert.SerializeObject(response, Formatting.Indented);

            return Ok(json);
        }

        // GET: api/<ValuesController>
        [HttpGet("enhance-description")]
        public async Task<IActionResult> EnhanceDescription(string productDescription)
        {
            var response = await _descriptionOrchestrator.OrchestrateAsync(productDescription);

            if (string.IsNullOrEmpty(response))
                return BadRequest($"Product Not Found");

            return Ok(response);
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
