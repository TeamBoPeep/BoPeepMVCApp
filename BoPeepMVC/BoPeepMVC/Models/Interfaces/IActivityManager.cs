using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BoPeepMVC.Models.Interfaces
{
    public interface IActivityManager
    {
        public Task<IEnumerable<Activity>> GetActivitiesByKeyword(string keyword, string[] tags);

        public Task<Activity> GetActivitiesByID(int id);

        public Task<HttpResponseMessage> CreateActivity(Activity activity);

        public Task<HttpResponseMessage> UpdateActivity(Activity activity);
    }
}
