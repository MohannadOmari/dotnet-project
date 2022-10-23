using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_project.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public DateTime BirthDate { get; set; }
        public int UserTypeId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        public string? isActive { get; set; }
    }
}