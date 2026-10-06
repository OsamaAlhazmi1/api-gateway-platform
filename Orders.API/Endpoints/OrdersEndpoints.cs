namespace Orders.API.Endpoints;

public static class OrdersrdersEndpoints
{
    public static void MapOrdersEndpoints(this WebApplication app)
    {
        var ordersGroup = app.MapGroup("/api/store");



        ordersGroup.MapGet("/{id}", (int id) =>
        {
            if (id <= 0)
                return Results.NotFound($"Order {id} Not Found ");

            else
            {
                return Results.Ok (new
                {
                    Id = id , 
                    name = $"order{id}"
                });
                
            }
            
        });
    }
}
