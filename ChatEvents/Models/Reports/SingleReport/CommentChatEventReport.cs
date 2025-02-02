namespace ChatEvents.Models.Reports.SingleReport;

public class CommentChatEventReport(ChatEventDto chatEventDb) : ChatEventReport(chatEventDb)
{
    public override string[] GetEvents()
    {
        return [$"{ChatEvent.UserName} comments: \"{ChatEvent.Comment}\""];
    }
}