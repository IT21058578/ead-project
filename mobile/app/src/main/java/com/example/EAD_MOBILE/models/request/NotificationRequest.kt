package com.example.EAD_MOBILE.models.request

import com.google.gson.annotations.Expose
import com.google.gson.annotations.SerializedName

class NotificationSearchRequest(
    @SerializedName("page")
    @Expose
    var page: Int = 1, // Default page to 1

    @SerializedName("pageSize")
    @Expose
    var pageSize: Int = 100, // Default page size to 100

    @SerializedName("sortBy")
    @Expose
    var sortBy: String = "Id", // Default sort by "Id"

    @SerializedName("sortDirection")
    @Expose
    var sortDirection: String = "desc", // Default sort direction to "desc"

    @SerializedName("filters")
    @Expose
    var filters: NotificationFilters // Filters for notifications
)

class NotificationFilters(
    @SerializedName("UserId")
    @Expose
    var userId: Filter // Filter for UserId
)

