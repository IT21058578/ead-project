import React, { useState } from 'react';
import { useGetAllNotificationsQuery, useDeleteNotificationMutation } from '../store/apiquery/notificationsApiSlice';
import Swal from 'sweetalert2';
import { Notification } from '../types';

/**
 * NotificationsList component
 * 
 * This component renders a notification button and a list of notifications.
 * The button shows the number of notifications and when clicked, it shows the list of notifications.
 * The list of notifications includes the notification reason and a delete button.
 * When the delete button is clicked, it shows a confirmation dialog to confirm the deletion of the notification.
 * If the deletion is confirmed, it calls the deleteNotification function with the given id.
 * @returns {JSX.Element} - a React node that renders the notification button and the list of notifications.
 */
const NotificationsList = () => {
  const { isLoading, data: notifications, isSuccess, isError } = useGetAllNotificationsQuery('api/notifications');
  const [deleteNotification, deletedResult] = useDeleteNotificationMutation();

  const [showNotifications, setShowNotifications] = useState(false);

  /**
   * Shows the list of notifications when called.
   * It sets the showNotifications state to true.
   * It also logs the value of showNotifications to the console.
   */
  const handleShowNotifications = () => {
    setShowNotifications(true);
    console.log(showNotifications);
  };

  /**
   * Hides the list of notifications when called.
   * It sets the showNotifications state to false.
   */
  const handleHideNotifications = () => {
    setShowNotifications(false);
  };

  /**
   * Deletes a notification with the given id.
   * It shows a confirmation dialog with a warning icon and asks the user if they are sure to delete the notification.
   * If the user confirms, it calls the deleteNotification function with the given id.
   * @param {string} id - the id of the notification to delete
   */
  const deleteItem = (id: string) => {
    Swal.fire({
      title: 'Are you sure?',
      text: "Are you sure to delete this notification?",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes, delete it!'
    }).then((r) => {
      if (r.isConfirmed) {
        deleteNotification(id);
      }
    });
  };

  return (
    <div>
      <a href="#" className="position-relative border-3 shadow border-light py-2 px-3 text-dark fd-hover-bg-primary" onClick={handleShowNotifications}>
        <i className="bi bi-bell"></i>
        {notifications?.data && notifications?.data.length > 0 && (
          <span className="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">
            {notifications?.data.length}
          </span>
        )}
      </a>

      {showNotifications && (
        <div className="position-absolute top-50 start-50 translate-middle p-4 border bg-white shadow" style={{ width: '500px' }}>
          <h5>Notifications</h5>
          <ul className="list-group">
            {notifications?.data && notifications?.data.map((notification: Notification) => (
              <li key={notification.id} className="d-flex justify-content-between align-items-center p-2">
                <span>{notification.reason}</span>
                <button className="btn" onClick={() => deleteItem(notification.id)}>
                  <i className="bi bi-x-circle text-danger"></i>
                </button>
              </li>
            ))}
          </ul>
          <button className="btn btn-sm btn-secondary" onClick={handleHideNotifications}>Close</button>
        </div>
      )}
    </div>
  );
};

export default NotificationsList;