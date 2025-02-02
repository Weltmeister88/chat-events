namespace ChatEvents.Models.Reports;

public interface IChatEventReport
{
    public string GetTime();
    public string[] GetEvents();
}