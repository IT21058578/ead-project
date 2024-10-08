import {createApi, fetchBaseQuery} from '@reduxjs/toolkit/query/react'
import { BASE_URL } from '../../Utils/Generals';
import { getItem } from '../../Utils/Generals';
import RoutePaths from '../../config';

const token = getItem(RoutePaths.token);


export const reviewApiSlice = createApi({
    
    reducerPath : 'api/reviews',
    baseQuery : fetchBaseQuery({baseUrl : BASE_URL ,
        prepareHeaders(headers) {
            if (token) {
              headers.set('Authorization', `Bearer ${token}`);
            }
            return headers;
          },}),
    tagTypes : ['Review'],

    /**
     * Endpoints for the review API
     * 
     * @remarks
     * The endpoints are:
     * - `getAllReview` : POST /reviews/search
     *     - query: {page: 1, pageSize: 100, sortBy: "Id", sortDirection: "asc", filters: {}},
     *     - providesTags: ["Review"]
     * - `createReview` : POST /reviews
     *     - query: {userId, review} ,
     *     - invalidatesTags: ["Review"]
     * - `downloadReviewsReports` : GET /reviews/
     *     - query: () => '/reviews/'
     * - `deleteReview` : DELETE /reviews/:id
     *     - query: (reviewId: String) => ({url: `/reviews/${reviewId}`, method: "DELETE"}),
     *     - invalidatesTags: ["Review"]
     * - `updateProduct` : PUT /reviews/:id
     *     - query: ({ reviewID, formData }) => ({url: `/reviews/${reviewID}`, method: "PUT", body: formData}),
     *     - invalidatesTags: ["Review"]
     */
    endpoints : (builder) => ({

        getAllReview : builder.query(({
            // query : () => '/reviews/search',
            query: (page = 1, pageSize = 100, sortBy = 'Id', sortDirection = 'asc') => ({
              url: 'reviews/search',
              method: 'POST',
              body: {
                page: 1,
                pageSize,
                sortBy,
                sortDirection,
                filters: {},
              },
            }),
            providesTags : ['Review']
        })),

        createReview: builder.mutation({
            query : ({userId,review}) => ({
                url : '/reviews',
                method : 'POST',
                user: userId,
                body : review,
            }),
           invalidatesTags : ['Review']
        }),

        downloadReviewsReports: builder.query(({
            query : () => '/reviews/'
        })),

        deleteReview: builder.mutation({
            query: (reviewId: String) => ({
              url: `/reviews/${reviewId}`,
              method: "DELETE",
            }),
            invalidatesTags: ["Review"],
        }),

        updateProduct: builder.mutation({
            query: ({ reviewID, formData }) => ({
              url: `/reviews/${reviewID}`,
              method: "PUT",
              body: formData,
            }),
            invalidatesTags: ["Review"],
          }),

    })
})


export const {
    useGetAllReviewQuery,
    useCreateReviewMutation,
    useDeleteReviewMutation,
    useUpdateProductMutation
 } = reviewApiSlice;
