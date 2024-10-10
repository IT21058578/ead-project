package com.example.EAD_MOBILE.api

import android.content.Context
import com.example.EAD_MOBILE.ustils.Constant.Companion.BASE_URL
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

class APIClientLogin(private var applicationContext: Context) {


    private val gson : Gson by lazy {
        GsonBuilder().setLenient().create()
    }

    private var httpClient: OkHttpClient = OkHttpClient.Builder()
        .addInterceptor(object : Interceptor {
            @Throws(IOException::class)
            override fun intercept(chain: Interceptor.Chain): Response {
                val request: Request = chain.request()
                val response: Response = chain.proceed(request)

                if (response.code == 500) {

                    return response
                }else if (response.code == 404){

                    applicationContext.logPrint("Error $response")
                    return response
                }
                return response
            }
        })
        .build()


    private val retrofit : Retrofit by lazy {

        Retrofit.Builder()
            .baseUrl(BASE_URL)
            .client(httpClient)
            .addConverterFactory(GsonConverterFactory.create(gson))
            .build()
    }

    val apiService :  APIServiceInterface by lazy{
        retrofit.create(APIServiceInterface::class.java)
    }

}