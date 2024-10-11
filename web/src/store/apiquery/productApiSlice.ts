import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { BASE_URL } from "../../Utils/Generals";
import { getItem } from "../../Utils/Generals";
import RoutePaths from "../../config";

const token = getItem(RoutePaths.token);
const userId = getItem('userId');

export const productApiSlice = createApi({
  reducerPath: "api/products",
  baseQuery: fetchBaseQuery({
    baseUrl: BASE_URL,
    prepareHeaders(headers) {
      if (token) {
        headers.set("Authorization", `Bearer ${token}`);
      }
      return headers;
    },
  }),
  tagTypes: ["Products"],

  /**
   * Endpoints for the product API
   * 
   * @prop {Function} getAllProducts - Returns all products
   * @prop {Function} getProduct - Returns a product given its id
   * @prop {Function} searchProduct - Returns a list of products given a search query
   * @prop {Function} getRandomProduct - Returns a random product
   * @prop {Function} getBestProducts - Returns a list of best selling products
   * @prop {Function} createProduct - Creates a new product and returns it
   * @prop {Function} updateProduct - Updates a product and returns it
   * @prop {Function} deleteProduct - Deletes a product given its id
   * @prop {Function} downloadProductReports - Returns a list of products
   * @prop {Function} uploadImages - Uploads images for a product
   * @prop {Function} getRecommendations - Returns a list of recommended products
   * @returns {Object} An object containing all the endpoints
   */
  endpoints: (builder) => ({
    getAllProducts: builder.query({
      // query: () => "/products/search",
      query: (page = 1, pageSize = 100, sortBy = 'Id', sortDirection = 'asc') => ({
        url: 'products/search',
        method: 'POST',
        body: {
          page: 1,
          pageSize,
          sortBy,
          sortDirection,
          filters: {},
        },
      }),

      providesTags: ["Products"],
    }),

    getVendorProducts: builder.query({
      query: (page = 1, pageSize = 100, sortBy = 'Id', sortDirection = 'asc') => ({
        url: 'products/search',
        method: 'POST',
        body: {
          page: 1,
          pageSize,
          sortBy,
          sortDirection,
          filters: {
            VendorId: {
              operator: "Equals",
              value: userId
            }
          },
        },
      }),

      providesTags: ["Products"],
    }),

    getProduct: builder.query({
      query: (id: string) => `/products/${id}`,
      providesTags: ["Products"],
    }),

    searchProduct: builder.query({
      query: (query: string) => `/products/search/${query}`,
      providesTags: ["Products"],
    }),

    getRandomProduct: builder.query({
      query: () => `/product/types/random`,
      providesTags: ["Products"],
    }),

    getBestProducts: builder.query({
      query: () => `/product/types/best-sellers`,
      providesTags: ["Products"],
    }),

    createProduct: builder.mutation({
      query: ({ formData }) => ({
        url: "/products",
        method: "POST",
        body: formData,
      }),
      invalidatesTags: ["Products"],
    }),

    updateProduct: builder.mutation({
      query: ({ productId, formData }) => ({
        url: `/products/${productId}`,
        method: "PUT",
        body: formData,
      }),
      invalidatesTags: ["Products"],
    }),

    deleteProduct: builder.mutation({
      query: (id: String) => ({
        url: `/products/${id}`,
        method: "DELETE",
      }),
      invalidatesTags: ["Products"],
    }),

    downloadProductReports: builder.query({
      query: () => "/products/reports",
      providesTags: ["Products"],
    }),

    uploadImages: builder.mutation({
      query: (file) => ({
        url: "/products/images",
        method: "POST",
        body: file,
      }),
      invalidatesTags: ["Products"],
    }),

    getRecommendations: builder.query({
      query: (getRecommendationsDto) => ({
        url: 'products/recommendations',
        method: 'POST',
        body: getRecommendationsDto,
      }),
    }),
      
  }),
});

export const {
  useGetAllProductsQuery,
  useGetVendorProductsQuery,
  useGetProductQuery,
  useSearchProductQuery,
  useGetRandomProductQuery,
  useGetBestProductsQuery,
  useUpdateProductMutation,
  useCreateProductMutation,
  useDeleteProductMutation,
  useDownloadProductReportsQuery,
  useUploadImagesMutation,
  useGetRecommendationsQuery
} = productApiSlice;
