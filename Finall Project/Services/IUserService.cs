using LoanAPI.Data;
using System.Collections.Generic;

namespace Finall_Project.Repository
{
    public interface IUserService
    {
        
        User AddUser(User user);
        User Login(LoginUser loginmodel);
        User GetUserById(int id);
        List<User> GetAll();
        User Update(User user);
        void Delete(int id);
    }

}
