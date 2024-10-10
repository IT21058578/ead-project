package com.example.EAD_MOBILE.data.database.repo

import androidx.annotation.WorkerThread
import com.example.EAD_MOBILE.data.database.dao.CartDao
import com.example.EAD_MOBILE.data.database.entities.Cart
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.withContext

class CartRepository(private val cartDao: CartDao) {


    fun getCartProducts() : Flow<List<Cart>> = cartDao.getCartProducts()


    @Suppress("RedundantSuspendModifier")
    @WorkerThread
    suspend fun addCartProducts(product : Cart) {
        cartDao.addCartProducts(product)
    }

    suspend fun deleteCartProductById(id: Long) {
        withContext(Dispatchers.IO) {
            cartDao.deleteCartProductById(id)
        }
    }

    suspend fun updateCartProducts(id: Long, newPrice: Double, newQuantity: Int) {
        withContext(Dispatchers.IO){
            cartDao.updateCartProducts(id, newPrice, newQuantity)
        }
    }

    fun getCartProductByProductId(productId: String): Cart? {
        return cartDao.getCartProductByProductId(productId)
    }

    @Suppress("RedundantSuspendModifier")
    @WorkerThread
    suspend fun clearCart() {
        cartDao.clearCart()
    }
}