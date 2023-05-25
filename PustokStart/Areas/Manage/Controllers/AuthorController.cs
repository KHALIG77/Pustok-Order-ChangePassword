using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using PustokStart.DAL;
using PustokStart.Helper.FileManager;
using PustokStart.Models;
using PustokStart.ViewModels;

namespace PustokStart.Areas.Manage.Controllers
{
	[Authorize(Roles = "SuperAdmin , Admin")]

	[Area("manage")]
    public class AuthorController : Controller
    {
        private readonly PustokContext _context;
        private readonly IWebHostEnvironment _env;

        public AuthorController(PustokContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env=env;
           
        }
        public IActionResult Index(int page=1)
        {

            var query =
                  _context.Authors.Include(x => x.Books).AsQueryable();
            return View(PaginatedList<Author>.Create(query, page, 2));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Author author) 
        {
            if (!ModelState.IsValid)
            {
                return View();

            }
            if (author.ImageFile != null)
            {
                author.Image = FileManager.Save(_env.WebRootPath, "uploads/authors", author.ImageFile);
            }
            Author existAuthor = _context.Authors.Include(x=>x.Books).FirstOrDefault(x=>x.FullName==author.FullName);
            if (existAuthor != null)
            {
                ModelState.AddModelError("FullName", "Already used");
                return View(existAuthor);
            }
          
            _context.Authors.Add(author);
            _context.SaveChanges();
            return RedirectToAction("index");
            
        }
        public IActionResult Edit(int id)
        {
            Author author = _context.Authors.FirstOrDefault(x=>x.Id==id);
           
            return View(author);
        }
        [HttpPost]
        public IActionResult Edit(Author author)
        {
            Author existAuthor = _context.Authors.Find(author.Id);
            if(existAuthor == null)
            {
                return View("Error");
            }
            if (!ModelState.IsValid)
            {
                return View(existAuthor);
            }

            if (author.FullName!=existAuthor.FullName && _context.Authors.Any(x => x.FullName == author.FullName)){
                ModelState.AddModelError("FullName", "Already used");
                return View(existAuthor);
            }
            string oldAuthorImage = null;
            if (author.ImageCheck == null && author.ImageFile==null)
            {
                oldAuthorImage = existAuthor.Image;
                existAuthor.Image = null;

            }
            
            if (author.ImageFile != null)
            {
                oldAuthorImage = existAuthor.Image;
                existAuthor.Image = FileManager.Save(_env.WebRootPath, "uploads/authors", author.ImageFile);

            }
           
            existAuthor.FullName= author.FullName;
            _context.SaveChanges();
            if(oldAuthorImage != null) { FileManager.Delete(_env.WebRootPath,"uploads/authors",oldAuthorImage); }
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {

            Author existAuthor = _context.Authors.Find(id);
            string authorImage = existAuthor.Image;
            if(existAuthor==null)
            {
                return StatusCode(404);
            }
           
            _context.Authors.Remove(existAuthor);
            _context.SaveChanges();
            if (existAuthor.Image != null)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/authors", authorImage);
            }
            return StatusCode(200);
        }
      
    }
}
