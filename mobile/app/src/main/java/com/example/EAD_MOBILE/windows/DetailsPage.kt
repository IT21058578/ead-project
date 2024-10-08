package com.example.EAD_MOBILE.windows

import android.annotation.SuppressLint
import android.content.Intent
import android.graphics.Color
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.Toast
import androidx.core.view.WindowInsetsControllerCompat
import androidx.recyclerview.widget.LinearLayoutManager
import com.bumptech.glide.Glide
import com.example.EAD_MOBILE.adapter.ReviewListAdapter
import com.example.EAD_MOBILE.api.APIClient
import com.example.EAD_MOBILE.databinding.ActivityDetailsPageBinding
import com.example.EAD_MOBILE.models.request.FilterValue
import com.example.EAD_MOBILE.models.request.Filters
import com.example.EAD_MOBILE.models.request.ReviewSearchRequest
import com.example.EAD_MOBILE.models.responce.ProductSearchResponse
import com.example.EAD_MOBILE.models.response.ReviewSearchResponse
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class DetailsPage : AppCompatActivity() {

    private lateinit var binding: ActivityDetailsPageBinding
    private lateinit var productId: String

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        binding = ActivityDetailsPageBinding.inflate(layoutInflater)
        setContentView(binding.root)

        // Get product ID from the intent
        productId = intent.getStringExtra("productId") ?: ""

        // Set up RecyclerView
        setupRecyclerView()

        // Navigate to OrderPage when the button is clicked
        binding.ibOrders.setOnClickListener {
            startActivity(Intent(this, ActivityCart::class.java))
        }

        // Fetch product details using the product ID
        fetchProductDetails(productId)

        // Fetch product reviews using the product ID
        fetchProductReviews(productId)

        // Set up the appearance of the status bar
        window.statusBarColor = Color.parseColor("#FFFFFF")
        WindowInsetsControllerCompat(window, window.decorView).isAppearanceLightStatusBars = true
    }

    private fun setupRecyclerView() {
        binding.reviewsRecycle.layoutManager = LinearLayoutManager(this)
    }

    private fun fetchProductDetails(productId: String) {
        // Fetch the product details using the product ID
        APIClient(this).apiService.getProductById(productId)
            .enqueue(object : Callback<ProductSearchResponse.ProductData> {
                override fun onResponse(
                    call: Call<ProductSearchResponse.ProductData>,
                    response: Response<ProductSearchResponse.ProductData>
                ) {
                    if (response.isSuccessful && response.body() != null) {
                        val product = response.body()
                        binding.tvPrice.text = "Rs. ${product!!.price}"
                        binding.tvOrigin.text = product.category
                        Glide.with(this@DetailsPage).load(product.imageUrl).into(binding.ivProductImage)
                        binding.tvProductName.text = product.name
                        binding.tvAboutProduct.text = product.description
                        binding.ratingBar.rating = product.rating?.toFloat() ?: 0f
                    } else {
                        Toast.makeText(this@DetailsPage, "Product not found", Toast.LENGTH_SHORT).show()
                    }
                }

                override fun onFailure(call: Call<ProductSearchResponse.ProductData>, t: Throwable) {
                    Toast.makeText(this@DetailsPage, t.message, Toast.LENGTH_SHORT).show()
                }
            })
    }

    private fun fetchProductReviews(productId: String) {
        val searchReviewRequest = ReviewSearchRequest(
            page = 1,
            pageSize = 100,
            sortBy = "Id",
            sortDirection = "desc",
            filters = Filters(
                ProductId = FilterValue(
                    operator = "Equals",
                    value = productId
                )
            )
        )

        APIClient(this).apiService.searchReviews(searchReviewRequest)
            .enqueue(object : Callback<ReviewSearchResponse> {
                override fun onResponse(
                    call: Call<ReviewSearchResponse>,
                    response: Response<ReviewSearchResponse>
                ) {
                    if (response.isSuccessful && response.body() != null) {
                        val reviews = response.body()!!.reviews
                        // Set up the RecyclerView adapter
                        val reviewListAdapter = ReviewListAdapter(reviews!!)
                        binding.reviewsRecycle.adapter = reviewListAdapter
                    } else {
                        Toast.makeText(this@DetailsPage, "No reviews found", Toast.LENGTH_SHORT).show()
                    }
                }

                override fun onFailure(call: Call<ReviewSearchResponse>, t: Throwable) {
                    Toast.makeText(this@DetailsPage, t.message, Toast.LENGTH_SHORT).show()
                }
            })
    }

    @SuppressLint("SetTextI18n")
    private fun setUpData(product: ProductSearchResponse.ProductData) {
        // Set up the product data in the UI
        binding.tvPrice.text = "Rs. ${product.price}"
        binding.tvOrigin.text = product.category
        Glide.with(this).load(product.imageUrl).into(binding.ivProductImage)
        binding.tvProductName.text = product.name
    }
}
