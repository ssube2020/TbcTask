using Core.Contracts.Repositories;
using Core.Models.Entities;
using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ConnectionRepository(ApplicationDbContext context) : IConnectionRepository
{
    public async Task AddConnectionAsync(ConnectedPerson entity)
    {
        await context.ConnectedPersons.AddAsync(entity);
    }

    public async Task<List<User>> GetConnectionsReportByType(ConnectionType entity)
    {
        var res = await context.Users
            .AsNoTracking()
            .Include(x => x.PhoneNumbers)
            .Include(x => x.Connections
                .Where(c=>c.ConnectionType == entity))
            .ToListAsync();

        return res;
    }

    public async Task<bool> ConnectionExistsAsync(int userId, int connectionId)
    {
        return await context.ConnectedPersons
            .AsNoTracking()
            .AnyAsync(x => x.UserId == userId && x.ConnectedPersonId == connectionId);
    }
}