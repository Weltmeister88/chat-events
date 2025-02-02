using ChatEvents.Models.Reports;

namespace ChatEvents.API.Endpoints.AggregatedEvents.DTOs;

[Serializable]
internal record AggregationOccurenceResponse
{
    public string GroupIdentifier { get; set; } = default!;
    public string[] Events { get; set; } = default!;

    public AggregationOccurenceResponse()
    {
    }
    
    public AggregationOccurenceResponse(IChatEventReport report)
    {
        GroupIdentifier = report.GetTime();
        Events = report.GetEvents();
    }
}