import React, { useEffect } from 'react'
import AdminHeader from '../../components/Admin/AdminHeader'
import AdminFooter from '../../components/Admin/AdminFooter'
import { Link, useNavigate } from 'react-router-dom'
import { getItem, removeItem, toggleLinkClass } from '../../Utils/Generals'
import RoutePaths from '../../config'
import Swal from 'sweetalert2'
import { useAppDispatch } from '../../hooks/redux-hooks'
import { logoutCurrentUser } from '../../store/userSlice'


/**
 * AdminPanel component
 * 
 * This component renders the admin panel with the following:
 * - AdminHeader
 * - AdminFooter
 * - a sidebar with links to different admin components
 * - a div to render the current component
 * - a logout link
 * 
 * The component also sets the class list of html, body, and #root to 'h-100 overflow-hidden' when the component mounts and removes it when the component unmounts.
 * @param {{ currentComponent: React.ReactNode }} props - an object with one property: currentComponent.
 * @returns {React.ReactNode} - a React node that renders the admin panel.
 */
const AdminPanel = ({ currentComponent }: { currentComponent: React.ReactNode }) => {

  useEffect(() => {

    document.querySelectorAll('html,body, #root').forEach((e) => e.classList.add('h-100', 'overflow-hidden'));

    return () => {
      document.querySelectorAll('html,body, #root').forEach((e) => e.classList.remove('h-100', 'overflow-hidden'));
    }
  }, [])

  const navigate = useNavigate()
  const dispatch = useAppDispatch()

  const userRole = getItem('userRole');

  /**
   * Logout user
   * 
   * This function is called when the user clicks the logout link.
   * It shows a confirmation dialog with a warning icon and asks the user if they are sure to logout.
   * If the user confirms, it deletes the token from local storage, removes the user from local storage, logs out the user by calling logoutCurrentUser(), and navigates to the login page.
   * @param {React.SyntheticEvent} e - the event object
   */
  const logoutUser = (e: React.SyntheticEvent) => {
    e.preventDefault();
    Swal.fire({
      title: 'Are you sure?',
      text: "Are you sure to logout ?",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes, Logout it!'
    }).then((r) => {
      if (r.isConfirmed) {
        removeItem(RoutePaths.token);
        removeItem('user');
        dispatch(logoutCurrentUser)
        navigate(RoutePaths.login)
      }
    })
  }

  return (
    <>
      <div className="admin-container h-100">
        <AdminHeader />
        <div className='admin-section d-flex justify-content-between gap-4 px-5 my-4 h-75'>
          <aside className='user-page w-25 fw-bold border border-1'>
            <div><Link to={RoutePaths.admin} className={toggleLinkClass(RoutePaths.admin)}>Dashboard<i className="bi bi-speedometer float-end"></i></Link></div>
            <div><Link to={RoutePaths.adminProducts} className={toggleLinkClass(RoutePaths.adminProducts)}>Products<i className="bi bi-shop float-end"></i></Link></div>
            <div><Link to={RoutePaths.adminCategories} className={toggleLinkClass(RoutePaths.adminCategories)}>Orders<i className="bi bi-cart-check float-end"></i></Link></div>
            <div><Link to={RoutePaths.adminReview} className={toggleLinkClass(RoutePaths.adminReview)}>Reviews<i className="bi bi-chat-left-text float-end"></i></Link></div>
            <div hidden={userRole === "Vendor"}><Link to={RoutePaths.adminCustomers} className={toggleLinkClass(RoutePaths.adminCustomers)}>Users<i className="bi bi-people float-end"></i></Link></div>
            <div hidden={userRole === "Vendor" || userRole === "Csr"}><Link to={RoutePaths.adminVendors} className={toggleLinkClass(RoutePaths.adminVendors)}>Vendors<i className="bi bi-shop-window float-end"></i></Link></div>
            <div><a href='#' className="d-block p-3 text-black" onClick={logoutUser}>Logout<i className="bi bi-person-slash float-end"></i></a></div>
          </aside>
          <div className="w-75 overflow-auto scroller">
            {currentComponent}
          </div>
        </div>
        <AdminFooter />
      </div>
    </>
  )
}

export default AdminPanel