﻿using System;
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
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
/*
        [JsonPropertyName("rate")]
        public int Rate { get; set; }*/

        //aggregate rating
        [JsonPropertyName("rating")]
        public double Rating { get; set; }

        //List of reviews
        [JsonPropertyName("reviews")]
        public List<Review> Reviews { get; set; }

        //whether inside or outside
        [JsonPropertyName("location")]
        public string Location { get; set; }

        //Related tags and categories
        [JsonPropertyName("tagDTO")]
        public List<Tag> Tags { get; set; }

        //link to external article/source/purchase page
        [JsonPropertyName("externalLink")]
        public string ExternalLink { get; set; }

        //url for header/icon image
        [JsonPropertyName("imageUrl")]
        public string ImageURL { get; set; }
    }
}
