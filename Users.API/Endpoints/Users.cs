namespace Users.API.Endpoints;

public static class Users
{
    
    public static void MapUsersEndpoints (this WebApplication app)
    {
       var userGroup =  app.MapGroup("/api/users");


       userGroup.MapGet("/{id}", (int id) =>
       {
           return Results.Ok(new
           {
               Id = id , 
               name = $"User {id}"
           });

       });
    }
}
