import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { BASE_URL } from "../../Utils/Generals";
import { getItem } from "../../Utils/Generals";
import RoutePaths from "../../config";

const token = getItem(RoutePaths.token);
const userRole = getItem('userRole');

export const notificationsApiSlice = createApi({
  reducerPath: "api/notifications",
  baseQuery: fetchBaseQuery({
    baseUrl: BASE_URL,
  /**
   * If the user is logged in, add the Authorization header with the
   * user's token to the request headers.
   * @param {Headers} headers The request headers.
   * @returns {Headers} The modified headers.
   */
    prepareHeaders(headers) {
      if (token) {
        headers.set("Authorization", `Bearer ${token}`);
      }
      return headers;
    },
  }),
  tagTypes: ["Notifications"],

  /**
   * The endpoints for the notifications API
   * 
   * - getAllNotifications: returns all notifications
   * - getNotification: returns a single notification by id
   * - searchNotification: returns a list of notifications that match the given query
   * - createNotification: creates a new notification
   * - updateNotification: updates a notification
   * - deleteNotification: deletes a notification
   * 
   * @param {import("@reduxjs/toolkit").MutationDefinition<ApiState, string, never, never>} builder - the builder object from createApi
   * @returns {import("@reduxjs/toolkit").MutationDefinition<ApiState, string, never, never>} - the endpoints object
   */
  endpoints: (builder) => ({
    getAllNotifications: builder.query({
      // query: () => "/notifications/search",
      query: (page = 1, pageSize = 100, sortBy = 'UserId', sortDirection = 'asc') => ({
        url: 'notifications/search',
        method: 'POST',
        body: {
          page: 1,
          pageSize,
          sortBy,
          sortDirection,
          filters: {
            Recipient: {
              operator: "Equals",
              value: userRole
            }
          },
        },
      }),

      providesTags: ["Notifications"],
    }),

    getNotification: builder.query({
      query: (id: string) => `/notifications/${id}`,
      providesTags: ["Notifications"],
    }),

    searchNotification: builder.query({
      query: (query: string) => `/notifications/search/${query}`,
      providesTags: ["Notifications"],
    }),

    createNotification: builder.mutation({
      query: ({ formData }) => ({
        url: "/notifications",
        method: "POST",
        body: formData,
      }),
      invalidatesTags: ["Notifications"],
    }),

    updateNotification: builder.mutation({
      query: ({ notificationId, formData }) => ({
        url: `/notifications/${notificationId}`,
        method: "PUT",
        body: formData,
      }),
      invalidatesTags: ["Notifications"],
    }),

    deleteNotification: builder.mutation({
      query: (id: String) => ({
        url: `/notifications/${id}`,
        method: "DELETE",
      }),
      invalidatesTags: ["Notifications"],
    }),
      
  }),
});

export const {
  useGetAllNotificationsQuery,
  useGetNotificationQuery,
  useSearchNotificationQuery,
  useUpdateNotificationMutation,
  useCreateNotificationMutation,
  useDeleteNotificationMutation,
} = notificationsApiSlice;
