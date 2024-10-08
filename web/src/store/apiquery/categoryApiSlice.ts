import {createApi, fetchBaseQuery} from '@reduxjs/toolkit/query/react'
import { BASE_URL } from '../../Utils/Generals';

export const categoryApiSlice = createApi({
    
    reducerPath : 'api/categories',
    baseQuery : fetchBaseQuery({baseUrl : BASE_URL}),
    tagTypes : ['Categories'],

    /**
     * Endpoints for the category API
     * 
     * @prop {Function} getAllCategories - Returns all categories
     * @prop {Function} getCategory - Returns a category given its id
     * @prop {Function} createCategory - Creates a new category and returns it
     * @prop {Function} updateCategory - Updates a category and returns it
     * @prop {Function} deleteCategory - Deletes a category given its id
     * @returns {Object} An object containing all the endpoints
     */
    endpoints : (builder) => ({

        getAllCategories : builder.query(({
            query : () => '/category',
            providesTags : ['Categories']
        })),

        getCategory : builder.query({
            query : (category) => `/category/${category.id}`,
            providesTags : ['Categories']
        }),

        createCategory: builder.mutation({
            query : (category) => ({
                url : `/category/create`,
                method : 'POST',
                body : category,
            }),
           invalidatesTags : ['Categories']
        }),

        updateCategory: builder.mutation({
            query : (data) => ({
                url : 'category/edit',
                method : 'POST',
                body : data,
            }),
            invalidatesTags : ['Categories']
        }),

        deleteCategory: builder.mutation({
            query : (id : number) => ({
                url : '/category/delete',
                method : 'DELETE',
                body : {id}
            }),
            invalidatesTags : ['Categories']
        })
    })
})


export const {
    useGetAllCategoriesQuery,
    useGetCategoryQuery,
    useUpdateCategoryMutation,
    useCreateCategoryMutation,
    useDeleteCategoryMutation,
 } = categoryApiSlice;
