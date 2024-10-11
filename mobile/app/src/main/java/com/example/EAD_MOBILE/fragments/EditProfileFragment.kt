package com.example.EAD_MOBILE.fragments

import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentTransaction
import com.example.EAD_MOBILE.R
import com.example.EAD_MOBILE.api.APIClient
import com.example.EAD_MOBILE.databinding.FragmentEditProfileBinding
import com.example.EAD_MOBILE.models.request.UserUpdateRequest
import com.example.EAD_MOBILE.models.responce.UserDetailsResponse
import com.example.EAD_MOBILE.storage.SharedPreferenceHelper
import com.example.EAD_MOBILE.ustils.Constant
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class EditProfileFragment : Fragment() {

    private lateinit var binding: FragmentEditProfileBinding
    private lateinit var apiClient: APIClient

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = FragmentEditProfileBinding.inflate(inflater, container, false)
        apiClient = APIClient(requireContext())

        // Load user details into EditTexts
        loadUserDetails()

        // Set up the click listener for the save button
        binding.saveProfileBtn.setOnClickListener {
            updateUserProfile()
        }

        return binding.root
    }

    private fun loadUserDetails() {
        val userId = SharedPreferenceHelper.getInstance(requireContext()).getSharedPreferenceString(Constant.USER_ID, "")
        Log.d("USERID","userId fetching ${userId}")

        if (!userId.isNullOrEmpty()) {
            apiClient.apiService.getUserById(userId).enqueue(object : Callback<UserDetailsResponse> {
                override fun onResponse(call: Call<UserDetailsResponse>, response: Response<UserDetailsResponse>) {
                    if (response.isSuccessful) {
                        val userDetails = response.body()
                        // Set the fetched user details into EditText fields
                        binding.firstNameProfile.setText(userDetails?.firstName)
                        binding.lastNameProfile.setText(userDetails?.lastName)
                    } else {
                        Log.e("EditProfileFragment", "Failed to fetch user details: ${response.code()}")
                        Toast.makeText(requireContext(), "Failed to load user details", Toast.LENGTH_SHORT).show()
                    }
                }

                override fun onFailure(call: Call<UserDetailsResponse>, t: Throwable) {
                    Log.e("EditProfileFragment", "Error fetching user details: ${t.message}")
                    Toast.makeText(requireContext(), "Error fetching user details", Toast.LENGTH_SHORT).show()
                }
            })
        }
    }

    private fun updateUserProfile() {
        val userId = SharedPreferenceHelper.getInstance(requireContext()).getSharedPreferenceString(Constant.USER_ID, "") ?: ""
        val updatedFirstName = binding.firstNameProfile.text.toString()
        val updatedLastName = binding.lastNameProfile.text.toString()

        // Create the user update request object
        val userUpdateRequest = UserUpdateRequest().apply {
            firstName = updatedFirstName
            lastName = updatedLastName
        }

        apiClient.apiService.editUserById(userId, userUpdateRequest).enqueue(object : Callback<UserDetailsResponse> {
            override fun onResponse(call: Call<UserDetailsResponse>, response: Response<UserDetailsResponse>) {
                if (response.isSuccessful) {
                    // Notify user of successful update
                    Log.i("EditProfileFragment", "Profile updated successfully")
                    Toast.makeText(requireContext(), "Profile updated successfully", Toast.LENGTH_SHORT).show()
                    navigateToEditProfile()

                } else {
                    Log.e("EditProfileFragment", "Failed to update profile: ${response.code()}")
                    Toast.makeText(requireContext(), "Failed to update profile", Toast.LENGTH_SHORT).show()
                }
            }

            override fun onFailure(call: Call<UserDetailsResponse>, t: Throwable) {
                Log.e("EditProfileFragment", "Error updating profile: ${t.message}")
                Toast.makeText(requireContext(), "Error updating profile", Toast.LENGTH_SHORT).show()
            }
        })
    }

    private fun navigateToEditProfile() {
        // Create an instance of the EditProfileFragment
        val profileFragment = ProfileFragment()

        // Replacing the current fragment with the EditProfileFragment
        val transaction: FragmentTransaction = requireActivity().supportFragmentManager.beginTransaction()
        transaction.replace(R.id.fragment_container, profileFragment)
        transaction.addToBackStack(null)
        transaction.commit()
    }

}
