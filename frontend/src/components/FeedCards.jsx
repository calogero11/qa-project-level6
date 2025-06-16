import { useEffect, useState } from "react";
import Toast from "./Toast.jsx";
import formatTimeAgo from "../utils/formatTimeAgo.js"
import { getAuthRoles } from "../utils/authRoles.js";
import { deleteFeed, updateFeed } from "../services/feedService.js";

function FeedCards({feeds, updateFeeds}) {
    const [userGuid] = useState(sessionStorage.getItem("userGuid"));
    const [userRoles, setUserRoles] = useState([]);
    const [editDetails, setEditDetails] = useState({id: null, title: null, content: null});
    const [toasterDetails, setToasterDetails] = useState({ isError: false, statusMessage: '', showToast: false});

    useEffect(() => {
        let roles = getAuthRoles();
        
        if(roles.includes(",")) {
            setUserRoles(roles.split(","))
        }

        return setUserRoles([roles]);
    }, []);
    
    const handleEdit = async () => {
        try {
            const id = editDetails.id;
            const title = editDetails.title;
            const content = editDetails.content;

            await updateFeed(id, title, content)
            setToasterDetails({isError: false, statusMessage: 'Your post has been updated successfully', showToast: true})
            updateFeeds()
            setEditDetails({id: null, title: null, content: null})
        } catch (error) {
            setToasterDetails({isError: true, statusMessage: error.message, showToast: true})
        }
    }
    
    const handleDelete = async (id) => {
        try {
            await deleteFeed(id)
            setToasterDetails({isError: false, statusMessage: 'Your post has been deleted successfully', showToast: true})
            updateFeeds()
        } catch (error) {
            setToasterDetails({isError: true, statusMessage: error.message, showToast: true})
        }
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
                                        <small className="text-muted me-2">Updated: {formatTimeAgo(feed.lastUpdatedDate)}</small> :
                                        <small className="text-muted me-2">{formatTimeAgo(feed.uploadedDate)}</small>
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