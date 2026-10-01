namespace Users.API.Endpoints;

public static class Users
{

    public static void MapUsersEndpoints(this WebApplication app)
    {
        var userGroup = app.MapGroup("/api/users");


        userGroup.MapGet("/{id}", (int id,  HttpRequest request) =>
        {
            if (id <= 0)
                return Results.NotFound($"User {id} Not found");
            else
            {
                var clientName = request.Headers["Client-Name"].ToString();
                return Results.Ok(new
                {
                    Id = id,
                    Name = $"User {id}",
                    ClientName = clientName
                    
                });

            }


        });
    }
}
