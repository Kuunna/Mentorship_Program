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

        public List<Question> GetQuestionsByQuizId(int quizId)
        {
            var questions = new List<Question>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(@"SELECT * FROM Questions 
                                               WHERE Id IN (
                                                    SELECT QuestionId 
                                                    FROM QuizQuestions 
                                                    WHERE QuizId = @QuizId
                                                    )", connection);
                command.Parameters.AddWithValue("@QuizId", quizId);
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

        public List<Question> GetQuestionsByLevel(string level)
        {
            var questions = new List<Question>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Questions WHERE LevelId = (SELECT Id FROM Levels WHERE LevelName = @Level)", connection);
                command.Parameters.AddWithValue("@Level", level);
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

        public List<Question> GetQuestionsByTopic(string topic)
        {
            var questions = new List<Question>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Questions WHERE TopicId = (SELECT Id FROM Topics WHERE TopicName = @Topic)", connection);
                command.Parameters.AddWithValue("@Topic", topic);
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

        public List<Models.Type> GetQuestionTypes()
        {
            var questionTypes = new List<Models.Type>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT Id, TypeName FROM Types", connection);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        questionTypes.Add(new Models.Type
                        {
                            Id = reader.GetInt32(0),
                            TypeName = reader.GetString(1)
                        });
                    }
                }
            }

            return questionTypes;
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
        public bool IsCorrectAnswer(int questionId, Answer answer)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(@"
                SELECT COUNT(*)
                FROM QuestionAnswer qa
                JOIN Answers a ON qa.AnswerId = a.Id
                WHERE qa.QuestionId = @QuestionId AND a.IsCorrect = 1 AND a.Id = @AnswerId", connection);
                command.Parameters.AddWithValue("@QuestionId", questionId);
                command.Parameters.AddWithValue("@AnswerId", answer.Id);

                connection.Open();
                var result = (int)command.ExecuteScalar();
                return result > 0; 
            }
        }

        public double GetScoreWeightByLevel(int levelId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT ScoreWeight FROM Levels WHERE Id = @LevelId", connection);
                command.Parameters.AddWithValue("@LevelId", levelId);
                connection.Open();

                var scoreWeight = command.ExecuteScalar();
                return scoreWeight != null ? (double)scoreWeight : 0;
            }
        }

        public int GetQuestionIdByAnswerId(int answerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(@" SELECT QuestionId FROM QuestionAnswer WHERE AnswerId = @AnswerId", connection);
                command.Parameters.AddWithValue("@AnswerId", answerId);

                connection.Open();
                var result = command.ExecuteScalar();
                return result != null ? (int)result : -1; // Trả về -1 nếu không tìm thấy
            }
        }
    }
}
