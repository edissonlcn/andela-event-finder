
using EventFinder.Business;
using EventFinder.Models;
using System.Text.Json;

var events = new List<Event>{
new Event{ Name = "Phantom of the Opera", City = "New York"},
new Event{ Name = "Metallica", City = "Los Angeles"},
new Event{ Name = "Metallica", City = "New York"},
new Event{ Name = "Metallica", City = "Boston"},
new Event{ Name = "LadyGaGa", City = "New York"},
new Event{ Name = "LadyGaGa", City = "Boston"},
new Event{ Name = "LadyGaGa", City = "Chicago"},
new Event{ Name = "LadyGaGa", City = "San Francisco"},
new Event{ Name = "LadyGaGa", City = "Washington"}
};

var customer = new Customer { Name = "Mr. Fake", City = "New York" };

//TASK 1.
//Write a LINQ query to find the events that are in the same city as the customer
//and print the event name and city to the console.
//The output should be:
//Mr. Fake: Phantom of the Opera in New York
//Mr. Fake: Metallica in New York
//Mr. Fake: LadyGaGa in New York
Console.WriteLine("---------TASK 1---------");
var sameCityEvents = events.Where(e => e.City == customer.City);
foreach (var localEvent in sameCityEvents)
{
    EventService.AddToEmail(customer, localEvent);
}

//TASK 2.
//Write a code to find the closest events to the customer
//The output should be:
//Mr.Fake: Phantom of the Opera in New York
//Mr. Fake: Metallica in New York
//Mr. Fake: LadyGaGa in New York
//Mr. Fake: LadyGaGa in Chicago(221 miles away)
//Mr.Fake: LadyGaGa in Washington(347 miles away)
Console.WriteLine("---------TASK 2---------");
var closestEvents = new Dictionary<Event, int>();
foreach (var item in events)
{
    closestEvents.Add(item, EventService.GetDistance(customer.City, item.City));
}

//TASK 5
var closest5 = EventService.SortEvents(closestEvents, (x => x.Value), 5);

for (var i = 0; i < closest5.Count(); i++)
{
    EventService.AddToEmail(customer, closest5.ElementAt(i).Key);
}

//TASK 3 / 4
//Creates and array of task to get the distance from the API
//cant run this without an actual api
var closestEventsApi = new Dictionary<Event, int>();
List<Task<HttpResponseMessage>> tasks = new List<Task<HttpResponseMessage>>();
foreach (var item in events)
{
    Task<HttpResponseMessage> t = Task.Run(() => EventService.GetDistanceApi(customer.City, item.City));
    tasks.Add(t);
}
Task.WaitAll(tasks.ToArray());
for (var i = 0; i < tasks.Count; i++)
{
    var response = tasks[i];
    //task returns 200
    if (response.Result.IsSuccessStatusCode)
    {
        var responseStream = await response.Result.Content.ReadAsStreamAsync();
        var distance = await JsonSerializer.DeserializeAsync<int>(responseStream);
        closestEventsApi.Add(events[i], distance);
    }
    else
    {
        closestEventsApi.Add(events[i], int.MaxValue);
    }    
}

//TASK 5
var pricedEvents = new Dictionary<Event, int>();
foreach (var item in events)
{
    //changing the value here.
    pricedEvents.Add(item, EventService.GetPrice(item));
}
var priced5 = EventService.SortEvents(closestEvents, (x => x.Value), 5);
for (var i = 0; i < priced5.Count(); i++)
{
    EventService.AddToEmail(customer, priced5.ElementAt(i).Key);
}

