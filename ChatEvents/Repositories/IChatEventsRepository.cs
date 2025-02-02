using ChatEvents.Models;
using ChatEvents.Models.Reports;

namespace ChatEvents.Repositories;

public interface IChatEventsRepository
{
    Task<IEnumerable<ChatEventDto>> GetAllEventsAsync(long chatRoomId, CancellationToken cancellationToken);
    Task<IEnumerable<GroupedChatEvents>> GetHourlyAggregatedEventsAsync(long chatRoomId, CancellationToken cancellationToken);
    Task<IEnumerable<GroupedChatEvents>> GetDailyAggregatedEventsAsync(long chatRoomId, CancellationToken cancellationToken);
}

