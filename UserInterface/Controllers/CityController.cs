using Microsoft.AspNetCore.Mvc;
using DataAccessLayer.Context;
using EntityLayer.Concrete;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using UserInterface.Services;

namespace UserInterface.Controllers
{
    public class CityController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly mydbContext _context;
        private readonly ILogger<CityController> _logger;

        public CityController(HttpClient httpClient, mydbContext context, ILogger<CityController> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        private async Task InsertCitiesAndTownsAsync()
        {
            try
            {
                var cityResponse = await _httpClient.GetAsync("https://turkiyeapi.dev/api/v1/provinces");
                if (cityResponse.IsSuccessStatusCode)
                {
                    var cityJson = await cityResponse.Content.ReadAsStringAsync();
                    var cityResponseObject = JsonConvert.DeserializeObject<EntityLayer.Concrete.CityResponse>(cityJson);

                    if (cityResponseObject?.Data != null)
                    {
                        _context.City.AddRange(cityResponseObject.Data);

                        foreach (var city in cityResponseObject.Data)
                        {
                            if (city.Towns != null)
                            {
                                foreach (var town in city.Towns)
                                {
                                    town.CityId = city.Id; // Ensure the foreign key is set correctly
                                    _context.Town.Add(town);
                                }
                            }
                        }

                        await _context.SaveChangesAsync();
                        _logger.LogInformation("All cities and towns have been successfully inserted into the database.");
                    }
                    else
                    {
                        _logger.LogWarning("No city data was found in the API response.");
                    }
                }
                else
                {
                    _logger.LogError($"Failed to fetch cities from API. Status code: {cityResponse.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while inserting cities and towns into the database: {ex.Message}");
            }
        }
        
        [HttpGet("insert-cities-towns")]
        public async Task<IActionResult> InsertCitiesAndTowns()
        {
            await InsertCitiesAndTownsAsync();
            return Ok("Cities and towns have been inserted successfully.");
        }

    }
}
