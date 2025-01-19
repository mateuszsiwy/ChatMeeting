import { useState } from 'react'
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import './App.css'
import LoginForm from './LoginForm';
import RegisterForm from './RegisterForm';
function App() {

  return (
     <>
        <Router>
              <Routes>
                  <Route path="/" element={<LoginForm />} />
                  <Route path="/register" element={<RegisterForm />} />
              </Routes>
        </Router>
    </>
  )
}

export default App
