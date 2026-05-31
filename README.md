# ArticleHub Platform

A comprehensive microservices-based platform for managing academic articles, journals, submissions, and peer review processes. Built with .NET 9.0 and modern architectural patterns.

[![.NET](https://img.shields.io/badge/.NET-9.0-purple.svg)](https://dotnet.microsoft.com/download)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![Docker](https://img.shields.io/badge/docker-compose-blue.svg)](https://docs.docker.com/compose/)

## 📋 Table of Contents

- [Overview](#overview)
- [Business Goals](#business-goals)
- [Architecture](#architecture)
- [Technologies](#technologies)
- [Services](#services)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [API Documentation](#api-documentation)
- [Development](#development)
- [Contributing](#contributing)

## 🎯 Overview

ArticleHub is a distributed microservices platform designed to streamline the entire lifecycle of academic article management—from submission and review to publication and journal management. The platform implements Domain-Driven Design (DDD) principles, CQRS patterns, and event-driven architecture to ensure scalability, maintainability, and resilience.

### Key Features

- **Multi-tenant Journal Management**: Support for multiple academic journals with customizable workflows
- **Article Submission Pipeline**: Comprehensive submission handling with file storage and metadata management
- **Peer Review System**: Structured review workflow with reviewer assignment and evaluation tracking
- **User Authentication & Authorization**: JWT-based security with role-based access control
- **Real-time Updates**: Event-driven communication between services using RabbitMQ
- **GraphQL API**: Hasura-powered GraphQL interface for flexible data querying
- **File Management**: GridFS-based document storage for manuscripts and supplementary materials
- **Email Notifications**: SMTP-based email service for workflow notifications

## 🎯 Business Goals

1. **Streamline Academic Publishing**: Reduce the time from submission to publication through automated workflows
2. **Improve Review Quality**: Enable efficient peer review processes with transparent tracking
3. **Enhance Collaboration**: Facilitate communication between authors, reviewers, and editors
4. **Data Integrity**: Ensure reliable storage and versioning of academic content
5. **Scalability**: Support growing numbers of journals, submissions, and users
6. **Compliance**: Meet academic publishing standards and data protection requirements

## 🏗️ Architecture

### Microservices Architecture

The platform follows a microservices architecture pattern with the following core principles:

- **Service Autonomy**: Each service owns its data and business logic
- **API Gateway Pattern**: Centralized entry point for client applications
- **Event-Driven Communication**: Asynchronous messaging via RabbitMQ
- **Database per Service**: Polyglot persistence with SQL Server, PostgreSQL, Redis, and MongoDB
- **Container Orchestration**: Docker Compose for local development, Kubernetes-ready

### Architectural Patterns

- **Domain-Driven Design (DDD)**: Rich domain models with clear bounded contexts
- **CQRS**: Separation of command and query responsibilities
- **Event Sourcing**: Event-driven state management for critical workflows
- **Repository Pattern**: Data access abstraction
- **Mediator Pattern**: MediatR for handling commands and queries
- **Vertical Slice Architecture**: Feature-based organization with FastEndpoints

## 🚀 Technologies

### Backend Technologies

- **.NET 9.0**: Latest LTS version with C# 13
- **ASP.NET Core**: Web API framework
- **Entity Framework Core**: ORM for SQL databases
- **FastEndpoints**: Endpoint-centric API design
- **Carter**: Minimal API routing
- **MediatR**: Mediator pattern implementation
- **FluentValidation**: Input validation
- **Mapster**: Object mapping
- **gRPC**: Inter-service communication
- **protobuf-net**: Protocol Buffers for .NET

### Databases

- **SQL Server**: Primary database for Auth, Submission, and Review services
- **PostgreSQL**: Database for ArticleHub service with Hasura GraphQL
- **Redis**: Caching and session storage for Journals service
- **MongoDB GridFS**: File storage for article manuscripts and attachments

### Message Broker

- **RabbitMQ**: Message queue for asynchronous communication and event-driven architecture

### Additional Technologies

- **Hasura GraphQL Engine**: Real-time GraphQL API over PostgreSQL
- **JWT Authentication**: Secure token-based authentication
- **Docker & Docker Compose**: Containerization and orchestration
- **Swagger/OpenAPI**: API documentation

### Communication Patterns

- **HTTP/REST**: External client communication
- **gRPC**: High-performance inter-service communication
- **GraphQL**: Flexible query interface via Hasura
- **RabbitMQ**: Asynchronous event messaging

## 🔧 Services

### 1. Auth API (Port 4401)
**Responsibility**: User authentication and authorization

- User registration and login
- JWT token generation and validation
- Role-based access control
- Password management
- User profile management

**Technology Stack**: ASP.NET Core, FastEndpoints, SQL Server, JWT

### 2. ArticleHub API (Port 4403)
**Responsibility**: Core article management and metadata

- Article catalog and metadata management
- Author and person information
- Article versioning
- Integration with Hasura GraphQL
- Real-time article queries

**Technology Stack**: ASP.NET Core, Carter, PostgreSQL, Hasura GraphQL

### 3. Journals API (Port 4402)
**Responsibility**: Journal configuration and management

- Journal creation and configuration
- Editorial board management
- Journal metadata and policies
- Caching for high-performance reads

**Technology Stack**: ASP.NET Core, Redis, REST API

### 4. Submission API (Port 4404)
**Responsibility**: Article submission workflow

- Manuscript submission handling
- File upload and storage (GridFS)
- Submission status tracking
- Author communication
- Supplementary material management

**Technology Stack**: ASP.NET Core, SQL Server, MongoDB GridFS, FastEndpoints

### 5. Review API (Port 4405)
**Responsibility**: Peer review process management

- Reviewer assignment
- Review request management
- Review submission and evaluation
- Review status tracking
- Feedback aggregation

**Technology Stack**: ASP.NET Core, SQL Server, FastEndpoints, MediatR

### Supporting Services

- **Hasura GraphQL (Port 4493)**: GraphQL API layer for ArticleHub
- **RabbitMQ Management (Default)**: Message broker with management UI
- **Redis Insight (Port 8801)**: Redis monitoring and management

## 📦 Prerequisites

Before running the application, ensure you have the following installed:

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (with Docker Compose)
- [Git](https://git-scm.com/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/) (recommended)
- At least 8GB RAM for running all services
- 10GB free disk space

### Optional Tools

- [Postman](https://www.postman.com/) or [Insomnia](https://insomnia.rest/) for API testing
- [Azure Data Studio](https://azure.microsoft.com/en-us/products/data-studio/) or [SSMS](https://docs.microsoft.com/en-us/sql/ssms/) for SQL Server
- [pgAdmin](https://www.pgadmin.org/) for PostgreSQL
- [MongoDB Compass](https://www.mongodb.com/products/compass) for MongoDB

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/articlehub.git
cd articlehub
```

### 2. Configure Environment Variables

The project uses Docker Compose override files for environment-specific configuration. Default development settings are pre-configured.

**Important**: For production deployments, update passwords and secrets in `docker-compose.override.yml` or use environment variables.

### 3. Build and Run with Docker Compose

Navigate to the `src` directory and start all services:

```bash
cd src
docker-compose up --build
```

This command will:
- Build all microservices from their Dockerfiles
- Start infrastructure services (SQL Server, PostgreSQL, Redis, RabbitMQ, MongoDB)
- Initialize databases and apply migrations
- Start all API services

**Initial startup may take 5-10 minutes** as Docker downloads images and builds services.

### 4. Verify Services are Running

Check that all containers are healthy:

```bash
docker-compose ps
```

Expected services:
- `auth.api` - Authentication Service
- `articlehub-api` - Article Hub Service
- `journals-api` - Journals Service
- `submission-api` - Submission Service
- `review-api` - Review Service
- `sqlserver` - SQL Server Database
- `postgres` - PostgreSQL Database
- `articlehub-hasura` - Hasura GraphQL Engine
- `rabbitmq` - RabbitMQ Message Broker
- `mongo-gridfs` - MongoDB File Storage
- `journals-redisdb` - Redis Cache

### 5. Access the Services

| Service | URL | Description |
|---------|-----|-------------|
| Auth API | http://localhost:4401 | Authentication endpoints |
| Journals API | http://localhost:4402 | Journal management |
| ArticleHub API | http://localhost:4403 | Article catalog |
| Submission API | http://localhost:4404 | Submission workflow |
| Review API | http://localhost:4405 | Peer review |
| Hasura Console | http://localhost:4493 | GraphQL IDE (Admin Secret: `secret`) |
| RabbitMQ Management | http://localhost:15672 | Message queue UI (guest/guest) |
| Redis Insight | http://localhost:8801 | Redis monitoring |

### 6. Run Locally Without Docker

For development, you can run services individually:

```bash
# Ensure infrastructure is running
cd src
docker-compose up sqlserver postgres redis rabbitmq mongo-gridfs -d

# Run a specific service
cd Services/Auth/Auth.API
dotnet run
```

**Note**: Update connection strings in `appsettings.Development.json` if running services locally.

## 📁 Project Structure

```
articles/
├── src/                                    # Main source code directory
│   ├── Services/                           # Microservices
│   │   ├── ArticleHub/                     # Article catalog service
│   │   │   ├── ArticleHub.API/             # API layer (Carter endpoints)
│   │   │   ├── ArticleHub.Domain/          # Domain entities and logic
│   │   │   └── ArticleHub.Persistence/     # Data access layer
│   │   ├── Auth/                           # Authentication service
│   │   │   ├── Auth.API/                   # API layer (FastEndpoints)
│   │   │   ├── Auth.Application/           # Application logic (MediatR)
│   │   │   ├── Auth.Domain/                # Domain entities
│   │   │   └── Auth.Persistence/           # EF Core DbContext
│   │   ├── Journals/                       # Journal management service
│   │   │   ├── Journals.Api/               # API layer
│   │   │   ├── Journals.Domain/            # Domain entities
│   │   │   └── Journals.Persistence/       # Redis caching
│   │   ├── Submission/                     # Submission workflow service
│   │   │   ├── Submission.API/             # API layer
│   │   │   ├── Submission.Application/     # Application logic
│   │   │   ├── Submission.Domain/          # Domain entities
│   │   │   └── Submission.Persistence/     # EF Core + GridFS
│   │   └── Review/                         # Peer review service
│   │       ├── Review.API/                 # API layer
│   │       ├── Review.Application/         # Application logic (CQRS)
│   │       ├── Review.Domain/              # Domain entities
│   │       └── Review.Persistence/         # EF Core DbContext
│   ├── BuildingBlocks/                     # Shared libraries
│   │   ├── Articles.Abstractions/          # Common interfaces
│   │   ├── Articles.Grpc.Contracts/        # gRPC service contracts
│   │   ├── Articles.IntegrationEvents.Contracts/  # Event contracts
│   │   ├── Blocks.EntityFramework/         # EF Core extensions
│   │   ├── Blocks.Exceptions/              # Exception handling
│   │   ├── Blocks.FastEndpoints/           # FastEndpoints utilities
│   │   ├── Blocks.Messaging/               # RabbitMQ messaging
│   │   └── Blocks.Redis/                   # Redis caching utilities
│   ├── Modules/                            # Reusable modules
│   │   ├── EmailService/                   # Email functionality
│   │   │   ├── EmailService.Contract/      # Email interfaces
│   │   │   └── EmailService.Smtp/          # SMTP implementation
│   │   └── FileStorage/                    # File storage functionality
│   │       ├── FileStorage.Contract/       # Storage interfaces
│   │       └── FileStorage.MongoGridFS/    # GridFS implementation
│   ├── Blocks.Core/                        # Core shared library
│   │   ├── Extensions/                     # Extension methods
│   │   ├── FluentValidation/               # Validation helpers
│   │   ├── Cache/                          # Caching abstractions
│   │   ├── Security/                       # Security utilities
│   │   └── Json/                           # JSON utilities
│   ├── Domain/                             # Shared domain building blocks
│   │   ├── Entities/                       # Base entity classes
│   │   └── ValueObjects/                   # Common value objects
│   ├── Articles.Security/                  # Security configuration
│   │   ├── JwtOptions.cs                   # JWT settings
│   │   └── ConfigureAuthentication.cs      # Auth setup
│   ├── docker-compose.yml                  # Docker services definition
│   ├── docker-compose.override.yml         # Development overrides
│   └── Articles.slnx                       # Solution file
├── BuildingBlocks/                         # Additional building blocks
│   └── Blocks.AspNetCore/                  # ASP.NET Core extensions
│       ├── Extensions/                     # Startup extensions
│       ├── Filters/                        # MVC filters
│       ├── Grpc/                           # gRPC client/server helpers
│       ├── Middlewares/                    # Custom middleware
│       └── Providers/                      # Context providers
└── Modules/                                # External modules
    └── EmailService/
        └── EmailService.Smtp/              # SMTP email implementation
```

### Layer Responsibilities

#### API Layer
- RESTful endpoints (FastEndpoints/Carter)
- Request/Response DTOs
- Input validation
- Authentication/Authorization
- Swagger documentation

#### Application Layer (where applicable)
- Use cases and business workflows
- Command/Query handlers (MediatR)
- DTOs and mapping
- Application services
- Integration events

#### Domain Layer
- Domain entities and aggregates
- Domain events
- Business logic and invariants
- Domain services
- Value objects

#### Persistence Layer
- DbContext configuration
- Entity configurations
- Repository implementations
- Database migrations
- Data seeding

## 📚 Building Blocks

### Blocks.AspNetCore
ASP.NET Core extensions and middleware:
- Global exception handling middleware
- gRPC client registration extensions
- User ID assignment filters and interceptors
- HTTP context providers

### Blocks.Core
Core utilities and extensions:
- Configuration extensions
- Mapster object mapping configuration
- FluentValidation setup
- Caching abstractions
- Guard clauses
- JSON serialization helpers

### Blocks.EntityFramework
Entity Framework Core extensions:
- Base DbContext with audit support
- Repository pattern implementation
- Unit of Work pattern
- Interceptors for domain events

### Blocks.Messaging
RabbitMQ messaging infrastructure:
- Message bus abstraction
- Event publishing
- Event subscription
- Retry policies

### Articles.Grpc.Contracts
gRPC service definitions for inter-service communication:
- Service contracts
- Message types
- Client stubs

### Articles.IntegrationEvents.Contracts
Event contracts for asynchronous communication:
- Article events
- Submission events
- Review events

## 📡 API Documentation

### Swagger/OpenAPI

Each service exposes Swagger UI for API exploration:

- Auth API: http://localhost:4401/swagger
- ArticleHub API: http://localhost:4403/swagger
- Journals API: http://localhost:4402/swagger
- Submission API: http://localhost:4404/swagger
- Review API: http://localhost:4405/swagger

### GraphQL

The ArticleHub service provides a GraphQL interface via Hasura:

**URL**: http://localhost:4493  
**Admin Secret**: `secret`

Use the Hasura Console to:
- Explore database schema
- Write GraphQL queries and mutations
- Set up permissions
- Configure relationships

Example GraphQL Query:
```graphql
query GetArticles {
  articles {
    id
    title
    abstract
    authors {
      name
      email
    }
    journal {
      name
    }
  }
}
```

### Authentication

Most endpoints require JWT authentication. To obtain a token:

```bash
POST http://localhost:4401/api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "YourPassword123!"
}
```

Use the returned token in subsequent requests:
```
Authorization: Bearer <your-token>
```

## 🛠️ Development

### Database Migrations

#### Entity Framework Core

Create a new migration:
```bash
cd src/Services/Auth/Auth.Persistence
dotnet ef migrations add MigrationName --startup-project ../Auth.API
```

Apply migrations:
```bash
dotnet ef database update --startup-project ../Auth.API
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests for a specific service
cd src/Services/Auth
dotnet test
```

### Debugging

#### Visual Studio 2022
1. Open `Articles.slnx`
2. Set `docker-compose` as the startup project
3. Press F5 to debug

#### VS Code
1. Open the workspace folder
2. Use the Docker extension to start services
3. Attach to running containers for debugging

### Code Quality

The project follows these conventions:
- **C# Coding Standards**: Microsoft's C# coding conventions
- **Clean Code Principles**: SOLID, DRY, KISS
- **Async/Await**: Asynchronous programming throughout
- **Nullable Reference Types**: Enabled for better null safety
- **Implicit Usings**: Reduced boilerplate

### Adding a New Service

1. Create service folder structure (API, Application, Domain, Persistence)
2. Add service to `docker-compose.yml`
3. Configure dependencies in `docker-compose.override.yml`
4. Add database if needed
5. Implement gRPC contracts for inter-service communication
6. Publish integration events for asynchronous workflows
7. Update this README

## 🔒 Security Considerations

- **JWT Authentication**: Token-based authentication with configurable expiration
- **Role-Based Authorization**: Granular permissions per service
- **HTTPS**: TLS encryption for production (configure certificates)
- **Secrets Management**: Use User Secrets for development, Azure Key Vault for production
- **SQL Injection Protection**: Parameterized queries via EF Core
- **CORS**: Configure allowed origins per environment
- **Rate Limiting**: Consider implementing rate limiting middleware
- **Input Validation**: FluentValidation on all inputs

## 🔄 CI/CD

### Recommended Pipeline

1. **Build**: Compile all services
2. **Test**: Run unit and integration tests
3. **Code Quality**: SonarQube/CodeQL analysis
4. **Container Build**: Build Docker images
5. **Security Scan**: Trivy/Snyk container scanning
6. **Deploy**: Push to container registry
7. **Release**: Deploy to Kubernetes/Azure Container Apps

### Environment Variables

Key environment variables to configure:

```env
# Database Connections
ConnectionStrings__Database=<your-connection-string>
ConnectionStrings__Redis=<your-redis-connection>

# JWT Configuration
JwtOptions__Secret=<your-secret-key>
JwtOptions__Issuer=<your-issuer>
JwtOptions__Audience=<your-audience>

# RabbitMQ
RabbitMQ__Host=<your-rabbitmq-host>
RabbitMQ__Username=<username>
RabbitMQ__Password=<password>

# Email (SMTP)
EmailOptions__Host=<smtp-host>
EmailOptions__Port=587
EmailOptions__Username=<email-username>
EmailOptions__Password=<email-password>
```

## 📈 Monitoring and Observability

### Recommended Tools

- **Application Insights**: For Azure deployments
- **Prometheus + Grafana**: Metrics collection and visualization
- **ELK Stack**: Centralized logging (Elasticsearch, Logstash, Kibana)
- **Jaeger**: Distributed tracing
- **Health Checks**: Built-in ASP.NET Core health checks

### Health Check Endpoints

Each service exposes health check endpoints:
- `/health` - Overall service health
- `/health/ready` - Readiness probe
- `/health/live` - Liveness probe

## 🤝 Contributing

Contributions are welcome! Please follow these guidelines:

1. **Fork the repository**
2. **Create a feature branch**: `git checkout -b feature/your-feature`
3. **Follow coding standards**: Maintain consistency with existing code
4. **Write tests**: Ensure adequate test coverage
5. **Commit your changes**: `git commit -m 'Add some feature'`
6. **Push to the branch**: `git push origin feature/your-feature`
7. **Open a Pull Request**: Describe your changes clearly

### Code Review Process

All submissions require review. We use GitHub pull requests for this purpose. Consult [GitHub Help](https://help.github.com/articles/about-pull-requests/) for more information.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📞 Support

For questions, issues, or feature requests:

- **GitHub Issues**: [Create an issue](https://github.com/yourusername/articlehub/issues)
- **Documentation**: [Wiki](https://github.com/yourusername/articlehub/wiki)
- **Email**: support@articlehub.com

## 🙏 Acknowledgments

Built with:
- [.NET](https://dotnet.microsoft.com/) by Microsoft
- [FastEndpoints](https://fast-endpoints.com/) by Đĵ ΝιΓα
- [Carter](https://github.com/CarterCommunity/Carter) by Carter Community
- [MediatR](https://github.com/jbogard/MediatR) by Jimmy Bogard
- [Hasura](https://hasura.io/) GraphQL Engine
- [RabbitMQ](https://www.rabbitmq.com/) by Pivotal
- And many other amazing open-source projects

## 🗺️ Roadmap

### Planned Features

- [ ] WebSocket support for real-time notifications
- [ ] Full-text search with Elasticsearch
- [ ] API Gateway with YARP or Ocelot
- [ ] Kubernetes deployment manifests
- [ ] Advanced analytics and reporting dashboard
- [ ] Mobile API support with BFF pattern
- [ ] Multi-language support (i18n)
- [ ] Automated testing suite expansion
- [ ] Performance monitoring and alerting
- [ ] Content versioning and rollback

---

**Made with ❤️ by the ArticleHub Team**
