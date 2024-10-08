import React from 'react'

/**
 * AdminFooter component
 * 
 * This component renders a footer bar at the bottom of the page
 * that displays the name of the application and the copyright
 * information.
 * 
 * The footer bar is fixed at the bottom of the page using CSS
 * position: fixed.
 * 
 * @returns {JSX.Element} - The rendered footer bar.
 */
const AdminFooter = () => {

    return (
        <div className="fixed-bottom d-flex bg-black justify-content-between px-5 pt-2">
            <p><span>Admin Panel</span></p>
            <p className="opacity-75">Copyright &copy; 2024 <span className='fw-bold'>Style Square - EAD</span></p>
        </div>
    )
}

export default AdminFooter