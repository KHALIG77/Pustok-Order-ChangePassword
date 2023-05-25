using System.ComponentModel.DataAnnotations;

namespace PustokStart.Models
{
    public class SubCategory
    {
        

        public int Id { get; set; }
       
        public int CategoryId {get; set; }
        [MaxLength(100)]
        public string Name { get; set; }    
        public Category Category { get; set; }  

    }
}
