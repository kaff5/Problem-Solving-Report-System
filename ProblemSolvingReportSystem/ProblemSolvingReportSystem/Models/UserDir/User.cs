namespace ProblemSolvingReportSystem.Models.UserDir
{
    public class User
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public int roleId { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string password { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public User()
        {

        }

    }
}
