namespace QuizChallenge.Models
{
    public class Type
    {
        public int Id { get; set; }
        public string TypeName { get; set; }

        public ICollection<Question> Questions { get; set; }
    }
}
