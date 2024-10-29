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

        public Quiz GetQuizById(int quizId) => _quizRepository.GetQuizById(quizId);

        public void AddQuestionToQuiz(int quizId, int questionId) => _quizRepository.AddQuestionToQuiz(quizId, questionId);

        public void RemoveQuestionFromQuiz(int quizId, int questionId) => _quizRepository.RemoveQuestionFromQuiz(quizId, questionId);

        public void SubmitAnswer(int quizId, int questionId, Answer answer)
        {
            // Giả định có một phương thức lưu câu trả lời
            // Lưu câu trả lời vào cơ sở dữ liệu (có thể trong một bảng Answer hoặc UserAnswers)
        }

        public QuizResult GetQuizResults(int quizId, int userId) => _quizRepository.GetQuizResults(quizId, userId);
    }

}
