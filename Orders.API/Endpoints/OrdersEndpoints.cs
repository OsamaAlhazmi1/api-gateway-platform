namespace Orders.API.Endpoints;

public static class OrdersrdersEndpoints
{
    public static void MapOrdersEndpoints(this WebApplication app)
    {
        var ordersGroup = app.MapGroup("/api/orders");



        ordersGroup.MapGet("/{id}", (int id) =>
        {
            
            return Results.Ok (new
            {
                Id = id , 
                name = $"order{id}"
            });
        });
    }
}
