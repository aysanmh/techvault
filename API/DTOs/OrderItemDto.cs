using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class OrderItemDto
    {
        public int DeviceId { get; set; }

        public required string DeviceName { get; set; }

        public required string ImageUrl { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
} 