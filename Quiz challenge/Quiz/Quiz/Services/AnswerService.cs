using QuizChallenge.Models;
using QuizChallenge.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace QuizChallenge.Services
{
    public class AnswerService
    {
        private readonly AnswerRepository _answerRepository;
        private readonly UserAnswerRepository _userAnswerRepository;

        public AnswerService(AnswerRepository answerRepository, UserAnswerRepository userAnswerRepository)
        {
            _answerRepository = answerRepository;
            _userAnswerRepository = userAnswerRepository;
        }

        public void AddAnswer(Answer answer) => _answerRepository.AddAnswer(answer);

        public Answer GetAnswerById(int id) => _answerRepository.GetAnswerById(id);

        public List<Answer> GetAnswersByQuestionId(int questionId) => _answerRepository.GetAnswersByQuestionId(questionId);

        public void UpdateAnswer(Answer answer) => _answerRepository.UpdateAnswer(answer);

        public void DeleteAnswer(int answerId) => _answerRepository.DeleteAnswer(answerId);

        public void SaveUserAnswer(UserAnswer userAnswer) => _userAnswerRepository.AddUserAnswer(userAnswer);

        public List<Answer> GetCorrectAnswersForQuestion(int questionId) => _answerRepository.GetCorrectAnswersForQuestion(questionId);

        public bool EvaluateAnswers(int userQuizId, List<UserAnswer> userAnswers)
        {
            bool isAllCorrect = true;

            foreach (var userAnswer in userAnswers)
            {
                var correctAnswers = GetCorrectAnswersForQuestion(userAnswer.QuestionId);
                var isAnswerCorrect = correctAnswers.Any(ca => ca.Id == userAnswer.AnswerId && ca.IsCorrect);

                if (!isAnswerCorrect)
                {
                    isAllCorrect = false;
                }
            }

            return isAllCorrect;
        }
    }

}
