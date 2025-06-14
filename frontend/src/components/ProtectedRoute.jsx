import React, { useEffect, useState } from 'react';
import {Navigate, useNavigate} from 'react-router-dom';

function ProtectedRoute({ children }) {
  const [authState, setAuthState] = useState(null);
  
  useEffect(() => {
    const verifyToken = async () => {
      const token = sessionStorage.getItem("token");

      if (!token) {
        setAuthState(false);
        return;
      }

      try {
        const response = await fetch("https://localhost:44324/auth/check", {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`,
          },
        });

        if (response.ok) {
          let data = await response.json();
          setAuthState(true);
          sessionStorage.setItem("userGuid", data.userGuid);
          sessionStorage.setItem("email", data.email);
          sessionStorage.setItem("roles", data.roles);
        } else {
          setAuthState(false);
        }
      } catch (error) {
        console.error("Fetch error:", error);
        setAuthState(false);
      }
    };
    
    verifyToken()

  }, []);
  
  if (authState === null) {
    return;
  }

  if (!authState) {
    return <Navigate to="/login" replace />;
  }

  return children;
}

export default ProtectedRoute;