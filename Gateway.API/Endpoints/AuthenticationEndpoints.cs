using Gateway.API.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.API.Auth;

public static class AuthenticationEndpoints
{

    public static void MapAuthenticationEndpoints(this WebApplication app)
    {
        var authGroup = app.MapGroup("/api/auth");

        authGroup.MapPost("/login", (
        LoginRequest request,
        JwtTokenService tokenService) =>
        {
            if (request.Username!="admin"|| request.Role!= "admin")
                return ApiResponse.Fail("Unauthorized", 400);
            else
            {
                var token = tokenService.GenerateToken(request.Username,request.Role );
                var accsesTokken = new {token};
                return ApiResponse.Success($"{accsesTokken}"); 
            }
        });
    }


    public record LoginRequest(string Username ,string Role);
}
