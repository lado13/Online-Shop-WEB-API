namespace Car_WEB_API.Model
{
    public class UserOrder
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User Users { get; set; }
        public int ProductId { get; set; }
        public Product Products { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
