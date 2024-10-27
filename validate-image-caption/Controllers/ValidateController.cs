using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace validate_image_caption.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidateController : ControllerBase
    {
        private readonly ILogger<ValidateController> _logger;

        // GET: api/<ValuesController>
        [HttpGet("caption-image")]
        public IEnumerable<string> CaptionImage()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/<ValuesController>
        [HttpGet("enhance-description")]
        public IEnumerable<string> EnhanceDescription()
        {
            return new string[] { "value1", "value2" };
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
