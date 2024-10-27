using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizChallenge.Models;
using QuizChallenge.Repositories;
using Microsoft.AspNetCore.OData.Query;

namespace QuizChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly QuestionRepository _questionRepository;

        public QuestionController(QuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult GetAllQuestions()
        {
            var questions = _questionRepository.GetAll();
            return Ok(questions);
        }

        [HttpGet("{id}")]
        public IActionResult GetQuestionById(int id)
        {
            var question = _questionRepository.GetById(id);
            if (question == null) return NotFound();
            return Ok(question);
        }

        [HttpPost]
        public IActionResult CreateQuestion([FromBody] Question question)
        {
            if (question == null) return BadRequest();
            _questionRepository.Add(question);
            return CreatedAtAction(nameof(GetQuestionById), new { id = question.Id }, question);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateQuestion(int id, [FromBody] Question question)
        {
            if (id != question.Id) return BadRequest();
            _questionRepository.Update(question);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteQuestion(int id)
        {
            _questionRepository.Delete(id);
            return NoContent();
        }
    }

}
