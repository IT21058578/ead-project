import { PropsWithChildren, useEffect } from "react"
import { Navigate, Outlet } from "react-router-dom"
import { getItem } from "../Utils/Generals"
import RoutePaths from "../config";
import { UserType } from "../types";
import { useAppSelector } from '../hooks/redux-hooks'

/**
 * PrivateRoute component
 * 
 * This component renders a route that requires the user to be authenticated.
 * If the user is not logged in, it redirects the user to the login page.
 * If the user is logged in, it renders the children components.
 * If the user is an admin, it redirects the user to the home page.
 * @param {{type : number, children : React.ReactNode}} props - an object with two properties: type and children.
 * @returns {React.ReactNode} - the rendered route or a redirect to the login page.
 */
const PrivateRoute = ({type = 0, children} : PropsWithChildren<{type : number}>) => {
    
    const isLogged = getItem(RoutePaths.token);
    // const find : UserType = useAppSelector(state => state.user);
    // const user = !isLogged ? null : JSON.parse(getItem('user') || '');

    // const admin = find.isVerified;

    if (!isLogged) {
        return <Navigate to={RoutePaths.login} replace />;
    }

    // if (type === 1 && admin===true) {

    //     return <Navigate to={RoutePaths.home} replace />;
    // }

    return <Outlet />;
}

export default PrivateRoute;