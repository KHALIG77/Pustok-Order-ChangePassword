using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PustokStart.Areas.Manage.Controllers
{
    [Authorize(Roles ="SuperAdmin , Admin")]
    [Area("manage")]
    public class DashboardController:Controller
    {

        public IActionResult Index() 
        {
            return View();
        }

    }
}
