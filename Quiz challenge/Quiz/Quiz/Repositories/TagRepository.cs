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
    }
}
