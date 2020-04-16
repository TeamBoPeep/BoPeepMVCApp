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
        private readonly IReviewManager _review;

        public HomeController(IActivityManager activity, ITagManager tag, IReviewManager review)
        {
            _activity = activity;
            _tag = tag;
            _review = review;
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
            IEnumerable<Tag> tagtag = await _tag.GetTags();
            IEnumerable<Tag> matchingtags = tagtag.Where(t => tagNames.Any(n => n == t.Name));
            foreach (Tag tag in matchingtags)
            {
                tags.Add(tag);
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


        [HttpGet]
        public async Task<IActionResult> Activity(int id)
        {
            var activity = await _activity.GetActivitiesByID(id);
            return View(activity);
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
        public async Task<IActionResult> Review(int activityId, string username, string review)
        {
            Review newReview = new Review
            {
                ActivityID = activityId,
                Name = username,
                Description = review
            };
            await _review.CreateReview(newReview);
            var reviewedActivity = await _activity.GetActivitiesByID(activityId);

            return View("Activity", reviewedActivity);
        }

        [HttpGet]
        [Route("/AboutUs", Name = "AboutUs")]
        public IActionResult AboutUs()
        {
            return View();
        }
    }
}
