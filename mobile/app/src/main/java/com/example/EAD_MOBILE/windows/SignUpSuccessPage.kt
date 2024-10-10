package com.example.EAD_MOBILE.windows

import android.content.Context
import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.util.Log
import android.widget.Toast
import com.example.EAD_MOBILE.api.APIClient
import com.example.EAD_MOBILE.databinding.ActivitySignUpSuccessPageBinding
import com.example.EAD_MOBILE.ustils.Constant.Companion.TAG
import com.example.EAD_MOBILE.ustils.Utility
import com.example.EAD_MOBILE.ustils.Utility.logPrint
import okhttp3.ResponseBody
import org.json.JSONObject
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class SignUpSuccessPage : AppCompatActivity() {
    private lateinit var apiClient: APIClient
    private lateinit var binding: ActivitySignUpSuccessPageBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        // Initialize the binding
        binding = ActivitySignUpSuccessPageBinding.inflate(layoutInflater)
        setContentView(binding.root)

        // Initialize the API client
        apiClient = APIClient(applicationContext)

        // Now you can safely access views through binding
        val email = intent.getStringExtra("USER_EMAIL")
        binding.userEmail.text = email

        binding.verifyBtn.setOnClickListener {
            val code = binding.pinViewOTP.text.toString()
            verifyUser(code, email)
        }
    }

    private fun verifyUser(code: String, email: String?) {
        if (email == null) {
            showToast("Email is required")
            return
        }

        val call = apiClient.apiService.verifyUser(code, email)

        // Make the API call
        call.enqueue(object : Callback<ResponseBody> {
            override fun onResponse(call: Call<ResponseBody>, response: Response<ResponseBody>) {
                // Embedding try-catch block for error and success handling
                val result = response.body()

                try {
                    logPrint("Verify Error: " + response.code().toString())
                    // Handle API success responses (200, 201)
                    if (response.isSuccessful && result != null) {
                        // Log and show success message
                        Log.d(TAG, "Verify successful!")
                        showToast("Verify successful!")

                        // Navigate to success page
                        startActivity(Intent(this@SignUpSuccessPage, LoginActivity::class.java))
                        finish()  // Prevent going back to this activity
                    } else {
                        // Handle API error responses (404, 501, etc.)
                        val errorMessage = Utility.getErrorResponse(response.errorBody()?.string().orEmpty())
                        Log.d(TAG, "Error response: $errorMessage")
                        showToast("Error: $errorMessage")
                        handleError(errorMessage)
                    }
                } catch (e: Exception) {
                    // Handle exceptions that may occur due to misconfiguration or unexpected issues
                    Log.e(TAG, "Exception in response handling: ${e.message}")
                    showToast("An error occurred: ${e.message}")
                }
            }

            override fun onFailure(call: Call<ResponseBody>, t: Throwable) {
                // Hide loading progress and show error message for network issues
                showToast("Network Error: ${t.message}")
                Log.e(TAG, "Network failure: ${t.message}")
            }
        })
    }

    private fun handleError(errorMessage: String) {
        try {
            val jsonObject = JSONObject(errorMessage)
            // Check for specific error message
            if (jsonObject.has("detail")) {
                val detailMessage = jsonObject.getString("detail")
                showToast(detailMessage)
            } else {
                showToast("Registration failed: $errorMessage")
            }
        } catch (e: Exception) {
            showToast("Error parsing error message")
        }
    }

    private fun showToast(message: String) {
        Toast.makeText(this, message, Toast.LENGTH_LONG).show()
        Log.d(TAG, message)
    }
}
