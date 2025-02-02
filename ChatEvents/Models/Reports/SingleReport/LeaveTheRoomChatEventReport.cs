namespace ChatEvents.Models.Reports.SingleReport;

public class LeaveTheRoomChatEventReport(ChatEventDto chatEventDb) : ChatEventReport(chatEventDb)
{
    public override string[] GetEvents()
    {
        return [$"{ChatEvent.UserName} enters the room"];
    }
}