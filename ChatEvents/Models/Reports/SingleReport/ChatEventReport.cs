namespace ChatEvents.Models.Reports.SingleReport;

public abstract class ChatEventReport(ChatEventDto chatEventDb) : IChatEventReport
{
    protected readonly ChatEventDto ChatEvent = chatEventDb;
    
    public string GetTime()
    {
        return ChatEvent.CreatedUtc.ToLocalTime().ToString("yyyy-MM-dd, h:mm tt");
    }

    public abstract string[] GetEvents();
}