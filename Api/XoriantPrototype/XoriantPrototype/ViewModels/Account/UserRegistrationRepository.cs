using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XoriantPrototype.Common;
using XoriantPrototype.Models;
using XoriantPrototype.Models.Account;

namespace XoriantPrototype.ViewModels.Account
{
    public class UserRegistrationRepository : IUserRegistrationRepository
    {
        private readonly AppDbContext context;
        private readonly ILogger<UserRegistrationRepository> logger;

        public UserRegistrationRepository(AppDbContext context, ILogger<UserRegistrationRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public bool GetUserByUsername(string username)
        {
            var isUserExsits = context.UserRegistration.Find(username);
            if (isUserExsits != null)
            {
                return false;
            }
            return true;
        }

        public bool SaveAll()
        {
            try
            {
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in calling SaveAll {ex}");
                return false;
            }
        }

        public bool SaveUserRegistrationInfo(UserInformation userInfo)
        {
            byte[] imageContent = null;
            if (userInfo.UserPhoto != null && userInfo.UserPhoto.Length > 0)
            {
                using (var imageStream = userInfo.UserPhoto.OpenReadStream())
                using (var memoryStream = new MemoryStream())
                {
                    imageStream.CopyTo(memoryStream);
                    imageContent = memoryStream.ToArray();
                }
            }

            //TODO: Use Auto Mapper
            UserRegistration userRegistration = new UserRegistration
            {
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                Email = userInfo.Email,
                Gender = userInfo.Gender,
                Mobile = userInfo.Mobile,
                Password = Helper.EncodePasswordMd5(userInfo.Password),
                UserName = userInfo.UserName,
                Image = imageContent
            };

            context.UserRegistration.Add(userRegistration);
            return SaveAll();
        }
    }
}
