using QuizChallenge.Models;
using QuizChallenge.Repositories;

namespace QuizChallenge.Services
{
    public class QuizService
    {
        private readonly QuizRepository _quizRepository;
        private readonly QuestionRepository _questionRepository;
        private readonly UserQuizRepository _userQuizRepository;
        private readonly LevelRepository _levelRepository;

        public QuizService(QuizRepository quizRepository, QuestionRepository questionRepository, UserQuizRepository userQuizRepository, LevelRepository levelRepository)
        {
            _quizRepository = quizRepository;
            _questionRepository = questionRepository;
            _userQuizRepository = userQuizRepository;
            _levelRepository = levelRepository;
        }

        public void CreateQuiz(Quiz quiz) => _quizRepository.AddQuiz(quiz);

        public Quiz GetQuizById(int quizId) => _quizRepository.GetQuizById(quizId);

        public void AddQuestionToQuiz(int quizId, int questionId) => _quizRepository.AddQuestionToQuiz(quizId, questionId);

        public void RemoveQuestionFromQuiz(int quizId, int questionId) => _quizRepository.RemoveQuestionFromQuiz(quizId, questionId);

        public double SubmitAnswer(int quizId, int userId, List<Answer> answers)
        {
            var questions = _questionRepository.GetQuestionsByQuizId(quizId);
            double totalScore = 0;

            foreach (var answer in answers)
            {
                var questionId = _questionRepository.GetQuestionIdByAnswerId(answer.Id);

                if (questionId > 0)
                {
                    var isCorrect = _questionRepository.IsCorrectAnswer(questionId, answer);
                    if (isCorrect)
                    {
                        var question = _questionRepository.GetQuestionById(questionId);
                        if (question != null)
                        {
                            var levelScoreWeight = _levelRepository.GetScoreWeightByLevelId(question.LevelId);
                            totalScore += levelScoreWeight;
                        }
                    }
                }
            }

            _userQuizRepository.SaveUserScore(userId, totalScore);

            return totalScore;
        }

        public void StartQuizByUser(int userId, int quizId)
        {
            var userQuiz = new UserQuiz
            {
                UserId = userId,
                QuizId = quizId,
                TotalQuestions = _questionRepository.GetQuestionsByQuizId(quizId).Count,
                StartTime = DateTime.Now
            };
            _userQuizRepository.AddUserQuiz(userQuiz);
        }

        public void EndQuizByUser(int userId, int quizId)
        {
            var userQuiz = _userQuizRepository.GetUserQuizByUserIdAndQuizId(userId, quizId);
            if (userQuiz != null)
            {
                userQuiz.EndTime = DateTime.Now;
                userQuiz.CompletionTime = (int)(userQuiz.EndTime.Value - userQuiz.StartTime).TotalMinutes;
                _userQuizRepository.UpdateUserQuiz(userQuiz);
            }
        }

        public double GetUserScoresByUserIdQuizId(int userId, int quizId) => _userQuizRepository.GetUserScoresByQuizId(userId, quizId);
    }

}
