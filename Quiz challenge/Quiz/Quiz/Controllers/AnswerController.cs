using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizChallenge.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Query;

namespace QuizChallenge.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerRepository _answerRepository;

        public AnswerController(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult GetAnswersByQuestionId(int questionId)
        {
            var answers = _answerRepository.GetByQuestionId(questionId);
            return Ok(answers);
        }

        [HttpGet("{id}")]
        public IActionResult GetAnswerById(int id)
        {
            var answer = _answerRepository.GetById(id);
            if (answer == null) return NotFound();
            return Ok(answer);
        }

        [HttpPost]
        public IActionResult CreateAnswer([FromBody] Answer answer)
        {
            if (answer == null) return BadRequest();
            _answerRepository.Add(answer);
            return CreatedAtAction(nameof(GetAnswerById), new { id = answer.Id }, answer);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAnswer(int id, [FromBody] Answer answer)
        {
            if (id != answer.Id) return BadRequest();
            _answerRepository.Update(answer);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAnswer(int id)
        {
            _answerRepository.Delete(id);
            return NoContent();
        }
    }

}
