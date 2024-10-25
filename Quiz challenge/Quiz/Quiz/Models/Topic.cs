namespace QuizChallenge.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentTopicId { get; set; }

        public Topic ParentTopic { get; set; }
        public ICollection<Topic> SubTopics { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}
