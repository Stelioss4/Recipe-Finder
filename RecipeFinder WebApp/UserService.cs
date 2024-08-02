using Microsoft.EntityFrameworkCore;
using Recipe_Finder;
using RecipeFinder_WebApp.Components.Account;
using System.Threading.Tasks;

namespace RecipeFinder_WebApp
{
    public class UserService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UsersProfile> GetUserAsync(string username, string password)
        {
            return await _dbContext.UsersProfiles
                .FirstOrDefaultAsync(u => u.Email == username && u.Password == password);
        }
    }
}
