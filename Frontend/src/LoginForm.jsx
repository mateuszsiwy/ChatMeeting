import React from 'react';
import { Link } from 'react-router-dom';


const handleSubmit = async (e) => {
    e.preventDefault();
    var password = document.getElementById('password').value;
    var username = document.getElementById('username').value;
    try {
        const response = await fetch('https://localhost:7099/api/Auth/login', {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({ username, password }),
        });

        if (response.ok) {
            alert("Zalogowano");
            const object = await response.json();
            const token = object.token;
            localStorage.setItem("token", token);
        }
        else {
            alert("Niepowodzenie")
        }

    } catch (error) {
        console.log("Error occured: ", error);
    }
}

function LoginForm() {
  return (
      <div className="container mt-5 align-items-center justify-content-center">
          <h1 className="text-center">Login</h1>
          <form onSubmit={handleSubmit}>
              <div className="mb-3">
                  <label className="form-label">Username</label>
                  <input type="text" className="form-control" id="username"></input>
              </div>
              <div className="mb-3">
                  <label className="form-label">Password</label>
                  <input type="text" className="form-control" id="password"></input>
              </div>
              <button type="submit" className="btn btn-primary">Login</button>
              <p><Link to="/register">Click here to Register</Link></p>
          </form>
      </div>
  );
}

export default LoginForm;