namespace Car_WEB_API.ViewModel.Order
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public int ProductId { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
    }
}
