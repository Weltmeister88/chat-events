using ChatEvents.Infrastructure.Services;
using ChatEvents.Models;
using ChatEvents.Models.Reports;
using ChatEvents.Models.Reports.AggregatedReport;
using ChatEvents.Models.Reports.SingleReport;
using ChatEvents.Repositories;
using ChatEvents.Services;
using Moq;

namespace ChatEvents.UnitTests.Services;

[TestFixture]
public class ChatEventsServiceTests
{
    private Mock<IChatEventsRepository> _chatEventsRepositoryMock;
    private IChatEventsService _chatEventsService;

    [SetUp]
    public void Setup()
    {
        _chatEventsRepositoryMock = new Mock<IChatEventsRepository>();
        _chatEventsService = new ChatEventsService(_chatEventsRepositoryMock.Object);
    }

    [Test]
    public async Task GetChatEventsAsync_GivenMinuteByMinuteGranularity_ShouldReturnMinuteByMinuteEvents()
    {
        // Arrange
        const long chatRoomId = 1L;
        var cancellationToken = CancellationToken.None;
        var mockEvents = new List<ChatEventDto>
        {
            new (EventType.Comment, 1, "User1", "Hello", null, null, DateTimeOffset.UtcNow),
            new (EventType.HighFiveAnotherUser, 1, "User1", null, 2, "User2", DateTimeOffset.UtcNow)
        };
        
        _chatEventsRepositoryMock
            .Setup(repo => repo.GetAllEventsAsync(chatRoomId, cancellationToken))
            .ReturnsAsync(mockEvents);

        // Act
        var result = await _chatEventsService.GetChatEventsAsync(chatRoomId, Granularity.MinuteByMinute, cancellationToken);

        // Assert
        Assert.That(result, Is.Not.EqualTo(Enumerable.Empty<IChatEventReport>()));
        var list = result.ToList();
        Assert.That(list[0], Is.InstanceOf<CommentChatEventReport>());
        Assert.That(list[0].GetEvents()[0], Is.EqualTo("User1 comments: \"Hello\""));
    }

    [Test]
    public async Task GetChatEventsAsync_GivenHourlyGranularity_ShouldReturnHourlyEvents()
    {
        // Arrange
        const long chatRoomId = 1L;
        var cancellationToken = CancellationToken.None;
        var timeFirstHour = new DateTimeOffset(2020, 01, 01, 1, 0, 0, TimeSpan.Zero);
        var chatEventsFirstHour = new List<ChatEventDto>
        {
            new(EventType.EnterTheRoom, 1, "User1", null, null, null, timeFirstHour),
            new(EventType.Comment, 2, "User2", "Test", null, null, timeFirstHour.AddMinutes(2))
        };
        var timeSecondHour = timeFirstHour.AddHours(1).AddMinutes(2);
        var chatEventsSecondHour = new List<ChatEventDto>
        {
            new(EventType.HighFiveAnotherUser, 1, "User1", null, 2, "User2", timeSecondHour),
            new(EventType.HighFiveAnotherUser, 2, "User2", null, 1, "User1", timeSecondHour.AddMinutes(3)),
            new(EventType.LeaveTheRoom, 2, "User2", null, null, null, timeSecondHour.AddMinutes(5))
        };
        var groupedEvents = new List<GroupedChatEvents>
        {
            new(timeFirstHour, chatEventsFirstHour),
            new(timeSecondHour, chatEventsSecondHour)
        };
        
        _chatEventsRepositoryMock
            .Setup(repo => repo.GetHourlyAggregatedEventsAsync(chatRoomId, cancellationToken))
            .ReturnsAsync(groupedEvents);

        // Act
        var result = await _chatEventsService.GetChatEventsAsync(chatRoomId, Granularity.Hourly, cancellationToken);

        // Assert
        Assert.That(result, Is.Not.EqualTo(Enumerable.Empty<IChatEventReport>()));
        var list = result.ToList();
        Assert.That(list, Has.Count.EqualTo(2));
        Assert.That(list[0], Is.InstanceOf<HourlyAggregatedChatEventReport>());

        Assert.That(list[0].GetEvents(), Has.Length.EqualTo(2));
        Assert.That(list[0].GetTime(), Is.EqualTo(timeFirstHour.ToLocalTime().ToString("yyyy-MM-dd, h tt")));
        Assert.That(list[0].GetEvents()[0], Is.EqualTo("1 person entered the room"));
        Assert.That(list[0].GetEvents()[1], Is.EqualTo("1 comment"));
        
        Assert.That(list[1], Is.InstanceOf<HourlyAggregatedChatEventReport>());
        Assert.That(list[1].GetEvents(), Has.Length.EqualTo(2));
        Assert.That(list[1].GetTime(), Is.EqualTo(timeSecondHour.ToLocalTime().ToString("yyyy-MM-dd, h tt")));
        Assert.That(list[1].GetEvents()[0], Is.EqualTo("2 persons high-fived 2 persons"));
        Assert.That(list[1].GetEvents()[1], Is.EqualTo("1 person left the room"));
    }

