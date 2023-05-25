using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Build.Framework;


namespace PustokStart.Models
{
    public class BookTags
    {
        public int Id { get; set; } 
        public int BookId {get; set;}
        public int TagId {get; set;}
        public Book Book { get; set;}
        public Tag Tag { get; set;}
    }
}
