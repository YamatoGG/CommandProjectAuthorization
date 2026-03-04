using System;
using System.Windows;
using WpfApp3.Services;

namespace WpfApp3.Views
{
	public partial class ProductsWindow : Window
	{
		private readonly ApiService _apiService;

		public ProductsWindow()
		{
			InitializeComponent();
			_apiService = new ApiService();
			LoadProducts();
		}

		private async void LoadProducts()
		{
			try
			{
				// Показываем статус загрузки
				dgProducts.ItemsSource = null;
				txtTotal.Text = "Загрузка...";
				btnRefresh.IsEnabled = false;
				this.Title = "Загрузка товаров...";

				// Получаем данные с сервера
				var products = await _apiService.GetProductsAsync();

				if (products != null && products.Count > 0)
				{
					// Успешно загрузили данные
					dgProducts.ItemsSource = products;
					txtTotal.Text = $"Всего товаров: {products.Count}";
					this.Title = $"Список товаров ({products.Count})";
				}
				else
				{
				
					MessageBox.Show(
						"Сервер вернул пустой список товаров или недоступен.\n" +
						"Проверьте подключение к серверу.",
						"Нет данных",
						MessageBoxButton.OK,
						MessageBoxImage.Information);

					txtTotal.Text = "Нет данных";
					this.Title = "Список товаров (нет данных)";
				}
			}
			catch (Exception ex)
			{

				MessageBox.Show(
                	$"Ошибка загрузки данных: {ex.Message}",
	                "Ошибка",
	                 MessageBoxButton.OK,
	                 MessageBoxImage.Error);

				txtTotal.Text = "Ошибка загрузки";
				this.Title = "Ошибка подключения";
			}
			finally
			{
				btnRefresh.IsEnabled = true;
			}
		}

		private void btnRefresh_Click(object sender, RoutedEventArgs e)
		{
			LoadProducts();
		}
	}
}