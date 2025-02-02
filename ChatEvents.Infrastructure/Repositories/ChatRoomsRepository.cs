using ChatEvents.Infrastructure.Database;
using ChatEvents.Models;
using ChatEvents.Models.DbEntities;
using ChatEvents.Models.Reports;
using ChatEvents.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChatEvents.Infrastructure.Repositories;

public class ChatRoomsRepository(ApplicationDbContext context)
    : GenericEntityFrameworkRepository<ChatRoomDbEntity>(context), IChatRoomsRepository
{
    public async Task<bool> RoomExistsAsync(long chatRoomId, CancellationToken cancellationToken)
    {
        return await Find(cr => cr.Id == chatRoomId).AnyAsync(cancellationToken);
    }
}