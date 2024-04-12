using Car_WEB_API.Data;
using Car_WEB_API.Interfaces.IBaseRepository;
using Car_WEB_API.Model;
using Microsoft.EntityFrameworkCore;

namespace Car_WEB_API.Services
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly AppDBContext _dbContext;

        public ProductRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product> Add(Product entity)
        {
            try
            {
                _dbContext.Products.Add(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the product.", ex);
            }
            return entity;  
        }

        public async Task Delete(int id)
        {
            try
            {
                var item = await _dbContext.Products.FindAsync(id);
                if (item != null)
                {
                    _dbContext.Products.Remove(item);    
                    await _dbContext.SaveChangesAsync();
                }          
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the product.", ex);
            }
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            try
            {
                return await _dbContext.Products.ToListAsync();
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while retrieving all products.", ex);
            }
        }

        public async Task<Product> GetById(int id)
        {
            try
            {

                return await _dbContext.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {

                throw new Exception($"An error occurred while retrieving product with ID {id}.", ex);
            }
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> Update(Product entity)
        {
            try
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the product.", ex);
            }
            return entity;  
        }




    }
}
