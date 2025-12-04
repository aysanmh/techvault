using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class DeviceCreateDto
    {

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Device-Group-Id is required.")]
        public int DeviceGroupId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Brand-Id is required.")]
        public int BrandId { get; set; }
       
        [Required]
        public  string Model { get; set; } = string.Empty;

        [Required]
        public  string ImageUrl { get; set; } = string.Empty;

        [Required]
        public  string Description { get; set; } = string.Empty;
       
       
        [Range(0.01,double.MaxValue,ErrorMessage ="Price must be greater than 0 ")]
        public decimal Price { get; set; }
    }
}
