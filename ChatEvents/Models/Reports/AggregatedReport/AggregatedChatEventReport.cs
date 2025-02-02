using ChatEvents.Extensions;

namespace ChatEvents.Models.Reports.AggregatedReport;

public abstract class AggregatedChatEventReport(GroupedChatEvents groupedEvents) : IChatEventReport
{
    private readonly IEnumerable<ChatEventDto> _events = groupedEvents.Events.ToList();
    protected readonly DateTimeOffset Time = groupedEvents.CreatedAtUtc;

    public abstract string GetTime();

    public string[] GetEvents()
    {
        string?[] array =
        [
            GetEnterTheRoomAggregateString(_events.GetTypedEvents(EventType.EnterTheRoom)),
            GetCommentAggregateString(_events.GetTypedEvents(EventType.Comment)),
            GetHighFiveAnotherUserAggregateString(_events.GetTypedEvents(EventType.HighFiveAnotherUser)),
            GetLeaveTheRoomAggregateString(_events.GetTypedEvents(EventType.LeaveTheRoom))
        ];
        return array.Where(e => e != null).Select(e => e!).ToArray();
    }
    
    private static string? GetCommentAggregateString(IEnumerable<ChatEventDto> eventReportSources)
    {
        int count = eventReportSources.Count();

        return count switch
        {
            0 => null,
            1 => count + " comment",
            _ => count + " comments"
        };
    }
    
    private static string? GetEnterTheRoomAggregateString(IEnumerable<ChatEventDto> eventReportSources)
    {
        int count = eventReportSources.Count();

        return count switch
        {
            0 => null,
            _ => $"{GetPersons(count)} entered the room"
        };
    }
    
    private static string? GetLeaveTheRoomAggregateString(IEnumerable<ChatEventDto> eventReportSources)
    {
        int count = eventReportSources.Count();

        return count switch
        {
            0 => null,
            _ => $"{GetPersons(count)} left the room"
        };
    }
    
    private static string? GetHighFiveAnotherUserAggregateString(IEnumerable<ChatEventDto> eventReportSources)
    {
        var (userCount, highFivedUsersCount) = eventReportSources
            .GroupBy(x => new { x.UserId, x.HighFivedUserId })
            .Aggregate(
                (UserIds: new HashSet<long>(), HighFivedUserIds: new HashSet<long?>()), 
                (acc, group) => 
                {
                    acc.UserIds.Add(group.Key.UserId);
                    acc.HighFivedUserIds.Add(group.Key.HighFivedUserId);
                    return acc;
                },
                acc => (acc.UserIds.Count, acc.HighFivedUserIds.Count));

        if (userCount == 0) return null;
        return $"{GetPersons(userCount)} high-fived {GetPersons(highFivedUsersCount)}";
    }

    private static string GetPersons(int count)
    {
        return count == 1 ? $"{count} person" : $"{count} persons";
    }
}