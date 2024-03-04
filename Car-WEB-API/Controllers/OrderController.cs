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
        private readonly IRepository<Order> _orderRepository;
        private readonly AppDBContext _appDBContext;

        public OrderController(IRepository<Order> orderRepository, AppDBContext appDBContext = null)
        {
            _orderRepository = orderRepository;
            _appDBContext = appDBContext;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await (from op in _appDBContext.OrderProducts
                                join p in _appDBContext.Products on op.ProductId equals p.Id
                                join u in _appDBContext.Users on op.OrderId equals u.Id
                                select new OrderDto
                                {
                                    OrderId =op.Id,
                                    UserId = u.Id,
                                    FirstName = u.FirstName,
                                    LastName = u.LastName,
                                    Email = u.Email,
                                    Role = u.Role,
                                    ProductId = p.Id,
                                    Image = p.Image,
                                    Title = p.Title,
                                    Model = p.Model,
                                    Price = p.Price
                                }).ToListAsync();

            return Ok(orders);
        }


        [HttpGet("Get")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var item = await _orderRepository.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }


        [HttpPost("Add")]
        public async Task<ActionResult> AddOrder([FromBody] Order order)
        {
            try
            {
                var item = await _orderRepository.Add(order);
                return CreatedAtAction(nameof(GetOrder), new { id = item.Id }, item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the category.");
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> RemoveOrder(int id)
        {
            await _orderRepository.Delete(id);
            return NoContent();
        }


        [HttpPut("Update")]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }
            await _orderRepository.Update(order);
            return NoContent();
        }

        [HttpPost("Upload")]
        public async Task<ActionResult<Order>> CreateOrder(Order orderDto)
        {
            try
            {
                var order = new Order
                {
                    UserId = orderDto.UserId,
                    OrderDate = orderDto.OrderDate
                };

                _appDBContext.Orders.Add(order);

                foreach (var productDto in orderDto.Products)
                {
                    var orderProduct = new OrderProduct
                    {
                        OrderId = orderDto.UserId,
                        ProductId = productDto.ProductId
                    };
                    _appDBContext.OrderProducts.Add(orderProduct);
                }
                await _appDBContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the order: {ex.Message}");
            }
        }


        [HttpDelete("DeleteOrderById")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var order = await _appDBContext.OrderProducts.FindAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }
            _appDBContext.OrderProducts.Remove(order);
            await _appDBContext.SaveChangesAsync();
            return Ok();
        }









    }
}

