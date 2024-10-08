package com.example.EAD_MOBILE.adapter

import android.annotation.SuppressLint
import android.view.LayoutInflater
import android.view.ViewGroup
import androidx.lifecycle.viewModelScope
import androidx.recyclerview.widget.RecyclerView
import com.bumptech.glide.Glide
import com.bumptech.glide.request.RequestOptions
import com.example.EAD_MOBILE.R
import com.example.EAD_MOBILE.data.database.entities.Cart
import com.example.EAD_MOBILE.databinding.ProductItemviewBinding
import com.example.EAD_MOBILE.models.responce.ProductSearchResponse
import com.example.EAD_MOBILE.viewModels.CartViewModel
import kotlinx.coroutines.launch

class ProductListAdapter(
    private val listOfProducts: List<ProductSearchResponse.ProductData>,
    private val cartViewModel: CartViewModel
) : RecyclerView.Adapter<ProductListAdapter.ViewHolder>() {

    private var onClickListener: OnClickListener? = null

    class ViewHolder(binding: ProductItemviewBinding) : RecyclerView.ViewHolder(binding.root) {
        val productImage = binding.ivProduct
        val productName = binding.tvProductName
        val productPrice = binding.tvPrice
        val productCard = binding.productCardview
        val productOrigin = binding.tvOrigin
        val addToCartButton = binding.appCompatImageButton
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ViewHolder {
        return ViewHolder(
            ProductItemviewBinding.inflate(
                LayoutInflater.from(parent.context),
                parent,
                false
            )
        )
    }

    override fun getItemCount(): Int {
        return listOfProducts.size
    }



    @SuppressLint("SetTextI18n")
    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        val product = listOfProducts[position]

        Glide.with(holder.productImage.context)
            .load(product.imageUrl)
            .apply(RequestOptions.placeholderOf(R.drawable.dress))
            .into(holder.productImage)

        holder.productName.text = product.name
        holder.productOrigin.text = product.category
        holder.productPrice.text = "Rs. ${product.price}"

        holder.productCard.setOnClickListener {
            if (onClickListener != null) {
                product.id?.let { id ->
                    // Send product ID instead of index
                    onClickListener?.onClick(id)
                }
            }
        }

        holder.addToCartButton.setOnClickListener {
            product.id?.let { productId ->
                // Use a coroutine to handle the asynchronous nature of database queries
                holder.itemView.context?.let { context ->
                    // Launch the coroutine in the ViewModel scope
                    cartViewModel.viewModelScope.launch {
                        // Get the cart product in a background thread
                        val existingCartProduct = cartViewModel.getCartProductByProductId(productId)

                        if (existingCartProduct != null) {
                            // If the product already exists, update the quantity and price
                            val updatedQuantity = existingCartProduct.quantity!! + 1
                            val updatedPrice = existingCartProduct.price!! + product.price!!

                            cartViewModel.updateCartProducts(
                                existingCartProduct.id,
                                updatedPrice,
                                updatedQuantity
                            )
                        } else {
                            // If the product doesn't exist, add a new product to the cart
                            val cartProduct = Cart(
                                productId = productId,
                                vendorId = product.vendorId!!,
                                quantity = 1, // Default to 1
                                name = product.name!!,
                                price = product.price
                            )
                            cartViewModel.addCartProducts(cartProduct)
                        }
                    }
                }
            }
        }

    }

    fun setOnClickListener(onClickListener: OnClickListener) {
        this.onClickListener = onClickListener
    }

    interface OnClickListener {
        fun onClick(productId: String)
    }
}