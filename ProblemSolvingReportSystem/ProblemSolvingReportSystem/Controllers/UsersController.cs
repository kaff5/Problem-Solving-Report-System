using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProblemSolvingReportSystem.Models;
using ProblemSolvingReportSystem.Models.UserDir;
using ProblemSolvingReportSystem.Services;

namespace ProblemSolvingReportSystem.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public IUserService service;

        public UsersController(IUserService service)
        {
            this.service = service;
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<UserDto>> Get()
        {
            return Ok(service.GetUsers());
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<UserMyselfDto> Get(int id)
        {
            return Ok(service.GetUser(id, User));
        }

        [HttpPatch("{id}")]
        [Authorize]
        public ActionResult<UserMyselfDto> Patch(int id, PatchUser model)
        {
            return service.Patch(id, model, User);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<MessageOk>> Delete(int id)
        {
            await service.Delete(id);
            return Ok(new MessageOk("OK"));
        }
    }
}