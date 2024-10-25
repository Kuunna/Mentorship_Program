using System.Reflection.Emit;

namespace QuizChallenge.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Format { get; set; }
        public int LevelId { get; set; }
        public int TopicId { get; set; }
        public int QuestionTypeId { get; set; }

        public Level Level { get; set; }
        public Topic Topic { get; set; }
        public ICollection<QuizQuestion> QuizQuestions { get; set; }
        public ICollection<QuestionAnswer> QuestionAnswers { get; set; }
    }
}
