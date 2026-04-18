using System.ComponentModel.DataAnnotations;

namespace SportsStore.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Please enter your full name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your address")]
        public string AddressLine1 { get; set; } = string.Empty;

        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }

        [Required(ErrorMessage = "Please enter your city")]
        public string City { get; set; } = string.Empty;

        public string? State { get; set; }

        public string? ZipCode { get; set; }

        public bool GiftWrap { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public List<OrderLine> Lines { get; set; } = new();
    }

    public class OrderLine
    {
        public int OrderLineId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}