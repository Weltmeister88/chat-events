namespace ChatEvents.Services;

public interface IChatRoomsService
{
    Task<bool> RoomExistsAsync(long chatRoomId, CancellationToken cancellationToken);
}