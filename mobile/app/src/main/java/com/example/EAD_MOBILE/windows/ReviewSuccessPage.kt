package com.example.EAD_MOBILE.windows

import android.content.Intent
import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.example.EAD_MOBILE.R
import com.example.EAD_MOBILE.databinding.ActivityReviewSuccessPageBinding

class ReviewSuccessPage : AppCompatActivity() {

    private lateinit var binding: ActivityReviewSuccessPageBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityReviewSuccessPageBinding.inflate(layoutInflater)
        setContentView(binding.root)

        binding.continueBtn.setOnClickListener {
            startActivity(Intent(this, MainActivity::class.java))
        }
    }



}