using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BoPeepMVC.Models
{
    public class Tag
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("names")]
        public string Name { get; set; }
        [JsonPropertyName("activitiesdtos")]
        public List<Activity> Activities { get; set; }
    }
}
