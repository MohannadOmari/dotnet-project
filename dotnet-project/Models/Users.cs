using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_project.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "User's Name is Required")]
        public string? FullName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [Email(ErrorMessage = "Must be an email format")]
        public string? Email { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Password must be longer than 8 characters long")]
        [MaxLength(30, ErrorMessage = "Password cannot be longer than 30 characters long")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Please select your gender")]
        public string? Gender { get; set; }
        [Required(ErrorMessage = "Please provide your address")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "Please provide us with you birthdate")]
        [Date]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Please select whether you want to be a seller or buyer")]
        public int UserTypeId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        public string? isActive { get; set; }
    }
}