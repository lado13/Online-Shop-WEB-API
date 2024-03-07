using Car_WEB_API.Data;
using Car_WEB_API.Interfaces.IBaseRepository;
using Car_WEB_API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Car_WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _productsRepository;
        private readonly IRepository<Category> _categoriesRepository;
        private readonly AppDBContext _appDBContext;

        public ProductController(IRepository<Product> productsRepository, AppDBContext appDBContext, IRepository<Category> categoriesRepository)
        {
            _productsRepository = productsRepository;
            _appDBContext = appDBContext;
            _categoriesRepository = categoriesRepository;
        }

        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productsRepository.GetAll();
            return Ok(products);
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var item = await _productsRepository.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetProducts(int page = 1, int pageSize = 10)
        {
            try
            {
                int skip = (page - 1) * pageSize;

                var products = await _appDBContext.Products
                    .OrderBy(p => p.Id)
                    .Skip(skip)
                    .Take(pageSize)
                    .ToListAsync();

                var totalCount = await _appDBContext.Products.CountAsync();

                return Ok(new { Products = products, TotalCount = totalCount });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddProduct(Product product)
        {
            try
            {
                var item = await _productsRepository.Add(product);
                return CreatedAtAction(nameof(GetProduct), new { id = item.Id }, item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the product.");
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> RemoveProduct(int id)
        {
            await _productsRepository.Delete(id);
            return NoContent();
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }
            await _productsRepository.Update(product);
            return NoContent();
        }  

        [HttpGet("SearchByTitle")]
        public async Task<IActionResult> Search(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return BadRequest("Keyword cannot be empty");

            var products = await _appDBContext.Products
                .Where(p => p.Title.Contains(keyword))
                .Select(p => new { p.Id, p.Title, p.Model, p.Price , p.Image})
                .ToListAsync();

            return Ok(products);
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> AddProducts([FromBody] Product product)
        {
            try
            {
                var category = await _appDBContext.Categories.FindAsync(product.CategoryId);
                if (category == null)
                {
                    return BadRequest("Specified category does not exist.");
                }
                product.Category = null;
                _appDBContext.Products.Add(product);
                await _appDBContext.SaveChangesAsync();
                return Ok(new { Message = "Product added successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Error = "An error occurred while adding the product." });
            }
        }

        [HttpGet("ProductByCategory")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(int categoryId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var products = await _appDBContext.Products
                    .Include(p => p.Category)
                    .Where(p => p.CategoryId == categoryId)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving products.");
            }
        }











    }
}
