package com.example.EAD_MOBILE.adapter

import android.view.LayoutInflater
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView
import com.bumptech.glide.load.model.GlideUrl
import com.example.EAD_MOBILE.R
import com.example.EAD_MOBILE.data.database.entities.Cart
import com.example.EAD_MOBILE.dummies.SampleOrders
import com.example.EAD_MOBILE.databinding.CartListItemviewBinding
import com.example.EAD_MOBILE.viewModels.CartViewModel

class CartListAdapter(private val listOfOrders : List<Cart> ,
                      private val cartViewModel: CartViewModel
) :
    RecyclerView.Adapter<CartListAdapter.ViewHolder>(){


    class ViewHolder(binding: CartListItemviewBinding) : RecyclerView.ViewHolder(binding.root) {
        val productImage = binding.ivProductImage
        val productName = binding.tvProductName
        val productPrice = binding.tvPrice
        val prodDelete = binding.ivDeleteBtn
        val ProdQuantity = binding.tvOrderState
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ViewHolder {
        return ViewHolder(CartListItemviewBinding.inflate(
            LayoutInflater.from(parent.context), parent, false
        ))
    }

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        val product = listOfOrders[position]

        holder.productImage.setImageResource(R.drawable.dress)
        holder.productName.text = product.name
        holder.productPrice.text = "Rs. ${product.price}"
        holder.ProdQuantity.text = "Qty. ${product.quantity.toString()}"

        holder.prodDelete.setOnClickListener {
            product.id?.let { productId ->
                cartViewModel.deleteCartProductById(productId)
            }
        }
    }

    override fun getItemCount(): Int {
        return listOfOrders.size
    }
}