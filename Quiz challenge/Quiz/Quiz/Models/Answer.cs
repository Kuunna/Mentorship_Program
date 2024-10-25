namespace QuizChallenge.Models
{
    public class Answer
    {
        public int Id { get; set; }                      
        public string AnswerText { get; set; }            
        public bool IsCorrect { get; set; }               
        public bool IsDynamic { get; set; }               
        public bool CanBeSuggested { get; set; }          

        public ICollection<QuestionAnswer> QuestionAnswers { get; set; } = new List<QuestionAnswer>();  
    }

}
