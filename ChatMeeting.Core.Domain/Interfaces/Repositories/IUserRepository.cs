using ChatMeeting.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatMeeting.Core.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task AddUser(User user);
        Task<User?> GetUserById(Guid id);
        Task<User?> GetUserByLogin(string login);
    }
}
