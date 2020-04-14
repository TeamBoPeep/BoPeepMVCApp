using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BoPeepMVC.Models
{
    public class Activity
    {
        //legacy code for testing routes
        //public string Keyword { get; set; }
        //public string ApiResponse { get; set; }

        //db id of activity
        //[JsonPropertyName("id")]
        //public int ID { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        //user's existing rating
        //will only be necessary once users are implemented in some way
        [JsonPropertyName("rate")]
        public int Rate { get; set; }

        //aggregate rating
        [JsonPropertyName("rating")]
        public double Rating { get; set; }

        //whether inside or outside
        [JsonPropertyName("location")]
        public string Location { get; set; }

        //link to external article/source/purchase page
        [JsonPropertyName("externallink")]
        public string ExternalLink { get; set; }

        //url for header/icon image
        [JsonPropertyName("imageurl")]
        public string ImageURL { get; set; }
    }
}
