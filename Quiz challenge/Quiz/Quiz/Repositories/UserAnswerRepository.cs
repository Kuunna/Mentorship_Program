using QuizChallenge.Models;
using System.Data.SqlClient;

namespace QuizChallenge.Repositories
{
    public class UserAnswerRepository
    {
        private readonly string _connectionString;

        public UserAnswerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUserAnswer(UserAnswer userAnswer)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(
                    "INSERT INTO UserAnswers (UserQuizId, QuestionId, AnswerId, FreeText) VALUES (@UserQuizId, @QuestionId, @AnswerId, @FreeText)",
                    connection
                );
                command.Parameters.AddWithValue("@UserQuizId", userAnswer.UserQuizId);
                command.Parameters.AddWithValue("@QuestionId", userAnswer.QuestionId);
                command.Parameters.AddWithValue("@AnswerId", userAnswer.AnswerId);
                command.Parameters.AddWithValue("@FreeText", userAnswer.FreeText ?? (object)DBNull.Value);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public UserAnswer GetUserAnswerById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM UserAnswers WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new UserAnswer
                        {
                            Id = (int)reader["Id"],
                            UserQuizId = (int)reader["UserQuizId"],
                            QuestionId = (int)reader["QuestionId"],
                            AnswerId = (int)reader["AnswerId"],
                            FreeText = reader["FreeText"] as string
                        };
                    }
                }
            }
            return null;
        }

        public List<UserAnswer> GetUserAnswersByUserQuizId(int userQuizId)
        {
            var userAnswers = new List<UserAnswer>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM UserAnswers WHERE UserQuizId = @UserQuizId", connection);
                command.Parameters.AddWithValue("@UserQuizId", userQuizId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userAnswers.Add(new UserAnswer
                        {
                            Id = (int)reader["Id"],
                            UserQuizId = (int)reader["UserQuizId"],
                            QuestionId = (int)reader["QuestionId"],
                            AnswerId = (int)reader["AnswerId"],
                            FreeText = reader["FreeText"] as string
                        });
                    }
                }
            }
            return userAnswers;
        }

        public void UpdateUserAnswer(UserAnswer userAnswer)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(
                    "UPDATE UserAnswers SET UserQuizId = @UserQuizId, QuestionId = @QuestionId, AnswerId = @AnswerId, FreeText = @FreeText WHERE Id = @Id",
                    connection
                );
                command.Parameters.AddWithValue("@UserQuizId", userAnswer.UserQuizId);
                command.Parameters.AddWithValue("@QuestionId", userAnswer.QuestionId);
                command.Parameters.AddWithValue("@AnswerId", userAnswer.AnswerId);
                command.Parameters.AddWithValue("@FreeText", userAnswer.FreeText ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Id", userAnswer.Id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteUserAnswer(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DELETE FROM UserAnswers WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
