import React, { useEffect } from "react";

function Toast({message, show, onClose, delay = 3000, isError}) {

    useEffect(() => {
        if (show) {
            console.log("toast visible")
            const timer = setTimeout(onClose, delay);
            return () => clearTimeout(timer);
        }
    }, [show, onClose, delay]);

    if (!show) {
        return (<div></div>);
    }
    
    return (
        <div
            className="toast show position-fixed top-0 end-0 m-3"
            role={isError ? "alert" : "status" }
            aria-live={isError ? "assertive" : "polite" }
            aria-atomic="true"
            style={{minWidth: "200px", zIndex: 1055}}
        >
            <div className="toast-header">

                {
                    !isError &&
                    <strong className="me-auto">
                        <span className="description text-success mt-2"> &#10004; </span>
                        Success
                    </strong>
                }

                {
                    isError &&
                    <strong className="me-auto">
                        <span className="description text-danger mt-2"> &#x2716; </span>
                        Failure
                    </strong>
                }


                <button
                    type="button"
                    className="btn-close"
                    onClick={onClose}
                    aria-label="Close"
                ></button>
            </div>
            <div className="toast-body">{message}</div>
        </div>
    );
}

export default Toast;