using QuizChallenge.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace QuizChallenge.Repositories
{
    public class QuestionRepository
    {
        private readonly string _connectionString;

        public QuestionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddQuestion(Question question)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("INSERT INTO Questions (Content, Format, LevelId, TopicId, TypeId) VALUES (@Content, @Format, @LevelId, @TopicId, @TypeId)", connection);
                command.Parameters.AddWithValue("@Content", question.Content);
                command.Parameters.AddWithValue("@Format", question.Format);
                command.Parameters.AddWithValue("@LevelId", question.LevelId);
                command.Parameters.AddWithValue("@TopicId", question.TopicId);
                command.Parameters.AddWithValue("@TypeId", question.TypeId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public Question GetQuestionById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Questions WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Question
                        {
                            Id = (int)reader["Id"],
                            Content = reader["Content"].ToString(),
                            Format = reader["Format"].ToString(),
                            LevelId = (int)reader["LevelId"],
                            TopicId = (int)reader["TopicId"],
                            TypeId = (int)reader["TypeId"]
                        };
                    }
                }
            }
            return null;
        }

        public List<Question> GetQuestionsByLevelIdAndTopicId(int levelId, int topicId)
        {
            var questions = new List<Question>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Questions WHERE LevelId = @LevelId AND TopicId = @TopicId", connection);
                command.Parameters.AddWithValue("@LevelId", levelId);
                command.Parameters.AddWithValue("@TopicId", topicId);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        questions.Add(new Question
                        {
                            Id = (int)reader["Id"],
                            Content = reader["Content"].ToString(),
                            Format = reader["Format"].ToString(),
                            LevelId = (int)reader["LevelId"],
                            TopicId = (int)reader["TopicId"],
                            TypeId = (int)reader["TypeId"]
                        });
                    }
                }
            }
            return questions;
        }

        public void UpdateQuestion(Question question)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("UPDATE Questions SET Content = @Content, Format = @Format, LevelId = @LevelId, TopicId = @TopicId, TypeId = @TypeId WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", question.Id);
                command.Parameters.AddWithValue("@Content", question.Content);
                command.Parameters.AddWithValue("@Format", question.Format);
                command.Parameters.AddWithValue("@LevelId", question.LevelId);
                command.Parameters.AddWithValue("@TopicId", question.TopicId);
                command.Parameters.AddWithValue("@TypeId", question.TypeId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteQuestion(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DELETE FROM Questions WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
