using NoteMiniApi.Dtos;
using NoteMiniApi.Services;

namespace NoteMiniApi.Requests
{
    public static class UserRequests
    {
         public static WebApplication RegisterEndpoints(this WebApplication app)
         {
             app.MapPost("/users/register", UserRequests.Register);
             app.MapPost("/users", UserRequests.Login);
             return app;
         }

         public static IResult Register(IUserService service, RegisterUserDto registerUserDto)
         {
             service.Register(registerUserDto);
             return Results.Ok();
         }

         public static IResult Login(IUserService service, LoginDto loginDto)
         {
            var token = service.GenerateJwt(loginDto);
            return Results.Ok(token);
         }
    }
}