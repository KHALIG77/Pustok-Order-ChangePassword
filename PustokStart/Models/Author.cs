using System.ComponentModel.DataAnnotations;
using PustokStart.Attiributes.ValidationAttiributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace PustokStart.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string FullName { get; set; }
        public string Image { get; set; } 
        [NotMapped]
        [AllowsFileType("image/jpeg", "image/png")]
        [MaxFileSize(10000000)]
        public IFormFile ImageFile {  get; set; }
        [NotMapped]
        public string ImageCheck {get; set; }
        public List<Book> Books { get; set; }
    }
}
