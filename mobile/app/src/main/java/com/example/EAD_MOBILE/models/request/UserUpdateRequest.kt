package com.example.EAD_MOBILE.models.request

import com.google.gson.annotations.Expose
import com.google.gson.annotations.SerializedName

class UserUpdateRequest {
    @SerializedName("firstName")
    @Expose
    var firstName: String? = null

    @SerializedName("lastName") // Corrected from "firstName" to "lastName"
    @Expose
    var lastName: String? = null
}
