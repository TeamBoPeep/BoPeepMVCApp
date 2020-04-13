using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BoPeepMVC.Models
{
    public class Activity
    {
        //legacy code for testing routes
        //public string Keyword { get; set; }
        //public string ApiResponse { get; set; }

        //db id of activity
        public int ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        //user's existing rating
        //will only be necessary once users are implemented in some way
        public int Rate { get; set; }

        //aggregate rating
        public double Rating { get; set; }

        //whether inside or outside
        public string Location { get; set; }

        //link to external article/source/purchase page
        public string ExternalLink { get; set; }

        //url for header/icon image
        public string ImageURL { get; set; }
    }
}
