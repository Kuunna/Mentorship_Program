using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace daily_dev.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class tagController : ControllerBase
    {
        // GET: api/<tagController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<tagController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<tagController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<tagController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<tagController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
