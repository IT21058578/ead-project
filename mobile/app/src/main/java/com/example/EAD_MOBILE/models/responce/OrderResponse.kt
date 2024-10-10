package com.example.EAD_MOBILE.models.response

import com.google.gson.annotations.Expose
import com.google.gson.annotations.SerializedName

class OrderResponse {

    @SerializedName("id")
    @Expose
    var id: String? = null

    @SerializedName("userId")
    @Expose
    var userId: String? = null

    @SerializedName("status")
    @Expose
    var status: String? = null

    @SerializedName("products")
    @Expose
    var products: List<ProductData>? = null

    @SerializedName("deliveryNote")
    @Expose
    var deliveryNote: String? = null

    @SerializedName("deliveryAddress")
    @Expose
    var deliveryAddress: String? = null

    @SerializedName("deliveryDate")
    @Expose
    var deliveryDate: String? = null

    @SerializedName("actualDeliveryDate")
    @Expose
    var actualDeliveryDate: String? = null

    @SerializedName("vendorIds")
    @Expose
    var vendorIds: List<String>? = null

    class ProductData {
        @SerializedName("productId")
        @Expose
        var productId: String? = null

        @SerializedName("vendorId")
        @Expose
        var vendorId: String? = null

        @SerializedName("quantity")
        @Expose
        var quantity: Int? = null

        @SerializedName("status")
        @Expose
        var status: String? = null

        @SerializedName("name")
        @Expose
        var name: String? = null

        @SerializedName("price")
        @Expose
        var price: Double? = null
    }
}
