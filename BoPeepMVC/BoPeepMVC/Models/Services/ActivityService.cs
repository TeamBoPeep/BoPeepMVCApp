using BoPeepMVC.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

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
        public async Task<IEnumerable<Activity>> GetActivitiesByKeyword(string keyword)
        {
            string route = "activities";

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var streamTask = await client.GetStreamAsync($"{baseURL}/{route}");
            var allactivities = await JsonSerializer.DeserializeAsync<List<Activity>>(streamTask);

            // Filters by keyword in the description or the title, case-insensitive, and then sorts by highest to lowest rating
            var response = allactivities.Where(a => a.Title.ToLower()
                .Contains(keyword.ToLower()) || a.Description.ToLower().Contains(keyword.ToLower()))
                .OrderByDescending(a=> a.Rating);

            return response;
        }
    }
}
