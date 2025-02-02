using ChatEvents.Models;
using ChatEvents.Models.Reports;
using ChatEvents.Models.Reports.SingleReport;

namespace ChatEvents.Extensions;

public static class ChatEventReportSourceExtensions
{
    public static IChatEventReport ToChatEventReport(this ChatEventDto chatEvent)
    {
        EventType eventType = chatEvent.EventType;
        return eventType switch
        {
            EventType.EnterTheRoom => new EnterTheRoomChatEventReport(chatEvent),
            EventType.LeaveTheRoom => new LeaveTheRoomChatEventReport(chatEvent),
            EventType.Comment => new CommentChatEventReport(chatEvent),
            EventType.HighFiveAnotherUser => new HighFiveAnotherUserChatEventReport(chatEvent),
            _ => throw new ArgumentOutOfRangeException(nameof(eventType), eventType,
                "Unsupported Event Type found")
        };
    }
    
    public static IEnumerable<ChatEventDto> GetTypedEvents(this IEnumerable<ChatEventDto> events, EventType eventType)
    {
        return events.Where(e => e.EventType == eventType);
        
    }
}