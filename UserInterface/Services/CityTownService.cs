using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace UserInterface.Services

{
    public class CityTownService
    {
        private readonly HttpClient _httpClient;
		private readonly ILogger<CityTownService> _logger; // Logger'ı sınıfınıza ekleyin
		private List<City> _citiesCache = null; // Şehirlerin ve ilçelerin önbelleği

		// Constructor
		public CityTownService(HttpClient httpClient, ILogger<CityTownService> logger)
		{
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

        // Şehir listesini alır
        public async Task<List<SelectListItem>> GetCities()
        {
            _logger.LogInformation("Fetching cities from API.");
            if (_citiesCache != null)  // Cache kontrolü
            {
                _logger.LogInformation($"Returning {_citiesCache.Count} cached cities.");
                return _citiesCache.OrderBy(c => c.Name).Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
            }

            try
            {
                var response = await _httpClient.GetAsync("https://turkiyeapi.dev/api/v1/provinces");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var citiesData = JsonConvert.DeserializeObject<CityResponse>(json);
                    if (citiesData != null && citiesData.Data != null)
                    {
                        _citiesCache = citiesData.Data;
                        _logger.LogInformation($"Fetched {citiesData.Data.Count} cities from API.");
                        return _citiesCache.OrderBy(c => c.Name).Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
                    }
                    else
                    {
                        _logger.LogWarning("No cities found in API response.");
                    }
                }
                else
                {
                    _logger.LogError($"Failed to fetch cities: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching cities from API.");
            }

            return new List<SelectListItem>();
        }


        // İlçe listesini alır
        public async Task<List<SelectListItem>> GetTowns(int cityId)
        {
            _logger.LogInformation($"Fetching towns for city ID: {cityId}");
            if (_citiesCache == null || !_citiesCache.Any())
            {
                await GetCities();  // Eğer cache boşsa, şehirleri tekrar yüklemeye çalış
                if (_citiesCache == null || !_citiesCache.Any())
                {
                    _logger.LogError("No cities loaded when trying to load towns.");
                    return new List<SelectListItem>();
                }
            }

            var city = _citiesCache.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                _logger.LogWarning($"City with ID {cityId} not found.");
                return new List<SelectListItem>();
            }

            if (city.Towns != null && city.Towns.Any())
            {
                var townList = city.Towns.OrderBy(t => t.Name).Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name }).ToList();
                _logger.LogInformation($"Found {townList.Count} towns for city ID {cityId}");
                return townList;
            }
            else
            {
                _logger.LogWarning($"No towns found for city ID {cityId}");
                return new List<SelectListItem>();
            }
        }

        public async Task<City> GetCityById(int cityId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/v1/provinces/{cityId}");
                var json = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"API Response for city ID {cityId}: {json}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"No valid city found with ID: {cityId}. Status code: {response.StatusCode}");
                    return null;
                }

                // JSON'dan yalnızca id ve name bilgilerini çekmek
                var jObject = JObject.Parse(json);
                var cityData = jObject["data"];
                var city = new City
                {
                    Id = (int)cityData["id"],
                    Name = (string)cityData["name"]
                };

                if (city == null || city.Id == 0 || string.IsNullOrEmpty(city.Name))
                {
                    _logger.LogError($"City data is incomplete or missing for ID: {cityId}");
                    throw new InvalidOperationException("City data is incomplete or missing.");
                }

                return city;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching city by ID {cityId}: {ex.Message}");
                throw;
            }
        }

        public async Task<Town> GetTownById(int townId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://turkiyeapi.dev/api/v1/districts/{townId}");
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to fetch town. Status code: {response.StatusCode}");
                    return null; // Başarısız yanıt durumunda null döndür.
                }

                var json = await response.Content.ReadAsStringAsync();
                var jObject = JObject.Parse(json);
                var townData = jObject["data"]; // 'data' nesnesine erişim sağla.

                if (townData == null)
                {
                    _logger.LogError("Town data is missing in the response.");
                    throw new InvalidOperationException("Town data is missing in the response.");
                }

                var town = new Town
                {
                    Id = (int)townData["id"],
                    Name = (string)townData["name"] // 'name' alanını doğru şekilde çıkar.
                };

                if (string.IsNullOrEmpty(town.Name))
                {
                    _logger.LogError($"Town name is missing for ID: {townId}");
                    throw new InvalidOperationException("Town name is missing.");
                }

                return town; // Town nesnesini doğru şekilde döndür.
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching town by ID {townId}: {ex.Message}");
                throw; // Hata durumunda exception fırlat.
            }
        }
    }

	// JSON verisini parse etmek için sınıflar
	public class CityResponse
	{
		public List<City> Data { get; set; }
	}
}
