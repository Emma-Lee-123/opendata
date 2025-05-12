![C#](https://img.shields.io/badge/Language-C%23-239120?logo=c-sharp&logoColor=white&style=flat)
![Azure Functions](https://img.shields.io/badge/Backend-Azure_Functions-brightgreen)
![Azure SQL](https://img.shields.io/badge/Database-Azure_SQL-blue)

### Trip Finder – Backend (Azure Functions)

This repository contains the backend service for the Trip Finder application, built with C# and Azure Functions (Isolated .NET). The backend exposes secure HTTP-triggered serverless APIs to handle transit route planning logic and respond to frontend requests with relevant schedule and routing data.

All APIs are deployed to Azure Function App, with automated CI/CD configured using GitHub Actions for streamlined deployments on every push to the main branch.

### Tech Stack

- **Runtime:** Azure Functionsp (.NET Isolated Process)  
- **Language:** C# (.NET 8)  
- **Database:** Azure SQL  
- **Deployment:** GitHub Actions to Azure Functin App 

### Features
- **Stop Suggestions API:** Returns autocomplete data for transit stop names  
- **Trip Lookup API:** Processes trip search queries using GTFS data  
- **Secure Endpoints:** APIs are protected using Azure Function authentication and scoped access via API keys or tokens 
 
### Configuration
Environment variables include:
- **SqlConnectionString:** connection string to Azure SQL   
- **CORS-enabled:** Configured for integration with a separate frontend (e.g., Azure Static Web Apps)
  
### Deployment
This backend is deployed using GitHub Actions for automatic deployment on code changes and integrated with:
- **Azure Function App (Isolated .NET)**  
- **Azure SQL Database**  
- **Azure Static Web App frontend**  
