namespace QuizChallenge.Models
{
    public class QuizTag
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
