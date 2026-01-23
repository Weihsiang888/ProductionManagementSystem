using DxBlazorApplication7.Data;
using CommonLibrary.AuthPack;

namespace DxBlazorApplication7.Data
{
    public class UserDataService : IAuthDataService
    {
        private readonly IServiceScopeFactory scopeFactory;
        public UserDataService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }
        public Task<List<UserDetails>> GetUserDetails()
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                return Task.FromResult(dbContext.UsersDetails.ToList());
            }
        }
        public Task<bool> CheckUserPassword(string username, string password)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                bool userExist = dbContext.UsersDetails.Any(x => x.UserName == username);
                if (userExist)
                {
                    UserDetails target = dbContext.UsersDetails.FirstOrDefault(x => x.UserName == username);
                    if (PasswordHelper.VerifyPassword(password, target.HashedPassword))
                    {
                        return Task.FromResult(true);
                    }
                    else
                    {
                        return Task.FromResult(false);
                    }
                }
                else
                {
                    return Task.FromResult(false);
                }
            }
        }

        public UserDetails GetUserByName(string userName)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                UserDetails res = dbContext.UsersDetails.FirstOrDefault(x => x.UserName == userName);
                if (res != null)
                {
                    return new UserDetails
                    {
                        UserName = res.UserName,
                        Department = res.Department,
                        Role = res.Role,
                        HashedPassword = res.HashedPassword,
                        CreateTime = res.CreateTime,
                        LastLoginTime = res.LastLoginTime,
                        Token = res.Token,
                    };
                }
                else
                {
                    return null;
                }
            }
        }

        public UserDetails GetUserByToken(string token)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                UserDetails res = dbContext.UsersDetails.FirstOrDefault(x => x.Token == token);
                if (res != null)
                {
                    return new UserDetails
                    {
                        UserName = res.UserName,
                        Department = res.Department,
                        Role = res.Role,
                        HashedPassword = res.HashedPassword,
                        CreateTime = res.CreateTime,
                        LastLoginTime = res.LastLoginTime,
                        Token = res.Token,
                    };
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<bool> UserLoginWithNewToken(string userName, string newToken)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                UserDetails res = dbContext.UsersDetails.FirstOrDefault(x => x.UserName == userName);
                if (res != null)
                {
                    res.Token = newToken;
                    res.LastLoginTime = DateTime.Now;
                    await dbContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> UserLoginWithToken(string token)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                UserDetails res = dbContext.UsersDetails.FirstOrDefault(x => x.Token == token);
                if (res != null)
                {
                    res.LastLoginTime = DateTime.Now;
                    await dbContext.SaveChangesAsync();

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> UserLogoutWithName(string userName)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                if (!string.IsNullOrEmpty(userName))
                {
                    UserDetails res = dbContext.UsersDetails.FirstOrDefault(x => x.UserName == userName);

                    if (res != null)
                    {
                        res.Token = "";
                        await dbContext.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task UpsertUser(UserDetails usersDetail)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                bool userExist = dbContext.UsersDetails.Any(x => x.UserName == usersDetail.UserName);
                if (userExist)
                {
                    //update
                    UserDetails target = dbContext.UsersDetails.FirstOrDefault(x => x.UserName == usersDetail.UserName);
                    target.Department = usersDetail.Department;
                    target.Role = usersDetail.Role;
                    target.HashedPassword = usersDetail.HashedPassword;
                }
                else
                {
                    await dbContext.UsersDetails.AddAsync(usersDetail);
                }
                await dbContext.SaveChangesAsync();
            }
        }

		public async Task DeleteUser(string userName)
		{
            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                bool userExist = dbContext.UsersDetails.Any(x => x.UserName == userName);
                if (userExist)
                {
					UserDetails? target = dbContext.UsersDetails.FirstOrDefault(x => x.UserName == userName);
                    dbContext.UsersDetails.Remove(target);
					await dbContext.SaveChangesAsync();
                }
                
            }
		}
	}
}
