using System.ComponentModel.DataAnnotations;

namespace PustokStart.ViewModels
{
    public class MemberLoginViewModel
    {
        [Required]
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email {get;set;}
        [Required]
        [DataType(DataType.Password)]
        [MaxLength(20)]
        public string Password { get;set;}
    }
}
