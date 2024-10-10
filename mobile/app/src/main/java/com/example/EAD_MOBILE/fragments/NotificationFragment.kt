package com.example.EAD_MOBILE.fragments

import android.graphics.Color
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.core.view.WindowInsetsControllerCompat
import androidx.fragment.app.Fragment
import androidx.recyclerview.widget.LinearLayoutManager
import com.example.EAD_MOBILE.adapter.NotificationListAdapter
import com.example.EAD_MOBILE.api.APIClient
import com.example.EAD_MOBILE.databinding.FragmentNotificationBinding
import com.example.EAD_MOBILE.models.request.Filter
import com.example.EAD_MOBILE.models.request.NotificationFilters
import com.example.EAD_MOBILE.models.request.NotificationSearchRequest
import com.example.EAD_MOBILE.models.responce.Notification
import com.example.EAD_MOBILE.models.responce.NotificationResponse
import com.example.EAD_MOBILE.storage.SharedPreferenceHelper
import com.example.EAD_MOBILE.ustils.Constant
import com.example.EAD_MOBILE.utils.showNotification
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class NotificationFragment : Fragment() {

    private lateinit var binding: FragmentNotificationBinding
    private lateinit var apiClient: APIClient
    private lateinit var notificationListAdapter: NotificationListAdapter

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        binding = FragmentNotificationBinding.inflate(inflater, container, false)
        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        // Initialize API client
        apiClient = APIClient(requireContext())

        // Set up RecyclerView layout
        binding.rvNotificatoinList.layoutManager = LinearLayoutManager(requireContext())

        // Customize status bar
        requireActivity().window.statusBarColor = Color.parseColor("#FFFFFF")
        WindowInsetsControllerCompat(requireActivity().window, requireActivity().window.decorView)
            .isAppearanceLightStatusBars = true

        // Load notifications from API
        loadNotifications()
    }

    private fun loadNotifications() {
        // Retrieve the logged-in user's UserId from SharedPreferences
        val userId = SharedPreferenceHelper.getInstance(requireContext())
            .getSharedPreferenceString(Constant.USER_ID, "")

        // Check if UserId is valid
        if (userId.isNullOrEmpty()) {
            Toast.makeText(requireContext(), "Error: User not logged in", Toast.LENGTH_SHORT).show()
            return
        }

        // Create the filters object
        val filters = NotificationFilters(
            userId = Filter(operator = "Equals", value = userId)
        )

        // Create the NotificationSearchRequest object
        val notificationSearchRequest = NotificationSearchRequest(
            filters = filters // Pass the filters object
        )

        // Make the API call to search notifications
        apiClient.apiService.searchNotifications(notificationSearchRequest).enqueue(object : Callback<NotificationResponse> {
            override fun onResponse(call: Call<NotificationResponse>, response: Response<NotificationResponse>) {
                if (response.isSuccessful && response.body() != null) {
                    val notifications = response.body()!!.data
                    if (notifications != null && notifications.isNotEmpty()) {
                        // Bind notifications to the RecyclerView
                        showNotifications(notifications)
                        // Show each notification as a real notification
                        for (notification in notifications) {
                            showNotification(requireContext(), notification.type, notification.reason)
                        }
                    } else {
                        Toast.makeText(requireContext(), "No notifications found.", Toast.LENGTH_SHORT).show()
                    }
                } else {
                    Toast.makeText(requireContext(), "Failed to retrieve notifications.", Toast.LENGTH_SHORT).show()
                }
            }

            override fun onFailure(call: Call<NotificationResponse>, t: Throwable) {
                Toast.makeText(requireContext(), "Error: ${t.message}", Toast.LENGTH_SHORT).show()
            }
        })
    }

    private fun showNotifications(notifications: List<Notification>) {
        notificationListAdapter = NotificationListAdapter(notifications)
        binding.rvNotificatoinList.adapter = notificationListAdapter
    }
}
