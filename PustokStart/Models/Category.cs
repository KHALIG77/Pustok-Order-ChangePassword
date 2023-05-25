using System.ComponentModel.DataAnnotations;

namespace PustokStart.Models
{
    public class Category
    {
        
       public int Id { get; set; }
        [MaxLength(100)] 
        public string Name { get; set; }    
        public List<SubCategory> SubCategories { get; set; }
    }
}
