namespace ProblemSolvingReportSystem.Models
{
    public class SolutionDto
    {
        public int id { get; set; }

        public string sourceCode { get; set; }
        public string programmingLanguage { get; set; }

        public string verdict { get; set; }

        public int authorId { get; set; }

        public int taskId { get; set; }
    }
}
