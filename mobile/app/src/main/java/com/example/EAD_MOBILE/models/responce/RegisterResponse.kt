package com.example.EAD_MOBILE.models.responce

import com.google.gson.annotations.SerializedName

data class RegisterResponse(
    @SerializedName("message")
    val message: String // Modify this based on the actual response from your backend
)