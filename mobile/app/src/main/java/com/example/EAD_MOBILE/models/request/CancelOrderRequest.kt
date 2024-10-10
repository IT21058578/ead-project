package com.example.EAD_MOBILE.models.request

import com.google.gson.annotations.Expose
import com.google.gson.annotations.SerializedName

class CancelOrderRequest {
    @SerializedName("orderId")
    @Expose
    var orderId: String? = null

    @SerializedName("userId")
    @Expose
    var userId: String? = null

    @SerializedName("reason")
    @Expose
    var reason: String? = null
}
