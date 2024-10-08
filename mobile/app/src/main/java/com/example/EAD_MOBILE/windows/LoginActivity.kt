// LoginActivity.kt
package com.example.EAD_MOBILE.windows

import android.content.Intent
import android.os.Bundle
import android.text.InputType
import android.util.Log
import android.view.View
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import com.example.EAD_MOBILE.R
import com.example.EAD_MOBILE.api.APIClient
import com.example.EAD_MOBILE.databinding.ActivityLoginBinding
import com.example.EAD_MOBILE.models.request.LoginRequest
import com.example.EAD_MOBILE.models.responce.LoginResponse
import com.example.EAD_MOBILE.storage.SharedPreferenceHelper
import com.example.EAD_MOBILE.ustils.Constant
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class LoginActivity : AppCompatActivity() {

    private lateinit var binding: ActivityLoginBinding
    private lateinit var apiClient: APIClient
    private val TAG = "LoginActivity"
    private var isPasswordVisible = false

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityLoginBinding.inflate(layoutInflater)
        setContentView(binding.root)

        // Initialize APIClient
        apiClient = APIClient(applicationContext)

        // Login button click listener
        binding.loginBtn.setOnClickListener {
            val email = binding.userIDEmail.text.toString()
            val password = binding.userIDPasswrod.text.toString()

            if (email.isEmpty() || password.isEmpty()) {
                showToast("Please enter email and password")
            } else {
                loginUser(email, password)
            }
        }

        // Show/Hide password toggle click listener
        binding.showHidePasswordToggleLogging.setOnClickListener {
            togglePasswordVisibility()
        }

        // Forgot button click listener to navigate to signup page
        binding.forgotNavBtn.setOnClickListener {
            startActivity(Intent(this, SignUpActivity::class.java))
        }
    }

    private fun loginUser(email: String, password: String) {
        // Show the progress bar layout
        binding.loginLayout3.visibility = View.VISIBLE
        binding.loginLayout1.visibility = View.GONE

        val loginRequest = LoginRequest(email, password)
        val call = apiClient.apiService.loginUser(loginRequest)

        call.enqueue(object : Callback<LoginResponse> {
            override fun onResponse(call: Call<LoginResponse>, response: Response<LoginResponse>) {
                // Hide the progress bar layout
                binding.loginLayout3.visibility = View.GONE
                binding.loginLayout1.visibility = View.VISIBLE

                if (response.isSuccessful) {
                    // Handle successful login
                    val loginResponse = response.body()
                    showToast("Login successful!")



                    // Navigate to MainActivity after successful login
                    startActivity(Intent(this@LoginActivity, MainActivity::class.java))
                    SharedPreferenceHelper.getInstance(applicationContext).setSharedPreferenceString(Constant.USER_ID, loginResponse?.id!!)
                    SharedPreferenceHelper.getInstance(applicationContext).setSharedPreferenceBoolean(Constant.IS_LOGGED_IN, true)
                    SharedPreferenceHelper.getInstance(applicationContext).setSharedPreferenceString(Constant.ACCESS_TOKEN, response.body()?.accessToken!!)
                    SharedPreferenceHelper.getInstance(applicationContext).setSharedPreferenceString(Constant.REFRESH_TOKEN, response.body()?.refreshToken!!)

                    // Optionally, log the access token and user info
                    Log.d(TAG, "AccessToken: ${loginResponse?.accessToken}")
                    Log.d(TAG, "User: ${loginResponse?.firstName} ${loginResponse?.lastName}")
                    Log.e(TAG,"ID:  ${loginResponse?.id}")
                } else {
                    // Handle login failure
                    val errorMessage = response.errorBody()?.string() ?: "Unknown error occurred"
                    showToast("Login failed")
                    Log.d(TAG, "Login error body: $errorMessage")
                }
            }

            override fun onFailure(call: Call<LoginResponse>, t: Throwable) {
                // Hide the progress bar layout
                binding.loginLayout3.visibility = View.GONE
                binding.loginLayout1.visibility = View.VISIBLE

                // Handle network failure
                showToast("Error, please try again in a moment")
                Log.d(TAG, "Network error: ${t.message}")
            }
        })
    }


    private fun togglePasswordVisibility() {
        if (isPasswordVisible) {
            // Hide the password
            binding.userIDPasswrod.inputType = InputType.TYPE_CLASS_TEXT or InputType.TYPE_TEXT_VARIATION_PASSWORD
            binding.showHidePasswordToggleLogging.setImageResource(R.drawable.iconoir_eye_off) // Change to eye icon
        } else {
            // Show the password
            binding.userIDPasswrod.inputType = InputType.TYPE_CLASS_TEXT or InputType.TYPE_TEXT_VARIATION_VISIBLE_PASSWORD
            binding.showHidePasswordToggleLogging.setImageResource(R.drawable.iconoir_eye) // Change to eye-off icon
        }
        isPasswordVisible = !isPasswordVisible

        // Move the cursor to the end of the password input field
        binding.userIDPasswrod.setSelection(binding.userIDPasswrod.text.length)
    }

    private fun showToast(message: String) {
        Toast.makeText(this, message, Toast.LENGTH_LONG).show()
        Log.d(TAG, message)
    }
}
