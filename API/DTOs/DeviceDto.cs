using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class DeviceDto
    {
        public int Id { get; set; }
        public string Model { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }

        public string BrandName { get; set; } = null!;
        public string DeviceGroupName { get; set; } = null!;
    }
}