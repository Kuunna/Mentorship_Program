namespace QuizChallenge.Models
{
    public class Level
    {
        public int Id { get; set; }
        public string LevelName { get; set; }
        public int ScoreWeight { get; set; }
        public int TimeConstraint { get; set; }

        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
