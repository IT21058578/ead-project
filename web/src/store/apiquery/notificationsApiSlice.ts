import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { BASE_URL } from "../../Utils/Generals";
import { getItem } from "../../Utils/Generals";
import RoutePaths from "../../config";

const token = getItem(RoutePaths.token);

export const productApiSlice = createApi({
  reducerPath: "api/notifications",
  baseQuery: fetchBaseQuery({
    baseUrl: BASE_URL,
    prepareHeaders(headers) {
      if (token) {
        headers.set("Authorization", `Bearer ${token}`);
      }
      return headers;
    },
  }),
  tagTypes: ["Notifications"],

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
          filters: {},
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
} = productApiSlice;
