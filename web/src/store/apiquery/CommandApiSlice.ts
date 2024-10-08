import {createApi, fetchBaseQuery} from '@reduxjs/toolkit/query/react'
import { BASE_URL } from '../../Utils/Generals';

export const commandApiSlice = createApi({
    
    reducerPath : 'api/commands',
    baseQuery : fetchBaseQuery({baseUrl : BASE_URL}),
    tagTypes : ['Commands'],

    /**
     * The endpoints for the commands API
     *
     * @remarks
     * The endpoints are:
     *
     * - `getAllCommands` : GET /command
     * - `getCommand` : GET /command/:id
     * - `createCommand` : POST /command/create
     * - `updateCommand` : POST /command/edit
     * - `deleteCommand` : DELETE /command/delete
     *
     * The tags are:
     *
     * - `Commands`
     */
    endpoints : (builder) => ({

        getAllCommands : builder.query(({
            query : () => '/command',
            providesTags : ['Commands']
        })),

        getCommand : builder.query({
            query : (id) => `/command/${id}`,
            providesTags : ['Commands']
        }),

        createCommand: builder.mutation({
            query : (command) => ({
                url : `/command/create`,
                method : 'POST',
                body : command,
            }),
           invalidatesTags : ['Commands']
        }),

        updateCommand: builder.mutation({
            query : (data) => ({
                url : 'command/edit',
                method : 'POST',
                body : data,
            }),
            invalidatesTags : ['Commands']
        }),

        deleteCommand: builder.mutation({
            query : (id : number) => ({
                url : '/command/delete',
                method : 'DELETE',
                body : {id}
            }),
            invalidatesTags : ['Commands']
        })
    })
})


export const {
    useGetAllCommandsQuery,
    useGetCommandQuery,
    useUpdateCommandMutation,
    useCreateCommandMutation,
    useDeleteCommandMutation,
 } = commandApiSlice;
