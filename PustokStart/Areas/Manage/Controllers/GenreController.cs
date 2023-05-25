using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokStart.DAL;
using PustokStart.Models;
using PustokStart.ViewModels;

namespace PustokStart.Areas.Manage.Controllers
{
	[Authorize(Roles = "SuperAdmin , Admin")]

	[Area("manage")]
    public class GenreController : Controller
    {
       
        private readonly PustokContext _context;

        public GenreController(PustokContext context)
        {
           _context = context;
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Index(int page=1)
        {
            var query =
                  _context.Genres.Include(x=>x.Books).AsQueryable(); 
            return View(PaginatedList<Genre>.Create(query, page, 1));
        }
        [HttpPost]
        public IActionResult Create(Genre genre) 
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_context.Genres.Any(x=>x.Name==genre.Name))
            {
                ModelState.AddModelError("Name", "Name is already used");
                return View();
            }
                _context.Genres.Add(genre);
                _context.SaveChanges();
            
            return RedirectToAction("index");
        }
        public IActionResult Edit(int id)
        {
            Genre genre = _context.Genres.FirstOrDefault(x=>x.Id==id);
            if (genre == null)
            {
                return View("error");
            }
           
            return View(genre);
        }
        [HttpPost]
        public IActionResult Edit(Genre genre) 
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Genre exists = _context.Genres.Find(genre.Id);
            if (exists == null)
            {
                return View("error");
            }
            if (genre.Name!=exists.Name && _context.Genres.Any(x => x.Name == genre.Name))
            {
                ModelState.AddModelError("Name", "Name is already used");
                return View();
            }
           
        
            exists.Name = genre.Name;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        [HttpPost]
        public IActionResult Delete(Genre genre) 
        {
            Genre existGenre = _context.Genres.Find(genre.Id);
            if (existGenre == null)
            {
                return View("error");
            }
            _context.Genres.Remove(existGenre);     
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Genre genre =_context.Genres.Include(x=>x.Books).FirstOrDefault(x=>x.Id==id);
            if (genre == null)
            {
                return View("error");
            }


            return View(genre);
        }
    }
}
