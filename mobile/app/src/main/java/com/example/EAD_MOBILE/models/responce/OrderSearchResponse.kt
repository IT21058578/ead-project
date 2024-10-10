package com.example.EAD_MOBILE.models.response

import com.google.gson.annotations.Expose
import com.google.gson.annotations.SerializedName

class OrderSearchResponse {

    @SerializedName("meta")
    @Expose
    var meta: Meta? = null

    @SerializedName("data")
    @Expose
    var data: List<OrderData>? = null

    class Meta {
        @SerializedName("page")
        @Expose
        var page: Int? = null

        @SerializedName("pageSize")
        @Expose
        var pageSize: Int? = null

        @SerializedName("total")
        @Expose
        var total: Int? = null

        @SerializedName("totalInPage")
        @Expose
        var totalInPage: Int? = null

        @SerializedName("totalPages")
        @Expose
        var totalPages: Int? = null

        @SerializedName("isFirst")
        @Expose
        var isFirst: Boolean? = null

        @SerializedName("isLast")
        @Expose
        var isLast: Boolean? = null
    }

    class OrderData {
        @SerializedName("id")
        @Expose
        var orderId: String? = null

        @SerializedName("status")
        @Expose
        var status: String? = null

        @SerializedName("deliveryNote")
        @Expose
        var deliveryNote: String? = null

        @SerializedName("deliveryAddress")
        @Expose
        var deliveryAddress: String? = null

        @SerializedName("deliveryDate")
        @Expose
        var deliveryDate: String? = null

        @SerializedName("products")
        @Expose
        var products: List<ProductData>? = null
    }

    class ProductData {

        @SerializedName("productId")
        @Expose
        var productId: String? = null

        @SerializedName("vendorId")
        @Expose
        var vendorId: String? = null

        @SerializedName("name")
        @Expose
        var productName: String? = null

        @SerializedName("status")
        @Expose
        var status: String? = null

        @SerializedName("price")
        @Expose
        var price: Double? = null

        @SerializedName("imageUrl")
        @Expose
        var imageUrl: String? = null
    }
}
