using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace XoriantPrototype.ViewModels.Account
{
    public class UserInformation
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string Gender { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Mobile { get; set; }
        public IFormFile UserPhoto { get; set; }
    }
}
