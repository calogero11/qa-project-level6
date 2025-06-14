import { useState } from "react";
import Toast from "./Toast.jsx"

function CreateFeedCard({onCreated}) {
    const [rows, setRows] = useState(1);
    const [toasterDetails, setToasterDetails] = useState({ isError: false, statusMessage: '', showToast: false});
    
    const handleCreation = async (event) => {
        event.preventDefault();

        let title = event.target.feedTitle.value;
        let content = event.target.feedContent.value;
        const token = sessionStorage.getItem("token");
        
        try {
            const response = await fetch("https://localhost:44324/feed", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${token}`
                },
                body: JSON.stringify({ title, content })
            });

            if (!response.ok) {
                throw new Error('Failed to create post.');
            }

            setToasterDetails({isError: false, statusMessage: 'Your post has been uploaded successfully', showToast: true})
            event.target.feedContent.value = ''
            setRows(1)
            onCreated()
        }
        catch (error) {
            setToasterDetails({isError: true, statusMessage: error.message, showToast: true})
        }
    }
    
    const handleChange = (event) => {
        const value = event.target.value;
        
        if (value.length > 0) {
            setRows(4)
        }
        else {
            setRows(1)
        }
    }
    
    return (
        <div className="card mx-auto mt-3 w-75">

            <Toast
                message={toasterDetails.statusMessage}
                show={toasterDetails.showToast}
                onClose={() => setToasterDetails({isError: false, showToast: false, statusMessage: ''})}
                isError={toasterDetails.isError}
            />

            <form onSubmit={handleCreation}>
                <div className="card-body">

                    <div className="border-1 rounded border mb-3">
                        {
                            rows == 4 &&
                            (
                                <input
                                    className="form-control mb-2 border-0"
                                    id="feedTitle"
                                    placeholder="Want to give your post a title?"/>
                            )
                        }

                        <textarea
                            className="form-control border-0"
                            id="feedContent"
                            rows={rows}
                            placeholder="Share thoughts, ideas, or updates"
                            onChange={handleChange}/>
                        
                    </div>


                    <button type="submit" className="btn btn-dark w-100">
                        Post
                    </button>
                </div>
            </form>

        </div>
    );
}

export default CreateFeedCard;