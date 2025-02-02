using ChatEvents.Infrastructure.Database;
using ChatEvents.Models;
using ChatEvents.Models.DbEntities;
using ChatEvents.Models.Reports;
using ChatEvents.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChatEvents.Infrastructure.Repositories;

public class ChatEventsRepository(ApplicationDbContext context)
    : GenericEntityFrameworkRepository<ChatEventDbEntity>(context), IChatEventsRepository
{
    public async Task<IEnumerable<ChatEventDto>> GetAllEventsAsync(long chatRoomId,
        CancellationToken cancellationToken)
    {
        var result = await GetAll().AsNoTracking()
            .Where(ce => ce.ChatRoomId == chatRoomId)
            .OrderBy(ce => ce.CreatedUtc)
            .Select(e => new
            {
                e.CreatedUtc,
                e.UserId,
                UserName = e.User.Name,
                e.Comment,
                e.EventType,
                e.HighFivedUserId,
                HighFivedUserName = e.HighFivedUser != null ? e.HighFivedUser.Name : null
            })
            .ToListAsync(cancellationToken);

        return result.Select(e =>
            new ChatEventDto(e.EventType, e.UserId, e.UserName, e.Comment, e.HighFivedUserId,
                e.HighFivedUserName, e.CreatedUtc));
    }

    public async Task<IEnumerable<GroupedChatEvents>> GetHourlyAggregatedEventsAsync(long chatRoomId,
        CancellationToken cancellationToken)
    {
        var result = await GetAll().AsNoTracking()
            .Where(ce => ce.ChatRoomId == chatRoomId)
            .OrderBy(ce => ce.CreatedUtc)
            .GroupBy(e => new { e.CreatedUtc.Date, e.CreatedUtc.Hour })
            .Select(group => new
            {
                Timestamp = group.First().CreatedUtc,
                Events = group.Select(e => new
                {
                    e.CreatedUtc,
                    e.UserId,
                    UserName = e.User.Name,
                    e.Comment,
                    e.EventType,
                    e.HighFivedUserId,
                    HighFivedUserName = e.HighFivedUser != null ? e.HighFivedUser.Name : null
                })
            })
            .ToListAsync(cancellationToken);

        return result.Select(r => new GroupedChatEvents(r.Timestamp,
            r.Events.Select(e =>
                new ChatEventDto(e.EventType, e.UserId, e.UserName, e.Comment, e.HighFivedUserId,
                    e.HighFivedUserName, e.CreatedUtc))));
    }

    public async Task<IEnumerable<GroupedChatEvents>> GetDailyAggregatedEventsAsync(long chatRoomId,
        CancellationToken cancellationToken)
    {
        var result = await GetAll().AsNoTracking()
            .Where(ce => ce.ChatRoomId == chatRoomId)
            .OrderBy(ce => ce.CreatedUtc)
            .GroupBy(e => new { e.CreatedUtc.Date })
            .Select(group => new
            {
                Timestamp = group.First().CreatedUtc,
                Events = group.Select(e => new
                {
                    e.CreatedUtc,
                    e.UserId,
                    UserName = e.User.Name,
                    e.Comment,
                    e.EventType,
                    e.HighFivedUserId,
                    HighFivedUserName = e.HighFivedUser != null ? e.HighFivedUser.Name : null
                })
            })
            .ToListAsync(cancellationToken);

        return result.Select(r => new GroupedChatEvents(r.Timestamp,
            r.Events.Select(e =>
                new ChatEventDto(e.EventType, e.UserId, e.UserName, e.Comment, e.HighFivedUserId,
                    e.HighFivedUserName, e.CreatedUtc))));
    }
}