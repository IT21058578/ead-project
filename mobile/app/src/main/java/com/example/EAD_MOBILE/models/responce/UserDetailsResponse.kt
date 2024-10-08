package com.example.EAD_MOBILE.models.responce

import com.google.gson.annotations.Expose
import com.google.gson.annotations.SerializedName

class UserDetailsResponse {
    @SerializedName("id")
    @Expose
    val id : String? = null

    @SerializedName("email")
    @Expose
    var email: String? = null

    @SerializedName("firstName")
    @Expose
    var firstName: String? = null

    @SerializedName("lastName")
    @Expose
    var lastName: String? = null

    @SerializedName("role")
    @Expose
    var role: String? = null

    @SerializedName("isVerified")
    @Expose
    var isVerified: Boolean = false

    @SerializedName("isApproved")
    @Expose
    var isApproved: Boolean = false

    @SerializedName("rating")
    @Expose
    var rating: Int = 0
}