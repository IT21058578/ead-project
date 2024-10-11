import android.content.Intent
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView
import com.bumptech.glide.Glide
import com.example.EAD_MOBILE.R
import com.example.EAD_MOBILE.databinding.OrderListItemviewBinding
import com.example.EAD_MOBILE.models.response.OrderSearchResponse
import com.example.EAD_MOBILE.windows.OrderCancelActivity
import com.example.EAD_MOBILE.windows.AddReviewActivity

class OrderListAdapter(private val listOfOrders: List<OrderSearchResponse.OrderData>) :
    RecyclerView.Adapter<OrderListAdapter.ViewHolder>() {

    class ViewHolder(binding: OrderListItemviewBinding) : RecyclerView.ViewHolder(binding.root) {
        val orderId = binding.tvOrderId
        val productImage = binding.ivProductImage
        val productName = binding.tvProductName
        val productPrice = binding.tvPrice
        val orderDate = binding.tvOrderDate
        val orderStatus = binding.tvOrderState
        val addReviewButton = binding.btnAddReview
        val cancelOrderButton = binding.btnCancelOrder
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ViewHolder {
        return ViewHolder(
            OrderListItemviewBinding.inflate(
                LayoutInflater.from(parent.context), parent, false
            )
        )
    }

    override fun getItemCount(): Int {
        return listOfOrders.size
    }

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        val order = listOfOrders[position]

        // Check if the products list is not null and has at least one product
        if (order.products != null && order.products!!.isNotEmpty()) {
            holder.orderId.text = "Order ID: ${order.orderId}"

            // Load image using Glide for the first product
            val product = order.products!![0]
            Glide.with(holder.productImage.context)
                .load(product.imageUrl)  // Load the image URL https://github.com/bumptech/glide
                .placeholder(R.drawable.dress)  // Optional placeholder image
                .error(R.drawable.dress)  // Optional error image
                .into(holder.productImage)

            holder.productName.text = product.productName
            holder.productPrice.text = "Rs. ${product.price}"

            // Check the status of the product to determine button visibility
            when (order.status) {
                "Delivered" -> {
                    holder.addReviewButton.visibility = View.VISIBLE
                    holder.cancelOrderButton.visibility = View.GONE  // Hide cancel button
                    holder.addReviewButton.setOnClickListener {
                        val context = holder.itemView.context
                        val intent = Intent(context, AddReviewActivity::class.java)
                        intent.putExtra("ORDER_ID", order.orderId)
                        intent.putExtra("PRODUCT_ID", product.productId)
                        intent.putExtra("VENDOR_ID", product.vendorId)
                        Log.d("InputExtra Send", "OrderID: ${order.orderId}, ProductID: ${product.productId}, VendorID: ${product.vendorId}")
                        context.startActivity(intent)
                    }
                }
                "Pending" -> {
                    holder.cancelOrderButton.visibility = View.VISIBLE
                    holder.addReviewButton.visibility = View.GONE  // Hide add review button
                    holder.cancelOrderButton.setOnClickListener {
                        val context = holder.itemView.context
                        val intent = Intent(context, OrderCancelActivity::class.java)
                        intent.putExtra("ORDER_ID", order.orderId)
                        Log.d("OrderListAdapter", "Passing Order ID: ${order.orderId}")
                        context.startActivity(intent)
                    }
                }
                else -> {
                    holder.addReviewButton.visibility = View.GONE
                    holder.cancelOrderButton.visibility = View.GONE
                }
            }
        } else {
            // Handle the case where there are no products or products list is null
            holder.productName.text = "No products available"
            holder.productPrice.text = ""
            // Optional: set a placeholder image
            holder.productImage.setImageResource(R.drawable.dress)
            holder.addReviewButton.visibility = View.GONE
            holder.cancelOrderButton.visibility = View.GONE
        }

        val rawDeliveryDate = order.deliveryDate
        val formattedDeliveryDate = rawDeliveryDate?.substring(0, 10)
        holder.orderDate.text = formattedDeliveryDate
        // To show overall order status if needed
        holder.orderStatus.text = order.status
    }
}
