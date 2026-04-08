# PF_Project_4P1W
BSIT - 32E1

Members and Roles:

Iterarion 1: Garcia, Erich Mae A.

Iteration 2: Obeal, Rydner S.

Iteration 3: Vilda, Gerry C.

Iteration 4: Villadiego, Hans Angelo D.

Iteration 5: Obeal, Rydner S.

Iteration 6: Garcia, Erich Mae A.

## Project Overview

A puzzle game web application with separate APIs for authentication and resources.

## Architecture

- **web-app**: React frontend (Vite)
- **auth-api**: .NET API for user authentication
- **resource-api**: .NET API for game resources (packs, puzzles, scores)

## Development Setup

### Prerequisites

- Node.js 18+
- .NET 9.0
- SQL Server (for resource-api)

### Running Locally

1. **Start auth-api**:
   ```bash
   cd auth-api
   dotnet run
   ```
   Runs on http://localhost:5068

2. **Start resource-api**:
   ```bash
   cd resource-api
   dotnet run
   ```
   Runs on http://localhost:5208

3. **Start web-app**:
   ```bash
   cd web-app
   npm install
   npm run dev
   ```
   Runs on http://localhost:5173

### Test Users

- Admin: admin@gmail.com / admin123
- Player: player@gmail.com / player123

## Deployment

### Web App (Frontend)

Deploy to Vercel, Netlify, or GitHub Pages:

1. Build the app:
   ```bash
   cd web-app
   npm run build
   ```

2. Deploy the `dist` folder to your hosting platform.

Update API URLs in `web-app/src/services/api.js` for production.

### APIs

Deploy to Azure App Service, AWS, or similar:

1. For auth-api: Deploy as .NET web app
2. For resource-api: Deploy as .NET web app with SQL database

Update connection strings and CORS origins in appsettings.json.

## CI/CD

GitHub Actions workflow builds all components on push/PR to main branch.

