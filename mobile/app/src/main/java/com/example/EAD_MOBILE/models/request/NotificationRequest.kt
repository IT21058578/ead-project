package com.example.EAD_MOBILE.models.request

import com.google.gson.annotations.Expose
import com.google.gson.annotations.SerializedName

class NotificationSearchRequest(
    @SerializedName("page")
    @Expose
    var page: Int = 1,

    @SerializedName("pageSize")
    @Expose
    var pageSize: Int = 100,

    @SerializedName("sortBy")
    @Expose
    var sortBy: String = "Id",

    @SerializedName("sortDirection")
    @Expose
    var sortDirection: String = "asc",

    @SerializedName("filters")
    @Expose
    var filters: NotificationFilters
)

class NotificationFilters(
    @SerializedName("UserId")
    @Expose
    var userId: Filter
)

