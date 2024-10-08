namespace api.Utilities
{
    /// <summary>
    /// The OrderStatus enum represents the status of an order.
    /// </summary>
    /// 
    /// <remarks>
    /// The OrderStatus enum is used to indicate the current status of an order.
    /// It includes the following values: Pending, PartiallyDelivered, Delivered, Completed, and Cancelled.
    /// </remarks>
    public enum OrderStatus
    {
        Pending,
        PartiallyDelivered,
        Delivered,
        Completed,
        Cancelled,
    }
}