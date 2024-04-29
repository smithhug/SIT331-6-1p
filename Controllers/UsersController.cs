using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Models;
using robot_controller_api.Persistence;

namespace robot_controller_api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserDataAccess _userDataAccess;

        public UsersController(IUserDataAccess userDataAccess)
        {
            _userDataAccess = userDataAccess;
        }
        [HttpGet(), Authorize(Policy = "UserOnly")]
        public IEnumerable<UserModel> GetUsers()
        {
            return _userDataAccess.GetUsers();
        }

        [HttpGet("admin"), Authorize(Policy = "AdminOnly")]
        public IEnumerable<UserModel> GetAdminUsers()
        {
            return _userDataAccess.GetUsers().Where(user => user.Role == "Admin");
        }

        [HttpGet("{id}"), Authorize(Policy = "AdminOnly")]
        public IActionResult GetUserById(int id)
        {
            var user = _userDataAccess.GetUsers().FirstOrDefault(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost, Authorize(Policy = "AdminOnly")]
        public IActionResult AddUser(UserModel newUser)
        {
            if (newUser == null)
            {
                return BadRequest();
            }
            
            var password = newUser.PasswordHash.ToString();
            var hasher = new PasswordHasher<UserModel>();
            var pwHash = hasher.HashPassword(newUser, password);
            var pwVerificationResult = hasher.VerifyHashedPassword(newUser, pwHash, password);
            if (pwVerificationResult == PasswordVerificationResult.Success)
            {
                newUser.PasswordHash = pwHash;
                newUser.CreatedDate = DateTime.Now;
                newUser.ModifiedDate = DateTime.Now;
                int newId = _userDataAccess.GetUsers().Count + 1;
                newUser.Id = newId;
                
                _userDataAccess.AddUser(newUser);

                return CreatedAtRoute(new { id = newUser.Id }, newUser);
            } else
                return BadRequest();
        }

        [HttpPut("{id}"), Authorize(Policy = "AdminOnly")]
        public IActionResult UpdateUser(int id, UserModel updatedUser)
        {
            var existingUser = _userDataAccess.GetUsers().FirstOrDefault(m => m.Id == id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.Description = updatedUser.Description;
            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;
            existingUser.Role = updatedUser.Role;
            existingUser.ModifiedDate = DateTime.Now;

            _userDataAccess.UpdateUser(existingUser);

            return NoContent();
        }

        [HttpDelete("{id}"), Authorize(Policy = "AdminOnly")]
        public IActionResult DeleteUser(int id)
        {
            var existingUser = _userDataAccess.GetUsers().FirstOrDefault(m => m.Id == id);
            if (existingUser == null)
            {
                return NotFound();
            }

            _userDataAccess.DeleteUser(id);

            return NoContent();
        }

        [HttpPatch("{id}"), Authorize(Policy = "AdminOnly")]
        public IActionResult UpdateUserEmailAndPassword(int id, LoginModel loginModel)
        {
            var existingUser = _userDataAccess.GetUsers().FirstOrDefault(m => m.Id == id);
            if (existingUser == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrWhiteSpace(loginModel.Email))
            {
                existingUser.Email = loginModel.Email;
            }

            if (!string.IsNullOrWhiteSpace(loginModel.Password))
            {
                var password = loginModel.Password;
                var hasher = new PasswordHasher<UserModel>();
                var pwHash = hasher.HashPassword(existingUser, password);
                var pwVerificationResult = hasher.VerifyHashedPassword(existingUser, pwHash, password);
                if (pwVerificationResult != PasswordVerificationResult.Success)
                    return BadRequest();  

                existingUser.PasswordHash = pwHash;
            }

            existingUser.ModifiedDate = DateTime.Now;

            _userDataAccess.PatchUser(existingUser);

            return NoContent();
        }
    }
}
