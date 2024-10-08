import React, { SyntheticEvent, useEffect, useState } from "react";
import { Link, Navigate } from "react-router-dom";
import { useRegisterMutation, useLoginMutation } from "../store/apiquery/AuthApiSlice";
import { HandleResult } from "./HandleResult";
import LoadingButton from "./LoadingButton";
import RoutePaths from "../config";
import { checkLogin } from "../Utils/Generals";
import { ToastContainer, toast } from "react-toastify";
import { useNavigate } from 'react-router-dom';

/**
 * LoginForm component renders a form for user to sign in.
 * It first checks if the user is already logged in, if so, it redirects to the user account page.
 * The form includes fields for email, password, and remember me.
 * When the form is submitted, it calls the sendUserInfo mutation and handles the result.
 * If the mutation is successful, it redirects to the user account page.
 * If the mutation fails, it shows an error message.
 * @returns {JSX.Element} - The login form component.
 */
const LoginForm = () => {

    if (checkLogin()) {
    
        return <Navigate to={RoutePaths.userAccount} replace />
    }


    const [data, setData] = useState({});
    const [sendUserInfo, result] = useLoginMutation();

    /**
     * Handles the change event for the form fields.
     * Updates the formData state with the changed value.
     * @param e - The change event.
     */
    const handleChange = ( e : SyntheticEvent) => {

        const target = e.target as HTMLInputElement

        setData({...data, [target.name]: target.value });
    }

    /**
     * Handles the form submission event.
     * Calls the sendUserInfo mutation with the formData.
     * @param e - The form submission event.
     */
    const handleSubmit = (e : SyntheticEvent) => {

        e.preventDefault();
        sendUserInfo(data);
    }


    return (
        <div className="login-form  bg-white shadow col-11 col-lg-4 mx-auto my-5 text-black p-3" style={{ minHeight: '400px' }}>
            <h3 className="fw-bold text-center">Sign In</h3>
            <form action="" onSubmit={handleSubmit}>
                {/* <div className="d-flex gap-2 sign-oauth my-4 text-white text-center">
                    <a href="#" className="d-block s-google w-50 bg-danger p-3 rounded-3"><i className="bi bi-google"></i><span> Google</span></a>
                    <a href="#" className="d-block s-facebook w-25 fd-bg-secondary rounded-3"><i className="bi bi-facebook" style={{ lineHeight: '55px' }}></i></a>
                    <a href="#" className="d-block s-twitter w-25 bg-info rounded-3"><i className="bi bi-twitter" style={{ lineHeight: '55px' }}></i></a>
                </div> */}
                <div className="my-4">
                    <div className="username w-100">
                        <label className="w-100">
                            <span>Email :</span> <input type="email" name="email" className="form-control rounded-0 p-2" onChange={handleChange} />
                        </label>
                    </div>
                    <div className="user-pass my-4">
                        <label className="w-100">
                            <span>Password :</span> <input type="password" name="password" className="form-control rounded-0 p-2" onChange={handleChange} />
                        </label>
                    </div>
                    <div className="remember-me">
                        <label className="w-100">
                            <input type="checkbox" name="remember" />
                            <span> Remember Me</span>
                        </label>
                    </div>
                    <HandleResult result={result} />
                    <div className="submit text-center my-4">
                        <LoadingButton loadingState={result.isLoading}>
                            <button type="submit" className="w-100 border-0 fd-btn">SIGN IN</button>
                        </LoadingButton>
                    </div>
                    {/* <div className="bt text-center">
                        <div><Link to="/reset-password" className="text-black opacity-75">Forget Password</Link></div>
                        <div className="signup mt-2"><span>Don't have account ?</span><Link to="/signup" className="fd-color-primary">Sign Up</Link></div>
                    </div> */}
                </div>
            </form>
        </div>
    )
}

/**
 * The SignUpForm component renders a form for users to register for an account.
 *
 * If the user is already logged in, it redirects to the user account page.
 *
 * The component uses the useRegisterMutation hook to call the register mutation
 * and handle the response.
 *
 * @returns A JSX element representing the register form
 */
