package com.example.EAD_MOBILE.models.request

data class ReviewSearchRequest(
    val page: Int,
    val pageSize: Int,
    val sortBy: String,
    val sortDirection: String,
    val filters: Filters
)

data class Filters(
    val ProductId: FilterValue
)

data class FilterValue(
    val operator: String,
    val value: String
)
