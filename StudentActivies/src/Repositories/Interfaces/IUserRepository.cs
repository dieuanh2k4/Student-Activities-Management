using StudentActivities.src.Models;

namespace StudentActivities.src.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<Users?> GetByUserNameAsync(string username);
    }
}
