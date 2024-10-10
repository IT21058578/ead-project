// NotificationUtils.kt
package com.example.EAD_MOBILE.utils

import android.app.NotificationManager
import android.app.PendingIntent
import android.content.Context
import android.content.Intent
import android.media.RingtoneManager
import android.net.Uri
import androidx.core.app.NotificationCompat
import com.example.EAD_MOBILE.R
import com.example.EAD_MOBILE.windows.MainActivity

fun showNotification(context: Context, title: String, message: String) {
    val notificationId = System.currentTimeMillis().toInt()
    val channelId = "STYLE_SQUARE" // This should match the channel ID you defined

    // Intent to open when the notification is clicked
    val intent = Intent(context, MainActivity::class.java)
    val pendingIntent = PendingIntent.getActivity(
        context,
        0,
        intent,
        PendingIntent.FLAG_UPDATE_CURRENT or PendingIntent.FLAG_IMMUTABLE
    )

    // Build the notification
    val notificationBuilder = NotificationCompat.Builder(context, channelId)
        .setSmallIcon(R.drawable.logo_ecommerce) // Your notification icon
        .setContentTitle(title)
        .setContentText(message)
        .setAutoCancel(true)
        .setContentIntent(pendingIntent)
        .setPriority(NotificationCompat.PRIORITY_DEFAULT)

    // Play default notification sound
    val notificationSound: Uri = RingtoneManager.getDefaultUri(RingtoneManager.TYPE_NOTIFICATION)
    notificationBuilder.setSound(notificationSound)

    // Show the notification
    val notificationManager = context.getSystemService(Context.NOTIFICATION_SERVICE) as NotificationManager
    notificationManager.notify(notificationId, notificationBuilder.build())
}
