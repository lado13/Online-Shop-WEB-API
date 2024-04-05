using Car_WEB_API.Data;
using Car_WEB_API.Interfaces.IBaseRepository;
using Car_WEB_API.Model;
using Microsoft.EntityFrameworkCore;

namespace Car_WEB_API.Services
{
    public class OrderRepository : IRepository<UserOrder>
    {

        private readonly AppDBContext _dbContext;

        public OrderRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }



        public async Task<UserOrder> Add(UserOrder entity)
        {
            try
            {
                _dbContext.UserOrders.Add(entity);
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
                var item = await _dbContext.UserOrders.FindAsync(id);
                if (item != null)
                {
                    _dbContext.UserOrders.Remove(item);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the order.", ex);
            }
        }

        public async Task<IEnumerable<UserOrder>> GetAll()
        {
            try
            {
                return await _dbContext.UserOrders.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all orders.", ex);
            }
        }

        public async Task<UserOrder> GetById(int id)
        {
            try
            {
                return await _dbContext.UserOrders.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving order with ID {id}.", ex);
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving user with email {email}.", ex);
            }
        }

        public async Task<UserOrder> Update(UserOrder entity)
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
