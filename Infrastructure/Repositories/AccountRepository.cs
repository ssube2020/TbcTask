using AutoMapper;
using Core.Contracts.Repositories;
using Core.Models.Entities;
using Infrastructure.DataContext;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AccountRepository(ApplicationDbContext context, IMapper mapper) : IAccountRepository
{
    public async Task AddUserAsync(User? entity)
    {
        await context.Users.AddAsync(entity);
    }

    public async Task EditUserAsync(User editModel)
    {
        var userToEdit = await context.Users
            .Include(x => x.PhoneNumbers)
            .SingleOrDefaultAsync(u => u.Id == editModel.Id);
        
        userToEdit.Name = editModel.Name;
        userToEdit.Surname = editModel.Surname;
        userToEdit.PersonalNumber = editModel.PersonalNumber;
        userToEdit.Gender = editModel.Gender;
        userToEdit.BirthDate = editModel.BirthDate;
        userToEdit.City = editModel.City;
        
        if (editModel.PhoneNumbers != null)
        {
            var phonesToRemove = userToEdit.PhoneNumbers
                .Where(p => !editModel.PhoneNumbers.Any(x => x.PhoneNumber == p.PhoneNumber && x.PhoneNumberType == p.PhoneNumberType))
                .ToList();

            foreach (var phone in phonesToRemove)
            {
                context.Phones.Remove(phone);
            }
            
            foreach (var newPhone in editModel.PhoneNumbers)
            {
                if (!userToEdit.PhoneNumbers.Any(p => p.PhoneNumber == newPhone.PhoneNumber))
                {
                    newPhone.UserId = userToEdit.Id;
                    userToEdit.PhoneNumbers.Add(newPhone);
                }
            }
        }

        context.Users.Update(userToEdit);
    }
    
    public async Task SavePhotoAsync(int userId, string photoUrl)
    {
        var user = await context
            .Users
            .SingleOrDefaultAsync(x => x.Id == userId);

        user.PhotoUrl = photoUrl;

        context.Users.Update(user);
    }

    public async Task DeleteUserAsync(int userId)
    {
        var user = await context
            .Users
            .Include(x=> x.PhoneNumbers)
            .Include(x => x.Connections)
            .SingleOrDefaultAsync(x => x.Id == userId);

        context.Phones.RemoveRange(user.PhoneNumbers);
        context.ConnectedPersons.RemoveRange(user.Connections);
        
        context.Users.Remove(user);
    }

    public async Task DeleteConnectedUserAsync(int userId, int connectedUserId)
    {
        var connectedPerson = await context
            .ConnectedPersons
            .SingleOrDefaultAsync(x => x.ConnectedPersonId == connectedUserId && x.UserId == userId);

        context.ConnectedPersons.Remove(connectedPerson);
    }

    public async Task<List<User>> GetUsersAsync(string? name, string? surname, string? personal, int page, int size)
    {
        return await context.Users
            .AsNoTracking()
            .Include(x=>x.Connections)
            .Include(x=>x.PhoneNumbers)
            .Where(x => (string.IsNullOrEmpty(name) || x.Name.ToLower().Contains(name.ToLower()))
                        && (string.IsNullOrEmpty(surname) || x.Surname.ToLower().Contains(surname.ToLower()))
                        && (string.IsNullOrEmpty(personal) || x.PersonalNumber.ToLower().Contains(personal.ToLower())))
            .Skip(page * size)
            .Take(size)
            .ToListAsync();
    }


    public async Task<User?> GetUserAsync(int userId)
    {
        var res = await context.Users
            .AsNoTracking()
            .Include(x => x.PhoneNumbers)
            .Include(x => x.Connections)
            .SingleOrDefaultAsync(x => x.Id == userId);

        return res;
    }

    public async Task<bool> CheckIfUserByPersonalNumber(string personalNumber)
    {
        return await context.Users.AsNoTracking().AnyAsync(u => u.PersonalNumber == personalNumber);
    }
    
    public async Task<bool> CheckUserByIdAsync(int Id)
    {
        return await context.Users.AsNoTracking().AnyAsync(u => u.Id == Id);
    }

    public async Task<int> GetUserIdsCountByIdsAsync(List<int> userIds)
    {
        return await context.Users
            .AsNoTracking()
            .Where(u => userIds.Contains(u.Id))
            .CountAsync();
    }


}