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
        /// Get all activities from API filtered by keyword and tags
        /// </summary>
        /// <param name="keyword">Keyword searched</param>
        /// <param name="tags">Tags to filter by</param>
        /// <returns>A list of all matching activities</returns>
        public async Task<IEnumerable<Activity>> GetActivitiesByKeyword(string keyword, string[] tags)
        {
            string route = "activities";

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var streamTask = await client.GetStreamAsync($"{baseURL}/{route}");
            var allactivities = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Activity>>(streamTask);

            // Filters by tags
            List<Activity> taggedActivities = new List<Activity>();
            foreach (var activity in allactivities)
            {
                Activity a = await GetActivitiesByID(activity.ID);
                if (a.Tags.Any(x => tags.Any(t => t == x.Name)))
                    taggedActivities.Add(a);
            }
          
            // Filters by keyword in the description or the title, case-insensitive, and then sorts by highest to lowest rating
            var response = taggedActivities.Where(a => a.Title.ToLower()
                .Contains(keyword.ToLower()) || a.Description.ToLower().Contains(keyword.ToLower()))
                .OrderByDescending(a => a.Rating);

            return response;
        }

        /// <summary>
        /// Gets a single activity by its ID
        /// </summary>
        /// <param name="id">The ID of the activity</param>
        /// <returns>The activity of the ID</returns>
        public async Task<Activity> GetActivitiesByID(int id)
        {
            string route = $"activities/{id}";

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var streamTask = await client.GetStreamAsync($"{baseURL}/{route}");
            var activity = await System.Text.Json.JsonSerializer.DeserializeAsync<Activity>(streamTask);

            return activity;
        }

        /// <summary>
        /// Sends an activity to the API to be created
        /// </summary>
        /// <param name="activity">The activity to be sent</param>
        /// <returns>The status response from the API</returns>
        public async Task<HttpResponseMessage> CreateActivity(Activity activity)
        {
            string route = "activities";

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var serializedActivity = JsonConvert.SerializeObject(activity);
            var response = await client.PostAsync($"{baseURL}/{route}", new StringContent(serializedActivity, Encoding.UTF8, "application/json"));
            return response;
        }

        /// <summary>
        /// Sends an activity to be updated by the API
        /// </summary>
        /// <param name="activity">The activity to be sent</param>
        /// <returns>The status response from the API</returns>
        public async Task<HttpResponseMessage> UpdateActivity(Activity activity)
        {
            string route = $"activities/{activity.ID}";

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var serializedActivity = JsonConvert.SerializeObject(activity);
            var response = await client.PutAsync($"{baseURL}/{route}", new StringContent(serializedActivity, Encoding.UTF8, "application/json"));
            return response;
        }
    }
}
