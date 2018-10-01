using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XoriantPrototype.Models.Account;
using XoriantPrototype.ViewModels.Account;

namespace XoriantPrototype.DAL.Registration
{
    public interface IUserRegistrationRepository
    {
        bool GetUserByEmail(string email);

        bool SaveAll();
        bool SaveUserRegistrationInfo(UserInformation model);
    }
}
