import React from 'react'
import NotFound from '../components/404';
import AdminFooter from '../components/Admin/AdminFooter';
import AdminHeader from '../components/Admin/AdminHeader';

const PageNotFound = () => {

  return (
    <>
        <AdminHeader />
        <NotFound />
        <AdminFooter />
    </>
)

}

export default PageNotFound;