import React, { useState } from 'react';
import { useNavigate, Link } from "react-router-dom";
import { motion } from 'framer-motion';
import { login } from "../../services/authenticationService.js";
import { setAuthToken } from "../../utils/authToken.js";

function Login() {
    const navigate = useNavigate();
    const [error, setError] = useState('');

    const handleLogin = async (event) => {
        event.preventDefault();
        
        const email = event.target.email.value;
        const password = event.target.password.value;
        
        try {
            const data = await login(email, password);
            setAuthToken(data.accessToken)
            navigate('/')
        }
        catch (error) {
            setError('Invalid username or password.');
        }
    }
    
    return (
        <motion.div
            initial={{ x: '50%', opacity: 0 }}
            animate={{ x: 0, opacity: 1, transition: { duration: 0.5, ease: 'easeInOut' } }}
             exit={{ opacity: 0, transition: { duration: 0.5, ease: 'easeInOut' } }}
        >
            <div className="container-fluid 100vw vh-100">
                <div className="h-100 row">
                    <div className="col-12 col-lg-6 position-relative">
                
                        <div className="d-flex position-absolute top-0 end-0 p-3 me-4">
                            <p>Don't have an account?</p>
                            <Link to="/register" className="ms-2">Create account</Link>
                        </div>
                
                        <div className="d-flex justify-content-center align-items-center h-100">
                
                            <div className="h-75 w-50 position-relative">
                                <h3 className="mb-5">Login</h3>
                                
                                <form onSubmit={handleLogin}>
                                    <div className="mb-3">
                                        <label htmlFor="email" className="form-label">Email address</label>
                                        <input
                                            id="email"
                                            type="email"
                                            required
                                            className='form-control'
                                            placeholder="Enter email"/>
                                    </div>
                
                                    <div className="mb-3">
                                        <label htmlFor="password" className="form-label">Password</label>
                                        <input
                                            type="password"
                                            required
                                            minLength={6}
                                            className='form-control'
                                            id="password"
                                            placeholder="Password"/>
                                    </div>
                
                                    <button type="submit" className='btn btn-primary w-100 btn-dark position-absolute bottom-0 mb-5'>
                                        Login
                                    </button>
                                </form>
                
                                {error && (
                                    <div className="alert alert-danger alert-dismissible fade show text-center" role="alert">
                                        {error}
                                        <button
                                            type="button"
                                            className="btn-close"
                                            aria-label="Close"
                                            onClick={() => setError('')}
                                        ></button>
                                    </div>
                                )}
                                
                            </div>
                
                        </div>
                    </div>
                
                    <div className="bg-black col-lg-6 d-none d-lg-flex align-items-center justify-content-center">
                        <h2 className="text-white">Welcome Back!</h2>
                    </div>
                </div>
            </div>
        </motion.div>
    );
}

export default Login;