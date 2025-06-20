using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CRUDApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string? Name { get; set; }
        [MaxLength(100)]
        public string? Brand { get; set; }
        [Precision(16, 2)] // Precision for decimal type
        public decimal Price { get; set; }
        [MaxLength(100)]
        public string? Category { get; set; }
        public string? Description { get; set; }
        [MaxLength(100)]
        public string? ImageFilePath { get; set; }
        public DateTime CreatedAt { get; set; }



    }
}
