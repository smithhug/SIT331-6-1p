using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using robot_controller_api.Controllers;
using robot_controller_api.Models;
using robot_controller_api.Persistence;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    
    private readonly UserDataAccess _userDataAccess = new UserDataAccess();
    public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        base.Response.Headers.Add("WWW-Authenticate", @"Basic realm=""Access to the robot controller.""");
        var authHeader = base.Request.Headers["Authorization"].ToString();
        // Authentication logic will be here. 
        var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();

        var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));

        var email = credentials.Split(':', 2)[0];
        var password = credentials.Split(':', 2)[1];
        var userlist = _userDataAccess.GetUsers();
        var existingUser = _userDataAccess.GetUsers().FirstOrDefault(m => m.Email == email);
        if (existingUser == null)
        {
            Response.StatusCode = 401;
            return Task.FromResult(AuthenticateResult.Fail($"Authentication Failed."));
        } 
        var hasher = new PasswordHasher<UserModel>();
        var pwVerificationResult = hasher.VerifyHashedPassword(existingUser, existingUser.PasswordHash, password);
        if (pwVerificationResult == PasswordVerificationResult.Success)
        {
            var claims = new[]
            {
                new Claim("name", $@"{existingUser.FirstName} {existingUser.LastName}"),
                new Claim(ClaimTypes.Role, existingUser.Role)
            };

            var identity = new ClaimsIdentity(claims, "Basic"); 
            var claimsPrincipal = new ClaimsPrincipal(identity); 
            var authTicket = new AuthenticationTicket(claimsPrincipal, Scheme.Name); 
            return Task.FromResult(AuthenticateResult.Success(authTicket)); 
        }
        Response.StatusCode = 401;
        return Task.FromResult(AuthenticateResult.Fail($"Authentication Failed."));



    }
}