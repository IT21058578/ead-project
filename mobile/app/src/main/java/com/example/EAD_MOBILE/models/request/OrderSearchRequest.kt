package com.example.EAD_MOBILE.models.request

import com.google.gson.annotations.Expose
import com.google.gson.annotations.SerializedName

class OrderSearchRequest(
    @SerializedName("page")
    @Expose
    var page: Int = 1, // Default page to 1

    @SerializedName("pageSize")
    @Expose
    var pageSize: Int = 10, // Default page size to 10

    @SerializedName("sortBy")
    @Expose
    var sortBy: String = "Id", // Default sort by "Id"

    @SerializedName("sortDirection")
    @Expose
    var sortDirection: String = "desc", // Default sort direction to "desc"

    @SerializedName("filters")
    @Expose
    var filters: Map<String, Filter> = mapOf() // Filters map, initialized to an empty map
)

// Declare `Filter` as a separate class
class Filter(
    @SerializedName("operator")
    @Expose
    var operator: String = "Equals", // Initialize operator via constructor

    @SerializedName("value")
    @Expose
    var value: String // Initialize value via constructor
)
