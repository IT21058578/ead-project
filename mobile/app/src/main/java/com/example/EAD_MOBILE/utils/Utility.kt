package com.example.EAD_MOBILE.ustils

import android.R
import android.app.AlarmManager
import android.app.AlertDialog
import android.content.Context
import android.content.Intent
import android.content.IntentFilter
import android.content.pm.PackageManager
import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.graphics.Color
import android.net.ConnectivityManager
import android.net.NetworkCapabilities
import android.net.Uri
import android.os.BatteryManager
import android.os.Build
import android.os.Handler
import android.os.Looper
import android.provider.SyncStateContract
import android.util.Log
import android.view.Gravity
import android.widget.TextView
import android.widget.Toast
import com.example.EAD_MOBILE.api.APIClient
import com.example.EAD_MOBILE.models.responce.LoginResponse
import com.example.EAD_MOBILE.storage.SharedPreferenceHelper
import com.example.EAD_MOBILE.windows.LoginActivity
import org.json.JSONObject
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

object Utility {

    // log print function
    fun Context.logPrint(message: String?) {

        try {
            if (Constant.LOG_ENABLE) {

                message?.let { Log.d(Constant.TAG, it) }

            }
        } catch (e: Exception) {

            logPrint("Log print error " + e.localizedMessage)
        }

    }

    @Suppress("DEPRECATION")
    fun Context.isInternetAvailable(): Boolean {
        var result = false

        try {

            val cm = getSystemService(Context.CONNECTIVITY_SERVICE) as ConnectivityManager?
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
                cm?.run {
                    cm.getNetworkCapabilities(cm.activeNetwork)?.run {

                        result = when {
                            hasTransport(NetworkCapabilities.TRANSPORT_WIFI) -> true
                            hasTransport(NetworkCapabilities.TRANSPORT_CELLULAR) -> true
                            hasTransport(NetworkCapabilities.TRANSPORT_ETHERNET) -> true

                            else -> false
                        }
                    }
                }
            } else {
                cm?.run {
                    cm.activeNetworkInfo?.run {
                        result = when (type) {
                            ConnectivityManager.TYPE_WIFI -> true
                            ConnectivityManager.TYPE_MOBILE -> true
                            else -> false
                        }

                    }
                }
            }


        } catch (e: Exception) {

            e.printStackTrace()
        }
        return result
    }


    @Suppress("DEPRECATION")
    fun Context.showErrorToast(message: String?) {

        try {
            val toast = Toast.makeText(this, message, Toast.LENGTH_LONG)

            // set message color
            val textView = toast.view?.findViewById(R.id.message) as TextView
            textView.setTextColor(Color.WHITE)
            textView.gravity = Gravity.CENTER

            // set background color
            toast.view?.setBackgroundColor(Color.RED)

            toast.setGravity(Gravity.TOP or Gravity.FILL_HORIZONTAL, 0, 0)

            toast.show()
        } catch (e: Exception) {
            e.printStackTrace()
        }

    }

    fun logOut(applicationContext: Context) {

        applicationContext.logPrint("Logout session expired ")
        try {
            val mainLooper = Looper.getMainLooper()

            Thread(Runnable {

                Handler(mainLooper).post {
                    val builder = AlertDialog.Builder(applicationContext)
                    builder.setTitle("Session Time Out")
                    builder.setMessage("Your session has time out. Please sign in again.")
                    builder.setIcon(android.R.drawable.ic_dialog_alert)
                    //performing positive action
                    builder.setPositiveButton("Ok") { _, which ->

                        SharedPreferenceHelper.getInstance(applicationContext).clearSharedPreference()
                        val loginIntent = Intent(applicationContext, LoginActivity::class.java)
                        applicationContext.startActivity(loginIntent)
                    }

                    // Create the AlertDialog
                    val alertDialog: AlertDialog = builder.create()
                    // Set other dialog properties
                    alertDialog.setCancelable(false)
                    alertDialog.show()
                }
            }).start()



        } catch (e: Exception) {

            applicationContext.logPrint("Logout error " + e.localizedMessage)
        }

    }

    fun getErrorResponse(errorBody: String): String {
        return try {
            // Parse the error response body assuming it's in JSON format
            val jsonObject = JSONObject(errorBody)

            // Check for a "message" field in the response
            if (jsonObject.has("message")) {
                jsonObject.getString("message")
            } else if (jsonObject.has("error")) {
                jsonObject.getString("error")
            } else {
                // Default error message if specific fields are not found
                "Something went wrong. Please try again."
            }
        } catch (e: Exception) {
            // Handle any errors in parsing the error body
            "Error parsing response. Please try again."
        }
    }

    fun refreshToken(applicationContext: Context) {

        val token = applicationContext.applicationContext?.let {
            SharedPreferenceHelper.getInstance(it)
                .getSharedPreferenceString(Constant.REFRESH_TOKEN, "REFRESH_TOKEN")
        }


        APIClient(applicationContext).apiService.reAuthenticate(token!!)
            .enqueue(object : Callback<LoginResponse> {
                override fun onFailure(call: Call<LoginResponse>, t: Throwable) {
//                    Toast.makeText(applicationContext, t.message, Toast.LENGTH_LONG)
//                        .show()
                    applicationContext.logPrint("onFailure " + t.message)

                }

                override fun onResponse(
                    call: Call<LoginResponse>,
                    response: Response<LoginResponse>
                ) {
                    try {

                        applicationContext.logPrint(" Response code get refresh token" + response.code())

                        if (response.code().toString() == "200") {

                            applicationContext.logPrint("refresh token got Successfully")
                            SharedPreferenceHelper.getInstance(applicationContext).setSharedPreferenceString(Constant.ACCESS_TOKEN, response.body()?.accessToken!!)
                            SharedPreferenceHelper.getInstance(applicationContext).setSharedPreferenceString(Constant.REFRESH_TOKEN, response.body()?.refreshToken!!)

                        } else {
//                            Toast.makeText(
//                                applicationContext.applicationContext,
//                                "The Error: Getting refresh token Unsuccessful",
//                                Toast.LENGTH_LONG
//                            ).show()
                            applicationContext.logPrint("The Error: Getting refresh token Unsuccessful")
                        }


                    } catch (e: Exception) {

                        e.printStackTrace()

                    }

                }
            })
    }

    fun Context.getAppVersion(context: Context): String {
        var version = ""
        try {
            val pInfo = context.packageManager.getPackageInfo(context.packageName, 0)
            version = pInfo.versionName
        } catch (e: PackageManager.NameNotFoundException) {
            e.printStackTrace()
        }

        return version
    }

    fun Context.getAppCode(context: Context): String {
        var version = ""
        try {
            val pInfo = context.packageManager.getPackageInfo(context.packageName, 0)
            version = pInfo.versionCode.toString()
        } catch (e: PackageManager.NameNotFoundException) {
            e.printStackTrace()
        }

        return version
    }


    fun getBatteryLevel(context: Context): Int {
        val batteryStatus: Intent? = IntentFilter(Intent.ACTION_BATTERY_CHANGED).let { ifilter ->
            context.registerReceiver(null, ifilter)
        }

        // Get the battery level from the intent
        val level: Int = batteryStatus?.getIntExtra(BatteryManager.EXTRA_LEVEL, -1) ?: -1
        val scale: Int = batteryStatus?.getIntExtra(BatteryManager.EXTRA_SCALE, -1) ?: -1

        // Calculate the battery percentage
        return if (level != -1 && scale != -1) {
            (level * 100 / scale.toFloat()).toInt()
        } else {
            -1
        }
    }



}