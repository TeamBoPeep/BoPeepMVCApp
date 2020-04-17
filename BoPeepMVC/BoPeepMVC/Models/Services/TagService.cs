using BoPeepMVC.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BoPeepMVC.Models.Services
{
    public class TagService : ITagManager
    {
        private static readonly HttpClient client = new HttpClient();
        public string baseURL = @"https://bobeepapi.azurewebsites.net/api";

        /// <summary>
        /// Get all the tags from the API
        /// </summary>
        /// <returns>An enumerable of all tags</returns>
        public async Task<IEnumerable<Tag>> GetTags()
        {
            string route = "tags";

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var streamTask = await client.GetStreamAsync($"{baseURL}/{route}");
            var allTags = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Tag>>(streamTask);

            return allTags;
        }
    }
}
