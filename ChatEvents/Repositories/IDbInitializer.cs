namespace ChatEvents.Repositories;

public interface IDbInitializer
{
    public Task InitializeAsync(CancellationToken cancellationToken = default);
}