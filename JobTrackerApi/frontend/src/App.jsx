import { useState } from 'react'
import './App.css'

function App() {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [token, setToken] = useState('')

  
  async function handleLogin() {
    const response = await fetch('https://localhost:7091/auth/login', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ InputEmail: email, InputPassword : password })
    })
    const data = await response.json()
    setToken(data)
  }

  return (
    <>
      <section id="center">
            <input
      type="text"
      value={email}
      onChange={(e) => setEmail(e.target.value)}
    />
        <input
      type="password"
      value={password}
      onChange={(e) => setPassword(e.target.value)}
    />
        <div>
          <h1>Login to register</h1>
        </div>
        <button 
          type="button"
          className="login"
          onClick={handleLogin}
        >
          login
        </button>
      </section>
    </>
  )
}


export default App
