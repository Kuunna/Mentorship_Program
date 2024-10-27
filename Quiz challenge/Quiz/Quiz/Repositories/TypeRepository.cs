using QuizChallenge.Models;
using System.Data.SqlClient;


namespace QuizChallenge.Repositories
{
    public class TypeRepository
    {
        private readonly string _connectionString;

        public TypeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddType(Models.Type type)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("INSERT INTO Type (TypeName) VALUES (@TypeName)", connection);
            command.Parameters.AddWithValue("@TypeName", type.TypeName);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public Models.Type GetTypeById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("SELECT * FROM Type WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Models.Type
                {
                    Id = (int)reader["Id"],
                    TypeName = reader["TypeName"].ToString()
                };
            }
            return null;
        }

        public List<Models.Type> GetAllTypes()
        {
            var types = new List<Models.Type>();
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("SELECT * FROM Type", connection);
            connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                types.Add(new Models.Type
                {
                    Id = (int)reader["Id"],
                    TypeName = reader["TypeName"].ToString()
                });
            }
            return types;
        }

        public void UpdateType(Models.Type type)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("UPDATE Type SET TypeName = @TypeName WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", type.Id);
            command.Parameters.AddWithValue("@TypeName", type.TypeName);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public void DeleteType(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("DELETE FROM Type WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
