package com.example.EAD_MOBILE.viewModels

import androidx.lifecycle.LiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.asLiveData
import androidx.lifecycle.viewModelScope
import com.example.EAD_MOBILE.data.database.entities.Cart
import com.example.EAD_MOBILE.data.database.repo.CartRepository
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext

class CartViewModel(private val repository: CartRepository , private val id: Long) : ViewModel() {


    val getCartProducts: LiveData<List<Cart>> = repository.getCartProducts().asLiveData()

    fun addCartProducts(cartEntity: Cart) {
        viewModelScope.launch {
            withContext(Dispatchers.IO) {
                repository.addCartProducts(cartEntity)
            }
        }
    }

    fun deleteCartProductById(id: Long) {
        viewModelScope.launch {
            repository.deleteCartProductById(id)
        }
    }

    fun updateCartProducts(id: Long, newPrice: Double, newQuantity: Int) {
        viewModelScope.launch {
            repository.updateCartProducts(id, newPrice, newQuantity)
        }
    }

    suspend fun getCartProductByProductId(productId: String): Cart? {
        return withContext(Dispatchers.IO) {
            repository.getCartProductByProductId(productId)
        }
    }

    fun clearCart() {
        viewModelScope.launch {
            withContext(Dispatchers.IO) {
                repository.clearCart()
            }
        }
    }

}

class CartViewModelFactory(private val repository: CartRepository , private val id: Long) : ViewModelProvider.Factory {
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        if (modelClass.isAssignableFrom(CartViewModel::class.java)) {
            @Suppress("UNCHECKED_CAST")
            return CartViewModel(repository , id) as T
        }
        throw IllegalArgumentException("Unknown ViewModel class")
    }
}