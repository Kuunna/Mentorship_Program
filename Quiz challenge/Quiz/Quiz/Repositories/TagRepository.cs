using QuizChallenge.Models;
using System.Data.SqlClient;

namespace QuizChallenge.Repositories
{
    public class TagRepository
    {
        private readonly string _connectionString;

        public TagRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddTag(Tag tag)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("INSERT INTO Tag (TagName) VALUES (@TagName)", connection);
            command.Parameters.AddWithValue("@TagName", tag.TagName);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public Tag GetTagById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("SELECT * FROM Tag WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Tag
                {
                    Id = (int)reader["Id"],
                    TagName = reader["TagName"].ToString()
                };
            }
            return null;
        }

        public List<Tag> GetAllTags()
        {
            var tags = new List<Tag>();
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("SELECT * FROM Tag", connection);
            connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                tags.Add(new Tag
                {
                    Id = (int)reader["Id"],
                    TagName = reader["TagName"].ToString()
                });
            }
            return tags;
        }

        public void UpdateTag(Tag tag)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("UPDATE Tag SET TagName = @TagName WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", tag.Id);
            command.Parameters.AddWithValue("@TagName", tag.TagName);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public void DeleteTag(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("DELETE FROM Tag WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();
            command.ExecuteNonQuery();
        }
        public List<Quiz> GetQuizzesByTag(int tagId)
        {
            var quizzes = new List<Quiz>();
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(@"SELECT q.Id, q.Title, q.CreatedAt, q.Description, q.TimeLimit
                                           FROM Quiz q
                                           INNER JOIN QuizTag qt ON q.Id = qt.QuizId
                                           WHERE qt.TagId = @TagId", connection);
            command.Parameters.AddWithValue("@TagId", tagId);
            connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                quizzes.Add(new Quiz
                {
                    Id = (int)reader["Id"],
                    Title = reader["Title"].ToString(),
                    CreatedAt = (DateTime)reader["CreatedAt"],
                    Description = reader["Description"].ToString(),
                    TimeLimit = (int)reader["TimeLimit"]
                });
            }
            return quizzes;
        }
    }
}
