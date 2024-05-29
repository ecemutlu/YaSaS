using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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

		/*
		public CityTownService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
		}
		*/

		// Şehir listesini alır
		public async Task<List<SelectListItem>> GetCities()
		{
			if (_citiesCache != null)  // Cache kontrolü
			{
				return _citiesCache.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
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
                        // Cache'i doldurduktan sonra alfabetik sırayla şehirleri döndür
                        return _citiesCache.OrderBy(c => c.Name).Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
                    }
				}
				else
				{
					_logger.LogError("Şehirler API isteği başarısız oldu: {StatusCode}", response.StatusCode);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Şehirler API isteği sırasında bir hata oluştu.");
			}

			return new List<SelectListItem>();
		}


		// İlçe listesini alır
		public async Task<List<SelectListItem>> GetTowns(int cityId)
		{
			if (_citiesCache == null || !_citiesCache.Any())
			{
				await GetCities();  // Eğer cache boşsa, şehirleri tekrar yüklemeye çalış
				if (_citiesCache == null || !_citiesCache.Any())
				{
					_logger.LogError("Şehirler yüklendikten sonra ilçe bilgileri yüklenemedi.");
					return new List<SelectListItem>();
				}
			}

			var city = _citiesCache.FirstOrDefault(c => c.Id == cityId);
			if (city != null && city.Districts != null)
			{
				return city.Districts.Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name }).ToList();
			}

			return new List<SelectListItem>();
		}
	}

	// JSON verisini parse etmek için sınıflar
	public class CityResponse
	{
		public List<City> Data { get; set; }
	}

	public class City
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<District> Districts { get; set; }
	}

	public class District
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
}
