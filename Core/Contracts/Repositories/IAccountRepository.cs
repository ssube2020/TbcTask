using Core.Models.Entities;

namespace Core.Contracts.Repositories;

public interface IAccountRepository
{
    Task<List<User>> GetUsersAsync(string? name, string? surname, string? personal, int page, int size);
    Task<User?> GetUserAsync(int userId);
    Task<bool> CheckIfUserByPersonalNumber(string personalNumber);
    Task<bool> CheckUserByIdAsync(int Id);
    Task AddUserAsync(User? entity);
    Task EditUserAsync(User editModel);
    Task SavePhotoAsync(int userId, string photoUrl);
    Task DeleteUserAsync(int userId);
    Task DeleteConnectedUserAsync(int userId, int connectedUserId);
    Task<int> GetUserIdsCountByIdsAsync(List<int> userIds);
}