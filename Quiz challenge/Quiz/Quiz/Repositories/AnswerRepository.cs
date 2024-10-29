using QuizChallenge.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace QuizChallenge.Repositories
{
    public class AnswerRepository
    {
        private readonly string _connectionString;

        public AnswerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddAnswer(Answer answer)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("INSERT INTO Answer (AnswerText, IsCorrect, IsDynamic, CanBeSuggested) VALUES (@AnswerText, @IsCorrect, @IsDynamic, @CanBeSuggested)", connection);
                command.Parameters.AddWithValue("@AnswerText", answer.AnswerText);
                command.Parameters.AddWithValue("@IsCorrect", answer.IsCorrect);
                command.Parameters.AddWithValue("@IsDynamic", answer.IsDynamic);
                command.Parameters.AddWithValue("@CanBeSuggested", answer.CanBeSuggested);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public Answer GetAnswerById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Answer WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Answer
                        {
                            Id = (int)reader["Id"],
                            AnswerText = reader["AnswerText"].ToString(),
                            IsCorrect = (bool)reader["IsCorrect"],
                            IsDynamic = (bool)reader["IsDynamic"],
                            CanBeSuggested = (bool)reader["CanBeSuggested"]
                        };
                    }
                }
            }
            return null;
        }

        public List<Answer> GetAnswersByQuestionId(int questionId)
        {
            var answers = new List<Answer>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Answer WHERE Id IN (SELECT AnswerId FROM QuestionAnswer WHERE QuestionId = @QuestionId)", connection);
                command.Parameters.AddWithValue("@QuestionId", questionId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        answers.Add(new Answer
                        {
                            Id = (int)reader["Id"],
                            AnswerText = reader["AnswerText"].ToString(),
                            IsCorrect = (bool)reader["IsCorrect"],
                            IsDynamic = (bool)reader["IsDynamic"],
                            CanBeSuggested = (bool)reader["CanBeSuggested"]
                        });
                    }
                }
            }
            return answers;
        }

        public List<Answer> GetCorrectAnswersForQuestion(int questionId)
        {
            var correctAnswers = new List<Answer>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(@"
                SELECT a.* 
                FROM Answer a
                JOIN QuestionAnswer qa ON a.Id = qa.AnswerId
                WHERE qa.QuestionId = @QuestionId AND a.IsCorrect = 1", connection);
                command.Parameters.AddWithValue("@QuestionId", questionId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        correctAnswers.Add(new Answer
                        {
                            Id = (int)reader["Id"],
                            AnswerText = reader["AnswerText"].ToString(),
                            IsCorrect = (bool)reader["IsCorrect"],
                            IsDynamic = (bool)reader["IsDynamic"],
                            CanBeSuggested = (bool)reader["CanBeSuggested"]
                        });
                    }
                }
            }
            return correctAnswers;
        }

        public void UpdateAnswer(Answer answer)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("UPDATE Answer SET AnswerText = @AnswerText, IsCorrect = @IsCorrect, IsDynamic = @IsDynamic, CanBeSuggested = @CanBeSuggested WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", answer.Id);
                command.Parameters.AddWithValue("@AnswerText", answer.AnswerText);
                command.Parameters.AddWithValue("@IsCorrect", answer.IsCorrect);
                command.Parameters.AddWithValue("@IsDynamic", answer.IsDynamic);
                command.Parameters.AddWithValue("@CanBeSuggested", answer.CanBeSuggested);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteAnswer(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DELETE FROM Answer WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        
    }
}
