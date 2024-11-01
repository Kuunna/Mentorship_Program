using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuizChallenge.Models;
using QuizChallenge.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QuizTest.RepositoryTests
{
    [TestClass]
    public class AnswerRepositoryTests
    {
        private readonly string _connectionString = "Data Source=HOAINAM;Initial Catalog=Quiz;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
        private AnswerRepository _answerRepository;

        [TestInitialize]
        public void Setup()
        {
            _answerRepository = new AnswerRepository(_connectionString);
        }

        [TestMethod]
        public void TestAddAnswer()
        {
            // Arrange
            var answer = new Answer
            {
                AnswerText = "This is a test Add answer.",
                IsCorrect = true,
                IsDynamic = false,
                CanBeSuggested = false
            };

            // Act
            _answerRepository.AddAnswer(answer);

            // Assert
            var addedAnswer = _answerRepository.GetAllAnswer().FirstOrDefault(a => a.AnswerText == "This is a test Add answer.");

            Assert.IsNotNull(addedAnswer, "Answer should be added successfully.");
            Assert.AreEqual(answer.AnswerText, addedAnswer.AnswerText);
            Assert.AreEqual(answer.IsCorrect, addedAnswer.IsCorrect);
            Assert.AreEqual(answer.IsDynamic, addedAnswer.IsDynamic);
            Assert.AreEqual(answer.CanBeSuggested, addedAnswer.CanBeSuggested);
        }

        [TestMethod]
        public void TestGetAnswerById()
        {
            // Arrange
            var answer = new Answer
            {
                AnswerText = "This is a test Get answer.",
                IsCorrect = true,
                IsDynamic = false,
                CanBeSuggested = false
            };
            _answerRepository.AddAnswer(answer);

            // Act
            var addedAnswer = _answerRepository.GetAllAnswer().FirstOrDefault(a => a.AnswerText == "This is a test Get answer.");

            // Assert
            Assert.IsNotNull(addedAnswer, "Answer should be retrieved successfully.");
            Assert.AreEqual(answer.AnswerText, addedAnswer.AnswerText);
        }

        [TestMethod]
        public void TestUpdateAnswer()
        {
            // Arrange
            var answer = new Answer
            {
                AnswerText = "This is a test update answer.",
                IsCorrect = true,
                IsDynamic = false,
                CanBeSuggested = false
            };
            _answerRepository.AddAnswer(answer);

            // Act
            var addedAnswer = _answerRepository.GetAllAnswer().FirstOrDefault(a => a.AnswerText == "This is a test update answer.");
            addedAnswer.AnswerText = "Updated answer text.";
            addedAnswer.IsCorrect = false;
            _answerRepository.UpdateAnswer(addedAnswer);

            // Assert
            var updatedAnswer = _answerRepository.GetAnswerById(addedAnswer.Id);
            Assert.AreEqual("Updated answer text.", updatedAnswer.AnswerText);
            Assert.IsFalse(updatedAnswer.IsCorrect);
        }

        [TestMethod]
        public void TestDeleteAnswer()
        {
            // Arrange
            var answer = new Answer
            {
                AnswerText = "This is a test Delete answer.",
                IsCorrect = true,
                IsDynamic = false,
                CanBeSuggested = false
            };
            _answerRepository.AddAnswer(answer);

            // Act
            var addedAnswer = _answerRepository.GetAllAnswer().FirstOrDefault(a => a.AnswerText == "This is a test Delete answer.");
            _answerRepository.DeleteAnswer(addedAnswer.Id);

            // Assert
            var deletedAnswer = _answerRepository.GetAnswerById(addedAnswer.Id);
            Assert.IsNull(deletedAnswer, "Answer should be deleted successfully.");
        }

        [TestMethod]
        public void TestGetAnswersByQuestionId()
        {
            
        }

        [TestMethod]
        public void TestGetCorrectAnswersForQuestion()
        {
            
        }
    }
}
