using ChatEvents.API.Endpoints.AggregatedEvents;
using ChatEvents.API.Endpoints.AggregatedEvents.DTOs;

namespace ChatEvents.API;

public static class Router
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGroup("api").AllowAnonymous()
            .MapAggregatedEventsEndpoints();
    }

    private static IEndpointRouteBuilder MapAggregatedEventsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("v1/chat-rooms/{chatRoomId:long}/aggregated-events/{granularity}", AggregatedEventsEndpoint.Get)
            .Produces<List<AggregationOccurenceResponse>>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithName("GetAggregatedEvents")
            .WithOpenApi();

        return app;
    }
}