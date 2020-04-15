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
        private readonly ITagManager _tag;

        public HomeController(IActivityManager activity, ITagManager tag)
        {
            _activity = activity;
            _tag = tag;
        }

        /// <summary>
        /// Our default view. sends the index view to GET a year range for our Results view.
        /// </summary>
        /// <returns>Opens the Home/Index View</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tags = await _tag.GetTags();
            return View("Index", tags);
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

        [HttpGet]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public IActionResult New(string title, string description, string location, string externallink, string imageurl)
        {
            Activity newActivity = new Activity()
            {
                Title = title,
                Description = description,
                Location = location,
                ExternalLink = externallink,
                ImageURL = imageurl
            };

            _activity.CreateActivity(newActivity);

            return View("Results", new List<Activity> { newActivity });
        }
    }
}
