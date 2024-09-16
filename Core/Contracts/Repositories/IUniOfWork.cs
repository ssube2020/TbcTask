namespace Core.Contracts.Repositories;

public interface IUnitOfWork
{
    IAccountRepository accountRepository { get; }
    IConnectionRepository connectionRepository { get; }
    Task Commit();
}