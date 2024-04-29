using robot_controller_api.Persistence;
using robot_controller_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<RobotContext>(options =>
    options.UseNpgsql("Host=localhost;Username=postgres;Password=password;Database=postgres"));
// 4.1P:
// builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandADO>();
// builder.Services.AddScoped<IMapDataAccess, MapADO>(); 
// 4.2C:
// builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandRepository>();
// builder.Services.AddScoped<IMapDataAccess, MapRepository>(); 
// 4.3D: 
builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandEF>();
builder.Services.AddScoped<IMapDataAccess, MapEF>();
// 5-1P:
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename)); 
    
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Robot Controller API",
        Description = "New backend service that provides resources for the Moon robot simulator.", 
        Contact = new OpenApiContact
        {
            Name = "Hugo Smith",
            Email = "smithhug@deakin.edu.au"
        },
    });
});
// 6-1P
builder.Services.AddScoped<IUserDataAccess, UserDataAccess>();
builder.Services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", default);
builder.Services.AddAuthorization(options => 
{ 
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Admin")); 
    options.AddPolicy("UserOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Admin", "User")); 
}); 

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(setup => setup.InjectStylesheet("/styles/theme-flattop.css"));
app.UseStaticFiles();

app.Run();
