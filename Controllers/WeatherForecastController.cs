using Microsoft.AspNetCore.Mvc;
using MyApi.Models;

namespace MyApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly List<WeatherForecast> arr;

    public WeatherForecastController()
    {
        arr = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToList();
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return arr;
    }

    [HttpGet("{id}")]
    public ActionResult<WeatherForecast> Get(int id)
    {
        if (id < 0 || id >= arr.Count)
            return BadRequest();
        return arr[id];
    }

    [HttpPost]
    public void Post(WeatherForecast newW)
    {
        arr.Add(newW);
    }

    [HttpPut("{id}")]
    public void Put(int id, WeatherForecast newW)
    {
        if (id < 0 || id >= arr.Count)
            return;
        arr[id] = newW;
    }
}
