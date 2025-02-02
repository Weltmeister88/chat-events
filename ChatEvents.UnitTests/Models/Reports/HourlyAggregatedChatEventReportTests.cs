using ChatEvents.Models;
using ChatEvents.Models.Reports;
using ChatEvents.Models.Reports.AggregatedReport;

namespace ChatEvents.UnitTests.Models.Reports;

[TestFixture]
public class HourlyAggregatedChatEventReportTests
{
    [Test]
    public void GetTime_ShouldReturnFormattedHour()
    {
        // Arrange
        var dateTimeOffset = new DateTimeOffset(2024, 1, 30, 14, 0, 0, TimeSpan.Zero); // UTC time
        var groupedEvents = new GroupedChatEvents(dateTimeOffset, new List<ChatEventDto>());
        var report = new HourlyAggregatedChatEventReport(groupedEvents);

        // Act
        string time = report.GetTime();

        // Assert
        Assert.That(dateTimeOffset.ToLocalTime().ToString("yyyy-MM-dd, h tt"), Is.EqualTo(time));
    }

    [Test]
    public void GetEvents_ShouldReturnAggregatedEvents()
    {
        // Arrange
        var events = new List<ChatEventDto>
        {
            new(EventType.Comment, 1, "User1", "Hello", null, null, DateTimeOffset.UtcNow),
            new(EventType.Comment, 2, "User2", "Hi", null, null, DateTimeOffset.UtcNow),
            new(EventType.EnterTheRoom, 3, "User3", null, null, null, DateTimeOffset.UtcNow),
            new(EventType.LeaveTheRoom, 4, "User4", null, null, null, DateTimeOffset.UtcNow),
            new(EventType.HighFiveAnotherUser, 1, "User1", null, 2, "User2", DateTimeOffset.UtcNow)
        };

        var groupedEvents = new GroupedChatEvents(DateTimeOffset.UtcNow, events);
        var report = new HourlyAggregatedChatEventReport(groupedEvents);

        // Act
        string[] eventSummaries = report.GetEvents();

        // Assert
        Assert.That(eventSummaries, Does.Contain("2 comments"));
        Assert.That(eventSummaries, Does.Contain("1 person entered the room"));
        Assert.That(eventSummaries, Does.Contain("1 person left the room"));
        Assert.That(eventSummaries, Does.Contain("1 person high-fived 1 person"));
    }

    [Test]
    public void GetEvents_ShouldReturnEmptyArray_WhenNoEvents()
    {
        // Arrange
        var groupedEvents = new GroupedChatEvents(DateTimeOffset.UtcNow, new List<ChatEventDto>());
        var report = new HourlyAggregatedChatEventReport(groupedEvents);

        // Act
        string[] eventSummaries = report.GetEvents();

        // Assert
        Assert.That(eventSummaries, Is.Empty);
    }
}