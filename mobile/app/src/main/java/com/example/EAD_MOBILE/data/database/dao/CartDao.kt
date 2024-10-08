package com.example.EAD_MOBILE.data.database.dao

import androidx.room.Dao
import androidx.room.Delete
import androidx.room.Insert
import androidx.room.OnConflictStrategy
import androidx.room.Query
import com.example.EAD_MOBILE.data.database.entities.Cart
import kotlinx.coroutines.flow.Flow
@Dao
interface CartDao {

    @Query("SELECT * FROM cart")
    fun getCartProducts(): Flow<List<Cart>>

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    fun addCartProducts(product: Cart)

    @Query("UPDATE cart SET quantity = :newQuantity, price = :newPrice WHERE id = :id")
    fun updateCartProducts(id: Long, newPrice: Double, newQuantity: Int)

    @Query("DELETE FROM cart WHERE id = :id")
    fun deleteCartProductById(id: Long)

    @Query("SELECT * FROM cart WHERE productId = :productId LIMIT 1")
    fun getCartProductByProductId(productId: String): Cart?

    @Query("DELETE FROM cart")
    fun clearCart()

}