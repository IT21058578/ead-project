import {createApi, fetchBaseQuery} from '@reduxjs/toolkit/query/react'
import { BASE_URL } from '../../Utils/Generals';
import { getItem } from '../../Utils/Generals';
import RoutePaths from '../../config';
import { useState } from 'react';

const isLogged = getItem(RoutePaths.token);
// const user = !isLogged ? null : JSON.parse(getItem("user") || "");

const token = getItem(RoutePaths.token);

export const orderApiSlice = createApi({
    
    reducerPath : 'api/orders',
    baseQuery : fetchBaseQuery({baseUrl : BASE_URL ,
        prepareHeaders(headers) {
            if (token) {
              headers.set('Authorization', `Bearer ${token}`);
            }
            return headers;
          },}),
    tagTypes : ['Order'],

    /**
     * Endpoints for managing orders
     * @param {import('@reduxjs/toolkit/query/react').MutationDefinition} builder
     * @returns {import('@reduxjs/toolkit/query/react').EndpointDefinitions}
     */
    endpoints : (builder) => ({

        getAllOrder : builder.query(({
            // query : () => '/Orders/search',
            query: (page = 1, pageSize = 100, sortBy = 'Id', sortDirection = 'asc') => ({
                url: 'orders/search',
                method: 'POST',
                body: {
                  page: 1,
                  pageSize,
                  sortBy,
                  sortDirection,
                  filters: {},
                },
              }),
            providesTags : ['Order'],
            
        })),

        // createOrder: builder.mutation({
        //     query : (createOrderDto) => ({
        //         url : '/Orders',
        //         method : 'POST',
        //         body : createOrderDto,
        //         headers: {
        //             userId: `Bearer ${user._id}`,
        //           },
        //     }),
        //    invalidatesTags : ['Order']
        // }),

        updateOrder: builder.mutation({
            query : (data) => ({
                url : '/Orders/edit',
                method : 'POST',
                body : {data},

            }),
            invalidatesTags : ['Order']
        }),
       

        updateOrderStatus: builder.mutation({
            query : ({orderID, formData}) => ({
                url : `/Orders/${orderID}`,
                method : 'PUT',
                body: formData,
            }),
            invalidatesTags : ['Order']
        }),


        deleteOrder: builder.mutation({
            query : (id : string) => ({
                url : `/Orders/${id}`,
                
                method : 'DELETE'
            }),
            invalidatesTags : ['Order']
        })
        
    })
})


export const {
    useGetAllOrderQuery,
    // useCreateOrderMutation,
    useUpdateOrderMutation,
    useUpdateOrderStatusMutation,
    useDeleteOrderMutation,
 } = orderApiSlice;
