using Finall_Project.Helpers;
using LoanAPI.Data;
using LoanAPI.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Finall_Project.Repository
{
 
    public class UserService : IUserService
    {
        private readonly UserContext _users;

        public UserService(UserContext users)
        {
            _users = users;
        }

        public User AddUser(User user)
        {
            _users.Users.Add(user);
            _users.SaveChanges();
            return user;
        }

        public User Login(LoginUser loginmodel)
        {
            if (string.IsNullOrEmpty(loginmodel.Username) || string.IsNullOrEmpty(loginmodel.Password))
                return null;

            var user = _users.Users.SingleOrDefault(x => x.Username == loginmodel.Username);

            if (user == null)
                return null;

            if (PasswordHasher.hashPass(loginmodel.Password) != user.Password)
                return null;

            return user;
        }
        public User GetUserById(int id)
        {
            return _users.Users.Include(a => a.Username).FirstOrDefault(x => x.Id == id);
        }
        public List<User> GetAll()
        {
            return _users.Users.Include(a => a.Username).ToList();
        }

        public User Update(User user)
        {
            _users.Users.Update(user);
            _users.SaveChanges();
            return user;
        }
        public void Delete(int id)
        {
            User user = _users.Users.FirstOrDefault(x => x.Id == id);

            _users.Users.Remove(user);
            _users.SaveChanges();
            return;
        }
    }
}
