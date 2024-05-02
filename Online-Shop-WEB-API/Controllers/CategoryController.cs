using Car_WEB_API.Interfaces.IBaseRepository;
using Car_WEB_API.Model;
using Microsoft.AspNetCore.Mvc;

namespace Car_WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {


        private readonly IRepository<Category> _categoryRepository;


        public CategoryController(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        //Displays all categories

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetCategories()
        {
            var item = await _categoryRepository.GetAll();
            return Ok(item);
        }


        //Returns one concrete category with id

        [HttpGet("Get")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var item = await _categoryRepository.GetById(id);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }



        //Adds a category

        [HttpPost("Add")]
        public async Task<ActionResult> AddCategory(Category category)
        {
            try
            {
                var item = await _categoryRepository.Add(category);
                return CreatedAtAction(nameof(GetCategory), new { id = item.Id }, item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the category.");
            }
        }




        //Removes a category

        [HttpDelete("Delete")]
        public async Task<IActionResult> RemoveCategory(int id)
        {
            await _categoryRepository.Delete(id);
            return NoContent();
        }



        //Updates the category

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }
            await _categoryRepository.Update(category);
            return NoContent();
        }







    }
}
