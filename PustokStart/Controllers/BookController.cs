
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PustokStart.DAL;
using PustokStart.Models;
using PustokStart.ViewModels;

namespace PustokStart.Controllers
{
    public class BookController:Controller
    {
        public readonly PustokContext _context;
        public BookController(PustokContext context)
        {
            _context= context;
        }
		public IActionResult Detail(int id )
		{
			Book book = _context.Books
				.Include(x => x.BookImages)
				.Include(x => x.Author)
				.Include(x => x.Genre).Include(x=>x.BookComments).ThenInclude(x=>x.AppUser).Include(x => x.Tags).ThenInclude(t => t.Tag).FirstOrDefault(x=>x.Id == id);
			if (book == null) return View("Error");
			BookDetailViewModel vm = new BookDetailViewModel
			{
				Book = book,
				RelatedBook = _context.Books.Include(x=>x.BookImages).Where(x => x.GenreId == book.GenreId).ToList(),
				Comment=new BookComment { BookId=id}
			};
			return View(vm);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Comment(BookComment comment)
		{
			if (!User.Identity.IsAuthenticated && !User.IsInRole("Member"))
			{
				return RedirectToAction("login", "account", new { returUrl = Url.Action("detail", "book", new { id = comment.BookId }) });
			}
			

			if (!ModelState.IsValid)
			{
				Book book = _context.Books
				.Include(x => x.BookImages)
				.Include(x => x.Author)
				.Include(x => x.Genre).Include(x => x.BookComments).ThenInclude(x => x.AppUser).Include(x => x.Tags).ThenInclude(t => t.Tag).FirstOrDefault(x => x.Id == comment.BookId);
				if (book == null) return View("Error");
				BookDetailViewModel vm = new BookDetailViewModel
				{
					Book = book,
					RelatedBook = _context.Books.Include(x => x.BookImages).Where(x => x.GenreId == book.GenreId).ToList(),
					Comment = new BookComment { BookId = comment.BookId }
				};
				vm.Comment = comment;
				return View("Detail",vm);
				
			}
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
             comment.AppUserId=userId;
			comment.CreatedAt = DateTime.UtcNow.AddHours(4);

			_context.BookComments.Add(comment);
			_context.SaveChanges();

			return RedirectToAction("detail",new { id = comment.BookId });

		}
        public IActionResult GetBookDetail(int id)
        {
            Book book =_context.Books.Include(x=>x.Author).Include(x=>x.BookImages).Include(x=>x.Tags).ThenInclude(x=>x.Tag).FirstOrDefault(x=>x.Id==id);
            if (book == null) StatusCode(404);

          return PartialView("_BookModalPartial",book);
        }
	
        public IActionResult AddToBasket(int id)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
            {
                string userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
                var basketItem = _context.BasketItems.FirstOrDefault(x=>x.BookId==id && x.AppUserId==userId);

                if (basketItem != null)
                {
                    basketItem.Count++;
                }
                else
                {
                    basketItem= new BasketItem() { AppUserId=userId,BookId=id,Count=1};
                    _context.BasketItems.Add(basketItem);


				}
				_context.SaveChanges();
				var basketItems = _context.BasketItems.Include(x=>x.Book).ThenInclude(x=>x.BookImages).Where(x => x.AppUserId ==userId).ToList();
				

				return PartialView("_BasketCartPartialView", GenerateBasketVM(basketItems));

			}
            else
            {
				List<BasketItemCookieViewModel> cookieItems = new List<BasketItemCookieViewModel>();
				BasketItemCookieViewModel cookieitem;
				var basketStr = Request.Cookies["Basket"];

				if (basketStr != null)
				{
					cookieItems = JsonConvert.DeserializeObject<List<BasketItemCookieViewModel>>(basketStr);
					cookieitem = cookieItems.FirstOrDefault(x => x.BookId == id);
					if (cookieitem != null)
					{
						cookieitem.Count++;
					}
					else
					{
						cookieitem = new BasketItemCookieViewModel { BookId = id, Count = 1 };
						cookieItems.Add(cookieitem);
						HttpContext.Response.Cookies.Append("Basket", JsonConvert.SerializeObject(cookieItems));

						//return Json(new
						//{
						//    length=cookieItems.Count,
						//});
					}
				}
				else
				{
					cookieitem = new BasketItemCookieViewModel { BookId = id, Count = 1 };
					cookieItems.Add(cookieitem);

				}


				HttpContext.Response.Cookies.Append("Basket", JsonConvert.SerializeObject(cookieItems));



                return PartialView("_BasketCartPartialView", GenerateBasketVM(cookieItems));
				//return RedirectToAction("index","home");
			}


		}
        public IActionResult ShowBasket()
        {
            var basket = new List<BasketItemCookieViewModel>();
            var basketStr = HttpContext.Request.Cookies["Basket"];
            if (basket!=null)
            {

                basket = JsonConvert.DeserializeObject<List<BasketItemCookieViewModel>>(basketStr);
            }

           
            return Json(new { basket });
        }
        private BasketViewModel GenerateBasketVM(List<BasketItemCookieViewModel> cookieItems)
        {
			BasketViewModel bv = new BasketViewModel();
			foreach (var ci in cookieItems)
			{
				BasketItemViewModel bi = new BasketItemViewModel
				{
					Count = (int)ci.Count,
					Book = _context.Books.Include(x => x.BookImages).FirstOrDefault(x => x.Id == ci.BookId),

				};
				bv.BasketItems.Add(bi);
				bv.TotalPrice += (bi.Book.DiscountPerctent > 0 ? (bi.Book.SalePrice * (100 - bi.Book.DiscountPerctent) / 100) : bi.Book.SalePrice) * bi.Count;
			}
            return bv;

		}
        private BasketViewModel GenerateBasketVM(List<BasketItem>basketItems )
        {
			BasketViewModel bv = new BasketViewModel();
			foreach (var item in basketItems)
			{
				BasketItemViewModel bi = new BasketItemViewModel
				{
					Count = (int)item.Count,
					Book = item.Book,

				};
				bv.BasketItems.Add(bi);
				bv.TotalPrice += (bi.Book.DiscountPerctent > 0 ? (bi.Book.SalePrice * (100 - bi.Book.DiscountPerctent) / 100) : bi.Book.SalePrice) * bi.Count;
			}
            return bv;
		}

