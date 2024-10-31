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

        public void AssignRoleToUser(int userId, int roleId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "INSERT INTO UserRoles (UserId, RoleId) VALUES (@UserId, @RoleId)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@RoleId", roleId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Role> GetRolesByUserId(int userId)
        {
            var roles = new List<Role>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT r.* FROM Roles r INNER JOIN UserRoles ur ON r.Id = ur.RoleId WHERE ur.UserId = @UserId", connection);
                command.Parameters.AddWithValue("@UserId", userId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        roles.Add(new Role
                        {
                            Id = (int)reader["Id"],
                            RoleName = reader["RoleName"].ToString()
                        });
                    }
                }
            }
            return roles;
        }

        public List<User> GetUsersByRoleId(int roleId)
        {
            var users = new List<User>();
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(
                @"SELECT u.Id, u.UserName, u.Email, u.Password, u.CreatedAt, u.UpdatedAt
              FROM Users u
              INNER JOIN UserRoles ur ON u.Id = ur.UserId
              WHERE ur.RoleId = @RoleId", connection);
            command.Parameters.AddWithValue("@RoleId", roleId);
            connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                users.Add(new User
                {
                    Id = (int)reader["Id"],
                    UserName = reader["UserName"].ToString(),
                    Email = reader["Email"].ToString(),
                    Password = reader["Password"].ToString(),
                    CreatedAt = (DateTime)reader["CreatedAt"],
                    UpdatedAt = (DateTime)reader["UpdatedAt"]
                });
            }
            return users;
        }
    }
}
