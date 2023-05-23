using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using ProblemSolvingReportSystem.Exceptions;
using ProblemSolvingReportSystem.Models;
using ProblemSolvingReportSystem.Models.TaskDir;
using ProblemSolvingReportSystem.Services;

namespace ProblemSolvingReportSystem.Controllers
{
    [Route("tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        public ITaskService service;
        private readonly IWebHostEnvironment _appEnvironment;

        public TaskController(ITaskService service, IWebHostEnvironment appEnvironment)
        {
            this.service = service;
            _appEnvironment = appEnvironment;
        }


        [HttpGet]
        public ActionResult<List<TasksDto>> Get(string? name, int? topicId)
        {
            return Ok(service.GetTasks(name, topicId));
        }

        [HttpPost]
        /*[Authorize(Roles = "Admin")]*/
        public ActionResult<List<TaskDto>> Post([FromBody] PostTaskModel model)
        {
            return Ok(service.PostTask(model));
        }

        [HttpGet("{id}")]
        public ActionResult<TaskDto> Get(int id)
        {
            return Ok(service.GetTask(id));
        }


        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<TaskDto> Patch(int id, [FromBody] PostTaskModel model)
        {
            return Ok(service.PatchTask(id, model));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<MessageOk>> Delete(int id)
        {
            await service.DeleteTask(id);
            return Ok(new MessageOk("OK"));
        }

        [HttpGet("{id}/input")]
        public ActionResult<PathDto> GetInput(int id)
        {
            string path = service.GetInput(id);
            if (path is null)
            {
                throw new ObjectNotFoundException("Not find input");
            }

            if (System.IO.File.Exists($"wwwroot{path}"))
            {
                return Ok(new PathDto
                {
                    path = path,
                });
            }
            else
            {
                throw new ObjectNotFoundException("Not find input");
            }
        }

        [HttpPost("{id}/input")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TaskDto>> Postinput(int id, IFormFile input)
        {
            if (input != null && input.ContentType == "text/plain")
            {
                string path = "/Files/" + input.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await input.CopyToAsync(fileStream);
                }

                await service.CreateInput(id, path);

                return Ok(service.GetTask(id));
            }

            throw new ValidationException("Bad data");
        }


        [HttpDelete("{id}/input")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteInput(int id)
        {
            string path = service.GetInput(id);
            if (System.IO.File.Exists($"wwwroot{path}"))
            {
                System.IO.File.Delete($"wwwroot{path}");
                await service.DeleteInput(id);
                return Ok(new MessageOk("OK"));
            }

            throw new ObjectNotFoundException("File maybe deleted earlier");
        }


        [HttpGet("{id}/output")]
        public FileResult GetOutput(int id)
        {
            string path = service.GetOutput(id);
            if (path is null)
            {
                throw new ObjectNotFoundException("Not find output");
            }

            if (System.IO.File.Exists($"wwwroot{path}"))
            {
                return File(path, MediaTypeNames.Text.Plain, "OutputFile.txt");
            }


            throw new ObjectNotFoundException("Not find output");
        }

        [HttpPost("{id}/output")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TaskDto>> PostOutput(int id, IFormFile output)
        {
            if (output != null && output.ContentType == "text/plain")
            {
                // путь к папке Files
                string path = "/Files/" + output.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await output.CopyToAsync(fileStream);
                }

                await service.CreateOutput(id, path);

                return Ok(service.GetTask(id));
            }

            throw new ValidationException("Bad data");
        }

        [HttpDelete("{id}/output")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOutput(int id)
        {
            string path = service.GetOutput(id);
            if (System.IO.File.Exists($"wwwroot{path}"))
            {
                System.IO.File.Delete($"wwwroot{path}");
                await service.DeleteOutput(id);
                return Ok(new MessageOk("OK"));
            }

            throw new ObjectNotFoundException("File maybe deleted earlier");
        }
    }
}