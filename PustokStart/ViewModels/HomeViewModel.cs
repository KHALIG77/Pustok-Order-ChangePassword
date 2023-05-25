using PustokStart.Models;

namespace PustokStart.ViewModels
{
    public class HomeViewModel
    {
        public List<Book> FeaturedBooks { get; set; }
        public List<Book> NewBooks { get; set; }
        public List<Book> DiscountedBooks { get; set; }
        public List<Slide> Slides { get; set; }
        public List<Feature>Features { get; set; }


    }
}
