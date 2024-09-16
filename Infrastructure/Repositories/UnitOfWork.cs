using Core.Contracts.Repositories;
using Infrastructure.DataContext;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Repositories;

public class UnitOfWork(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
    : IUnitOfWork
{
    public IAccountRepository accountRepository => serviceProvider.GetService<IAccountRepository>();
    public IConnectionRepository connectionRepository => serviceProvider.GetService<IConnectionRepository>();

    public async Task Commit()
    {
        await dbContext.SaveChangesAsync();
    }
}