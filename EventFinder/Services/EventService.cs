using EventFinder.Models;
using System.Linq;
using System.Text.Json;

namespace EventFinder.Business
{
    public class EventService
    {

        public EventService()
        {
            
        }
    
        public static void AddToEmail(Customer c, Event e, int? price = null)

        {
            var distance = GetDistance(c.City, e.City);
            Console.Out.WriteLine($"{c.Name}: {e.Name} in {e.City}"
            + (distance > 0 ? $" ({distance} miles away)" : "")
            + (price.HasValue ? $" for ${price}" : ""));
        }
        public static int GetPrice(Event e)
        {
            return (AlphebiticalDistance(e.City, "") + AlphebiticalDistance(e.Name, "")) / 10;
        }
        public static int GetDistance(string fromCity, string toCity)
        {
            return AlphebiticalDistance(fromCity, toCity);
        }
         
        //TASK 3 / TASK 4
        
        public static Task<HttpResponseMessage> GetDistanceApi(string fromCity, string toCity)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                "URL_HERE");

            var client = new HttpClient();
            try
            {
                return client.SendAsync(request);
            }
            catch
            {
                return null!;
            }

        }

        //TASK 5
        public static IEnumerable<KeyValuePair<Event,int>> SortEvents(Dictionary<Event, int> events, Func<KeyValuePair<Event, int>,int> func,  int take)
        {            
            return events.OrderBy(func).Take(take);
        }
        
        private static int AlphebiticalDistance(string s, string t)
        {
            var result = 0;
            var i = 0;
            for (i = 0; i < Math.Min(s.Length, t.Length); i++)
            {
                // Console.Out.WriteLine($"loop 1 i={i} {s.Length} {t.Length}");
                result += Math.Abs(s[i] - t[i]);
            }
            for (; i < Math.Max(s.Length, t.Length); i++)
            {
                // Console.Out.WriteLine($"loop 2 i={i} {s.Length} {t.Length}");
                result += s.Length > t.Length ? s[i] : t[i];
            }
            return result;
        }
    }
}
