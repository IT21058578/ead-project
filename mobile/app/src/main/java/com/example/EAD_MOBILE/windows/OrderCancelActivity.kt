package com.example.EAD_MOBILE.windows

import android.os.Bundle
import android.util.Log
import android.widget.EditText
import android.widget.TextView
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import com.example.EAD_MOBILE.R
import com.example.EAD_MOBILE.api.APIClient
import com.example.EAD_MOBILE.models.request.CancelOrderRequest
import com.example.EAD_MOBILE.models.responce.CancelOrderResponse
import com.example.EAD_MOBILE.storage.SharedPreferenceHelper
import com.example.EAD_MOBILE.ustils.Constant
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class OrderCancelActivity : AppCompatActivity() {

    private lateinit var commentEditText: EditText
    private lateinit var cancelBtn: TextView
    private lateinit var apiClient: APIClient
    private lateinit var orderId: String

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
            Log.e("OrderCancelActivity", "Order ID is missing")
            finish() // Close the activity if no orderId is provided
            return
        }

        Log.d("OrderCancelActivity", "Received Order ID: $orderId")

        // Set onClickListener for cancel button to send cancel request
        cancelBtn.setOnClickListener {
            submitCancelRequest()
        }
    }

    private fun submitCancelRequest() {
        // Retrieve the logged-in user's UserId from SharedPreferences
        val userId = SharedPreferenceHelper.getInstance(this)
            .getSharedPreferenceString(Constant.USER_ID, "")

        // Check if UserId is valid
        if (userId.isNullOrEmpty()) {
            Toast.makeText(this, "Error: User not logged in", Toast.LENGTH_SHORT).show()
            Log.e("OrderCancelActivity", "User not logged in, UserId is null or empty")
            return
        }

        // Get reason for cancellation from commentEditText
        val reason = commentEditText.text.toString()

        if (reason.isEmpty()) {
            Toast.makeText(this, "Please provide a reason for cancellation", Toast.LENGTH_SHORT).show()
            Log.e("OrderCancelActivity", "Cancellation reason is missing")
            return
        }

        // Log the orderId, userId, and reason before making the request
        Log.d("OrderCancelActivity", "OrderId: $orderId, UserId: $userId, Reason: $reason")

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
        apiClient.apiService.cancelOrder(cancelOrderRequest).enqueue(object : Callback<CancelOrderResponse> {
            override fun onResponse(call: Call<CancelOrderResponse>, response: Response<CancelOrderResponse>) {
                if (response.isSuccessful) {
                    Toast.makeText(this@OrderCancelActivity, "Order cancelled successfully", Toast.LENGTH_SHORT).show()
                    Log.d("OrderCancelActivity", "Order cancelled successfully for Order ID: ${cancelOrderRequest.orderId}")
                    finish() // Optionally finish this activity
                } else {
                    Toast.makeText(this@OrderCancelActivity, "Failed to cancel order", Toast.LENGTH_SHORT).show()
                    Log.e("OrderCancelActivity", "Failed to cancel order for Order ID: ${cancelOrderRequest.orderId}")
                }
            }

            override fun onFailure(call: Call<CancelOrderResponse>, t: Throwable) {
                Toast.makeText(this@OrderCancelActivity, "Error: ${t.message}", Toast.LENGTH_SHORT).show()
                Log.e("OrderCancelActivity", "Error during order cancellation: ${t.message}")
            }
        })
    }

    private fun clearInputFields() {
        commentEditText.text.clear() // Clear the comment EditText
    }
}
