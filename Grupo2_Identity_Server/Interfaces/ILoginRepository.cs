using Grupo2_Identity_Server.Entities;

namespace Grupo2_Identity_Server.Interfaces
{
    public interface ILoginRepository
    {
        Task AddAsync(User user);
        Task<IEnumerable<User>> GetByEmailAsync(string email);
    }
}
