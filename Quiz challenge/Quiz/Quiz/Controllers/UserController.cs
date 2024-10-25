using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizChallenge.Models;
using Microsoft.AspNetCore.OData.Query;

namespace QuizChallenge.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [EnableQuery] // Enable OData querying
        public IActionResult GetAllUsers()
        {
            var users = _userRepository.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public IActionResult RegisterUser([FromBody] User user)
        {
            if (user == null) return BadRequest();
            _userRepository.Add(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.Id) return BadRequest();
            _userRepository.Update(user);
            return NoContent();
        }
    }

}
