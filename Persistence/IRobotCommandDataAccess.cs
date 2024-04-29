using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public interface IRobotCommandDataAccess
    {
        void DeleteRobotCommand(int id);
        List<RobotCommand> GetRobotCommands();
        void InsertRobotCommand(RobotCommand robotCommand);
        void UpdateRobotCommand(RobotCommand robotCommand);
    }
}