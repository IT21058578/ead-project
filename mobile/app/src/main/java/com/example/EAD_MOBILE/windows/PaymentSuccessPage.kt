package com.example.EAD_MOBILE.windows

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import com.example.EAD_MOBILE.databinding.ActivityPaymentSuccessPageBinding

class PaymentSuccessPage : AppCompatActivity() {

    private lateinit var binding: ActivityPaymentSuccessPageBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityPaymentSuccessPageBinding.inflate(layoutInflater)
        setContentView(binding.root)
        binding.loginBtn.setOnClickListener {
            startActivity(Intent(this, MainActivity::class.java))
        }
    }
}