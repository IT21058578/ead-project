namespace api.Utilities
{
    /// <summary>
    /// The NotificationType enum represents the types of notifications that can be sent.
    /// </summary>
    /// 
    /// <remarks>
    /// The NotificationType enum is used to specify the type of notification to be sent.
    /// It includes options for user approval request, order cancellation request,
    /// low stock warning, order cancelled warning, and order completed notification.
    /// </remarks>
    public enum NotificationType
    {
        UserApprovalRequest,
        OrderCancellationRequest,
        LowStockWarning,
        OrderCancelledWarning,
        OrderCompletedNotification
    }
}