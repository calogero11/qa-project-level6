import { useEffect, useState } from "react";
import Toast from "./Toast.jsx";
import {title} from "framer-motion/m";

function FeedCards({feeds, updateFeeds}) {
    const [userGuid] = useState(sessionStorage.getItem("userGuid"));
    const [userRoles, setUserRoles] = useState([]);
    const [editDetails, setEditDetails] = useState({id: null, title: null, content: null});
    const [toasterDetails, setToasterDetails] = useState({ isError: false, statusMessage: '', showToast: false});

    useEffect(() => {
        let roles = sessionStorage.getItem("roles")
        if (roles === null || roles === undefined) {
            return
        }

        if(roles.includes(",")) {
            setUserRoles(roles.split(","))
        }

        return setUserRoles(roles);
    }, []);
    
    const handleEdit = async () => {
        try {
            let token = sessionStorage.getItem("token");
            let id = editDetails.id;
            let title = editDetails.title;
            let content = editDetails.content;
            
            const response = await fetch(`${__API_URL__}/feed/${id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    "Authorization": `Bearer ${token}`,
                },
                body: JSON.stringify({ title, content })
            });

            if (!response.ok) throw new Error('Failed to update post');

            setToasterDetails({isError: false, statusMessage: 'Your post has been updated successfully', showToast: true})
            updateFeeds()
            setEditDetails({id: null, title: null, content: null})
        } catch (error) {
            setToasterDetails({isError: true, statusMessage: error.message, showToast: true})
        }
    }
    
    const handleDelete = async (id) => {
        try {
            let token = sessionStorage.getItem("token");
            
            const response = await fetch(`${__API_URL__}/feed/${id}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    "Authorization": `Bearer ${token}`,
                }
            });

            if (!response.ok) throw new Error('Failed to delete');

            setToasterDetails({isError: false, statusMessage: 'Your post has been deleted successfully', showToast: true})
            updateFeeds()
        } catch (error) {
            setToasterDetails({isError: true, statusMessage: error.message, showToast: true})
        }
    }
    
    const getTimePassed = (date) => {
        const now = new Date();
        const past = new Date(date);
        const seconds = Math.floor((now - past) / 1000);

        if (seconds < 60) return `less than a minute ago`;
        const minutes = Math.floor(seconds / 60);
        if (minutes < 60) return `${minutes} minutes ago`;
        const hours = Math.floor(minutes / 60);
        if (hours < 24) return `${hours} hours ago`;
        const days = Math.floor(hours / 24);
        if (days < 30) return `${days} days ago`;
        const months = Math.floor(days / 30);
        if (months < 12) return `${months} months ago`;
        const years = Math.floor(months / 12);
        return `${years} years ago`;
    }
    
    return (
        <div>
            <Toast
                message={toasterDetails.statusMessage}
                show={toasterDetails.showToast}
                onClose={() => setToasterDetails({isError: false, showToast: false, statusMessage: ''} )}
                isError={toasterDetails.isError}
            />
            
            {feeds.map((feed) => (
                <div className="card mx-auto mt-3 w-75">

                    <div className="card-header d-flex justify-content-between align-items-center">

                        { editDetails.id === feed.id ?
                            <input 
                                className="form-control border-0"
                                type="text"
                                value={editDetails.title}
                                onChange={
                                    (e) => 
                                        setEditDetails((prev) =>
                                            ({...prev, title: e.target.value }) 
                                        )
                                }>
                            </input> 
                            
                            : <p className="mb-0">{feed.title}</p> }


                        <div className="text-center d-flex align-items-center">
                            <div className="d-block">
                                <small className="text-muted d-block">{feed.userName}</small>
                                {
                                    feed.lastUpdatedDate != null ?
                                        <small className="text-muted me-2">Updated: {getTimePassed(feed.lastUpdatedDate)}</small> :
                                        <small className="text-muted me-2">{getTimePassed(feed.uploadedDate)}</small>
                                }
                            </div>

                            { (userGuid === feed.userGuid || userRoles.includes("Admin")) &&
                                <div className="dropdown ms-3">
                                    <button className="btn text-dark fs-4" type="button" id="dropdownMenuButton"
                                            data-bs-toggle="dropdown" aria-expanded="false">
                                        &#8942;
                                    </button>
                                    <ul className="dropdown-menu dropdown-menu-end"
                                        aria-labelledby="dropdownMenuButton">
                                        <li><a className="dropdown-item" onClick={() => setEditDetails({
                                            id: feed.id,
                                            title: feed.title,
                                            content: feed.content
                                        })}>Edit</a></li>
                                        <li><a className="dropdown-item text-danger" data-bs-toggle="modal"
                                               data-bs-target={`#deleteModal-${feed.id}`}>Delete</a></li>
                                    </ul>
                                </div>
                            }


                        </div>
                    </div>

                    <div className="card-body">
                        {editDetails.id === feed.id ?
                            <textarea
                                className="form-control border-0"
                                value={editDetails.content}
                                onChange={
                                    (e) =>
                                        setEditDetails((prev) =>
                                            ({...prev, content: e.target.value})
                                        )
                                }>
                            </textarea>
                            : <p className="mb-0">{feed.content}</p>}
                    </div>

                    {editDetails.id === feed.id ?
                        <div>
                            <btn className="btn btn-primary m-3 d-block" onClick={handleEdit}>Update</btn>
                            <btn className="btn btn-light mx-3 mb-3 mt-1 d-block" onClick={() => setEditDetails({id: null, title: null, content: null})}>Cancel</btn>
                        </div>

                        : ""}


                    {/*Delete Confirmation Modal*/}
                    <div className="modal fade" id={`deleteModal-${feed.id}`} tabIndex="-1" aria-labelledby={`deleteModalLabel-${feed.id}`} aria-hidden="true">
                        <div className="modal-dialog">
                            <div className="modal-content">
                                <div className="modal-header">
                                    <h5 className="modal-title" id="deleteModalLabel">Confirm Deletion</h5>
                                    <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                </div>
                                <div className="modal-body">
                                Are you sure you want to delete this item? This action cannot be undone.
                                </div>
                                <div className="modal-footer">
                                    <button type="button" className="btn btn-outline-dark" data-bs-dismiss="modal">Cancel</button>
                                    <button type="button" className="btn btn-danger" data-bs-dismiss="modal" onClick={() => handleDelete(feed.id)}>Delete</button>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            ))
            }
        </div>
    );

}

export default FeedCards;