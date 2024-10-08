package com.example.EAD_MOBILE

import android.app.Application
import android.app.NotificationChannel
import android.app.NotificationManager
import android.os.Build
import com.example.EAD_MOBILE.data.database.EADAppDataBase
import com.example.EAD_MOBILE.data.database.repo.CartRepository
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.SupervisorJob

class MyApplication : Application() {

    val applicationScope = CoroutineScope(SupervisorJob())

    val database by lazy { EADAppDataBase.getDatabase(this, applicationScope) }
    val cartRepository by lazy { CartRepository(database.cartDao()) }

    override fun onCreate() {
        super.onCreate()
        createNotificationChannel() // Create the notification channel when the application starts
    }

    private fun createNotificationChannel() {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            val channelId = "STYLE_SQUARE"
            val channelName = "Style Square"
            val channelDescription = "Style Square is e-commerce platform" // Description for the channel
            val channelImportance = NotificationManager.IMPORTANCE_DEFAULT // Set the importance level

            // Create the notification channel
            val channel = NotificationChannel(channelId, channelName, channelImportance).apply {
                description = channelDescription
            }

            // Register the channel with the system
            val notificationManager = getSystemService(NotificationManager::class.java)
            notificationManager.createNotificationChannel(channel)
        }
    }


}