package com.example.EAD_MOBILE.windows

import android.content.Intent
import android.os.Build
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.util.Log
import android.widget.Toast
import androidx.activity.viewModels
import androidx.annotation.RequiresApi
import com.example.EAD_MOBILE.MyApplication
import com.example.EAD_MOBILE.api.APIClient
import com.example.EAD_MOBILE.data.database.entities.Cart
import com.example.EAD_MOBILE.databinding.ActivityCheckOutBinding
import com.example.EAD_MOBILE.models.request.OrderRequest
import com.example.EAD_MOBILE.models.response.OrderResponse
import com.example.EAD_MOBILE.ustils.Constant.Companion.TAG
import com.example.EAD_MOBILE.viewModels.CartViewModel
import com.example.EAD_MOBILE.viewModels.CartViewModelFactory
import kotlinx.coroutines.runBlocking
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import java.time.ZonedDateTime
import java.time.format.DateTimeFormatter

class CheckOutActivity : AppCompatActivity() {

    private lateinit var binding: ActivityCheckOutBinding
    private var id: Long? = null

    private val cartViewModel: CartViewModel by viewModels {
        CartViewModelFactory((application as MyApplication).cartRepository, id ?: 0 )
    }

    @RequiresApi(Build.VERSION_CODES.O)
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityCheckOutBinding.inflate(layoutInflater)
        setContentView(binding.root)

        // Retrieve the total price passed from ActivityCart
        val totalPrice = intent.getDoubleExtra("TOTAL_PRICE", 0.0)
        Log.d("TOTAL_PRICE", "${totalPrice}")

        // Display the total price in the tv_price_amount TextView
        binding.tvPriceAmount.text = "Rs. $totalPrice"

        // Observe cart products
        cartViewModel.getCartProducts.observe(this) { cartItems ->
            binding.checkoutBtn.setOnClickListener {
                if (cartItems.isNotEmpty()) {
                    // Use runBlocking to ensure synchronous execution (not recommended for UI thread)
                    runBlocking {
                        sendCart(cartItems)
                        showToast("Calling the Checkout API")
                    }
                } else {
                    showToast("No items in the cart")
                }
            }
        }
    }

    @RequiresApi(Build.VERSION_CODES.O)
    private fun sendCart(cartItems: List<Cart>) {
        runBlocking {
            val deliveryNote = binding.addressEditText.text.toString()
            val deliveryAddress = binding.postalCodeEditText.text.toString()

            // Delivery date is the current timestamp
            val currentTimestamp = ZonedDateTime.now().toString()
            val deliveryDate = getDeliveryDateFromTimestamp(currentTimestamp)

            val orderRequest = OrderRequest().apply {
                userId = "6700156f657c946cb29494a8"
                vendorId = "66feb530725d795f333be0c7"
                this.deliveryAddress = deliveryAddress
                this.deliveryNote = deliveryNote
                this.deliveryDate = deliveryDate
                products = cartItems.map { cartItem ->
                    OrderRequest.Product().apply {
                        productId = cartItem.productId
                        vendorId = cartItem.vendorId
                        price = cartItem.price
                        quantity = cartItem.quantity
                        name = cartItem.name
                    }
                }
            }

            // Send the cart to the API
            sendCartToApi(orderRequest)
        }
    }


    private fun sendCartToApi(orderRequest: OrderRequest) {
        val call = APIClient(applicationContext).apiService.sendCart(orderRequest)
        call.enqueue(object : Callback<OrderResponse> {
            override fun onResponse(call: Call<OrderResponse>, response: Response<OrderResponse>) {
                if (response.isSuccessful) {
                    // Navigate to the payment success page
                    val intent = Intent(this@CheckOutActivity, PaymentSuccessPage::class.java)
                    cartViewModel.clearCart()
                    startActivity(intent)
                    finish()

                } else {
                    println("Else Occured")
                }
            }

            override fun onFailure(call: Call<OrderResponse>, t: Throwable) {
                println("Error Occured")
            }
        })
    }

    @RequiresApi(Build.VERSION_CODES.O)
    private fun getDeliveryDateFromTimestamp(timestamp: String): String {
        // Parse the timestamp into a ZonedDateTime object
        val zonedDateTime = ZonedDateTime.parse(timestamp)

        // Extract the date portion only, without time
        return zonedDateTime.format(DateTimeFormatter.ISO_LOCAL_DATE)
    }

    private fun showToast(message: String) {
        Toast.makeText(this, message, Toast.LENGTH_LONG).show()
        Log.d(TAG, message)
    }
}