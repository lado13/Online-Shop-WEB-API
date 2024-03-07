using Car_WEB_API.Model;
using System.ComponentModel.DataAnnotations;

namespace Car_WEB_API
{
    public class OrderProduct
    {

        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; } 
        public int ProductId { get; set; }
    }
}
