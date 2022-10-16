using WebCactusAPI.Models;

namespace WebCactusAPI.Models
{
    public class UserRepository : IUserRepository
    {
        private readonly List<UserDTO> Users = new List<UserDTO>();

        public UserRepository()
        {
            Users.Add(new UserDTO{ UserName = "Vlad", Password = "pass123", Role = "admin"});
            Users.Add(new UserDTO { UserName = "user", Password = "pass123", Role = "user" });
        }

        public UserDTO GetUser(UserModel userModel)
        {
            return Users.Where(x => x.UserName.ToLower() == userModel.UserName.ToLower()
                        && x.Password == userModel.Password).FirstOrDefault();
        }
    }
}
