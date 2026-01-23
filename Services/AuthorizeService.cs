using CommonLibrary;

namespace DxBlazorApplication7.Data
{
    public interface AuthorizeUserService
    {
        Task<Account> Authenticate(string username, string password);
        Task<IEnumerable<Account>> GetAll();
    }

    public class AuthorizeService : AuthorizeUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<Account> _users = new List<Account>
        {
            new Account { UId = 1, FirstName = "Test", LastName = "User", UserName = "test", Password = "test" }
        };

        public async Task<Account> Authenticate(string username, string password)
        {
            // wrapped in "await Task.Run" to mimic fetching user from a db
            var user = await Task.Run(() => _users.SingleOrDefault(x => x.UserName == username && x.Password == password));

            // on auth fail: null is returned because user is not found
            // on auth success: user object is returned
            return user;
        }

        public async Task<IEnumerable<Account>> GetAll()
        {
            // wrapped in "await Task.Run" to mimic fetching users from a db
            return await Task.Run(() => _users);
        }
    }
}
