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
            var command = new SqlCommand(
                @"INSERT INTO UserQuiz (UserId, QuizId, TotalQuestions, CompletionTime, StartTime, Score, CorrectAnswers)
              VALUES (@UserId, @QuizId, @TotalQuestions, @CompletionTime, @StartTime, @Score, @CorrectAnswers)", connection);

            command.Parameters.AddWithValue("@UserId", userQuiz.UserId);
            command.Parameters.AddWithValue("@QuizId", userQuiz.QuizId);
            command.Parameters.AddWithValue("@TotalQuestions", userQuiz.TotalQuestions);
            command.Parameters.AddWithValue("@CompletionTime", userQuiz.CompletionTime);
            command.Parameters.AddWithValue("@StartTime", userQuiz.StartTime);
            command.Parameters.AddWithValue("@Score", userQuiz.Score);
            command.Parameters.AddWithValue("@CorrectAnswers", userQuiz.CorrectAnswers);

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
                    TotalQuestions = (int)reader["TotalQuestions"],
                    CompletionTime = (int)reader["CompletionTime"],
                    StartTime = (DateTime)reader["StartTime"],
                    EndTime = reader["EndTime"] as DateTime?,
                    Score = (int)reader["Score"],
                    CorrectAnswers = (int)reader["CorrectAnswers"]
                };
            }
            return null;
        }

        public UserQuiz GetUserQuizByUserIdAndQuizId(int userId, int quizId)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("SELECT * FROM UserQuiz WHERE UserId = @UserId AND QuizId = @QuizId", connection);
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@QuizId", quizId);
            connection.Open();

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new UserQuiz
                {
                    Id = (int)reader["Id"],
                    UserId = (int)reader["UserId"],
                    QuizId = (int)reader["QuizId"],
                    TotalQuestions = (int)reader["TotalQuestions"],
                    CompletionTime = (int)reader["CompletionTime"],
                    StartTime = (DateTime)reader["StartTime"],
                    EndTime = reader["EndTime"] as DateTime?,
                    Score = (int)reader["Score"],
                    CorrectAnswers = (int)reader["CorrectAnswers"]
                };
            }
            return null;
        }

        public List<UserQuiz> GetUserQuizByUserId(int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("SELECT * FROM UserQuiz WHERE UserId = @UserId", connection);
            command.Parameters.AddWithValue("@UserId", userId);
            connection.Open();

            using var reader = command.ExecuteReader();
            var userQuizzes = new List<UserQuiz>();
            while (reader.Read())
            {
                userQuizzes.Add(new UserQuiz
                {
                    Id = (int)reader["Id"],
                    UserId = (int)reader["UserId"],
                    QuizId = (int)reader["QuizId"],
                    TotalQuestions = (int)reader["TotalQuestions"],
                    CompletionTime = (int)reader["CompletionTime"],
                    StartTime = (DateTime)reader["StartTime"],
                    EndTime = reader["EndTime"] as DateTime?,
                    Score = (int)reader["Score"],
                    CorrectAnswers = (int)reader["CorrectAnswers"]
                });
            }
            return userQuizzes;
        }

        public double GetUserScoresByQuizId(int userId, int quizId)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("SELECT Score FROM UserQuiz WHERE UserId = @UserId AND QuizId = @QuizId", connection);
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@QuizId", quizId);
            connection.Open();

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return (double)reader["Score"];
            }
            return 0;
        }

        public void UpdateUserQuiz(UserQuiz userQuiz)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(
                @"UPDATE UserQuiz 
                  SET QuizId = @QuizId, CompletionTime = @CompletionTime, TotalQuestions = @TotalQuestions, 
                      StartTime = @StartTime, EndTime = @EndTime, 
                      Score = @Score, CorrectAnswers = @CorrectAnswers
                  WHERE Id = @Id", connection);

            command.Parameters.AddWithValue("@Id", userQuiz.Id);
            command.Parameters.AddWithValue("@QuizId", userQuiz.QuizId);
            command.Parameters.AddWithValue("@CompletionTime", userQuiz.CompletionTime);
            command.Parameters.AddWithValue("@TotalQuestions", userQuiz.TotalQuestions);
            command.Parameters.AddWithValue("@StartTime", userQuiz.StartTime);
            command.Parameters.AddWithValue("@EndTime", userQuiz.EndTime ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Score", userQuiz.Score);
            command.Parameters.AddWithValue("@CorrectAnswers", userQuiz.CorrectAnswers);

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

        public void SaveUserScore(int userId, double totalScore)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("UPDATE UserQuiz SET Score = @Score WHERE UserId = @UserId", connection);
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@Score", totalScore);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public void SaveQuizCompletionTime(int userId, int quizId, DateTime completionTime)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("UPDATE UserQuiz SET EndTime = @EndTime WHERE UserId = @UserId AND QuizId = @QuizId", connection);
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@QuizId", quizId);
            command.Parameters.AddWithValue("@EndTime", completionTime);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
