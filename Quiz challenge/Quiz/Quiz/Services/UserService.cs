using QuizChallenge.Models;
using QuizChallenge.Repositories;
using System.Linq;

namespace QuizChallenge.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly RoleRepository _roleRepository;

        public UserService(UserRepository userRepository, RoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }
        public User AuthenticateUser(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username);
            return (user != null && user.Password == password) ? user : null;
        }

        public void CreateUser(User user) => _userRepository.AddUser(user);

        public User GetUserById(int id) => _userRepository.GetUserById(id);

        public void UpdateUser(User user) => _userRepository.UpdateUser(user);

        public void DeleteUser(int userId) => _userRepository.DeleteUser(userId);

        public void UpdateUserProfile(User user) => _userRepository.UpdateUser(user);

        public void AssignRoleToUser(int userId, int roleId) => _roleRepository.AssignRoleToUser(userId, roleId);

        public List<Role> GetUserRoles(int userId) => _roleRepository.GetRolesByUserId(userId);
    }
}
