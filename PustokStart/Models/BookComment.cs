using System.ComponentModel.DataAnnotations;

namespace PustokStart.Models
{
	public class BookComment
	{
		public int Id { get; set; }
		public string AppUserId { get; set; }
		[Required]
		[MaxLength(500)]
		public string Text {get; set; }
		[Range(1, 5)]
		public int Rate {get; set; }
		public DateTime CreatedAt { get; set; }

		public int BookId { get; set; }
		public AppUser AppUser { get; set; }	
		public Book Book { get; set; }
		 
	}
}
