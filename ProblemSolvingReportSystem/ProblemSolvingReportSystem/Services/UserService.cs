using System.Security.Claims;
using System.Web.Helpers;
using ProblemSolvingReportSystem.Exceptions;
using ProblemSolvingReportSystem.Models.Data;
using ProblemSolvingReportSystem.Models.UserDir;

namespace ProblemSolvingReportSystem.Services
{
    public interface IUserService
    {
        public List<UserDto> GetUsers();
        public UserMyselfDto GetUser(int id, ClaimsPrincipal User);
        public Task Delete(int id);
        public UserMyselfDto Patch(int id, PatchUser model, ClaimsPrincipal User);
    }

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }


        public List<UserDto> GetUsers()
        {
            return _context.Users.Select(x => new UserDto
            {
                userId = x.userId,
                userName = x.userName,
                roleId = x.roleId
            }).ToList();
        }

        public UserMyselfDto GetUser(int id, ClaimsPrincipal User)
        {
            var user = _context.Users.FirstOrDefault(x => x.userId == id);

            if (user is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }

            if (user.userName == User.Identity.Name || User.IsInRole("Admin"))
            {
                return new UserMyselfDto(_context.Users.Find(id));
            }
            else
            {
                throw new NotPermissionException("Not enough permissions");
            }
        }

        public UserMyselfDto Patch(int id, PatchUser model, ClaimsPrincipal User)
        {
            var user = _context.Users.FirstOrDefault(x => x.userId == id);
            if (user is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }

            if (user.userName == User.Identity.Name || User.IsInRole("Admin"))
            {
                user.password = Crypto.HashPassword(model.password);
                user.name = model.name;
                user.surname = model.surname;
            }
            else
            {
                throw new NotPermissionException("Not enough permissions");
            }

            _context.SaveChanges();

            return new UserMyselfDto(_context.Users.Find(id));
        }


        public async Task Delete(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.userId == id);
            if (user is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task Role(int id, RoleBody model)
        {
            var user = _context.Users.FirstOrDefault(x => x.userId == id);
            if (user is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }

            user.roleId = model.roleId;
            await _context.SaveChangesAsync();
        }
    }
}