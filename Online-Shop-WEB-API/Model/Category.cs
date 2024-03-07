using System.ComponentModel.DataAnnotations;

namespace Car_WEB_API.Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
