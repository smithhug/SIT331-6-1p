using robot_controller_api.Models;
using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Persistence;
using Microsoft.AspNetCore.Authorization;

namespace robot_controller_api.Controllers;

[ApiController]
[Route("api/robot-commands")]
public class RobotCommandsController : ControllerBase
{
    private readonly IRobotCommandDataAccess _robotCommandsRepo;
    public RobotCommandsController(IRobotCommandDataAccess robotCommandsRepo)
    {
        _robotCommandsRepo = robotCommandsRepo;
    }
    /// <summary>
    /// Retrieves all robot commands.
    /// </summary>
    /// <returns>A collection of all robot commands.</returns>
    [Authorize(Policy = "UserOnly")]
    [HttpGet]
    public IEnumerable<RobotCommand> GetAllRobotCommands()
    {
        // return _commands;
        return _robotCommandsRepo.GetRobotCommands();
    }
    /// <summary>
    /// Retrieves all move commands.
    /// </summary>
    /// <returns>A collection of all move commands.</returns>
    [Authorize(Policy = "UserOnly")]
    [HttpGet("move")]
    public IEnumerable<RobotCommand> GetMoveCommandsOnly()
    {
        // return _commands.Where(command => command.IsMoveCommand);
        return _robotCommandsRepo.GetRobotCommands().Where(command => command.IsMoveCommand);
    }
    /// <summary>
    /// Retrieves a specific robot command by its ID.
    /// </summary>
    /// <param name="id">The ID of the robot command to retrieve.</param>
    /// <returns>The robot command with the specified ID.</returns>
    /// <response code="200">Returns the requested robot command.</response>
    /// <response code="404">If the robot command with the specified ID is not found.</response>
    [Authorize(Policy = "UserOnly")]
    [HttpGet("{id}")]
    public IActionResult GetRobotCommandById(int id)
    {
        // RobotCommand command = _commands.FirstOrDefault(c => c.Id == id);
        RobotCommand command = _robotCommandsRepo.GetRobotCommands().FirstOrDefault(c => c.Id == id);

        if (command == null)
        {
            return NotFound();
        }

        return Ok(command);
    }

    /// <summary> 
    /// Creates a robot command. 
    /// </summary> 
    /// <param name="newCommand">A new robot command from the HTTP request.</param> 
    /// <returns>A newly created robot command</returns> 
    /// <remarks> 
    /// Sample request: 
    /// 
    /// POST /api/robot-commands 
    /// { 
    ///     "name": "DANCE", 
    ///     "isMoveCommand": true, 
    ///     "description": "Salsa on the Moon" 
    /// }
    /// 
    /// </remarks> 
    /// <response code="201">Returns the newly created robot command</response> 
    /// <response code="400">If the robot command is null</response> 
    /// <response code="409">If a robot command with the same name already exists.</response> 
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Authorize(Policy = "AdminOnly")]
    [HttpPost()]
    public IActionResult AddRobotCommand(RobotCommand newCommand)
    {
        if (newCommand == null)
        {
            return BadRequest();
        }

        if (_robotCommandsRepo.GetRobotCommands().Any(c => c.Name == newCommand.Name))
        {
            return Conflict();
        }

        int newId = _robotCommandsRepo.GetRobotCommands().Count + 1;
        newCommand.CreatedDate = DateTime.Now;
        newCommand.ModifiedDate = DateTime.Now;
        newCommand.Id = newId;

        _robotCommandsRepo.InsertRobotCommand(newCommand);

        return CreatedAtRoute(new { id = newCommand.Id }, newCommand);
    }
    /// <summary>
    /// Updates an existing robot command.
    /// </summary>
    /// <param name="id">The ID of the robot command to update.</param>
    /// <param name="updatedCommand">The updated information for the robot command.</param>
    /// <returns>No content if the update is successful.</returns>
    /// <response code="204">If the update is successful.</response>
    /// <response code="400">If the updated command is null.</response>
    /// <response code="404">If the robot command with the specified ID is not found.</response>
    /// <response code="409">If a robot command with the same name already exists.</response>
    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id}")]
    public IActionResult UpdateRobotCommand(int id, RobotCommand updatedCommand)
    {
        var existingCommand = _robotCommandsRepo.GetRobotCommands().FirstOrDefault(c => c.Id == id);
        if (existingCommand == null)
        {
            return NotFound();
        }

        if (updatedCommand == null)
        {
            return BadRequest();
        }

        if (_robotCommandsRepo.GetRobotCommands().Any(c => c.Name == updatedCommand.Name && c.Id != id))
        {
            return Conflict();
        }

        existingCommand.Name = updatedCommand.Name;
        existingCommand.Description = updatedCommand.Description;
        existingCommand.IsMoveCommand = updatedCommand.IsMoveCommand;
        existingCommand.ModifiedDate = DateTime.Now;

        _robotCommandsRepo.UpdateRobotCommand(existingCommand);

        return NoContent();
    }
    /// <summary>
    /// Deletes a robot command.
    /// </summary>
    /// <param name="id">The ID of the robot command to delete.</param>
    /// <returns>No content if the deletion is successful.</returns>
    /// <response code="204">If the deletion is successful.</response>
    /// <response code="404">If the robot command with the specified ID is not found.</response>
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id}")]
    public IActionResult DeleteRobotCommand(int id)
    {
        var existingCommand = _robotCommandsRepo.GetRobotCommands().FirstOrDefault(c => c.Id == id);
        if (existingCommand == null)
        {
            return NotFound();
        }

        _robotCommandsRepo.DeleteRobotCommand(id);

        return NoContent();
    }

}