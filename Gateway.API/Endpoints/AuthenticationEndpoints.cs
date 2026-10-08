using Gateway.API.DTOs.AuthDTOs;
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
        LoginDTO dto,
        JwtTokenService tokenService) =>
        {
            if (dto.Username!="admin"|| dto.Role!= "admin")
                return ApiResponse.Fail("Unauthorized", 400);
            else
            {
                var token = tokenService.GenerateToken(dto.Username,dto.Role );
                var accsesTokken = new {token};
                return ApiResponse.Success($"{accsesTokken}"); 
            }
        });
    }


}
