using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public interface IUserDataAccess
    {
        List<UserModel> GetUsers();
        void AddUser(UserModel userModel);
        void UpdateUser(UserModel userModel);
        void DeleteUser(int id);
        void PatchUser(UserModel userModel);
    }
}
