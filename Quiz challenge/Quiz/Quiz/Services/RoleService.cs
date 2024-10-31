using QuizChallenge.Models;
using QuizChallenge.Repositories;

namespace QuizChallenge.Services
{
    public class RoleService
    {
        private readonly RoleRepository _roleRepository;

        public RoleService(RoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public void CreateRole(Role role) => _roleRepository.AddRole(role);

        public List<Role> GetRoles() => _roleRepository.GetAllRoles();

        public void AssignRoleToUser(int userId, int roleId) => _roleRepository.AssignRoleToUser(userId, roleId);

        public List<User> GetUsersByRole(int roleId) => _roleRepository.GetUsersByRoleId(roleId);
    }

}
