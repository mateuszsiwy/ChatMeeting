import React from 'react';
import { Link } from 'react-router-dom';


function LoginForm() {
  return (
      <div className="container mt-5 align-items-center justify-content-center">
          <h1 className="text-center">Login</h1>
          <form>
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