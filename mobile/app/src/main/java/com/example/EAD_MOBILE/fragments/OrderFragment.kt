package com.example.EAD_MOBILE.fragments

import OrderListAdapter
import android.graphics.Color
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.core.view.WindowInsetsControllerCompat
import androidx.fragment.app.Fragment
import androidx.recyclerview.widget.LinearLayoutManager
import com.example.EAD_MOBILE.api.APIClient
import com.example.EAD_MOBILE.models.response.OrderSearchResponse
import com.example.EAD_MOBILE.databinding.FragmentOrderBinding
import com.example.EAD_MOBILE.models.request.Filter
import com.example.EAD_MOBILE.models.request.OrderSearchRequest
import com.example.EAD_MOBILE.models.responce.ProductSearchResponse
import com.example.EAD_MOBILE.storage.SharedPreferenceHelper
import com.example.EAD_MOBILE.ustils.Constant
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class OrderFragment : Fragment() {

    private lateinit var binding: FragmentOrderBinding
    private lateinit var apiClient: APIClient

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        binding = FragmentOrderBinding.inflate(inflater, container, false)
        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        // Initialize API client
        apiClient = APIClient(requireContext())

        // Set up RecyclerView layout
        binding.rvOrderList.layoutManager = LinearLayoutManager(requireContext())

        // Customize status bar
        requireActivity().window.statusBarColor = Color.parseColor("#FFFFFF")
        WindowInsetsControllerCompat(requireActivity().window, requireActivity().window.decorView)
            .isAppearanceLightStatusBars = true

        // Load orders from API
        loadOrders()

        // Set up click listeners for chips
        binding.chipAll.setOnClickListener { loadOrders() }
        binding.chipDelivered.setOnClickListener { filterOrders("Delivered") }
        binding.chipCenceled.setOnClickListener { filterOrders("Canceled") }
        binding.chipPending.setOnClickListener { filterOrders("Pending") }
    }

    private fun loadOrders() {
        // Retrieve the logged-in user's UserId from SharedPreferences
        val userId = SharedPreferenceHelper.getInstance(requireContext())
            .getSharedPreferenceString(Constant.USER_ID, "")

        // Check if UserId is valid
        if (userId.isNullOrEmpty()) {
            Toast.makeText(requireContext(), "Error: User not logged in", Toast.LENGTH_SHORT).show()
            return
        }

        // Create filter for UserId
        val filters = mapOf(
            "UserId" to Filter(operator = "Equals", value = userId)
        )

        // Create the OrderSearchRequest object
        val orderSearchRequest = OrderSearchRequest(
            page = 1,
            pageSize = 100,
            sortBy = "Id",
            sortDirection = "asc",
            filters = filters
        )

        // Make the API call to search orders
        apiClient.apiService.searchOrder(orderSearchRequest).enqueue(object : Callback<OrderSearchResponse> {
            override fun onResponse(call: Call<OrderSearchResponse>, response: Response<OrderSearchResponse>) {
                if (response.isSuccessful && response.body() != null) {
                    val orderData = response.body()!!.data
                    if (orderData != null) {
                        // Fetch product details for each order
                        fetchProductDetails(orderData)
                    } else {
                        Toast.makeText(requireContext(), "No orders found.", Toast.LENGTH_SHORT).show()
                    }
                } else {
                    Toast.makeText(requireContext(), "Failed to retrieve orders.", Toast.LENGTH_SHORT).show()
                }
            }

            override fun onFailure(call: Call<OrderSearchResponse>, t: Throwable) {
                Toast.makeText(requireContext(), "Error: ${t.message}", Toast.LENGTH_SHORT).show()
            }
        })
    }

    private fun fetchProductDetails(orderData: List<OrderSearchResponse.OrderData>) {
        val updatedOrders = mutableListOf<OrderSearchResponse.OrderData>()

        val apiService = apiClient.apiService

        // Loop through each order and fetch product details
        for (order in orderData) {
            val products = order.products
            if (!products.isNullOrEmpty()) {
                val productId = products[0].productId!!
                apiService.getProductById(productId).enqueue(object : Callback<ProductSearchResponse.ProductData> {
                    override fun onResponse(call: Call<ProductSearchResponse.ProductData>, response: Response<ProductSearchResponse.ProductData>) {
                        if (response.isSuccessful && response.body() != null) {
                            val productData = response.body()
                            // Update the order with product image URL
                            products[0].imageUrl = productData?.imageUrl // Set the image URL
                            updatedOrders.add(order)

                            // Show the order history once all orders are processed
                            if (updatedOrders.size == orderData.size) {
                                showOrderHistory(updatedOrders)
                            }
                        }
                    }

                    override fun onFailure(call: Call<ProductSearchResponse.ProductData>, t: Throwable) {
                        // Handle error if needed
                    }
                })
            } else {
                updatedOrders.add(order) // If no products, add the order as is
            }
        }
    }

    private fun filterOrders(status: String) {
        // Retrieve the logged-in user's UserId from SharedPreferences
        val userId = SharedPreferenceHelper.getInstance(requireContext())
            .getSharedPreferenceString(Constant.USER_ID, "")

        // Check if UserId is valid
        if (userId.isNullOrEmpty()) {
            Toast.makeText(requireContext(), "Error: User not logged in", Toast.LENGTH_SHORT).show()
            return
        }

        // Create filters
        val filters = mapOf(
            "UserId" to Filter(operator = "Equals", value = userId),
            "Status" to Filter(operator = "Equals", value = status)
        )

        // Create the OrderSearchRequest object
        val orderSearchRequest = OrderSearchRequest(
            page = 1,
            pageSize = 100,
            sortBy = "Id",
            sortDirection = "desc",
            filters = filters
        )

        // Make the API call to search orders with the specified status
        apiClient.apiService.searchOrder(orderSearchRequest).enqueue(object : Callback<OrderSearchResponse> {
            override fun onResponse(call: Call<OrderSearchResponse>, response: Response<OrderSearchResponse>) {
                if (response.isSuccessful && response.body() != null) {
                    val orderData = response.body()!!.data
                    if (orderData != null) {
                        // Fetch product details for each order
                        fetchProductDetails(orderData)
                    } else {
                        Toast.makeText(requireContext(), "No orders found.", Toast.LENGTH_SHORT).show()
                    }
                } else {
                    Toast.makeText(requireContext(), "Failed to retrieve orders.", Toast.LENGTH_SHORT).show()
                }
            }

            override fun onFailure(call: Call<OrderSearchResponse>, t: Throwable) {
                Toast.makeText(requireContext(), "Error: ${t.message}", Toast.LENGTH_SHORT).show()
            }
        })
    }

    private fun showOrderHistory(orderHistory: List<OrderSearchResponse.OrderData>) {
        val adapter = OrderListAdapter(orderHistory)
        binding.rvOrderList.adapter = adapter
    }
}
