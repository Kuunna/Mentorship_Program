namespace QuizChallenge.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string TopicName { get; set; }
        public int? ParentTopicID { get; set; }

        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<QuizTag> QuizTags { get; set; } = new List<QuizTag>();
    }
}
