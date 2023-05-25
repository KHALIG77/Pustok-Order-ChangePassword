namespace PustokStart.ViewModels
{
	public class OrderViewModel
	{
		public List<CheckoutItem> CheckoutItems { get; set; }=new List<CheckoutItem>();
		public OrderCreateViewModel Order  { get; set; }
		public decimal TotalPrice {get; set; }
	}
}
