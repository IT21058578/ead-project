package com.example.EAD_MOBILE.windows

import OrderListAdapter
import android.graphics.Color
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import androidx.core.view.WindowInsetsControllerCompat
import androidx.recyclerview.widget.LinearLayoutManager
import com.example.EAD_MOBILE.dummies.SampleOrders
import com.example.EAD_MOBILE.dummies.orderHistory // Ensure this is properly defined
import com.example.EAD_MOBILE.databinding.ActivityOrderPageBinding

class OrderPage : AppCompatActivity() {

    private lateinit var binding: ActivityOrderPageBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        binding = ActivityOrderPageBinding.inflate(layoutInflater)
        setContentView(binding.root)

        // Set up the RecyclerView
        binding.rvOrderList.layoutManager = LinearLayoutManager(this)

        // Change the status bar color
        window.statusBarColor = Color.parseColor("#FFFFFF")
        WindowInsetsControllerCompat(window, window.decorView).isAppearanceLightStatusBars = true

        // Initially show all orders
        showOrderHistory(orderHistory)

        // Set up chip click listeners
        binding.chipAll.setOnClickListener { showOrderHistory(orderHistory) }
        binding.chipDelivered.setOnClickListener { filterOrdersByStatus("Delivered") }
        binding.chipProcessed.setOnClickListener { filterOrdersByStatus("Processing") }
        binding.chipPending.setOnClickListener { filterOrdersByStatus("Pending") }
    }

    private fun showOrderHistory(orderHistory: List<SampleOrders>) {
//        val adapter = OrderListAdapter(orderHist)
//        binding.rvOrderList.adapter = adapter
    }

    private fun filterOrdersByStatus(status: String) {
        val filteredOrders = orderHistory.filter { it.status == status }
        showOrderHistory(filteredOrders)
    }
}
