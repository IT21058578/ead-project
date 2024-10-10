using api.Utilities;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace api.Models
{
    /// <summary>
    /// The Order class represents an order in the system.
    /// </summary>
    /// 
    /// <remarks>
    /// The Order class is a derived class of the BaseModel class and inherits its common properties.
    /// It contains properties such as UserId, Status, Products, DeliveryNote, DeliveryAddress,
    /// DeliveryDate, ActualDeliveryDate, and VendorIds that represent various aspects of an order.
    /// The class also includes a nested class called Item, which represents an item within an order.
    /// </remarks>
    [Collection("orders")]
    public class Order : BaseModel
    {
        public ObjectId UserId { get; set; } = ObjectId.Empty;
        public OrderStatus Status { get; set; } = OrderStatus.Pending!;
        public List<Item> Products { get; set; } = [];
        public string DeliveryNote { get; set; } = "";
        public string DeliveryAddress { get; set; } = "";
        public DateTime DeliveryDate { get; set; } = DateTime.Now;
        public DateTime? ActualDeliveryDate { get; set; }
        public IEnumerable<string> VendorIds { get; set; } = [];
        // Assumption: Payments are always done when Order is created.

        public class Item
        {
            public ObjectId ProductId { get; set; } = ObjectId.Empty;
            public ObjectId VendorId { get; set; } = ObjectId.Empty;
            public int Quantity { get; set; } = 0;
            public OrderStatus Status { get; set; } = OrderStatus.Pending!;
            public string Name { get; set; } = "";
            public double Price { get; set; } = 0.0;
        }
    }
}