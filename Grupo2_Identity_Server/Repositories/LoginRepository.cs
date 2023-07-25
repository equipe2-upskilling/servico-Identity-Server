using Grupo2_Identity_Server.Context;
using Grupo2_Identity_Server.Entities;
using Grupo2_Identity_Server.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Grupo2_Identity_Server.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly AppDbContext _context;

        public LoginRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetByEmailAsync(string email)
        {
            var user = await _context.Users.Where(u => u.Email == email).ToListAsync();
            return user;
        }
    }
}
