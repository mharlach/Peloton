using System;
using Newtonsoft.Json;

namespace Peloton
{
    public class Ride
    {
        [JsonProperty("id")]
        public string Id{get;set;}

        [JsonProperty("duration")]
        public long Duration { get; set; }

        [JsonProperty("fitness_discipline")]
        public string ClassType { get; set; }

        [JsonProperty("instructor_id")]
        public string InstructorId { get; set; }

        [JsonProperty("scheduled_start_time")]
        public long StartTimeUnix { get; set; }

        public DateTime ClassDate
        {
            get
            {
                var epoch = new DateTime(1970,1,1, 0, 0, 0, DateTimeKind.Utc);
                return epoch.AddSeconds(StartTimeUnix);
            }
        }
    }
}
