using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuizChallenge.Models;
using QuizChallenge.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QuizTest.RepositoryTests
{
    [TestClass]
    public class QuizRepositoryTests
    {
        private QuizRepository _quizRepository;
        private const string TestConnectionString = "Data Source=HOAINAM;Initial Catalog=Quiz;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;"; 

        [TestInitialize]
        public void Setup()
        {
            _quizRepository = new QuizRepository(TestConnectionString);
        }

        [TestMethod]
        public void TestAddQuiz()
        {
            var quiz = new Quiz
            {
                Title = "Test Quiz",
                CreatedAt = DateTime.Now,
                Description = "This is a test quiz",
                TimeLimit = 30
            };

            _quizRepository.AddQuiz(quiz);

            var addedQuiz = _quizRepository.GetAllQuizzes().FirstOrDefault(q => q.Title == "Test Quiz");
            Assert.IsNotNull(addedQuiz);
            Assert.AreEqual(quiz.Description.TrimEnd('.'), addedQuiz.Description.TrimEnd('.'));
            Assert.AreEqual(quiz.TimeLimit, addedQuiz.TimeLimit);
        }


        [TestMethod]
        public void TestGetQuizById()
        {
            // Arrange
            var quiz = new Quiz
            {
                Title = "Another Test Quiz",
                CreatedAt = DateTime.Now,
                Description = "Another test description",
                TimeLimit = 45
            };
            _quizRepository.AddQuiz(quiz);

            // Act
            var addedQuiz = _quizRepository.GetAllQuizzes().FirstOrDefault(q => q.Title == "Another Test Quiz");
            var retrievedQuiz = _quizRepository.GetQuizById(addedQuiz.Id);

            // Assert
            Assert.IsNotNull(retrievedQuiz);
            Assert.AreEqual(addedQuiz.Title, retrievedQuiz.Title);
            Assert.AreEqual(addedQuiz.Description, retrievedQuiz.Description);
        }

        [TestMethod]
        public void TestUpdateQuiz()
        {
            // Arrange
            var quiz = new Quiz
            {
                Title = "Quiz to Update",
                CreatedAt = DateTime.Now,
                Description = "Update this quiz",
                TimeLimit = 30
            };
            _quizRepository.AddQuiz(quiz);

            var addedQuiz = _quizRepository.GetAllQuizzes().FirstOrDefault(q => q.Title == "Quiz to Update");
            Assert.IsNotNull(addedQuiz, "Quiz should be added successfully.");

            addedQuiz.Description = "Updated description";
            addedQuiz.TimeLimit = 60;

            // Act
            _quizRepository.UpdateQuiz(addedQuiz);
            var updatedQuiz = _quizRepository.GetQuizById(addedQuiz.Id);

            // Assert
            Assert.AreEqual("Updated description", updatedQuiz.Description);
            Assert.AreEqual(60, updatedQuiz.TimeLimit);
        }


        [TestMethod]
        public void TestDeleteQuiz()
        {
            // Arrange
            var quiz = new Quiz
            {
                Title = "Quiz to Delete",
                CreatedAt = DateTime.Now,
                Description = "This quiz will be deleted",
                TimeLimit = 30
            };
            _quizRepository.AddQuiz(quiz);
            var addedQuiz = _quizRepository.GetAllQuizzes().FirstOrDefault(q => q.Title == "Quiz to Delete");

            // Act
            _quizRepository.DeleteQuiz(addedQuiz.Id);
            var deletedQuiz = _quizRepository.GetQuizById(addedQuiz.Id);

            // Assert
            Assert.IsNull(deletedQuiz);
        }
    }
}
