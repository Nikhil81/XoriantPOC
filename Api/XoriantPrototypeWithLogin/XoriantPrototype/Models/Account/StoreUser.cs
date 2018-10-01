using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XoriantPrototype.Models.Account
{
    public class StoreUser : IdentityUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
