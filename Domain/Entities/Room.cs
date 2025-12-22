using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }

        public string Name { get; set; }
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
    }
}
