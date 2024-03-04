using Car_WEB_API.Data;
using Car_WEB_API.Interfaces.IBaseRepository;
using Car_WEB_API.Model;
using Microsoft.EntityFrameworkCore;

namespace Car_WEB_API.Services
{
    public class OrderRepository : IRepository<Order>
    {
        private readonly AppDBContext _dbContext;
        public OrderRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order> Add(Order entity)
        {
            try
            {
                _dbContext.Orders.Add(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the Order.", ex);
            }
            return entity;
        }

        public async Task Delete(int id)
        {
            try
            {
                var item = await _dbContext.Orders.FindAsync(id);
                if (item != null)
                {
                    _dbContext.Orders.Remove(item);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the order.", ex);
            }
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            try
            {
                return await _dbContext.Orders.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all orders.", ex);
            }
        }

        public async Task<Order> GetById(int id)
        {
            try
            {
                return await _dbContext.Orders.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving order with ID {id}.", ex);
            }
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<Order> Update(Order entity)
        {
            try
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the order.", ex);
            }
            return entity;
        }



    }
}
