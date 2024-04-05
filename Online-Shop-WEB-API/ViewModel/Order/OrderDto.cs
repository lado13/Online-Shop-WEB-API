namespace Car_WEB_API.ViewModel.Order
{
    public class OrderDto
    {
        public int UserId { get; set; }
        public List<int> ProductIds { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
