using System.ComponentModel.DataAnnotations;

namespace XoriantPrototype.Models.Account
{
    public class UserRegistration
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Gender { get; set; }
        [Key]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Mobile { get; set; }
        public byte[] Image { get; set; }
    }
}
