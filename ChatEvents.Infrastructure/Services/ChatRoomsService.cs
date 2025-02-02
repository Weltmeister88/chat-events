using ChatEvents.Repositories;
using ChatEvents.Services;

namespace ChatEvents.Infrastructure.Services;

public class ChatRoomsService(IChatRoomsRepository chatRoomsRepository) : IChatRoomsService
{
    public async Task<bool> RoomExistsAsync(long chatRoomId, CancellationToken cancellationToken)
    {
        return await chatRoomsRepository.RoomExistsAsync(chatRoomId, cancellationToken);
    }
}