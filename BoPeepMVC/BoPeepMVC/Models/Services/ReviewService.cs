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

        /// <summary>
        /// Sends a review to the API to be created
        /// </summary>
        /// <param name="review">The review to be sent</param>
        /// <returns>The status response from the API</returns>
        public async Task<HttpResponseMessage> CreateReview(Review review)
        {
                string route = "reviews";

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var serializedActivity = JsonConvert.SerializeObject(review);
                var response = await client.PostAsync($"{baseURL}/{route}", new StringContent(serializedActivity, Encoding.UTF8, "application/json"));
                return response;
        }

        /// <summary>
        /// Gets all reviews from the API
        /// </summary>
        /// <returns>An enumerable of all reviews</returns>
        public async Task<IEnumerable<Review>> GetReviews()
        {
            string route = "reviews";

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var streamTask = await client.GetStreamAsync($"{baseURL}/{route}");
            var allReviews = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Review>>(streamTask);

            return allReviews;
        }

        public async Task DeleteReviews(int id)
        {
            string route = $"reviews/{id}";
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var streamTask = await client.DeleteAsync($"{baseURL}/{route}");

        }
    }
}
