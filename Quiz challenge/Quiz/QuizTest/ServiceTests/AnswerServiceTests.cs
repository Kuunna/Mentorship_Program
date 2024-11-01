using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QuizChallenge.Models;
using QuizChallenge.Repositories;
using QuizChallenge.Services;
using System.Collections.Generic;
using System.Linq;

namespace QuizTest.ServiceTests
{
    [TestClass]
    public class AnswerServiceTests
    {
        private Mock<AnswerRepository> _mockAnswerRepository;
        private Mock<UserAnswerRepository> _mockUserAnswerRepository;
        private AnswerService _answerService;

        [TestInitialize]
        public void Setup()
        {
            // Initialize mocks
            _mockAnswerRepository = new Mock<AnswerRepository>("fake_connection_string");
            _mockUserAnswerRepository = new Mock<UserAnswerRepository>("fake_connection_string");

            // Initialize AnswerService with mocked repositories
            _answerService = new AnswerService(_mockAnswerRepository.Object, _mockUserAnswerRepository.Object);
        }
    }
}
