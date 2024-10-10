package com.example.EAD_MOBILE.models.responce


import com.google.gson.annotations.Expose
import com.google.gson.annotations.SerializedName

data class NotificationResponse(
    @SerializedName("meta")
    @Expose
    var meta: Meta,

    @SerializedName("data")
    @Expose
    var data: List<Notification>
)

data class Meta(
    @SerializedName("page")
    @Expose
    var page: Int,

    @SerializedName("pageSize")
    @Expose
    var pageSize: Int,

    @SerializedName("total")
    @Expose
    var total: Int,

    @SerializedName("totalInPage")
    @Expose
    var totalInPage: Int,

    @SerializedName("totalPages")
    @Expose
    var totalPages: Int,

    @SerializedName("isFirst")
    @Expose
    var isFirst: Boolean,

    @SerializedName("isLast")
    @Expose
    var isLast: Boolean
)

data class Notification(
    @SerializedName("id")
    @Expose
    var id: String,

    @SerializedName("createdBy")
    @Expose
    var createdBy: String,

    @SerializedName("createdAt")
    @Expose
    var createdAt: String,

    @SerializedName("updatedBy")
    @Expose
    var updatedBy: String,

    @SerializedName("updatedAt")
    @Expose
    var updatedAt: String, // You can use a specific date-time type if preferred

    @SerializedName("recipient")
    @Expose
    var recipient: String,

    @SerializedName("type")
    @Expose
    var type: String,

    @SerializedName("userId")
    @Expose
    var userId: String,

    @SerializedName("orderId")
    @Expose
    var orderId: String,

    @SerializedName("productId")
    @Expose
    var productId: String,

    @SerializedName("reason")
    @Expose
    var reason: String,

    @SerializedName("status")
    @Expose
    var status: String,

    @SerializedName("addresedBy")
    @Expose
    var addresedBy: String
)
