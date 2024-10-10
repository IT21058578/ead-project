package com.example.EAD_MOBILE.fragments

import android.content.Intent
import android.graphics.Color
import android.os.Build
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.annotation.RequiresApi
import androidx.core.widget.addTextChangedListener
import androidx.fragment.app.Fragment
import androidx.fragment.app.viewModels // Make sure this is imported
import androidx.recyclerview.widget.GridLayoutManager
import com.example.EAD_MOBILE.MyApplication
import com.example.EAD_MOBILE.adapter.ProductListAdapter
import com.example.EAD_MOBILE.api.APIClient
import com.example.EAD_MOBILE.databinding.FragmentHomeBinding
import com.example.EAD_MOBILE.models.request.ProductSearchRequest
import com.example.EAD_MOBILE.models.responce.ProductSearchResponse
import com.example.EAD_MOBILE.storage.SharedPreferenceHelper
import com.example.EAD_MOBILE.ustils.Constant
import com.example.EAD_MOBILE.ustils.Utility.isInternetAvailable
import com.example.EAD_MOBILE.ustils.Utility.logPrint
import com.example.EAD_MOBILE.ustils.Utility.showErrorToast
import com.example.EAD_MOBILE.viewModels.CartViewModel
import com.example.EAD_MOBILE.viewModels.CartViewModelFactory
import com.example.EAD_MOBILE.windows.DetailsPage

import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class HomeFragment : Fragment() {

    private var _binding: FragmentHomeBinding? = null
    private val binding get() = _binding!!
    private var productList: List<ProductSearchResponse.ProductData> = listOf()
    private var id: Long? = null

    private val cartViewModel: CartViewModel by viewModels {
        CartViewModelFactory((requireActivity().application as MyApplication).cartRepository, id ?: 0 )
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentHomeBinding.inflate(inflater, container, false)
        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        // Setup RecyclerView
        binding.rvProductListing.layoutManager = GridLayoutManager(requireContext(), 2)


        // Set up the search bar listener
        setupSearchBar()

        // Fetch products from API
        fetchProducts()
    }

    private fun setupSearchBar() {
        binding.etSearch.addTextChangedListener { text ->
            filterProducts(text.toString())
        }
    }

    private fun filterProducts(query: String) {
        val filteredList = productList.filter { product ->
            // Check if the product name contains the query
            val matchesName = product.name!!.contains(query, ignoreCase = true)

            val matchesOrigin = product.category!!.contains(query, ignoreCase = true)

            // Check if the product price matches the query
            val matchesPrice = query.toDoubleOrNull()?.let { priceQuery ->
                product.price == priceQuery
            } ?: false

            // Return true if either the name or price matches
            matchesName || matchesPrice || matchesOrigin
        }
        setUpProductListing(filteredList)
    }
    private fun setUpProductListing(productList: List<ProductSearchResponse.ProductData>) {
        val adapter = ProductListAdapter(productList,cartViewModel)

        adapter.setOnClickListener(object : ProductListAdapter.OnClickListener {
            override fun onClick(productId: String) {
                val intent = Intent(requireContext(), DetailsPage::class.java)
                intent.putExtra("productId", productId)  // Send product ID to DetailsPage
                startActivity(intent)
            }
        })
        binding.rvProductListing.adapter = adapter
    }

    private fun fetchProducts() {
        if (context?.isInternetAvailable() == true) {
            val request = ProductSearchRequest()
            request.page = 1
            request.pageSize = 10
            request.sortBy = "Id"
            request.sortDirection = "desc"
            request.filters = mapOf()

            val token = context?.let {
                SharedPreferenceHelper.getInstance(it)
                    .getSharedPreferenceString(Constant.ACCESS_TOKEN, "ACCESS_TOKEN")
            }

            APIClient(requireContext()).apiService.searchProduct(request)
                .enqueue(object : Callback<ProductSearchResponse> {
                    @RequiresApi(Build.VERSION_CODES.Q)
                    override fun onResponse(
                        call: Call<ProductSearchResponse>,
                        response: Response<ProductSearchResponse>
                    ) {
                        if (response.isSuccessful && response.body() != null) {
                            val productResponse = response.body()
                            productList = productResponse?.data ?: listOf()
                            setUpProductListing(productList)
                        } else {
                            Toast.makeText(
                                requireContext(),
                                "Product failed with code: " + response.code(),
                                Toast.LENGTH_LONG
                            ).show()
                        }
                    }

                    override fun onFailure(call: Call<ProductSearchResponse>, t: Throwable) {
                        Toast.makeText(requireContext(), t.message, Toast.LENGTH_LONG).show()
                    }
                })

        } else {
            context?.showErrorToast("Internet not available. Please try again!!")
        }
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}
