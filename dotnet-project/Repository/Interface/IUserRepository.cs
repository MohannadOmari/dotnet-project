using dotnet_project.Models;

namespace dotnet_project.Repository.Interface
{
    public interface IUserRepository
    {
        public Task GetUser(string email, string password);
        public Task AddUser(Users user);
        public Task GetUserById(int id);
        public Task UpdateUser(Users user);
    }
}
