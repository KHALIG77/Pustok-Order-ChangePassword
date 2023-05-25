using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PustokStart.Models
{
    public class AppUser:IdentityUser
    {
        
        public string FullName {get; set;}
        public bool IsAdmin {get; set;} 
        public string Phone {get; set;}
        public string Address {get; set;}
    }
}
