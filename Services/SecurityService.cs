using WebApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services

{
    public class SecurityService
    {
        List<UserModel> knownUsers = new List<UserModel>();
        UsersDAO usersDAO = new UsersDAO();

        public SecurityService()
        {
            
        }

        public bool IsValid(UserModel user)
        {
            return usersDAO.FindUserByNameAndPassword(user);
        }
    }
}
