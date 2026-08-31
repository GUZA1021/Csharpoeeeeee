import { useState } from 'react'
import './App.css'

function App() {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [token, setToken] = useState('')
  const [job, setApplication] = useState([])
  const [company, setCompany] = useState('')
  const [position, setPosition] = useState('')

  async function addJobApplications(authToken) {
    const response = await fetch('https://localhost:7091/jobapplications', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + authToken,
      },
      body: JSON.stringify({ company, position }),
    })

    setCompany('')
    setPosition('')
    handleGetApplications(authToken)
  }

  async function handleGetApplications(authToken) {
    const response = await fetch('https://localhost:7091/jobapplications', {
      headers: {
        Authorization: 'Bearer ' + authToken,
      },
    })

    const jobs = await response.json()
    setApplication(jobs)
  }

  async function handleLogin() {
    const response = await fetch('https://localhost:7091/auth/login', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        InputEmail: email,
        InputPassword: password,
      }),
    })

    const data = await response.json()
    setToken(data.token)
    handleGetApplications(data.token)
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
          Login
        </button>

        {job.map((application) => (
          <div className="job-card" key={application.id}>
            <strong>{application.company}</strong>
            <span>{application.position}</span>
            <span>{application.status}</span>
          </div>
        ))}

        <section id="company">
          <input
            type="text"
            value={company}
            onChange={(e) => setCompany(e.target.value)}
          />

          <input
            type="text"
            value={position}
            onChange={(e) => setPosition(e.target.value)}
          />

          <button
            type="button"
            className="AddApplication"
            onClick={() => addJobApplications(token)}
          >
            Add Application
          </button>
        </section>
      </section>
    </>
  )
}

export default App