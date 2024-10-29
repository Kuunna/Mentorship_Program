using QuizChallenge.Models;
using QuizChallenge.Repositories;

namespace QuizChallenge.Services
{
    public class TagService
    {
        private readonly TagRepository _tagRepository;
        private readonly QuizTagRepository _quizTagRepository;

        public TagService(TagRepository tagRepository, QuizTagRepository quizTagRepository)
        {
            _tagRepository = tagRepository;
            _quizTagRepository = quizTagRepository;
        }

        public void CreateTag(Tag tag) => _tagRepository.AddTag(tag);

        public void AssignTagToQuiz(int quizId, int tagId) => _quizTagRepository.AddQuizTag(new QuizTag { QuizId = quizId, TagId = tagId });

        //public List<Quiz> GetQuizzesByTag(int tagId) => _tagRepository.GetQuizzesByTag(tagId);

        public List<Tag> GetAllTags() => _tagRepository.GetAllTags();
    }

}
