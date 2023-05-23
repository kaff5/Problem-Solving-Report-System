using ProblemSolvingReportSystem.Models.TopicDir;

namespace ProblemSolvingReportSystem.Models.TopicDir
{
    public class TopicsDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public int? parentId { get; set; }


        public TopicsDto()
        {

        }
    }
    
}
