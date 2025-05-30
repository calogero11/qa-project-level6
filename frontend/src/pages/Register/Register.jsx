import React, { useState } from "react";
import {Link} from "react-router-dom";
import { motion } from 'framer-motion';

function Register() {
    const [emailError, setEmailError] = useState(null);
    const [passwordError, setPasswordError] = useState(null);
    
    function validatePassword(event) {
        const value = event.target.value;
        const atLeast6Charaters = /^.{6,}$/;
        const atLeast1NonAlphanumericCharater = /[^a-zA-Z0-9]/;
        const atLeast1Digit = /^(?=.*\d).+$/;
        const atLeast1LowercaseCharater = /^(?=.*[a-z]).+$/;
        const atLeast1UppercaseCharater = /^(?=.*[A-Z]).+$/;
        const atLeast1UniqueCharater = /^(?=.*[A-Z]).+$/;
        
        if (!atLeast6Charaters.test(value)) {
            setPasswordError('Password must be at least 6 characters.')
        }
        else if (!atLeast1NonAlphanumericCharater.test(value)) {
            setPasswordError('Passwords must have at least one non alphanumeric character.')
        }
        else if (!atLeast1Digit.test(value)) {
            setPasswordError('Password must have at least one digit (\'0\'-\'9\').')
        }
        else if (!atLeast1LowercaseCharater.test(value)) {
            setPasswordError('Password must have at least one lowercase (\'a\'-\'z\').')
        }
        else if (!atLeast1UppercaseCharater.test(value)) {
            setPasswordError('Password must have at least one uppercase (A-Z).')
        }
        else if (!atLeast1UniqueCharater.test(value)) {
            setPasswordError('Password must use at least 1 different character.')
        }
        else {
            setPasswordError('')
        }
    }
    
    function validateEmail(event) {
        const value = event.target.value;
        const validEmailPattern = /\S+@\S+\.\S+/;
        
        if (!validEmailPattern.test(value)) {
            setEmailError('Invalid Email Address');
        }
        else {
            setEmailError('');
        }
    }
    
    const handleRegistration = async (event) => {
        event.preventDefault()
        
        let email = event.target.email.value;
        let password = event.target.password.value;

        try {
            const response = await fetch('https://localhost:44324/register', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ email, password })
            });

            if (!response.ok) {
                throw new Error('Incorrect username or password.')
            }

            window.location.replace('http://localhost:5173/login');
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
                                            onChange={validateEmail}
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
                                            onChange={validatePassword}
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