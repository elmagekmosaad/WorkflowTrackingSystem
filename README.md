# WorkflowTrackingSystem Documentation

## Overview
WorkflowTrackingSystem is a robust .NET 8 solution designed for process and workflow management. The system leverages modern software engineering principles and several advanced features to ensure scalability, security, and maintainability.

---

## API Documentation & Testing
- **Postman Collection:** Full API documentation and test collection available at: [Postman Documentation](https://documenter.getpostman.com/view/13698047/2sB3HqGdBE)

---

## Architecture: Clean Architecture
- **Separation of Concerns:** The solution is structured into distinct layers: Domain, Application, Infrastructure, API, and Shared.
- **Dependency Inversion:** Core business logic is isolated from infrastructure and presentation concerns, making the system testable and extensible.
- **Project Structure:**
  - `Domain`: Entities, value objects, and domain services.
  - `Application`: Use cases, DTOs, and service interfaces.
  - `Infrastructure`: Data access, repositories, and external integrations.
  - `API`: Controllers and endpoints.
  - `Shared`: Common utilities and base types.

---

## Security: Secret Management
- **App Secrets:** Sensitive configuration values (e.g., connection strings, API keys) are stored securely using .NET Secret Manager or environment variables.
- **Best Practices:** Secrets are never hardcoded; access is abstracted via configuration providers.

---

## Endpoint Security & Routing
- **Authentication & Authorization:** Endpoints are protected using ASP.NET Core Identity and JWT Bearer tokens, following Clean Architecture principles.
- **Route Definition:** All API routes are clearly defined and versioned for maintainability.
- **Consistent Responses:** Endpoints return standardized responses using `BaseResponse` for reliability and clarity.

---

## Fast Search: Indexing
- **Indexing:** The system supports fast search operations by leveraging database indexes on key fields (e.g., process name, workflow ID).
- **Performance:** Optimized queries and repository patterns ensure efficient data retrieval.

---

## Pagination: PaginatedList
- **PaginatedList Utility:** Provides efficient pagination for large datasets, reducing memory usage and improving API responsiveness.
- **Usage:** API endpoints return paginated results with metadata (total count, page size, etc.).

---

## Logging: Serilog & Logs/log-.txt
- **Serilog Integration:** Advanced logging is implemented using Serilog, supporting structured logs, rolling files, and various sinks.
- **Log Files:** All significant operations and errors are logged to `Logs/log-.txt` for traceability and debugging.
- **Log Format:** Includes timestamps, log levels, and contextual information.

---

## API Responses: BaseResponse
- **BaseResponse:** Standardizes API responses with consistent structure (status, message, data, errors).
- **Error Handling:** All endpoints return meaningful error messages and status codes.

---

## Additional Features
- **Validation:** Integrated validation service for process execution and data integrity.
- **AutoMapper:** Used for mapping between domain entities and DTOs (see `ProcessProfile.cs`).
- **Enhanced Endpoints:**
  - Filtering by workflow ID, status, and assigned user.
  - Improved error handling and logging in controllers.
- **Extensibility:** The architecture allows easy addition of new features and integrations.

---

## Getting Started
1. **Configuration:** Set up secrets and environment variables as per the documentation.
2. **Database:** Ensure indexes are created for fast search fields.
3. **Run the API:** Build and run the solution using .NET 8 SDK.
4. **Check Logs:** Monitor `Logs/log-.txt` for system activity and errors.

---

## Project Structure Diagram

<img width="325" height="230" alt="00" src="https://github.com/user-attachments/assets/7a902758-39ee-431b-ba17-05c2100ceb29" />
<br>
<img width="462" height="828" alt="01" src="https://github.com/user-attachments/assets/fe9d9b1d-c2a0-4c91-8184-90916f1c5da6" />
<br>
<img width="462" height="864" alt="02" src="https://github.com/user-attachments/assets/6a18fbdc-a774-4a9d-810f-39725e155318" />

```
WorkflowTrackingSystem/
├── WorkflowTrackingSystem.Api/
│   ├── Controllers/
│   ├── Mapping/
│   ├── Requests/
│   └── Logs/
├── WorkflowTrackingSystem.Application/
│   ├── DTOs/
│   └── Services/
├── WorkflowTrackingSystem.Domain/
│   ├── Entities/
│   ├── Enums/
│   └── Repositories/
├── WorkflowTrackingSystem.Infrastructure/
│   ├── Contexts/
│   ├── Logging/
│   ├── Migrations/
│   └── Repositories/
└── WorkflowTrackingSystem.Shared/
    ├── PaginatedList.cs
    ├── BaseResponse.cs
    └── BaseService.cs
```

This structure follows Clean Architecture principles, ensuring separation of concerns and maintainability.

---

## References
- [Microsoft Docs: Clean Architecture](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)
- [ASP.NET Core Secret Manager](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets)
- [Serilog Documentation](https://serilog.net/)

---

For further details, refer to the source code and inline comments in each project layer.


## Developer Information

- **Name:** Mosaad Ghanem
- **Role:** Full-Stack .NET Developer (.NET Core & Angular)
- **Email:** mosaadghanem97@gmail.com
- **LinkedIn:** [linkedin.com/in/elmagekmosaad](https://linkedin.com/in/elmagekmosaad)
- **GitHub:** [github.com/elmagekmosaad](https://github.com/elmagekmosaad)
