using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Persistence;
using robot_controller_api.Models;
using Microsoft.AspNetCore.Authorization;

namespace robot_controller_api.Controllers;

[ApiController]
[Route("api/maps")]
public class MapsController : ControllerBase
{
    private readonly IMapDataAccess _mapRepo;
    public MapsController(IMapDataAccess mapRepo)
    {
        _mapRepo = mapRepo;
    }
    /// <summary>
    /// Retrieves all maps.
    /// </summary>
    /// <returns>A collection of all maps.</returns>
    [Authorize(Policy = "UserOnly")]
    [HttpGet]
    public IEnumerable<Map> GetAllMaps()
    {
        return _mapRepo.GetMaps();
    }
    /// <summary>
    /// Retrieves all square maps
    /// </summary>
    /// <returns>A collection of all square maps.</returns>
    [Authorize(Policy = "UserOnly")]
    [HttpGet("square")]
    public IEnumerable<Map> GetSquareMaps()
    {
        return _mapRepo.GetMaps().Where(map => map.Columns == map.Rows);
    }
    /// <summary>
    /// Retrieves a specific map by its ID.
    /// </summary>
    /// <param name="id">The ID of the map to retrieve.</param>
    /// <returns>The map with the specified ID.</returns>
    /// <response code="200">Returns the requested map.</response>
    /// <response code="404">If the map with the specified ID is not found.</response>
    [Authorize(Policy = "UserOnly")]
    [HttpGet("{id}")]
    public IActionResult GetMapById(int id)
    {
        var map = _mapRepo.GetMaps().FirstOrDefault(m => m.Id == id);

        if (map == null)
        {
            return NotFound();
        }

        return Ok(map);
    }
    /// <summary>
    /// Creates a new map.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    /// POST /api/maps
    /// {
    ///     "name": "Map Name",
    ///     "rows": 10,
    ///     "columns": 10,
    ///     "description": "Description of the map"
    /// }
    /// </remarks>
    /// <param name="newMap">The new map to be created.</param>
    /// <returns>The newly created map.</returns>
    /// <response code="201">Returns the newly created map.</response>
    /// <response code="400">If the map is null.</response>
    /// <response code="409">If a map with the same name already exists.</response>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public IActionResult AddMap(Map newMap)
    {
        if (newMap == null)
        {
            return BadRequest();
        }

        if (_mapRepo.GetMaps().Any(m => m.Name == newMap.Name))
        {
            return Conflict();
        }

        int newId = _mapRepo.GetMaps().Count + 1;
        newMap.CreatedDate = DateTime.Now;
        newMap.ModifiedDate = DateTime.Now;
        newMap.Id = newId;

        _mapRepo.InsertMap(newMap);
        // _maps.Add(newMap);

        return CreatedAtRoute(new { id = newMap.Id }, newMap);
    }
    /// <summary>
    /// Updates an existing map.
    /// </summary>
    /// <param name="id">The ID of the map to update.</param>
    /// <param name="updatedMap">The updated information for the map.</param>
    /// <returns>No content if the update is successful.</returns>
    /// <response code="204">If the update is successful.</response>
    /// <response code="400">If the updated map is null.</response>
    /// <response code="404">If the map with the specified ID is not found.</response>
    /// <response code="409">If a map with the same name already exists.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id}")]
    public IActionResult UpdateMap(int id, Map updatedMap)
    {
        var existingMap = _mapRepo.GetMaps().FirstOrDefault(m => m.Id == id);

        if (existingMap == null)
        {
            return NotFound();
        }

        existingMap.Name = updatedMap.Name;
        existingMap.Columns = updatedMap.Columns;
        existingMap.Rows = updatedMap.Rows;
        existingMap.Description = updatedMap.Description;
        existingMap.ModifiedDate = DateTime.Now;

        _mapRepo.UpdateMap(existingMap);

        return NoContent();
    }
    /// <summary>
    /// Deletes a map.
    /// </summary>
    /// <param name="id">The ID of the map to delete.</param>
    /// <returns>No content if the deletion is successful.</returns>
    /// <response code="204">If the deletion is successful.</response>
    /// <response code="404">If the map with the specified ID is not found.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id}")]
    public IActionResult DeleteMap(int id)
    {
        var existingMap = _mapRepo.GetMaps().FirstOrDefault(m => m.Id == id);

        if (existingMap == null)
        {
            return NotFound();
        }

        _mapRepo.DeleteMap(id);
        //_maps.Remove(existingMap);

        return NoContent();
    }

    /// <summary>
    /// Retrieves the data at the specified coordinates (x, y) on the map with the given ID.
    /// </summary>
    /// <param name="id">The ID of the map.</param>
    /// <param name="x">The x-coordinate.</param>
    /// <param name="y">The y-coordinate.</param>
    /// <returns>The data at the specified coordinates.</returns>
    /// <response code="200">Returns the data at the specified coordinates.</response>
    /// <response code="400">If the provided coordinates are out of bounds.</response>
    /// <response code="404">If the map with the specified ID is not found.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "UserOnly")]
    [HttpGet("{id}/{x}-{y}")]
    public IActionResult CheckCoordinate(int id, int x, int y)
    {
        // return BadRequest() if coordinate provided is in the wrong format, e.g. negative values
        if (x < 0 || y < 0)
            return BadRequest();

        Map result = _mapRepo.GetMaps().FirstOrDefault(m => m.Id == id);

        // return NotFound() if the map does not exist 
        if (result == null)
            return NotFound();

        bool isOnMap = false;

        // logic to check whether the coordinate is on the map here 
        if (x <= result.Columns && y <= result.Rows)
            isOnMap = true;

        return Ok(isOnMap);
    }
}
