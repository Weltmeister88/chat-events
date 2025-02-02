using ChatEvents.Models;
using ChatEvents.Models.DbEntities;
using ChatEvents.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ChatEvents.Infrastructure.Database;

public class DbInitializer(IServiceScopeFactory serviceScopeFactory) : IDbInitializer
{
    private readonly IServiceScope _scope = serviceScopeFactory.CreateScope();

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        var dbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
        
        if (dbContext.ChatEvents.Any())
        {
            return;
        }

        var kate = new UserDbEntity
        {
            Name = "Kate"
        };
        var bob = new UserDbEntity
        {
            Name = "Bob"
        };
        var users = new[]
        {
            kate,
            bob
        };
        dbContext.Users.AddRange(users);

        var chatRoom = new ChatRoomDbEntity
        {
            Name = "Chat room 1"
        };
        dbContext.ChatRooms.Add(chatRoom);
        
        var chatEvents = new[]
        {
            new ChatEventDbEntity
            {
                ChatRoom = chatRoom,
                EventType = EventType.EnterTheRoom,
                User = bob,
                CreatedUtc = CreateDateForGivenTime(5, 0)
            },
            new ChatEventDbEntity
            {
                ChatRoom = chatRoom,
                EventType = EventType.EnterTheRoom,
                User = kate,
                CreatedUtc = CreateDateForGivenTime(5, 5)
            },
            new ChatEventDbEntity
            {
                ChatRoom = chatRoom,
                EventType = EventType.Comment,
                Comment = "Hey, Kate - high five?",
                User = bob,
                CreatedUtc = CreateDateForGivenTime(5, 15)
            },
            new ChatEventDbEntity
            {
                ChatRoom = chatRoom,
                EventType = EventType.HighFiveAnotherUser,
                User = kate,
                HighFivedUser = bob,
                CreatedUtc = CreateDateForGivenTime(5, 17)
            },
            new ChatEventDbEntity
            {
                ChatRoom = chatRoom,
                EventType = EventType.HighFiveAnotherUser,
                User = bob,
                HighFivedUser = kate,
                CreatedUtc = CreateDateForGivenTime(5, 17)
            },
            new ChatEventDbEntity
            {
                ChatRoom = chatRoom,
                EventType = EventType.LeaveTheRoom,
                User = bob,
                CreatedUtc = CreateDateForGivenTime(5, 18)
            },
            new ChatEventDbEntity
            {
                ChatRoom = chatRoom,
                EventType = EventType.Comment,
                User = kate,
                Comment = "Oh, typical",
                CreatedUtc = CreateDateForGivenTime(5, 20)
            },
            new ChatEventDbEntity
            {
                ChatRoom = chatRoom,
                EventType = EventType.LeaveTheRoom,
                User = kate,
                CreatedUtc = CreateDateForGivenTime(5, 21)
            },
            new ChatEventDbEntity
            {
                ChatRoom = chatRoom,
                EventType = EventType.EnterTheRoom,
                User = kate,
                CreatedUtc = CreateDateForGivenTime(6, 6)
            },
            new ChatEventDbEntity
            {
                ChatRoom = chatRoom,
                EventType = EventType.LeaveTheRoom,
                User = kate,
                CreatedUtc = CreateDateForGivenTime(6, 8)
            },
            new ChatEventDbEntity
            {
                ChatRoom = chatRoom,
                EventType = EventType.EnterTheRoom,
                User = bob,
                CreatedUtc = CreateDateForGivenTime(6, 6).AddDays(1)
            },
            new ChatEventDbEntity
            {
                ChatRoom = chatRoom,
                EventType = EventType.LeaveTheRoom,
                User = bob,
                CreatedUtc = CreateDateForGivenTime(6, 8).AddDays(1)
            }
        };
        dbContext.ChatEvents.AddRange(chatEvents);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static DateTimeOffset CreateDateForGivenTime(int hours, int minutes)
    {
        return new DateTimeOffset(new DateOnly(2025, 1, 31), new TimeOnly(hours, minutes), TimeSpan.Zero);
    }
}