const SignUpForm = () => {
    const navigate = useNavigate();

    if (checkLogin()) {
    
        return <Navigate to={RoutePaths.userAccount} replace />
    }

    // const [data, setData] = useState({});
    const [sendUserInfo] = useRegisterMutation();

    // const handleChange = ( e : SyntheticEvent) => {

    //     const target = e.target as HTMLInputElement

    //     setData({...data, [target.name]: target.value });
    // }

    // const handleSubmit = async (e : SyntheticEvent) => {

    //     e.preventDefault();
    //     try {
    //         const response = await sendUserInfo(data);
    
    //         if (!("error" in response)) {
    //             // Registration was successful, navigate to the login form
    //             return <Navigate to={RoutePaths.login} replace />
    //         }
    //     } catch (error) {
    //         // Handle errors here, if necessary
    //         console.error("An error occurred during registration:", error);
    //     }
    // }

    const [userDto, setFormData] = useState({
        firstName: '',
        lastName: '',
        email: '',
        password: '',
      });
    
    /**
     * Handles the change event for the form fields.
     * Updates the formData state with the changed value.
     * @param e - The change event.
     */
      const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
        const { name, value } = e.target;
        setFormData({ ...userDto, [name]: value });
      };
    
    /**
     * Handles the submission of the form. Calls the sendUserInfo function and
     * displays a toast notification based on the result.
     * If the registration is successful, it navigates to the login form.
     * @param e - The form submission event.
     * @returns {Promise<void>}
     */
      const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
    
        try {
          const result = await sendUserInfo(userDto);
          console.log('Registration successfull');
          toast.success("Registration successfully");
          setTimeout(() => {
            navigate('/login', { replace: true });
          }, 2000);
    
          if ('data' in result && result.data) {
            console.log('Registration successfull');
            toast.success("Registration successfully");
            setFormData({         
                firstName: '',
                lastName: '',
                email: '',
                password: ''});
            return <Navigate to={RoutePaths.login} replace />
          } else if ('error' in result && result.error) {
            console.error('Registration failed', result.error);
            toast.error("Registration failed");
          }
        } catch (error) {
          console.error('Registration failed`', error);
          toast.error("Registration failed");
        }
      };

    return (
        <div className="login-form  bg-white shadow col-11 col-lg-4 mx-auto my-5 text-black p-3" style={{ minHeight: '500px' }}>
            <h3 className="fw-bold text-center">Register Account</h3>
            <form action="" onSubmit={handleSubmit}>
                {/* <div className="d-flex gap-2 sign-oauth my-4 text-white text-center">
                    <a href="#" className="d-block s-google w-50 bg-danger p-3 rounded-3"><i className="bi bi-google"></i><span> Google</span></a>
                    <a href="#" className="d-block s-facebook w-25 fd-bg-secondary rounded-3"><i className="bi bi-facebook" style={{ lineHeight: '55px' }}></i></a>
                    <a href="#" className="d-block s-twitter w-25 bg-info rounded-3"><i className="bi bi-twitter" style={{ lineHeight: '55px' }}></i></a>
                </div> */}
                <div className="my-4">
                    <div className="d-flex w-100 gap-2">
                        <label className="w-50">
                            <span>Firstname :</span> <input type="text" value={userDto.firstName} name="firstName" placeholder="Enter Firt Name"className="form-control rounded-0 p-2" onChange={handleChange}/>
                        </label>
                        <label className="w-50">
                            <span>Lastname :</span> <input type="text" value={userDto.lastName} name="lastName" placeholder="Enter Last Name" className="form-control rounded-0 p-2" onChange={handleChange}/>
                        </label>
                    </div>
                    <div className="username w-100">
                        <label className="w-100 mt-4">
                            <span>Email :</span> <input type="email" value={userDto.email} name="email" placeholder="Enter Email" className="form-control rounded-0 p-2" onChange={handleChange}/>
                        </label>
                    </div>
                    {/* <div className="d-flex w-100 gap-2 mt-4">
                        <label className="w-50">
                            <span>Region :</span> <input type="text" value={userDto.region} name="region" placeholder="Enter Region" className="form-control rounded-0 p-2" onChange={handleChange}/>
                        </label>
                        <label className="w-50">
                            <span>Country :</span> <input type="text" value={userDto.country} name="country" placeholder="Enter Country" className="form-control rounded-0 p-2" onChange={handleChange}/>
                        </label>
                    </div> */}
                    <div className="user-pass my-4">
                        <label className="w-100">
                            <span>Password :</span> <input type="password" value={userDto.password} name="password" placeholder="Create New Password" className="form-control rounded-0 p-2" onChange={handleChange}/>
                        </label>
                    </div>
                    <div className="user-pass my-4">
                        <label className="w-100">
                            <span>Confirm Password :</span> <input type="password" name="password_confirmation" className="form-control rounded-0 p-2" />
                        </label>
                    </div>
                    <div className="mt-4">
                        <ToastContainer/>
                    </div>
                    <div className="submit text-center my-4">
                        {/* <LoadingButton loadingState={result.isLoading}> */}
                            <button type="submit" className="w-100 border-0 fd-btn">REGISTER</button>
                        {/* </LoadingButton> */}
                    </div>
                    <div className="bt text-center">
                        <div className="signup mt-2"><span>Already have an account ?</span><Link to="/login" className="fd-color-primary">Sign In</Link></div>
                    </div>
                </div>
            </form> 
        </div>
    )
}

/**
 * The ResetPassword component renders a form for users to reset their passwords.
 *
 * If the user is already logged in, it redirects to the user account page.
 *
 * The component renders a form with one text input for the email address and a
 * button labeled "SEND".
 *
 * @returns A JSX element representing the reset password form
 */
const ResetPassword = () => {
    if (checkLogin()) {
    
        return <Navigate to={RoutePaths.userAccount} replace />
    }

    return (
        <div className="login-form  bg-white shadow col-11 col-lg-4 mx-auto my-5 text-black p-3" style={{ minHeight: '300px' }}>
            <h3 className="fw-bold text-center">Forgot Password</h3>
            <form action="">
                <div className="my-4">
                    <div className="username w-100">
                        <label className="w-100">
                            <span>Email :</span> <input type="email" name="email" className="form-control rounded-0 p-2" />
                        </label>
                    </div>
                    <div className="submit text-center my-4"><a href="#" className="fd-btn">SEND</a></div>
                    <div className="bt text-center">
                        <div className="signup mt-2"><span>Forget it?, send me back to the sign in.</span></div>
                    </div>
                </div>
            </form>
        </div>
    )
}


export { LoginForm, SignUpForm, ResetPassword}