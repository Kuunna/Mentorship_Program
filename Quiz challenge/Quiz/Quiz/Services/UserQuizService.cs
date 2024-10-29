using QuizChallenge.Repositories;

namespace QuizChallenge.Services
{
    public class UserQuizService
    {
        private readonly QuizRepository _quizRepository;

        public UserQuizService(QuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        //public List<QuizResult> GetUserQuizHistory(int userId) => _quizRepository.GetQuizResultsByUser(userId);

        //public int GetQuizScore(int userId, int quizId) => _quizRepository.GetQuizScore(userId, quizId);

        //public void SaveQuizCompletionTime(int userId, int quizId, DateTime completionTime) => _quizRepository.SaveQuizCompletionTime(userId, quizId, completionTime);

        //public QuizResult CalculateQuizResult(int userId, int quizId)
        //{
        //    var score = GetQuizScore(userId, quizId);
        //    return new QuizResult { UserId = userId, QuizId = quizId, Score = score };
        //}
    }

}
