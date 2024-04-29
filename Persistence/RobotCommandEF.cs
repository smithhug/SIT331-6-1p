using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public class RobotCommandEF : IRobotCommandDataAccess
    {
        private readonly RobotContext _context;

        public RobotCommandEF(RobotContext context)
        {
            _context = context;
        }

        public List<RobotCommand> GetRobotCommands()
        {
            return _context.RobotCommands.ToList();
        }

        public void InsertRobotCommand(RobotCommand newCommand)
        {
            newCommand.Id = default;
            newCommand.CreatedDate = DateTime.Now;
            newCommand.ModifiedDate = DateTime.Now;
            _context.RobotCommands.Add(newCommand);
            _context.SaveChanges();
        }

        public void UpdateRobotCommand(RobotCommand updatedCommand)
        {
            var existingCommand = _context.RobotCommands.Find(updatedCommand.Id);
            if (existingCommand == null)
            {
                throw new ArgumentException("Robot command not found");
            }

            updatedCommand.ModifiedDate = DateTime.Now;
            _context.Update(updatedCommand);
            _context.SaveChanges();
        }

        public void DeleteRobotCommand(int id)
        {
            var existingCommand = _context.RobotCommands.Find(id);
            if (existingCommand == null)
            {
                throw new ArgumentException("Robot command not found");
            }

            _context.RobotCommands.Remove(existingCommand);
            _context.SaveChanges();
        }
    }
}
