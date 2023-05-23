namespace ProblemSolvingReportSystem.Models
{
    public class Solution
    {
        public int id { get; set; }

        public string sourceCode { get; set; }
        public int programmingLanguage { get; set; }

        public int verdict { get; set; }

        public int authorId { get; set; }

        public int taskId { get; set; }


        public static int GetVerdict(string verdict)
        {
            switch (verdict)
            {
                case "Pending":
                    return 1;
                case "Ok":
                    return 2;
                case "Rejected":
                    return 3;
                default:
                    throw new Exception();
            }
        }

        public static int GetLang(string verdict)
        {
            switch (verdict)
            {
                case "Python":
                    return 1;
                case "C++":
                    return 2;
                case "C#":
                    return 3;
                case "Java":
                    return 4;
                default:
                    throw new Exception();
            }
        }

        public static string PostLangInString(int verdict)
        {
            switch (verdict)
            {
                case 1:
                    return "Python";
                case 2:
                    return "C++";
                case 3:
                    return "C#";
                case 4:
                    return "Java";
                default:
                    throw new Exception();
            }
        }

        public static string PostVerdictInString(int verdict)
        {
            switch (verdict)
            {
                case 1:
                    return "Pending";
                case 2:
                    return "Ok";
                case 3:
                    return "Rejected";
                default:
                    throw new Exception();
            }
        }
    }
}