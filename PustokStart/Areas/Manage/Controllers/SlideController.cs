using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using PustokStart.DAL;
using PustokStart.Helper.FileManager;
using PustokStart.Migrations;
using PustokStart.Models;
using PustokStart.ViewModels;

namespace PustokStart.Areas.Manage.Controllers
{
	[Authorize(Roles = "SuperAdmin , Admin")]

	[Area("manage")]
    public class SlideController : Controller
    {
        private readonly PustokContext _context;
        private readonly IWebHostEnvironment _env;

        public SlideController(PustokContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page=1)
        {

            var query =
                _context.Slides.OrderBy(x=>x.Order).AsQueryable();
            return View(PaginatedList<Slide>.Create(query, page, 2));

        }

        public IActionResult Create()
        {
            var slide = new Slide();
            slide.Order= _context.Slides.Any()?_context.Slides.Max(x=>x.Order)+1:1;
            return View(slide);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Slide slide) 
        { 
            
            if(slide.ImageFile == null)
            {
               
                ModelState.AddModelError("ImageFile", "Image is required");
            }
            if (!ModelState.IsValid)
            {
              
                return View();
            }
            if (slide.Order >= _context.Slides.Max(x => x.Order) + 1)
            {
              

                ModelState.AddModelError("Order", "Please write what is offered");
                return View();
            }
            
        
            foreach (var item in _context.Slides.Where(x=>x.Order>=slide.Order))
            {
                item.Order++;
            }
            //string path =_env.WebRootPath+"\\uploads\\sliders\\"+slide.ImageFile.FileName;
            //string path = Path.Combine(_env.WebRootPath, "uploads/sliders", slide.ImageFile.FileName);



            slide.ImageName = FileManager.Save(_env.WebRootPath, "uploads/sliders", slide.ImageFile);
            _context.Slides.Add(slide);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            Slide slide = _context.Slides.Find(id);
            if(slide == null)
            {
                return View("Error");   
            }
            return View(slide);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Slide slide)
        {
            if (!ModelState.IsValid)
            {
                return View(slide);
            }
           
            Slide existslide = _context.Slides.FirstOrDefault(x=>x.Id==slide.Id);
            if (existslide == null)
            {
                return View("Error");
            }
           
            existslide.Title1= slide.Title1;
            existslide.Title2= slide.Title2;
            existslide.Order=slide.Order;
            existslide.BtnText = slide.BtnText;
            existslide.BtnUrl = slide.BtnUrl;
            existslide.Desc=slide.Desc;

            string oldFileName = null;
            if (slide.ImageFile != null)
            {
                oldFileName= existslide.ImageName;
                existslide.ImageName= FileManager.Save(_env.WebRootPath, "uploads/sliders", slide.ImageFile);
            }
          

          if(oldFileName!= null)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/sliders",oldFileName);
            }

            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
