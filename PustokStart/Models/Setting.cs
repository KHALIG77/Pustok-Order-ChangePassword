

using System.ComponentModel.DataAnnotations;

namespace PustokStart.Models
{
    public class Setting
    {
        
        [Required]
        [MaxLength(250)]

        public string Key {get; set;}
        public string Value { get; set;}
    }
}
