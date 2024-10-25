namespace QuizChallenge.Models
{
    public class UserQuiz
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int QuizId { get; set; }
        public int CompletionTime { get; set; }
        public DateTime AttemptAt { get; set; }

        public User User { get; set; }
        public Quiz Quiz { get; set; }
        public ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
