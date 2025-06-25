# QA Project Level6

This application is a full-stack team collaboration tool built with .NET 8 (backend) and React.js (frontend).

## ⛩️ Architecture
```
.github
    /workflow 				// GitHub Action Workflows
backend/
    /Controllers/			// API endpoints that handle HTTP requests and responses
    /Data/				// Database context and migrations
    /Exceptions/			// Custom exception handling and error classes
    /Extensions/			// Extension methods to enhance existing classes or functionality
    /Interfaces/			// Interface definitions for dependency injection and abstraction
    /Models/
        /Configuration/			// Configuration models, e.g., appsettings binding classes
        /Entities/			// Domain models representing database tables
        /Requests/			// Models representing incoming API request payloads
        /Responses/			// Models representing outgoing API response payloads
    /Properties/			// Assembly or project metadata (e.g., launchSettings.json)
    /Services/				// Business logic and service classes handling core operations
frontend/
    /src/
        /components/			// Reusable UI components
        /pages/				// Page-level components mapped to routes (views)
        /routes/			// Route configuration and navigation logic
        /services/			// API service calls and data fetching logic
        /utils/				// Utility/helper functions and common logic
tests/					// Unit tests
```

## 🚀 Quick Start

See individual setup guides
- [Backend README](./backend/README.md)
- [Frontend README](./frontend/README.md)
