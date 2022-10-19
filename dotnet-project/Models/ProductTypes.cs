using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_project.Models
{
    public class ProductTypes
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductType { get; set; }
    }
}