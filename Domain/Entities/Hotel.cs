using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Hotel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty; 

        [Required]
        [MaxLength(250)]
        public string Address { get; set; } = string.Empty; 

        [MaxLength(1000)]
        public string? Description { get; set; } 

        public ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}
