using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoPeepMVC.Models;
using BoPeepMVC.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BoPeepMVC.Controllers
{
    public class HomeController : Controller
    {
        private IActivityManager _activity;
        public HomeController(IActivityManager activity)
        {
            _activity = activity;
        }

        /// <summary>
        /// Our default view. sends the index view to GET a year range for our Results view.
        /// </summary>
        /// <returns>Opens the Home/Index View</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string keyword)
        {
            string response = await _activity.GetHello();
            return RedirectToAction("Results", new { response, keyword });
        }

        public IActionResult Results(string response, string keyword)
        {
            Activity activity = new Activity
            {
                Keyword = keyword,
                ApiResponse = response
            };
            return View("Results", activity);
        }
        
    }
}
