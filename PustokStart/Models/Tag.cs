using System.ComponentModel.DataAnnotations;

namespace PustokStart.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        public List<BookTags> Books { get; set; }
    }
}
