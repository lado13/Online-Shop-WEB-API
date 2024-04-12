namespace Car_WEB_API.ViewModel.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
