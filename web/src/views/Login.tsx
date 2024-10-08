import {useEffect} from 'react'
import {Navigate, useNavigate} from "react-router-dom"
import { LoginForm } from '../components/Form';
import RoutePaths from '../config';
import { checkLogin } from '../Utils/Generals';
import AdminHeader from '../components/Admin/AdminHeader';
import AdminFooter from '../components/Admin/AdminFooter';

const Login = () => {

  return (
    <>
      <AdminHeader />
      <LoginForm />
      <AdminFooter />
    </>
  )
}

export default Login;
