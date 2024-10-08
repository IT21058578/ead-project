package com.example.EAD_MOBILE.models.request

import com.google.gson.annotations.Expose
import com.google.gson.annotations.SerializedName

class OrderRequest {

    @SerializedName("userId")
    @Expose
    var userId: String? = null

    @SerializedName("vendorId")
    @Expose
    var vendorId: String? = null

    @SerializedName("products")
    @Expose
    var products: List<Product>? = null

    @SerializedName("deliveryNote")
    @Expose
    var deliveryNote: String? = null

    @SerializedName("deliveryAddress")
    @Expose
    var deliveryAddress: String? = null

    @SerializedName("deliveryDate")
    @Expose
    var deliveryDate: String? = null

    class Product{
        @SerializedName("productId")
        @Expose
        var productId: String? = null

        @SerializedName("vendorId")
        @Expose
        var vendorId: String? = null

        @SerializedName("quantity")
        @Expose
        var quantity: Int? = null

        @SerializedName("name")
        @Expose
        var name: String? = null

        @SerializedName("price")
        @Expose
        var price: Double? = null
    }
}