using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProblemSolvingReportSystem.Models;
using ProblemSolvingReportSystem.Models.TaskDir;
using ProblemSolvingReportSystem.Services;

namespace ProblemSolvingReportSystem.Controllers
{
    [ApiController]
    public class SolutionsController : ControllerBase
    {
        public ISolutionService service;

        public SolutionsController(ISolutionService service)
        {
            this.service = service;
        }

        [Route("solutions")]
        [HttpGet]
        public ActionResult<List<SolutionDto>> Get(int? taskId, int? userId)
        {
            return Ok(service.GetSolutions(taskId, userId));
        }

        [Route("solutions/{solutionId}/postmoderation")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<TaskDto>> Post(int solutionId, [FromBody] PostVerdictDto model)
        {
            return Ok(service.PostVerdict(solutionId, model));
        }


        [HttpPost("tasks/{id}/solution")]
        [Authorize]
        public ActionResult<SolutionDto> PostSolution(int id, PostSolutionModel model)
        {
            return Ok(service.PostSolution(id, model, User.Identity.Name));
        }
    }
}