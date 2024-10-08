package com.example.EAD_MOBILE.adapter

import android.util.Log
import android.view.LayoutInflater
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView
import com.example.EAD_MOBILE.databinding.NotificationItemviewBinding
import com.example.EAD_MOBILE.models.responce.Notification

class NotificationListAdapter(private val listOfNotifications: List<Notification>) :
    RecyclerView.Adapter<NotificationListAdapter.ViewHolder>() {

    class ViewHolder(private val binding: NotificationItemviewBinding) : RecyclerView.ViewHolder(binding.root) {
        fun bind(notification: Notification) {
            // Display the notification message
            binding.tvNotificationMessage.text = notification.reason
            // Display the notification time
            binding.tvNotificationTime.text = notification.createdAt.take(10)

            // Log the notification details for debugging
            Log.d("NotificationListAdapter", "Notification ID: ${notification.id}")
            Log.d("NotificationListAdapter", "Message: ${notification.reason}")
            Log.d("NotificationListAdapter", "Created At: ${notification.createdAt}")
        }
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ViewHolder {
        // Inflate the notification item layout using the binding class
        val binding = NotificationItemviewBinding.inflate(
            LayoutInflater.from(parent.context), parent, false
        )
        return ViewHolder(binding)
    }

    override fun getItemCount(): Int {
        return listOfNotifications.size
    }

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        val notification = listOfNotifications[position]
        holder.bind(notification)
    }
}
