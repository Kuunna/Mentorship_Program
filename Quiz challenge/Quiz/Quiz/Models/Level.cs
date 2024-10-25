namespace QuizChallenge.Models
{
    public class Level
    {
        public int Id { get; set; }
        public string LevelName { get; set; }
        public float ScoreWeight { get; set; }
        public int TimeConstraint { get; set; }

        public ICollection<Question> Questions { get; set; }
    }
}
