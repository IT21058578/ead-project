import React from 'react'
import Header from './includes/Header';
import Footer from './includes/Footer';
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