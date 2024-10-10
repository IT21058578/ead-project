package com.example.EAD_MOBILE.fragments

import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentTransaction
import com.example.EAD_MOBILE.R
import com.example.EAD_MOBILE.api.APIClient
import com.example.EAD_MOBILE.databinding.FragmentProfileBinding
import com.example.EAD_MOBILE.models.responce.UserDetailsResponse
import com.example.EAD_MOBILE.storage.SharedPreferenceHelper
import com.example.EAD_MOBILE.ustils.Constant
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class ProfileFragment : Fragment() {

    private lateinit var binding: FragmentProfileBinding
    private lateinit var apiClient: APIClient

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = FragmentProfileBinding.inflate(inflater, container, false)
        apiClient = APIClient(requireContext())

        binding.editProfileBtn.setOnClickListener {
            navigateToEditProfile()
        }

        // Fetch and display user details
        fetchUserDetails()

        return binding.root
    }


    private fun fetchUserDetails() {
        val userId = SharedPreferenceHelper.getInstance(requireContext()).getSharedPreferenceString(Constant.USER_ID, "")

        if (!userId.isNullOrEmpty()) {
            apiClient.apiService.getUserById(userId).enqueue(object : Callback<UserDetailsResponse> {
                override fun onResponse(call: Call<UserDetailsResponse>, response: Response<UserDetailsResponse>) {
                    if (response.isSuccessful) {
                        val userDetails = response.body()

                        // Display user details
                        binding.firstNameProfile.text = userDetails?.firstName
                        binding.lastNameProfile.text = userDetails?.lastName
                        binding.userEmailProfile.text = userDetails?.email
                    } else {
                        // Handle unsuccessful response
                        Log.e("ProfileFragment", "Failed to fetch user details: ${response.code()}")
                    }
                }

                override fun onFailure(call: Call<UserDetailsResponse>, t: Throwable) {
                    // Handle network error
                    Log.e("ProfileFragment", "Error fetching user details: ${t.message}")
                }
            })
        }
    }

    private fun navigateToEditProfile() {
        // Create an instance of the EditProfileFragment
        val editProfileFragment = EditProfileFragment()

        // Replace the current fragment with the EditProfileFragment
        val transaction: FragmentTransaction = requireActivity().supportFragmentManager.beginTransaction()
        transaction.replace(R.id.fragment_container, editProfileFragment) // Make sure fragment_container is the ID of your Fragment layout
        transaction.addToBackStack(null) // Optional: Add this transaction to the back stack
        transaction.commit()
    }
}
