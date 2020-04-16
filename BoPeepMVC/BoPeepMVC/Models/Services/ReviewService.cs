using BoPeepMVC.Models.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BoPeepMVC.Models.Services
{
    class ReviewService : IReviewManager
    {
        private static readonly HttpClient client = new HttpClient();
        public string baseURL = @"https://bobeepapi.azurewebsites.net/api";

        public async Task<HttpResponseMessage> CreateReview(Review review)
        {
                string route = "reviews";

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var serializedActivity = JsonConvert.SerializeObject(review);
                var response = await client.PostAsync($"{baseURL}/{route}", new StringContent(serializedActivity, Encoding.UTF8, "application/json"));
                return response;
        }

        public async Task<IEnumerable<Review>> GetReviews()
        {
            string route = "reviews";

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var streamTask = await client.GetStreamAsync($"{baseURL}/{route}");
            var allReviews = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Review>>(streamTask);

            return allReviews;
        }
    }
}
