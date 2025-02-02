namespace ChatEvents.Models.Reports;

public record ChatEventDto(
    EventType EventType,
    long UserId,
    string UserName,
    string? Comment,
    long? HighFivedUserId,
    string? HighFivedUserName,
    DateTimeOffset CreatedUtc);