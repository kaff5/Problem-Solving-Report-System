using Microsoft.EntityFrameworkCore;
using ProblemSolvingReportSystem.Exceptions;
using ProblemSolvingReportSystem.Models.Data;
using ProblemSolvingReportSystem.Models.TaskDir;

namespace ProblemSolvingReportSystem.Services
{
    public interface ITaskService
    {
        List<TasksDto> GetTasks(string? name, int? topicId);
        public List<TaskDto> PostTask(PostTaskModel model);
        public TaskDto GetTask(int id);
        public TaskDto PatchTask(int id, PostTaskModel model);
        public Task DeleteTask(int id);
        public Task CreateInput(int id, string path);
        public string GetInput(int id);
        public string GetOutput(int id);
        public Task DeleteInput(int id);
        public Task DeleteOutput(int id);
        public Task CreateOutput(int id, string path);
    }

    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;

        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<TasksDto> GetTasks(string? name, int? topicId)
        {
            if (name is not null && topicId is not null)
            {
                List<Task1> tasks = _context.Tasks.Where(x => (x.name == name) && (x.topicId == topicId)).ToList();
                List<TasksDto> tasksDto = new List<TasksDto>();

                foreach (Task1 task in tasks)
                {
                    TasksDto taskdto = new TasksDto()
                    {
                        id = task.id,
                        name = task.name,
                        topicId = task.topicId
                    };
                    tasksDto.Add(taskdto);
                }

                return tasksDto;
            }

            if (name is not null)
            {
                List<Task1> tasks = _context.Tasks.Where(x => x.name == name).ToList();
                List<TasksDto> tasksDto = new List<TasksDto>();

                foreach (Task1 task in tasks)
                {
                    TasksDto taskdto = new TasksDto()
                    {
                        id = task.id,
                        name = task.name,
                        topicId = task.topicId
                    };
                    tasksDto.Add(taskdto);
                }

                return tasksDto;
            }

            if (topicId is not null)
            {
                List<Task1> tasks = _context.Tasks.Where(x => x.topicId == topicId).ToList();
                List<TasksDto> tasksDto = new List<TasksDto>();

                foreach (Task1 task in tasks)
                {
                    TasksDto taskdto = new TasksDto()
                    {
                        id = task.id,
                        name = task.name,
                        topicId = task.topicId
                    };
                    tasksDto.Add(taskdto);
                }

                return tasksDto;
            }


            return _context.Tasks.Select(x => new TasksDto
            {
                id = x.id,
                name = x.name,
                topicId = x.topicId
            }).ToList();
        }

        public List<TaskDto> PostTask(PostTaskModel model)
        {
            Task1 task = new Task1
            {
                id = 0,
                name = model.name,
                topicId = model.topicId,
                description = model.description,
                price = model.price,
                isDraft = false
            };

            _context.Tasks.Add(task);
            _context.SaveChanges();

            return new List<TaskDto>
            {
                new TaskDto
                {
                    id = task.id,
                    name = task.name,
                    topicId = task.topicId,
                    description = task.description,
                    price = task.price,
                    isDraft = task.isDraft
                }
            };
        }

        public TaskDto GetTask(int id)
        {
            var task = _context.Tasks.FirstOrDefault(x => x.id == id);

            if (task is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }

            return new TaskDto
            {
                id = task.id,
                name = task.name,
                topicId = task.topicId,
                description = task.description,
                price = task.price,
                isDraft = task.isDraft
            };
        }

        public TaskDto PatchTask(int id, PostTaskModel model)
        {
            var task = _context.Tasks.FirstOrDefault(x => x.id == id);

            if (task is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }

            task.name = model.name;
            task.topicId = model.topicId;
            task.description = model.description;
            task.price = model.price;

            _context.SaveChanges();

            return new TaskDto
            {
                id = task.id,
                name = task.name,
                topicId = task.topicId,
                description = task.description,
                price = task.price,
                isDraft = task.isDraft
            };
        }

        public async Task DeleteTask(int id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.id == id);
            if (task is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }

            if (task.input != null)
            {
                if (System.IO.File.Exists($"wwwroot{task.input}"))
                {
                    System.IO.File.Delete($"wwwroot{task.input}");
                }
            }

            if (task.output != null)
            {
                if (System.IO.File.Exists($"wwwroot{task.output}"))
                {
                    System.IO.File.Delete($"wwwroot{task.output}");
                }
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }

        public string GetInput(int id)
        {
            Task1 task = _context.Tasks.Find(id);

            if (task is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }
            return task.input;
        }


        public async Task CreateInput(int id, string path)
        {
            Task1 task = _context.Tasks.Find(id);

            if (task is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }

            task.input = path;
            _context.SaveChanges();
        }

        public async Task DeleteInput(int id)
        {
            Task1 task = _context.Tasks.Find(id);

            if (task is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }

            task.input = null;
            _context.SaveChanges();
        }

        public string GetOutput(int id)
        {
            Task1 task = _context.Tasks.Find(id);

            if (task is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }

            return task.output;
        }

        public async Task CreateOutput(int id, string path)
        {
            Task1 task = _context.Tasks.Find(id);

            if (task is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }

            task.output = path;
            _context.SaveChanges();
        }

        public async Task DeleteOutput(int id)
        {
            Task1 task = _context.Tasks.Find(id);

            if (task is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }

            task.output = null;
            _context.SaveChanges();
        }
    }
}