using QuizChallenge.Models;
using System.Data.SqlClient;

namespace QuizChallenge.Repositories
{
    public class QuizTagRepository
    {
        private readonly string _connectionString;

        public QuizTagRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddQuizTag(QuizTag quizTag)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("INSERT INTO QuizTag (QuizId, TagId) VALUES (@QuizId, @TagId)", connection);
            command.Parameters.AddWithValue("@QuizId", quizTag.QuizId);
            command.Parameters.AddWithValue("@TagId", quizTag.TagId);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public List<QuizTag> GetQuizTagsByQuizId(int quizId)
        {
            var quizTags = new List<QuizTag>();
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("SELECT * FROM QuizTag WHERE QuizId = @QuizId", connection);
            command.Parameters.AddWithValue("@QuizId", quizId);
            connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                quizTags.Add(new QuizTag
                {
                    QuizId = (int)reader["QuizId"],
                    TagId = (int)reader["TagId"]
                });
            }
            return quizTags;
        }

        public void DeleteQuizTag(int quizId, int tagId)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("DELETE FROM QuizTag WHERE QuizId = @QuizId AND TagId = @TagId", connection);
            command.Parameters.AddWithValue("@QuizId", quizId);
            command.Parameters.AddWithValue("@TagId", tagId);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
