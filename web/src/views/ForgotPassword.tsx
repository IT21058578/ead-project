import React from 'react'
import { ResetPassword } from '../components/Form';
import AdminHeader from '../components/Admin/AdminHeader';
import AdminFooter from '../components/Admin/AdminFooter';

const ForgotPassword = () => {

  return (
    <>
        <AdminHeader />
        <ResetPassword />
        <AdminFooter />
    </>
  )
}

export default ForgotPassword;