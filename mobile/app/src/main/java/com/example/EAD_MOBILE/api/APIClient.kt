package com.example.EAD_MOBILE.api

import android.content.Context
import com.example.EAD_MOBILE.ustils.Constant.Companion.BASE_URL
import com.example.EAD_MOBILE.ustils.Utility
import com.example.EAD_MOBILE.ustils.Utility.isInternetAvailable
import com.example.EAD_MOBILE.ustils.Utility.logPrint
import com.google.gson.Gson
import com.google.gson.GsonBuilder
import okhttp3.Interceptor
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import java.io.IOException

class APIClient(private var applicationContext: Context) {

    // Gson instance for handling JSON parsing
    private val gson: Gson by lazy {
        GsonBuilder().setLenient().create()
    }

    // OkHttpClient with an interceptor to manage error responses
    private var httpClient: OkHttpClient = OkHttpClient.Builder()
        .addInterceptor(object : Interceptor {
            @Throws(IOException::class)
            override fun intercept(chain: Interceptor.Chain): Response {
                val request: Request = chain.request()
                val response: Response = chain.proceed(request)

                // Handle various HTTP response codes
                when (response.code) {
                    500 -> {
                        // Log or manage server errors
                        applicationContext.logPrint("Server Error: $response")
                    }
                    404 -> {
                        // Handle Not Found error
                        applicationContext.logPrint("Not Found: $response")
                    }
                    401 -> {
                        // Handle Unauthorized error and try to refresh token
                        return if (applicationContext.isInternetAvailable()) {
                            // Try to refresh the token if internet is available
                            Utility.refreshToken(applicationContext)
                            applicationContext.logPrint("Token refreshed for: $response")
                            response
                        } else {
                            // Log the user out if no internet and session expired
                            Utility.logOut(applicationContext)
                            applicationContext.logPrint("User logged out due to expired token: $response")
                            response
                        }
                    }
                }
                return response
            }
        })
        .build()

    // Retrofit instance setup
    private val retrofit: Retrofit by lazy {
        Retrofit.Builder()
            .baseUrl(BASE_URL) // Base URL of the API
            .client(httpClient) // OkHttpClient with the interceptor
            .addConverterFactory(GsonConverterFactory.create(gson)) // Gson for JSON parsing
            .build()
    }

    // API service interface to make network calls
    val apiService: APIServiceInterface by lazy {
        retrofit.create(APIServiceInterface::class.java)
    }
}
