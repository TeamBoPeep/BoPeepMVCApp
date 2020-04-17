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

        /// <summary>
        /// Constructor for HomeController
        /// </summary>
        /// <param name="activity">Activity interface dependency</param>
        /// <param name="tag">Tag interface dependency</param>
        /// <param name="review">Review interface dependency</param>
        public HomeController(IActivityManager activity, ITagManager tag, IReviewManager review)
        {
            _activity = activity;
            _tag = tag;
            _review = review;
        }

        /// <summary>
        /// Displays home page with list of existing tags
        /// </summary>
        /// <returns>The Home/Index View with current tags</returns>
        [HttpGet]
        [Route("/", Name = "Home")]
        public async Task<IActionResult> Index()
        {
            var tags = await _tag.GetTags();
            return View("Index", tags);
        }

        /// <summary>
        /// Redirects to a search based on keyword and selected tags
        /// </summary>
        /// <param name="keyword">Phrase to search</param>
        /// <param name="tags">Tags to filter by</param>
        /// <returns>Redirect to results route</returns>
        [HttpPost]
        public IActionResult Index(string keyword, string[] tags)
        {
            return RedirectToAction("Results", new { keyword, tags });
        }

        /// <summary>
        /// Performs a search based on keyword and selected tags
        /// </summary>
        /// <param name="keyword">Phrase to search</param>
        /// <param name="tags">Tags to filter by</param>
        /// <returns>Displays results view with results</returns>
        public async Task<IActionResult> Results(string keyword, string[] tags)
        {
            var response = await _activity.GetActivitiesByKeyword(keyword, tags);
            return View("Results", response);
        }

        /// <summary>
        /// Displays new activity page
        /// </summary>
        /// <returns>New activity view</returns>
        [HttpGet]
        [Route("/New", Name = "New")]
        public async Task<IActionResult> New()
        {
            var tags = await _tag.GetTags();
            return View(tags);
        }

        /// <summary>
        /// Creates a new activity from given values
        /// </summary>
        /// <param name="title">Title of the activity</param>
        /// <param name="description">Description of the activity</param>
        /// <param name="location">Whether the location is indoor or outdoor</param>
        /// <param name="tagNames">Tags related to the activity</param>
        /// <param name="externallink">Link to more info about the activity</param>
        /// <param name="imageurl">URL for the activity's header image</param>
        /// <returns>Results view with activity title searched</returns>
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


        /// <summary>
        /// Displays the page for an individual activity
        /// </summary>
        /// <param name="id">ID of the activity to be displayed</param>
        /// <returns>The view for the given activity</returns>
        [HttpGet]
        public async Task<IActionResult> Activity(int id)
        {
            var activity = await _activity.GetActivitiesByID(id);
            return View(activity);
        }

        /// <summary>
        /// Redirects to the review page for an activity
        /// </summary>
        /// <param name="id">ID of the activity to be reviewed</param>
        /// <returns>Redirect to review view</returns>
        [HttpPost]
        public async Task<IActionResult> Results(int id)
        {
            var activity = await _activity.GetActivitiesByID(id);
            return RedirectToAction("Review", activity);
        }

        /// <summary>
        /// Displays review page for an individual activity
        /// </summary>
        /// <param name="activity">The activity to be reviewed</param>
        /// <returns>Review view for activity</returns>
        [HttpGet]
        [Route("/review", Name ="Review")]
        public IActionResult Review(Activity activity)
        {
            return View(activity);
        }

        /// <summary>
        /// Creates a new review for an activity from given values
        /// </summary>
        /// <param name="activityId">ID of the activity</param>
        /// <param name="username">Name of user writing review</param>
        /// <param name="review">Text content of review</param>
        /// <param name="rate">The user's upvote or downvote on the activity</param>
        /// <returns>Activity view for reviewed activity</returns>
        [HttpPost]
        [Route("/review", Name = "Review")]
        public async Task<IActionResult> Review(int activityId, string username, string review, int rate)
        {
            Review newReview = new Review
            {
                ActivityID = activityId,
                Name = username,
                Description = review,
                Rate = rate
            };
            await _review.CreateReview(newReview);
            var reviewedActivity = await _activity.GetActivitiesByID(activityId);

            return View("Activity", reviewedActivity);
        }

        [HttpPost, ActionName("DeletePost")]
        public async Task<IActionResult> ConfirmDelete(int id, int activityID)
        {
            await _review.DeleteReviews(id);

            return RedirectToAction("");
        }

        /// <summary>
        /// Displays about us page
        /// </summary>
        /// <returns>About us view</returns>
        [HttpGet]
        [Route("/AboutUs", Name = "AboutUs")]
        public IActionResult AboutUs()
        {
            return View();
        }
    }
}
