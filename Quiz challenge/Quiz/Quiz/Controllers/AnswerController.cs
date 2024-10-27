using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizChallenge.Models;
using QuizChallenge.Repositories;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Query;

namespace QuizChallenge.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly AnswerRepository _answerRepository;

        public AnswerController(AnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        
    }

}
