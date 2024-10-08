package com.example.EAD_MOBILE.windows

import android.content.Intent
import android.os.Bundle
import android.os.StatFs
import android.text.InputType
import android.util.Log
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import com.example.EAD_MOBILE.R
import com.example.EAD_MOBILE.api.APIClient
import com.example.EAD_MOBILE.databinding.ActivitySignUpBinding
import com.example.EAD_MOBILE.models.request.RegisterRequest
import com.example.EAD_MOBILE.models.responce.RegisterResponse
import com.example.EAD_MOBILE.ustils.Utility
import com.example.EAD_MOBILE.ustils.Utility.logPrint
import okhttp3.ResponseBody
import org.json.JSONObject
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class SignUpActivity : AppCompatActivity() {

    private lateinit var binding: ActivitySignUpBinding
    private lateinit var apiClient: APIClient
    private val TAG = "SignUpActivity"
    private var isPasswordVisible = false

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivitySignUpBinding.inflate(layoutInflater)
        setContentView(binding.root)

        // Initialize APIClient
        apiClient = APIClient(applicationContext)

        binding.signUpBtn.setOnClickListener {
            val email = binding.userIDInput.text.toString() // email input field
            val firstName = binding.firstNameInput.text.toString() // first name input field
            val lastName = binding.lastNameInput.text.toString() // last name input field
            val password = binding.passLog.text.toString() // password input field

            // Validate input fields
            if (email.isEmpty() || firstName.isEmpty() || lastName.isEmpty() || password.isEmpty()) {
                showToast("Please fill out all fields")
            } else {
                registerUser(email, password, firstName, lastName)
            }
        }

        // Show/Hide password toggle click listener
        binding.recheckShowHidePasswordToggleLogging.setOnClickListener {
            togglePasswordVisibility()
        }

        // Navigate to the login page
        binding.forgotNavBtn.setOnClickListener {
            startActivity(Intent(this, LoginActivity::class.java))
        }
    }
    private fun registerUser(email: String, password: String, firstName: String, lastName: String) {
        val registerRequest = RegisterRequest(email, password, firstName, lastName)
        val call = apiClient.apiService.registerUser(registerRequest)

        // Show progress UI
        binding.loginLayout3.visibility = android.view.View.VISIBLE
        binding.loginLayout1.visibility = android.view.View.GONE

        // Make the API call
        call.enqueue(object : Callback<ResponseBody> {
            override fun onResponse(call: Call<ResponseBody>, response: Response<ResponseBody>) {
                binding.loginLayout3.visibility = android.view.View.GONE
                binding.loginLayout1.visibility = android.view.View.VISIBLE

                // Embedding try-catch block for error and success handling
                val result = response.body()

                try {
                    logPrint("Signup Error" +response.code().toString())
                    // Handle API success responses (200, 201)
                    if (response.isSuccessful && result != null) {
                        // Log and show success message
                        Log.d(TAG, "Registration successful!")
                        showToast("Registration successful!")

                        // Navigate to success page
                        val intent = Intent(this@SignUpActivity, SignUpSuccessPage::class.java)
                        intent.putExtra("USER_EMAIL",email)
                        startActivity(intent)
                        finish()  // Prevent going back to this activity

                        clearInputFields()

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
                binding.loginLayout3.visibility = android.view.View.GONE
                binding.loginLayout1.visibility = android.view.View.VISIBLE
                showToast("Network Error: ${t.message}")
                Log.e(TAG, "Network failure: ${t.message}")
                clearInputFields()
            }
        })
    }

    private fun clearInputFields() {
        binding.userIDInput.text.clear()
        binding.firstNameInput.text.clear()
        binding.lastNameInput.text.clear()
        binding.passLog.text.clear()
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

    private fun togglePasswordVisibility() {
        if (isPasswordVisible) {
            // Hide the password
            binding.passLog.inputType = InputType.TYPE_CLASS_TEXT or InputType.TYPE_TEXT_VARIATION_PASSWORD
            binding.recheckShowHidePasswordToggleLogging.setImageResource(R.drawable.iconoir_eye_off) // Change to eye icon
        } else {
            // Show the password
            binding.passLog.inputType = InputType.TYPE_CLASS_TEXT or InputType.TYPE_TEXT_VARIATION_VISIBLE_PASSWORD
            binding.recheckShowHidePasswordToggleLogging.setImageResource(R.drawable.iconoir_eye) // Change to eye-off icon
        }
        isPasswordVisible = !isPasswordVisible

        // Move the cursor to the end of the password input field
        binding.passLog.setSelection(binding.passLog.text.length)
    }

    private fun showToast(message: String) {
        Toast.makeText(this, message, Toast.LENGTH_LONG).show()
        Log.d(TAG, message)
    }
}
