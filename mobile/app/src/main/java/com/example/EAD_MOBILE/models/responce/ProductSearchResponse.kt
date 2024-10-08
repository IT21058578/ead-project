package com.example.EAD_MOBILE.models.responce

import com.google.gson.annotations.Expose
import com.google.gson.annotations.SerializedName

class ProductSearchResponse {

    @SerializedName("data")
    @Expose
    var   data: List<ProductData>? = null

    class ProductData {
        @SerializedName("id")
        @Expose
        var id: String? = null

        @SerializedName("createdBy")
        @Expose
        var createdBy: String? = null

        @SerializedName("createdAt")
        @Expose
        var createdAt: String? = null

        @SerializedName("vendorId")
        @Expose
        var vendorId: String? = null

        @SerializedName("name")
        @Expose
        var name: String? = null

        @SerializedName("description")
        @Expose
        var description: String? = null

        @SerializedName("category")
        @Expose
        var category: String? = null

        @SerializedName("price")
        @Expose
        var price: Double? = null

        @SerializedName("isActive")
        @Expose
        var isActive: Boolean? = null

        @SerializedName("countInStock")
        @Expose
        var countInStock: Int? = null

        @SerializedName("lowStockThreshold")
        @Expose
        var lowStockThreshold: Int? = null

        @SerializedName("rating")
        @Expose
        var rating: Double? = null

        @SerializedName("imageUrl")
        @Expose
        var imageUrl: String? = null
    }
}
