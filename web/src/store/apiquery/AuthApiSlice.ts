import {createApi, fetchBaseQuery} from '@reduxjs/toolkit/query/react'
import { BASE_URL } from '../../Utils/Generals';


export const authApiSlice = createApi({
    
    reducerPath : 'api/auth',
    baseQuery : fetchBaseQuery({baseUrl : BASE_URL}),
    tagTypes : ['Auth'],

    /**
     * Endpoints for authentication.
     * @property {function} refresh - Refresh user's token.
     * @property {function} login - Login user.
     * @property {function} register - Register a new user.
     */
    endpoints : (builder) => ({

        refresh : builder.query(({
            query : () => '/auth/authorize',
            providesTags : ['Auth']
        })),

        login: builder.mutation({
        /**
         * Login user.
         * @param {Object} category - User's email and password.
         * @returns {Promise<Object>} - The user's token.
         */
            query : (category) => ({
                url : `/auth/login`,
                method : 'POST',
                body : category,
            }),
           invalidatesTags : ['Auth']
        }),

        register: builder.mutation({
        /**
         * Register a new user.
         * @param {Object} userDto - The user to register.
         * @returns {Promise<Object>} - The user's token.
         */
            query : (userDto) => ({
                url : '/auth/register',
                method : 'POST',
                body : userDto,
            }),
            invalidatesTags : ['Auth']
        })
    })
})


export const {
    useRefreshQuery,
    useLoginMutation,
    useRegisterMutation
 } = authApiSlice;