        public IActionResult RemoveBasket(int id)
        {
			if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
            {
				string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				var basketItem=_context.BasketItems.Include(x=>x.Book).ThenInclude(x=>x.BookImages).FirstOrDefault(x=>x.BookId==id && x.AppUserId==userId);
				if(basketItem != null)
				{
					if (basketItem.Count == 1)
					{
						_context.BasketItems.Remove(basketItem);
					}
					else
					{
						basketItem.Count--;
					}
				}
				_context.SaveChanges();
				var basketItems = _context.BasketItems.Include(x => x.Book).ThenInclude(x => x.BookImages).Where(x => x.AppUserId == userId).ToList();

				return PartialView("_BasketCartPartialView", GenerateBasketVM(basketItems));


			}
			else
            {
				List<BasketItemCookieViewModel> cookieItems = new List<BasketItemCookieViewModel>();
				var basketStr = HttpContext.Request.Cookies["basket"];
				if (basketStr != null)
				{
					cookieItems = JsonConvert.DeserializeObject<List<BasketItemCookieViewModel>>(basketStr);
					var item = cookieItems.FirstOrDefault(x => x.BookId == id);
					BasketViewModel bv = new BasketViewModel();
					if (item != null)
					{
						if (item.Count == 1)
						{
							cookieItems.Remove(item);
						}
						else
						{
							item.Count--;
						}

						Response.Cookies.Append("basket", JsonConvert.SerializeObject(cookieItems));

						foreach (var ci in cookieItems)
						{
							BasketItemViewModel bi = new BasketItemViewModel
							{
								Count = (int)ci.Count,
								Book = _context.Books.Include(x => x.BookImages).FirstOrDefault(x => x.Id == ci.BookId),

							};
							bv.BasketItems.Add(bi);
							bv.TotalPrice += (bi.Book.DiscountPerctent > 0 ? (bi.Book.SalePrice * (100 - bi.Book.DiscountPerctent) / 100) : bi.Book.SalePrice) * bi.Count;
						}
					}
					return PartialView("_BasketCartPartialView", bv);


				}
				else
				{
					return NotFound();
				}
			}
           



        }


    }
}
