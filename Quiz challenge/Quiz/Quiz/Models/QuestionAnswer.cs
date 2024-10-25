namespace QuizChallenge.Models
{
    public class QuestionAnswer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }

        public Question Question { get; set; }
        public Answer Answer { get; set; }
    }
}
