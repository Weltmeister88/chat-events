namespace ChatEvents.Models.Reports.AggregatedReport;

public class DailyAggregatedChatEventReport(GroupedChatEvents groupedEvents) : AggregatedChatEventReport(groupedEvents)
{
    public override string GetTime()
    {
        return Time.ToLocalTime().ToString("yyyy-MM-dd");
    }
}