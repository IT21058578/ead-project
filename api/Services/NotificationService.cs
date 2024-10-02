using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Requests;
using api.Models;
using api.Repositories;
using api.Transformers;
using api.Utilities;

namespace api.Services
{
	public class NotificationService(NotificationRepository notificationRepository, ILogger<NotificationService> logger)
	{
		private readonly ILogger<NotificationService> _logger = logger;
		private readonly NotificationRepository _notificationRepository = notificationRepository;

		public Notification CreateNotification(Notification request)
		{
			_logger.LogInformation("Creating notification");
			var savedNotification = _notificationRepository.Add(request);
			_logger.LogInformation("Notification created with id {id}", savedNotification.Id);
			return savedNotification;
		}

		public void DeleteNotification(string id)
		{
			_logger.LogInformation("Deleting notification {id}", id);
			_notificationRepository.Delete(id);
			_logger.LogInformation("Notification deleted");
		}

		public Notification GetNotification(string id)
		{
			_logger.LogInformation("Getting notification {id}", id);
			var notification = _notificationRepository.GetById(id) ?? throw new Exception("Notification not found");
			_logger.LogInformation("Notification found with id {id}", notification.Id);
			return notification;
		}

		public Page<Notification> SearchNotifications(PageRequest<Notification> request)
		{
			_logger.LogInformation("Searching notifications with page {page} and page size {pageSize}", request.Page, request.PageSize);
			var notifications = _notificationRepository.GetPage(request);
			_logger.LogInformation("Found {count} notifications", notifications.Data.Count());
			return notifications;
		}
	}
}