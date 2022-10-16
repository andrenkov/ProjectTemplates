namespace WebCactusAPI.Models
{
    public interface IUserRepository
    {
        public UserDTO GetUser(UserModel userModel);
    }
}
