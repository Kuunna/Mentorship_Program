namespace QuizChallenge.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string TagName { get; set; }

        public ICollection<QuizTag> QuizTags { get; set; } = new List<QuizTag>();
    }
}
