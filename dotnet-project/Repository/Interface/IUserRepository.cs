using dotnet_project.Models;

namespace dotnet_project.Repository.Interface
{
    public interface IUserRepository
    {
        public Task<Users?> GetUser(Users user);
        public Task AddUser(Users user);
        public Task<Users> GetUserById(int id);
        public Task UpdateUser(Users user, int? id);
    }
}
