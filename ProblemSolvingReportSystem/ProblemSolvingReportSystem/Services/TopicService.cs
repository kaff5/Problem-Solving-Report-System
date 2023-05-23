using ProblemSolvingReportSystem.Exceptions;
using ProblemSolvingReportSystem.Models.Data;
using ProblemSolvingReportSystem.Models.TaskDir;
using ProblemSolvingReportSystem.Models.TopicDir;

namespace ProblemSolvingReportSystem.Services
{
    public interface ITopicService
    {
        public List<TopicsDto> GetTopics(string? name, int? parentId);
        public List<TopicsDtoWithChild> CreateTopic(TopicPostModelDto model);

        public List<TopicsDtoWithChild> GetTopic(int id);

        public Task Delete(int id);

        public List<TopicsDtoWithChild> PatchTopic(int id, TopicPatchModel model);

        public List<ChildsTopicDto> GetChild(int id);

        public List<TopicsDtoWithChild> PostChild(int id, List<int> model);

        public List<TopicsDtoWithChild> DeleteChild(int id, List<int> model);
    }


    public class TopicService : ITopicService
    {
        private readonly ApplicationDbContext _context;

        public TopicService(ApplicationDbContext context)
        {
            _context = context;
        }


        public List<TopicsDto> GetTopics(string? name, int? parentId)
        {
            if (name is not null && parentId is not null)
            {
                List<Topic> topics = _context.Topics.Where(x => (x.name == name) && (x.parentId == parentId)).ToList();
                List<TopicsDto> topicsDto = new List<TopicsDto>();

                foreach (Topic topic in topics)
                {
                    TopicsDto topicDto = new TopicsDto()
                    {
                        id = topic.id,
                        name = topic.name,
                        parentId = topic.parentId
                    };
                    topicsDto.Add(topicDto);
                }

                return topicsDto;
            }

            if (name is not null)
            {
                List<Topic> topics = _context.Topics.Where(x => x.name == name).ToList();
                List<TopicsDto> topicsDto = new List<TopicsDto>();

                foreach (Topic topic in topics)
                {
                    TopicsDto topicDto = new TopicsDto()
                    {
                        id = topic.id,
                        name = topic.name,
                        parentId = topic.parentId
                    };
                    topicsDto.Add(topicDto);
                }

                return topicsDto;
            }

            if (parentId is not null)
            {
                List<Topic> topics = _context.Topics.Where(x => x.parentId == parentId).ToList();
                List<TopicsDto> topicsDto = new List<TopicsDto>();

                foreach (Topic topic in topics)
                {
                    TopicsDto topicDto = new TopicsDto()
                    {
                        id = topic.id,
                        name = topic.name,
                        parentId = topic.parentId
                    };
                    topicsDto.Add(topicDto);
                }

                return topicsDto;
            }

            return _context.Topics.Select(x => new TopicsDto
            {
                id = x.id,
                name = x.name,
                parentId = x.parentId,
            }).ToList();
        }


        public List<TopicsDtoWithChild> CreateTopic(TopicPostModelDto model)
        {
            Topic topic = new Topic
            {
                id = 0,
                name = model.name,
                parentId = model.parentId
            };

            _context.Topics.Add(topic);
            _context.SaveChanges();

            if (topic.parentId is not null)
            {
                return GetTopic((int)topic.parentId);
            }

            List<ChildsTopicDto> child = new List<ChildsTopicDto>();

            return new List<TopicsDtoWithChild>()
            {
                new TopicsDtoWithChild()
                {
                    id = topic.id,
                    name = topic.name,
                    parentId = topic.parentId,
                    childs = child
                }
            };
        }

        public List<TopicsDtoWithChild> GetTopic(int id)
        {
            Topic topic = _context.Topics.FirstOrDefault(x => x.id == id);
            if (topic is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }

            List<Topic> childs = new List<Topic>(_context.Topics.Where(x => x.parentId == topic.id));
            List<ChildsTopicDto> topicsDto = new List<ChildsTopicDto>();

            foreach (Topic child in childs)
            {
                topicsDto.Add(new ChildsTopicDto()
                {
                    id = child.id,
                    name = child.name,
                    parentId = child.parentId
                });
            }

            return new List<TopicsDtoWithChild>()
            {
                new TopicsDtoWithChild()
                {
                    id = topic.id,
                    name = topic.name,
                    parentId = topic.parentId,
                    childs = topicsDto
                }
            };
        }


        public List<TopicsDtoWithChild> PatchTopic(int id, TopicPatchModel model)
        {
            Topic topic = _context.Topics.FirstOrDefault(x => x.id == id);
            if (topic is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }

            topic.name = model.name;
            topic.parentId = model.parentId;

            _context.SaveChanges();

            return GetTopic(id);
        }


        public async Task Delete(int id)
        {
            Topic topic = _context.Topics.FirstOrDefault(x => x.id == id);
            if (topic is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }

            List<Topic> listChilds = _context.Topics.Where(x => x.parentId == id).ToList();

            foreach (Topic child in listChilds)
            {
                FindChilds(child);
            }

            List<Task1> listTasks = _context.Tasks.Where(x => x.topicId == topic.id).ToList();
            foreach (Task1 task in listTasks)
            {
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
            }


            DeleteTopic(topic);
            await _context.SaveChangesAsync();
        }

        void FindChilds(Topic topic)
        {
            List<Topic> listChild = _context.Topics.Where(x => x.parentId == topic.id).ToList();

            foreach (Topic child in listChild)
            {
                FindChilds(child);
            }

            List<Task1> listTasks = _context.Tasks.Where(x => x.topicId == topic.id).ToList();
            foreach (Task1 task in listTasks)
            {
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
            }

            DeleteTopic(topic);
        }


        void DeleteTopic(Topic topic)
        {
            _context.Topics.Remove(topic);
        }

        public List<ChildsTopicDto> GetChild(int id)
        {
            Topic topic = _context.Topics.Find(id);
            if (topic is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }

            List<ChildsTopicDto> childs = new List<ChildsTopicDto>();

            var topics = _context.Topics.Where(x => x.parentId == topic.id);
            foreach (var child in topics)
            {
                childs.Add(new ChildsTopicDto()
                {
                    id = child.id,
                    name = child.name,
                    parentId = child.parentId,
                });
            }

            return childs;
        }

        public List<TopicsDtoWithChild> PostChild(int id, List<int> model)
        {
            var topic = _context.Topics.Find(id);
            if (topic is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }

            foreach (int child in model)
            {
                Topic childT = _context.Topics.Find(child);
                childT.parentId = topic.id;
                _context.SaveChanges();
            }

            return GetTopic(id);
        }

        public List<TopicsDtoWithChild> DeleteChild(int id, List<int> model)
        {
            var topic = _context.Topics.Find(id);
            if (topic is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }


            foreach (int n in model)
            {
                Topic childTopic = _context.Topics.Find(n);
                if (childTopic is null)
                {
                    throw new ObjectNotFoundException("Check if the topic has child");
                }

                childTopic.parentId = null;
                _context.SaveChanges();
            }

            return GetTopic(id);
        }
    }
}