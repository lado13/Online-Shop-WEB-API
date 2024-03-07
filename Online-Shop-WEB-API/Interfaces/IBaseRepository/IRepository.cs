using Car_WEB_API.Model;

namespace Car_WEB_API.Interfaces.IBaseRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task Delete(int id);
        Task<User> GetUserByEmailAsync(string email);
    }
}
