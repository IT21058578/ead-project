package com.example.EAD_MOBILE.models.request

import com.google.gson.annotations.Expose
import com.google.gson.annotations.SerializedName

class ProductSearchRequest {

    @SerializedName("page")
    @Expose
    var page: Int? = null

    @SerializedName("pageSize")
    @Expose
    var pageSize: Int? = null

    @SerializedName("sortBy")
    @Expose
    var sortBy: String? = null

    @SerializedName("sortDirection")
    @Expose
    var sortDirection: String? = null

    @SerializedName("filters")
    @Expose
    var filters: Map<String, Filter>? = null

    class Filter {
        @SerializedName("operator")
        @Expose
        var operator: String? = null

        @SerializedName("value")
        @Expose
        var value: String? = null
    }
}
