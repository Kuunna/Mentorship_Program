using QuizChallenge.Models;
using QuizChallenge.Repositories;

namespace QuizChallenge.Services
{
    public class QuestionService
    {
        private readonly QuestionRepository _questionRepository;
        private readonly AnswerRepository _answerRepository;

        public QuestionService(QuestionRepository questionRepository, AnswerRepository answerRepository)
        {
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
        }

        public void CreateQuestion(Question question)
        {
            _questionRepository.AddQuestion(question);
        }

        public List<Question> GetQuestionsByLevel(string level)
        {
            return _questionRepository.GetQuestionsByLevel(level);
        }

        public List<Question> GetQuestionsByTopic(string topic)
        {
            return _questionRepository.GetQuestionsByTopic(topic);
        }

        public Question GetQuestionById(int id)
        {
            return _questionRepository.GetQuestionById(id);
        }

        public List<Answer> GetAnswersForQuestion(int questionId)
        {
            return _answerRepository.GetAnswersByQuestionId(questionId);
        }

        public List<Answer> GetCorrectAnswersForQuestion(int questionId)
        {
            return _answerRepository.GetCorrectAnswersForQuestion(questionId);
        }

        public void UpdateQuestion(Question question)
        {
            _questionRepository.UpdateQuestion(question);
        }

        public void DeleteQuestion(int questionId)
        {
            _questionRepository.DeleteQuestion(questionId);
        }
    }

}
