using ChatEvents.Extensions;
using ChatEvents.Models;
using ChatEvents.Models.Reports;
using ChatEvents.Models.Reports.AggregatedReport;
using ChatEvents.Repositories;
using ChatEvents.Services;

namespace ChatEvents.Infrastructure.Services;

public class ChatEventsService(IChatEventsRepository chatEventsRepository) : IChatEventsService
{
    public async Task<IEnumerable<IChatEventReport>> GetChatEventsAsync(long chatRoomId, Granularity granularity, CancellationToken cancellationToken)
    {
        switch (granularity)
        {
            case Granularity.MinuteByMinute:
                return await GetAllEventsAsync(chatRoomId, cancellationToken);
            case Granularity.Hourly:
                return await GetHourlyEventsAsync(chatRoomId, cancellationToken);
            case Granularity.Daily:
                return await GetDailyEventsAsync(chatRoomId, cancellationToken);
            default:
                throw new ArgumentOutOfRangeException(nameof(granularity), granularity, null);
        }
    }

    private async Task<IEnumerable<IChatEventReport>> GetDailyEventsAsync(long chatRoomId, CancellationToken cancellationToken)
    {
        var dailyEvents = await chatEventsRepository.GetDailyAggregatedEventsAsync(chatRoomId, cancellationToken);
        return dailyEvents.Select(ce => new DailyAggregatedChatEventReport(ce));
    }

    private async Task<IEnumerable<IChatEventReport>> GetHourlyEventsAsync(long chatRoomId, CancellationToken cancellationToken)
    {
        var hourlyEvents = await chatEventsRepository.GetHourlyAggregatedEventsAsync(chatRoomId, cancellationToken);
        return hourlyEvents.Select(ce => new HourlyAggregatedChatEventReport(ce));
    }

    private async Task<IEnumerable<IChatEventReport>> GetAllEventsAsync(long chatRoomId, CancellationToken cancellationToken)
    {
        var events = await chatEventsRepository.GetAllEventsAsync(chatRoomId, cancellationToken);
        return events.Select(ce => ce.ToChatEventReport());
    }
}