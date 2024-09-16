using Core.Models.Entities;

namespace Core.Contracts.Repositories;

public interface IConnectionRepository
{
    Task AddConnectionAsync(ConnectedPerson entity);
    Task<List<User>> GetConnectionsReportByType(ConnectionType type);
    
    Task<bool> ConnectionExistsAsync(int userId, int connectionId);
}