import React, { useState } from 'react';

const handleSubmit = async (event, setMessage) => {
    event.preventDefault();

    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    try {
        const response = await fetch('https://localhost:7099/api/Auth/register', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ username, password }),
        });

        const data = await response.json();

        if (response.ok) {
            setMessage({ type: 'success', text: data.message }); 
        } else {
            setMessage({ type: 'error', text: data.message }); 
        }
    } catch (error) {
        setMessage({ type: 'error', text: 'An error occurred while registering. Please try again later.' });
        console.error(error);
    }
};

function RegisterForm() {
    const [message, setMessage] = useState(null); 

    return (
        <div className="container mt-5">
            <h1 className="text-center">Register</h1>
            <form onSubmit={(event) => handleSubmit(event, setMessage)}>
                <div className="mb-3">
                    <label className="form-label">Username</label>
                    <input type="text" className="form-control" id="username"></input>
                </div>
                <div className="mb-3">
                    <label className="form-label">Password</label>
                    <input type="password" className="form-control" id="password"></input>
                </div>
                <button type="submit" className="btn btn-primary">Register</button>
                <p><a href="/">Back to Login</a></p>
            </form>
            {message && (
                <div
                    className={`alert mt-3 ${message.type === 'success' ? 'alert-success' : 'alert-danger'
                        }`}
                    role="alert"
                >
                    {message.text}
                </div>
            )}
        </div>
    );
}

export default RegisterForm;
