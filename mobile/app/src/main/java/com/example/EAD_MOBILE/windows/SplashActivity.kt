package com.example.EAD_MOBILE.windows

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import com.example.EAD_MOBILE.databinding.ActivitySplashBinding
import com.example.EAD_MOBILE.storage.SharedPreferenceHelper
import com.example.EAD_MOBILE.ustils.Constant
import com.example.EAD_MOBILE.ustils.Utility.logPrint

class SplashActivity : AppCompatActivity() {

    private lateinit var binding: ActivitySplashBinding

    override fun onCreate(savedInstanceState: Bundle?) {

        binding = ActivitySplashBinding.inflate(layoutInflater)

        super.onCreate(savedInstanceState)
        setContentView(binding.root)

        val isLoggedIn: Boolean = SharedPreferenceHelper.getInstance(applicationContext).getSharedPreferenceBoolean(
            Constant.IS_LOGGED_IN, false)

        logPrint("Login Check $isLoggedIn")

        if (isLoggedIn) {
            startActivity(Intent(this, MainActivity::class.java))
        }
        else{

            binding.loginBtnSplash.setOnClickListener {
                startActivity(Intent(this, LoginActivity::class.java))
            }
            binding.signUpBtn.setOnClickListener {
                startActivity(Intent(this, SignUpActivity::class.java))
            }
        }
    }


}