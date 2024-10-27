using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizChallenge.Models;
using QuizChallenge.Repositories;
using Microsoft.AspNetCore.OData.Query;

namespace QuizChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly QuizRepository _quizRepository;

        public QuizController(QuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        [HttpGet]
        [EnableQuery] // Enable OData querying
        public IActionResult GetAllQuizzes()
        {
            var quizzes = _quizRepository.GetAll();
            return Ok(quizzes);
        }

        [HttpGet("{id}")]
        public IActionResult GetQuizById(int id)
        {
            var quiz = _quizRepository.GetById(id);
            if (quiz == null) return NotFound();
            return Ok(quiz);
        }

        [HttpPost]
        public IActionResult CreateQuiz([FromBody] Quiz quiz)
        {
            if (quiz == null) return BadRequest();
            _quizRepository.Add(quiz);
            return CreatedAtAction(nameof(GetQuizById), new { id = quiz.Id }, quiz);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateQuiz(int id, [FromBody] Quiz quiz)
        {
            if (id != quiz.Id) return BadRequest();
            _quizRepository.Update(quiz);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteQuiz(int id)
        {
            _quizRepository.Delete(id);
            return NoContent();
        }
    }
}
