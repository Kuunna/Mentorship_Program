using QuizChallenge.Models;
using QuizChallenge.Repositories;

namespace QuizChallenge.Services
{
    using System;
    using System.Collections.Generic;
    using QuizChallenge.Models;
    using QuizChallenge.Repositories;

    public class UserQuizService
    {
        private readonly UserQuizRepository _userQuizRepository;

        public UserQuizService(UserQuizRepository userQuizRepository)
        {
            _userQuizRepository = userQuizRepository;
        }

        public UserQuiz GetQuizResultByUserId(int userId, int quizId) => _userQuizRepository.GetUserQuizByUserIdAndQuizId(userId, quizId);

        public List<UserQuiz> GetUserQuizHistory(int userId) => _userQuizRepository.GetUserQuizByUserId(userId);

        public void SaveQuizCompletionTime(int userId, int quizId, DateTime completionTime) => _userQuizRepository.SaveQuizCompletionTime(userId, quizId, completionTime);


        //public List<UserLeaderboard> GetLeaderboard() => _userQuizRepository.GetLeaderboard();

    }
}
