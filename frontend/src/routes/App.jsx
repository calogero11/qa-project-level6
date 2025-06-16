import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { AnimatePresence} from "framer-motion";
import Login from '.././pages/Login/Login.jsx';
import Home from '.././pages/Home/Home.jsx';
import Register from '.././pages/Register/Register.jsx';
import ProtectedRoute from ".././components/ProtectedRoute.jsx";

function App() {
    return (
        <Router>
            <AnimatePresence mode="wait">
                <Routes>
                    <Route path="/login" element={<Login/>} />
                    <Route path="/register" element={<Register/>}/>
                    
                    {/*Protected Routes*/}
                    <Route path="/" element={<ProtectedRoute><Home/></ProtectedRoute>}/>
                </Routes>
            </AnimatePresence>
        </Router>
    );
}

export default App
