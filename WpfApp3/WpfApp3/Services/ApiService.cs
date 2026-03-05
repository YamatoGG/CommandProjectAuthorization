using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WpfApp3.Models;

namespace WpfApp3.Services
{
	public class ApiService
	{
		private readonly HttpClient _httpClient;
	
		private readonly string _baseUrl = "http://localhost:5196";

		public ApiService()
		{
			_httpClient = new HttpClient();
			_httpClient.BaseAddress = new Uri(_baseUrl);
			_httpClient.Timeout = TimeSpan.FromSeconds(10);
		}

		public async Task<List<Product>> GetProductsAsync()
		{
			try
			{
				
				HttpResponseMessage response = await _httpClient.GetAsync("/products");

				if (response.IsSuccessStatusCode)
				{
					string json = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<List<Product>>(json);
				}

				throw new Exception($"Сервер вернул ошибку: {response.StatusCode}");
			}
			catch (Exception ex)
			{
				throw new Exception($"Ошибка подключения к серверу: {ex.Message}");
			}
		}
	}
}