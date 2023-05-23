namespace ProblemSolvingReportSystem.Models.UserDir
{
    public class UserMyselfDto
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public int roleId { get; set; }
        public string name { get; set; }
        public string surname { get; set; }

        public UserMyselfDto(User user)
        {
            userId = user.userId;
            userName = user.userName;
            roleId = user.roleId;
            name = user.name;
            surname = user.surname;
        }
    }
}
