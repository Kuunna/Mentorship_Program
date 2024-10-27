using QuizChallenge.Models;
using System.Data.SqlClient;

namespace QuizChallenge.Repositories
{
    public class UserQuizRepository
    {
        private readonly string _connectionString;

        public UserQuizRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUserQuiz(UserQuiz userQuiz)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("INSERT INTO UserQuiz (UserId, QuizId, CompletionTime, AttemptAt) VALUES (@UserId, @QuizId, @CompletionTime, @AttemptAt)", connection);
            command.Parameters.AddWithValue("@UserId", userQuiz.UserId);
            command.Parameters.AddWithValue("@QuizId", userQuiz.QuizId);
            command.Parameters.AddWithValue("@CompletionTime", userQuiz.CompletionTime);
            command.Parameters.AddWithValue("@AttemptAt", userQuiz.AttemptAt);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public UserQuiz GetUserQuizById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("SELECT * FROM UserQuiz WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new UserQuiz
                {
                    Id = (int)reader["Id"],
                    UserId = (int)reader["UserId"],
                    QuizId = (int)reader["QuizId"],
                    CompletionTime = (DateTime)reader["CompletionTime"],
                    AttemptAt = (DateTime)reader["AttemptAt"]
                };
            }
            return null;
        }

        public List<UserQuiz> GetUserQuizzesByUserId(int userId)
        {
            var userQuizzes = new List<UserQuiz>();
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("SELECT * FROM UserQuiz WHERE UserId = @UserId", connection);
            command.Parameters.AddWithValue("@UserId", userId);
            connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                userQuizzes.Add(new UserQuiz
                {
                    Id = (int)reader["Id"],
                    UserId = (int)reader["UserId"],
                    QuizId = (int)reader["QuizId"],
                    CompletionTime = (DateTime)reader["CompletionTime"],
                    AttemptAt = (DateTime)reader["AttemptAt"]
                });
            }
            return userQuizzes;
        }

        public void UpdateUserQuiz(UserQuiz userQuiz)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("UPDATE UserQuiz SET QuizId = @QuizId, CompletionTime = @CompletionTime, AttemptAt = @AttemptAt WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", userQuiz.Id);
            command.Parameters.AddWithValue("@QuizId", userQuiz.QuizId);
            command.Parameters.AddWithValue("@CompletionTime", userQuiz.CompletionTime);
            command.Parameters.AddWithValue("@AttemptAt", userQuiz.AttemptAt);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public void DeleteUserQuiz(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("DELETE FROM UserQuiz WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
