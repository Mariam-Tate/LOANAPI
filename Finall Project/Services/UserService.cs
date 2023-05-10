using Finall_Project.Helpers;
using LoanAPI.Data;
using LoanAPI.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Finall_Project.Services
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
            if (string.IsNullOrEmpty(loginmodel.UserName) || string.IsNullOrEmpty(loginmodel.Password))
                return null;
            var x = _users.Users.ToList();
            var user = _users.Users.FirstOrDefault(x => x.UserName == loginmodel.UserName);

            if (user == null)
                return null;

            if (PasswordHasher.HashPass(loginmodel.Password) != user.Password)
                return null;

            return user;
        }
        public User GetUserById(int id)
        {
            return _users.Users.FirstOrDefault(x => x.Id == id);
        }
        public List<User> GetAll()
        {
            return _users.Users.ToList();
        }
        public User Update(User user)
        {
            _users.Users.Update(user);
            _users.SaveChanges();
            return user;
        }
        public User BlockUser(int id, bool isBlocked)
        {
            var user = _users.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                user.IsBlocked = isBlocked;
                _users.Entry(user).State = EntityState.Modified;
                _users.SaveChanges();
            }
            return user;
        }
        public void Delete(int id)
        {
            User user = _users.Users.FirstOrDefault(x => x.Id == id);
            _users.Users.Remove(user);
            _users.SaveChanges();
            return;
        }
        public User GetUserByUsername(string username)
        {
            return _users.Users.FirstOrDefault(u => u.UserName == username);
        }
       
    }
}
