# MultiTenant-App
MultiTenant Licensing System
🧩 Multi-Tenant License Management System
📘 Overview

The Multi-Tenant License Management System is a microservices-based .NET 8 application designed for government agencies to manage professional licenses across multiple tenants (e.g., different agencies or departments).
It includes services for License management, Payments, and Notifications, and a web UI (MVC) for user interaction.

This solution demonstrates:

🏗️ Clean architecture (Domain, Application, Persistence, UI)
⚙️ CQRS (Command and Query Responsibility Segregation)
🔐 Multi-tenancy support
📡 Microservices architecture
🧾 SQL Server persistence
🧱 Repository pattern (without Entity Framework)
☸️ Containerized deployment (Docker + AKS ready)
🏗️ Solution Structure

## 🗂️ Folder Structure

```plaintext
License
│
├── Application
│   ├── Command
│   │   ├── IssueLicense.cs
│   │   ├── IssueLicenseCommand.cs
│   │   ├── RenewLicense.cs
│   │   └── RenewLicenseCommand.cs
│   │
│   └── Queries
│       ├── GetLicenseById.cs
│       └── GetLicenses.cs
│
├── Controllers
│   └── LicenseController.cs
│
├── Domain
│   ├── Dto
│   │   └── LicenseDto.cs
│   │
│   └── Entities
│       ├── AuditEntity.cs
│       ├── BaseEntity.cs
│       └── Licenses.cs
│
├── Persistence
│   ├── Config
│   │   └── SqlConnectionConfig.cs
│   │
│   └── Repositories
│       ├── BaseRepository.cs
│       ├── ILicenseRepository.cs
│       └── LicenseRepository.cs
│
├── appsettings.json
├── DockerFile
└── Program.cs

LicenseUI
│
├── Controllers
│   └── LicenseController.cs
│
├── Models
│   ├── ErrorViewModel.cs
│   └── LicenseViewModel.cs
│
├── Views
│   ├── Home
│   │   └── Index.cshtml
│   ├── License
│   │   └── Index.cshtml
│   └── Shared
│       └── _Layout.cshtml
│
├── appsettings.json
└── Program.cs


Notification
│
├── Controllers
│   └── NotificationController.cs
│
├── Models
│   └── NotificationMessage.cs
│
├── Services
│   ├── INotificationService.cs
│   └── NotificationService.cs
│
├── appsettings.json
├── DockerFile
└── Program.cs

Payment
│
├── Config
│   ├── AzureAdSettings.cs
│   └── ExternalServiceConfig.cs
│
├── Controllers
│   └── PaymentController.cs
│
├── Gateway
│   ├── IPaymentGateway.cs
│   └── PaymentGateway.cs
│
├── Handler
│   └── PaymentMessageHandler.cs
│
├── Models
│   ├── External
│   │   └── PaymentResult.cs
│   └── Internal
│       └── PaymentRequest.cs
│
├── Services
│   ├── IPaymentService.cs
│   └── PaymentService.cs
│
├── appsettings.json
├── DockerFile
└── Program.cs

SharedTenant
└── TenantStore.cs
```
---
🧰 Technology Stack
Layer	Technology
Framework	.NET 8
Frontend	ASP.NET Core MVC
Database	SQL Server (Dapper ORM)
Architecture	CQRS + Microservices
Auth	JWT / Cookie Authentication
Containerization	Docker, Docker Compose
Deployment	AKS (Azure Kubernetes Service) Ready

⚙️ Core Components
🧾 License Service

Implements the CQRS pattern to handle license lifecycle operations.

Commands
IssueLicenseCommand – creates a new license
RenewLicenseCommand – renews existing licenses

Queries
GetLicenseById – fetches details by ID
GetLicenses – retrieves all licenses for a tenant

Persistence
Uses Dapper for data access (no Entity Framework)
SqlConnectionConfig manages DB connections
LicenseRepository implements CRUD

Multi-Tenancy
Each license record includes a TenantId
Repositories ensure tenant-specific filtering

🧩 LicenseUI

ASP.NET Core MVC application providing a web interface.

Controllers
LicenseController – interacts with the License API

Views
License dashboard for applicants/agencies

Models
Strongly typed view models for UI rendering

Auth
Cookie-based authentication for simplicity

💳 Payment Service
Handles payment operations and integrates with external gateways.

Config
AzureAdSettings for secure API calls
ExternalServiceConfig for third-party endpoints

Gateway
PaymentGateway encapsulates communication with payment providers

Handler
PaymentMessageHandler processes async payment notifications

Services
PaymentService manages business logic for transactions

✉️ Notification Service
Responsible for sending out notifications.

NotificationController – RESTful endpoint to trigger notifications
NotificationService – implements message delivery (email/SMS/push)
INotificationService – interface for easy substitution (e.g., Twilio, SendGrid)

🧱 SharedTenant
Provides shared logic for tenant management.
TenantStore.cs
Maintains tenant context and isolation
Used by all services for per-tenant data handling

🚀 Running Locally
1. Clone the repository
```
git clone https://github.com/<your-org>/multi-tenant-license-system.git
cd multi-tenant-license-system
```

2. Build all microservices
```
docker-compose build
```

4. Start the stack
```
docker-compose up
```

6. Access the system
```
Service	URL
License UI	http://localhost:8080
License API	http://localhost:5001/api/licenses
Payment API	http://localhost:5002/api/payments
Notification API	http://localhost:5003/api/notifications
SQL Server	localhost,1433 (User: sa, Pass: <password>)
```

📦 Environment Configuration

Each microservice uses an appsettings.json file to define:
Connection strings
Service endpoints
Tenant configuration
API keys for external services

Example:
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=sqlserver;Database=LicenseDB;User Id=sa;Password=P@ssw0rd123!;"
  },
  "ApiGateway": {
    "BaseUrl": "http://apigateway"
  }
}
```

☸️ Deployment (AKS Ready)

Push images to Azure Container Registry (ACR)
```
docker tag license-service <acr>.azurecr.io/license-service:v1
docker push <acr>.azurecr.io/license-service:v1
```

Apply Kubernetes manifests in /k8s folder
```
kubectl apply -f k8s/
```

🧪 Testing

Unit Tests: Each service can be extended with xUnit test projects
Integration Tests: Use HttpClientFactory for end-to-end validation
Postman Collections: Useful for testing REST endpoints manually

🤝 Contributing

Fork the repository
Create a feature branch
```
git checkout -b feature/my-new-feature
```

Commit your changes

```
git commit -am "Add new feature"
```

Push to your fork
```
git push origin feature/my-new-feature
```

Create a Pull Request
