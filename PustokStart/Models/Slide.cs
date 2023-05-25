using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using PustokStart.Attiributes.ValidationAttiributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PustokStart.Models
{
    public class Slide
    {
        public int Id { get; set; }
        public int Order { get; set; }
        [MaxLength(20)]
        public string Title1 { get; set; }
        [MaxLength(20)]
        public string Title2 { get; set; }
        [MaxLength(250)]
        public string Desc { get; set; }
        [MaxLength(50)]
        public string BtnText { get; set; }
        [MaxLength(250)]
        public string BtnUrl { get; set; }
        [MaxLength(100)]
        public string ImageName { get; set; }
        [AllowsFileType("image/jpeg","image/png")]
        [MaxFileSize(10000000)]
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
