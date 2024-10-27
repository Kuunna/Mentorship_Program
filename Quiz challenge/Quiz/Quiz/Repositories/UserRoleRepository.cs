using QuizChallenge.Models;
using System.Data.SqlClient;

namespace QuizChallenge.Repositories
{
    public class UserRoleRepository
    {
        private readonly string _connectionString;

        public UserRoleRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUserRole(UserRole userRole)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("INSERT INTO UserRole (UserId, RoleId) VALUES (@UserId, @RoleId)", connection);
            command.Parameters.AddWithValue("@UserId", userRole.UserId);
            command.Parameters.AddWithValue("@RoleId", userRole.RoleId);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public UserRole GetUserRoleById(int userId, int roleId)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("SELECT * FROM UserRole WHERE UserId = @UserId AND RoleId = @RoleId", connection);
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@RoleId", roleId);
            connection.Open();

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new UserRole
                {
                    UserId = (int)reader["UserId"],
                    RoleId = (int)reader["RoleId"]
                };
            }
            return null;
        }

        public List<UserRole> GetRolesByUserId(int userId)
        {
            var userRoles = new List<UserRole>();
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("SELECT * FROM UserRole WHERE UserId = @UserId", connection);
            command.Parameters.AddWithValue("@UserId", userId);
            connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                userRoles.Add(new UserRole
                {
                    UserId = (int)reader["UserId"],
                    RoleId = (int)reader["RoleId"]
                });
            }
            return userRoles;
        }

        public void DeleteUserRole(int userId, int roleId)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("DELETE FROM UserRole WHERE UserId = @UserId AND RoleId = @RoleId", connection);
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@RoleId", roleId);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
