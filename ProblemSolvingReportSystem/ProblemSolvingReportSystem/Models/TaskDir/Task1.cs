using ProblemSolvingReportSystem.Models.TopicDir;

namespace ProblemSolvingReportSystem.Models.TaskDir
{
    public class Task1
    {
        public int id { get; set; }
        public string name { get; set; }
        public int topicId { get; set; }
        public string description { get; set; }
        public int price { get; set; }
        public bool isDraft { get; set; }

        public string? input { get; set; }
        public string? output { get; set; }

    }
}
