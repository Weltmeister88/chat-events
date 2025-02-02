using ChatEvents.Models;
using ChatEvents.Models.Reports;

namespace ChatEvents.Services;

public interface IChatEventsService
{
    Task<IEnumerable<IChatEventReport>> GetChatEventsAsync(long chatRoomId, Granularity granularity, CancellationToken cancellationToken);
}