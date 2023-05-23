using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProblemSolvingReportSystem.Models.RoleDir;
using ProblemSolvingReportSystem.Services;

namespace ProblemSolvingReportSystem.Controllers
{
    [Route("roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        public IRoleService service;

        public RoleController(IRoleService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<List<RoleDto>> Get()
        {
            return Ok(service.GetRoles());
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<RoleDto> Get(int id)
        {
            return Ok(service.GetRole(id));
        }
    }
}