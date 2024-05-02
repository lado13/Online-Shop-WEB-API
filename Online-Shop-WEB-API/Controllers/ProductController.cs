using Car_WEB_API.Data;
using Car_WEB_API.Interfaces.IBaseRepository;
using Car_WEB_API.Model;
using Car_WEB_API.ViewModel.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Car_WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {


        private readonly IRepository<Product> _productsRepository;
        private readonly AppDBContext _appDBContext;



        public ProductController(IRepository<Product> productsRepository, AppDBContext appDBContext)
        {
            _productsRepository = productsRepository;
            _appDBContext = appDBContext;
        }





        //Shows all products

        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _appDBContext.Products
                    .OrderBy(p => p.Id)
                    .Select(p => new ProductDto
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Model = p.Model,
                        Price = p.Price,
                        Image = p.Image,
                        CategoryId = p.CategoryId,
                        CategoryName = p.Category.Name
                    })
                    .ToListAsync();

                var totalCount = await _appDBContext.Products.CountAsync();

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }



        //Shows one specific product with a specific id

        [HttpGet("Get")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var item = await _productsRepository.GetById(id);
            if (item == null)
            {
                return NotFound();
            }

            var productDto = new ProductDto
            {
                Id = item.Id,
                Title = item.Title,
                Model = item.Model,
                Price = item.Price,
                Image = item.Image,
                CategoryId = item.CategoryId,
                CategoryName = item.Category.Name
            };

            return Ok(productDto);
        }


        //Shows all products by page

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
                    .Select(p => new ProductDto
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Model = p.Model,
                        Price = p.Price,
                        Image = p.Image,
                        CategoryId = p.CategoryId,
                        CategoryName = p.Category.Name 
                    })
                    .ToListAsync();

                var totalCount = await _appDBContext.Products.CountAsync();

                return Ok(new { Products = products, TotalCount = totalCount });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }



        //Deletes the product with the concrete id

        [HttpDelete("Delete")]
        public async Task<IActionResult> RemoveProduct(int id)
        {
            await _productsRepository.Delete(id);
            return NoContent();
        }





        //Updates a product with a specific id

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


        //Live Search searches for products by name

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



        //Filters products by price

        [HttpGet("FilterByPrice")]
        public async Task<IActionResult> FilterByPrice(decimal minPrice, decimal maxPrice)
        {
            if (minPrice < 0 || maxPrice < 0 || minPrice > maxPrice)
                return BadRequest("Invalid price range");

            var products = await _appDBContext.Products
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                .Select(p => new { p.Id, p.Title, p.Model, p.Price, p.Image })
                .ToListAsync();

            return Ok(products);
        }







        //Loads the product into the database

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



        //Filters products by category and displays them as pages

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
