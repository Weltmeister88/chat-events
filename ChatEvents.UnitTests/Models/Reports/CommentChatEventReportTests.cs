using ChatEvents.Models;
using ChatEvents.Models.Reports;
using ChatEvents.Models.Reports.SingleReport;

namespace ChatEvents.UnitTests.Models.Reports;

[TestFixture]
public class CommentChatEventReportTests
{
    [Test]
    public void GetTime_ShouldReturnFormattedTime()
    {
        // Arrange
        var dateTimeOffset = new DateTimeOffset(2024, 1, 30, 14, 30, 0, TimeSpan.Zero); // UTC time
        var chatEvent = new ChatEventDto(EventType.Comment, 1, "User1", "Hello", null, null, dateTimeOffset);
        var report = new CommentChatEventReport(chatEvent);

        // Act
        string time = report.GetTime();

        // Assert
        Assert.That(dateTimeOffset.ToLocalTime().ToString("yyyy-MM-dd, h:mm tt"), Is.EqualTo(time));
    }

    [Test]
    public void GetEvents_ShouldReturnFormattedComment()
    {
        // Arrange
        var chatEvent = new ChatEventDto(EventType.Comment, 1, "User1", "Hello", null, null, DateTimeOffset.UtcNow);
        var report = new CommentChatEventReport(chatEvent);

        // Act
        string[] events = report.GetEvents();

        // Assert
        Assert.That(events, Has.Length.EqualTo(1));
        Assert.That(events[0], Is.EqualTo("User1 comments: \"Hello\""));
    }
}