﻿namespace QuizChallenge.Models
{
    public class UserAnswer
    {
        public int Id { get; set; }
        public int UserQuizId { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public string FreeText { get; set; }
    }
}
