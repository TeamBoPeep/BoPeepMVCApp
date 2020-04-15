using BoPeepMVC.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

namespace BoPeepMVC.Models.Services
{
    public class ActivityService : IActivityManager
    {
        private static readonly HttpClient client = new HttpClient();
        public string baseURL = @"https://bobeepapi.azurewebsites.net/api";

        /// <summary>
        /// Returns "Hello World" from deployed API
        /// </summary>
        /// <returns>string of response</returns>
        public async Task<IEnumerable<Activity>> GetActivitiesByKeyword(string keyword, string[] tags)
        {
            string route = "activities";

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var streamTask = await client.GetStreamAsync($"{baseURL}/{route}");
            var allactivities = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Activity>>(streamTask);
            // IDEA FOR IMPLEMENTING TAGS

            /* List<Activity> taggedActivities = new List<Activity>()
             * foreach(var activity in allactivities) 
             * {
             *      if(activity.Tags.Any(x => tags.Any(t => t == x)))
             *          taggedActivities.push(activity);
             * }
             * 
             * var response = taggedActivities.Where(a => etc...
             * */

            // Filters by keyword in the description or the title, case-insensitive, and then sorts by highest to lowest rating
            var response = allactivities.Where(a => a.Title.ToLower()
                .Contains(keyword.ToLower()) || a.Description.ToLower().Contains(keyword.ToLower()))
                .OrderByDescending(a=> a.Rating);

            return response;
        }

        public async Task<HttpResponseMessage> CreateActivity(Activity activity)
        {
            string route = "activities";

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var serializedActivity = JsonConvert.SerializeObject(activity);
            var response = await client.PostAsync($"{baseURL}/{route}", new StringContent(serializedActivity, Encoding.UTF8, "application/json"));
            return response;
        }
    }
}
