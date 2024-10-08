package com.example.EAD_MOBILE.windows

import ReviewRequest
import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.util.Log
import android.widget.EditText
import android.widget.ImageView
import android.widget.RatingBar
import android.widget.TextView
import android.widget.Toast
import com.example.EAD_MOBILE.R
import com.example.EAD_MOBILE.api.APIClient
import com.example.EAD_MOBILE.models.responce.ReviewResponse
import com.example.EAD_MOBILE.storage.SharedPreferenceHelper
import com.example.EAD_MOBILE.ustils.Constant
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class AddReviewActivity : AppCompatActivity() {

    private lateinit var ratingBar: RatingBar
    private lateinit var ivMinusBtn: ImageView
    private lateinit var ivAddBtn: ImageView
    private lateinit var tvQuantity: TextView
    private lateinit var commentEditText: EditText
    private lateinit var reviewBtn: TextView
    private var quantity: Int = 0

    private lateinit var vendorId: String
    private lateinit var productId: String
    private lateinit var apiClient: APIClient

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_add_review)

        // Initialize the APIClient
        apiClient = APIClient(applicationContext)

        // Initialize views
        ratingBar = findViewById(R.id.ratingBar)
        ivMinusBtn = findViewById(R.id.iv_minus_btn)
        ivAddBtn = findViewById(R.id.iv_add_btn)
        tvQuantity = findViewById(R.id.tv_quantity)
        commentEditText = findViewById(R.id.comment_editText)
        reviewBtn = findViewById(R.id.review_btn)



        // Set initial quantity
        tvQuantity.text = quantity.toString()

        // Set onClickListener for minus button
        ivMinusBtn.setOnClickListener {
            if (quantity > 0) {
                quantity--
                updateQuantityAndRating()
            }
        }

        // Set onClickListener for add button
        ivAddBtn.setOnClickListener {
            if (quantity < 5) { // Assuming max rating is 5
                quantity++
                updateQuantityAndRating()
            }
        }

        // Set onClickListener for review button to send review
        reviewBtn.setOnClickListener {
            submitReview()
        }
    }

    private fun updateQuantityAndRating() {
        tvQuantity.text = quantity.toString()
        ratingBar.rating = quantity.toFloat()
    }

    private fun submitReview() {
        // Retrieve the logged-in user's UserId from SharedPreferences
        val userId = SharedPreferenceHelper.getInstance(this)
            .getSharedPreferenceString(Constant.USER_ID, "")
        vendorId = intent.getStringExtra("VENDOR_ID") ?: ""
        productId = intent.getStringExtra("PRODUCT_ID") ?: ""
        Log.d("VEDORID:" ,"${vendorId}, ${productId}")

        // Check if UserId is valid
        if (userId.isNullOrEmpty()) {
            Toast.makeText(this, "Error: User not logged in", Toast.LENGTH_SHORT).show()
            return
        }

        // Create an instance of ReviewRequest
        val reviewRequest = ReviewRequest().apply {
            this.vendorId = this@AddReviewActivity.vendorId // Assign the vendorId from the activity
            this.productId = this@AddReviewActivity.productId // Assign the productId from the activity
            this.message = commentEditText.text.toString() // Get comment from EditText
            this.rating = quantity // Set the rating
            this.userId = userId // Set userId from SharedPreferences
            Log.d("reviewRequest", "Vendor ID: ${this.vendorId}, Product ID: ${this.productId}, Message: $message")
        }


        // Call the API to post the review
        postReview(reviewRequest)

        // Clear input fields
        clearInputFields()
    }

    private fun postReview(reviewRequest: ReviewRequest) {


        // Make the API call to post the review
        apiClient.apiService.postReview(reviewRequest).enqueue(object : Callback<ReviewResponse> {
            override fun onResponse(call: Call<ReviewResponse>, response: Response<ReviewResponse>) {
                if (response.isSuccessful) {
                    Toast.makeText(this@AddReviewActivity, "Review submitted successfully", Toast.LENGTH_SHORT).show()
                    // Navigate to the success page
                    startActivity(Intent(this@AddReviewActivity, ReviewSuccessPage::class.java))
                    finish() // Optionally finish this activity
                } else {
                    Toast.makeText(this@AddReviewActivity, "Failed to submit review", Toast.LENGTH_SHORT).show()
                }
            }

            override fun onFailure(call: Call<ReviewResponse>, t: Throwable) {
                Toast.makeText(this@AddReviewActivity, "Error: ${t.message}", Toast.LENGTH_SHORT).show()
            }
        })
    }

    private fun clearInputFields() {
        commentEditText.text.clear() // Clear the comment EditText
        quantity = 0 // Reset quantity
        tvQuantity.text = quantity.toString() // Update the quantity TextView
        ratingBar.rating = 0f // Reset the RatingBar
    }
}
