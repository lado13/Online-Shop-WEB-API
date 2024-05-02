using Car_WEB_API.Data;
using Car_WEB_API.Interfaces.IBaseRepository;
using Car_WEB_API.Model;
using Microsoft.EntityFrameworkCore;

namespace Car_WEB_API.Services
{

    //Abstraction is used

    public class UserRepository : IRepository<User>
    {

        private readonly AppDBContext _dbContext;


        public UserRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }



        //Adds a user to the database

        public async Task<User> Add(User entity)
        {
            try
            {
                _dbContext.Users.Add(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the user.", ex);
            }
            return entity;
        }



        //Removes a user from the database

        public async Task Delete(int id)
        {
            try
            {
                var item = await _dbContext.Users.FindAsync(id);
                if (item != null)
                {
                    _dbContext.Users.Remove(item);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the user.", ex);
            }
        }


        //returns all users

        public async Task<IEnumerable<User>> GetAll()
        {
            try
            {
                return await _dbContext.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all users.", ex);
            }
        }



        //return a specific user with a specific ID

        public async Task<User> GetById(int id)
        {
            try
            {
                return await _dbContext.Users.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving user with ID {id}.", ex);
            }
        }





        //Updates user data from the database

        public async Task<User> Update(User entity)
        {
            try
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the user.", ex);
            }
            return entity;
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

                throw new Exception("Error Get User By Email !!!");
            }
        }



    }
}
