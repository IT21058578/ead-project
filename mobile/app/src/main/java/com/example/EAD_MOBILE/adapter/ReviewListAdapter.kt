package com.example.EAD_MOBILE.adapter

import android.util.Log
import android.view.LayoutInflater
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView
import com.example.EAD_MOBILE.databinding.ReviewItemviewBinding
import com.example.EAD_MOBILE.models.response.ReviewSearchResponse

class ReviewListAdapter(private val listOfReviews: List<ReviewSearchResponse.Review>) :
    RecyclerView.Adapter<ReviewListAdapter.ViewHolder>() {

    class ViewHolder(private val binding: ReviewItemviewBinding) : RecyclerView.ViewHolder(binding.root) {

        fun bind(review: ReviewSearchResponse.Review) {
            // UserId is the reviewer’s name
            binding.tvReivewName.text = "User: ${review.userId}"
            binding.tvReview.text = review.message
            binding.reviewRatingBar.rating = review.rating?.toFloat() ?: 0f

            // Log the review details for debugging
            Log.d("ReviewListAdapter", "User ID: ${review.userId}")
            Log.d("ReviewListAdapter", "Message: ${review.message}")
            Log.d("ReviewListAdapter", "Rating: ${review.rating}")
        }
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ViewHolder {
        val binding = ReviewItemviewBinding.inflate(
            LayoutInflater.from(parent.context), parent, false
        )
        return ViewHolder(binding)
    }

    override fun getItemCount(): Int {
        return listOfReviews.size
    }

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        val review = listOfReviews[position]
        holder.bind(review)
    }
}
