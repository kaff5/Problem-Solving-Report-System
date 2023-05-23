using ProblemSolvingReportSystem.Exceptions;
using ProblemSolvingReportSystem.Models.Data;
using ProblemSolvingReportSystem.Models.RoleDir;

namespace ProblemSolvingReportSystem.Services
{
    public interface IRoleService
    {
        public List<RoleDto> GetRoles();
        public RoleDto GetRole(int id);
    }

    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _context;

        public RoleService(ApplicationDbContext context)
        {
            _context = context;
        }


        public List<RoleDto> GetRoles()
        {
            return _context.Roles.Select(x => new RoleDto
            {
                roleId = x.roleId,
                name = x.name
            }).ToList();
        }

        public RoleDto GetRole(int id)
        {
            Role role = _context.Roles.Find(id);
            if (role is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }

            RoleDto roleDto = new RoleDto
            {
                roleId = role.roleId,
                name = role.name
            };
            return roleDto;
        }
    }
}