using System.ComponentModel.DataAnnotations;

namespace PustokStart.Models
{
    public class Feature
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string Name { get;set; }
        [Required]
        [MaxLength(60)]
        public string Desc { get; set; }

        [Required]
        [MaxLength(30)]
        public string Icon {get; set; }
    }
}
