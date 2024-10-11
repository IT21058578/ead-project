package com.example.EAD_MOBILE.api


import ReviewRequest
import com.example.EAD_MOBILE.models.request.CancelOrderRequest
import com.example.EAD_MOBILE.models.request.LoginRequest
import com.example.EAD_MOBILE.models.request.NotificationSearchRequest
import com.example.EAD_MOBILE.models.request.OrderRequest
import com.example.EAD_MOBILE.models.request.OrderSearchRequest
import com.example.EAD_MOBILE.models.request.ProductSearchRequest
import com.example.EAD_MOBILE.models.request.RegisterRequest
import com.example.EAD_MOBILE.models.request.ReviewSearchRequest
import com.example.EAD_MOBILE.models.request.UserUpdateRequest
import com.example.EAD_MOBILE.models.responce.CancelOrderResponse
import com.example.EAD_MOBILE.models.responce.LoginResponse
import com.example.EAD_MOBILE.models.responce.NotificationResponse
import com.example.EAD_MOBILE.models.responce.ProductSearchResponse
import com.example.EAD_MOBILE.models.responce.ReviewResponse
import com.example.EAD_MOBILE.models.responce.UserDetailsResponse
import com.example.EAD_MOBILE.models.response.OrderResponse
import com.example.EAD_MOBILE.models.response.OrderSearchResponse
import com.example.EAD_MOBILE.models.response.ReviewSearchResponse
import okhttp3.ResponseBody
import retrofit2.Call
import retrofit2.http.*

interface APIServiceInterface {


    @POST("auth/refresh")
    fun reAuthenticate(@Body refreshToken: String): Call<LoginResponse>

    @Headers("Content-Type:application/json")
    @POST("auth/register")
    fun registerUser(@Body registerRequest: RegisterRequest): Call<ResponseBody>

    @POST("products/search")
    fun searchProduct(@Body request: ProductSearchRequest): Call<ProductSearchResponse>

    @GET("products/{id}")
    fun getProductById(@Path("id") productId: String): Call<ProductSearchResponse.ProductData>

    @GET("users/{id}")
    fun getUserById(@Path("id") userId: String): Call<UserDetailsResponse>

    @PUT("users/{id}")
    fun editUserById(@Path("id") userId: String, @Body user:  UserUpdateRequest): Call<UserDetailsResponse>

    @Headers("Content-Type:application/json")
    @POST("auth/login")
    fun loginUser(@Body registerRequest: LoginRequest): Call<LoginResponse>

    @PUT("auth/register/verify")
    fun verifyUser(@Query("code") code: String, @Query("email") email: String): Call<ResponseBody>

    @POST("orders")
    fun sendCart(@Body cartItems: OrderRequest): Call<OrderResponse>

    @POST("orders/search")
    fun searchOrder(@Body orderSearchRequest: OrderSearchRequest): Call<OrderSearchResponse>

    @POST("reviews")
    fun postReview(@Body postReview: ReviewRequest): Call<ReviewResponse>

    @POST("customer-requests/order-cancellation")
    fun cancelOrder(@Body cancelOrder: CancelOrderRequest): Call<ResponseBody>

    @POST("reviews/search")
    fun searchReviews(@Body searchReviewRequest: ReviewSearchRequest): Call<ReviewSearchResponse>

    @POST("notifications/search")
    fun searchNotifications(@Body searchNotificationRequest: NotificationSearchRequest): Call<NotificationResponse>




}
