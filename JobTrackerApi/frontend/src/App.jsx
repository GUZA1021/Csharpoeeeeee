import { use, useState } from 'react'
import './App.css'

function App() {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [token, setToken] = useState('')
  const [job, setApplication] = useState([])
  const [company, setCompany] = useState('')
  const [position, setPosition] = useState('')
  const [editingID, setEditingId] = useState(null)
  const [isAdding, setAddingStatus] = useState(false)
  const [status, setStatus] = useState('Applied')
  

  function handleLogout() {
    setCompany('')
    setPosition('')
    setStatus('Applied')
    setEditingId(null)
    setToken('')
    setAddingStatus(false)
    setApplication([])
  }

  async function addJobApplications(authToken) {
    if (editingID){
    const response = await fetch(`https://localhost:7091/jobapplications/${editingID}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + authToken,
      },
      body: JSON.stringify({ company, position, status })
    })
    }
    else {
    const response = await fetch('https://localhost:7091/jobapplications', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + authToken,
      },
      body: JSON.stringify({ company, position, status })
    })
    }

    setCompany('')
    setPosition('')
    setStatus('Applied')
    setEditingId(null)
    setAddingStatus(false)
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


  async function handleDeleteApplications(authToken, id) {
    const url = `https://localhost:7091/jobapplications/${id}`
    const response = await fetch(url,
    {
      method: 'DELETE',
      headers: {
        Authorization: 'Bearer ' + authToken,
      },
    })

    handleGetApplications(authToken)


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
      {!token ? (
        <div>
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
              <h1>Welcome</h1>
            </div>

            <button
              type="button"
              className="login"
              onClick={handleLogin}
            >
              Login
            </button>
          </section>
        </div>
      ) : ( //logged in mode
        <div>
          <button
            type="button"
            className="AddApplication"
            onClick={handleLogout}
          >
            Logout
          </button>

              <button
                type="button"
                className="AddApplication"
                onClick={() => {
                  setAddingStatus(true)
                  setEditingId(null)
                }}
              >
                Add new application
              </button>

          {job.map((application) => (
              application.id === editingID ? (
                <div className="job-card" key={application.id}>
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

                    <select
                      value={status}
                      onChange={(e) => setStatus(e.target.value)}
                    >
                      <option value="Applied">Applied</option>
                      <option value="Rejected">Rejected</option>
                      <option value="Interview">Interview</option>
                      <option value="Accepted">Accepted</option>
                    </select>

                    <button
                      type="button"
                      className="AddApplication"
                      onClick={() => addJobApplications(token)}
                    >
                      Save
                    </button>
                    <button
                      type="button"
                      className="AddApplication"
                      onClick={() => setEditingId(null)}
                    >
                      Cancel
                    </button>
                  </section>
                </div>
              ) : (
                <div className="job-card" key={application.id}>
                    <strong>{application.company}</strong>

                    <span>{application.position}</span>

                    <span>{application.status}</span>

                    <button
                      type="button"
                      className="AddApplication"
                      onClick={() =>
                        handleDeleteApplications(token, application.id)
                      }
                    >
                      Delete
                    </button>
                    
                    <button
                      type="button"
                      className="AddApplication"
                      onClick={() => {
                        setCompany(application.company)
                        setPosition(application.position)
                        setStatus(application.status)
                        setEditingId(application.id)
                        setAddingStatus(false)
                      }}
                    >
                      Edit
                    </button>
                </div>
              )
          ))}
          {(isAdding && (
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

            <select
              value={status}
              onChange={(e) => setStatus(e.target.value)}
            >
              <option value="Applied">Applied</option>
              <option value="Rejected">Rejected</option>
              <option value="Interview">Interview</option>
              <option value="Accepted">Accepted</option>
            </select>

            <button
              type="button"
              className="AddApplication"
              onClick={() => addJobApplications(token)}
            >
              {editingID ? 'Update Application' : 'Add Application'}
            </button>
          </section>
          ))}
        </div>
      )}
    </>
  )
}



export default App