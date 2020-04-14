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
        private readonly IActivityManager _activity;
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
        public IActionResult Index(string keyword, string[] tags)
        {
            return RedirectToAction("Results", new { keyword, tags });
        }

        public async Task<IActionResult> Results(string keyword, string[] tags)
        {
            var response = await _activity.GetActivitiesByKeyword(keyword);
            return View("Results", response);
        }
        
    }
}
