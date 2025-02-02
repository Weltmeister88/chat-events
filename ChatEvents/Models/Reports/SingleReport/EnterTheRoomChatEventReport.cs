namespace ChatEvents.Models.Reports.SingleReport;

public class EnterTheRoomChatEventReport(ChatEventDto chatEventDb) : ChatEventReport(chatEventDb)
{
    public override string[] GetEvents()
    {
        return [$"{ChatEvent.UserName} enters the room"];
    }
}