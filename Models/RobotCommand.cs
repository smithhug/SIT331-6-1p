using System;
using System.Collections.Generic;

namespace robot_controller_api.Models;

public partial class RobotCommand
{
    public RobotCommand()
    {

    }
    public RobotCommand(int id, string name, bool isMoveCommand, DateTime createdDate, DateTime modifiedDate, string? description)
    {
        Id = id;
        Name = name;
        IsMoveCommand = isMoveCommand;
        CreatedDate = createdDate;
        ModifiedDate = modifiedDate;
        Description = description;
    }

    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsMoveCommand { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime ModifiedDate { get; set; }
}
