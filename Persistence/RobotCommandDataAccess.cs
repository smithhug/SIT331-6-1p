using Npgsql; 
using robot_controller_api.Models;
 
namespace robot_controller_api.Persistence;

public class RobotCommandADO : IRobotCommandDataAccess
{
    // Connection string is usually set in a config file for the ease of change. 
    private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=password;Database=postgres";

    public List<RobotCommand> GetRobotCommands()
    {
        var robotCommands = new List<RobotCommand>();
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        using var cmd = new NpgsqlCommand("SELECT * FROM robotcommand", conn);
        using var dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            var Id = dr.GetInt32(0);
            var Name = dr.GetString(1);
            var Description = dr.IsDBNull(2) ? null : dr.GetString(2);
            var IsMoveCommand = dr.GetBoolean(3);
            var CreatedDate = dr.GetDateTime(4);
            var ModifiedDate = dr.GetDateTime(5);

            var robotCommand = new RobotCommand(Id, Name, IsMoveCommand, CreatedDate, ModifiedDate, Description);

            //read values off the data reader and create a new robotCommand here and then add it to the result list. 
            robotCommands.Add(robotCommand);
        }
        return robotCommands;
    }
    //UPDATE
    public void UpdateRobotCommand(RobotCommand robotCommand)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        using var cmd = new NpgsqlCommand("UPDATE robotcommand SET \"Name\" = @Name, Description = @Description, IsMoveCommand = @IsMoveCommand, CreatedDate = @CreatedDate, ModifiedDate = @ModifiedDate WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", robotCommand.Id);
        cmd.Parameters.AddWithValue("@Name", robotCommand.Name);
        cmd.Parameters.AddWithValue("@Description", robotCommand.Description ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@IsMoveCommand", robotCommand.IsMoveCommand);
        cmd.Parameters.AddWithValue("@CreatedDate", robotCommand.CreatedDate);
        cmd.Parameters.AddWithValue("@ModifiedDate", robotCommand.ModifiedDate);

        cmd.ExecuteNonQuery();
    }
    //INSERT
    public void InsertRobotCommand(RobotCommand robotCommand)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        using var cmd = new NpgsqlCommand("INSERT INTO robotcommand (\"Name\", description, ismovecommand, createddate, modifieddate) VALUES (@Name, @Description, @IsMoveCommand, @CreatedDate, @ModifiedDate)", conn);
        cmd.Parameters.AddWithValue("@Name", robotCommand.Name);
        cmd.Parameters.AddWithValue("@Description", robotCommand.Description ?? (object)DBNull.Value); // Handle null values
        cmd.Parameters.AddWithValue("@IsMoveCommand", robotCommand.IsMoveCommand);
        cmd.Parameters.AddWithValue("@CreatedDate", robotCommand.CreatedDate);
        cmd.Parameters.AddWithValue("@ModifiedDate", robotCommand.ModifiedDate);

        cmd.ExecuteNonQuery();
    }
    //DELETE
    public void DeleteRobotCommand(int id)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        using var cmd = new NpgsqlCommand("DELETE FROM robotcommand WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);

        cmd.ExecuteNonQuery();
    }
} 