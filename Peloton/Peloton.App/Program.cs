using System;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Peloton.App
{
    class Program
    {
        private static readonly string apiUri = "https://api.onepeloton.com/api";
        static async Task Main(string[] args)
        {

            var restClient = new RestClient(apiUri);
            int page = 0;
            var instructors = new Dictionary<string, Instructor>();
            var rides = new List<Ride>();
            var thisYear = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            do
            {
                var request = new RestRequest("v2/ride/archived")
                    .AddQueryParameter("page", page.ToString());

                var response = await restClient.ExecuteGetAsync(request);
                if (response.IsSuccessful)
                {
                    var content = JObject.Parse(response.Content);
                    var jClassData = content["data"] as JArray;
                    var responseRides = jClassData?.ToObject<List<Ride>>() ?? new List<Ride>();
                    Console.WriteLine($"[{response.StatusCode}] {responseRides.Count}");

                    if (responseRides.Any() == false)
                    {
                        break;
                    }
                    var jInstructors = content["instructors"] as JArray;
                    var responseInstructors = jInstructors?.ToObject<List<Instructor>>() ?? new List<Instructor>();

                    foreach (var i in responseInstructors)
                    {
                        if (instructors.ContainsKey(i.Id) == false)
                            instructors.Add(i.Id, i);
                    }

                    var anyThisYear = responseRides.Where(x => x.ClassDate >= thisYear).Any();
                    rides.AddRange(responseRides);

                    if (anyThisYear == false)
                        break;

                    page++;

                }
                else
                {
                    break;
                }

            } while (true);

            using var stream = new StreamWriter("peloton.csv");
            stream.WriteLine("Id,Time,Instructor,Duration,Type");
            foreach (var r in rides)
            {
                try
                {
                    stream.WriteLine($"{r.Id},{r.ClassDate},{instructors[r.InstructorId].Name},{r.Duration},{r.ClassType}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw;
                }
                
            }

        }
    }
}
