using ChatEvents.Models;
using ChatEvents.Models.Reports;
using ChatEvents.Models.Reports.AggregatedReport;

namespace ChatEvents.UnitTests.Models.Reports;

[TestFixture]
public class AggregatedChatEventReportTests
{
    private record MockGroupedChatEvents(IEnumerable<ChatEventDto> Events, DateTimeOffset CreatedAtUtc)
        : GroupedChatEvents(CreatedAtUtc, Events);

    private class TestReport(GroupedChatEvents groupedEvents) : AggregatedChatEventReport(groupedEvents)
    {
        public override string GetTime()
        {
            return Time.ToString("o");
        }
    }

    private static IEnumerable<ChatEventDto> CreateMockEvents(IEnumerable<EventType> eventTypes, long userId)
    {
        return eventTypes.Select(type => new ChatEventDto
        (
            type,
            userId,
            "User",
            null,
            type == EventType.HighFiveAnotherUser ? 1 : null,
            "HighFivedUser",
            DateTimeOffset.UtcNow
        ));
    }

    [Test]
    public void Test_NoEvents_ReturnsEmptyArray()
    {
        var groupedEvents = new MockGroupedChatEvents(Enumerable.Empty<ChatEventDto>(), DateTimeOffset.Now);
        var report = new TestReport(groupedEvents);

        string[] events = report.GetEvents();

        Assert.That(events.Length, Is.EqualTo(0));
    }

    [Test]
    public void Test_SingleComment_ReturnsSingleComment()
    {
        var groupedEvents = new MockGroupedChatEvents(
            CreateMockEvents(new[] { EventType.Comment }, 1),
            DateTimeOffset.Now);
        var report = new TestReport(groupedEvents);

        string[] events = report.GetEvents();

        Assert.That(events, Contains.Item("1 comment"));
    }

    [Test]
    public void Test_MultipleComments_ReturnsMultipleComments()
    {
        var groupedEvents = new MockGroupedChatEvents(
            CreateMockEvents(new[] { EventType.Comment, EventType.Comment }, 1),
            DateTimeOffset.Now);
        var report = new TestReport(groupedEvents);

        string[] events = report.GetEvents();

        Assert.That(events, Contains.Item("2 comments"));
    }

    [Test]
    public void Test_SingleEnterTheRoom_ReturnsSinglePersonEntered()
    {
        var groupedEvents = new MockGroupedChatEvents(
            CreateMockEvents(new[] { EventType.EnterTheRoom }, 1),
            DateTimeOffset.Now);
        var report = new TestReport(groupedEvents);

        string[] events = report.GetEvents();

        Assert.That(events, Contains.Item("1 person entered the room"));
    }

    [Test]
    public void Test_MultipleEnterTheRoom_ReturnsMultiplePersonsEntered()
    {
        var groupedEvents = new MockGroupedChatEvents(
            CreateMockEvents(new[] { EventType.EnterTheRoom, EventType.EnterTheRoom }, 1),
            DateTimeOffset.Now);
        var report = new TestReport(groupedEvents);

        string[] events = report.GetEvents();

        Assert.That(events, Contains.Item("2 persons entered the room"));
    }

    [Test]
    public void Test_SingleLeaveTheRoom_ReturnsSinglePersonLeft()
    {
        var groupedEvents = new MockGroupedChatEvents(
            CreateMockEvents(new[] { EventType.LeaveTheRoom }, 1),
            DateTimeOffset.Now);
        var report = new TestReport(groupedEvents);

        string[] events = report.GetEvents();

        Assert.That(events, Contains.Item("1 person left the room"));
    }

    [Test]
    public void Test_MultipleLeaveTheRoom_ReturnsMultiplePersonsLeft()
    {
        var groupedEvents = new MockGroupedChatEvents(
            CreateMockEvents(new[] { EventType.LeaveTheRoom, EventType.LeaveTheRoom }, 1),
            DateTimeOffset.Now);
        var report = new TestReport(groupedEvents);

        string[] events = report.GetEvents();

        Assert.That(events, Contains.Item("2 persons left the room"));
    }

    [Test]
    public void Test_SingleHighFive_ReturnsSingleHighFive()
    {
        var groupedEvents = new MockGroupedChatEvents(
            CreateMockEvents(new[] { EventType.HighFiveAnotherUser }, 1),
            DateTimeOffset.Now);
        var report = new TestReport(groupedEvents);

        string[] events = report.GetEvents();

        Assert.That(events, Contains.Item("1 person high-fived 1 person"));
    }

    [Test]
    public void Test_MultipleHighFives_ReturnsMultipleHighFives()
    {
        var groupedEvents = new MockGroupedChatEvents(
            [
                new ChatEventDto
                (
                    EventType.HighFiveAnotherUser,
                    1,
                    "User",
                    null,
                    2,
                    "HighFivedUser",
                    DateTimeOffset.UtcNow
                ),
                new ChatEventDto
                (
                    EventType.HighFiveAnotherUser,
                    2,
                    "HighFivedUser",
                    null,
                    1,
                    "User",
                    DateTimeOffset.UtcNow.AddSeconds(2)
                )
            ],
            DateTimeOffset.Now);
        var report = new TestReport(groupedEvents);

        string[] events = report.GetEvents();

        Assert.That(events, Contains.Item("2 persons high-fived 2 persons"));
    }

    [Test]
    public void Test_MixedEvents_ReturnsCorrectAggregates()
    {
        var groupedEvents = new MockGroupedChatEvents(
            CreateMockEvents(
                new[]
                {
                    EventType.EnterTheRoom, EventType.Comment, EventType.LeaveTheRoom, EventType.HighFiveAnotherUser
                }, 1),
            DateTimeOffset.Now);
        var report = new TestReport(groupedEvents);

        string[] events = report.GetEvents();

        Assert.That(events, Has.Member("1 person entered the room"));
        Assert.That(events, Has.Member("1 comment"));
        Assert.That(events, Has.Member("1 person left the room"));
        Assert.That(events, Has.Member("1 person high-fived 1 person"));
    }
}