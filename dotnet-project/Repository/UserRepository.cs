using Dapper;
using dotnet_project.Context;
using dotnet_project.Models;
using dotnet_project.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace dotnet_project.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;

        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task AddUser(Users user)
        {
            var query = "SELECT 1 FROM Users WHERE Email = @Email";
            var Email = user.Email;
            string Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            using (var connection = _context.CreateConnection())
            {
                var isRegisterd = await connection.QuerySingleOrDefaultAsync(query, new { Email });
                if (isRegisterd == null)
                {
                    query = "INSERT INTO Users (FullName, Email, Password, Gender, Address, BirthDate, UserTypeId, CreatedAt, UpdatedAt, isActive)" 
                        + "VALUES (@FullName, @Email, @Password, @Gender, @Address, @BirthDate, @UserTypeId, @CreatedAt, @UpdatedAt, @isActive)";
                    var parameters = new DynamicParameters();
                    parameters.Add("FullName", user.FullName);
                    parameters.Add("Email", user.Email);
                    parameters.Add("Password", Password);
                    parameters.Add("Gender", user.Gender);
                    parameters.Add("Address", user.Address);
                    parameters.Add("BirthDate", user.BirthDate);
                    parameters.Add("UserTypeId", user.UserTypeId);
                    parameters.Add("CreatedAt", user.CreatedAt);
                    parameters.Add("UpdatedAt", DateTime.Now);
                    parameters.Add("isActive", "Yes");
                    await connection.ExecuteAsync(query, parameters);
                }
            }
        }

        public async Task GetUser(string email, string password)
        {
            var query = "SELECT Password FROM Users WHERE Email = @Email AND isActive = 'Yes'";
            var Email = email;
            using (var connection = _context.CreateConnection())
            {
                var user = await connection.QuerySingleOrDefaultAsync<Users>(query, new { Email });
                var hashedPassword = user.Password;
                if (user != null)
                {
                    bool passwordMatch = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
                    if (passwordMatch)
                    {
                        Console.WriteLine("User Logged in");
                    }
                    else
                    {
                        Console.WriteLine("Wrong Password");
                    }
                }
                else
                {
                    Console.WriteLine("Wrong email");
                }
            }

        }

        public Task GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUser(Users user)
        {
            throw new NotImplementedException();
        }
    }
}
