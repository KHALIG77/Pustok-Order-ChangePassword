using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PustokStart.Attiributes.ValidationAttiributes;

namespace PustokStart.Models
{
    public class Book
    {
        public int Id { get; set; }
        public int GenreId {get; set; }
        public int AuthorId {get; set; }
        [Required]
        [MaxLength(20)]
        public string Name {get; set;}
       
        [MaxLength(500)]
        public string Desc {get; set;}
        [Column(TypeName ="money")]

        public decimal SalePrice { get; set;}
        [Column(TypeName = "money")]
        public decimal CostPrice {get; set;}
        [Column(TypeName = "money")]
        public decimal DiscountPerctent {get; set;}
        [Required]
        public bool StockStatus {get; set;}
        [Required]

        public bool IsFeatured {get; set;}
        public bool IsNew {get; set;}
        public Genre Genre { get; set;}
        public Author Author { get; set;} 
        public List<BookImage> BookImages { get; set;} = new List<BookImage>();
        public List<BookTags> Tags { get; set;}= new List<BookTags>();
        [NotMapped]
        [AllowsFileType("image/jpeg", "image/png")]
        [MaxFileSize(10000000)]
        public IFormFile PosterImage { get; set;}
        [NotMapped]
        [AllowsFileType("image/jpeg", "image/png")]
        [MaxFileSize(10000000)]
        public IFormFile HoverPosterImage { get; set; }
        [NotMapped]
        [AllowsFileType("image/jpeg", "image/png")]
        [MaxFileSize(10000000)]
        public List<IFormFile> Images { get; set; }=new List<IFormFile>();
        [NotMapped]
        public List<int> TagIds { get; set;}=new List<int>();
        [NotMapped]
        public List<int> BookImageIds { get; set; } = new List<int>();
        public List<BookComment> BookComments { get; set; }

    }
}
