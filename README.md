# Job Application Tracker

A full-stack web app for tracking job applications — built from scratch as a learning project. The goal was to build every part of the stack myself, end to end.

## Features

- User registration and login with JWT authentication
- Create, read, update, and delete job applications
- Track application status (`Applied`, `Interview`, `Rejected`, `Accepted`)
- Inline editing directly on each application card (no page navigation needed)
- Applications are scoped per user — the backend derives the owner from the JWT, never from client input
- Responsive layout
- Rate limiting on `/auth/login` and `POST /jobapplications` to prevent brute-force and spam

## Tech stack

**Backend**
- ASP.NET Core Web API (C#)
- Entity Framework Core
- PostgreSQL

**Frontend**
- React (Vite)
- Custom CSS

## Project structure

```
/JobTrackerApi   → ASP.NET Core Web API backend
/frontend        → React (Vite) frontend
```

## Getting started

### Prerequisites

- .NET SDK
- Node.js + npm
- PostgreSQL

### Backend

```bash
cd JobTrackerApi
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<your-postgres-connection-string>"
dotnet user-secrets set "Jwt:Key" "<your-jwt-signing-key>"
dotnet ef database update
dotnet run
```

The API runs by default at `https://localhost:7091`.

### Frontend

```bash
cd frontend
npm install
npm run dev
```

The frontend runs by default at `http://localhost:5173`.

## API overview

| Method | Endpoint                    | Auth required | Description                        |
|--------|------------------------------|:--------------:|------------------------------------|
| POST   | `/auth/register`             | No             | Create a new user                  |
| POST   | `/auth/login`                | No             | Log in, returns a JWT              |
| GET    | `/jobapplications`           | Yes            | Get all applications for the current user |
| GET    | `/jobapplications/{id}`      | Yes            | Get a single application           |
| POST   | `/jobapplications`           | Yes            | Create a new application           |
| PUT    | `/jobapplications/{id}`      | Yes            | Update an application              |
| DELETE | `/jobapplications/{id}`      | Yes            | Delete an application              |

## Design notes

- **Ownership is server-side only.** The `UserId` on a job application is read from the JWT's claims on the server, never trusted from the request body. Prevents one user from creating or editing applications under another user's account.
- **Passwords** are hashed with ASP.NET Core's `PasswordHasher`, never stored in plain text.
- **Secrets** (database connection string, JWT signing key) are kept out of source control via .NET User Secrets in development.

## Planned improvements
- Automated tests (unit + integration)
- Deployment (Docker + hosting)
- Machine learning and AI for CV review

## Author

Karim
