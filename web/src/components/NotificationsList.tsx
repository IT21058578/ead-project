import React, { useState } from 'react';
import { useGetAllNotificationsQuery, useDeleteNotificationMutation } from '../store/apiquery/notificationsApiSlice';
import Swal from 'sweetalert2';
import { Notification } from '../types';

const NotificationsList = () => {
  const { isLoading, data: notifications, isSuccess, isError } = useGetAllNotificationsQuery('api/notifications');
  const [deleteNotification, deletedResult] = useDeleteNotificationMutation();

  const [showNotifications, setShowNotifications] = useState(false);

  const handleShowNotifications = () => {
    setShowNotifications(true);
    console.log(showNotifications);
  };

  const handleHideNotifications = () => {
    setShowNotifications(false);
  };

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
              <li key={notification._id} className="d-flex justify-content-between align-items-center p-2">
                <span>{notification.reason}</span>
                <button className="btn" onClick={() => deleteItem(notification._id)}>
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