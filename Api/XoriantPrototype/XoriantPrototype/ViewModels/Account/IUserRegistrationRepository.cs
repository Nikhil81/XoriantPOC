using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XoriantPrototype.Models.Account;

namespace XoriantPrototype.ViewModels.Account
{
    public interface IUserRegistrationRepository
    {
        bool GetUserByUsername(string username);

        bool SaveAll();
        bool SaveUserRegistrationInfo(UserInformation model);
    }
}
