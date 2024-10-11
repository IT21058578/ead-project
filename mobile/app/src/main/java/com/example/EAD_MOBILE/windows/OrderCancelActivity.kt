package com.example.EAD_MOBILE.windows

import android.Manifest
import android.app.NotificationChannel
import android.app.NotificationManager
import android.app.PendingIntent
import android.content.Context
import android.content.pm.PackageManager
import android.graphics.Color
import android.os.Build
import android.os.Bundle
import android.util.Log
import android.widget.EditText
import android.widget.TextView
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.core.app.ActivityCompat
import androidx.core.app.NotificationCompat
import androidx.core.content.ContextCompat
import com.example.EAD_MOBILE.R
import com.example.EAD_MOBILE.api.APIClient
import com.example.EAD_MOBILE.models.request.CancelOrderRequest
import com.example.EAD_MOBILE.storage.SharedPreferenceHelper
import com.example.EAD_MOBILE.ustils.Constant
import com.example.EAD_MOBILE.ustils.Utility
import okhttp3.ResponseBody
import org.json.JSONObject
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class OrderCancelActivity : AppCompatActivity() {

    private lateinit var commentEditText: EditText
    private lateinit var cancelBtn: TextView
    private lateinit var apiClient: APIClient
    private lateinit var orderId: String
    private val TAG = "OrderCancelActivity"

    private val NOTIFICATION_PERMISSION_REQUEST_CODE = 1001
    private val CHANNEL_ID = "STYLE_SQUARE"
    private val CHANNEL_NAME = "Style Square Notifications"

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_order_cancel)

        // Initialize the APIClient
        apiClient = APIClient(applicationContext)

        // Initialize views
        commentEditText = findViewById(R.id.comment_editText)
        cancelBtn = findViewById(R.id.cancel_btn)

        // Retrieve orderId from Intent
        orderId = intent.getStringExtra("ORDER_ID") ?: ""

        // Ensure that orderId is not null or empty
        if (orderId.isEmpty()) {
            Toast.makeText(this, "Error: Order ID is missing", Toast.LENGTH_SHORT).show()
            Log.e(TAG, "Order ID is missing")
            finish() // Close the activity if no orderId is provided
            return
        }

        Log.d(TAG, "Received Order ID: $orderId")

        // Check and request notification permission for Android 13+
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.TIRAMISU) {
            if (ContextCompat.checkSelfPermission(
                    this,
                    Manifest.permission.POST_NOTIFICATIONS
                ) != PackageManager.PERMISSION_GRANTED
            ) {
                ActivityCompat.requestPermissions(
                    this,
                    arrayOf(Manifest.permission.POST_NOTIFICATIONS),
                    NOTIFICATION_PERMISSION_REQUEST_CODE
                )
            }
        }

        // Create notification channel
        createNotificationChannel()

        // Set onClickListener for cancel button to send cancel request
        cancelBtn.setOnClickListener {
            submitCancelRequest()
        }
    }

    // Handle the permission request result
    override fun onRequestPermissionsResult(
        requestCode: Int,
        permissions: Array<out String>,
        grantResults: IntArray
    ) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
        if (requestCode == NOTIFICATION_PERMISSION_REQUEST_CODE) {
            if ((grantResults.isNotEmpty() && grantResults[0] == PackageManager.PERMISSION_GRANTED)) {
                // Permission granted
                Toast.makeText(this, "Notification permission granted", Toast.LENGTH_SHORT).show()
            } else {
                // Permission denied
                Toast.makeText(this, "Notification permission denied", Toast.LENGTH_SHORT).show()
            }
        }
    }

    private fun submitCancelRequest() {
        // Retrieve the logged-in user's UserId from SharedPreferences
        val userId = SharedPreferenceHelper.getInstance(this)
            .getSharedPreferenceString(Constant.USER_ID, "")

        // Check if UserId is valid
        if (userId.isNullOrEmpty()) {
            Toast.makeText(this, "Error: User not logged in", Toast.LENGTH_SHORT).show()
            Log.e(TAG, "User not logged in, UserId is null or empty")
            return
        }

        // Get reason for cancellation from commentEditText
        val reason = commentEditText.text.toString()

        if (reason.isEmpty()) {
            Toast.makeText(this, "Please provide a reason for cancellation", Toast.LENGTH_SHORT)
                .show()
            Log.e(TAG, "Cancellation reason is missing")
            return
        }

        // Log the orderId, userId, and reason before making the request
        Log.d(TAG, "OrderId: $orderId, UserId: $userId, Reason: $reason")

        // Create an instance of CancelOrderRequest and set its properties
        val cancelOrderRequest = CancelOrderRequest().apply {
            this.orderId = this@OrderCancelActivity.orderId
            this.userId = userId
            this.reason = reason
        }

        // Call the API to cancel the order
        cancelOrder(cancelOrderRequest)
        clearInputFields()
    }

    private fun cancelOrder(cancelOrderRequest: CancelOrderRequest) {
        // Make the API call to cancel the order
        apiClient.apiService.cancelOrder(cancelOrderRequest)
            .enqueue(object : Callback<ResponseBody> {
                override fun onResponse(
                    call: Call<ResponseBody>,
                    response: Response<ResponseBody>
                ) {
                    val result = response.body()
                    try {
                        Log.d(TAG, "Cancel Order Response Code: ${response.code()}")
                        if (response.isSuccessful && result != null) {
                            // Handle success
                            Toast.makeText(
                                this@OrderCancelActivity,
                                "Order cancelled successfully",
                                Toast.LENGTH_SHORT
                            ).show()
                            Log.d(
                                TAG,
                                "Order cancelled successfully for Order ID: ${cancelOrderRequest.orderId}"
                            )

                            // Trigger notification
                            val notificationTitle = "Order Cancelled"
                            val notificationMessage = "Your cancellation request  for the order ${cancelOrderRequest.orderId}  has been send successfully."
                            showNotification(

                                notificationTitle,
                                notificationMessage
                            )



                            finish()
                        } else {
                            // Handle API error responses
                            val errorBody = response.errorBody()?.string().orEmpty()
                            val errorMessage = Utility.getErrorResponse(errorBody)
                            Log.d(TAG, "Error response: $errorMessage")
                            Toast.makeText(
                                this@OrderCancelActivity,
                                "Failed to cancel order: $errorMessage",
                                Toast.LENGTH_SHORT
                            ).show()
                            handleError(errorMessage)
                        }
                    } catch (e: Exception) {
                        // Handle exceptions
                        Log.e(TAG, "Exception in response handling: ${e.message}")
                        Toast.makeText(
                            this@OrderCancelActivity,
                            "An error occurred: ${e.message}",
                            Toast.LENGTH_SHORT
                        ).show()
                    }
                }

                override fun onFailure(call: Call<ResponseBody>, t: Throwable) {
                    // Handle failure
                    Toast.makeText(
                        this@OrderCancelActivity,
                        "Network Error: ${t.message}",
                        Toast.LENGTH_SHORT
                    ).show()
                    Log.e(TAG, "Error during order cancellation: ${t.message}")
                }
            })
    }

    private fun handleError(errorMessage: String) {
        try {
            val jsonObject = JSONObject(errorMessage)
            // Check for specific error message
            if (jsonObject.has("detail")) {
                val detailMessage = jsonObject.getString("detail")
                Toast.makeText(this, detailMessage, Toast.LENGTH_SHORT).show()
            } else {
                Toast.makeText(this, "Cancellation failed: $errorMessage", Toast.LENGTH_SHORT)
                    .show()
            }
        } catch (e: Exception) {
            Toast.makeText(this, "Error parsing error message", Toast.LENGTH_SHORT).show()
        }
    }

    private fun clearInputFields() {
        commentEditText.text.clear() // Clear the comment EditText
    }

    private fun createNotificationChannel() {
        // Create the NotificationChannel, but only on API 26+ (Android 8.0 and above)
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            val channelDescription = "Notifications for order updates"
            val channelImportance = NotificationManager.IMPORTANCE_HIGH

            val channel = NotificationChannel(CHANNEL_ID, CHANNEL_NAME, channelImportance).apply {
                description = channelDescription
                enableLights(true)
                lightColor = Color.GREEN
                enableVibration(true)
            }

            // Register the channel with the system
            val notificationManager: NotificationManager =
                getSystemService(Context.NOTIFICATION_SERVICE) as NotificationManager
            notificationManager.createNotificationChannel(channel)
        }
    }



    private fun showNotification(title: String, message: String) {
        val notificationId = System.currentTimeMillis().toInt()

        // Intent to open when the notification is clicked
        val intent = intent // Reuse the existing intent
        val pendingIntent =
            PendingIntent.getActivity(
                this,
                0,
                intent,
                PendingIntent.FLAG_UPDATE_CURRENT or PendingIntent.FLAG_IMMUTABLE
            )

        // Build the notification
        val notificationBuilder = NotificationCompat.Builder(this, CHANNEL_ID)
            .setSmallIcon(R.drawable.logo_ecommerce)
            .setContentTitle(title)
            .setContentText(message)
            .setAutoCancel(true)
            .setContentIntent(pendingIntent)
            .setPriority(NotificationCompat.PRIORITY_HIGH)
            .setDefaults(NotificationCompat.DEFAULT_ALL)

        // Show the notification
        val notificationManager = getSystemService(Context.NOTIFICATION_SERVICE) as NotificationManager
        notificationManager.notify(notificationId, notificationBuilder.build())
    }
}
