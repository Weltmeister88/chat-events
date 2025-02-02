using ChatEvents.API.Endpoints.AggregatedEvents.DTOs;
using ChatEvents.Models;
using ChatEvents.Models.Reports;
using ChatEvents.Services;

namespace ChatEvents.API.Endpoints.AggregatedEvents;

internal static class AggregatedEventsEndpoint
{
    internal static async Task<IResult> Get(long chatRoomId, Granularity granularity,
        IChatRoomsService chatRoomsService, IChatEventsService chatEventsService,
        CancellationToken cancellationToken)
    {
        bool chatRoomExists = await chatRoomsService.RoomExistsAsync(chatRoomId, cancellationToken);
        if (!chatRoomExists) return Results.NotFound();

        IEnumerable<IChatEventReport> chatEvents =
            await chatEventsService.GetChatEventsAsync(chatRoomId, granularity, cancellationToken);

        return Results.Ok(chatEvents
            .Select(ce => new AggregationOccurenceResponse(ce))
            .ToList());
    }
}