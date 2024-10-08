package com.example.EAD_MOBILE.models.response

import com.google.gson.annotations.Expose
import com.google.gson.annotations.SerializedName

class ReviewSearchResponse {

    @SerializedName("meta")
    @Expose
    var meta: Meta? = null

    @SerializedName("data")
    @Expose
    var reviews: List<Review>? = null

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

    class Review {
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

        @SerializedName("productId")
        @Expose
        var productId: String? = null

        @SerializedName("userId")
        @Expose
        var userId: String? = null

        @SerializedName("message")
        @Expose
        var message: String? = null

        @SerializedName("rating")
        @Expose
        var rating: Int? = null
    }
}
