

using api.Utilities;

namespace api.DTOs.Models
{
    /// <summary>
    /// The OrderDto class represents the data transfer object for an order.
    /// </summary>
    /// 
    /// <remarks>
    /// The OrderDto class is used to transfer data for an order.
    /// It contains properties for the order ID, created by user, creation date, 
    /// updated by user, update date, user ID, order status, products, delivery note, 
    /// delivery address, delivery date, actual delivery date, and vendor IDs.
    /// </remarks>
    public class OrderDto
    {
        public string Id { get; set; } = "";
        public string CreatedBy { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; } = "";
        public DateTime UpdatedAt { get; set; }
        public string UserId { get; set; } = "";
        public OrderStatus Status { get; set; } = OrderStatus.Pending!;
        public List<Item> Products { get; set; } = [];
        public string DeliveryNote { get; set; } = "";
        public string DeliveryAddress { get; set; } = "";
        public DateTime DeliveryDate { get; set; } = DateTime.Now;
        public DateTime? ActualDeliveryDate { get; set; }
        public IEnumerable<string> VendorIds { get; set; } = [];

        public class Item
        {
            public string ProductId { get; set; } = "";
            public string VendorId { get; set; } = "";
            public int Quantity { get; set; } = 0;
            public OrderStatus Status { get; set; } = OrderStatus.Pending!;
            public string Name { get; set; } = "";
            public double Price { get; set; }
        }
    }
}