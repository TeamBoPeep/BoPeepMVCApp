using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BoPeepMVC.Models.Interfaces
{
    public interface IReviewManager
    {
        public Task<IEnumerable<Review>> GetReviews();

        public Task<HttpResponseMessage> CreateReview(Review review);
    }
}
