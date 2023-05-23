namespace ProblemSolvingReportSystem.Models.TopicDir
{
    public class Topic
    {
        public int id { get; set; }
        public string name { get; set; }
        public int? parentId { get; set; }

        public Topic()
        {

        }

    }
}
