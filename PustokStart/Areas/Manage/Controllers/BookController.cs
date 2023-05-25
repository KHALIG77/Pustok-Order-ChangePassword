using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokStart.DAL;
using PustokStart.Helper.FileManager;
using PustokStart.Models;
using PustokStart.ViewModels;

namespace PustokStart.Areas.Manage.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]

    [Area("manage")]
    public class BookController : Controller
    {
        private readonly PustokContext _context;
        private readonly IWebHostEnvironment _env;

        public BookController(PustokContext context,IWebHostEnvironment env) 
        {
            _context = context;
            _env = env;
        }
        
        public IActionResult Index(int page=1,string search=null)
        {
            var query =
                _context.Books.
                Include(x => x.Author).Include(x=>x.BookImages.Where(x=>x.PosterStatus==true)).
                Include(x => x.Genre).AsQueryable();
          if(search != null)
            {
                query=query.Where(x=>x.Name.Contains(search));  
            }

          
            ViewBag.Search = search;

               
            return View(PaginatedList<Book>.Create(query,page,3));
        }
        public IActionResult Create()
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres=_context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book)
        {
            if (!_context.Authors.Any(x => x.Id == book.AuthorId))
            {
                ModelState.AddModelError("AuthorId", "Author is not found");
                return View();
            }
            if (!_context.Genres.Any(x => x.Id == book.GenreId))
            {
                ModelState.AddModelError("GenreId", "Genre is not found");
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (book.PosterImage == null)
            {
                ModelState.AddModelError("PosterImage", "Posterimage is required");
            }
            if (book.HoverPosterImage == null)
            {
                ModelState.AddModelError("HoverPosterImage", "Posterimage is required");
            }


            BookImage poster = new BookImage
            {
                ImageName = FileManager.Save(_env.WebRootPath, "uploads/books", book.PosterImage),
                 PosterStatus = true,
                 Book=book,
            };
            book.BookImages.Add(poster);
            BookImage hoverPoster = new BookImage
            {
                ImageName = FileManager.Save(_env.WebRootPath, "uploads/books", book.HoverPosterImage),
                PosterStatus = false,
                Book = book,
            };
           
                foreach (var img in book.Images)
                {
                    BookImage bookImages = new BookImage
                    {
                        ImageName = FileManager.Save(_env.WebRootPath, "uploads/books", img)
                    };
                    book.BookImages.Add(bookImages);
                }
                foreach (var tagId in book.TagIds ) 
            { 
                BookTags bookTag=new BookTags()
                {
                    TagId= tagId,   
                };
               book.Tags.Add(bookTag);
            }
         
        
     
            book.BookImages.Add(hoverPoster);
            _context.Books.Add(book);
            _context.SaveChanges(); 
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            Book book =_context.Books.Include(x=>x.BookImages).Include(x=>x.Tags).FirstOrDefault(x=>x.Id == id);

            book.TagIds=book.Tags.Select(x=>x.TagId).ToList();
            return View(book);
        }
        [HttpPost]
        public IActionResult Edit(Book book)
        {
            Book existBook = _context.Books.Include(x=>x.Tags).Include(x=>x.BookImages).FirstOrDefault(x => x.Id == book.Id);
            if (existBook == null) return View("Error");
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (book.AuthorId!=existBook.AuthorId && !_context.Authors.Any(x => x.Id == book.AuthorId))
            {
                ModelState.AddModelError("AuthorId", "Author is not found");
                return View();
            }
            if (book.GenreId!=existBook.GenreId && !_context.Genres.Any(x => x.Id == book.GenreId))
            {
                ModelState.AddModelError("GenreId", "Genre is not found");
                return View();
            }
            string oldPoster=null;
            if (book.PosterImage != null)
            {
                BookImage poster = existBook.BookImages.FirstOrDefault(x => x.PosterStatus == true);
                oldPoster = poster?.ImageName;
                if (poster == null)
                {
                    poster = new BookImage()
                    {
                        PosterStatus= true,
                    };
                    poster.ImageName = FileManager.Save(_env.WebRootPath, "uploads/books", book.PosterImage);
                    existBook.BookImages.Add(poster);

                }
                else
                {
                    poster.ImageName = FileManager.Save(_env.WebRootPath, "uploads/books", book.PosterImage);

                }

            }
           

            string oldHoverPoster = null;
            if (book.HoverPosterImage != null)
            {
                BookImage hoverPoster = existBook.BookImages.FirstOrDefault(x => x.PosterStatus == false);
                oldHoverPoster = hoverPoster?.ImageName;
                if (hoverPoster == null)
                {
                    hoverPoster = new BookImage()
                    {
                        PosterStatus=false
                    };
                    hoverPoster.ImageName = FileManager.Save(_env.WebRootPath, "uploads/books", book.HoverPosterImage);
                    existBook.BookImages.Add(hoverPoster);

                }
                else
                {
                    hoverPoster.ImageName = FileManager.Save(_env.WebRootPath, "uploads/books", book.HoverPosterImage);

                }

            }
          existBook.Tags.RemoveAll(x=>!book.TagIds.Contains(x.Id));

            var mewTagId = book.TagIds.Where(x => !existBook.Tags.Any(bt => bt.TagId == x));
            foreach (var tagId in mewTagId)
            {
                BookTags bookTags = new BookTags()
                {
                    TagId=tagId,
                    
                };
                existBook.Tags.Add(bookTags);
            }
          var removedImages =  existBook.BookImages.FindAll(x=> x.PosterStatus == null && !book.BookImageIds.Contains(x.Id));
          existBook.BookImages.RemoveAll(x =>x.PosterStatus==null && !book.BookImageIds.Contains(x.Id));

            foreach (var item in book.Images)
            {
                BookImage bookImages = new BookImage
                {
                    ImageName = FileManager.Save(_env.WebRootPath, "uploads/books", item)
                };
                existBook.BookImages.Add(bookImages);
            }

            existBook.Name = book.Name;
            existBook.SalePrice=book.SalePrice;
            existBook.CostPrice=book.CostPrice;
            existBook.Desc=book.Desc;   
            existBook.IsFeatured=book.IsFeatured;
            existBook.IsNew=book.IsNew;
            existBook.StockStatus = book.StockStatus;
            existBook.DiscountPerctent=book.DiscountPerctent;   
            existBook.AuthorId=book.AuthorId;
            existBook.GenreId = book.GenreId;

            _context.SaveChanges();

            if(oldPoster!= null) FileManager.Delete(_env.WebRootPath, "uploads/books", oldPoster);
            if (oldHoverPoster != null) FileManager.Delete(_env.WebRootPath, "uploads/books", oldHoverPoster);
            if(removedImages.Any())
            FileManager.DeleteAll(_env.WebRootPath, "uploads/books",removedImages.Select(x=>x.ImageName).ToList());





           return RedirectToAction("Index");
        }
    }
}
