namespace ChatEvents.Models.Reports.SingleReport;

public class HighFiveAnotherUserChatEventReport(ChatEventDto chatEventDb) : ChatEventReport(chatEventDb)
{
    public override string[] GetEvents()
    {
        return [$"{ChatEvent.UserName} high-fives {ChatEvent.HighFivedUserName}"];
    }
}