namespace WpfApp3.Models
{
	public class Product
	{
		public int Id { get; set; }          
		public string Name { get; set; }      
		public decimal Price { get; set; }   

		public string DisplayName => Name;
		public string DisplayPrice => $"{Price:N2} руб.";
	}
}