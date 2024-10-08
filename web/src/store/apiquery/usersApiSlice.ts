import {createApi, fetchBaseQuery} from '@reduxjs/toolkit/query/react'
import { BASE_URL } from '../../Utils/Generals';
import { getItem } from '../../Utils/Generals';
import RoutePaths from '../../config';

const token = getItem(RoutePaths.token);

export const usersApiSlice = createApi({
    
    reducerPath : 'api/users',
    baseQuery : fetchBaseQuery({baseUrl : BASE_URL ,
    /**
     * If the user is logged in, add the Authorization header with the
     * user's token to the request headers.
     * @param {Headers} headers The request headers.
     * @returns {Headers} The modified headers.
     */
        prepareHeaders(headers) {
            if (token) {
              headers.set('Authorization', `Bearer ${token}`);
            }
            return headers;
          },}),
    tagTypes : ['Users'],

    /**
     * Endpoints for the users API
     * 
     * @prop {Function} getAllUsers - Returns all users
     * @prop {Function} getUser - Returns a user given its id
     * @prop {Function} getStartistics - Returns the statistics about the numbers of users, products, orders, ...
     * @prop {Function} createUser - Creates a new user and returns it
     * @prop {Function} updateUser - Updates a user and returns it
     * @prop {Function} deleteUser - Deletes a user given its id
     * @returns {Object} An object containing all the endpoints
     */
    endpoints : (builder) => ({

        getAllUsers : builder.query(({
            // query : () => '/users/search',
            query: (page = 1, pageSize = 100, sortBy = 'Id', sortDirection = 'asc') => ({
                url: 'users/search',
                method: 'POST',
                body: {
                  page: 1,
                  pageSize,
                  sortBy,
                  sortDirection,
                  filters: {},
                },
              }),
            providesTags : ['Users'],
        })),

        getUser : builder.query({
            query : (id) => `/users/${id}`,
            providesTags : ['Users']
        }),

        getStartistics: builder.query({
            query: () => '/statistics',
        }),

        createUser: builder.mutation({
            query : (user) => ({
                url : `/user/create`,
                method : 'POST',
                body : user,
            }),
           invalidatesTags : ['Users']
        }),

        updateUser: builder.mutation({
            query : (data) => ({
                url : '/user/edit',
                method : 'POST',
                body : {_method : 'patch', ...data},
            }),
            invalidatesTags : ['Users']
        }),

        deleteUser: builder.mutation({
            query : (id : String) => ({
                url : `/users/${id}`,
                method : 'DELETE',
            }),
            invalidatesTags : ['Users']
        })
    })
})


export const {
    useGetAllUsersQuery,
    useGetUserQuery,
    useUpdateUserMutation,
    useCreateUserMutation,
    useDeleteUserMutation,
    useGetStartisticsQuery
 } = usersApiSlice;
