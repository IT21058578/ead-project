
using api.DTOs.Requests;
using api.Exceptions;
using api.Models;
using api.Repositories;
using api.Transformers;
using api.Utilities;

namespace api.Services
{
	/// <summary>
	/// The NotificationService class provides methods for managing notifications.
	/// </summary>
	/// 
	/// <remarks>
	/// The NotificationService class is responsible for creating, updating, deleting, and retrieving notifications.
	/// It interacts with the NotificationRepository to perform database operations.
	/// </remarks>
	public class NotificationService(NotificationRepository notificationRepository, ILogger<NotificationService> logger)
	{
		private readonly ILogger<NotificationService> _logger = logger;
		private readonly NotificationRepository _notificationRepository = notificationRepository;

		// This is a method for creating a notification
		public Notification CreateNotification(Notification notification)
		{
			_logger.LogInformation("Creating notification");
			var savedNotification = _notificationRepository.Add(notification);
			_logger.LogInformation("Notification created with id {id}", savedNotification.Id);
			return savedNotification;
		}

		// This is a method for updating a notification
		public Notification CreateNotification(CreateOrderCancellationRequestDto request)
		{
			_logger.LogInformation("Creating notification for order cancellation request");
			var notification = request.ToModel();
			var savedNotification = _notificationRepository.Add(notification);
			_logger.LogInformation("Notification created with id {id}", savedNotification.Id);
			return savedNotification;
		}

		// This is a method for updating a notification
		public Notification UpdateNotification(string id, UpdateNotificationRequestDto request)
		{
			_logger.LogInformation("Updating notification {id}", id);
			var notification = GetNotification(id);
			notification = request.ToModel(notification);
			var updatedNotification = _notificationRepository.Update(notification);
			_logger.LogInformation("Notification updated with id {id}", updatedNotification.Id);
			return updatedNotification;
		}

		// This is a method for deleting a notification
		public void DeleteNotification(string id)
		{
			_logger.LogInformation("Deleting notification {id}", id);
			_notificationRepository.Delete(id);
			_logger.LogInformation("Notification deleted");
		}

		// This is a method for getting a notification
		public Notification GetNotification(string id)
		{
			_logger.LogInformation("Getting notification {id}", id);
			var notification = _notificationRepository.GetById(id) ?? throw new NotFoundException($"Notification with id ${id} not found");
			_logger.LogInformation("Notification found with id {id}", notification.Id);
			return notification;
		}

		// This is a method for searching notifications
		public Page<Notification> SearchNotifications(PageRequest<Notification> request)
		{
			_logger.LogInformation("Searching notifications with page {page} and page size {pageSize}", request.Page, request.PageSize);
			var notifications = _notificationRepository.GetPage(request);
			_logger.LogInformation("Found {count} notifications", notifications.Data.Count());
			return notifications;
		}
	}
}