using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PustokStart.DAL;
using PustokStart.Models;
using PustokStart.ViewModels;

namespace PustokStart.Controllers
{
	public class OrderController:Controller
	{
		private readonly PustokContext _context;
		private readonly UserManager<AppUser> _userManager;

		public OrderController(PustokContext context,UserManager<AppUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}
       
	

		public  async Task<IActionResult> Checkout()
		{
			OrderViewModel vm = new OrderViewModel();
			vm.CheckoutItems=GetCheckoutItems();
			if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
			{
				AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);


				vm.Order = new OrderCreateViewModel
				{
					Address = user.Address,
					Email = user.Email,
					FullName = user.FullName,
					Phone=user.Phone,

				};
			}
			vm.TotalPrice = vm.TotalPrice = vm.CheckoutItems.Any() ? vm.CheckoutItems.Sum(x => x.Price * x.Count) : 0;
			return View(vm);
		}


		private List<CheckoutItem> GenerateCheckoutItemsFromDb(string userId)
		{
		 return	_context.BasketItems.Include(x => x.Book).Where(x => x.AppUserId == userId).Select(x => new CheckoutItem
			{
				Count = x.Count,
				Name = x.Book.Name,
				Price = x.Book.DiscountPerctent > 0 ? (x.Book.SalePrice * (100 - x.Book.DiscountPerctent) / 100) : x.Book.SalePrice

			}).ToList();
		}
		private List<CheckoutItem> GenerateCheckoutItemsFromCookie()
		{ 
			List<CheckoutItem> checkoutItems = new List<CheckoutItem>();	
			var basketStr = Request.Cookies["basket"];
			if (basketStr != null)
			{
				List<BasketItemCookieViewModel> cookieItems = JsonConvert.DeserializeObject<List<BasketItemCookieViewModel>>(basketStr);
				foreach (var item in cookieItems)
				{
					Book book = _context.Books.FirstOrDefault(x => x.Id == item.BookId);

					CheckoutItem checkoutItem = new CheckoutItem()
					{
						Count = (int)item.Count,
						Name = book.Name,
						Price = book.DiscountPerctent > 0 ? (book.SalePrice * (100 - book.DiscountPerctent) / 100) : book.SalePrice

					};
					checkoutItems.Add(checkoutItem);

				}
				
			}
			return checkoutItems;
		}


		public  List<CheckoutItem> GetCheckoutItems()
		{
			if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
			{
				string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			 return GenerateCheckoutItemsFromDb(userId);
			}
			else
			{
				return GenerateCheckoutItemsFromCookie();
			}
		}






			[HttpPost]
		[ValidateAntiForgeryToken]
		public  IActionResult Create(OrderCreateViewModel orderVM)
		{
			if (!ModelState.IsValid)
			{
				OrderViewModel vm =new OrderViewModel();
				vm.CheckoutItems=GetCheckoutItems();
				vm.Order=orderVM;
				return View("Checkout", vm);
			}
			return Json(new { });
			

		}
	}
	
}
