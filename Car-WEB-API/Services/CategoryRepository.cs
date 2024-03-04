using Car_WEB_API.Data;
using Car_WEB_API.Interfaces.IBaseRepository;
using Car_WEB_API.Model;
using Microsoft.EntityFrameworkCore;

namespace Car_WEB_API.Services
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly AppDBContext _dbContext;
        public CategoryRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Category> Add(Category entity)
        {
            try
            {
                _dbContext.Categories.Add(entity);
                await _dbContext.SaveChangesAsync();    
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the category.", ex);
            }
            return entity;
        }

        public async Task Delete(int id)
        {
            try
            {
                var item = await _dbContext.Categories.FindAsync(id);
                if (item != null)
                {
                    _dbContext.Categories.Remove(item); 
                    await _dbContext.SaveChangesAsync();
                }            
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the category.", ex);
            }
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            try
            {
                return await _dbContext.Categories.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all categories.", ex);
            }
        }
        public async Task<Category> GetById(int id)
        {
            try
            {
                return await _dbContext.Categories.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving category with ID {id}.", ex);
            }
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<Category> Update(Category entity)
        {
            try
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();    
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the category.", ex);
            }
            return entity;  
        }
    }
}
