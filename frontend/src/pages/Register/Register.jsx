import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { motion } from 'framer-motion';
import { register } from "../../services/authenticationService.js";
import validatePassword from "../../utils/validatePassword.js";
import validateEmail from "../../utils/validateEmail.js";

function Register() {
    const navigate = useNavigate();
    const [emailError, setEmailError] = useState(null);
    const [passwordError, setPasswordError] = useState(null);
    
    function handlePasswordValidation(event) {
        const value = event.target.value;
        const error = validatePassword(value)
        setPasswordError(error)
    }
    
    function handleEmailValidation(event) {
        const value = event.target.value;
        const error = validateEmail(value)
        setEmailError(error)
    }
    
    const handleRegistration = async (event) => {
        event.preventDefault()
        
        let email = event.target.email.value;
        let password = event.target.password.value;

        try {
            await register(email, password)
            navigate('/login')
        }
        catch (error) {
            console.log('error')
        }
    }
    
    return (
        <motion.div
            initial={{ x: '-50%', opacity: 0 }}
            animate={{ x: 0, opacity: 1, transition: { duration: 0.5, ease: 'easeInOut' } }}
            exit={{ opacity: 0, transition: { duration: 0.5, ease: 'easeInOut' } }}
        >
            <div className="container-fluid vh-100">
                <div className="h-100 row">
                    <div className="bg-black col-lg-6 d-none d-lg-flex align-items-center justify-content-center">
                        <h2 className="text-white">Create your account now</h2>
                    </div>

                    <div className="col-12 col-lg-6 position-relative">

                        <div className="d-flex position-absolute top-0 end-0 p-3 me-4">
                            <p>Already have an account?</p>
                            <Link to="/login" className="ms-2">Log in</Link>
                        </div>

                        <div className="d-flex justify-content-center align-items-center h-100">

                            <div className="h-75 w-50 position-relative">
                                <h3 className="mb-5">Sign up</h3>

                                <form onSubmit={handleRegistration}>
                                    <div className="mb-3">
                                        <label htmlFor="email" className="form-label">
                                            Email address
                                            {emailError === '' &&
                                                <span className="description text-success mt-2"> &#10004;</span>
                                            }
                                        </label>
                                        <input
                                            id="email"
                                            type="email"
                                            required
                                            className={`
                                            form-control
                                            ${
                                                emailError === ''
                                                    ? 'border-success'
                                                    : emailError !== null
                                                        ? 'border-danger' : ''
                                            }
                                        `}
                                            onChange={handleEmailValidation}
                                            placeholder="Enter email"/>
                                        {emailError &&
                                            <p className="description text-danger mt-2">&#x2716; {emailError}</p>}
                                    </div>

                                    <div className="mb-3">
                                        <label htmlFor="password" className="form-label">
                                            Password
                                            {passwordError === '' &&
                                                <span className="description text-success mt-2"> &#10004;</span>
                                            }
                                        </label>
                                        <input
                                            type="password"
                                            required
                                            className={`
                                            form-control
                                            ${
                                                passwordError === ''
                                                    ? 'border-success'
                                                    : passwordError !== null
                                                        ? 'border-danger' : ''
                                            }
                                        `}
                                            id="password"
                                            onChange={handlePasswordValidation}
                                            placeholder="Password"/>
                                        {passwordError &&
                                            <p className="description text-danger mt-2">&#x2716; {passwordError}</p>}
                                        <p className="description mt-3">Password should be at least 6 characters long
                                            and
                                            include at least a digit and an non alphanumeric character.</p>
                                    </div>

                                    <button type="submit" className={`${emailError || passwordError ? 'disabled' : ''}
                                btn btn-primary w-100 btn-dark position-absolute bottom-0 mb-5`}>Create account
                                    </button>

                                </form>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </motion.div>
    );
}

export default Register;