using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace daily_dev.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class sourceController : ControllerBase
    {
        // GET: <sourceController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET <sourceController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST <sourceController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT <sourceController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE <sourceController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
