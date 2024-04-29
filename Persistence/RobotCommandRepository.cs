// using Npgsql;
// using robot_controller_api.Models;

// namespace robot_controller_api.Persistence;
// public class RobotCommandRepository : IRobotCommandDataAccess, IRepository
// {
//     private IRepository _repo => this;
//     public List<RobotCommand> GetRobotCommands()
//     {
//         var commands = _repo.ExecuteReader<RobotCommand>("SELECT * FROM public.robotcommand"); 
//         return commands; 
//     }
//     public void UpdateRobotCommand(RobotCommand updatedCommand)
//     {
//         var sqlParams = new NpgsqlParameter[]{
//             new("id", updatedCommand.Id),
//             new("name", updatedCommand.Name),
//             new("description", updatedCommand.Description ?? (object)DBNull.Value),
//             new("ismovecommand", updatedCommand.IsMoveCommand)
//         };

//         var result = _repo.ExecuteReader<RobotCommand>(
//             "UPDATE robotcommand SET \"Name\"=@name, description=@description, ismovecommand = @ismovecommand, modifieddate = current_timestamp WHERE id = @id RETURNING *; ", 
//             sqlParams);
//         //return result;
//     } 
//     public void DeleteRobotCommand(int id)
//     {
//         var sql = "DELETE FROM robotcommand WHERE Id = @Id";
//         var sqlParams = new NpgsqlParameter[]
//         {
//             new NpgsqlParameter("@Id", id)
//         };
//         var result = _repo.ExecuteReader<RobotCommand>(sql, sqlParams);
//     }
//     public void InsertRobotCommand(RobotCommand newCommand)
//     {
//         var sql = "INSERT INTO robotcommand (\"Name\", Description, IsMoveCommand, CreatedDate, ModifiedDate) " +
//                     "VALUES (@Name, @Description, @IsMoveCommand, @CreatedDate, @ModifiedDate)";
//         var sqlParams = new NpgsqlParameter[]
//         {
//             new NpgsqlParameter("@Name", newCommand.Name),
//             new NpgsqlParameter("@Description", newCommand.Description ?? (object)DBNull.Value),
//             new NpgsqlParameter("@IsMoveCommand", newCommand.IsMoveCommand),
//             new NpgsqlParameter("@CreatedDate", newCommand.CreatedDate),
//             new NpgsqlParameter("@ModifiedDate", newCommand.ModifiedDate)
//         };
//         var result = _repo.ExecuteReader<RobotCommand>(sql, sqlParams);
//     }
// } 