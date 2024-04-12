using Car_WEB_API.Data;
using Car_WEB_API.Interfaces.IBaseRepository;
using Car_WEB_API.Model;
using Car_WEB_API.ViewModel.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Car_WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IRepository<UserOrder> _orderRepository;
        private readonly AppDBContext _appDBContext;

        public OrderController(IRepository<UserOrder> orderRepository, AppDBContext appDBContext = null)
        {
            _orderRepository = orderRepository;
            _appDBContext = appDBContext;
        }




        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _appDBContext.UserOrders
                    .Include(o => o.Users)
                    .Include(o => o.Products)
                    .Select(o => new OrderInfoDto
                    {
                        OrderId = o.Id,
                        UserId = o.UserId,
                        Avatar = o.Users.Image,
                        FirstName = o.Users.FirstName,
                        LastName = o.Users.LastName,
                        Email = o.Users.Email,
                        Role = o.Users.Role,
                        ProductId = o.ProductId,
                        Image = o.Products.Image,
                        Title = o.Products.Title,
                        Model = o.Products.Model,
                        Price = o.Products.Price,
                        OrderDate = o.OrderDate.ToShortDateString(),

                    }).ToListAsync();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving orders: {ex.Message}");
            }
        }


        [HttpGet("Get")]
        public async Task<IActionResult> GetOrder(int id)
        {
            try
            {
                var order = await _appDBContext.UserOrders
                    .Include(o => o.Users)
                    .Include(o => o.Products)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                {
                    return NotFound();
                }

                var orderDto = new OrderInfoDto
                {
                    OrderId = order.Id,
                    UserId = order.UserId,
                    Avatar = order.Users.Image,
                    FirstName = order.Users.FirstName,
                    LastName = order.Users.LastName,
                    Email = order.Users.Email,
                    Role = order.Users.Role,
                    ProductId = order.ProductId,
                    Image = order.Products.Image,
                    Title = order.Products.Title,
                    Model = order.Products.Model,
                    Price = order.Products.Price,
                    OrderDate = order.OrderDate.ToShortDateString()
                };

                return Ok(orderDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving order: {ex.Message}");
            }
        }






        [HttpPost("Upload")]
        public async Task<ActionResult<UserOrder>> CreateOrder(OrderDto orderDTO)
        {
            var user = await _appDBContext.Users.FindAsync(orderDTO.UserId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            foreach (var productId in orderDTO.ProductIds)
            {
                var product = await _appDBContext.Products.FindAsync(productId);

                if (product == null)
                {
                    return NotFound($"Product with ID {productId} not found.");
                }

                var order = new UserOrder
                {
                    UserId = orderDTO.UserId,
                    ProductId = productId,
                    OrderDate = orderDTO.OrderDate
                };

                _appDBContext.UserOrders.Add(order);
            }

            await _appDBContext.SaveChangesAsync();

            return Ok(orderDTO);
        }






        [HttpDelete("DeleteOrderById")]
        public async Task<IActionResult> RemoveOrder(int id)
        {
            await _orderRepository.Delete(id);
            return NoContent();
        }









    }
}

