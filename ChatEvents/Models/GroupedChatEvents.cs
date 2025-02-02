using ChatEvents.Models.Reports;

namespace ChatEvents.Models;

public record GroupedChatEvents(DateTimeOffset CreatedAtUtc, IEnumerable<ChatEventDto> Events);