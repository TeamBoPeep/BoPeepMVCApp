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
        [Route("/", Name = "Home")]
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
            var response = await _activity.GetActivitiesByKeyword(keyword, tags);
            return View("Results", response);
        }

        [HttpGet]
        [Route("/New", Name = "New")]
        public async Task<IActionResult> New()
        {
            var tags = await _tag.GetTags();
            return View(tags);
        }

        [HttpPost]
        [Route("/New", Name = "New")]
        public async Task<IActionResult> New(string title, string description, string location, List<string> tagNames, string externallink, string imageurl)
        {
            List<Tag> tags = new List<Tag>();
            foreach(string name in tagNames)
            {
                tags.Add(new Tag
                {
                    Name = name
                });
            }

            Activity newActivity = new Activity()
            {
                Title = title,
                Description = description,
                Location = location,
                Tags = tags,
                ExternalLink = externallink,
                ImageURL = imageurl
            };

            await _activity.CreateActivity(newActivity);

            return View("Results", new List<Activity> { newActivity });
        }

        [HttpPost]
        public async Task<IActionResult> Results(int id)
        {
            var activity = await _activity.GetActivitiesByID(id);
            return RedirectToAction("Review", activity);
        }


        [HttpGet]
        [Route("/review", Name ="Review")]
        public IActionResult Review(Activity activity)
        {
            return View(activity);
        }

        [HttpPost]
        [Route("/review", Name = "Review")]
        public async Task<IActionResult> Review(int id, string username, string review)
        {
            Review newReview = new Review
            {
                Name = username,
                Description = review
            };
            var reviewedActivity = await _activity.GetActivitiesByID(id);
            reviewedActivity.Reviews.Add(newReview);

            await _activity.UpdateActivity(reviewedActivity);

            return View("Results", new List<Activity> { reviewedActivity });
        }

        [HttpGet]
        [Route("/AboutUs", Name = "AboutUs")]
        public IActionResult AboutUs()
        {
            return View();
        }
    }
}
