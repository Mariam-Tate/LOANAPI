using LoanAPI.Data;
using System.Collections.Generic;

namespace Finall_Project.Services
{
    public interface IUserService
    {
        User AddUser(User user);
        User Login(LoginUser loginmodel);
        User GetUserById(int id);
        List<User> GetAll();
        User Update(User user);
        User BlockUser(int id, bool isBlocked);
        void Delete(int id);
        User GetUserByUsername(string username);
       
    }

}
