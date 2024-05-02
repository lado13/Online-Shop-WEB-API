using Car_WEB_API.Data;
using Car_WEB_API.Interfaces.IBaseRepository;
using Car_WEB_API.Model;
using Microsoft.EntityFrameworkCore;

namespace Car_WEB_API.Services
{

    //Abstraction is used

    public class OrderRepository : IRepository<UserOrder>
    {

        private readonly AppDBContext _dbContext;

        public OrderRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }



        //This method sends the order to the database

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


        //Deletes a customer order from the database

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



        //Returns all customer orders

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


        //Retrieves a specific single customer order by id

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




        //Searching for customers by email

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



        //This method does not need to be used with orders

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
