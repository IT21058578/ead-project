package com.example.EAD_MOBILE.windows

import android.annotation.SuppressLint
import android.content.Intent
import android.os.Bundle
import android.view.MenuItem
import android.widget.Toast
import androidx.appcompat.app.ActionBarDrawerToggle
import androidx.appcompat.app.AppCompatActivity
import androidx.appcompat.widget.AppCompatImageButton
import androidx.appcompat.widget.Toolbar
import androidx.core.view.GravityCompat
import androidx.drawerlayout.widget.DrawerLayout
import com.example.EAD_MOBILE.R
import com.example.EAD_MOBILE.fragments.HomeFragment
import com.example.EAD_MOBILE.fragments.NotificationFragment
import com.example.EAD_MOBILE.fragments.OrderFragment
import com.example.EAD_MOBILE.fragments.ProfileFragment
import com.example.EAD_MOBILE.storage.SharedPreferenceHelper
import com.example.EAD_MOBILE.ustils.Constant
import com.google.android.material.navigation.NavigationView

class MainActivity : AppCompatActivity(), NavigationView.OnNavigationItemSelectedListener {
    private lateinit var drawerLayout: DrawerLayout

    @SuppressLint("MissingInflatedId")
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)
        drawerLayout = findViewById<DrawerLayout>(R.id.drawer_layout)
        val toolbar = findViewById<Toolbar>(R.id.toolbar)
        setSupportActionBar(toolbar)
        val navigationView = findViewById<NavigationView>(R.id.nav_view)
        navigationView.setNavigationItemSelectedListener(this)
        val toggle = ActionBarDrawerToggle(this, drawerLayout, toolbar, R.string.open_nav, R.string.close_nav)
        drawerLayout.addDrawerListener(toggle)
        toggle.syncState()
        if (savedInstanceState == null) {
            supportFragmentManager.beginTransaction()
                .replace(R.id.fragment_container, HomeFragment()).commit()
            navigationView.setCheckedItem(R.id.nav_home)
        }

        val ibOrders = findViewById<AppCompatImageButton>(R.id.ib_orders)

        // Handle orders button click
        ibOrders.setOnClickListener {
            startActivity(Intent(this, ActivityCart::class.java))
        }
    }
    override fun onNavigationItemSelected(item: MenuItem): Boolean {
        when (item.itemId) {
            R.id.nav_home -> supportFragmentManager.beginTransaction()
                .replace(R.id.fragment_container, HomeFragment()).commit()
            R.id.nav_settings -> supportFragmentManager.beginTransaction()
                .replace(R.id.fragment_container, OrderFragment()).commit()
            R.id.nav_share -> supportFragmentManager.beginTransaction()
                .replace(R.id.fragment_container, ProfileFragment()).commit()
            R.id.nav_notification -> supportFragmentManager.beginTransaction()
                .replace(R.id.fragment_container, NotificationFragment()).commit()
            R.id.nav_logout -> {
                val intent = Intent(this, LoginActivity::class.java)
                startActivity(intent)
                SharedPreferenceHelper.getInstance(applicationContext).setSharedPreferenceBoolean(
                    Constant.IS_LOGGED_IN, false)
                finish()

            }
        }
        drawerLayout.closeDrawer(GravityCompat.START)
        return true
    }
    @SuppressLint("MissingSuperCall")
    override fun onBackPressed() {
        if (drawerLayout.isDrawerOpen(GravityCompat.START)) {
            drawerLayout.closeDrawer(GravityCompat.START)
        } else {
            onBackPressedDispatcher.onBackPressed()
        }
    }
}