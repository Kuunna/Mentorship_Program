using QuizChallenge.Models;
using System.Data.SqlClient;

namespace QuizChallenge.Repositories
{
    public class RoleRepository
    {
        private readonly string _connectionString;

        public RoleRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddRole(Role role)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("INSERT INTO Role (RoleName) VALUES (@RoleName)", connection);
            command.Parameters.AddWithValue("@RoleName", role.RoleName);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public Role GetRoleById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("SELECT * FROM Role WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Role
                {
                    Id = (int)reader["Id"],
                    RoleName = reader["RoleName"].ToString()
                };
            }
            return null;
        }

        public List<Role> GetAllRoles()
        {
            var roles = new List<Role>();
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("SELECT * FROM Role", connection);
            connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                roles.Add(new Role
                {
                    Id = (int)reader["Id"],
                    RoleName = reader["RoleName"].ToString()
                });
            }
            return roles;
        }

        public void UpdateRole(Role role)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("UPDATE Role SET RoleName = @RoleName WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", role.Id);
            command.Parameters.AddWithValue("@RoleName", role.RoleName);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public void DeleteRole(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("DELETE FROM Role WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
