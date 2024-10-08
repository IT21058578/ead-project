import { checkLogin } from '../Utils/Generals'
import { Navigate, Outlet } from 'react-router-dom'
import RoutePaths from '../config'

/**
 * If the user is logged in, redirect them to the home page.
 * Otherwise, render the child components.
 * @returns {JSX.Element}
 */
const RedirectIfAuthenticate = () => {

  

    return <Outlet />
}

export default RedirectIfAuthenticate