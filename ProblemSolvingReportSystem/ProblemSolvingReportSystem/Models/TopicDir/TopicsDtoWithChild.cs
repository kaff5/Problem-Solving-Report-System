namespace ProblemSolvingReportSystem.Models.TopicDir
{
    public class TopicsDtoWithChild
    {
        public int id { get; set; }
        public string name { get; set; }
        public int? parentId { get; set; }

        public List<ChildsTopicDto> childs { get; set; }

        public TopicsDtoWithChild()
        {

        }
    }
}
