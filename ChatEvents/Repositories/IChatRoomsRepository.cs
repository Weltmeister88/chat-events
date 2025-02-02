namespace ChatEvents.Repositories;

public interface IChatRoomsRepository
{
    Task<bool> RoomExistsAsync(long chatRoomId, CancellationToken cancellationToken);
}