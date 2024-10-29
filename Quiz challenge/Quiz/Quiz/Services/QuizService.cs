using QuizChallenge.Models;
using QuizChallenge.Repositories;

namespace QuizChallenge.Services
{
    public class QuizService
    {
        private readonly QuizRepository _quizRepository;
        private readonly QuestionRepository _questionRepository;

        public QuizService(QuizRepository quizRepository, QuestionRepository questionRepository)
        {
            _quizRepository = quizRepository;
            _questionRepository = questionRepository;
        }

        public void CreateQuiz(Quiz quiz) => _quizRepository.AddQuiz(quiz);

        public Quiz GetQuiz(int quizId) => _quizRepository.GetQuizById(quizId);

        //public List<Quiz> GetQuizzesByCriteria(string criteria) => _quizRepository.GetQuizzesByCriteria(criteria);

        //public void AddQuestionToQuiz(int quizId, int questionId) => _quizRepository.AddQuestionToQuiz(quizId, questionId);

        //public void SaveQuizResult(QuizResult quizResult) => _quizRepository.SaveQuizResult(quizResult);

        //public List<QuizResult> GetQuizResultsByUser(int userId) => _quizRepository.GetQuizResultsByUser(userId);
    }

}