    [Test]
    public async Task GetChatEventsAsync_GivenDailyGranularity_ShouldReturnDailyEvents()
    {
        // Arrange
        const long chatRoomId = 1L;
        var cancellationToken = CancellationToken.None;
        var firstDayTime = new DateTimeOffset(2020, 01, 01, 1, 0, 0, TimeSpan.Zero);
        var firstDayEvents = new List<ChatEventDto>
        {
            new(EventType.LeaveTheRoom, 3, "User3", null, null, null, firstDayTime),
            new(EventType.HighFiveAnotherUser, 4, "User4", null, 5, "User5",firstDayTime)
        };
        var secondDayTime = firstDayTime.AddDays(1);
        var secondDayEvents = new List<ChatEventDto>
        {
            new(EventType.EnterTheRoom, 1, "User1", null, null, null, secondDayTime),
            new(EventType.Comment, 1, "User1", "Hi!", null, null, secondDayTime.AddMinutes(6)),
            new(EventType.LeaveTheRoom, 1, "User1", null, null, null, secondDayTime.AddMinutes(13)),
        };
        var groupedEvents = new List<GroupedChatEvents>
        {
            new(firstDayTime, firstDayEvents),
            new(secondDayTime, secondDayEvents)
        };
        
        _chatEventsRepositoryMock
            .Setup(repo => repo.GetDailyAggregatedEventsAsync(chatRoomId, cancellationToken))
            .ReturnsAsync(groupedEvents);

        // Act
        var result = await _chatEventsService.GetChatEventsAsync(chatRoomId, Granularity.Daily, cancellationToken);

        // Assert
        Assert.That(result, Is.Not.EqualTo(Enumerable.Empty<IChatEventReport>()));
        var list = result.ToList();
        Assert.That(list, Has.Count.EqualTo(2));
        
        Assert.That(list[0], Is.InstanceOf<DailyAggregatedChatEventReport>());
        Assert.That(list[0].GetEvents(), Has.Length.EqualTo(2));
        Assert.That(list[0].GetTime(), Is.EqualTo(firstDayTime.ToLocalTime().ToString("yyyy-MM-dd")));
        Assert.That(list[0].GetEvents(), Does.Contain("1 person left the room"));
        Assert.That(list[0].GetEvents(), Does.Contain("1 person high-fived 1 person"));
        
        Assert.That(list[1], Is.InstanceOf<DailyAggregatedChatEventReport>());
        Assert.That(list[1].GetEvents(), Has.Length.EqualTo(3));
        Assert.That(list[1].GetTime(), Is.EqualTo(secondDayTime.ToLocalTime().ToString("yyyy-MM-dd")));
        Assert.That(list[1].GetEvents(), Does.Contain("1 person entered the room"));
        Assert.That(list[1].GetEvents(), Does.Contain("1 person left the room"));
        Assert.That(list[1].GetEvents(), Does.Contain("1 comment"));
    }

    [Test]
    public void GetChatEventsAsync_ShouldThrowException_ForInvalidGranularity()
    {
        // Arrange
        const long chatRoomId = 1L;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            await _chatEventsService.GetChatEventsAsync(chatRoomId, (Granularity)99, cancellationToken));
    }
}