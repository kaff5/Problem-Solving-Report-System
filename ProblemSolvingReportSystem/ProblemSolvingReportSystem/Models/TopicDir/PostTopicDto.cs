namespace ProblemSolvingReportSystem.Models.TopicDir
{
    public class TopicPostModelDto
    {
        public string name { get; set; }
        public int? parentId { get; set; }

        public TopicPostModelDto(string name, int? parentId)
        {
            this.name = name;
            this.parentId = parentId;
        }
    }
}
