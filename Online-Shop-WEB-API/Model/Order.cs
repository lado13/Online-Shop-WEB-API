using System.ComponentModel.DataAnnotations;

namespace Car_WEB_API.Model
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<OrderProduct> Products { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
