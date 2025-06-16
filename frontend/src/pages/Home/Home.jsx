import { motion } from 'framer-motion';
import Navbar from "../../components/Navbar.jsx";
import CreateFeedCard from "../../components/CreateFeedCard.jsx";
import FeedCards from "../../components/FeedCards.jsx";
import { useEffect, useState } from "react";
import { getFeeds } from "../../services/feedService.js"

function Home() {
    const [feeds, setFeeds] = useState([]);

    useEffect (() => {
        fetchFeeds()
    }, []);

    const fetchFeeds = async () => {
        let feeds = await getFeeds()
        setFeeds(feeds.reverse());
    };
    
    return (
        <motion.div
            initial={{opacity: 0}}
            animate={{opacity: 1, transition: {duration: 0.5, ease: 'easeInOut'}}}
            exit={{opacity: 0, transition: {duration: 0.5, ease: 'easeInOut'}}}>
            
            <Navbar/>
            
            <div className="d-flex justify-content-center">
                <div className="w-75">
                    
                    <CreateFeedCard onCreated={fetchFeeds}/>
                    <FeedCards feeds={feeds} updateFeeds={fetchFeeds}/>
                    
                </div>
            </div>
        </motion.div>

    );
}

export default Home;