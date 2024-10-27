namespace QuizChallenge.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        public int TimeLimit { get; set; }

        public ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
        public ICollection<QuizTag> QuizTags { get; set; } = new List<QuizTag>();
    }
}
