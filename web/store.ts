import { configureStore } from "@reduxjs/toolkit";
import { productApiSlice } from "./src/store/apiquery/productApiSlice";
import { categoryApiSlice } from "./src/store/apiquery/categoryApiSlice";
import { authApiSlice } from "./src/store/apiquery/AuthApiSlice";
import { slideApiSlice } from "./src/store/apiquery/slideApiSlice";
import { usersApiSlice } from "./src/store/apiquery/usersApiSlice";
import { userSlice } from "./src/store/userSlice";
import { commandApiSlice } from "./src/store/apiquery/CommandApiSlice";
import { reviewApiSlice } from "./src/store/apiquery/ReviewApiSlice";
import { orderApiSlice } from "./src/store/apiquery/OrderApiSlice";
import { notificationsApiSlice } from "./src/store/apiquery/notificationsApiSlice";

export const store = configureStore({
    reducer : {
        [productApiSlice.reducerPath] : productApiSlice.reducer,
        [authApiSlice.reducerPath] : authApiSlice.reducer,
        [categoryApiSlice.reducerPath] : categoryApiSlice.reducer,
        [slideApiSlice.reducerPath] : slideApiSlice.reducer,
        [usersApiSlice.reducerPath] : usersApiSlice.reducer,
        [commandApiSlice.reducerPath] : commandApiSlice.reducer,
        [reviewApiSlice.reducerPath] : reviewApiSlice.reducer,
        [orderApiSlice.reducerPath] : orderApiSlice.reducer,
        [notificationsApiSlice.reducerPath] : notificationsApiSlice.reducer,
        user : userSlice.reducer,
    },
    
    /**
     * Middleware to handle all the RTK-Query APIs in the application.
     * It combines all the individual middlewares from each API slice into one.
     * This is what makes RTK-Query work with Redux Toolkit.
     * @param {getDefaultMiddleware} getDefaultMiddleware - A function that returns the default middleware in Redux Toolkit.
     * @returns {Array<Middleware>} - An array of middleware functions that can be used in the Redux store.
     */
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware()
        .concat(
            productApiSlice.middleware,
            categoryApiSlice.middleware,
            authApiSlice.middleware,
            slideApiSlice.middleware,
            usersApiSlice.middleware,
            commandApiSlice.middleware,
            reviewApiSlice.middleware,
            orderApiSlice.middleware,
            notificationsApiSlice.middleware
        ),
})

// Infer the `RootState` and `AppDispatch` types from the store itself
export type RootState = ReturnType<typeof store.getState>
// Inferred type: {posts: PostsState, comments: CommentsState, users: UsersState}
export type AppDispatch = typeof store.dispatch