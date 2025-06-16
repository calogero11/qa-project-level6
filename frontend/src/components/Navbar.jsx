import { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom"; // Optional, if using React Router

export default function Navbar() {
    const navigate = useNavigate();
    const [isCollapsed, setIsCollapsed] = useState(true);
    const [email, setEmail] = useState("");

    useEffect(() => {
        setEmail(sessionStorage.getItem("email"));
    }, []);
    
    const toggleNavbar = () => setIsCollapsed(!isCollapsed);
    
    const handleLogout = () => {
        sessionStorage.clear()

        const goToLogin = () => {
            navigate('/login');
        };

        goToLogin();
    }
    
    return (
        <nav className="navbar navbar-expand-lg bg-black sticky-top">
            <div className="container-fluid">
                
                <a className="navbar-brand text-white" href="/">MyApp</a>

                <button
                    className="navbar-toggler"
                    type="button"
                    onClick={toggleNavbar}
                    aria-controls="navbarNav"
                    aria-expanded={!isCollapsed}
                    aria-label="Toggle navigation"
                >
                    <span className="navbar-toggler-icon"></span>
                </button>

                <div className={`collapse navbar-collapse ${!isCollapsed ? "show" : ""}`} id="navbarNav">
                    <ul className="navbar-nav ms-auto">
                        <li className="nav-item d-flex align-items-center">
                            <p className="m-0 text-white-50">{email}</p>
                        </li>
                        <li className="nav-item">
                            <a className="nav-link text-white cursor-pointer text-hover-grey" onClick={handleLogout}>Logout</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    );
}
