package com.example.EAD_MOBILE.models.responce

import com.google.gson.annotations.Expose
import com.google.gson.annotations.SerializedName

class CancelOrderResponse {

    @SerializedName("type")
    @Expose
    var type: String? = null

    @SerializedName("title")
    @Expose
    var title: String? = null

    @SerializedName("status")
    @Expose
    var status: Int = 0

    @SerializedName("errors")
    @Expose
    var errors: Map<String, List<String>>? = mapOf()

    @SerializedName("traceId")
    @Expose
    var traceId: String? = null



}