import { motion } from 'framer-motion';
import { useLocation } from "react-router-dom";
import Navbar from "../../components/Navbar.jsx";
import CreateFeedCard from "../../components/CreateFeedCard.jsx";
import FeedCards from "../../components/FeedCards.jsx";
import {useEffect, useState} from "react";

function Home() {
    const [feeds, setFeeds] = useState([]);
    
    const location = useLocation();
    const username = location.state?.username;
    
    const fetchFeeds = async () => {
        const token = sessionStorage.getItem("token");

        const response = await fetch("https://localhost:44324/feed/", {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`,
            },
        });
        
        const data = await response.json();
        setFeeds(data.reverse());
    };

    useEffect(() => {
        fetchFeeds()
    }, []);
    
    const updateFeeds = async () => {
        fetchFeeds()
    }
    
    
    return (
        <motion.div
            initial={{opacity: 0}}
            animate={{opacity: 1, transition: {duration: 0.5, ease: 'easeInOut'}}}
            exit={{opacity: 0, transition: {duration: 0.5, ease: 'easeInOut'}}}
        >
            <Navbar/>
            
            <div className="d-flex justify-content-center">
                <div className="w-75">

                    <CreateFeedCard onCreated={updateFeeds}/>
                    
                    <FeedCards feeds={feeds} updateFeeds={updateFeeds}/>
                        
                </div>
            </div>
        </motion.div>

    );
}

export default Home;