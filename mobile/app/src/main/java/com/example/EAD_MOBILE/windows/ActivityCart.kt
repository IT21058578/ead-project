package com.example.EAD_MOBILE.windows

import android.content.Intent
import android.graphics.Color
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.util.Log
import androidx.activity.viewModels
import androidx.core.view.WindowInsetsControllerCompat
import androidx.fragment.app.viewModels
import androidx.recyclerview.widget.LinearLayoutManager
import com.example.EAD_MOBILE.MyApplication
import com.example.EAD_MOBILE.adapter.CartListAdapter
import com.example.EAD_MOBILE.api.APIClient
import com.example.EAD_MOBILE.data.database.entities.Cart
import com.example.EAD_MOBILE.databinding.ActivityCartBinding
import com.example.EAD_MOBILE.dummies.SampleOrders
import com.example.EAD_MOBILE.dummies.orderHistory
import com.example.EAD_MOBILE.models.request.OrderRequest
import com.example.EAD_MOBILE.models.response.OrderResponse
import com.example.EAD_MOBILE.viewModels.CartViewModel
import com.example.EAD_MOBILE.viewModels.CartViewModelFactory
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class ActivityCart : AppCompatActivity() {

    private lateinit var binding: ActivityCartBinding
    private var id: Long? = null
    private var totalPrice: String? = null

    private val cartViewModel: CartViewModel by viewModels {
        CartViewModelFactory((application as MyApplication).cartRepository, id ?: 0 )
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityCartBinding.inflate(layoutInflater)
        setContentView(binding.root)

        binding.rvCartItems.layoutManager = LinearLayoutManager(this)

        window.statusBarColor = (Color.parseColor("#FFFFFF"))
        WindowInsetsControllerCompat(window, window.decorView).isAppearanceLightStatusBars = true

        cartViewModel.getCartProducts.observe(this) { cartItems ->
            showOrderCart(cartItems)
            // Assign the calculated total price to the totalPrice variable
            totalPrice = calculateTotalPrice(cartItems)
        }

        binding.checkoutBtn.setOnClickListener {
            if (totalPrice != null) {
                try {
                    val totalPriceDouble = totalPrice!!.toDouble() // Convert the totalPrice to Double
                    val intent = Intent(this, CheckOutActivity::class.java)
                    intent.putExtra("TOTAL_PRICE", totalPriceDouble) // Pass the total price as Double
                    startActivity(intent)
                    Log.d("TOTAL_PRICE", "$totalPriceDouble")
                } catch (e: NumberFormatException) {
                    Log.d("TOTAL_PRICE", "Failed to convert total price to Double")
                }
            } else {
                Log.d("TOTAL_PRICE", "Total price is null, cannot proceed to checkout.")
            }
        }
    }

    private fun showOrderCart(cartItems: List<Cart>) {
        val adapter = CartListAdapter(cartItems, cartViewModel)
        binding.rvCartItems.adapter = adapter
    }

    private fun calculateTotalPrice(cartItems: List<Cart>): String {
        val total = cartItems.sumOf { it.price!! * it.quantity!! } // Calculate total price
        binding.tvPriceAmount.text = "Rs. $total" // Update the TextView with total
        return total.toString() // Return the total price as a String
    }




}