using Newtonsoft.Json;

namespace Peloton
{
    public class Instructor
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
