package com.example.EAD_MOBILE.ustils

import com.google.gson.annotations.Expose
import com.google.gson.annotations.SerializedName

class Constant {

    companion object{
        const val BASE_URL = "http://10.0.2.2:5158/api/v1/"

        const val CONTENT_TYPE = "application/x-www-form-urlencoded"
        const val LOG_ENABLE = true
        const val TAG = "EAD"

        //Login Authentications
        const val ACCESS_TOKEN = "ACCESS_TOKEN"
        const val REFRESH_TOKEN = "REFRESH_TOKEN"
        const val IS_LOGGED_IN = "IS_LOGGED_IN"
        const val USER_ID = "USER_ID"
    }
}